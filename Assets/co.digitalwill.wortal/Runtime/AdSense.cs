using System;
using UnityEngine;

#if UNITY_WEBGL && !UNITY_EDITOR
using AFG.Api;
using AOT;
#endif

namespace DigitalWill.Wortal
{
    /// <summary>
    /// Handles calls to the AFG plugin to serve ads and delegates for the callback functions.
    /// </summary>
    public class AdSense
    {
#pragma warning disable 67
        /// <summary>
        /// Subscribe to be notified when a call is made to retrieve an ad. This is useful for starting a timer to
        /// check for error conditions and prevent the game from waiting indefinitely.
        /// </summary>
        public static event Action AdCalled;
#pragma warning restore 67

        /// <summary>
        /// Was an ad successfully returned or not. This gets set in the beforeAd callback and checked in Wortal.cs
        /// on a timer to ensure we don't get stuck in an infinite loop waiting for an due to an error.
        /// </summary>
        public static bool IsAdAvailable { get; private set; }

        /// <summary>
        /// Shows an interstitial ad.
        /// </summary>
        /// <param name="type">Type of ad placement. Corresponds with Google's AdSense placement types.</param>
        /// <param name="name">Name of the ad placement.</param>
        public static void ShowInterstitialAd(Placement type, string name)
        {
            IsAdAvailable = false;
            AdCalled?.Invoke();

#if UNITY_WEBGL && !UNITY_EDITOR
            string afgType;
            switch (type)
            {
                case Placement.Start:
                    afgType = InterstitialAdType.Start;
                    break;
                case Placement.Next:
                    afgType = InterstitialAdType.Next;
                    break;
                case Placement.Pause:
                    afgType = InterstitialAdType.Pause;
                    break;
                case Placement.Browse:
                    afgType = InterstitialAdType.Browse;
                    break;
                default:
                    Debug.LogError("AdSense.ShowInterstitialAd was called with an invalid type.");
                    return;
            }

            AdBreak.ShowInterstitialAd(
                afgType,
                name,
                BeforeAdCallback,
                AfterAdCallback,
                AdBreakDoneCallback);
#endif
        }

        /// <summary>
        /// Requests a rewarded ad.
        /// </summary>
        /// <param name="name">Name of the ad to be shown.</param>
        public static void RequestRewardedAd(string name)
        {
            IsAdAvailable = false;
            AdCalled?.Invoke();

#if UNITY_WEBGL && !UNITY_EDITOR
            AdBreak.RequestRewardedAd(
                name,
                BeforeAdCallback,
                AfterAdCallback,
                AdBreakDoneCallback,
                BeforeRewardCallback,
                AdDismissedCallback,
                AdViewedCallback);
#endif
        }

        /// <summary>
        /// Shows a rewarded ad. Should be used in the BeforeRewardCallback to be called after RequestRewardedAd has
        /// returned an ad to show, but we provide an API for it anyways for situations where we might want to do
        /// something in the game before beginning the ad.
        /// </summary>
        public static void ShowRewardedAd()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            // We don't invoke AdCalled here because there is no BeforeAdCallback attached to this method.
            AdBreak.ShowRewardedAd();
#endif
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        [MonoPInvokeCallback(typeof(AdBreak.BeforeAdDelegate))]
        private static void BeforeAdCallback()
        {
            // Called only if the ad is available and before the ad is displayed.
            // You should pause or stop the game flow here to ensure anything
            // doesn't change while the ad is being displayed.
            // We set this flag to let Wortal.cs know that we reached the BeforeAdCallback so we have an ad returned.
            IsAdAvailable = true;
            Debug.Log("Before Ad");


        }

        [MonoPInvokeCallback(typeof(AdBreak.AfterAdDelegate))]
        private static void AfterAdCallback()
        {
            // Called only if the ad is displayed after the ad is finished (for any reason).
            Debug.Log("After Ad");


        }

        [MonoPInvokeCallback(typeof(AdBreak.AdBreakDoneDelegate))]
        private static void AdBreakDoneCallback()
        {
            // Called after the AdBreak finished, regardless if an ad was
            // displayed or not. You can resume or restart your game inside
            // this callback.
            // We make sure to set this here too in case we don't get an ad, but still receive a response from AdSense.
            IsAdAvailable = true;
            Debug.Log("AdBreak Done");


        }

        [MonoPInvokeCallback(typeof(AdBreak.BeforeRewardDelegate))]
        private static void BeforeRewardCallback()
        {
            // Called if a rewarded ad is available. You should show a dialog/pop-up
            // to ask if the user wants to see a rewarded ad or not. If the user agrees
            // to it you can show it with calling AdBreak.ShowRewardedAd();
            Debug.Log("Before Reward Ad");


        }

        [MonoPInvokeCallback(typeof(AdBreak.AdDismissedDelegate))]
        private static void AdDismissedCallback()
        {
            // Called only for rewarded ads when the player dismisses the ad.
            // It is only called if the player dismisses the ad before it completes.
            // In this case the reward should not be granted.
            Debug.Log("Ad Dismissed");


        }

        [MonoPInvokeCallback(typeof(AdBreak.AdViewedDelegate))]
        private static void AdViewedCallback()
        {
            // Called only for rewarded ads when the player completes the ad
            // and should be granted the reward.
            Debug.Log("Ad Viewed");


        }
#endif
    }

    /// <summary>
    /// Ad placement type defined by Google AdSense. https://developers.google.com/ad-placement/docs/placement-types
    /// </summary>
    public enum Placement
    {
        /// <summary>
        /// Used when ad is shown before next level, restart, revive, etc.
        /// </summary>
        Next,
        /// <summary>
        /// Used only at the very beginning when the game is first loading. For preroll ads.
        /// </summary>
        Start,
        /// <summary>
        /// Used when an ad is shown after the player pauses the game.
        /// </summary>
        Pause,
        /// <summary>
        /// Used when an ad is shown outside of gameplay. Such as a player browser character skin options, etc.
        /// </summary>
        Browse,
    }
}
