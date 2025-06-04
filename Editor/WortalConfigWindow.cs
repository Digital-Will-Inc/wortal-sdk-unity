using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEditor.Build;
using DigitalWill.WortalSDK;
using DigitalWill.WortalEditor.Optimizations;

namespace DigitalWill.WortalEditor
{
    public class WortalConfigWindow : EditorWindow
    {
        private enum ConfigTab { General, WebGL, Android, iOS, Optimizations }
        private ConfigTab currentTab = ConfigTab.General;

        // General state
        private static AddRequest addRequest;
        private static ListRequest listRequest;
        private static bool isInstalling = false;

        // WebGL state
        private const string TEMPLATE_NAME = "PROJECT:Wortal";
        private const string TEMPLATE_PATH = "Assets/WebGLTemplates/Wortal";
        private const string TEMPLATE_SOURCE = "Packages/jp.co.digitalwill.wortal/Assets/WebGLTemplates/Wortal";

        // Dependencies state
        private const string EDM4U_PACKAGE_NAME = "com.google.external-dependency-manager";
        private const string EDM4U_GIT_URL = "https://github.com/googlesamples/unity-jar-resolver.git?path=upm";
        private const string DEPENDENCIES_COPIED_KEY = "WortalSDK_DependenciesCopied";

        // Optimizations state
        private LazyLoadConfig lazyConfig;
        private LazyLoadSystem lazyLoadSystem;
        private UnusedAssetScanner unusedAssetScanner;
        private const string DefaultConfigPath = "Assets/Resources/LazyLoadConfig.asset";

        // UI
        private Vector2 scrollPosition;
        private GUIStyle tabButtonStyle;
        private Color activeTabColor = new Color(0.4f, 0.6f, 1f, 0.3f);

        [MenuItem("Wortal/Configuration", false, 0)]
        public static void ShowWindow()
        {
            var window = GetWindow<WortalConfigWindow>("Wortal Configuration");
            window.minSize = new Vector2(500, 400);
        }

        private void OnEnable()
        {
            InitializeOptimizations();
        }

        private void OnDisable()
        {
            lazyLoadSystem?.Cleanup();
        }

        private void InitializeOptimizations()
        {
            try
            {
                string configPath = EditorPrefs.GetString("WortalSDK_LazyLoadConfigPath", DefaultConfigPath);
                lazyConfig = AssetDatabase.LoadAssetAtPath<LazyLoadConfig>(configPath);

                if (lazyConfig == null)
                {
                    lazyConfig = ScriptableObject.CreateInstance<LazyLoadConfig>();
                    var dir = Path.GetDirectoryName(configPath);
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    AssetDatabase.CreateAsset(lazyConfig, configPath);
                    AssetDatabase.SaveAssets();
                }

                lazyLoadSystem = new LazyLoadSystem();
                lazyLoadSystem.Initialize(null, lazyConfig);
                unusedAssetScanner = new UnusedAssetScanner();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Wortal SDK: Could not initialize optimizations: {e.Message}");
            }
        }

        private void OnGUI()
        {
            DrawHeader();
            DrawTabs();

            EditorGUILayout.Space(10);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            switch (currentTab)
            {
                case ConfigTab.General: DrawGeneralTab(); break;
                case ConfigTab.WebGL: DrawWebGLTab(); break;
                case ConfigTab.Android: DrawAndroidTab(); break;
                case ConfigTab.iOS: DrawIOSTab(); break;
                case ConfigTab.Optimizations: DrawOptimizationsTab(); break;
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawHeader()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            var titleStyle = new GUIStyle(EditorStyles.boldLabel) { fontSize = 16 };
            EditorGUILayout.LabelField("Wortal SDK Configuration", titleStyle);
            EditorGUILayout.LabelField("Version 6.2.2 - Unified SDK for WebGL, Android, and iOS", EditorStyles.miniLabel);
            EditorGUILayout.EndVertical();
        }

        private void DrawTabs()
        {
            if (tabButtonStyle == null)
            {
                tabButtonStyle = new GUIStyle(EditorStyles.toolbarButton);
                tabButtonStyle.fixedHeight = 25;
            }

            EditorGUILayout.BeginHorizontal();

            DrawTab("General", ConfigTab.General);
            DrawTab("WebGL", ConfigTab.WebGL);
            DrawTab("Android", ConfigTab.Android);
            DrawTab("iOS", ConfigTab.iOS);
            DrawTab("Optimizations", ConfigTab.Optimizations);

            EditorGUILayout.EndHorizontal();
        }

        private void DrawTab(string label, ConfigTab tab)
        {
            var isActive = currentTab == tab;
            var originalColor = GUI.backgroundColor;

            if (isActive) GUI.backgroundColor = activeTabColor;

            if (GUILayout.Button(label, tabButtonStyle))
                currentTab = tab;

            GUI.backgroundColor = originalColor;
        }

        private void DrawGeneralTab()
        {
            EditorGUILayout.LabelField("General Settings", EditorStyles.boldLabel);

            // Dependencies section
            DrawSection("Dependencies", () =>
            {
                EditorGUILayout.HelpBox("External Dependency Manager is required for Android and iOS builds.", MessageType.Info);

                EditorGUILayout.BeginHorizontal();
                GUI.enabled = !isInstalling;
                if (GUILayout.Button(isInstalling ? "Installing..." : "Install Dependencies"))
                {
                    CheckAndInstallDependencies();
                }
                GUI.enabled = true;

                if (GUILayout.Button("Check Status"))
                {
                    CheckDependenciesStatus();
                }
                EditorGUILayout.EndHorizontal();
            });

            // About section
            DrawSection("About", () =>
            {
                EditorGUILayout.LabelField("Wortal SDK for Unity", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Unified SDK for WebGL, Android, and iOS game services");
                EditorGUILayout.LabelField("© Digital Will Inc", EditorStyles.miniLabel);

                if (GUILayout.Button("Documentation", GUILayout.Width(120)))
                {
                    Application.OpenURL("https://github.com/Digital-Will-Inc/wortal-sdk-unity");
                }
            });
        }

        private void DrawWebGLTab()
        {
            EditorGUILayout.LabelField("WebGL Configuration", EditorStyles.boldLabel);

            DrawSection("Template & Settings", () =>
            {
                EditorGUILayout.HelpBox("Wortal WebGL template is required for proper integration.", MessageType.Info);

                var currentTemplate = PlayerSettings.WebGL.template;
                var isCorrectTemplate = currentTemplate == TEMPLATE_NAME;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Current Template:", currentTemplate);
                if (!isCorrectTemplate)
                {
                    EditorGUILayout.LabelField("⚠️", GUILayout.Width(20));
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Install WebGL Template"))
                {
                    InstallWebGLTemplate();
                }
                if (GUILayout.Button("Apply All Settings"))
                {
                    SetWebGLProjectSettings();
                }
                EditorGUILayout.EndHorizontal();
            });

            DrawSection("Build Settings", () =>
            {
                PlayerSettings.WebGL.compressionFormat = (WebGLCompressionFormat)EditorGUILayout.EnumPopup(
                    "Compression Format", PlayerSettings.WebGL.compressionFormat);

                PlayerSettings.WebGL.memorySize = EditorGUILayout.IntSlider(
                    "Memory Size (MB)", PlayerSettings.WebGL.memorySize, 64, 2048);

                var runInBackground = EditorGUILayout.Toggle("Run In Background", PlayerSettings.runInBackground);
                if (runInBackground != PlayerSettings.runInBackground)
                    PlayerSettings.runInBackground = runInBackground;

                var decompressionFallback = EditorGUILayout.Toggle("Decompression Fallback", PlayerSettings.WebGL.decompressionFallback);
                if (decompressionFallback != PlayerSettings.WebGL.decompressionFallback)
                    PlayerSettings.WebGL.decompressionFallback = decompressionFallback;
            });
        }

        private void DrawAndroidTab()
        {
            EditorGUILayout.LabelField("Android Configuration", EditorStyles.boldLabel);

            DrawSection("Dependencies", () =>
            {
                var hasDependencies = EditorPrefs.GetBool(DEPENDENCIES_COPIED_KEY, false);
                var isAndroidPlatform = EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android;

                EditorGUILayout.HelpBox($"Current Platform: {EditorUserBuildSettings.activeBuildTarget}",
                    isAndroidPlatform ? MessageType.Info : MessageType.Warning);

                EditorGUILayout.BeginHorizontal();
                GUI.enabled = hasDependencies;
                if (GUILayout.Button("Resolve Dependencies"))
                {
                    ResolveAndroidDependencies();
                }
                GUI.enabled = true;

                if (GUILayout.Button("Refresh Dependencies"))
                {
                    RefreshDependencies();
                }
                EditorGUILayout.EndHorizontal();

                if (!isAndroidPlatform && GUILayout.Button("Switch to Android Platform"))
                {
                    EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
                }
            });

            DrawSection("Google Play Services", () =>
            {
                EditorGUILayout.HelpBox("Android dependencies include Google Play Services for achievements, leaderboards, and cloud save.", MessageType.Info);

                if (GUILayout.Button("Open Manual Instructions"))
                {
                    Application.OpenURL("https://github.com/Digital-Will-Inc/wortal-sdk-unity#android-setup");
                }
            });
        }

        private void DrawIOSTab()
        {
            EditorGUILayout.LabelField("iOS Configuration", EditorStyles.boldLabel);

            DrawSection("Dependencies", () =>
            {
                var hasDependencies = EditorPrefs.GetBool(DEPENDENCIES_COPIED_KEY, false);
                var isiOSPlatform = EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS;

                EditorGUILayout.HelpBox($"Current Platform: {EditorUserBuildSettings.activeBuildTarget}",
                    isiOSPlatform ? MessageType.Info : MessageType.Warning);

                EditorGUILayout.BeginHorizontal();
                GUI.enabled = hasDependencies;
                if (GUILayout.Button("Resolve Dependencies"))
                {
                    ResolveIOSDependencies();
                }
                GUI.enabled = true;

                if (GUILayout.Button("Refresh Dependencies"))
                {
                    RefreshDependencies();
                }
                EditorGUILayout.EndHorizontal();

                if (!isiOSPlatform && GUILayout.Button("Switch to iOS Platform"))
                {
                    EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
                }
            });

            DrawSection("Game Center", () =>
            {
                EditorGUILayout.HelpBox("iOS dependencies include Game Center for achievements, leaderboards, and multiplayer features.", MessageType.Info);
                EditorGUILayout.HelpBox("Post-build processing will automatically configure Game Center entitlements.", MessageType.Info);

                if (GUILayout.Button("Open Manual Instructions"))
                {
                    Application.OpenURL("https://github.com/Digital-Will-Inc/wortal-sdk-unity#ios-setup");
                }
            });
        }

        private void DrawOptimizationsTab()
        {
            EditorGUILayout.LabelField("Build Optimizations", EditorStyles.boldLabel);

            if (lazyConfig == null)
            {
                EditorGUILayout.HelpBox("Optimization config not loaded. Try reopening the window.", MessageType.Warning);
                return;
            }

            DrawSection("Lazy Loading", () =>
            {
                var enableLazy = EditorGUILayout.Toggle("Enable Lazy Loading", lazyConfig.enableLazyLoad);
                if (enableLazy != lazyConfig.enableLazyLoad)
                {
                    Undo.RecordObject(lazyConfig, "Toggle Lazy Loading");
                    lazyConfig.enableLazyLoad = enableLazy;
                    EditorUtility.SetDirty(lazyConfig);
                }

                EditorGUILayout.HelpBox("Lazy loading reduces initial bundle size by loading assets on demand.", MessageType.Info);

                if (lazyConfig.enableLazyLoad && lazyLoadSystem != null)
                {
                    EditorGUILayout.Space(5);
                    lazyLoadSystem.OnGUI();
                }
            });

            DrawSection("Asset Analysis", () =>
            {
                EditorGUILayout.HelpBox("Analyze which assets might be stripped from builds to optimize size.", MessageType.Info);

                if (unusedAssetScanner != null)
                {
                    unusedAssetScanner.OnGUI();
                }
            });
        }

        private void DrawSection(string title, System.Action content)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField(title, EditorStyles.miniBoldLabel);
            EditorGUILayout.Space(3);
            content();
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(5);
        }

        // Dependency management methods
        private void CheckAndInstallDependencies()
        {
            if (isInstalling) return;
            Debug.Log("Wortal SDK: Checking dependencies...");
            listRequest = Client.List();
            EditorApplication.update += CheckListProgress;
        }

        private void CheckListProgress()
        {
            if (!listRequest.IsCompleted) return;
            EditorApplication.update -= CheckListProgress;

            if (listRequest.Status == StatusCode.Success)
            {
                bool hasEDM4U = listRequest.Result.Any(p => p.name == EDM4U_PACKAGE_NAME);
                if (hasEDM4U)
                {
                    Debug.Log("Wortal SDK: All dependencies installed! ✓");
                    RefreshDependencies();
                }
                else
                {
                    InstallEDM4U();
                }
            }
            else
            {
                Debug.LogError($"Wortal SDK: Failed to check packages: {listRequest.Error.message}");
            }
            listRequest = null;
        }

        private void InstallEDM4U()
        {
            isInstalling = true;
            Debug.Log("Wortal SDK: Installing External Dependency Manager...");
            addRequest = Client.Add(EDM4U_GIT_URL);
            EditorApplication.update += CheckInstallProgress;
        }

        private void CheckInstallProgress()
        {
            if (!addRequest.IsCompleted) return;
            EditorApplication.update -= CheckInstallProgress;
            isInstalling = false;

            if (addRequest.Status == StatusCode.Success)
            {
                Debug.Log("Wortal SDK: External Dependency Manager installed! ✓");
                RefreshDependencies();
            }
            else
            {
                Debug.LogError($"Wortal SDK: Installation failed: {addRequest.Error.message}");
            }
            addRequest = null;
        }

        private void CheckDependenciesStatus()
        {
            Debug.Log("Checking Wortal SDK dependencies...");
            CheckAndInstallDependencies();
        }

        // WebGL methods
        private void InstallWebGLTemplate()
        {
            try
            {
                string destinationFolder = Path.GetFullPath(TEMPLATE_PATH);
                string sourceFolder = Path.GetFullPath(TEMPLATE_SOURCE);

                if (!Directory.Exists(destinationFolder))
                    Directory.CreateDirectory(destinationFolder);

                foreach (string dirPath in Directory.GetDirectories(sourceFolder, "*", SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(sourceFolder, destinationFolder));

                foreach (string filePath in Directory.GetFiles(sourceFolder, "*.*", SearchOption.AllDirectories))
                    File.Copy(filePath, filePath.Replace(sourceFolder, destinationFolder), true);

                AssetDatabase.Refresh();
                PlayerSettings.WebGL.template = TEMPLATE_NAME;
                Debug.Log("Wortal SDK: WebGL template installed successfully!");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to install WebGL template: {e.Message}");
            }
        }

        private void SetWebGLProjectSettings()
        {
            PlayerSettings.runInBackground = true;
#if UNITY_2022_1_OR_NEWER
            PlayerSettings.SetIl2CppCodeGeneration(UnityEditor.Build.NamedBuildTarget.WebGL, UnityEditor.Build.Il2CppCodeGeneration.OptimizeSize);
#else
            EditorUserBuildSettings.il2CppCodeGeneration = UnityEditor.Il2CppCodeGeneration.OptimizeSize;
#endif
            Debug.Log("Wortal SDK: WebGL settings applied!");
        }

        // Platform dependency methods (simplified versions)
        private void RefreshDependencies()
        {
            try
            {
                DependencyProcessor.RefreshDependencies();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Could not refresh dependencies: {e.Message}");
            }
        }

        private void ResolveAndroidDependencies()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            {
                if (EditorUtility.DisplayDialog("Platform Mismatch",
                    "Switch to Android platform to resolve dependencies?", "Switch", "Cancel"))
                {
                    EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
                    EditorApplication.delayCall += ResolveAndroidDependencies;
                }
                return;
            }

            try
            {
                EditorApplication.ExecuteMenuItem("Assets/External Dependency Manager/Android Resolver/Resolve");
            }
            catch
            {
                EditorUtility.DisplayDialog("Manual Resolution Required",
                    "Please resolve manually via:\nAssets > External Dependency Manager > Android Resolver > Resolve", "OK");
            }
        }

        private void ResolveIOSDependencies()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS)
            {
                if (EditorUtility.DisplayDialog("Platform Mismatch",
                    "Switch to iOS platform to resolve dependencies?", "Switch", "Cancel"))
                {
                    EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
                    EditorApplication.delayCall += ResolveIOSDependencies;
                }
                return;
            }

            try
            {
                EditorApplication.ExecuteMenuItem("Assets/External Dependency Manager/iOS Resolver/Install Cocoapods");
            }
            catch
            {
                EditorUtility.DisplayDialog("Manual Resolution Required",
                    "Please resolve manually via:\nAssets > External Dependency Manager > iOS Resolver > Install Cocoapods", "OK");
            }
        }
    }
}