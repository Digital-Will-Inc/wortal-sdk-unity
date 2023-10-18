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
        private static Action _beforeAdCallback;
        private static Action _afterAdCallback;
        private static Action _adDismissedCallback;
        private static Action _adViewedCallback;
        private static Action _noFillCallback;

#region Public API

        /// <summary>
        /// Returns whether ads are blocked for the current session. This can be used to determine if an alternative flow should
        /// be used instead of showing ads, or prompt the player to disable the ad blocker.
        /// </summary>
        /// <returns>True if ads are blocked for the current session. False if ads are not blocked.</returns>
        /// <example><code>
        /// if (Wortal.Ads.IsAdBlocked())
        /// {
        ///     // Show a message to the player to disable their ad blocker.
        ///     // Or use an alternative flow that doesn't require ads - social invites for rewards as an example.
        /// }
        /// </code></example>
        public bool IsAdBlocked()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return IsAdBlockedJS();
#else
            Debug.Log("[Wortal] Mock Ads.IsAdBlocked()");
            return true;
#endif
        }

        /// <summary>
        /// Shows an interstitial ad. These can be shown at various points in the game such as a level end, restart or a timed
        /// interval in games with longer levels.
        /// </summary>
        /// <param name="placement">Placement type for the ad.</param>
        /// <param name="description">Description of the placement.</param>
        /// <param name="beforeAdCallback">Callback for before the ad is shown. Pause the game here.</param>
        /// <param name="afterAdCallback">Callback for after the ad is shown. Resume the game here.</param>
        /// <param name="noFillCallback">Callback for when no ad is filled. Resume the game here.</param>
        /// <example><code>
        /// Wortal.Ads.ShowInterstitial(Placement.Next, "NextLevel",
        ///     () => PauseGame(),
        ///     () => ResumeGame());
        /// </code></example>
        /// <throws><ul>
        /// <li>INVALID_PARAM</li>
        /// </ul></throws>
        public void ShowInterstitial(Placement placement,
                                     string description,
                                     Action beforeAdCallback,
                                     Action afterAdCallback,
                                     Action noFillCallback = null)
        {
            _beforeAdCallback = beforeAdCallback;
            _afterAdCallback = afterAdCallback;
            _noFillCallback = noFillCallback ?? afterAdCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            ShowInterstitialJS(
                placement.ToString().ToLower(),
                description,
                BeforeAdCallback,
                AfterAdCallback,
                NoFillCallback);
#else
            Debug.Log($"[Wortal] Mock Ads.ShowInterstitial({placement}, {description})");
            BeforeAdCallback();
            AfterAdCallback();
#endif
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
        /// <param name="noFillCallback">Callback for when no ad is filled. Resume the game here.</param>
        /// <remarks>When calling in editor for testing, passing "dismiss" for description will trigger the adDismissedCallback. Any other description will trigger adViewedCallback.</remarks>
        /// <example><code>
        /// Wortal.Ads.ShowRewarded("ReviveAndContinue",
        ///     () => PauseGame(),
        ///     () => ResumeGame(),
        ///     () => DontReward(),
        ///     () => RewardPlayer());
        ///</code></example>
        /// <throws><ul>
        /// <li>INVALID_PARAM</li>
        /// </ul></throws>
        public void ShowRewarded(string description,
                                 Action beforeAdCallback,
                                 Action afterAdCallback,
                                 Action adDismissedCallback,
                                 Action adViewedCallback,
                                 Action noFillCallback = null)
        {
            _beforeAdCallback = beforeAdCallback;
            _afterAdCallback = afterAdCallback;
            _adDismissedCallback = adDismissedCallback;
            _adViewedCallback = adViewedCallback;
            _noFillCallback = noFillCallback ?? afterAdCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            ShowRewardedJS(
                description,
                BeforeAdCallback,
                AfterAdCallback,
                AdDismissedCallback,
                AdViewedCallback,
                NoFillCallback);
#else
            Debug.Log($"[Wortal] Mock Ads.ShowRewarded({description})");
            BeforeAdCallback();
            AfterAdCallback();
            if (description == "dismiss")
            {
                AdDismissedCallback();
            }
            else
            {
                AdViewedCallback();
            }
#endif
        }

        /// <summary>
        /// Shows a banner ad. These are small ads that are shown at the top or bottom of the screen.
        /// They are typically used on menus or other non-gameplay screens. They can be shown or hidden at any time.
        /// </summary>
        /// <param name="shouldShow">Whether the banner should be shown or hidden. Default is show.</param>
        /// <param name="position">Where the banner should be shown. Top or bottom of the screen. Default is the bottom.</param>
        public void ShowBanner(bool shouldShow = true, BannerPosition position = BannerPosition.Bottom)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ShowBannerJS(shouldShow, position.ToString().ToLower());
#else
            Debug.Log($"[Wortal] Mock Ads.ShowBanner({shouldShow}, {position})");
#endif
        }

#endregion Public API
#region JSlib Interface

        [DllImport("__Internal")]
        private static extern bool IsAdBlockedJS();

        [DllImport("__Internal")]
        private static extern void ShowInterstitialJS(string type,
                                                      string description,
                                                      Action beforeAdCallback,
                                                      Action afterAdCallback,
                                                      Action noFillCallback);

        [DllImport("__Internal")]
        private static extern void ShowRewardedJS(string description,
                                                  Action beforeAdCallback,
                                                  Action afterAdCallback,
                                                  Action adDismissedCallback,
                                                  Action adViewedCallback,
                                                  Action noFillCallback);

        [DllImport("__Internal")]
        private static extern void ShowBannerJS(bool shouldShow, string position);

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

        [MonoPInvokeCallback(typeof(Action))]
        private static void NoFillCallback()
        {
            _noFillCallback?.Invoke();
        }

#endregion JSlib Interface
    }
}
