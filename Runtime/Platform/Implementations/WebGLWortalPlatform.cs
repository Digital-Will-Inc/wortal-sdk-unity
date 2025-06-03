using System;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// WebGL platform implementation using existing jslib files
    /// </summary>
    public class WebGLWortalPlatform : BaseWortalPlatform
    {
        public override WortalPlatformType PlatformType => WortalPlatformType.WebGL;

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

        public override IWortalAuthentication Authentication => _authentication ??= new WebGLWortalAuthentication();
        public override IWortalAchievements Achievements => _achievements ??= new WebGLWortalAchievements();
        public override IWortalAds Ads => _ads ??= new WebGLWortalAds();
        public override IWortalAnalytics Analytics => _analytics ??= new WebGLWortalAnalytics();
        public override IWortalContext Context => _context ??= new WebGLWortalContext();
        public override IWortalIAP IAP => _iap ??= new WebGLWortalIAP();
        public override IWortalLeaderboard Leaderboard => _leaderboard ??= new WebGLWortalLeaderboard();
        public override IWortalNotifications Notifications => _notifications ??= new WebGLWortalNotifications();
        public override IWortalPlayer Player => _player ??= new WebGLWortalPlayer();
        public override IWortalSession Session => _session ??= new WebGLWortalSession();
        public override IWortalStats Stats => _stats ??= new WebGLWortalStats();
        public override IWortalTournament Tournament => _tournament ??= new WebGLWortalTournament();
        protected override void InitializePlatformSpecific(Action onSuccess, Action<WortalError> onError)
        {
            // WebGL initialization is handled by the jslib files
            onSuccess?.Invoke();
        }

        public override string[] GetSupportedAPIs()
        {
            return new string[]
            {
                "Authentication", "Achievements", "Ads", "Analytics",
                "Context", "IAP", "Leaderboard", "Player", "Session"
            };
        }
    }
}
