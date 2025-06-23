using System;
using System.Reflection;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Android platform implementation with Google Play Games Services
    /// </summary>
    public class AndroidWortalPlatform : BaseWortalPlatform
    {
        public override WortalPlatformType PlatformType => WortalPlatformType.Android;

        // Lazy-loaded API implementations
        private IWortalAuthentication _authentication;
        private IWortalAchievements _achievements;
        private IWortalAds _ads;
        private IWortalAnalytics _analytics;
        private IWortalContext _context;
        private IWortalIAP _iap;
        private IWortalLeaderboard _leaderboard;
        private IWortalNotifications _notifications;
        private IWortalPlayer _player;
        private IWortalSession _session;
        private IWortalStats _stats;
        private IWortalTournament _tournament;

        public override IWortalAuthentication Authentication => _authentication ??= new AndroidWortalAuthentication();
        public override IWortalAchievements Achievements => _achievements ??= new AndroidWortalAchievements();
        public override IWortalAds Ads => _ads ??= new AndroidWortalAds();
        public override IWortalAnalytics Analytics => _analytics ??= new AndroidWortalAnalytics();
        public override IWortalContext Context => _context ??= new AndroidWortalContext();
        public override IWortalIAP IAP => _iap ??= new AndroidWortalIAP();
        public override IWortalLeaderboard Leaderboard => _leaderboard ??= new AndroidWortalLeaderboard();
        public override IWortalNotifications Notifications => _notifications ??= new AndroidWortalNotifications();
        public override IWortalPlayer Player => _player ??= new AndroidWortalPlayer();
        public override IWortalSession Session => _session ??= new AndroidWortalSession();
        public override IWortalStats Stats => _stats ??= new AndroidWortalStats();
        public override IWortalTournament Tournament => _tournament ??= new AndroidWortalTournament();

        protected override void InitializePlatformSpecific(Action onSuccess, Action<WortalError> onError)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            // Initialize Google Play Games Services with silent login
            InitializeGooglePlayGamesWithSilentLogin(onSuccess, onError);
#else
            Debug.LogWarning("[Wortal] Google Play Games Services not available. Some features may be limited.");
            onSuccess?.Invoke();
#endif
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        private void InitializeGooglePlayGamesWithSilentLogin(Action onSuccess, Action<WortalError> onError)
        {
            try
            {
                // Initialize using reflection to avoid compile-time dependencies
                InitializeGooglePlayGamesReflection();
                
                // Attempt silent login after initialization
                AttemptSilentLogin(onSuccess, onError);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[Android] Google Play Games initialization failed: {e.Message}");
                // Don't fail completely - continue without GPG
                onSuccess?.Invoke();
            }
        }

        private void InitializeGooglePlayGamesReflection()
        {
            try
            {
                var playGamesPlatformType = Type.GetType("GooglePlayGames.PlayGamesPlatform, GooglePlayGames");
                var configBuilderType = Type.GetType("GooglePlayGames.BasicApi.PlayGamesClientConfiguration+Builder, GooglePlayGames.BasicApi");
                
                if (playGamesPlatformType == null || configBuilderType == null)
                {
                    Debug.LogWarning("[Android] Google Play Games types not found");
                    return;
                }

                // Create configuration
                var builderInstance = Activator.CreateInstance(configBuilderType);
                
                // Call RequestServerAuthCode if needed
                if (_settings.requestServerAuthCode)
                {
                    var requestServerAuthCodeMethod = configBuilderType.GetMethod("RequestServerAuthCode", new[] { typeof(bool) });
                    requestServerAuthCodeMethod?.Invoke(builderInstance, new object[] { true });
                }

                // Call RequestEmail and RequestIdToken
                var requestEmailMethod = configBuilderType.GetMethod("RequestEmail");
                requestEmailMethod?.Invoke(builderInstance, null);
                
                var requestIdTokenMethod = configBuilderType.GetMethod("RequestIdToken");
                requestIdTokenMethod?.Invoke(builderInstance, null);

                // Build configuration
                var buildMethod = configBuilderType.GetMethod("Build");
                var config = buildMethod?.Invoke(builderInstance, null);

                // Initialize instance
                var initializeInstanceMethod = playGamesPlatformType.GetMethod("InitializeInstance", BindingFlags.Public | BindingFlags.Static);
                initializeInstanceMethod?.Invoke(null, new[] { config });

                // Set debug logging
                var debugLogEnabledProperty = playGamesPlatformType.GetProperty("DebugLogEnabled", BindingFlags.Public | BindingFlags.Static);
                debugLogEnabledProperty?.SetValue(null, _settings.enableDebugLogging);

                // Activate platform
                var activateMethod = playGamesPlatformType.GetMethod("Activate", BindingFlags.Public | BindingFlags.Static);
                activateMethod?.Invoke(null, null);

                Debug.Log("[Android] Google Play Games initialized successfully");
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[Android] Google Play Games reflection initialization failed: {e.Message}");
            }
        }

        private void AttemptSilentLogin(Action onSuccess, Action<WortalError> onError)
        {
            try
            {
                var playGamesPlatformType = Type.GetType("GooglePlayGames.PlayGamesPlatform, GooglePlayGames");
                var signInStatusType = Type.GetType("GooglePlayGames.BasicApi.SignInStatus, GooglePlayGames.BasicApi");
                
                if (playGamesPlatformType == null || signInStatusType == null)
                {
                    Debug.LogWarning("[Android] Cannot perform silent login - types not found");
                    onSuccess?.Invoke();
                    return;
                }

                var instanceProperty = playGamesPlatformType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
                var playGamesInstance = instanceProperty?.GetValue(null);

                if (playGamesInstance == null)
                {
                    Debug.LogWarning("[Android] PlayGamesPlatform instance not available");
                    onSuccess?.Invoke();
                    return;
                }

                // Check if already authenticated
                var isAuthenticatedMethod = playGamesPlatformType.GetMethod("IsAuthenticated");
                var isAuthenticated = (bool)isAuthenticatedMethod.Invoke(playGamesInstance, null);

                if (isAuthenticated)
                {
                    Debug.Log("[Android] Already authenticated with Google Play Games");
                    onSuccess?.Invoke();
                    return;
                }

                // Attempt silent authentication
                Debug.Log("[Android] Attempting silent login with Google Play Games...");
                
                var authenticateMethod = playGamesPlatformType.GetMethod("Authenticate", new[] { typeof(Action<>).MakeGenericType(signInStatusType) });

                Action<object> silentAuthCallback = (signInStatus) =>
                {
                    var successValue = Enum.Parse(signInStatusType, "Success");
                    
                    if (signInStatus.Equals(successValue))
                    {
                        Debug.Log("[Android] Silent login successful!");
                        onSuccess?.Invoke();
                    }
                    else
                    {
                        Debug.Log($"[Android] Silent login failed: {signInStatus}. User can still authenticate manually.");
                        // Don't treat this as an error - just continue without authentication
                        onSuccess?.Invoke();
                    }
                };

                authenticateMethod?.Invoke(playGamesInstance, new object[] { silentAuthCallback });
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[Android] Silent login attempt failed: {e.Message}");
                // Continue without failing the entire initialization
                onSuccess?.Invoke();
            }
        }
#endif

        public override string[] GetSupportedAPIs()
        {
            return new string[]
            {
                "Authentication", "Achievements", "Leaderboard",
                "Player", "Session", "Analytics"
            };
        }
    }
}