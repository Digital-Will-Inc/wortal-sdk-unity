using UnityEngine;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Wortal API
    /// </summary>
    public static class Wortal
    {
        /// <summary>
        /// Ads API
        /// </summary>
        public static WortalAds Ads { get; private set; }
        /// <summary>
        /// Analytics API
        /// </summary>
        public static WortalAnalytics Analytics { get; private set; }
        /// <summary>
        /// Context API
        /// </summary>
        public static WortalContext Context { get; private set; }
        /// <summary>
        /// In-App Purchasing API
        /// </summary>
        public static WortalIAP IAP { get; private set; }
        /// <summary>
        /// Leaderboard API
        /// </summary>
        public static WortalLeaderboard Leaderboard { get; private set; }
        /// <summary>
        /// Player API
        /// </summary>
        public static WortalPlayer Player { get; private set; }
        /// <summary>
        /// Session API
        /// </summary>
        public static WortalSession Session { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            Debug.Log("[Wortal] Initializing Unity SDK..");
            Ads = new WortalAds();
            Analytics = new WortalAnalytics();
            Context = new WortalContext();
            IAP = new WortalIAP();
            Leaderboard = new WortalLeaderboard();
            Player = new WortalPlayer();
            Session = new WortalSession();
            Debug.Log("[Wortal] Unity SDK initialization complete.");
        }
    }
}
