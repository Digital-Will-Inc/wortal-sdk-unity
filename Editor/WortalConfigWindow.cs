using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Globalization;
using System.Text;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEditor.Build;
using DigitalWill.WortalSDK;
using DigitalWill.WortalEditor.Optimizations;
using System.Collections.Generic; // Added for List<T>

namespace DigitalWill.WortalEditor
{
    public class WortalConfigWindow : EditorWindow
    {
        // State for the new single-page layout
        private bool _overviewExpanded = true;
        private bool _generalSettingsExpanded = true;
        private bool _crossPlatformMappingExpanded = true;
        private bool _platformSettingsExpanded = true;

        private enum MappingTab { Achievements, Leaderboard }
        private MappingTab _currentMappingTab = MappingTab.Achievements;

        private enum PlatformTab { WebGL, Android, iOS }
        private PlatformTab _currentPlatformTab = PlatformTab.WebGL;

        private enum OptimizationTab { AssetStrip, LazyLoad }
        private OptimizationTab _currentOptimizationTab = OptimizationTab.AssetStrip;

        // Used for Google Play Games XML input in settings tab
        private string xmlInputText = "";

        // General state
        private static AddRequest addRequest;
        private static ListRequest listRequest;
        private static bool isInstalling = false;

        // Settings state
        private WortalSettings wortalSettings;
        private SerializedObject serializedSettings;

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
        private const string ConfigPathKey = "WortalSDK_LazyLoadConfigPath";

        // UI
        private Vector2 scrollPosition;
        private GUIStyle sectionHeaderStyle;
        private GUIStyle warningBoxStyle;
        private GUIStyle successBoxStyle;

        [MenuItem("Window/Wortal/Configuration")]
        public static void ShowWindow()
        {
            var window = GetWindow<WortalConfigWindow>("Wortal Configuration");
            window.minSize = new Vector2(700, 600);
        }

        private void OnEnable()
        {
            InitializeSettings();
            InitializeOptimizations();
            // Styles are initialized in OnGUI to avoid issues with EditorStyles.
        }

        private void OnDisable()
        {
            lazyLoadSystem?.Cleanup();
        }

        private void InitializeSettings()
        {
            wortalSettings = WortalSettings.Instance;
            if (wortalSettings != null)
            {
                serializedSettings = new SerializedObject(wortalSettings);
            }
        }

        private void InitializeStyles()
        {
            sectionHeaderStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 12,
                normal = { textColor = new Color(0.2f, 0.4f, 0.8f) }
            };
            warningBoxStyle = new GUIStyle(EditorStyles.helpBox)
            {
                normal = { textColor = new Color(0.8f, 0.4f, 0.2f) }
            };
            successBoxStyle = new GUIStyle(EditorStyles.helpBox)
            {
                normal = { textColor = new Color(0.2f, 0.6f, 0.2f) }
            };
        }

        private void InitializeOptimizations()
        {
            try
            {
                string configPath = EditorPrefs.GetString(ConfigPathKey, DefaultConfigPath);
                lazyConfig = AssetDatabase.LoadAssetAtPath<LazyLoadConfig>(configPath);
                if (lazyConfig == null)
                {
                    lazyConfig = CreateInstance<LazyLoadConfig>();
                    var dir = Path.GetDirectoryName(configPath);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                    AssetDatabase.CreateAsset(lazyConfig, configPath);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                ValidateConfig();
                lazyLoadSystem = new LazyLoadSystem();
                lazyLoadSystem.Initialize(this, lazyConfig);
                unusedAssetScanner = new UnusedAssetScanner();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Wortal SDK: Could not initialize optimizations: {e.Message}");
            }
        }

        private void ValidateConfig()
        {
            if (lazyConfig.assetPriorityGroups == null)
                lazyConfig.assetPriorityGroups = new List<AssetPriorityGroup>();

            for (int i = 0; i < lazyConfig.assetPriorityGroups.Count; i++)
            {
                var group = lazyConfig.assetPriorityGroups[i];
                if (group == null)
                {
                    lazyConfig.assetPriorityGroups.RemoveAt(i--);
                    continue;
                }
                group.RebuildDictionary();
                if (group.assetsByType == null)
                    group.assetsByType = new Dictionary<string, List<string>>();

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
            if (sectionHeaderStyle == null)
            {
                InitializeStyles();
            }

            if (wortalSettings == null)
            {
                EditorGUILayout.HelpBox("WortalSettings asset not found. Please create one to configure the SDK.", MessageType.Error);
                if (GUILayout.Button("Create Wortal Settings Asset"))
                {
                    CreateWortalSettings();
                }
                return;
            }

            serializedSettings.Update();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            DrawHeaderAndSetupGuide();

            var overviewRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
            EditorGUI.DrawRect(overviewRect, new Color(0.18f, 0.18f, 0.18f, 1f));
            _overviewExpanded = EditorGUI.Foldout(overviewRect, _overviewExpanded, "Overview", true, EditorStyles.foldoutHeader);
            if (_overviewExpanded)
            {
                DrawOverviewSection();
            }

            var generalRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
            EditorGUI.DrawRect(generalRect, new Color(0.18f, 0.18f, 0.18f, 1f));
            _generalSettingsExpanded = EditorGUI.Foldout(generalRect, _generalSettingsExpanded, "General Settings", true, EditorStyles.foldoutHeader);
            if (_generalSettingsExpanded)
            {
                DrawGeneralSettingsSection();
            }

            var mappingRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
            EditorGUI.DrawRect(mappingRect, new Color(0.18f, 0.18f, 0.18f, 1f));
            _crossPlatformMappingExpanded = EditorGUI.Foldout(mappingRect, _crossPlatformMappingExpanded, "Cross-Platform Mapping", true, EditorStyles.foldoutHeader);
            if (_crossPlatformMappingExpanded)
            {
                DrawCrossPlatformMappingSection();
            }

            var platformRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
            EditorGUI.DrawRect(platformRect, new Color(0.18f, 0.18f, 0.18f, 1f));
            _platformSettingsExpanded = EditorGUI.Foldout(platformRect, _platformSettingsExpanded, "Platform Settings", true, EditorStyles.foldoutHeader);
            if (_platformSettingsExpanded)
            {
                DrawPlatformSettingsSection();
            }

            EditorGUILayout.EndScrollView();

            serializedSettings.ApplyModifiedProperties();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(wortalSettings);
                AssetDatabase.SaveAssets();
            }
        }

        private void DrawHeaderAndSetupGuide()
        {
            EditorGUILayout.BeginHorizontal();

            // Left side: Title, Version, Status, Docs
            EditorGUILayout.BeginVertical();
            var titleStyle = new GUIStyle(EditorStyles.boldLabel) { fontSize = 16 };


            EditorGUILayout.LabelField("Wortal SDK Configuration", titleStyle, GUILayout.Width(250));
            EditorGUILayout.LabelField("Version 6.2.2 - Unified SDK for WebGL, Android, and iOS", EditorStyles.miniLabel);

            var statusValid = wortalSettings != null && wortalSettings.ValidateAll();
            var statusText = statusValid ? "Status: Ready!" : "Status: Needs Configuration";
            var statusColor = statusValid ? Color.green : new Color(1.0f, 0.8f, 0.4f);
            var originalColor = GUI.contentColor;
            GUI.contentColor = statusColor;
            EditorGUILayout.LabelField(statusText, EditorStyles.boldLabel);
            GUI.contentColor = originalColor;

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Documentation", GUILayout.Width(120)))
            {
                Application.OpenURL("https://games-api.ai/wortal-unity/");
            }

            if (GUILayout.Button("GitHub", GUILayout.Width(120)))
            {
                Application.OpenURL("https://github.com/Digital-Will-Inc/wortal-sdk-unity");
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();

            // Right side: Setup Guide
            EditorGUILayout.BeginVertical();
            var rightAlignTitle = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleRight };
            var rightAlign = new GUIStyle(EditorStyles.miniLabel) { alignment = TextAnchor.MiddleRight };
            EditorGUILayout.LabelField("Setup Guide", rightAlignTitle, GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("1. Configure your settings", rightAlign, GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("2. Install Platform Dependencies", rightAlign, GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("3. Configure Platform Specific Settings", rightAlign, GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("4. Build and Enjoy :)!", rightAlign, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        private void DrawOverviewSection()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Dependencies", EditorStyles.boldLabel);
            DrawDependencyStatus();
            EditorGUILayout.Space();
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
            EditorGUILayout.EndVertical();
        }

        private void DrawGeneralSettingsSection()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.PropertyField(serializedSettings.FindProperty("enableDebugLogging"),
                new GUIContent("Enable Debug Logging", "Show detailed logs in console"));
            EditorGUILayout.PropertyField(serializedSettings.FindProperty("autoInitialize"),
                new GUIContent("Auto Initialize", "Automatically initialize SDK on game start"));
            EditorGUILayout.EndVertical();
        }

        private void DrawCrossPlatformMappingSection()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            _currentMappingTab = (MappingTab)GUILayout.Toolbar((int)_currentMappingTab, new[] { "Achievements", "Leaderboard" });
            EditorGUILayout.Space();

            switch (_currentMappingTab)
            {
                case MappingTab.Achievements:
                    DrawAchievementMappings();
                    break;
                case MappingTab.Leaderboard:
                    DrawLeaderboardMappings();
                    break;
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawPlatformSettingsSection()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            _currentPlatformTab = (PlatformTab)GUILayout.Toolbar((int)_currentPlatformTab, new[] { "WebGL", "Android", "iOS" });
            EditorGUILayout.Space();

            switch (_currentPlatformTab)
            {
                case PlatformTab.WebGL:
                    DrawWebGLPlatformTab();
                    break;
                case PlatformTab.Android:
                    DrawAndroidPlatformTab();
                    break;
                case PlatformTab.iOS:
                    DrawiOSPlatformTab();
                    break;
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawSection(string title, Action content)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            EditorGUILayout.Space(3);
            content();
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(5);
        }

        private void DrawWebGLPlatformTab()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("WebGL Game ID", GUILayout.Width(EditorGUIUtility.labelWidth));
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedSettings.FindProperty("webglGameId"), GUIContent.none);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("Game ID for WebGL will be automatically assigned when you upload your build to the Wortal Dashboard. No manual input required.", MessageType.Info);
            EditorGUILayout.Space();

            DrawSection("WebGL Configuration", () =>
            {
                EditorGUILayout.LabelField("Template and Settings", EditorStyles.boldLabel);
                var isCorrectTemplate = PlayerSettings.WebGL.template == TEMPLATE_NAME;
                if (!isCorrectTemplate)
                {
                    EditorGUILayout.HelpBox("Wortal WebGL template required for proper integration.", MessageType.Warning);
                }
                else
                {
                    EditorGUILayout.HelpBox("Wortal WebGL template is correctly assigned.", MessageType.Info);
                }

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Install WebGL Template")) InstallWebGLTemplate();
                if (GUILayout.Button("Apply All Settings")) SetWebGLProjectSettings();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Build Settings", EditorStyles.boldLabel);
                PlayerSettings.WebGL.compressionFormat = (WebGLCompressionFormat)EditorGUILayout.EnumPopup(
                    "Compression Format", PlayerSettings.WebGL.compressionFormat);
                PlayerSettings.WebGL.memorySize = EditorGUILayout.IntField(
                    "Memory Size (MB)", PlayerSettings.WebGL.memorySize);
                PlayerSettings.runInBackground = EditorGUILayout.Toggle("Run In Background", PlayerSettings.runInBackground);
                PlayerSettings.WebGL.decompressionFallback = EditorGUILayout.Toggle("Decompression Fallback", PlayerSettings.WebGL.decompressionFallback);
            });

            DrawSection("Optimization", () =>
            {
                _currentOptimizationTab = (OptimizationTab)GUILayout.Toolbar((int)_currentOptimizationTab, new[] { "Asset Strip", "Lazy Load" });
                EditorGUILayout.Space();

                switch (_currentOptimizationTab)
                {
                    case OptimizationTab.AssetStrip:
                        DrawWebGLAssetStripSubTab();
                        break;
                    case OptimizationTab.LazyLoad:
                        if (lazyConfig != null)
                        {
                            lazyConfig.enableLazyLoad = EditorGUILayout.Toggle(
                               new GUIContent("Enable Lazy Loading", "Toggle this ON to enable LazyLoadManager."),
                               lazyConfig.enableLazyLoad);
                        }
                        DrawWebGLLazyLoadSubTab();
                        break;
                }
            });
        }

        private void DrawAndroidPlatformTab()
        {
            DrawSection("Google Play Games Settings", () =>
            {
                EditorGUILayout.PropertyField(serializedSettings.FindProperty("enableGooglePlayGames"),
                    new GUIContent("Enable Google Play Games", "Enable Google Play Games services for Android"));
                if (wortalSettings.enableGooglePlayGames)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.LabelField("Paste Google Play Games XML Resource:");
                    xmlInputText = EditorGUILayout.TextArea(xmlInputText, GUILayout.Height(150));
                    if (GUILayout.Button("Import from XML"))
                    {
                        ImportGooglePlayXml(xmlInputText);
                        SyncWortalToGPGSetup();
                    }
                    EditorGUILayout.Space(5);

                    EditorGUILayout.PropertyField(serializedSettings.FindProperty("googlePlayGamesAppId"),
                        new GUIContent("App ID", "Google Play Games application ID"));
                    EditorGUILayout.PropertyField(serializedSettings.FindProperty("googlePlayGamesClientId"),
                        new GUIContent("Web App Client ID", "OAuth 2.0 Web Application Client ID from Google Cloud Console"));

                    if (string.IsNullOrEmpty(wortalSettings.googlePlayGamesAppId)) EditorGUILayout.HelpBox("Google Play Games App ID is required.", MessageType.Error);
                    if (string.IsNullOrEmpty(wortalSettings.googlePlayGamesClientId)) EditorGUILayout.HelpBox("Web App Client ID is required.", MessageType.Error);

                    if (GUILayout.Button("🔄 Sync to Google Play Games Setup"))
                    {
                        SyncWortalToGPGSetup();
                        EditorUtility.DisplayDialog("Sync Complete", "Wortal settings have been synced to Google Play Games Plugin setup.", "OK");
                    }
                    EditorGUI.indentLevel--;
                }
            });

            DrawSection("Dependencies", () =>
            {
                var isAndroidPlatform = EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android;
                EditorGUILayout.HelpBox($"Current Platform: {EditorUserBuildSettings.activeBuildTarget}",
                    isAndroidPlatform ? MessageType.Info : MessageType.Warning);

                if (GUILayout.Button("Resolve Android Dependencies"))
                {
                    ResolveAndroidDependencies();
                }

                if (!isAndroidPlatform && GUILayout.Button("Switch to Android Platform"))
                {
                    EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
                }
            });
        }

        private void DrawiOSPlatformTab()
        {
            DrawSection("Apple Game Center Settings", () =>
            {
                EditorGUILayout.PropertyField(serializedSettings.FindProperty("enableAppleGameCenter"),
                    new GUIContent("Enable Apple Game Center", "Enable Apple Game Center for iOS"));
                if (wortalSettings.enableAppleGameCenter)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(serializedSettings.FindProperty("appleGameCenterBundleId"),
                        new GUIContent("Bundle ID", "Apple Game Center bundle identifier"));
                    if (string.IsNullOrEmpty(wortalSettings.appleGameCenterBundleId))
                    {
                        EditorGUILayout.HelpBox("Bundle ID should match your iOS app bundle identifier", MessageType.Info);
                    }
                    EditorGUI.indentLevel--;
                }
            });

            DrawSection("Dependencies", () =>
            {
                var isiOSPlatform = EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS;
                EditorGUILayout.HelpBox($"Current Platform: {EditorUserBuildSettings.activeBuildTarget}",
                    isiOSPlatform ? MessageType.Info : MessageType.Warning);

                if (GUILayout.Button("Resolve iOS Dependencies (Install Cocoapods)"))
                {
                    ResolveIOSDependencies();
                }

                if (!isiOSPlatform && GUILayout.Button("Switch to iOS Platform"))
                {
                    EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
                }
            });
        }

        private void CreateWortalSettings()
        {
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }
            var settings = ScriptableObject.CreateInstance<WortalSettings>();
            AssetDatabase.CreateAsset(settings, "Assets/Resources/WortalSettings.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            InitializeSettings();
            Debug.Log("WortalSettings created at Assets/Resources/WortalSettings.asset");
        }

        private void ImportGooglePlayXml(string xml)
        {
            try
            {
                var doc = XDocument.Parse(xml);
                foreach (var element in doc.Descendants("string"))
                {
                    var name = (string)element.Attribute("name");
                    var value = element.Value;

                    if (name == "app_id")
                    {
                        wortalSettings.googlePlayGamesAppId = value;
                        continue;
                    }

                    if (name.StartsWith("achievement_"))
                    {
                        var mapping = wortalSettings.achievements.FirstOrDefault(a => a.achievementId == name)
                                      ?? new AchievementMapping { achievementId = name };
                        mapping.googlePlayGamesId = value;
                        var displayNameKey = name.Replace("achievement_", "");
                        mapping.displayName = ConvertToPascalCase(displayNameKey);
                        if (!wortalSettings.achievements.Contains(mapping))
                            wortalSettings.achievements.Add(mapping);
                    }
                    else if (name.StartsWith("leaderboard_"))
                    {
                        var mapping = wortalSettings.leaderboards.FirstOrDefault(l => l.leaderboardId == name)
                                      ?? new LeaderboardMapping { leaderboardId = name };
                        mapping.googlePlayGamesId = value;
                        var displayNameKey = name.Replace("leaderboard_", "");
                        mapping.displayName = ConvertToPascalCase(displayNameKey);
                        if (!wortalSettings.leaderboards.Contains(mapping))
                            wortalSettings.leaderboards.Add(mapping);
                    }
                }
                wortalSettings.RefreshMappings();
                EditorUtility.SetDirty(wortalSettings);
                AssetDatabase.SaveAssets();
                Debug.Log("Successfully imported from Google Play XML.");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to import Google Play XML: {e.Message}");
            }
        }

        private string ConvertToPascalCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            var parts = input.Split('_');
            var result = new StringBuilder();
            foreach (var part in parts)
            {
                if (!string.IsNullOrEmpty(part))
                {
                    result.Append(char.ToUpper(part[0], CultureInfo.InvariantCulture));
                    if (part.Length > 1)
                        result.Append(part.Substring(1).ToLower(CultureInfo.InvariantCulture));
                }
            }
            return result.ToString();
        }

        private void SyncWortalToGPGSetup()
        {
            try
            {
                GenerateGPGSIdsFile();
                var gpgSettingsType = Type.GetType("GooglePlayGames.Editor.GPGSProjectSettings, GooglePlayGames.Editor");
                if (gpgSettingsType == null)
                {
                    gpgSettingsType = Type.GetType("GooglePlayGames.Editor.GPGSProjectSettings, Assembly-CSharp-Editor");
                }

                if (gpgSettingsType != null)
                {
                    var instanceProperty = gpgSettingsType.GetProperty("Instance", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    if (instanceProperty != null)
                    {
                        var gpgSettings = instanceProperty.GetValue(null);
                        var setMethod = gpgSettingsType.GetMethod("Set", new[] { typeof(string), typeof(string) });
                        if (setMethod != null)
                        {
                            setMethod.Invoke(gpgSettings, new object[] { "android.app_id", wortalSettings.googlePlayGamesAppId });
                            setMethod.Invoke(gpgSettings, new object[] { "android.client_id", wortalSettings.googlePlayGamesClientId });
                            var saveMethod = gpgSettingsType.GetMethod("Save");
                            saveMethod?.Invoke(gpgSettings, null);
                        }
                    }
                }
                AssetDatabase.Refresh();
                Debug.Log("✅ Wortal settings synced to Google Play Games Plugin successfully!");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to sync Wortal settings to Google Play Games Plugin: {e.Message}\nStack trace: {e.StackTrace}");
            }
        }

        private void GenerateGPGSIdsFile()
        {
            var directoryPath = "Assets/GooglePlayGames/Generated";
            var filePath = Path.Combine(directoryPath, "GPGSIds.cs");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var sb = new StringBuilder();
            sb.AppendLine("// This file was generated by Wortal SDK");
            sb.AppendLine();
            sb.AppendLine("public static class GPGSIds");
            sb.AppendLine("{");
            if (!string.IsNullOrEmpty(wortalSettings.googlePlayGamesAppId))
            {
                sb.AppendLine($"    public const string app_id = \"{wortalSettings.googlePlayGamesAppId}\";");
            }
            foreach (var achievement in wortalSettings.achievements)
            {
                if (!string.IsNullOrEmpty(achievement.googlePlayGamesId))
                {
                    var constantName = "achievement_" + achievement.displayName.Replace(" ", "_").ToLower();
                    sb.AppendLine($"    public const string {constantName} = \"{achievement.googlePlayGamesId}\";");
                }
            }
            foreach (var leaderboard in wortalSettings.leaderboards)
            {
                if (!string.IsNullOrEmpty(leaderboard.googlePlayGamesId))
                {
                    var constantName = "leaderboard_" + leaderboard.displayName.Replace(" ", "_").ToLower();
                    sb.AppendLine($"    public const string {constantName} = \"{leaderboard.googlePlayGamesId}\";");
                }
            }
            sb.AppendLine("}");
            File.WriteAllText(filePath, sb.ToString());
            AssetDatabase.ImportAsset(filePath);
        }

        private void DrawAchievementMappings()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("🏆 Achievements", EditorStyles.boldLabel);
            if (GUILayout.Button("Add Achievement", GUILayout.Width(120)))
            {
                wortalSettings.achievements.Add(new AchievementMapping
                {
                    achievementId = $"achievement_{wortalSettings.achievements.Count + 1}",
                    displayName = "New Achievement"
                });
                EditorUtility.SetDirty(wortalSettings);
            }
            EditorGUILayout.EndHorizontal();

            if (wortalSettings.achievements.Count == 0)
            {
                EditorGUILayout.HelpBox("No achievements configured. Click 'Add Achievement' to get started.", MessageType.Info);
            }
            else
            {
                for (int i = 0; i < wortalSettings.achievements.Count; i++)
                {
                    DrawAchievementMapping(i);
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawAchievementMapping(int index)
        {
            var achievement = wortalSettings.achievements[index];
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.BeginHorizontal();
            achievement.achievementId = EditorGUILayout.TextField("Achievement ID", achievement.achievementId);
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("✕", GUILayout.Width(25), GUILayout.Height(18)))
            {
                if (EditorUtility.DisplayDialog("Delete Achievement", $"Are you sure you want to delete '{achievement.displayName}'?", "Delete", "Cancel"))
                {
                    wortalSettings.achievements.RemoveAt(index);
                    EditorUtility.SetDirty(wortalSettings);
                    return;
                }
            }
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();

            achievement.displayName = EditorGUILayout.TextField("Display Name", achievement.displayName);
            achievement.description = EditorGUILayout.TextField("Description", achievement.description);
            EditorGUILayout.Space(3);
            EditorGUILayout.LabelField("Platform-Specific IDs:", EditorStyles.miniBoldLabel);
            EditorGUI.indentLevel++;
            achievement.googlePlayGamesId = EditorGUILayout.TextField("Google Play Games ID", achievement.googlePlayGamesId);
            achievement.appleGameCenterId = EditorGUILayout.TextField("Apple Game Center ID", achievement.appleGameCenterId);
            achievement.webglId = EditorGUILayout.TextField("WebGL/Wortal ID", achievement.webglId);
            EditorGUI.indentLevel--;

            if (!achievement.IsValid())
            {
                EditorGUILayout.HelpBox("⚠️ Achievement needs an ID, display name, and at least one platform ID.", MessageType.Warning);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(2);
        }

        private void DrawLeaderboardMappings()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("🏁 Leaderboards", EditorStyles.boldLabel);
            if (GUILayout.Button("Add Leaderboard", GUILayout.Width(120)))
            {
                wortalSettings.leaderboards.Add(new LeaderboardMapping
                {
                    leaderboardId = $"leaderboard_{wortalSettings.leaderboards.Count + 1}",
                    displayName = "New Leaderboard"
                });
                EditorUtility.SetDirty(wortalSettings);
            }
            EditorGUILayout.EndHorizontal();
            if (wortalSettings.leaderboards.Count == 0)
            {
                EditorGUILayout.HelpBox("No leaderboards configured. Click 'Add Leaderboard' to get started.", MessageType.Info);
            }
            else
            {
                for (int i = 0; i < wortalSettings.leaderboards.Count; i++)
                {
                    DrawLeaderboardMapping(i);
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawLeaderboardMapping(int index)
        {
            var leaderboard = wortalSettings.leaderboards[index];
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.BeginHorizontal();
            leaderboard.leaderboardId = EditorGUILayout.TextField("Leaderboard ID", leaderboard.leaderboardId);
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("✕", GUILayout.Width(25), GUILayout.Height(18)))
            {
                if (EditorUtility.DisplayDialog("Delete Leaderboard", $"Are you sure you want to delete '{leaderboard.displayName}'?", "Delete", "Cancel"))
                {
                    wortalSettings.leaderboards.RemoveAt(index);
                    EditorUtility.SetDirty(wortalSettings);
                    return;
                }
            }
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();

            leaderboard.displayName = EditorGUILayout.TextField("Display Name", leaderboard.displayName);
            leaderboard.description = EditorGUILayout.TextField("Description", leaderboard.description);
            EditorGUILayout.Space(3);
            EditorGUILayout.LabelField("Platform-Specific IDs:", EditorStyles.miniBoldLabel);
            EditorGUI.indentLevel++;
            leaderboard.googlePlayGamesId = EditorGUILayout.TextField("Google Play Games ID", leaderboard.googlePlayGamesId);
            leaderboard.appleGameCenterId = EditorGUILayout.TextField("Apple Game Center ID", leaderboard.appleGameCenterId);
            leaderboard.webglId = EditorGUILayout.TextField("WebGL/Wortal ID", leaderboard.webglId);
            EditorGUI.indentLevel--;

            if (!leaderboard.IsValid())
            {
                EditorGUILayout.HelpBox("⚠️ Leaderboard needs an ID, display name, and at least one platform ID.", MessageType.Warning);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(2);
        }

        private void DrawWebGLLazyLoadSubTab()
        {
            if (lazyConfig == null || !lazyConfig.enableLazyLoad)
            {
                EditorGUILayout.HelpBox("Lazy Loading is not enabled. Enable it to configure.", MessageType.Info);
                return;
            }

            if (lazyLoadSystem != null)
            {
                lazyLoadSystem.OnGUI();
            }
            else
            {
                EditorGUILayout.HelpBox("Lazy Load system not initialized.", MessageType.Warning);
            }
        }

        private void DrawWebGLAssetStripSubTab()
        {
            EditorGUILayout.HelpBox("Analyze which assets might be stripped from builds to optimize size.", MessageType.Info);
            if (unusedAssetScanner != null)
            {
                unusedAssetScanner.OnGUI();
            }
            else
            {
                EditorGUILayout.HelpBox("Asset scanner not initialized.", MessageType.Warning);
            }
        }

        private void CheckAndInstallDependencies()
        {
            if (isInstalling) return;
            Debug.Log("Wortal SDK: Checking dependencies...");
            listRequest = Client.List();
            EditorApplication.update += CheckListProgress;
        }

        private void CheckListProgress()
        {
            if (listRequest == null || !listRequest.IsCompleted) return;
            EditorApplication.update -= CheckListProgress;
            if (listRequest.Status == StatusCode.Success)
            {
                bool hasEDM4U = listRequest.Result.Any(p => p.name == EDM4U_PACKAGE_NAME);
                if (!hasEDM4U)
                {
                    InstallEDM4U();
                }
                else
                {
                    Debug.Log("Wortal SDK: All dependencies installed! ✓");
                    RefreshDependencies();
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
            if (addRequest == null || !addRequest.IsCompleted) return;
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

        private void InstallWebGLTemplate()
        {
            try
            {
                string destinationFolder = Path.GetFullPath(TEMPLATE_PATH);
                string sourceFolder = Path.GetFullPath(TEMPLATE_SOURCE);
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }
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
            PlayerSettings.SetIl2CppCodeGeneration(NamedBuildTarget.WebGL, Il2CppCodeGeneration.OptimizeSize);
#else
            EditorUserBuildSettings.il2CppCodeGeneration = Il2CppCodeGeneration.OptimizeSize;
#endif
            Debug.Log("Wortal SDK: WebGL settings applied!");
        }

        private void RefreshDependencies()
        {
            try
            {
                var method = Type.GetType("Google.JarResolver.PlayServicesResolver, Google.JarResolver").GetMethod("MenuResolve", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                method?.Invoke(null, null);
                Debug.Log("Refreshed Android dependencies.");
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Could not automatically refresh Android dependencies: {e.Message}");
            }
        }

        private void ResolveAndroidDependencies()
        {
            try
            {
                EditorApplication.ExecuteMenuItem("Assets/External Dependency Manager/Android Resolver/Resolve");
            }
            catch
            {
                EditorUtility.DisplayDialog("Manual Resolution Required", "Please resolve manually via:\nAssets > External Dependency Manager > Android Resolver > Resolve", "OK");
            }
        }

        private void ResolveIOSDependencies()
        {
            try
            {
                EditorApplication.ExecuteMenuItem("Assets/External Dependency Manager/iOS Resolver/Install Cocoapods");
            }
            catch
            {
                EditorUtility.DisplayDialog("Manual Resolution Required", "Please resolve manually via:\nAssets > External Dependency Manager > iOS Resolver > Install Cocoapods", "OK");
            }
        }

        private void DrawDependencyStatus()
        {
            EditorGUILayout.Space();
            var status = WortalDependencyChecker.GetDependencyStatus();

            // Google Play Games Status
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Google Play Games:", GUILayout.Width(150));
            var gpgColor = status.HasGooglePlayGames ? Color.green : Color.red;
            var gpgText = status.HasGooglePlayGames ? "✓ Available" : "✗ Missing";
            var originalColor = GUI.contentColor;
            GUI.contentColor = gpgColor;
            EditorGUILayout.LabelField(gpgText, EditorStyles.boldLabel);
            GUI.contentColor = originalColor;
            EditorGUILayout.EndHorizontal();

            // Apple Game Center Status
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Apple Game Center:", GUILayout.Width(150));
            var agcColor = status.HasAppleGameKit ? Color.green : Color.red;
            var agcText = status.HasAppleGameKit ? "✓ Available" : "✗ Missing";
            GUI.contentColor = agcColor;
            EditorGUILayout.LabelField(agcText, EditorStyles.boldLabel);
            GUI.contentColor = originalColor;
            EditorGUILayout.EndHorizontal();

            if (!status.HasGooglePlayGames || !status.HasAppleGameKit)
            {
                EditorGUILayout.HelpBox("Some platform dependencies are missing. Use the 'Install Dependencies' button to install the External Dependency Manager, then install specific platform packages (like Google Play Games) from the Package Manager.", MessageType.Warning);
            }
        }
    }
}
