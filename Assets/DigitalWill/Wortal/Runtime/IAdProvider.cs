namespace DigitalWill
{
    /// <summary>
    /// Interface for ad providers. Used for implementation ad calls for different platforms.
    /// </summary>
    public interface IAdProvider
    {
        /// <summary>
        /// Shows an interstitial ad.
        /// </summary>
        /// <param name="type">Type of ad placement.</param>
        /// <param name="name">Name of the ad placement.</param>
        void ShowInterstitialAd(Placement type, string name);
        /// <summary>
        /// Requests a rewarded ad. Should be called before <see cref="ShowRewardedAd"/>.
        /// </summary>
        /// <param name="name">Name of the ad to be shown.</param>
        void ShowRewardedAd(string name);
    }
}
