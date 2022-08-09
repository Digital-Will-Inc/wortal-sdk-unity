using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;
using AOT;

namespace DigitalWill
{
    /// <summary>
    /// Handles ads provided by AdSense.
    /// </summary>
    internal class AdSense : IAdProvider
    {
        private static bool _isAdEventTriggered;

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

        public void ShowRewardedAd(string name)
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
            Debug.Log("[Wortal] BeforeAdCallback");
            Wortal.CallBeforeAd();
        }

        [MonoPInvokeCallback(typeof(AfterAdDelegate))]
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
            Wortal.CallAdDone();

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
            ShowRewardedAdAdSense();
        }

        [MonoPInvokeCallback(typeof(AdDismissedDelegate))]
        private static void AdDismissedCallback()
        {
            Debug.Log("[Wortal] AdDismissedCallback");
            Wortal.CallAdDismissed();
        }

        [MonoPInvokeCallback(typeof(AdViewedDelegate))]
        private static void AdViewedCallback()
        {
            Debug.Log("[Wortal] AdViewedCallback");
            Wortal.CallAdViewed();
        }

        [MonoPInvokeCallback(typeof(NoShowDelegate))]
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
            Wortal.CallAdTimedOut();
        }
    }
}
