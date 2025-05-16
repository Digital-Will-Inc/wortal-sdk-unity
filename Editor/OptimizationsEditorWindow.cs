using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using DigitalWill.WortalSDK;
using System;
using System.Threading;

namespace DigitalWill.WortalEditor
{

    public class OptimizationsEditorWindow : EditorWindow
    {
        private enum OptimizationTab { General, LazyLoad }
        private OptimizationTab currentTab = OptimizationTab.General;


        // Constants for file extensions and paths to avoid magic strings
        private const string DefaultConfigPath = "Assets/Resources/LazyLoadConfig.asset";
        private const string ConfigPathKey = "WortalSDK_LazyLoadConfigPath";
        private const string CSharpExtension = ".cs";
        private const string SceneExtension = ".unity";
        private const string AssetsFolder = "Assets/";
        private const int MaxVisibleAssetsPerType = 30; // Limit for performance

        // UI state tracking
        private LazyLoadConfig config;
        private string configPath;
        private Vector2 scrollPos;
        private Dictionary<string, bool> typeFoldouts = new Dictionary<string, bool>();
        private string newGroupName = "";
        private string groupFilter = "";
        private string assetSearchTerm = "";
        private int? highlightedGroupIndex = null;
        private float highlightTimer = 0f;
        private bool isProcessingHeavyTask = false;
        private bool isFirstOpen = true;

        // UI colors for highlighting
        private readonly Color highlightColor = new Color(0.6f, 0.9f, 0.6f, 0.5f);

        [MenuItem("Wortal/Optimizations/Configurator")]
        public static void ShowWindow()
        {
            GetWindow<OptimizationsEditorWindow>("Optimizations Configurator");
        }

        private void OnEnable()
        {
            configPath = EditorPrefs.GetString(ConfigPathKey, DefaultConfigPath);
            LoadOrCreateConfig();

            // Auto-group on first open if no groups exist
            if (isFirstOpen && (config.assetPriorityGroups == null || config.assetPriorityGroups.Count == 0))
            {
                isFirstOpen = false;
                AutoGroupAssetsBySceneAndType();
            }

            // Set up editor update callback for animations
            EditorApplication.update += OnEditorUpdate;
        }

        private void OnDisable()
        {
            // Clean up editor update callback
            EditorApplication.update -= OnEditorUpdate;
        }

        // Update method for animations
        private void OnEditorUpdate()
        {
            if (highlightedGroupIndex.HasValue)
            {
                highlightTimer += Time.deltaTime;
                if (highlightTimer > 2f) // Reset highlight after 2 seconds
                {
                    highlightedGroupIndex = null;
                    highlightTimer = 0f;
                    Repaint();
                }
            }
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

            GUILayout.Space(10);
            EditorGUILayout.LabelField("Wortal SDK WebGL Optimization Config", EditorStyles.boldLabel);
            GUILayout.Space(5);

            // Draw tab buttons
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Toggle(currentTab == OptimizationTab.General, "General Optimizations", EditorStyles.toolbarButton))
                currentTab = OptimizationTab.General;

            if (config.enableLazyLoad && GUILayout.Toggle(currentTab == OptimizationTab.LazyLoad, "Lazy Load Settings", EditorStyles.toolbarButton))
                currentTab = OptimizationTab.LazyLoad;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(10);

            // Render selected tab
            switch (currentTab)
            {
                case OptimizationTab.General:
                    DrawGeneralSettingsTab();
                    break;
                case OptimizationTab.LazyLoad:
                    DrawLazyLoadSettingsTab();
                    break;
            }
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
                "3. Enable 'Strip Engine Code'",
                MessageType.Info);


            EditorGUILayout.Space(5);
            EditorGUILayout.HelpBox("These settings affect build size, compatibility, and performance for WebGL. Be sure to test your builds after applying changes.", MessageType.None);
        }


        private void DrawLazyLoadSettingsTab()
        {
            // Display processing indicator if heavy tasks are running
            if (isProcessingHeavyTask)
            {
                EditorGUILayout.HelpBox("Processing assets... This may take a while for large projects.", MessageType.Info);
                return;
            }

            // General settings section
            EditorGUILayout.LabelField("Asset Loading Strategy", EditorStyles.boldLabel);

            // IMPROVED: Add Undo support
            EditorGUI.BeginChangeCheck();
            int newPreloadBudget = EditorGUILayout.IntSlider(
                new GUIContent("Preload Budget (MB)", "Maximum amount of assets to preload in MB"),
                config.preloadBudgetMB, 5, 100);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(config, "Changed Preload Budget");
                config.preloadBudgetMB = newPreloadBudget;
                EditorUtility.SetDirty(config);
            }

            EditorGUI.BeginChangeCheck();
            bool newPrewarmValue = EditorGUILayout.Toggle(
                new GUIContent("Enable Asset Prewarming", "Prewarm assets on startup to reduce first-load lag"),
                config.enableAssetPrewarm);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(config, "Changed Asset Prewarming");
                config.enableAssetPrewarm = newPrewarmValue;
                EditorUtility.SetDirty(config);
            }

            EditorGUILayout.Space(10);

            // Search and filter controls
            EditorGUILayout.BeginHorizontal();
            groupFilter = EditorGUILayout.TextField(
                new GUIContent("Filter Groups", "Filter groups by name"),
                groupFilter);

            assetSearchTerm = EditorGUILayout.TextField(
                new GUIContent("Search Assets", "Search for specific assets within groups"),
                assetSearchTerm);
            EditorGUILayout.EndHorizontal();

            DrawPriorityGroupsSection();
            EditorGUILayout.Space(10);

            // Config path controls
            EditorGUILayout.LabelField("Config File Path", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField(configPath, EditorStyles.label);
            EditorGUI.EndDisabledGroup();
            // if (GUILayout.Button("Change Path", GUILayout.Width(100)))
            // {
            //     string newPath = EditorUtility.SaveFilePanelInProject("Save Lazy Load Config", "LazyLoadConfig", "asset", "Choose config save location");
            //     if (!string.IsNullOrEmpty(newPath))
            //     {
            //         configPath = newPath;
            //         EditorPrefs.SetString(ConfigPathKey, configPath);
            //         LoadOrCreateConfig();
            //     }
            // }
            EditorGUILayout.EndHorizontal();

            // Import/Export/Save section
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Save Config"))
            {
                SaveConfig();
            }

            if (GUILayout.Button("Export Config"))
            {
                ExportConfig();
            }

            if (GUILayout.Button("Import Config"))
            {
                ImportConfig();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void SaveConfig()
        {
            try
            {
                foreach (var group in config.assetPriorityGroups)
                {
                    group.SyncToSerializableList();
                }
                EditorUtility.SetDirty(config);
                AssetDatabase.SaveAssets();
                Debug.Log("Lazy Load Config Saved");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save config: {e.Message}");
            }
        }

        private void ExportConfig()
        {
            string path = EditorUtility.SaveFilePanel("Export Lazy Load Config", "", "LazyLoadConfig", "json");
            if (string.IsNullOrEmpty(path))
                return;

            try
            {
                string json = JsonUtility.ToJson(config, true);
                File.WriteAllText(path, json);
                Debug.Log($"Config exported to {path}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to export config: {e.Message}");
            }
        }

        private void ImportConfig()
        {
            string path = EditorUtility.OpenFilePanel("Import Lazy Load Config", "", "json");
            if (string.IsNullOrEmpty(path))
                return;

            try
            {
                string json = File.ReadAllText(path);
                Undo.RecordObject(config, "Import Config");
                JsonUtility.FromJsonOverwrite(json, config);
                ValidateConfig();
                SaveConfig();
                Debug.Log($"Config imported from {path}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to import config: {e.Message}");
            }
        }

        // IMPROVED: Added manual group creation, filtering, and highlighting
        private void DrawPriorityGroupsSection()
        {
            EditorGUILayout.LabelField(
                new GUIContent("Asset Priority Groups", "Organize assets by priority to optimize loading sequence"),
                EditorStyles.boldLabel);

            EditorGUILayout.HelpBox("Organize assets by scene with nested types for better control.", MessageType.Info);

            // Auto-grouping button
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Refresh Grouping"))
            {
                // Confirm before overwriting
                if (config.assetPriorityGroups != null && config.assetPriorityGroups.Count > 0)
                {
                    if (!EditorUtility.DisplayDialog("Confirm Regrouping",
                        "This will replace all existing groups. Continue?", "Yes", "Cancel"))
                    {
                        return;
                    }
                }

                StartHeavyTask(() => AutoGroupAssetsBySceneAndType());
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            newGroupName = EditorGUILayout.TextField("New Group Name", newGroupName);

            EditorGUI.BeginDisabledGroup(string.IsNullOrWhiteSpace(newGroupName));
            if (GUILayout.Button("Create Group", GUILayout.Width(100)))
            {
                CreateNewGroup();
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Expand All Types"))
            {
                foreach (var key in typeFoldouts.Keys.ToList())
                    typeFoldouts[key] = true;
            }

            if (GUILayout.Button("Collapse All Types"))
            {
                foreach (var key in typeFoldouts.Keys.ToList())
                    typeFoldouts[key] = false;
            }

            if (GUILayout.Button("Sort Groups by Priority"))
            {
                Undo.RecordObject(config, "Sort Groups");
                config.assetPriorityGroups = config.assetPriorityGroups
                    .OrderBy(g => g.priority)
                    .ToList();
                SaveConfig();
            }
            EditorGUILayout.EndHorizontal();

            // Track actions to apply after layout
            int? moveUpIndex = null;
            int? moveDownIndex = null;
            int? removeIndex = null;

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(400));

            // Filter groups based on search term
            var filteredGroups = config.assetPriorityGroups
                .Where(g => string.IsNullOrEmpty(groupFilter) ||
                            g.name.IndexOf(groupFilter, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            for (int i = 0; i < filteredGroups.Count; i++)
            {
                var group = filteredGroups[i];
                if (group == null) continue;

                // Find the actual index in the main list
                int actualIndex = config.assetPriorityGroups.IndexOf(group);

                // Check if this group should be highlighted
                bool isHighlighted = highlightedGroupIndex.HasValue && highlightedGroupIndex.Value == actualIndex;

                // Draw the group with background color if highlighted
                if (isHighlighted)
                {
                    var originalColor = GUI.backgroundColor;
                    GUI.backgroundColor = highlightColor;
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    GUI.backgroundColor = originalColor;
                }
                else
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                }

                EditorGUILayout.BeginHorizontal();

                // Group name with option to edit
                EditorGUI.BeginChangeCheck();
                string newName = EditorGUILayout.TextField(group.name, GUILayout.Width(200));
                if (EditorGUI.EndChangeCheck() && newName != group.name)
                {
                    Undo.RecordObject(config, "Rename Group");
                    group.name = newName;
                    EditorUtility.SetDirty(config);
                    highlightedGroupIndex = actualIndex;
                    highlightTimer = 0f;
                }

                // Priority dropdown
                EditorGUI.BeginChangeCheck();
                var newPriority = (DigitalWill.WortalSDK.LoadPriority)EditorGUILayout.EnumPopup(group.priority, GUILayout.Width(100));
                if (EditorGUI.EndChangeCheck() && newPriority != group.priority)
                {
                    Undo.RecordObject(config, "Change Priority");
                    group.priority = newPriority;
                    EditorUtility.SetDirty(config);
                    highlightedGroupIndex = actualIndex;
                    highlightTimer = 0f;
                }

                EditorGUILayout.LabelField($"{group.assetsCount} assets", GUILayout.Width(100));
                EditorGUILayout.LabelField($"~{group.estimatedSizeMB:F1} MB", GUILayout.Width(100));

                if (GUILayout.Button("▲", GUILayout.Width(25)) && actualIndex > 0)
                {
                    moveUpIndex = actualIndex;
                }

                if (GUILayout.Button("▼", GUILayout.Width(25)) && actualIndex < config.assetPriorityGroups.Count - 1)
                {
                    moveDownIndex = actualIndex;
                }

                if (GUILayout.Button("Edit Assets", GUILayout.Width(100)))
                {
                    AssetSelectorPopup.Show(config, group);
                    highlightedGroupIndex = actualIndex;
                    highlightTimer = 0f;
                }

                // Confirm before removal
                if (GUILayout.Button("Remove", GUILayout.Width(70)))
                {
                    if (EditorUtility.DisplayDialog("Confirm Removal",
                        $"Are you sure you want to remove the group '{group.name}'?", "Remove", "Cancel"))
                    {
                        removeIndex = actualIndex;
                    }
                }

                EditorGUILayout.EndHorizontal();

                // Nested asset foldouts with asset search
                if (group.assetsByType != null)
                {
                    foreach (var kvp in group.assetsByType)
                    {
                        string typeKey = $"{group.name}-{kvp.Key}";
                        if (!typeFoldouts.ContainsKey(typeKey))
                            typeFoldouts[typeKey] = false;

                        // Count assets that match the search term
                        int matchingAssetCount = string.IsNullOrEmpty(assetSearchTerm) ?
                            kvp.Value.Count :
                            kvp.Value.Count(p => p.IndexOf(assetSearchTerm, StringComparison.OrdinalIgnoreCase) >= 0);

                        // Only show types that match search
                        if (matchingAssetCount > 0)
                        {
                            typeFoldouts[typeKey] = EditorGUILayout.Foldout(
                                typeFoldouts[typeKey],
                                $"{kvp.Key} ({matchingAssetCount})",
                                true
                            );

                            if (typeFoldouts[typeKey])
                            {
                                EditorGUI.indentLevel++;

                                int shownAssets = 0;
                                foreach (var path in kvp.Value)
                                {
                                    // Filter assets by search term
                                    if (!string.IsNullOrEmpty(assetSearchTerm) &&
                                        path.IndexOf(assetSearchTerm, StringComparison.OrdinalIgnoreCase) < 0)
                                        continue;

                                    // IMPROVED: More memory-efficient asset display
                                    EditorGUILayout.BeginHorizontal();
                                    EditorGUILayout.LabelField(Path.GetFileName(path));

                                    if (GUILayout.Button("Select", GUILayout.Width(60)))
                                    {
                                        var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                                        if (asset != null)
                                        {
                                            Selection.activeObject = asset;
                                            EditorGUIUtility.PingObject(asset);
                                        }
                                    }

                                    if (GUILayout.Button("Remove", GUILayout.Width(60)))
                                    {
                                        Undo.RecordObject(config, "Remove Asset");
                                        kvp.Value.Remove(path);
                                        group.UpdateCountAndSize();
                                        EditorUtility.SetDirty(config);
                                        GUIUtility.ExitGUI(); // Break out of the collection we're modifying
                                    }

                                    EditorGUILayout.EndHorizontal();

                                    // Limit number of displayed assets for performance
                                    if (++shownAssets >= MaxVisibleAssetsPerType)
                                    {
                                        int remaining = matchingAssetCount - MaxVisibleAssetsPerType;
                                        if (remaining > 0)
                                        {
                                            EditorGUILayout.LabelField($"...and {remaining} more assets (open in 'edit assets' to view all)");
                                        }
                                        break;
                                    }
                                }
                                EditorGUI.indentLevel--;
                            }
                        }
                    }
                }

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndScrollView();

            // Apply deferred actions AFTER layout ends
            try
            {
                if (moveUpIndex.HasValue && moveUpIndex > 0)
                {
                    int i = moveUpIndex.Value;
                    Undo.RecordObject(config, "Move Group Up");
                    (config.assetPriorityGroups[i - 1], config.assetPriorityGroups[i]) =
                        (config.assetPriorityGroups[i], config.assetPriorityGroups[i - 1]);
                    highlightedGroupIndex = i - 1;
                    highlightTimer = 0f;
                    SaveConfig();
                    GUIUtility.ExitGUI(); // Redraw UI
                }

                if (moveDownIndex.HasValue && moveDownIndex < config.assetPriorityGroups.Count - 1)
                {
                    int i = moveDownIndex.Value;
                    Undo.RecordObject(config, "Move Group Down");
                    (config.assetPriorityGroups[i + 1], config.assetPriorityGroups[i]) =
                        (config.assetPriorityGroups[i], config.assetPriorityGroups[i + 1]);
                    highlightedGroupIndex = i + 1;
                    highlightTimer = 0f;
                    SaveConfig();
                    GUIUtility.ExitGUI();
                }

                if (removeIndex.HasValue)
                {
                    Undo.RecordObject(config, "Remove Group");
                    config.assetPriorityGroups.RemoveAt(removeIndex.Value);
                    SaveConfig();
                    GUIUtility.ExitGUI();
                }
            }
            catch (ExitGUIException)
            {
                // Let Unity handle itself, this is expected
                throw;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error applying UI actions: {e.Message}");
            }
        }

        private void CreateNewGroup()
        {
            if (string.IsNullOrWhiteSpace(newGroupName))
                return;

            Undo.RecordObject(config, "Create Asset Group");
            var newGroup = new DigitalWill.WortalSDK.AssetPriorityGroup(newGroupName, DigitalWill.WortalSDK.LoadPriority.Medium);
            newGroup.assetsByType = new Dictionary<string, List<string>>();
            config.assetPriorityGroups.Add(newGroup);

            // Set highlight to the new group
            highlightedGroupIndex = config.assetPriorityGroups.Count - 1;
            highlightTimer = 0f;

            // Clear the input field
            newGroupName = "";
            SaveConfig();
        }

        // IMPROVED: Added progress bar and better error handling
        private void AutoGroupAssetsBySceneAndType()
        {
            try
            {
                EditorUtility.DisplayProgressBar("Auto-Grouping", "Clearing existing groups...", 0f);

                Undo.RecordObject(config, "Auto-Group Assets");
                config.assetPriorityGroups.Clear();

                EditorUtility.DisplayProgressBar("Auto-Grouping", "Finding scenes...", 0.1f);
                var sceneAssetPaths = EditorBuildSettings.scenes
                    .Where(s => s.enabled)
                    .Select(s => s.path)
                    .ToArray();

                var assetSceneMap = new Dictionary<string, List<string>>(); // assetPath -> scenes

                for (int i = 0; i < sceneAssetPaths.Length; i++)
                {
                    var scenePath = sceneAssetPaths[i];
                    float progress = 0.1f + 0.4f * ((float)i / sceneAssetPaths.Length);
                    string sceneName = Path.GetFileNameWithoutExtension(scenePath);

                    EditorUtility.DisplayProgressBar("Auto-Grouping", $"Processing scene: {sceneName}...", progress);

                    try
                    {
                        var dependencies = AssetDatabase.GetDependencies(scenePath, true)
                            .Where(path => path != scenePath && !path.EndsWith(CSharpExtension) && !AssetDatabase.IsValidFolder(path))
                            .ToArray();

                        foreach (var dep in dependencies)
                        {
                            if (!assetSceneMap.ContainsKey(dep))
                                assetSceneMap[dep] = new List<string>();

                            if (!assetSceneMap[dep].Contains(scenePath))
                                assetSceneMap[dep].Add(scenePath);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning($"Error processing scene {sceneName}: {e.Message}");
                    }
                }

                // Scene-specific groups
                EditorUtility.DisplayProgressBar("Auto-Grouping", "Creating scene-specific groups...", 0.5f);
                for (int i = 0; i < sceneAssetPaths.Length; i++)
                {
                    var scenePath = sceneAssetPaths[i];
                    float progress = 0.5f + 0.3f * ((float)i / sceneAssetPaths.Length);

                    string sceneName = Path.GetFileNameWithoutExtension(scenePath);
                    EditorUtility.DisplayProgressBar("Auto-Grouping", $"Creating group for scene: {sceneName}...", progress);

                    DigitalWill.WortalSDK.LoadPriority priority = i switch
                    {
                        <= 1 => DigitalWill.WortalSDK.LoadPriority.High,
                        <= 3 => DigitalWill.WortalSDK.LoadPriority.Medium,
                        _ => DigitalWill.WortalSDK.LoadPriority.Low
                    };

                    var group = new DigitalWill.WortalSDK.AssetPriorityGroup($"Scene: {sceneName}", priority);

                    foreach (var kvp in assetSceneMap)
                    {
                        if (kvp.Value.Contains(scenePath))
                        {
                            string assetPath = kvp.Key;
                            var type = AssetDatabase.GetMainAssetTypeAtPath(assetPath);
                            string typeName = type != null ? type.Name : "Unknown";

                            if (!group.assetsByType.ContainsKey(typeName))
                                group.assetsByType[typeName] = new List<string>();

                            group.assetsByType[typeName].Add(assetPath);
                        }
                    }

                    group.UpdateCountAndSize();
                    group.SyncToSerializableList();

                    if (group.assetsByType.Count > 0)
                        config.assetPriorityGroups.Add(group);
                }

                // Unreferenced group
                EditorUtility.DisplayProgressBar("Auto-Grouping", "Finding unreferenced assets...", 0.8f);
                var allAssets = AssetDatabase.GetAllAssetPaths()
                    .Where(path => path.StartsWith(AssetsFolder) && !AssetDatabase.IsValidFolder(path))
                    .ToArray();

                var unreferenced = new DigitalWill.WortalSDK.AssetPriorityGroup("Unreferenced", DigitalWill.WortalSDK.LoadPriority.Low);

                foreach (var asset in allAssets)
                {
                    if (!assetSceneMap.ContainsKey(asset) && !asset.EndsWith(SceneExtension) && !asset.EndsWith(CSharpExtension))
                    {
                        var type = AssetDatabase.GetMainAssetTypeAtPath(asset);
                        string typeName = type != null ? type.Name : "Unknown";

                        if (!unreferenced.assetsByType.ContainsKey(typeName))
                            unreferenced.assetsByType[typeName] = new List<string>();

                        unreferenced.assetsByType[typeName].Add(asset);
                    }
                }

                unreferenced.UpdateCountAndSize();
                unreferenced.SyncToSerializableList();

                if (unreferenced.assetsByType.Count > 0)
                    config.assetPriorityGroups.Add(unreferenced);

                EditorUtility.SetDirty(config);
                AssetDatabase.SaveAssets();
                EditorUtility.ClearProgressBar();
                Debug.Log("Auto-grouped assets by scene and type.");
            }
            catch (Exception e)
            {
                EditorUtility.ClearProgressBar();
                Debug.LogError($"Error auto-grouping assets: {e.Message}\n{e.StackTrace}");
            }
        }

        // Helper method for background processing
        private void StartHeavyTask(Action action)
        {
            isProcessingHeavyTask = true;
            EditorApplication.delayCall += () =>
            {
                try
                {
                    action?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error in heavy task: {e.Message}");
                }
                finally
                {
                    isProcessingHeavyTask = false;
                    Repaint();
                }
            };
        }

        // Old
        private string GetSceneNameFromPath(string path)
        {
            string[] parts = path.Split('/');
            return parts.Length >= 3 ? parts[2] : "Global";
        }
    }
}