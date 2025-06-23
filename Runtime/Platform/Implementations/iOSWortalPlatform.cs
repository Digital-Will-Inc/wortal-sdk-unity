using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// iOS platform implementation with Game Center
    /// </summary>
    public class iOSWortalPlatform : BaseWortalPlatform
    {
        public override WortalPlatformType PlatformType => WortalPlatformType.iOS;

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

        public override IWortalAuthentication Authentication => _authentication ??= new iOSWortalAuthentication();
        public override IWortalAchievements Achievements => _achievements ??= new iOSWortalAchievements();
        public override IWortalAds Ads => _ads ??= new iOSWortalAds();
        public override IWortalAnalytics Analytics => _analytics ??= new iOSWortalAnalytics();
        public override IWortalContext Context => _context ??= new iOSWortalContext();
        public override IWortalIAP IAP => _iap ??= new iOSWortalIAP();
        public override IWortalLeaderboard Leaderboard => _leaderboard ??= new iOSWortalLeaderboard();
        public override IWortalNotifications Notifications => _notifications ??= new iOSWortalNotifications();
        public override IWortalPlayer Player => _player ??= new iOSWortalPlayer();
        public override IWortalSession Session => _session ??= new iOSWortalSession();
        public override IWortalStats Stats => _stats ??= new iOSWortalStats();
        public override IWortalTournament Tournament => _tournament ??= new iOSWortalTournament();

        protected override void InitializePlatformSpecific(Action onSuccess, Action<WortalError> onError)
        {
#if UNITY_IOS && !UNITY_EDITOR
            // Initialize Apple Game Center with silent authentication
            InitializeGameCenterWithSilentAuth(onSuccess, onError);
#else
            Debug.LogWarning("[Wortal] Apple Game Center not available. Some features may be limited.");
            onSuccess?.Invoke();
#endif
        }

#if UNITY_IOS && !UNITY_EDITOR
        private void InitializeGameCenterWithSilentAuth(Action onSuccess, Action<WortalError> onError)
        {
            try
            {
                Debug.Log("[iOS] Initializing Game Center...");

                // Check if Game Center is available
                if (!IsGameCenterAvailable())
                {
                    Debug.LogWarning("[iOS] Game Center not available on this device");
                    onSuccess?.Invoke(); // Don't fail completely
                    return;
                }

                // Attempt silent authentication
                AttemptSilentAuthentication(onSuccess, onError);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[iOS] Game Center initialization failed: {e.Message}");
                // Don't fail completely - continue without Game Center
                onSuccess?.Invoke();
            }
        }

        private bool IsGameCenterAvailable()
        {
            // Game Center is generally available on iOS 4.1+
            // Unity's Social API will handle the availability check
            return Application.platform == RuntimePlatform.IPhonePlayer;
        }

        private void AttemptSilentAuthentication(Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[iOS] Attempting silent Game Center authentication...");

            // Check if already authenticated
            if (Social.localUser.authenticated)
            {
                Debug.Log("[iOS] Already authenticated with Game Center");
                onSuccess?.Invoke();
                return;
            }

            // Attempt silent authentication
            Social.localUser.Authenticate((success) =>
            {
                if (success)
                {
                    Debug.Log("[iOS] Silent Game Center authentication successful!");
                    Debug.Log($"[iOS] User: {Social.localUser.userName} (ID: {Social.localUser.id})");
                }
                else
                {
                    Debug.Log("[iOS] Silent Game Center authentication failed. User can authenticate manually later.");
                }

                // Always continue initialization - silent auth failure is not critical
                onSuccess?.Invoke();
            });
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
