using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace DigitalWill.H5Portal
{
    /// <summary>
    /// Handles ads provided by Rakuten Games Link.
    /// </summary>
    internal class Link : IAdProvider
    {
        private static bool _isAdAvailable;
        public bool IsAdAvailable => _isAdAvailable;

        private delegate void BeforeAdDelegate();
        private delegate void AfterAdDelegate();
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
                    Debug.LogError("[Wortal] Link.ShowInterstitialAd was called with an invalid type.");
                    return;
            }

            ShowInterstitialAdLink(
                typeArg,
                name,
                BeforeAdCallback,
                AfterAdCallback,
                NoShowCallback);
        }

        public void RequestRewardedAd(string name)
        {
            ShowRewardedAdLink(
                name,
                BeforeAdCallback,
                AfterAdCallback,
                AdDismissedCallback,
                AdViewedCallback,
                NoShowCallback);
        }

        public void ShowRewardedAd()
        {
        }

        [DllImport("__Internal")]
        private static extern void TriggerBeforeAdLink();

        [DllImport("__Internal")]
        private static extern void TriggerAfterAdLink();

        [DllImport("__Internal")]
        private static extern void TriggerAdDismissedLink();

        [DllImport("__Internal")]
        private static extern void TriggerAdViewedAdLink();

        [DllImport("__Internal")]
        private static extern void TriggerNoShowAdLink();

        [DllImport("__Internal")]
        private static extern void ShowInterstitialAdLink(string type, string name,
                                                             BeforeAdDelegate beforeAdCallback,
                                                             AfterAdDelegate afterAdCallback,
                                                             NoShowDelegate noShowDelegate);

        [DllImport("__Internal")]
        private static extern void ShowRewardedAdLink(string name, BeforeAdDelegate beforeAdCallback,
                                                            AfterAdDelegate afterAdCallback,
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
