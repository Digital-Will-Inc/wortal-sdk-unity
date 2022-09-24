using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;
using AOT;

namespace DigitalWill
{
    /// <summary>
    /// Provides ads for games deployed to the Wortal. These ads are served by Google AdSense.
    /// </summary>
    internal class AdSense : IAdProvider
    {
        private static bool _isAdEventTriggered;

        private delegate void AdBreakDoneDelegate();
        private delegate void BeforeRewardDelegate();

        public void ShowInterstitialAd(Placement type, string description)
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

            ShowInterstitialAd(
                typeArg,
                "",
                description,
                BeforeAdCallback,
                AfterAdCallback,
                NoShowCallback,
                AdBreakDoneCallback);
        }

        public void ShowRewardedAd(string description)
        {
            RequestRewardedAd(
                description,
                "",
                BeforeAdCallback,
                AfterAdCallback,
                AdDismissedCallback,
                AdViewedCallback,
                NoShowCallback,
                BeforeRewardCallback,
                AdBreakDoneCallback);
        }

        [DllImport("__Internal")]
        private static extern void ShowRewardedAd();

        [DllImport("__Internal")]
        private static extern void ShowInterstitialAd(string type, string adUnitId, string description,
                                                      IAdProvider.BeforeAdDelegate beforeAdCallback,
                                                      IAdProvider.AfterAdDelegate afterAdCallback,
                                                      IAdProvider.NoShowDelegate noShowDelegate,
                                                      AdBreakDoneDelegate adBreakDoneDelegate);

        [DllImport("__Internal")]
        private static extern void RequestRewardedAd(string adUnitId, string description,
                                                     IAdProvider.BeforeAdDelegate beforeAdCallback,
                                                     IAdProvider.AfterAdDelegate afterAdCallback,
                                                     IAdProvider.AdDismissedDelegate adDismissedDelegate,
                                                     IAdProvider.AdViewedDelegate adViewedDelegate,
                                                     IAdProvider.NoShowDelegate noShowDelegate,
                                                     BeforeRewardDelegate beforeRewardDelegate,
                                                     AdBreakDoneDelegate adBreakDoneDelegate);

        [MonoPInvokeCallback(typeof(IAdProvider.BeforeAdDelegate))]
        private static void BeforeAdCallback()
        {
            Debug.Log("[Wortal] BeforeAdCallback");
            Wortal.CallBeforeAd();
        }

        [MonoPInvokeCallback(typeof(IAdProvider.AfterAdDelegate))]
        private static void AfterAdCallback()
        {
            // We don't trigger an event here because it's redundant. If this is called, AdBreakDone will be called
            // immediately after.
            Debug.Log("[Wortal] AfterAdCallback");
        }

        [MonoPInvokeCallback(typeof(AdBreakDoneDelegate))]
        private static void AdBreakDoneCallback()
        {
            Debug.Log("[Wortal] AdBreakDoneCallback");
            Wortal.CallAfterAd();

            // If AdSense doesn't serve an ad, we'll still receive this callback, but the Wortal SDK will have
            // triggered a 500ms timeout before it triggers the NoShow callback. We set this flag to avoid firing
            // both AdDone and AdTimedOut events.
            _isAdEventTriggered = true;
            Task.Delay(510).ContinueWith(_ => _isAdEventTriggered = false);
        }

        [MonoPInvokeCallback(typeof(BeforeRewardDelegate))]
        private static void BeforeRewardCallback()
        {
            // We could delay here and give the dev or player the opportunity to start the ad, but we'll keep
            // it simple for now and just show the ad.
            Debug.Log("[Wortal] BeforeRewardCallback");
            ShowRewardedAd();
        }

        [MonoPInvokeCallback(typeof(IAdProvider.AdDismissedDelegate))]
        private static void AdDismissedCallback()
        {
            Debug.Log("[Wortal] AdDismissedCallback");
            Wortal.CallAdDismissed();
        }

        [MonoPInvokeCallback(typeof(IAdProvider.AdViewedDelegate))]
        private static void AdViewedCallback()
        {
            Debug.Log("[Wortal] AdViewedCallback");
            Wortal.CallAdViewed();
        }

        [MonoPInvokeCallback(typeof(IAdProvider.NoShowDelegate))]
        private static void NoShowCallback()
        {
            // We check this because if AdSense decides not to show the player an ad, we'll receive an AdBreakDone
            // callback and then a NoShow callback shortly after when the Wortal SDK calls it an ad timeout.
            // This can be an issue for games that are subscribing to both events to continue play after an ad.
            if (_isAdEventTriggered)
            {
                Debug.Log("[Wortal] AdBreakDoneCallback still active, skipping NoShowCallback.");
                return;
            }

            Debug.Log("[Wortal] NoShowCallback");
            Wortal.CallAfterAd();
        }
    }
}
