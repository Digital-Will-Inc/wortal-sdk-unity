using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Manages the lazy loading of assets based on priority groups and budgets
    /// </summary>
    public class LazyLoadManager : MonoBehaviour
    {
        public static LazyLoadManager Instance { get; private set; }

        /// <summary>
        /// Configuration for the lazy loading system
        /// </summary>
        public LazyLoadConfig config;

        /// <summary>
        /// Event triggered when all startup assets have been loaded
        /// </summary>
        public event Action OnStartupLoadingComplete;

        /// <summary>
        /// Tracks the loaded assets to prevent duplicate loading
        /// </summary>
        private readonly HashSet<string> _loadedAssets = new HashSet<string>();

        /// <summary>
        /// Tracks load operations to allow cancellation if needed
        /// </summary>
        private readonly Dictionary<string, AsyncOperationHandle> _loadHandles = new Dictionary<string, AsyncOperationHandle>();

        /// <summary>
        /// Current total MB of assets loaded
        /// </summary>
        private float _totalLoadedMB = 0f;

        /// <summary>
        /// Indicates if startup loading has completed
        /// </summary>
        public bool IsStartupLoadingComplete { get; private set; } = false;

        private void Awake()
        {
            Debug.Log("[LazyLoadManager] Awake called");
            if (Instance != null && Instance != this)
            {
                Debug.Log("[LazyLoadManager] Multiple instances detected. Destroying duplicate.");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            Debug.Log("[LazyLoadManager] Instance set");


            if (!gameObject.scene.name.Equals("DontDestroyOnLoad"))
            {
                DontDestroyOnLoad(gameObject);
            }

            LoadConfiguration();

            if (config == null)
            {
                Debug.LogError("[LazyLoadManager] Configuration not set! Please assign a LazyLoadConfig asset.");
                return;
            }

            if (config.enableLazyLoad)
            {
                Debug.Log("[LazyLoadManager] Lazy loading enabled. Starting asset loading...");
                StartCoroutine(LoadStartupAssets());
            }
            else
            {
                Debug.Log("[LazyLoadManager] Lazy loading disabled. No assets will be loaded.");
                return;
            }

        }


        /// <summary>
        /// Loads the configuration from Resources if not set in the inspector
        /// </summary>
        private void LoadConfiguration()
        {
            if (config == null)
            {
                config = Resources.Load<LazyLoadConfig>("LazyLoadConfig");
            }

            if (config == null)
            {
                Debug.LogError("[LazyLoadManager] No config found in Resources! Create a LazyLoadConfig asset in a Resources folder.");
            }
        }

        /// <summary>
        /// Loads all assets marked for startup following priority order and budget constraints
        /// </summary>
        private IEnumerator LoadStartupAssets()
        {
            if (config == null)
                yield break;

            Debug.Log("[LazyLoadManager] Starting load of startup assets...");
            float startTime = Time.realtimeSinceStartup;

            // Sort groups by priority (High first)
            var sortedGroups = new List<AssetPriorityGroup>(config.assetPriorityGroups);
            sortedGroups.Sort((a, b) => a.priority.CompareTo(b.priority));

            foreach (var group in sortedGroups)
            {
                // Skip groups not marked for startup loading
                if (!group.loadOnStartup)
                    continue;

                Debug.Log($"[LazyLoadManager] Loading group: {group.name} (Priority: {group.priority}, Size: {group.estimatedSizeMB:F2} MB)");

                // Load each asset in the group
                foreach (var typeEntry in group.assetsByType)
                {
                    string type = typeEntry.Key;
                    List<string> paths = typeEntry.Value;

                    foreach (var path in paths)
                    {
                        if (string.IsNullOrEmpty(path) || _loadedAssets.Contains(path))
                            continue;

                        // Check if we've exceeded our budget
                        if (config.preloadBudgetMB > 0 && _totalLoadedMB >= config.preloadBudgetMB)
                        {
                            Debug.LogWarning($"[LazyLoadManager] Preload budget of {config.preloadBudgetMB} MB exceeded. " +
                                $"Current: {_totalLoadedMB:F2} MB. Remaining assets will be loaded on demand.");
                            IsStartupLoadingComplete = true;
                            OnStartupLoadingComplete?.Invoke();
                            yield break;
                        }

                        yield return LoadAssetAsync(path, type);
                    }
                }
            }

            Debug.Log($"[LazyLoadManager] Finished preloading startup assets. Total size: {_totalLoadedMB:F2} MB");
            Debug.Log($"[LazyLoadManager] Startup load completed in {Time.realtimeSinceStartup - startTime:F2} seconds");
            IsStartupLoadingComplete = true;
            OnStartupLoadingComplete?.Invoke();
        }

        /// <summary>
        /// Loads a specific asset by path
        /// </summary>
        /// <param name="path">Asset path</param>
        /// <param name="type">Asset type category</param>
        /// <returns>Coroutine waiting for the load to complete</returns>
        private IEnumerator LoadAssetAsync(string path, string type)
        {
            AsyncOperationHandle<UnityEngine.Object> handle;
            try
            {
                handle = Addressables.LoadAssetAsync<UnityEngine.Object>(path);
                _loadHandles[path] = handle;
            }
            catch (Exception e)
            {
                Debug.LogError($"[LazyLoadManager] Error loading asset at path: {path}. Exception: {e.Message}");
                yield break;
            }

            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _loadedAssets.Add(path);
                Debug.Log($"[LazyLoadManager] Loaded {path} (Type: {type})");

                if (config.enableAssetPrewarm)
                    yield return PrewarmAsset(handle.Result);

                float assetSizeMB = EstimateAssetSizeMB(path);
                _totalLoadedMB += assetSizeMB;
            }
            else
            {
                Debug.LogWarning($"[LazyLoadManager] Failed to load asset at path: {path}. Status: {handle.Status}");
            }
        }

        /// <summary>
        /// Loads an asset on demand that wasn't loaded during startup
        /// </summary>
        /// <param name="path">Path to the asset</param>
        /// <typeparam name="T">Type of asset to load</typeparam>
        /// <returns>Task with the loaded asset</returns>
        public async Task<T> LoadAssetOnDemand<T>(string path) where T : UnityEngine.Object
        {
            if (_loadedAssets.Contains(path))
            {
                // Asset already loaded, retrieve it
                AsyncOperationHandle handle = _loadHandles[path];
                return (T)handle.Result;
            }

            try
            {
                var loadOp = Addressables.LoadAssetAsync<T>(path);
                _loadHandles[path] = loadOp;

                await loadOp.Task;

                if (loadOp.Status == AsyncOperationStatus.Succeeded)
                {
                    _loadedAssets.Add(path);

                    // Update our loaded size tracking
                    float assetSizeMB = EstimateAssetSizeMB(path);
                    _totalLoadedMB += assetSizeMB;

                    Debug.Log($"[LazyLoadManager] On-demand loaded {path}");
                    return loadOp.Result;
                }
                else
                {
                    Debug.LogWarning($"[LazyLoadManager] Failed to load asset on demand at path: {path}");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[LazyLoadManager] Error during on-demand loading for {path}: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Releases a loaded asset to free memory
        /// </summary>
        /// <param name="path">Path of the asset to release</param>
        public void ReleaseAsset(string path)
        {
            if (_loadHandles.TryGetValue(path, out var handle))
            {
                try
                {
                    Addressables.Release(handle);
                    _loadHandles.Remove(path);
                    _loadedAssets.Remove(path);
                    Debug.Log($"[LazyLoadManager] Released asset: {path}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"[LazyLoadManager] Error releasing asset {path}: {e.Message}");
                }
            }
        }

        /// <summary>
        /// Prewarm an asset by instantiating it once
        /// </summary>
        /// <param name="asset">The asset to prewarm</param>
        private IEnumerator PrewarmAsset(UnityEngine.Object asset)
        {

            if (asset is GameObject prefab)
            {
                var instance = Instantiate(prefab);
                instance.SetActive(false); // Don't actually show it
                yield return null; // Allow Unity to process the instantiation
                Destroy(instance);
                Debug.Log($"[LazyLoadManager] Prewarmed prefab: {prefab.name}");
            }
            else if (asset is Texture texture)
            {
                // Prewarm texture by accessing its data
                var temp = new Texture2D(texture.width, texture.height);
                Graphics.CopyTexture(texture, temp);
                Destroy(temp);
                Debug.Log($"[LazyLoadManager] Prewarmed texture: {texture.name}");
            }
        }

        /// <summary>
        /// Estimates the size of an asset in megabytes
        /// </summary>
        /// <param name="path">Path to the asset</param>
        /// <returns>Size in MB</returns>
        private float EstimateAssetSizeMB(string path)
        {
            try
            {
                // For file paths that exist on disk
                string fullPath = Path.Combine(Application.dataPath.Replace("Assets", ""), path);
                if (File.Exists(fullPath))
                {
                    var info = new FileInfo(fullPath);
                    return info.Length / (1024f * 1024f);
                }

                // For addressable assets, check if we can find the bundled file
                string bundlePath = Path.Combine(Application.persistentDataPath, "Addressables", Path.GetFileName(path));
                if (File.Exists(bundlePath))
                {
                    var info = new FileInfo(bundlePath);
                    return info.Length / (1024f * 1024f);
                }

                // Fallback: use a default estimation or try to derive from memory footprint
                if (_loadHandles.TryGetValue(path, out var handle) && handle.Result != null)
                {
                    // Attempt to get more accurate size from loaded object - this is a rough approximation
                    if (handle.Result is Texture tex)
                        return EstimateTextureSizeMB(tex);
                    if (handle.Result is Mesh mesh)
                        return EstimateMeshSizeMB(mesh);
                    if (handle.Result is AudioClip clip)
                        return EstimateAudioClipSizeMB(clip);
                }

                // Default estimation when we can't determine actual size
                return 1.0f; // 1MB as default estimate
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[LazyLoadManager] Error estimating size for {path}: {e.Message}");
                return 0.5f;
            }
        }

        /// <summary>
        /// Estimates the memory footprint of a texture
        /// </summary>
        private float EstimateTextureSizeMB(Texture texture)
        {
            if (texture == null) return 0.5f;

            // Simplified estimation based on dimensions and format
            long bytesPerPixel = 4; // Assume RGBA32 format
            long sizeBytes = (long)texture.width * texture.height * bytesPerPixel;

            // Account for mipmaps if applicable
            if (texture is Texture2D tex2D && tex2D.mipmapCount > 1)
                sizeBytes = (long)(sizeBytes * 1.33f); // ~33% overhead for mipmaps

            return sizeBytes / (1024f * 1024f);
        }

        /// <summary>
        /// Estimates the memory footprint of a mesh
        /// </summary>
        private float EstimateMeshSizeMB(Mesh mesh)
        {
            if (mesh == null) return 0.5f;

            // Basic estimation based on vertex count and assumed attributes
            long bytesPerVertex = 32; // Position, normal, UV, tangent, etc.
            long sizeBytes = mesh.vertexCount * bytesPerVertex;

            // Add triangles
            sizeBytes += (long)mesh.triangles.Length * sizeof(int);

            return sizeBytes / (1024f * 1024f);
        }

        /// <summary>
        /// Estimates the memory footprint of an audio clip
        /// </summary>
        private float EstimateAudioClipSizeMB(AudioClip clip)
        {
            if (clip == null) return 0.5f;

            // Estimation based on length, channels and assumed sample rate
            float bytesPerSecond = clip.channels * clip.frequency * 2; // 16-bit samples
            long sizeBytes = (long)(clip.length * bytesPerSecond);

            return sizeBytes / (1024f * 1024f);
        }

        /// <summary>
        /// Releases loaded assets when the game exits
        /// </summary>
        private void OnDestroy()
        {
            foreach (var handle in _loadHandles.Values)
            {
                try
                {
                    Addressables.Release(handle);
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"[LazyLoadManager] Error releasing handle during cleanup: {e.Message}");
                }
            }

            _loadHandles.Clear();
            _loadedAssets.Clear();
        }

    }
}