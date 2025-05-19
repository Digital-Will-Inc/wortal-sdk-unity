using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DigitalWill.WortalEditor.Optimizations
{
    // Implementation of the Unused Asset Scanner tab for the Optimizations window
    public class UnusedAssetScanner
    {
        // Scroll position for the asset list
        private Vector2 stripScrollPos;

        // Container for unused assets
        private List<UnusedAssetInfo> unusedAssets = new List<UnusedAssetInfo>();

        // Filter settings
        private string searchFilter = "";
        private AssetType selectedAssetType = AssetType.All;
        private SortMethod currentSortMethod = SortMethod.Path;
        private bool sortAscending = true;

        // Asset selection tracking
        private List<int> selectedAssetIndices = new List<int>();
        private bool selectAllToggle = false;

        // Scan settings
        private bool includePackages = false;
        private bool scanResourcesFolder = true;
        private bool scanStreamingAssetsFolder = true;

        // Statistics
        private long totalUnusedSize = 0;
        private DateTime lastScanTime;
        private bool isScanning = false;

        // Draw the tab UI
        public void OnGUI()
        {
            DrawAssetStripPreviewTab();
        }

        private void DrawAssetStripPreviewTab()
        {
            EditorGUILayout.LabelField("Unused Asset Scanner", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("This tool identifies assets that are in your project but not used in any build scenes. You can select and remove them to reduce WebGL build size.", MessageType.Info);

            DrawScanSettings();
            EditorGUILayout.Space(5);

            // Only show filtering and stats if we have assets
            if (unusedAssets.Count > 0)
            {
                // Asset filtering options
                DrawFilterOptions();
                EditorGUILayout.Space(5);

                // Statistics
                DrawStatistics();
                EditorGUILayout.Space(5);

                // Actions for selected assets
                DrawBatchActions();
                EditorGUILayout.Space(5);
            }

            // Main asset list
            DrawAssetList();
        }

        private void DrawScanSettings()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Scan Settings", EditorStyles.boldLabel);

            includePackages = EditorGUILayout.Toggle("Include Packages", includePackages);
            scanResourcesFolder = EditorGUILayout.Toggle("Scan Resources Folder", scanResourcesFolder);
            scanStreamingAssetsFolder = EditorGUILayout.Toggle("Scan StreamingAssets", scanStreamingAssetsFolder);

            EditorGUILayout.BeginHorizontal();
            GUI.enabled = !isScanning;
            if (GUILayout.Button("Scan for Unused Assets"))
            {
                FindUnusedAssets();
            }
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        private void DrawFilterOptions()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Filter and Sort", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            searchFilter = EditorGUILayout.TextField("Search", searchFilter);
            if (GUILayout.Button("Clear", GUILayout.Width(60)))
            {
                searchFilter = "";
                GUI.FocusControl(null);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            selectedAssetType = (AssetType)EditorGUILayout.EnumPopup("Asset Type", selectedAssetType);

            EditorGUILayout.LabelField("Sort by:", GUILayout.Width(50));
            var newSortMethod = (SortMethod)EditorGUILayout.EnumPopup(currentSortMethod, GUILayout.Width(80));
            if (newSortMethod != currentSortMethod)
            {
                currentSortMethod = newSortMethod;
                SortAssets();
            }

            if (GUILayout.Button(sortAscending ? "↑" : "↓", GUILayout.Width(25)))
            {
                sortAscending = !sortAscending;
                SortAssets();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        private void DrawStatistics()
        {
            if (unusedAssets.Count > 0)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                EditorGUILayout.LabelField("Statistics", EditorStyles.boldLabel);
                EditorGUILayout.LabelField($"Total Unused Assets: {unusedAssets.Count}");
                EditorGUILayout.LabelField($"Total Size: {totalUnusedSize.FormatSize()}");

                if (lastScanTime != default)
                {
                    EditorGUILayout.LabelField($"Last Scan: {lastScanTime.ToString("yyyy-MM-dd HH:mm:ss")}");
                }

                EditorGUILayout.EndVertical();
            }
        }

        private void DrawBatchActions()
        {
            EditorGUILayout.BeginHorizontal();

            // Selection summary
            if (selectedAssetIndices.Count > 0)
            {
                // Calculate size of selected assets
                long selectedSize = 0;
                foreach (var index in selectedAssetIndices)
                {
                    selectedSize += unusedAssets[index].size;
                }

                EditorGUILayout.LabelField($"Selected: {selectedAssetIndices.Count} assets ({selectedSize.FormatSize()})");
            }
            else
            {
                EditorGUILayout.LabelField("No assets selected");
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Export CSV", GUILayout.Width(100)))
            {
                ExportToCSV();
            }

            GUI.enabled = selectedAssetIndices.Count > 0;
            if (GUILayout.Button("Delete Selected", GUILayout.Width(120)))
            {
                DeleteSelectedAssets();
            }
            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();
        }

        private void DrawAssetList()
        {
            // Filter assets based on current settings
            var filteredAssets = unusedAssets
                .Where(asset => (selectedAssetType == AssetType.All || asset.type == selectedAssetType) &&
                                (string.IsNullOrEmpty(searchFilter) ||
                                 asset.path.IndexOf(searchFilter, StringComparison.OrdinalIgnoreCase) >= 0))
                .ToList();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            // Draw header with select all option
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            var newSelectAll = EditorGUILayout.Toggle(selectAllToggle, GUILayout.Width(20));
            if (newSelectAll != selectAllToggle)
            {
                selectAllToggle = newSelectAll;
                selectedAssetIndices.Clear();
                if (selectAllToggle)
                {
                    for (int i = 0; i < filteredAssets.Count; i++)
                    {
                        selectedAssetIndices.Add(unusedAssets.IndexOf(filteredAssets[i]));
                    }
                }
            }

            EditorGUILayout.LabelField("Asset Path", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Type", EditorStyles.boldLabel, GUILayout.Width(80));
            EditorGUILayout.LabelField("Size", EditorStyles.boldLabel, GUILayout.Width(80));
            EditorGUILayout.LabelField("", GUILayout.Width(120)); // Actions column
            EditorGUILayout.EndHorizontal();

            stripScrollPos = EditorGUILayout.BeginScrollView(stripScrollPos);

            if (filteredAssets.Count == 0)
            {
                EditorGUILayout.LabelField("No unused assets detected (or not scanned yet).");
            }
            else
            {
                for (int i = 0; i < filteredAssets.Count; i++)
                {
                    var asset = filteredAssets[i];
                    int actualIndex = unusedAssets.IndexOf(asset);

                    EditorGUILayout.BeginHorizontal();

                    // Selection toggle
                    bool isSelected = selectedAssetIndices.Contains(actualIndex);
                    bool newSelected = EditorGUILayout.Toggle(isSelected, GUILayout.Width(20));
                    if (newSelected != isSelected)
                    {
                        if (newSelected)
                            selectedAssetIndices.Add(actualIndex);
                        else
                            selectedAssetIndices.Remove(actualIndex);
                    }

                    // Asset path (truncated if needed)
                    string displayPath = asset.path;
                    if (displayPath.Length > 40)
                    {
                        displayPath = "..." + displayPath.Substring(displayPath.Length - 40);
                    }
                    EditorGUILayout.LabelField(displayPath);

                    // Type and size
                    EditorGUILayout.LabelField(asset.type.ToString(), GUILayout.Width(80));
                    EditorGUILayout.LabelField(asset.size.FormatSize(), GUILayout.Width(80));

                    // Actions
                    if (GUILayout.Button("Select", GUILayout.Width(60)))
                    {
                        SelectAsset(asset.path);
                    }

                    if (GUILayout.Button("Delete", GUILayout.Width(60)))
                    {
                        if (DeleteAsset(asset.path))
                        {
                            // Remove asset from list if successfully deleted
                            unusedAssets.RemoveAt(actualIndex);
                            selectedAssetIndices.Remove(actualIndex);
                            RecalculateTotalSize();
                            // Adjust indices for selections after this point
                            for (int j = 0; j < selectedAssetIndices.Count; j++)
                            {
                                if (selectedAssetIndices[j] > actualIndex)
                                {
                                    selectedAssetIndices[j]--;
                                }
                            }
                            break; // Break to avoid enumeration issues
                        }
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void FindUnusedAssets()
        {
            isScanning = true;
            unusedAssets.Clear();
            selectedAssetIndices.Clear();

            // Get all asset paths based on scan settings
            List<string> allAssets = new List<string>();

            // Start with all assets from the project
            allAssets.AddRange(AssetDatabase.GetAllAssetPaths()
                .Where(path => path.StartsWith("Assets/") &&
                               !AssetDatabase.IsValidFolder(path) &&
                               !path.EndsWith(".cs") && // Skip code now. evaluate later
                               !path.EndsWith(".unity") && // Skip scenes
                               !path.EndsWith(".meta"))); // Skip meta files

            // Handle package filtering
            if (includePackages)
            {
                allAssets.AddRange(AssetDatabase.GetAllAssetPaths()
                    .Where(path => path.StartsWith("Packages/") &&
                                   !AssetDatabase.IsValidFolder(path) &&
                                   !path.EndsWith(".cs") &&
                                   !path.EndsWith(".unity") &&
                                   !path.EndsWith(".meta")));
            }

            // Handle Resources and StreamingAssets filtering
            if (!scanResourcesFolder)
            {
                allAssets = allAssets.Where(path => !path.Contains("/Resources/")).ToList();
            }

            if (!scanStreamingAssetsFolder)
            {
                allAssets = allAssets.Where(path => !path.Contains("/StreamingAssets/")).ToList();
            }

            // Convert to HashSet for faster lookup
            HashSet<string> allAssetsSet = new HashSet<string>(allAssets);

            // Find all used assets in build scenes
            HashSet<string> usedAssets = new HashSet<string>();
            var scenePaths = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();

            // If no scenes in build settings, warn user
            if (scenePaths.Length == 0)
            {
                Debug.LogWarning("[Asset Strip Preview] No scenes in build settings. Add scenes to get accurate results.");
                EditorUtility.DisplayDialog("No Build Scenes",
                    "No scenes are added to build settings. Please add and enable scenes in build settings to get accurate results.",
                    "OK");
                isScanning = false;
                return;
            }

            // Progress bar
            int processedScenes = 0;
            EditorUtility.DisplayProgressBar("Scanning Assets", "Finding dependencies in build scenes...", 0);

            try
            {
                foreach (var scenePath in scenePaths)
                {
                    EditorUtility.DisplayProgressBar("Scanning Assets",
                        $"Finding dependencies in {Path.GetFileNameWithoutExtension(scenePath)}...",
                        (float)processedScenes / scenePaths.Length);

                    // Get all dependencies of this scene
                    var deps = AssetDatabase.GetDependencies(scenePath, true);
                    foreach (var dep in deps)
                    {
                        usedAssets.Add(dep);
                    }

                    processedScenes++;
                }

                // Collect unused assets
                EditorUtility.DisplayProgressBar("Scanning Assets", "Collecting unused assets...", 0.9f);

                foreach (var asset in allAssetsSet)
                {
                    if (!usedAssets.Contains(asset))
                    {
                        unusedAssets.Add(new UnusedAssetInfo(asset));
                    }
                }

                // Sort assets
                SortAssets();

                // Calculate stats
                RecalculateTotalSize();
                lastScanTime = DateTime.Now;

                Debug.Log($"[Asset Strip Preview] Found {unusedAssets.Count} unused assets with total size of {totalUnusedSize.FormatSize()}.");
            }
            finally
            {
                EditorUtility.ClearProgressBar();
                isScanning = false;
            }
        }

        private void SortAssets()
        {
            switch (currentSortMethod)
            {
                case SortMethod.Path:
                    unusedAssets = sortAscending
                        ? unusedAssets.OrderBy(a => a.path).ToList()
                        : unusedAssets.OrderByDescending(a => a.path).ToList();
                    break;
                case SortMethod.Size:
                    unusedAssets = sortAscending
                        ? unusedAssets.OrderBy(a => a.size).ToList()
                        : unusedAssets.OrderByDescending(a => a.size).ToList();
                    break;
                case SortMethod.Type:
                    unusedAssets = sortAscending
                        ? unusedAssets.OrderBy(a => a.type).ToList()
                        : unusedAssets.OrderByDescending(a => a.type).ToList();
                    break;
                case SortMethod.Name:
                    unusedAssets = sortAscending
                        ? unusedAssets.OrderBy(a => a.filename).ToList()
                        : unusedAssets.OrderByDescending(a => a.filename).ToList();
                    break;
            }
        }

        private void RecalculateTotalSize()
        {
            totalUnusedSize = unusedAssets.Sum(asset => asset.size);
        }

        private void SelectAsset(string path)
        {
            var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
            if (asset != null)
            {
                Selection.activeObject = asset;
                EditorGUIUtility.PingObject(asset);
            }
        }

        private bool DeleteAsset(string path)
        {
            if (EditorUtility.DisplayDialog("Delete Asset",
                $"Are you sure you want to delete this asset?\n\n{path}",
                "Delete", "Cancel"))
            {
                bool success = AssetDatabase.DeleteAsset(path);
                if (success)
                {
                    AssetDatabase.Refresh();
                    return true;
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "Failed to delete asset.", "OK");
                }
            }
            return false;
        }

        private void DeleteSelectedAssets()
        {
            if (selectedAssetIndices.Count == 0) return;

            if (EditorUtility.DisplayDialog("Delete Selected Assets",
                $"Are you sure you want to delete {selectedAssetIndices.Count} assets?",
                "Delete", "Cancel"))
            {
                // Create a copy of the indices and sort in descending order 
                // to avoid index shifting issues when removing items
                var indicesToDelete = selectedAssetIndices.OrderByDescending(i => i).ToList();
                int successCount = 0;

                foreach (var index in indicesToDelete)
                {
                    if (index >= 0 && index < unusedAssets.Count)
                    {
                        string path = unusedAssets[index].path;
                        if (AssetDatabase.DeleteAsset(path))
                        {
                            successCount++;
                            unusedAssets.RemoveAt(index);
                        }
                    }
                }

                if (successCount > 0)
                {
                    selectedAssetIndices.Clear();
                    selectAllToggle = false;
                    RecalculateTotalSize();
                    AssetDatabase.Refresh();
                    Debug.Log($"[Asset Strip Preview] Successfully deleted {successCount} assets.");
                }
            }
        }

        private void ExportToCSV()
        {
            if (unusedAssets.Count == 0)
            {
                EditorUtility.DisplayDialog("Export Error", "No unused assets found to export.", "OK");
                return;
            }

            string path = EditorUtility.SaveFilePanel("Save CSV", "", "UnusedAssets.csv", "csv");
            if (string.IsNullOrEmpty(path)) return;

            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.WriteLine("Path,Type,Size,Extension");

                    foreach (var asset in unusedAssets)
                    {
                        writer.WriteLine($"\"{asset.path}\",{asset.type},{asset.size},{asset.extension}");
                    }
                }

                Debug.Log($"[Asset Strip Preview] CSV exported to: {path}");

                if (EditorUtility.DisplayDialog("Export Complete",
                    $"Successfully exported {unusedAssets.Count} unused assets to CSV.",
                    "Open File", "Close"))
                {
                    System.Diagnostics.Process.Start(path);
                }
            }
            catch (Exception e)
            {
                EditorUtility.DisplayDialog("Export Error", $"Failed to export CSV: {e.Message}", "OK");
                Debug.LogError($"[Asset Strip Preview] Export error: {e}");
            }
        }
    }
}