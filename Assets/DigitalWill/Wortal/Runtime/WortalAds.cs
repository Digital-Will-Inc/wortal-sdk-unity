﻿using System;
using System.Runtime.InteropServices;
using AOT;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Ads API
    /// </summary>
    public class WortalAds
    {
        private static Action _beforeAdCallback;
        private static Action _afterAdCallback;
        private static Action _adDismissedCallback;
        private static Action _adViewedCallback;

#region Public API
        /// <summary>
        /// Shows an interstitial ad. These can be shown at various points in the game such as a level end, restart or a timed
        /// interval in games with longer levels.
        /// </summary>
        /// <param name="placement">Placement type for the ad.</param>
        /// <param name="description">Description of the placement.</param>
        /// <param name="beforeAdCallback">Callback for before the ad is shown. Pause the game here.</param>
        /// <param name="afterAdCallback">Callback for after the ad is shown. Resume the game here.</param>
        /// <example><code>
        /// Wortal.Ads.ShowInterstitial(Placement.Next, "NextLevel",
        ///     () => PauseGame(),
        ///     () => ResumeGame());
        /// </code></example>
        public void ShowInterstitial(Placement placement, string description,
                                     Action beforeAdCallback, Action afterAdCallback)
        {
            _beforeAdCallback = beforeAdCallback;
            _afterAdCallback = afterAdCallback;
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
        /// <param name="beforeAdCallback">Callback for before the ad is shown. Pause the game here.</param>
        /// <param name="afterAdCallback">Callback for after the ad is shown. Resume the game here.</param>
        /// <param name="adDismissedCallback">Callback for when the player dismissed the ad. Do not reward the player.</param>
        /// <param name="adViewedCallback">Callback for when the player has successfully watched the ad. Reward the player here.</param>
        /// <example><code>
        /// Wortal.Ads.ShowRewarded("ReviveAndContinue",
        ///     () => PauseGame(),
        ///     () => ResumeGame(),
        ///     () => DontReward(),
        ///     () => RewardPlayer());
        ///</code></example>
        public void ShowRewarded(string description, Action beforeAdCallback, Action afterAdCallback,
                                 Action adDismissedCallback, Action adViewedCallback)
        {
            _beforeAdCallback = beforeAdCallback;
            _afterAdCallback = afterAdCallback;
            _adDismissedCallback = adDismissedCallback;
            _adViewedCallback = adViewedCallback;
            ShowRewardedJS(
                description,
                BeforeAdCallback,
                AfterAdCallback,
                AdDismissedCallback,
                AdViewedCallback);
        }
#endregion Public API

#region JSlib Interface
        [DllImport("__Internal")]
        private static extern void ShowInterstitialJS(string type, string description,
                                                      Action beforeAdCallback,
                                                      Action afterAdCallback);

        [DllImport("__Internal")]
        private static extern void ShowRewardedJS(string description,
                                                  Action beforeAdCallback,
                                                  Action afterAdCallback,
                                                  Action adDismissedCallback,
                                                  Action adViewedCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void BeforeAdCallback()
        {
            _beforeAdCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void AfterAdCallback()
        {
            _afterAdCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void AdDismissedCallback()
        {
            _adDismissedCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void AdViewedCallback()
        {
            _adViewedCallback?.Invoke();
        }
#endregion JSlib Interface
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
