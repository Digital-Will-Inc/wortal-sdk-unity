using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Ads API
    /// </summary>
    public class WortalAds
    {
        private delegate void BeforeAdDelegate();
        private delegate void AfterAdDelegate();
        private delegate void AdDismissedDelegate();
        private delegate void AdViewedDelegate();

        /// <summary>
        /// An ad was requested and successfully returned. This is fired before the ad is shown so it can be used
        /// for pausing the game, muting audio, etc.
        /// </summary>
        public event Action BeforeAd;
        /// <summary>
        /// An ad request has finished. This does not guarantee an ad was shown, only that the request to the provider
        /// has finished and the player can resume the game now.
        /// </summary>
        public event Action AfterAd;
        /// <summary>
        /// A rewarded ad was successfully viewed and the player should be given a reward.
        /// </summary>
        public event Action RewardPlayer;
        /// <summary>
        /// A rewarded ad was dismissed and the player should not receive a reward.
        /// </summary>
        public event Action RewardSkipped;

        /// <summary>
        /// Shows an interstitial ad. These can be shown at various points in the game such as a level end, restart or a timed
        /// interval in games with longer levels.
        /// </summary>
        /// <param name="placement">Placement type for the ad.</param>
        /// <param name="description">Description of the placement.</param>
        /// <example>
        /// // Player reached the next level.
        /// <code>Wortal.Ads.ShowInterstitial(Placement.Next, "NextLevel");</code>
        ///
        /// // Player paused the game.
        /// <code>Wortal.Ads.ShowInterstitial(Placement.Pause, "PausedGame");</code>
        ///
        /// // Player opened the IAP shop.
        /// <code>Wortal.Ads.ShowInterstitial(Placement.Browse, "BrowseShop");</code>
        /// </example>
        public void ShowInterstitial(Placement placement, string description)
        {
            ShowInterstitialJS(
                placement.ToString().ToLower(),
                description,
                BeforeAdCallback,
                AfterAdCallback);
        }

        /// <summary>
        /// Shows a rewarded ad. These are longer, optional ads that the player can earn a reward for watching. The player
        /// must be notified of the ad and give permission to show before it can be shown.
        /// </summary>
        /// <param name="description">Description of the placement.</param>
        /// <example>
        /// <code>Wortal.Ads.ShowRewarded("ReviveAndContinue");</code>
        /// </example>
        public void ShowRewarded(string description)
        {
            ShowRewardedJS(
                description,
                BeforeAdCallback,
                AfterAdCallback,
                AdDismissedCallback,
                AdViewedCallback);
        }

        [DllImport("__Internal")]
        private static extern void ShowInterstitialJS(string type, string description,
                                                      BeforeAdDelegate beforeAdCallback,
                                                      AfterAdDelegate afterAdCallback);

        [DllImport("__Internal")]
        private static extern void ShowRewardedJS(string description,
                                                     BeforeAdDelegate beforeAdCallback,
                                                     AfterAdDelegate afterAdCallback,
                                                     AdDismissedDelegate adDismissedDelegate,
                                                     AdViewedDelegate adViewedDelegate);

        [MonoPInvokeCallback(typeof(BeforeAdDelegate))]
        private static void BeforeAdCallback()
        {
            Wortal.Ads.BeforeAd?.Invoke();
        }

        [MonoPInvokeCallback(typeof(AfterAdDelegate))]
        private static void AfterAdCallback()
        {
            Wortal.Ads.AfterAd?.Invoke();
        }

        [MonoPInvokeCallback(typeof(AdDismissedDelegate))]
        private static void AdDismissedCallback()
        {
            Wortal.Ads.RewardSkipped?.Invoke();
        }

        [MonoPInvokeCallback(typeof(AdViewedDelegate))]
        private static void AdViewedCallback()
        {
            Wortal.Ads.RewardPlayer?.Invoke();
        }
    }

    /// <summary>
    /// Types of ad placements as defined by Google:
    /// https://developers.google.com/ad-placement/docs/placement-types
    /// </summary>
    public enum Placement
    {
        /// <summary>
        /// The player navigates to the next level.
        /// </summary>
        Next,
        /// <summary>
        /// Your game has loaded, the UI is visible and sound is enabled, the player can interact with the game,
        /// but the game play has not started yet.
        /// </summary>
        Start,
        /// <summary>
        /// The player pauses the game.
        /// </summary>
        Pause,
        /// <summary>
        /// The player explores options outside of gameplay.
        /// </summary>
        Browse,
    }
}
