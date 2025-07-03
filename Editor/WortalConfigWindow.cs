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

namespace DigitalWill.WortalEditor
{
    public class WortalConfigWindow : EditorWindow
    {
        private enum ConfigTab { General, Settings, WebGL, Android, iOS }
        private enum WebGLSubTab { Settings, LazyLoad, AssetStrip }

        private ConfigTab currentTab = ConfigTab.General;
        private WebGLSubTab currentWebGLSubTab = WebGLSubTab.Settings;

        // Used for Google Play Games XML input in settings tab
        private string xmlInputText = "";

        // General state
        private static AddRequest addRequest;
        private static ListRequest listRequest;
        private static bool isInstalling = false;

        // Settings state
        private WortalSettings wortalSettings;
        private SerializedObject serializedSettings;
        private bool settingsExpanded = true;

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
        private GUIStyle tabButtonStyle;
        private GUIStyle subTabButtonStyle;
        private GUIStyle sectionHeaderStyle;
        private GUIStyle warningBoxStyle;
        private GUIStyle successBoxStyle;
        private Color activeTabColor = new Color(0.4f, 0.6f, 1f, 0.3f);
        private Color activeSubTabColor = new Color(0.6f, 0.8f, 1f, 0.4f);

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
            // InitializeStyles(); // Moved to OnGUI to avoid EditorStyles null reference
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
                    lazyConfig = ScriptableObject.CreateInstance<LazyLoadConfig>();
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
                lazyConfig.assetPriorityGroups = new System.Collections.Generic.List<DigitalWill.WortalSDK.AssetPriorityGroup>();

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
                    group.assetsByType = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>();

                foreach (var key in group.assetsByType.Keys.ToList())
                {
                    if (group.assetsByType[key] == null)
                        group.assetsByType[key] = new System.Collections.Generic.List<string>();
                    else
                        group.assetsByType[key] = group.assetsByType[key].Where(p => !string.IsNullOrEmpty(p)).ToList();
                }
            }
        }

        private void OnGUI()
        {
            if (sectionHeaderStyle == null || warningBoxStyle == null || successBoxStyle == null)
            {
                InitializeStyles();
            }

            DrawHeader();
            DrawTabs();

            EditorGUILayout.Space(10);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            switch (currentTab)
            {
                case ConfigTab.General: DrawGeneralTab(); break;
                case ConfigTab.Settings: DrawSettingsTab(); break;
                case ConfigTab.WebGL: DrawWebGLTab(); break;
                case ConfigTab.Android: DrawAndroidTab(); break;
                case ConfigTab.iOS: DrawIOSTab(); break;
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawHeader()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            var titleStyle = new GUIStyle(EditorStyles.boldLabel) { fontSize = 16 };
            EditorGUILayout.LabelField("Wortal SDK Configuration", titleStyle);
            EditorGUILayout.LabelField("Version 6.2.2 - Unified SDK for WebGL, Android, and iOS", EditorStyles.miniLabel);

            // Quick status indicator
            var status = WortalDependencyChecker.GetDependencyStatus();
            var statusText = "Status: ";
            var statusColor = Color.green;

            if (!status.AndroidReady || !status.iOSReady)
            {
                statusText += "⚠️ Configuration needed";
                statusColor = Color.yellow;
            }
            else
            {
                statusText += "✅ Ready";
            }

            var originalColor = GUI.color;
            GUI.color = statusColor;
            EditorGUILayout.LabelField(statusText, EditorStyles.miniLabel);
            GUI.color = originalColor;

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
            DrawTab("Settings", ConfigTab.Settings);
            DrawTab("WebGL", ConfigTab.WebGL);
            DrawTab("Android", ConfigTab.Android);
            DrawTab("iOS", ConfigTab.iOS);

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

        private void DrawSettingsTab()
        {
            if (wortalSettings == null || serializedSettings == null)
            {
                EditorGUILayout.HelpBox("WortalSettings not found. Creating default settings...", MessageType.Warning);
                if (GUILayout.Button("Create Settings"))
                {
                    CreateWortalSettings();
                }
                return;
            }

            EditorGUILayout.LabelField("Wortal SDK Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            serializedSettings.Update();

            // General Settings Section
            DrawSettingsSection("General Settings", () =>
            {
                EditorGUILayout.PropertyField(serializedSettings.FindProperty("enableDebugLogging"),
                    new GUIContent("Enable Debug Logging", "Show detailed logs in console"));
                EditorGUILayout.PropertyField(serializedSettings.FindProperty("autoInitialize"),
                    new GUIContent("Auto Initialize", "Automatically initialize SDK on game start"));
            });

            // WebGL Settings Section
            DrawSettingsSection("WebGL Settings", () =>
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(serializedSettings.FindProperty("webglGameId"),
                    new GUIContent("WebGL Game ID", "Your Wortal game ID for WebGL platform"));
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.HelpBox("Game ID for WebGL will be automatically assigned when you upload your build to the Wortal Dashboard. No manual input required.", MessageType.Info);
            });

            // Google Play Games Settings Section
            DrawSettingsSection("Google Play Games Settings", () =>
            {
                EditorGUILayout.PropertyField(serializedSettings.FindProperty("enableGooglePlayGames"),
                    new GUIContent("Enable Google Play Games", "Enable Google Play Games services for Android"));

                if (wortalSettings.enableGooglePlayGames)
                {
                    EditorGUI.indentLevel++;

                    // XML Import Section
                    EditorGUILayout.LabelField("Paste Google Play Games XML Resource:");
                    xmlInputText = EditorGUILayout.TextArea(xmlInputText, GUILayout.Height(150));
                    if (GUILayout.Button("Import from XML"))
                    {
                        ImportGooglePlayXml(xmlInputText);
                        SyncWortalToGPGSetup(); // Auto-sync after import
                    }

                    EditorGUILayout.Space(5);

                    // Configuration Fields
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(serializedSettings.FindProperty("googlePlayGamesAppId"),
                        new GUIContent("App ID", "Google Play Games application ID"));

                    EditorGUILayout.PropertyField(serializedSettings.FindProperty("googlePlayGamesClientId"),
                        new GUIContent("Web App Client ID", "OAuth 2.0 Web Application Client ID from Google Cloud Console"));

                    // Auto-sync when values change
                    if (EditorGUI.EndChangeCheck())
                    {
                        serializedSettings.ApplyModifiedProperties();
                        SyncWortalToGPGSetup();
                    }

                    // Error Messages and Help
                    if (string.IsNullOrEmpty(wortalSettings.googlePlayGamesAppId))
                    {
                        EditorGUILayout.HelpBox("Google Play Games App ID is required when GPG is enabled", MessageType.Error);
                        if (GUILayout.Button("How to get App ID?"))
                        {
                            Application.OpenURL("https://developers.google.com/games/services/console/enabling");
                        }
                    }

                    if (string.IsNullOrEmpty(wortalSettings.googlePlayGamesClientId))
                    {
                        EditorGUILayout.HelpBox("Web App Client ID is required for OAuth 2.0 authentication", MessageType.Error);
                        if (GUILayout.Button("How to get Client ID?"))
                        {
                            Application.OpenURL("https://developers.google.com/games/services/console/enabling#step_2_generate_an_oauth_20_client_id");
                        }
                    }

                    EditorGUILayout.Space(5);

                    // Authentication Options
                    EditorGUILayout.PropertyField(serializedSettings.FindProperty("requestServerAuthCode"),
                        new GUIContent("Request Server Auth Code", "Request server authentication code"));
                    EditorGUILayout.PropertyField(serializedSettings.FindProperty("forceResolve"),
                        new GUIContent("Force Resolve", "Force resolve Google Play Games conflicts"));

                    EditorGUILayout.Space(10);

                    // Manual Sync Button
                    if (GUILayout.Button("🔄 Sync to Google Play Games Setup"))
                    {
                        SyncWortalToGPGSetup();
                        EditorUtility.DisplayDialog("Sync Complete", "Wortal settings have been synced to Google Play Games Plugin setup.", "OK");
                    }

                    EditorGUI.indentLevel--;
                }

                // Dependency Status
                var status = WortalDependencyChecker.GetDependencyStatus();
                if (wortalSettings.enableGooglePlayGames)
                {
                    if (status.HasGooglePlayGames)
                    {
                        EditorGUILayout.HelpBox("✅ Google Play Games SDK detected and configured", MessageType.Info);
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("⚠️ Google Play Games SDK not found. Install it from Package Manager.", MessageType.Warning);
                        if (GUILayout.Button("Open Package Manager"))
                        {
                            UnityEditor.PackageManager.UI.Window.Open("com.google.play.games");
                        }
                    }
                }
            });

            // Apple Game Center Settings Section
            DrawSettingsSection("Apple Game Center Settings", () =>
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

                // Show dependency status
                var status = WortalDependencyChecker.GetDependencyStatus();
                if (wortalSettings.enableAppleGameCenter)
                {
                    if (status.HasAppleGameKit)
                    {
                        EditorGUILayout.HelpBox("✅ Apple Game Center available", MessageType.Info);
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("⚠️ Apple Game Center not available in current environment", MessageType.Warning);
                    }
                }
            });

            // Cross-Platform Mappings Section
            DrawSettingsSection("Cross-Platform Mappings", () =>
            {
                EditorGUILayout.LabelField("Configure achievements and leaderboards for all platforms in one place:", EditorStyles.miniLabel);
                EditorGUILayout.Space(3);


                DrawAchievementMappings();


                EditorGUILayout.Space(5);


                DrawLeaderboardMappings();

            });

            // Debug Settings Section
            DrawSettingsSection("Debug Settings", () =>
            {
                EditorGUILayout.PropertyField(serializedSettings.FindProperty("forceGooglePlayGamesDetection"),
                    new GUIContent("Force GPG Detection", "Override Google Play Games detection for testing"));
                EditorGUILayout.PropertyField(serializedSettings.FindProperty("forceAppleGameCenterDetection"),
                    new GUIContent("Force Game Center Detection", "Override Apple Game Center detection for testing"));

                EditorGUILayout.Space(5);
                if (GUILayout.Button("Debug Dependency Detection"))
                {
                    WortalDependencyChecker.DebugDependencyDetection();
                }
            });

            // Validation Section
            DrawSettingsSection("Validation", () =>
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Validate Settings"))
                {
                    ValidateAllSettings();
                }
                if (GUILayout.Button("Reset to Defaults"))
                {
                    if (EditorUtility.DisplayDialog("Reset Settings",
                        "Are you sure you want to reset all settings to defaults?", "Reset", "Cancel"))
                    {
                        ResetToDefaults();
                    }
                }
                EditorGUILayout.EndHorizontal();
            });

            serializedSettings.ApplyModifiedProperties();

            // Auto-save when settings change
            if (GUI.changed)
            {
                EditorUtility.SetDirty(wortalSettings);
                AssetDatabase.SaveAssets();
            }
        }

        private void DrawSettingsSection(string title, System.Action content)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(title, sectionHeaderStyle);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(3);
            content();
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(5);
        }

        private void CreateWortalSettings()
        {
            // Create Resources folder if it doesn't exist
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }

            // Create WortalSettings asset
            var settings = ScriptableObject.CreateInstance<WortalSettings>();
            AssetDatabase.CreateAsset(settings, "Assets/Resources/WortalSettings.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // Reinitialize
            InitializeSettings();

            Debug.Log("WortalSettings created at Assets/Resources/WortalSettings.asset");
        }

        private void ValidateAllSettings()
        {
            if (wortalSettings == null) return;

            bool isValid = wortalSettings.ValidateAll();

            if (isValid)
            {
                EditorUtility.DisplayDialog("Validation Result", "✅ All settings are valid!", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("Validation Result",
                    "⚠️ Some settings need attention. Check the console for details.", "OK");
            }
        }

        private void ResetToDefaults()
        {
            if (wortalSettings == null) return;

            // Reset to default values
            wortalSettings.enableDebugLogging = true;
            wortalSettings.autoInitialize = true;
            wortalSettings.webglGameId = "";
            wortalSettings.enableGooglePlayGames = true;
            wortalSettings.googlePlayGamesAppId = "";
            wortalSettings.requestServerAuthCode = false;
            wortalSettings.forceResolve = true;
            wortalSettings.enableAppleGameCenter = true;
            wortalSettings.appleGameCenterBundleId = "";

            EditorUtility.SetDirty(wortalSettings);
            AssetDatabase.SaveAssets();

            // Refresh serialized object
            serializedSettings = new SerializedObject(wortalSettings);
        }

        // Auto-sync method
        private void SyncWortalToGPGSetup()
        {
            try
            {
                // First, generate the GPGSIds file
                GenerateGPGSIdsFile();

                // Try to configure Google Play Games Plugin settings
                var gpgSettingsType = System.Type.GetType("GooglePlayGames.Editor.GPGSProjectSettings, GooglePlayGames.Editor");
                if (gpgSettingsType == null)
                {
                    // Try alternative type name for newer versions
                    gpgSettingsType = System.Type.GetType("GooglePlayGames.Editor.GPGSProjectSettings, Assembly-CSharp-Editor");
                }

                if (gpgSettingsType != null)
                {
                    // Get GPG settings instance
                    var instanceProperty = gpgSettingsType.GetProperty("Instance",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

                    if (instanceProperty != null)
                    {
                        var gpgSettings = instanceProperty.GetValue(null);

                        // Set app ID and client ID using reflection
                        var setMethod = gpgSettingsType.GetMethod("Set", new[] { typeof(string), typeof(string) });
                        if (setMethod != null)
                        {
                            setMethod.Invoke(gpgSettings, new object[] { "android.app_id", wortalSettings.googlePlayGamesAppId });
                            setMethod.Invoke(gpgSettings, new object[] { "android.client_id", wortalSettings.googlePlayGamesClientId });

                            // Save settings
                            var saveMethod = gpgSettingsType.GetMethod("Save");
                            saveMethod?.Invoke(gpgSettings, null);
                        }
                    }
                }

                // Force refresh of assets
                AssetDatabase.Refresh();

                Debug.Log("✅ Wortal settings synced to Google Play Games Plugin successfully!");
                Debug.Log($"Generated GPGSIds.cs at: Assets/GooglePlayGames/Generated/GPGSIds.cs");

            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to sync Wortal settings to Google Play Games Plugin: {e.Message}");
                Debug.LogError($"Stack trace: {e.StackTrace}");
            }
        }

        private void GenerateGPGSIdsFile()
        {
            // Use the correct path that Google Play Games Plugin expects
            var directoryPath = "Assets/GooglePlayGames/Generated";
            var filePath = Path.Combine(directoryPath, "GPGSIds.cs");

            // Create directory if it doesn't exist
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("// <auto-generated>");
            sb.AppendLine("// This file was generated by Wortal SDK");
            sb.AppendLine("// </auto-generated>");
            sb.AppendLine();
            sb.AppendLine("public static class GPGSIds");
            sb.AppendLine("{");

            // Add app_id if available
            if (!string.IsNullOrEmpty(wortalSettings.googlePlayGamesAppId))
            {
                sb.AppendLine($"    public const string app_id = \"{wortalSettings.googlePlayGamesAppId}\";");
            }

            // Add achievements
            foreach (var achievement in wortalSettings.achievements)
            {
                if (!string.IsNullOrEmpty(achievement.googlePlayGamesId))
                {
                    var constantName = achievement.achievementId.Replace("achievement_", "").Replace("_", "");
                    sb.AppendLine($"    public const string {constantName} = \"{achievement.googlePlayGamesId}\";");
                }
            }

            // Add leaderboards
            foreach (var leaderboard in wortalSettings.leaderboards)
            {
                if (!string.IsNullOrEmpty(leaderboard.googlePlayGamesId))
                {
                    var constantName = leaderboard.leaderboardId.Replace("leaderboard_", "").Replace("_", "");
                    sb.AppendLine($"    public const string {constantName} = \"{leaderboard.googlePlayGamesId}\";");
                }
            }

            sb.AppendLine("}");

            File.WriteAllText(filePath, sb.ToString());

            // Force Unity to recognize the new file
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

            // Header with delete button
            EditorGUILayout.BeginHorizontal();
            achievement.achievementId = EditorGUILayout.TextField("Achievement ID", achievement.achievementId);

            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("✕", GUILayout.Width(25), GUILayout.Height(18)))
            {
                if (EditorUtility.DisplayDialog("Delete Achievement",
                    $"Are you sure you want to delete '{achievement.displayName}'?", "Delete", "Cancel"))
                {
                    wortalSettings.achievements.RemoveAt(index);
                    EditorUtility.SetDirty(wortalSettings);
                    return;
                }
            }
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();

            // Basic info
            achievement.displayName = EditorGUILayout.TextField("Display Name", achievement.displayName);
            achievement.description = EditorGUILayout.TextField("Description", achievement.description);

            EditorGUILayout.Space(3);
            EditorGUILayout.LabelField("Platform-Specific IDs:", EditorStyles.miniBoldLabel);

            EditorGUI.indentLevel++;
            achievement.googlePlayGamesId = EditorGUILayout.TextField("Google Play Games ID", achievement.googlePlayGamesId);
            achievement.appleGameCenterId = EditorGUILayout.TextField("Apple Game Center ID", achievement.appleGameCenterId);
            // achievement.steamId = EditorGUILayout.TextField("Steam ID (Future)", achievement.steamId);
            achievement.webglId = EditorGUILayout.TextField("WebGL/Wortal ID", achievement.webglId);
            EditorGUI.indentLevel--;

            // Validation
            if (!achievement.IsValid())
            {
                EditorGUILayout.HelpBox("⚠️ Achievement needs at least an ID, display name, and one platform ID", MessageType.Warning);
            }
            else if (!achievement.IsConfiguredForCurrentPlatform())
            {
                EditorGUILayout.HelpBox($"⚠️ No ID configured for current platform ({EditorUserBuildSettings.activeBuildTarget})", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.HelpBox($"✅ Current platform ID: {achievement.GetPlatformId()}", MessageType.Info);
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

            // Header with delete button
            EditorGUILayout.BeginHorizontal();
            leaderboard.leaderboardId = EditorGUILayout.TextField("Leaderboard ID", leaderboard.leaderboardId);

            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("✕", GUILayout.Width(25), GUILayout.Height(18)))
            {
                if (EditorUtility.DisplayDialog("Delete Leaderboard",
                    $"Are you sure you want to delete '{leaderboard.displayName}'?", "Delete", "Cancel"))
                {
                    wortalSettings.leaderboards.RemoveAt(index);
                    EditorUtility.SetDirty(wortalSettings);
                    return;
                }
            }
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();

            // Basic info
            leaderboard.displayName = EditorGUILayout.TextField("Display Name", leaderboard.displayName);
            leaderboard.description = EditorGUILayout.TextField("Description", leaderboard.description);

            EditorGUILayout.Space(3);
            EditorGUILayout.LabelField("Platform-Specific IDs:", EditorStyles.miniBoldLabel);

            EditorGUI.indentLevel++;
            leaderboard.googlePlayGamesId = EditorGUILayout.TextField("Google Play Games ID", leaderboard.googlePlayGamesId);
            leaderboard.appleGameCenterId = EditorGUILayout.TextField("Apple Game Center ID", leaderboard.appleGameCenterId);
            // leaderboard.steamId = EditorGUILayout.TextField("Steam ID (Future)", leaderboard.steamId);
            leaderboard.webglId = EditorGUILayout.TextField("WebGL/Wortal ID", leaderboard.webglId);
            EditorGUI.indentLevel--;

            // Validation
            if (!leaderboard.IsValid())
            {
                EditorGUILayout.HelpBox("⚠️ Leaderboard needs at least an ID, display name, and one platform ID", MessageType.Warning);
            }
            else if (!leaderboard.IsConfiguredForCurrentPlatform())
            {
                EditorGUILayout.HelpBox($"⚠️ No ID configured for current platform ({EditorUserBuildSettings.activeBuildTarget})", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.HelpBox($"✅ Current platform ID: {leaderboard.GetPlatformId()}", MessageType.Info);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(2);
        }


        private void DrawWebGLSubTabs()
        {
            if (subTabButtonStyle == null)
            {
                subTabButtonStyle = new GUIStyle(EditorStyles.toolbarButton);
                subTabButtonStyle.fixedHeight = 22;
                subTabButtonStyle.fontSize = 11;
            }

            EditorGUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();

            DrawSubTab("Settings", WebGLSubTab.Settings);

            if (lazyConfig != null && lazyConfig.enableLazyLoad)
            {
                DrawSubTab("Lazy Load", WebGLSubTab.LazyLoad);
            }

            DrawSubTab("Asset Strip", WebGLSubTab.AssetStrip);

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(5);
        }

        private void DrawSubTab(string label, WebGLSubTab subTab)
        {
            var isActive = currentWebGLSubTab == subTab;
            var originalColor = GUI.backgroundColor;

            if (isActive) GUI.backgroundColor = activeSubTabColor;

            if (GUILayout.Button(label, subTabButtonStyle))
                currentWebGLSubTab = subTab;

            GUI.backgroundColor = originalColor;
        }

        private void DrawGeneralTab()
        {
            EditorGUILayout.LabelField("General Overview", EditorStyles.boldLabel);

            // Quick Setup Guide
            DrawSection("Quick Setup Guide", () =>
            {
                EditorGUILayout.LabelField("1. Configure your settings in the 'Settings' tab", EditorStyles.miniLabel);
                EditorGUILayout.LabelField("2. Install platform dependencies below", EditorStyles.miniLabel);
                EditorGUILayout.LabelField("3. Configure platform-specific settings in respective tabs", EditorStyles.miniLabel);
                EditorGUILayout.LabelField("4. Build and deploy!", EditorStyles.miniLabel);
            });

            // Dependencies section
            DrawSection("Dependencies", () =>
            {
                DrawDependencyStatus();
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

            // Current Configuration Status
            DrawSection("Configuration Status", () =>
            {
                if (wortalSettings != null)
                {
                    var isValid = wortalSettings.ValidateAll();
                    var statusText = isValid ? "✅ Configuration Valid" : "⚠️ Configuration Issues";
                    var messageType = isValid ? MessageType.Info : MessageType.Warning;

                    EditorGUILayout.HelpBox(statusText, messageType);

                    if (!isValid && GUILayout.Button("Go to Settings"))
                    {
                        currentTab = ConfigTab.Settings;
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("⚠️ WortalSettings not found", MessageType.Warning);
                    if (GUILayout.Button("Create Settings"))
                    {
                        CreateWortalSettings();
                    }
                }
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

            DrawWebGLSubTabs();

            switch (currentWebGLSubTab)
            {
                case WebGLSubTab.Settings:
                    DrawWebGLSettingsSubTab();
                    break;
                case WebGLSubTab.LazyLoad:
                    DrawWebGLLazyLoadSubTab();
                    break;
                case WebGLSubTab.AssetStrip:
                    DrawWebGLAssetStripSubTab();
                    break;
            }
        }

        private void DrawWebGLSettingsSubTab()
        {
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

            DrawSection("General Optimizations", () =>
            {
                if (lazyConfig == null)
                {
                    EditorGUILayout.HelpBox("Optimization config not loaded. Try reopening the window.", MessageType.Warning);
                    return;
                }

                EditorGUI.BeginChangeCheck();
                bool newEnableLazy = EditorGUILayout.Toggle(
                    new GUIContent("Enable Lazy Loading", "Toggle this ON to enable LazyLoadManager, which loads assets dynamically based on scene, type, and priority."),
                    lazyConfig.enableLazyLoad);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(lazyConfig, "Toggled Lazy Load Enabled");
                    lazyConfig.enableLazyLoad = newEnableLazy;
                    EditorUtility.SetDirty(lazyConfig);

                    if (newEnableLazy)
                        currentWebGLSubTab = WebGLSubTab.LazyLoad;
                }

                EditorGUILayout.HelpBox("When enabled, Lazy Load will load only essential assets at startup. The rest are loaded on demand to reduce memory usage and improve load times.", MessageType.Info);

                EditorGUILayout.HelpBox(
                    "To strip unused engine code in WebGL builds:\n" +
                    "1. Open File → Build Settings → WebGL\n" +
                    "2. Click 'Player Settings' → 'Publishing Settings'\n" +
                    "3. Enable 'Strip Engine Code'\n" +
                    "Use the 'Asset Strip' tab to preview what will be stripped.",
                    MessageType.Info);
            });
        }

        private void DrawWebGLLazyLoadSubTab()
        {
            if (lazyConfig == null || !lazyConfig.enableLazyLoad)
            {
                EditorGUILayout.HelpBox("Lazy Loading is not enabled. Enable it in the Settings tab first.", MessageType.Info);
                if (GUILayout.Button("Go to Settings"))
                {
                    currentWebGLSubTab = WebGLSubTab.Settings;
                }
                return;
            }

            EditorGUILayout.LabelField("Lazy Load Configuration", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            if (lazyLoadSystem != null)
            {
                lazyLoadSystem.OnGUI();
            }
            else
            {
                EditorGUILayout.HelpBox("Lazy Load system not initialized. Try reopening the window.", MessageType.Warning);
            }
        }

        private void DrawWebGLAssetStripSubTab()
        {
            EditorGUILayout.LabelField("Asset Strip Preview", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            EditorGUILayout.HelpBox("Analyze which assets might be stripped from builds to optimize size.", MessageType.Info);

            if (unusedAssetScanner != null)
            {
                unusedAssetScanner.OnGUI();
            }
            else
            {
                EditorGUILayout.HelpBox("Asset scanner not initialized. Try reopening the window.", MessageType.Warning);
            }
        }

        private void DrawAndroidTab()
        {
            EditorGUILayout.LabelField("Android Configuration", EditorStyles.boldLabel);

            // Settings Summary
            DrawSection("Current Settings", () =>
            {
                if (wortalSettings != null)
                {
                    EditorGUILayout.LabelField($"Google Play Games: {(wortalSettings.enableGooglePlayGames ? "Enabled" : "Disabled")}");
                    if (wortalSettings.enableGooglePlayGames)
                    {
                        EditorGUILayout.LabelField($"App ID: {(string.IsNullOrEmpty(wortalSettings.googlePlayGamesAppId) ? "Not Set ⚠️" : wortalSettings.googlePlayGamesAppId)}");
                    }

                    if (GUILayout.Button("Edit Settings"))
                    {
                        currentTab = ConfigTab.Settings;
                    }
                }
            });

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
        }

        private void DrawIOSTab()
        {
            EditorGUILayout.LabelField("iOS Configuration", EditorStyles.boldLabel);

            // Settings Summary
            DrawSection("Current Settings", () =>
            {
                if (wortalSettings != null)
                {
                    EditorGUILayout.LabelField($"Apple Game Center: {(wortalSettings.enableAppleGameCenter ? "Enabled" : "Disabled")}");
                    if (wortalSettings.enableAppleGameCenter)
                    {
                        EditorGUILayout.LabelField($"Bundle ID: {(string.IsNullOrEmpty(wortalSettings.appleGameCenterBundleId) ? "Not Set" : wortalSettings.appleGameCenterBundleId)}");
                    }

                    if (GUILayout.Button("Edit Settings"))
                    {
                        currentTab = ConfigTab.Settings;
                    }
                }
            });

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

        // Platform dependency methods
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

        private void DrawDependencyStatus()
        {
            EditorGUILayout.Space();
            var status = WortalDependencyChecker.GetDependencyStatus();

            // Google Play Games
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Google Play Games:");
            var gpgColor = status.HasGooglePlayGames ? Color.green : Color.red;
            var gpgText = status.HasGooglePlayGames ? "✓ Available" : "✗ Missing";

            var originalColor = GUI.color;
            GUI.color = gpgColor;
            EditorGUILayout.LabelField(gpgText);
            GUI.color = originalColor;
            EditorGUILayout.EndHorizontal();

            // Apple Game Center
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Apple Game Center:");
            var agcColor = status.HasAppleGameKit ? Color.green : Color.red;
            var agcText = status.HasAppleGameKit ? "✓ Available" : "✗ Missing";

            GUI.color = agcColor;
            EditorGUILayout.LabelField(agcText);
            GUI.color = originalColor;
            EditorGUILayout.EndHorizontal();

            if (!status.HasGooglePlayGames)
            {
                EditorGUILayout.HelpBox("Install Google Play Games from Package Manager for Android features", MessageType.Warning);
            }
        }

        private void ImportGooglePlayXml(string xml)
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
        }

        // Helper for gpg xml importer
        string ConvertToPascalCase(string input)
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
    }
}
