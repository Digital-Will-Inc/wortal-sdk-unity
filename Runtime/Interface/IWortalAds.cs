using System;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Interface for ads functionality across platforms
    /// </summary>
    public interface IWortalAds
    {
        /// <summary>
        /// Shows an interstitial ad
        /// </summary>
        /// <param name="placement">Placement configuration for the ad</param>
        /// <param name="onSuccess">Callback for successful ad display</param>
        /// <param name="onError">Callback for ad errors</param>
        void ShowInterstitial(Placement placement, Action onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Shows a rewarded ad
        /// </summary>
        /// <param name="placement">Placement configuration for the ad</param>
        /// <param name="onSuccess">Callback for successful ad completion with reward</param>
        /// <param name="onError">Callback for ad errors</param>
        void ShowRewarded(Placement placement, Action onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Shows a banner ad
        /// </summary>
        /// <param name="shouldShow">Whether to show or hide the banner</param>
        /// <param name="position">Position of the banner</param>
        /// <param name="onSuccess">Callback for successful banner operation</param>
        /// <param name="onError">Callback for banner errors</param>
        void ShowBanner(bool shouldShow, BannerPosition position, Action onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Checks if ads are blocked by the platform or user
        /// </summary>
        /// <returns>True if ads are blocked</returns>
        bool IsAdBlocked();

        /// <summary>
        /// Checks if ads are enabled on this platform
        /// </summary>
        /// <returns>True if ads are enabled for the current session. False if ads are disabled.</returns>
        bool IsEnabled();

        /// <summary>
        /// Checks if ads are supported on this platform
        /// </summary>
        bool IsSupported { get; }
    }
}
