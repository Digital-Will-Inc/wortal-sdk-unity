using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading;
using DigitalWill.WortalSDK;
using DigitalWill.WortalEditor.Optimizations;

namespace DigitalWill.WortalEditor
{

    public class OptimizationsEditorWindow : EditorWindow
    {
        private enum OptimizationTab { General, LazyLoad, AssetStripPreview }
        private OptimizationTab currentTab = OptimizationTab.General;


        // Constants for file extensions and paths to avoid magic strings
        private const string DefaultConfigPath = "Assets/Resources/LazyLoadConfig.asset";
        private const string ConfigPathKey = "WortalSDK_LazyLoadConfigPath";

        // UI state tracking
        private LazyLoadConfig config;
        private string configPath;
        private bool isFirstOpen = true;

        // UI colors for highlighting
        private readonly Color highlightColor = new Color(0.6f, 0.9f, 0.6f, 0.5f);

        private LazyLoadSystem lazyLoadSystem;
        private UnusedAssetScanner unusedAssetScanner;
        [MenuItem("Wortal/Optimizations/Configurator")]
        public static void ShowWindow()
        {
            GetWindow<OptimizationsEditorWindow>("Optimizations Configurator");
        }

        private void OnEnable()
        {
            configPath = EditorPrefs.GetString(ConfigPathKey, DefaultConfigPath);
            LoadOrCreateConfig();

            lazyLoadSystem = new LazyLoadSystem();
            lazyLoadSystem.Initialize(this, config);

            unusedAssetScanner = new UnusedAssetScanner();
        }

        private void OnDisable()
        {
            // Clean up editor update callback
            lazyLoadSystem?.Cleanup();
        }

        // IMPROVED: Better error handling and validation
        private void LoadOrCreateConfig()
        {
            try
            {
                config = AssetDatabase.LoadAssetAtPath<LazyLoadConfig>(configPath);

                if (config == null)
                {
                    config = ScriptableObject.CreateInstance<LazyLoadConfig>();

                    var dir = Path.GetDirectoryName(configPath);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    {
                        try
                        {
                            Directory.CreateDirectory(dir);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError($"Failed to create directory: {e.Message}");
                            return;
                        }
                    }

                    AssetDatabase.CreateAsset(config, configPath);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                // Validate configuration to ensure no null references
                ValidateConfig();
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load or create config: {e.Message}");
            }
        }

        //Validate config to prevent null reference exceptions
        private void ValidateConfig()
        {
            if (config.assetPriorityGroups == null)
                config.assetPriorityGroups = new List<DigitalWill.WortalSDK.AssetPriorityGroup>();

            for (int i = 0; i < config.assetPriorityGroups.Count; i++)
            {
                var group = config.assetPriorityGroups[i];
                if (group == null)
                {
                    config.assetPriorityGroups.RemoveAt(i--);
                    continue;
                }

                group.RebuildDictionary(); // Rebuild the dictionary from the serialized list

                if (group.assetsByType == null)
                    group.assetsByType = new Dictionary<string, List<string>>();

                // Remove any null entries in asset lists
                foreach (var key in group.assetsByType.Keys.ToList())
                {
                    if (group.assetsByType[key] == null)
                        group.assetsByType[key] = new List<string>();
                    else
                        group.assetsByType[key] = group.assetsByType[key].Where(p => !string.IsNullOrEmpty(p)).ToList();
                }

            }
        }

        private void OnGUI()
        {
            if (config == null)
            {
                EditorGUILayout.HelpBox("Config could not be loaded or created.", MessageType.Error);
                if (GUILayout.Button("Try Again"))
                {
                    LoadOrCreateConfig();
                }
                return;
            }

            DrawHeader();

            // Draw tab buttons
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Toggle(currentTab == OptimizationTab.General, "General Optimizations", EditorStyles.toolbarButton))
                currentTab = OptimizationTab.General;

            if (config.enableLazyLoad && GUILayout.Toggle(currentTab == OptimizationTab.LazyLoad, "Lazy Load Settings", EditorStyles.toolbarButton))
                currentTab = OptimizationTab.LazyLoad;

            if (GUILayout.Toggle(currentTab == OptimizationTab.AssetStripPreview, "Asset Strip Viewer", EditorStyles.toolbarButton))
                currentTab = OptimizationTab.AssetStripPreview;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(10);

            // Render selected tab
            switch (currentTab)
            {
                case OptimizationTab.General:
                    DrawGeneralSettingsTab();
                    break;
                case OptimizationTab.LazyLoad:
                    lazyLoadSystem.OnGUI();
                    break;
                case OptimizationTab.AssetStripPreview:
                    unusedAssetScanner.OnGUI();
                    break;
            }
        }

        private void DrawHeader()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Wortal SDK Optimization Tools", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Tools to help optimize your WebGL build size and performance");
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(5);
        }

        private void DrawGeneralSettingsTab()
        {
            EditorGUILayout.LabelField("General WebGL Optimizations", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            // Section: Lazy Loading
            EditorGUILayout.LabelField("Lazy Load System", EditorStyles.miniBoldLabel);
            EditorGUI.BeginChangeCheck();
            bool newEnableLazy = EditorGUILayout.Toggle(
                new GUIContent("Enable Lazy Loading", "Toggle this ON to enable LazyLoadManager, which loads assets dynamically based on scene, type, and priority."),
                config.enableLazyLoad);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(config, "Toggled Lazy Load Enabled");
                config.enableLazyLoad = newEnableLazy;
                EditorUtility.SetDirty(config);

                // Auto switch to Lazy Load tab
                if (newEnableLazy)
                    currentTab = OptimizationTab.LazyLoad;
            }

            EditorGUILayout.HelpBox("When enabled, Lazy Load will load only essential assets at startup. The rest are loaded on demand to reduce memory usage and improve load times.", MessageType.Info);
            EditorGUILayout.Space(10);

            // Section: WebGL Build Settings
            EditorGUILayout.LabelField("WebGL Build Settings", EditorStyles.miniBoldLabel);

            // Compression Format
            PlayerSettings.WebGL.compressionFormat = (WebGLCompressionFormat)EditorGUILayout.EnumPopup(
                new GUIContent("Compression Format", "Choose how to compress your WebGL build files:\n- Brotli: smallest size (recommended)\n- Gzip: broader browser support\n- Disabled: no compression"),
                PlayerSettings.WebGL.compressionFormat);

            // Memory/Heap Size
            PlayerSettings.WebGL.memorySize = EditorGUILayout.IntSlider(
                new GUIContent("Heap Size (MB)", "Sets the memory allocated to WebGL at startup. Higher values reduce out-of-memory errors.\nRecommended: 256–1024 MB for 3D games."),
                PlayerSettings.WebGL.memorySize, 64, 2048);

            // Strip Engine Code
            EditorGUILayout.HelpBox(
                "To strip unused engine code in WebGL builds:\n" +
                "1. Open File → Build Settings → WebGL\n" +
                "2. Click 'Player Settings' → 'Publishing Settings'\n" +
                "3. Enable 'Strip Engine Code'\n" +
                "Use our 'Asset Strip Viewer' tool to preview what will be stripped.\n",
                MessageType.Info);


            EditorGUILayout.Space(5);
            EditorGUILayout.HelpBox("These settings affect build size, compatibility, and performance for WebGL. Be sure to test your builds after applying changes.", MessageType.None);
        }
    }
}