using System;
#if UNITY_WEBGL
using System.Runtime.InteropServices;
using AOT;
#endif
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
#if UNITY_WEBGL
            return IsAdBlockedJS();
#elif UNITY_EDITOR
            Debug.Log("[Wortal] Mock Ads.IsAdBlocked()");
            return true;
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Ads.IsAdBlocked not supported on Android. Returning false.");
            return false;
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Ads.IsAdBlocked not supported on iOS. Returning false.");
            return false;
#else
            Debug.LogWarning("[Wortal] Ads.IsAdBlocked not supported on this platform. Returning false.");
            return false;
#endif
        }

        /// <summary>
        /// Returns whether ads are enabled for the current session. This can be used to determine if an alternative flow should
        /// be used instead of showing ads
        /// </summary>
        /// <returns>True if ads are enabled for the current session. False if ads are disabled.</returns>
        /// <example><code>
        /// if (Wortal.Ads.IsEnabled())
        /// {
        ///     // Show ad
        ///     // Show ad button
        /// }
        /// </code></example>
        public bool IsEnabled()
        {
#if UNITY_WEBGL
            return IsEnabledJS();
#elif UNITY_EDITOR
            Debug.Log("[Wortal] Mock Ads.IsEnabled()");
            return false;
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Ads.IsEnabled not supported on Android. Returning false.");
            return false;
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Ads.IsEnabled not supported on iOS. Returning false.");
            return false;
#else
            Debug.LogWarning("[Wortal] Ads.IsEnabled not supported on this platform. Returning false.");
            return false;
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
#if UNITY_WEBGL
            ShowInterstitialJS(
                placement.ToString().ToLower(),
                description,
                BeforeAdCallback,
                AfterAdCallback,
                NoFillCallback);
#elif UNITY_EDITOR
            Debug.Log($"[Wortal] Mock Ads.ShowInterstitial({placement}, {description})");
            _beforeAdCallback?.Invoke();
            _afterAdCallback?.Invoke();
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Ads.ShowInterstitial({placement}, {description}) not supported on Android.");
            _beforeAdCallback?.Invoke();
            _noFillCallback?.Invoke();
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Ads.ShowInterstitial({placement}, {description}) not supported on iOS.");
            _beforeAdCallback?.Invoke();
            _noFillCallback?.Invoke();
#else
            Debug.LogWarning($"[Wortal] Ads.ShowInterstitial({placement}, {description}) not supported on this platform.");
            _beforeAdCallback?.Invoke();
            _noFillCallback?.Invoke();
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
#if UNITY_WEBGL
            ShowRewardedJS(
                description,
                BeforeAdCallback,
                AfterAdCallback,
                AdDismissedCallback,
                AdViewedCallback,
                NoFillCallback);
#elif UNITY_EDITOR
            Debug.Log($"[Wortal] Mock Ads.ShowRewarded({description})");
            _beforeAdCallback?.Invoke();
            _afterAdCallback?.Invoke();
            if (description == "dismiss")
            {
                _adDismissedCallback?.Invoke();
            }
            else
            {
                _adViewedCallback?.Invoke();
            }
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Ads.ShowRewarded({description}) not supported on Android.");
            _beforeAdCallback?.Invoke();
            _adDismissedCallback?.Invoke(); // Or _noFillCallback, depending on desired behavior for "not supported"
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Ads.ShowRewarded({description}) not supported on iOS.");
            _beforeAdCallback?.Invoke();
            _adDismissedCallback?.Invoke(); // Or _noFillCallback
#else
            Debug.LogWarning($"[Wortal] Ads.ShowRewarded({description}) not supported on this platform.");
            _beforeAdCallback?.Invoke();
            _adDismissedCallback?.Invoke(); // Or _noFillCallback
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
#if UNITY_WEBGL
            ShowBannerJS(shouldShow, position.ToString().ToLower());
#elif UNITY_EDITOR
            Debug.Log($"[Wortal] Mock Ads.ShowBanner({shouldShow}, {position})");
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Ads.ShowBanner({shouldShow}, {position}) not supported on Android.");
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Ads.ShowBanner({shouldShow}, {position}) not supported on iOS.");
#else
            Debug.LogWarning($"[Wortal] Ads.ShowBanner({shouldShow}, {position}) not supported on this platform.");
#endif
        }

#endregion Public API
#region JSlib Interface

#if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern bool IsAdBlockedJS();

        [DllImport("__Internal")]
        private static extern bool IsEnabledJS();        

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
#endif

#if UNITY_WEBGL
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
#endif

#endregion JSlib Interface
    }
}
