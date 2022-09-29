using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace DigitalWill
{
    /// <summary>
    /// Ad handler for the Wortal.
    /// </summary>
    internal class WortalAds
    {
        public delegate void BeforeAdDelegate();
        public delegate void AfterAdDelegate();
        public delegate void AdDismissedDelegate();
        public delegate void AdViewedDelegate();

        /// <summary>
        /// Shows an interstitial ad.
        /// </summary>
        /// <param name="placement">Type of ad placement.</param>
        /// <param name="description">Description of the ad being shown. Ex: NextLevel</param>
        public void ShowInterstitialAd(Placement placement, string description)
        {
            ShowInterstitialAd(
                placement.ToString().ToLower(),
                description,
                BeforeAdCallback,
                AfterAdCallback);
        }

        /// <summary>
        /// Shows a rewarded ad.
        /// </summary>
        /// <param name="description">Description of the ad being shown. Ex: ReviveAndContinue</param>
        public void ShowRewardedAd(string description)
        {
            ShowRewardedAd(
                description,
                BeforeAdCallback,
                AfterAdCallback,
                AdDismissedCallback,
                AdViewedCallback);
        }

        [DllImport("__Internal")]
        private static extern void ShowInterstitialAd(string type, string description,
                                                      BeforeAdDelegate beforeAdCallback,
                                                      AfterAdDelegate afterAdCallback);

        [DllImport("__Internal")]
        private static extern void ShowRewardedAd(string description,
                                                     BeforeAdDelegate beforeAdCallback,
                                                     AfterAdDelegate afterAdCallback,
                                                     AdDismissedDelegate adDismissedDelegate,
                                                     AdViewedDelegate adViewedDelegate);

        [MonoPInvokeCallback(typeof(BeforeAdDelegate))]
        private static void BeforeAdCallback()
        {
            Debug.Log("[Wortal] BeforeAdCallback");
            Wortal.InvokeBeforeAd();
        }

        [MonoPInvokeCallback(typeof(AfterAdDelegate))]
        private static void AfterAdCallback()
        {
            Debug.Log("[Wortal] AfterAdCallback");
            Wortal.InvokeAfterAd();
        }

        [MonoPInvokeCallback(typeof(AdDismissedDelegate))]
        private static void AdDismissedCallback()
        {
            Debug.Log("[Wortal] AdDismissedCallback");
            Wortal.InvokeRewardSkipped();
        }

        [MonoPInvokeCallback(typeof(AdViewedDelegate))]
        private static void AdViewedCallback()
        {
            Debug.Log("[Wortal] AdViewedCallback");
            Wortal.InvokeRewardPlayer();
        }
    }
}
