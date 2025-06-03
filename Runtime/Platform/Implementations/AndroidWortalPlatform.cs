using System;
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
#if WORTAL_GOOGLE_PLAY_GAMES && UNITY_ANDROID
            // Initialize Google Play Games Services
            InitializeGooglePlayGames(onSuccess, onError);
#else
            Debug.LogWarning("[Wortal] Google Play Games Services not available. Some features may be limited.");
            onSuccess?.Invoke();
#endif
        }

#if WORTAL_GOOGLE_PLAY_GAMES && UNITY_ANDROID
        private void InitializeGooglePlayGames(Action onSuccess, Action<WortalError> onError)
        {
            try
            {
                var config = new GooglePlayGames.BasicApi.PlayGamesClientConfiguration.Builder()
                    .RequestServerAuthCode(_settings.requestServerAuthCode)
                    .RequestEmail()
                    .RequestIdToken()
                    .Build();

                GooglePlayGames.PlayGamesPlatform.InitializeInstance(config);
                GooglePlayGames.PlayGamesPlatform.DebugLogEnabled = _settings.enableDebugLogging;
                GooglePlayGames.PlayGamesPlatform.Activate();

                onSuccess?.Invoke();
            }
            catch (Exception e)
            {
                var error = new WortalError
                {
                    Code = WortalErrorCodes.INITIALIZATION_ERROR.ToString(),
                    Message = $"Failed to initialize Google Play Games: {e.Message}"
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
