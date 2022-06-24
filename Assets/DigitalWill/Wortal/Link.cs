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

        public void ShowRewardedAd(string name)
        {
            ShowRewardedAdLink(
                name,
                BeforeAdCallback,
                AfterAdCallback,
                AdDismissedCallback,
                AdViewedCallback,
                NoShowCallback);
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
            Debug.Log("[Wortal] BeforeAdCallback");
            Wortal.CallBeforeAd();
        }

        [MonoPInvokeCallback(typeof(AfterAdDelegate))]
        private static void AfterAdCallback()
        {
            Debug.Log("[Wortal] AfterAdCallback");
            Wortal.CallAdDone();
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
            Debug.Log("[Wortal] NoShowCallback");
            Wortal.CallAdTimedOut();
        }
    }
}
