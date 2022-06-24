using System.Runtime.InteropServices;
using UnityEngine;
using AOT;

namespace DigitalWill.H5Portal
{
    /// <summary>
    /// Handles ads provided by AdSense.
    /// </summary>
    internal class AdSense : IAdProvider
    {
        private static bool _isAdAvailable;
        public bool IsAdAvailable => _isAdAvailable;

        private delegate void BeforeAdDelegate();
        private delegate void AfterAdDelegate();
        private delegate void AdBreakDoneDelegate();
        private delegate void BeforeRewardDelegate();
        private delegate void AdDismissedDelegate();
        private delegate void AdViewedDelegate();
        private delegate void NoShowDelegate();

        public void ShowInterstitialAd(Placement type, string name)
        {
            string typeArg;
            switch (type)
            {
                case Placement.Start:
                    typeArg = "start";
                    break;
                case Placement.Next:
                    typeArg = "next";
                    break;
                case Placement.Pause:
                    typeArg = "pause";
                    break;
                case Placement.Browse:
                    typeArg = "browse";
                    break;
                default:
                    Debug.LogError("[Wortal] AdSense.ShowInterstitialAd was called with an invalid type.");
                    return;
            }

            ShowInterstitialAdAdSense(
                typeArg,
                name,
                BeforeAdCallback,
                AfterAdCallback,
                AdBreakDoneCallback,
                NoShowCallback);
        }

        public void RequestRewardedAd(string name)
        {
            RequestRewardedAdAdSense(
                name,
                BeforeAdCallback,
                AfterAdCallback,
                AdBreakDoneCallback,
                BeforeRewardCallback,
                AdDismissedCallback,
                AdViewedCallback,
                NoShowCallback);
        }

        public void ShowRewardedAd()
        {
            // We don't invoke AdCalled here because there is no BeforeAdCallback attached to this method.
            ShowRewardedAdAdSense();
        }

        [DllImport("__Internal")]
        private static extern void TriggerBeforeAdAdSense();

        [DllImport("__Internal")]
        private static extern void TriggerAfterAdAdSense();

        [DllImport("__Internal")]
        private static extern void TriggerAdBreakDoneAdSense();

        [DllImport("__Internal")]
        private static extern void TriggerBeforeRewardAdSense();

        [DllImport("__Internal")]
        private static extern void TriggerAdDismissedAdSense();

        [DllImport("__Internal")]
        private static extern void TriggerAdViewedAdSense();

        [DllImport("__Internal")]
        private static extern void TriggerNoShowAdSense();

        [DllImport("__Internal")]
        private static extern void ShowRewardedAdAdSense();

        [DllImport("__Internal")]
        private static extern void ShowInterstitialAdAdSense(string type, string name,
                                                             BeforeAdDelegate beforeAdCallback,
                                                             AfterAdDelegate afterAdCallback,
                                                             AdBreakDoneDelegate adBreakDoneDelegate,
                                                             NoShowDelegate noShowDelegate);

        [DllImport("__Internal")]
        private static extern void RequestRewardedAdAdSense(string name, BeforeAdDelegate beforeAdCallback,
                                                            AfterAdDelegate afterAdCallback,
                                                            AdBreakDoneDelegate adBreakDoneDelegate,
                                                            BeforeRewardDelegate beforeRewardDelegate,
                                                            AdDismissedDelegate adDismissedDelegate,
                                                            AdViewedDelegate adViewedDelegate,
                                                            NoShowDelegate noShowDelegate);

        [MonoPInvokeCallback(typeof(BeforeAdDelegate))]
        private static void BeforeAdCallback()
        {
            // Called only if the ad is available and before the ad is displayed.
            // You should pause or stop the game flow here to ensure anything
            // doesn't change while the ad is being displayed.
            // We set this flag to let Wortal.cs know that we reached the BeforeAdCallback so we have an ad returned.
            _isAdAvailable = true;
            Debug.Log("[Wortal] BeforeAdCallback");
        }

        [MonoPInvokeCallback(typeof(AfterAdDelegate))]
        private static void AfterAdCallback()
        {
            // Called only if the ad is displayed after the ad is finished (for any reason).
            Debug.Log("[Wortal] AfterAdCallback");
        }

        [MonoPInvokeCallback(typeof(AdBreakDoneDelegate))]
        private static void AdBreakDoneCallback()
        {
            // Called after the AdBreak finished, regardless if an ad was
            // displayed or not. You can resume or restart your game inside
            // this callback.
            // We make sure to set this here too in case we don't get an ad, but still receive a response from AdSense.
            _isAdAvailable = true;
            Debug.Log("[Wortal] AdBreakDoneCallback");
        }

        [MonoPInvokeCallback(typeof(BeforeRewardDelegate))]
        private static void BeforeRewardCallback()
        {
            // Called if a rewarded ad is available. You should show a dialog/pop-up
            // to ask if the user wants to see a rewarded ad or not. If the user agrees
            // to it you can show it with calling AdBreak.ShowRewardedAd();
            Debug.Log("[Wortal] BeforeRewardCallback");
        }

        [MonoPInvokeCallback(typeof(AdDismissedDelegate))]
        private static void AdDismissedCallback()
        {
            // Called only for rewarded ads when the player dismisses the ad.
            // It is only called if the player dismisses the ad before it completes.
            // In this case the reward should not be granted.
            Debug.Log("[Wortal] AdDismissedCallback");
        }

        [MonoPInvokeCallback(typeof(AdViewedDelegate))]
        private static void AdViewedCallback()
        {
            // Called only for rewarded ads when the player completes the ad
            // and should be granted the reward.
            Debug.Log("[Wortal] AdViewedCallback");
        }

        [MonoPInvokeCallback(typeof(NoShowDelegate))]
        private static void NoShowCallback()
        {
            // Called when a timeout was reached and an ad was not returned. This can occur due to ad blockers
            // or other browser issues.
            Debug.Log("[Wortal] NoShowCallback");
        }
    }
}
