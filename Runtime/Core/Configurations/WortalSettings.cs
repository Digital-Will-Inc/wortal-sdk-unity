// Runtime/Core/Configuration/WortalSettings.cs
using UnityEngine;

namespace DigitalWill.WortalSDK.Core
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

        [Tooltip("Request server auth code")]
        public bool requestServerAuthCode = false;

        [Tooltip("Force resolve")]
        public bool forceResolve = true;

        [Header("Apple Game Center Settings")]
        [Tooltip("Enable Apple Game Center")]
        public bool enableAppleGameCenter = true;

        [Tooltip("Apple Game Center bundle ID")]
        public string appleGameCenterBundleId = "";

        [Header("Feature Toggles")]
        [Tooltip("Enable achievements")]
        public bool enableAchievements = true;

        [Tooltip("Enable leaderboards")]
        public bool enableLeaderboards = true;

        [Tooltip("Enable cloud save")]
        public bool enableCloudSave = true;

        [Tooltip("Enable analytics")]
        public bool enableAnalytics = true;

        [Tooltip("Enable ads")]
        public bool enableAds = true;

        [Tooltip("Enable in-app purchases")]
        public bool enableIAP = true;

        /// <summary>
        /// Validates the settings configuration
        /// </summary>
        public bool ValidateSettings()
        {
            bool isValid = true;

            if (enableGooglePlayGames && string.IsNullOrEmpty(googlePlayGamesAppId))
            {
                Debug.LogError("[Wortal] Google Play Games is enabled but App ID is not set");
                isValid = false;
            }

            if (enableAppleGameCenter && string.IsNullOrEmpty(appleGameCenterBundleId))
            {
                Debug.LogError("[Wortal] Apple Game Center is enabled but Bundle ID is not set");
                isValid = false;
            }

            return isValid;
        }
    }
}
