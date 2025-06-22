using System;
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Main entry point for the Wortal SDK
    /// Provides unified API across all supported platforms (WebGL, Android with Google Play Games, iOS with Game Center)
    /// </summary>
    public static class Wortal
    {
        private static Action _initializeCallback;
        private static Action _startGameCallback;
        private static Action<AuthResponse> _authenticateCallback;
        private static Action<bool> _linkAccountCallback;
        private static Action _performHapticFeedbackCallback;
        private static bool _isInitialized = false;
        private static IWortalPlatform _platform;
        private static WortalSettings _settings;

        public static Action<WortalError> WortalError;
        public static Action OnPause;

        public static Action OnResume;

        /// <summary>
        /// Gets the current platform implementation
        /// </summary>
        public static IWortalPlatform Platform
        {
            get
            {
                if (_platform == null)
                {
                    _platform = WortalPlatformManager.CurrentPlatform;
                }
                return _platform;
            }
        }

        /// <summary>
        /// Gets the Wortal SDK settings
        /// </summary>
        public static WortalSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = WortalSettings.Instance;
                }
                return _settings;
            }
        }

        #region API Properties

        /// <summary>
        /// Authentication API - Unified authentication across Google Play Games, Apple Game Center, and WebGL
        /// </summary>
        public static IWortalAuthentication Authentication => Platform.Authentication;

        /// <summary>
        /// Achievements API - Unified achievements across all platforms
        /// </summary>
        public static IWortalAchievements Achievements => Platform.Achievements;

        /// <summary>
        /// Ads API - Unified advertising across all platforms
        /// </summary>
        public static IWortalAds Ads => Platform.Ads;

        /// <summary>
        /// Analytics API - Unified analytics tracking across all platforms
        /// </summary>
        public static IWortalAnalytics Analytics => Platform.Analytics;

        /// <summary>
        /// Context API - Social context and multiplayer features
        /// </summary>
        public static IWortalContext Context => Platform.Context;

        /// <summary>
        /// In-App Purchase API - Unified IAP across all platforms
        /// </summary>
        public static IWortalIAP IAP => Platform.IAP;

        /// <summary>
        /// Leaderboard API - Unified leaderboards across all platforms
        /// </summary>
        public static IWortalLeaderboard Leaderboard => Platform.Leaderboard;

        /// <summary>
        /// Notifications API - Unified notifications across all platforms
        /// </summary>
        public static IWortalNotifications Notifications => Platform.Notifications;

        /// <summary>
        /// Player API - Unified player data and social features
        /// </summary>
        public static IWortalPlayer Player => Platform.Player;

        /// <summary>
        /// Session API - Game session management and platform-specific features
        /// </summary>
        public static IWortalSession Session => Platform.Session;

        /// <summary>
        /// Stats API - Unified game statistics tracking across all platforms
        /// </summary>
        public static IWortalStats Stats => Platform.Stats;

        /// <summary>
        /// Tournament API - Unified tournament management across all platforms
        /// </summary>
        public static IWortalTournament Tournament => Platform.Tournament;

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the Wortal SDK with automatic platform detection
        /// </summary>
        /// <param name="onSuccess">Callback for successful initialization</param>
        /// <param name="onError">Callback for initialization errors</param>
        public static void Initialize(Action onSuccess = null, Action<WortalError> onError = null)
        {
            if (_isInitialized)
            {
                if (Settings.enableDebugLogging)
                {
                    Debug.Log("[Wortal] SDK already initialized");
                }
                onSuccess?.Invoke();
                return;
            }

            if (Settings.enableDebugLogging)
            {
                Debug.Log($"[Wortal] Initializing SDK for platform: {Platform.PlatformType}");
                WortalDependencyChecker.CheckAllDependencies();
            }

            if (!Settings.ValidateAll())
            {
                Debug.LogWarning("[Wortal] SDK initialized with configuration warnings. Check console for details.");
            }



            // Validate configuration before initialization
            if (!Settings.ValidateSettings())
            {
                var error = new WortalError
                {
                    Code = WortalErrorCodes.INVALID_CONFIGURATION.ToString(),
                    Message = "Invalid Wortal SDK configuration. Check console for details."
                };
                onError?.Invoke(error);
                return;
            }

            Platform.Initialize(
                () =>
                {
                    _isInitialized = true;
                    if (Settings.enableDebugLogging)
                    {
                        Debug.Log($"[Wortal] SDK initialized successfully on {Platform.PlatformType}");
                        LogSupportedAPIs();
                    }
                    onSuccess?.Invoke();
                },
                (error) =>
                {
                    Debug.LogError($"[Wortal] Failed to initialize SDK: {error.Message}");
                    onError?.Invoke(error);
                }
            );
        }

        /// <summary>
        /// Auto-initializes the SDK if enabled in settings
        /// Called by Unity's RuntimeInitializeOnLoadMethod
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void AutoInitialize()
        {
            if (Settings.autoInitialize)
            {
                Initialize(
                    () =>
                    {
                        if (Settings.enableDebugLogging)
                        {
                            Debug.Log("[Wortal] Auto-initialization completed");
                        }
                    },
                    (error) =>
                    {
                        Debug.LogError($"[Wortal] Auto-initialization failed: {error.Message}");
                    }
                );
            }
        }

        #endregion

        #region Game Lifecycle

        /// <summary>
        /// Starts the game (primarily for WebGL compatibility, but works on all platforms)
        /// </summary>
        /// <param name="onSuccess">Callback for successful game start</param>
        /// <param name="onError">Callback for errors</param>
        public static void StartGame(Action onSuccess = null, Action<WortalError> onError = null)
        {
            if (!_isInitialized)
            {
                var error = new WortalError
                {
                    Code = WortalErrorCodes.INITIALIZATION_ERROR.ToString(),
                    Message = "Wortal SDK must be initialized before starting the game"
                };
                onError?.Invoke(error);
                return;
            }

            if (Platform.PlatformType == WortalPlatformType.WebGL)
            {
                // WebGL-specific implementation handles platform requirements
                Platform.Session.StartGame(onSuccess, onError);
            }
            else
            {
                // For native platforms (Android/iOS), game is already "started"
                if (Settings.enableDebugLogging)
                {
                    Debug.Log($"[Wortal] Game started on {Platform.PlatformType}");
                }
                onSuccess?.Invoke();
            }
        }

        /// <summary>
        /// Sets the loading progress (useful for WebGL platforms)
        /// </summary>
        /// <param name="progress">Loading progress (0-100)</param>
        public static void SetLoadingProgress(int progress)
        {
            if (_isInitialized)
            {
                Platform.Session.SetLoadingProgress(progress);
            }
        }

        /// <summary>
        /// Notifies the platform that the game is ready
        /// </summary>
        public static void GameReady()
        {
            if (_isInitialized)
            {
                Platform.Session.GameReady();
                if (Settings.enableDebugLogging)
                {
                    Debug.Log("[Wortal] Game ready signal sent");
                }
            }
        }

        /// <summary>
        /// Notifies the platform that gameplay has started
        /// </summary>
        public static void GameplayStart()
        {
            if (_isInitialized)
            {
                Platform.Session.GameplayStart();
                if (Settings.enableDebugLogging)
                {
                    Debug.Log("[Wortal] Gameplay started");
                }
            }
        }

        /// <summary>
        /// Notifies the platform that gameplay has stopped
        /// </summary>
        public static void GameplayStop()
        {
            if (_isInitialized)
            {
                Platform.Session.GameplayStop();
                if (Settings.enableDebugLogging)
                {
                    Debug.Log("[Wortal] Gameplay stopped");
                }
            }
        }

        /// <summary>
        /// Notifies the platform of a happy moment (good time for ads/prompts)
        /// </summary>
        public static void HappyTime()
        {
            if (_isInitialized)
            {
                Platform.Session.HappyTime();
                if (Settings.enableDebugLogging)
                {
                    Debug.Log("[Wortal] Happy time signal sent");
                }
            }
        }

        #endregion

        #region Platform Information

        /// <summary>
        /// Gets the supported APIs for the current platform
        /// </summary>
        /// <returns>Array of supported API names</returns>
        public static string[] GetSupportedAPIs()
        {
            return Platform.GetSupportedAPIs();
        }

        /// <summary>
        /// Checks if the SDK is initialized
        /// </summary>
        public static bool IsInitialized => _isInitialized && Platform.IsInitialized;

        /// <summary>
        /// Gets the current platform type
        /// </summary>
        public static WortalPlatformType PlatformType => Platform.PlatformType;

        /// <summary>
        /// Checks if a specific API is supported on the current platform
        /// </summary>
        /// <param name="apiName">Name of the API to check</param>
        /// <returns>True if the API is supported</returns>
        public static bool IsAPISupported(string apiName)
        {
            var supportedAPIs = GetSupportedAPIs();
            return Array.IndexOf(supportedAPIs, apiName) > -1;
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Get current dependency status
        /// </summary>
        public static DependencyStatus GetDependencyStatus()
        {
            return WortalDependencyChecker.GetDependencyStatus();
        }

        /// <summary>
        /// Check if platform features are available
        /// </summary>
        public static bool IsPlatformReady()
        {
            var status = GetDependencyStatus();

#if UNITY_ANDROID
            return status.AndroidReady;
#elif UNITY_IOS
            return status.iOSReady;
#else
            return true; // WebGL and other platforms
#endif
        }

        /// <summary>
        /// Logs the supported APIs for debugging purposes
        /// </summary>
        private static void LogSupportedAPIs()
        {
            var supportedAPIs = GetSupportedAPIs();
            if (supportedAPIs.Length > 0)
            {
                Debug.Log($"[Wortal] Supported APIs on {PlatformType}: {string.Join(", ", supportedAPIs)}");
            }
            else
            {
                Debug.Log($"[Wortal] No APIs reported as supported on {PlatformType}");
            }
        }

        /// <summary>
        /// Forces reinitialization of the platform (useful for testing)
        /// </summary>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void ForceReinitialize()
        {
            _isInitialized = false;
            _platform = null;
            WortalPlatformManager.ForceReinitialize();

            if (Settings.enableDebugLogging)
            {
                Debug.Log("[Wortal] Platform reinitialization forced");
            }
        }

        #endregion

        #region Legacy Compatibility Methods

        /// <summary>
        /// [DEPRECATED] Use Authentication.Authenticate() instead
        /// Authenticates the player on the current platform
        /// </summary>
        [Obsolete("Use Authentication.Authenticate() instead", false)]
        public static void Authenticate(Action<AuthResponse> onSuccess, Action<WortalError> onError)
        {
            if (Settings.enableDebugLogging)
            {
                Debug.LogWarning("[Wortal] Wortal.Authenticate() is deprecated. Use Wortal.Authentication.Authenticate() instead.");
            }
            Authentication.Authenticate(onSuccess, onError);
        }

        /// <summary>
        /// [DEPRECATED] Use Authentication.LinkAccount() instead
        /// Links the current account with platform-specific account
        /// </summary>
        [Obsolete("Use Authentication.LinkAccount() instead", false)]
        public static void LinkAccount(Action<bool> onSuccess, Action<WortalError> onError)
        {
            if (Settings.enableDebugLogging)
            {
                Debug.LogWarning("[Wortal] Wortal.LinkAccount() is deprecated. Use Wortal.Authentication.LinkAccount() instead.");
            }
            Authentication.LinkAccount(onSuccess, onError);
        }

        /// <summary>
        /// [DEPRECATED] Use Achievements.GetAchievements() instead
        /// Gets all achievements for the game
        /// </summary>
        [Obsolete("Use Achievements.GetAchievements() instead", false)]
        public static void GetAchievements(Action<Achievement[]> onSuccess, Action<WortalError> onError)
        {
            if (Settings.enableDebugLogging)
            {
                Debug.LogWarning("[Wortal] Wortal.GetAchievements() is deprecated. Use Wortal.Achievements.GetAchievements() instead.");
            }
            Achievements.GetAchievements(onSuccess, onError);
        }

        /// <summary>
        /// [DEPRECATED] Use Achievements.UnlockAchievement() instead
        /// Unlocks an achievement
        /// </summary>
        [Obsolete("Use Achievements.UnlockAchievement() instead", false)]
        public static void UnlockAchievement(string achievementID, Action onSuccess, Action<WortalError> onError)
        {
            if (Settings.enableDebugLogging)
            {
                Debug.LogWarning("[Wortal] Wortal.UnlockAchievement() is deprecated. Use Wortal.Achievements.UnlockAchievement() instead.");
            }
            Achievements.UnlockAchievement(achievementID, onSuccess, onError);
        }

        #endregion

        #region Debug Information

        /// <summary>
        /// Gets debug information about the current SDK state
        /// </summary>
        /// <returns>Debug information string</returns>
        public static string GetDebugInfo()
        {
            var info = $"Wortal SDK Debug Info:\n";
            info += $"- Platform: {PlatformType}\n";
            info += $"- Initialized: {IsInitialized}\n";
            info += $"- Auto Initialize: {Settings.autoInitialize}\n";
            info += $"- Debug Logging: {Settings.enableDebugLogging}\n";
            info += $"- Supported APIs: {string.Join(", ", GetSupportedAPIs())}\n";

            // Platform-specific debug info
            switch (PlatformType)
            {
                case WortalPlatformType.Android:
                    info += $"- Google Play Games: {Settings.enableGooglePlayGames}\n";
                    info += $"- GPG App ID: {(string.IsNullOrEmpty(Settings.googlePlayGamesAppId) ? "Not Set" : "Set")}\n";
                    break;
                case WortalPlatformType.iOS:
                    info += $"- Apple Game Center: {Settings.enableAppleGameCenter}\n";
                    info += $"- Bundle ID: {(string.IsNullOrEmpty(Settings.appleGameCenterBundleId) ? "Not Set" : "Set")}\n";
                    break;
                case WortalPlatformType.WebGL:
                    info += $"- WebGL Game ID: {(string.IsNullOrEmpty(Settings.webglGameId) ? "Not Set" : "Set")}\n";
                    break;
            }

            return info;
        }

        /// <summary>
        /// Prints debug information to the console
        /// </summary>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void PrintDebugInfo()
        {
            Debug.Log(GetDebugInfo());
        }

        #endregion

        #region Internal

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            Debug.Log("[Wortal] Initializing Unity SDK..");
#if UNITY_WEBGL && !UNITY_EDITOR
            //registering callback
            OnPauseJS(OnPauseCallback);
            OnResumeJS(OnResumeCallback);
#endif
            Debug.Log("[Wortal] Unity SDK initialization complete.");
        }

        [DllImport("__Internal")]
        private static extern bool IsInitializedJS();

        [DllImport("__Internal")]
        private static extern void InitializeJS(Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void StartGameJS(Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void AuthenticateJS(Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void LinkAccountJS(Action<bool> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void SetLoadingProgressJS(int value);

        [DllImport("__Internal")]
        private static extern void OnPauseJS(Action callback);

        [DllImport("__Internal")]
        private static extern void OnResumeJS(Action callback);

        [DllImport("__Internal")]
        private static extern string GetSupportedAPIsJS();

        [DllImport("__Internal")]
        private static extern void PerformHapticFeedbackJS(Action callback, Action<string> errorCallback);

        [MonoPInvokeCallback(typeof(Action<string>))]
        public static void WortalErrorCallback(string error)
        {
            WortalError wortalError;

            try
            {
                wortalError = JsonConvert.DeserializeObject<WortalError>(error);
            }
            catch (Exception e)
            {
                wortalError = new WortalError
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message
                };
            }

            WortalError?.Invoke(wortalError);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void InitializeCallback()
        {
            _initializeCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void StartGameCallback()
        {
            _startGameCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void AuthenticateCallback(string status)
        {
            AuthResponse authResponse;

            try
            {
                authResponse = JsonConvert.DeserializeObject<AuthResponse>(status);
            }
            catch (Exception e)
            {
                WortalError error = new()
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message
                };

                WortalError?.Invoke(error);
                return;
            }

            _authenticateCallback?.Invoke(authResponse);
        }

        [MonoPInvokeCallback(typeof(Action<bool>))]
        private static void LinkAccountCallback(bool status)
        {
            _linkAccountCallback?.Invoke(status);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnPauseCallback()
        {
            OnPause?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnResumeCallback()
        {
            OnResume?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void PerformHapticFeedbackCallback()
        {
            _performHapticFeedbackCallback?.Invoke();
        }

        #endregion Internal
    }
}
