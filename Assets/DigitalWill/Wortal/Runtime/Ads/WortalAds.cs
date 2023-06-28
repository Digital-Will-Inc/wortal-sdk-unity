﻿using System;
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

#endregion Public API
#region JSlib Interface

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