using System;

namespace DigitalWill.H5Portal
{
    /// <summary>
    /// Interface for ad providers. Used for implementation ad calls for different platforms.
    /// </summary>
    public interface IAdProvider
    {
        /// <summary>
        /// Was an ad successfully returned or not. This gets set in the beforeAd callback and checked in Wortal.cs
        /// on a timer to ensure we don't get stuck in an infinite loop waiting for an due to an error.
        /// </summary>
        bool IsAdAvailable { get; }

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
        void RequestRewardedAd(string name);
        /// <summary>
        /// Shows a rewarded ad.
        /// </summary>
        void ShowRewardedAd();
    }
}
