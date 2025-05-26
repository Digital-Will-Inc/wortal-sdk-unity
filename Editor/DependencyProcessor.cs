using System.IO;
using UnityEditor;
using UnityEngine;

namespace DigitalWill.Wortal.Editor
{
    [InitializeOnLoad]
    public class DependencyProcessor : AssetPostprocessor
    {
        private const string DEPENDENCIES_COPIED_KEY = "WortalSDK_DependenciesCopied";
        private const string DEPENDENCIES_RESOLVED_KEY = "WortalSDK_DependenciesResolved";

        // Updated file names to match your actual files and EDM4U expectations
        private static readonly string[] DEPENDENCY_FILES = {
            "GooglePlayManifest.xml",    // Your Android dependencies
            "GameCenterManifest.xml"     // Your iOS dependencies
        };

        // EDM4U expects these specific names in the target directory
        private static readonly string[] TARGET_FILE_NAMES = {
            "WortalAndroidDependencies.xml",  // EDM4U will recognize this pattern
            "WortalIOSDependencies.xml"       // EDM4U will recognize this pattern
        };

        static DependencyProcessor()
        {
            // Check if dependencies need to be copied after domain reload
            EditorApplication.delayCall += CheckAndCopyDependencies;
        }

        private static void CheckAndCopyDependencies()
        {
            // Only copy once per project (or when user explicitly requests it)
            if (EditorPrefs.GetBool(DEPENDENCIES_COPIED_KEY, false))
            {
                return;
            }

            CopyDependencyFiles();
            EditorPrefs.SetBool(DEPENDENCIES_COPIED_KEY, true);
        }

        [MenuItem("Wortal/Refresh Dependencies", false, 10)]
        public static void RefreshDependencies()
        {
            CopyDependencyFiles();
            Debug.Log("Wortal SDK: Dependencies refreshed!");
        }

        [MenuItem("Wortal/Reset Dependencies", false, 11)]
        public static void ResetDependencies()
        {
            EditorPrefs.DeleteKey(DEPENDENCIES_COPIED_KEY);
            EditorPrefs.DeleteKey(DEPENDENCIES_RESOLVED_KEY);
            CopyDependencyFiles();
            Debug.Log("Wortal SDK: Dependencies reset and refreshed!");
        }

        [MenuItem("Wortal/Resolve Android Dependencies", false, 12)]
        public static void ResolveAndroidDependencies()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            {
                if (EditorUtility.DisplayDialog(
                    "Platform Mismatch",
                    "Current build target is not Android. Switch to Android platform to resolve Android dependencies?",
                    "Switch Platform",
                    "Cancel"))
                {
                    EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
                    EditorApplication.delayCall += () => TriggerAndroidDependencyResolution();
                }
                return;
            }

            TriggerAndroidDependencyResolution();
        }

        [MenuItem("Wortal/Resolve iOS Dependencies", false, 13)]
        public static void ResolveIOSDependencies()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS)
            {
                if (EditorUtility.DisplayDialog(
                    "Platform Mismatch",
                    "Current build target is not iOS. Switch to iOS platform to resolve iOS dependencies?",
                    "Switch Platform",
                    "Cancel"))
                {
                    EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
                    EditorApplication.delayCall += () => TriggerIOSDependencyResolution();
                }
                return;
            }

            TriggerIOSDependencyResolution();
        }

        private static void CopyDependencyFiles()
        {
            string packagePath = GetWortalPackagePath();
            if (string.IsNullOrEmpty(packagePath))
            {
                Debug.LogWarning("Wortal SDK: Could not find package path for dependency copying");
                return;
            }

            string sourceDependenciesPath = Path.Combine(packagePath, "Editor", "Dependencies");
            string targetDependenciesPath = Path.Combine(Application.dataPath, "WortalSDK", "Editor");

            // Create target directory if it doesn't exist
            if (!Directory.Exists(targetDependenciesPath))
            {
                Directory.CreateDirectory(targetDependenciesPath);
            }

            // Copy each dependency file with proper naming for EDM4U
            for (int i = 0; i < DEPENDENCY_FILES.Length; i++)
            {
                string sourceFile = Path.Combine(sourceDependenciesPath, DEPENDENCY_FILES[i]);
                string targetFile = Path.Combine(targetDependenciesPath, TARGET_FILE_NAMES[i]);

                if (File.Exists(sourceFile))
                {
                    try
                    {
                        File.Copy(sourceFile, targetFile, true);
                        Debug.Log($"Wortal SDK: Copied {DEPENDENCY_FILES[i]} to {targetFile}");
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"Wortal SDK: Failed to copy {DEPENDENCY_FILES[i]}: {e.Message}");
                    }
                }
                else
                {
                    Debug.LogWarning($"Wortal SDK: Dependency file not found: {sourceFile}");
                }
            }

            // Refresh asset database to make Unity recognize the new files
            AssetDatabase.Refresh();

            // Show completion message with manual resolution instructions
            ShowDependencyCopyComplete();
        }

        private static void ShowDependencyCopyComplete()
        {
            Debug.Log("Wortal SDK: Dependencies copied successfully!");
            Debug.Log("To resolve dependencies:");
            Debug.Log("- For Android: Switch to Android platform, then use 'Wortal > Resolve Android Dependencies'");
            Debug.Log("- For iOS: Switch to iOS platform, then use 'Wortal > Resolve iOS Dependencies'");
            Debug.Log("- Or use manual resolution via Assets > External Dependency Manager");

            // Show dialog to user
            if (EditorUtility.DisplayDialog(
                "Wortal SDK Dependencies Copied",
                "Dependencies have been copied successfully!\n\n" +
                "To complete the setup:\n\n" +
                "• For Android: Switch to Android platform, then use 'Wortal > Resolve Android Dependencies'\n" +
                "• For iOS: Switch to iOS platform, then use 'Wortal > Resolve iOS Dependencies'\n\n" +
                "Or resolve manually via Assets > External Dependency Manager",
                "OK",
                "Resolve Now"))
            {
                // User clicked OK
            }
            else
            {
                // User wants to resolve now
                ResolveDependenciesForCurrentPlatform();
            }
        }

        private static void ResolveDependenciesForCurrentPlatform()
        {
            BuildTarget currentTarget = EditorUserBuildSettings.activeBuildTarget;

            switch (currentTarget)
            {
                case BuildTarget.Android:
                    TriggerAndroidDependencyResolution();
                    break;
                case BuildTarget.iOS:
                    TriggerIOSDependencyResolution();
                    break;
                default:
                    Debug.LogWarning($"Wortal SDK: Dependency resolution not supported for platform: {currentTarget}");
                    break;
            }
        }

        private static void TriggerAndroidDependencyResolution()
        {
            try
            {
                Debug.Log("Wortal SDK: Resolving Android dependencies...");

                // Method 1: Try using GooglePlayServices.PlayServicesResolver
                var androidResolverType = System.Type.GetType("GooglePlayServices.PlayServicesResolver, GooglePlayServices.PlayServicesResolver");
                if (androidResolverType != null)
                {
                    // Try the AutoResolve method first
                    var autoResolveMethod = androidResolverType.GetMethod("AutoResolve",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

                    if (autoResolveMethod != null)
                    {
                        Debug.Log("Wortal SDK: Triggering Android auto-resolve...");
                        autoResolveMethod.Invoke(null, new object[] { true });
                        return;
                    }

                    // Fallback to Resolve method
                    var resolveMethod = androidResolverType.GetMethod("Resolve",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    if (resolveMethod != null)
                    {
                        Debug.Log("Wortal SDK: Triggering Android dependency resolution...");
                        resolveMethod.Invoke(null, new object[] { null, true });
                        return;
                    }
                }

                // Method 2: Try using menu items
                EditorApplication.delayCall += () =>
                {
                    try
                    {
                        EditorApplication.ExecuteMenuItem("Assets/External Dependency Manager/Android Resolver/Resolve");
                        Debug.Log("Wortal SDK: Executed Android resolve menu item");
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogWarning($"Wortal SDK: Could not execute Android resolve menu: {e.Message}");
                        ShowManualAndroidInstructions();
                    }
                };
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Wortal SDK: Could not auto-trigger Android dependency resolution: {e.Message}");
                ShowManualAndroidInstructions();
            }
        }

        private static void TriggerIOSDependencyResolution()
        {
            try
            {
                Debug.Log("Wortal SDK: Resolving iOS dependencies...");

                // Method 1: Try using Google.IOSResolver
                var iosResolverType = System.Type.GetType("Google.IOSResolver, Google.IOSResolver");
                if (iosResolverType != null)
                {
                    var installMethod = iosResolverType.GetMethod("PodInstallAsync",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

                    if (installMethod != null)
                    {
                        Debug.Log("Wortal SDK: Triggering iOS dependency resolution...");
                        installMethod.Invoke(null, null);
                        return;
                    }
                }

                // Method 2: Try using menu items
                EditorApplication.delayCall += () =>
                {
                    try
                    {
                        EditorApplication.ExecuteMenuItem("Assets/External Dependency Manager/iOS Resolver/Install Cocoapods");
                        Debug.Log("Wortal SDK: Executed iOS resolve menu item");
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogWarning($"Wortal SDK: Could not execute iOS resolve menu: {e.Message}");
                        ShowManualIOSInstructions();
                    }
                };
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Wortal SDK: Could not auto-trigger iOS dependency resolution: {e.Message}");
                ShowManualIOSInstructions();
            }
        }

        private static void ShowManualAndroidInstructions()
        {
            Debug.Log("Wortal SDK: Please manually resolve Android dependencies via:");
            Debug.Log("- Assets > External Dependency Manager > Android Resolver > Resolve");

            EditorUtility.DisplayDialog(
                "Manual Android Resolution Required",
                "Please manually resolve Android dependencies:\n\n" +
                "Assets > External Dependency Manager > Android Resolver > Resolve",
                "OK"
            );
        }

        private static void ShowManualIOSInstructions()
        {
            Debug.Log("Wortal SDK: Please manually resolve iOS dependencies via:");
            Debug.Log("- Assets > External Dependency Manager > iOS Resolver > Install Cocoapods");

            EditorUtility.DisplayDialog(
                "Manual iOS Resolution Required",
                "Please manually resolve iOS dependencies:\n\n" +
                "Assets > External Dependency Manager > iOS Resolver > Install Cocoapods",
                "OK"
            );
        }

        private static string GetWortalPackagePath()
        {
            // Method 1: Try to find by looking for this specific file
            string[] guids = AssetDatabase.FindAssets("DependencyProcessor");
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (assetPath.Contains("DependencyProcessor.cs") && assetPath.Contains("DigitalWill"))
                {
                    // Go up from Editor/DependencyProcessor.cs to package root
                    string packagePath = Path.GetDirectoryName(Path.GetDirectoryName(assetPath));
                    return packagePath;
                }
            }

            // Method 2: Try to find by assembly definition
            guids = AssetDatabase.FindAssets("DigitalWill.WortalEditor");
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (assetPath.EndsWith(".asmdef"))
                {
                    // Go up from Editor/DigitalWill.WortalEditor.asmdef to package root
                    string packagePath = Path.GetDirectoryName(Path.GetDirectoryName(assetPath));
                    return packagePath;
                }
            }

            // Method 3: Try to find by looking for dependency files directly
            guids = AssetDatabase.FindAssets("GooglePlayManifest");
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (assetPath.Contains("Dependencies") && assetPath.EndsWith(".xml"))
                {
                    // Go up from Editor/Dependencies/GooglePlayManifest.xml to package root
                    string packagePath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(assetPath)));
                    return packagePath;
                }
            }

            return null;
        }

        // Clean up when package is removed
        public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            // Check if our dependency files were deleted (indicating package removal)
            foreach (string deletedAsset in deletedAssets)
            {
                if (deletedAsset.Contains("WortalSDK") && deletedAsset.Contains("Dependencies"))
                {
                    Debug.Log("Wortal SDK: Dependency files removed");
                    EditorPrefs.DeleteKey(DEPENDENCIES_COPIED_KEY);
                    EditorPrefs.DeleteKey(DEPENDENCIES_RESOLVED_KEY);
                    break;
                }
            }
        }

        // Debug method to check paths
        [MenuItem("Wortal/Debug Package Path", false, 50)]
        public static void DebugPackagePath()
        {
            string packagePath = GetWortalPackagePath();
            Debug.Log($"Wortal SDK Package Path: {packagePath}");
            Debug.Log($"Current Build Target: {EditorUserBuildSettings.activeBuildTarget}");

            if (!string.IsNullOrEmpty(packagePath))
            {
                string dependenciesPath = Path.Combine(packagePath, "Editor", "Dependencies");
                Debug.Log($"Dependencies Path: {dependenciesPath}");
                Debug.Log($"Dependencies Path Exists: {Directory.Exists(dependenciesPath)}");

                if (Directory.Exists(dependenciesPath))
                {
                    string[] files = Directory.GetFiles(dependenciesPath, "*.xml");
                    Debug.Log($"XML files found: {string.Join(", ", files)}");
                }
            }
        }

        // Menu validation - only show resolve options when appropriate
        [MenuItem("Wortal/Resolve Android Dependencies", true)]
        private static bool ValidateResolveAndroidDependencies()
        {
            return EditorPrefs.GetBool(DEPENDENCIES_COPIED_KEY, false);
        }

        [MenuItem("Wortal/Resolve iOS Dependencies", true)]
        private static bool ValidateResolveIOSDependencies()
        {
            return EditorPrefs.GetBool(DEPENDENCIES_COPIED_KEY, false);
        }
    }
}