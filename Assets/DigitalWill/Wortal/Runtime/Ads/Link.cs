using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace DigitalWill
{
    /// <summary>
    /// Provides ads for games deployed to the Link Game Platform.
    /// </summary>
    internal class Link : IAdProvider
    {
        private delegate void BeforeAdDelegate();
        private delegate void AfterAdDelegate();
        private delegate void AdDismissedDelegate();
        private delegate void AdViewedDelegate();
        private delegate void NoShowDelegate();

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
                    Debug.LogError("[Wortal] Link.ShowInterstitialAd was called with an invalid type.");
                    return;
            }

            ShowInterstitialAdLink(
                typeArg,
                Wortal.Settings.LinkInterstitialId,
                description,
                BeforeAdCallback,
                AfterAdCallback,
                NoShowCallback);
        }

        public void ShowRewardedAd(string description)
        {
            ShowRewardedAdLink(
                Wortal.Settings.LinkRewardedId,
                description,
                BeforeAdCallback,
                AfterAdCallback,
                AdDismissedCallback,
                AdViewedCallback,
                NoShowCallback);
        }

        [DllImport("__Internal")]
        private static extern void ShowInterstitialAdLink(string type, string adUnitId, string description,
                                                             BeforeAdDelegate beforeAdCallback,
                                                             AfterAdDelegate afterAdCallback,
                                                             NoShowDelegate noShowDelegate);

        [DllImport("__Internal")]
        private static extern void ShowRewardedAdLink(string adUnitId, string description, BeforeAdDelegate beforeAdCallback,
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
