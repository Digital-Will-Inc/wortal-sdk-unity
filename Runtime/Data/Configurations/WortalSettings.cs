using System.Collections.Generic;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// ScriptableObject for Wortal SDK configuration
    /// </summary>
    [CreateAssetMenu(fileName = "WortalSettings", menuName = "Wortal/Settings", order = 1)]
    public class WortalSettings : ScriptableObject
    {
        private static WortalSettings _instance;

        public static WortalSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<WortalSettings>("WortalSettings");
                    if (_instance == null)
                    {
                        Debug.LogWarning("[Wortal] WortalSettings not found in Resources. Creating default settings.");
                        _instance = CreateInstance<WortalSettings>();
                    }
                }
                return _instance;
            }
        }

        [Header("General Settings")]
        [Tooltip("Enable debug logging")]
        public bool enableDebugLogging = true;

        [Tooltip("Auto-initialize on game start")]
        public bool autoInitialize = true;

        [Header("WebGL Settings")]
        [Tooltip("WebGL game ID")]
        public string webglGameId = "";

        [Header("Google Play Games Settings")]
        [Tooltip("Enable Google Play Games Services")]
        public bool enableGooglePlayGames = true;

        [Tooltip("Google Play Games application ID")]
        public string googlePlayGamesAppId = "";

        [Tooltip("Google Play Games Web App Client ID (OAuth 2.0)")]
        public string googlePlayGamesClientId = "";

        [Tooltip("Request server auth code")]
        public bool requestServerAuthCode = false;

        [Tooltip("Force resolve")]
        public bool forceResolve = true;

        [Header("Apple Game Center Settings")]
        [Tooltip("Enable Apple Game Center")]
        public bool enableAppleGameCenter = true;

        [Tooltip("Apple Game Center bundle ID")]
        public string appleGameCenterBundleId = "";

        [Header("Cross-Platform Mappings")]
        [Tooltip("Achievement configurations for all platforms")]
        public List<AchievementMapping> achievements = new List<AchievementMapping>();

        [Tooltip("Leaderboard configurations for all platforms")]
        public List<LeaderboardMapping> leaderboards = new List<LeaderboardMapping>();

        [Header("Debug Settings")]
        [Tooltip("Force Google Play Games detection (for testing)")]
        public bool forceGooglePlayGamesDetection = false;

        [Tooltip("Force Apple Game Center detection (for testing)")]
        public bool forceAppleGameCenterDetection = false;

        // Runtime dictionaries for fast lookup
        private Dictionary<string, AchievementMapping> _achievementDict;
        private Dictionary<string, LeaderboardMapping> _leaderboardDict;

        /// <summary>
        /// Get achievement mapping by internal ID
        /// </summary>
        public AchievementMapping GetAchievement(string achievementId)
        {
            if (_achievementDict == null)
                _achievementDict = PlatformMappingHelper.CreateAchievementDictionary(achievements);

            _achievementDict.TryGetValue(achievementId, out var achievement);
            return achievement;
        }

        /// <summary>
        /// Get leaderboard mapping by internal ID
        /// </summary>
        public LeaderboardMapping GetLeaderboard(string leaderboardId)
        {
            if (_leaderboardDict == null)
                _leaderboardDict = PlatformMappingHelper.CreateLeaderboardDictionary(leaderboards);

            _leaderboardDict.TryGetValue(leaderboardId, out var leaderboard);
            return leaderboard;
        }

        /// <summary>
        /// Get platform-specific achievement ID
        /// </summary>
        public string GetPlatformAchievementId(string achievementId)
        {
            var achievement = GetAchievement(achievementId);
            return achievement?.GetPlatformId() ?? achievementId;
        }

        /// <summary>
        /// Get platform-specific leaderboard ID
        /// </summary>
        public string GetPlatformLeaderboardId(string leaderboardId)
        {
            var leaderboard = GetLeaderboard(leaderboardId);
            return leaderboard?.GetPlatformId() ?? leaderboardId;
        }

        /// <summary>
        /// Refresh runtime dictionaries (call when achievements/leaderboards are modified)
        /// </summary>
        public void RefreshMappings()
        {
            _achievementDict = PlatformMappingHelper.CreateAchievementDictionary(achievements);
            _leaderboardDict = PlatformMappingHelper.CreateLeaderboardDictionary(leaderboards);
        }

        /// <summary>
        /// Validates the settings configuration
        /// </summary>
        public bool ValidateSettings()
        {
            bool isValid = true;

#if UNITY_ANDROID
            if (enableGooglePlayGames && string.IsNullOrEmpty(googlePlayGamesAppId))
            {
                Debug.LogError("[Wortal] Google Play Games is enabled but App ID is not set");
                isValid = false;
            }
#elif UNITY_IOS
            if (enableAppleGameCenter && string.IsNullOrEmpty(appleGameCenterBundleId))
            {
                Debug.LogError("[Wortal] Apple Game Center is enabled but Bundle ID is not set");
                isValid = false;
            }
#endif

            // Validate achievements
            foreach (var achievement in achievements)
            {
                if (!achievement.IsValid())
                {
                    Debug.LogWarning($"[Wortal] Achievement '{achievement.achievementId}' has invalid configuration");
                    isValid = false;
                }
            }


            // Validate leaderboards
            foreach (var leaderboard in leaderboards)
            {
                if (!leaderboard.IsValid())
                {
                    Debug.LogWarning($"[Wortal] Leaderboard '{leaderboard.leaderboardId}' has invalid configuration");
                    isValid = false;
                }
            }

            return isValid;
        }

        /// <summary>
        /// Check and validate platform dependencies
        /// </summary>
        public bool ValidateDependencies()
        {
            bool isValid = true;

            // Check platform-specific dependencies
            var dependencyStatus = WortalDependencyChecker.GetDependencyStatus();

#if UNITY_ANDROID
            if (enableGooglePlayGames && !dependencyStatus.AndroidReady)
            {
                Debug.LogError("[Wortal] Google Play Games is enabled but SDK is not available");
                isValid = false;
            }
#elif UNITY_IOS
            if (enableAppleGameCenter && !dependencyStatus.iOSReady)
            {
                Debug.LogError("[Wortal] Apple Game Center is enabled but not available");
                isValid = false;
            }
#endif

            return isValid;
        }

        /// <summary>
        /// Get comprehensive validation including dependencies
        /// </summary>
        public bool ValidateAll()
        {
            return ValidateSettings() && ValidateDependencies();
        }

        private void OnValidate()
        {
            // Refresh mappings when settings are modified in editor
            RefreshMappings();
        }
    }
}
