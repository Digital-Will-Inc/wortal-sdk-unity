using System;
using UnityEngine;

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
#if WORTAL_APPLE_GAMEKIT && UNITY_IOS
            // Initialize Apple Game Center
            InitializeGameCenter(onSuccess, onError);
#else
            Debug.LogWarning("[Wortal] Apple Game Center not available. Some features may be limited.");
            onSuccess?.Invoke();
#endif
        }

#if WORTAL_APPLE_GAMEKIT && UNITY_IOS
        private void InitializeGameCenter(Action onSuccess, Action<WortalError> onError)
        {
            try
            {
                // Initialize Game Center
                onSuccess?.Invoke();
            }
            catch (Exception e)
            {
                var error = new WortalError
                {
                    Code = WortalErrorCodes.INITIALIZATION_ERROR.ToString(),
                    Message = $"Failed to initialize Game Center: {e.Message}"
                };
                onError?.Invoke(error);
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
