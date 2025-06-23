using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Serializable achievement configuration for cross-platform support
    /// </summary>
    [System.Serializable]
    public class AchievementMapping
    {
        [Tooltip("Internal achievement identifier used in your game code")]
        public string achievementId = "";

        [Tooltip("Display name for this achievement")]
        public string displayName = "";

        [Tooltip("Description of the achievement")]
        public string description = "";

        [Header("Platform-Specific IDs")]
        [Tooltip("Google Play Games achievement ID")]
        public string googlePlayGamesId = "";

        [Tooltip("Apple Game Center achievement ID")]
        public string appleGameCenterId = "";

        // [Tooltip("Steam achievement ID (for future use)")]
        // public string steamId = "";

        [Tooltip("WebGL/Wortal achievement ID (for future use)")]
        public string webglId = "";

        /// <summary>
        /// Get platform-specific ID for current platform
        /// </summary>
        public string GetPlatformId()
        {
#if UNITY_ANDROID
            return googlePlayGamesId;
#elif UNITY_IOS
            return appleGameCenterId;
#elif UNITY_WEBGL
            return webglId;
#else
            return achievementId; // Fallback to internal ID
#endif
        }

        /// <summary>
        /// Check if this achievement is configured for the current platform
        /// </summary>
        public bool IsConfiguredForCurrentPlatform()
        {
            var platformId = GetPlatformId();
            return !string.IsNullOrEmpty(platformId);
        }

        /// <summary>
        /// Validate this achievement mapping
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(achievementId) || string.IsNullOrEmpty(displayName))
                return false;

            // At least one platform ID should be configured
            return !string.IsNullOrEmpty(googlePlayGamesId) ||
                   !string.IsNullOrEmpty(appleGameCenterId) ||
                   !string.IsNullOrEmpty(webglId);
            //    !string.IsNullOrEmpty(steamId) ||
        }
    }

    /// <summary>
    /// Serializable leaderboard configuration for cross-platform support
    /// </summary>
    [System.Serializable]
    public class LeaderboardMapping
    {
        [Tooltip("Internal leaderboard identifier used in your game code")]
        public string leaderboardId = "";

        [Tooltip("Display name for this leaderboard")]
        public string displayName = "";

        [Tooltip("Description of the leaderboard")]
        public string description = "";

        [Header("Platform-Specific IDs")]
        [Tooltip("Google Play Games leaderboard ID")]
        public string googlePlayGamesId = "";

        [Tooltip("Apple Game Center leaderboard ID")]
        public string appleGameCenterId = "";

        [Tooltip("Steam leaderboard ID (for future use)")]
        public string steamId = "";

        [Tooltip("WebGL/Wortal leaderboard ID (for future use)")]
        public string webglId = "";

        /// <summary>
        /// Get platform-specific ID for current platform
        /// </summary>
        public string GetPlatformId()
        {
#if UNITY_ANDROID
            return googlePlayGamesId;
#elif UNITY_IOS
            return appleGameCenterId;
#elif UNITY_WEBGL
            return webglId;
#else
            return leaderboardId; // Fallback to internal ID
#endif
        }

        /// <summary>
        /// Check if this leaderboard is configured for the current platform
        /// </summary>
        public bool IsConfiguredForCurrentPlatform()
        {
            var platformId = GetPlatformId();
            return !string.IsNullOrEmpty(platformId);
        }

        /// <summary>
        /// Validate this leaderboard mapping
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(leaderboardId) || string.IsNullOrEmpty(displayName))
                return false;

            // At least one platform ID should be configured
            return !string.IsNullOrEmpty(googlePlayGamesId) ||
                   !string.IsNullOrEmpty(appleGameCenterId) ||
                   !string.IsNullOrEmpty(steamId) ||
                   !string.IsNullOrEmpty(webglId);
        }
    }

    /// <summary>
    /// Helper class to convert serializable lists to dictionaries at runtime
    /// </summary>
    public static class PlatformMappingHelper
    {
        /// <summary>
        /// Convert achievement list to dictionary for fast lookup
        /// </summary>
        public static Dictionary<string, AchievementMapping> CreateAchievementDictionary(List<AchievementMapping> achievements)
        {
            return achievements?.Where(a => a.IsValid())
                              .ToDictionary(a => a.achievementId, a => a)
                   ?? new Dictionary<string, AchievementMapping>();
        }

        /// <summary>
        /// Convert leaderboard list to dictionary for fast lookup
        /// </summary>
        public static Dictionary<string, LeaderboardMapping> CreateLeaderboardDictionary(List<LeaderboardMapping> leaderboards)
        {
            return leaderboards?.Where(l => l.IsValid())
                               .ToDictionary(l => l.leaderboardId, l => l)
                   ?? new Dictionary<string, LeaderboardMapping>();
        }
    }
}
