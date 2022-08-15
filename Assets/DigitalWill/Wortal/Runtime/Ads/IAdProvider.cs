namespace DigitalWill
{
    /// <summary>
    /// Interface for ad provider services. Will call ads based on the platform the game is deployed to.
    /// </summary>
    public interface IAdProvider
    {
        public delegate void BeforeAdDelegate();
        public delegate void AfterAdDelegate();
        public delegate void AdDismissedDelegate();
        public delegate void AdViewedDelegate();
        public delegate void NoShowDelegate();

        /// <summary>
        /// Shows an interstitial ad.
        /// </summary>
        /// <param name="type">Type of ad placement.</param>
        /// <param name="description">Description of the ad being shown. Ex: NextLevel</param>
        void ShowInterstitialAd(Placement type, string description);
        /// <summary>
        /// Shows a rewarded ad.
        /// </summary>
        /// <param name="description">Description of the ad being shown. Ex: ReviveAndContinue</param>
        void ShowRewardedAd(string description);
    }
}
