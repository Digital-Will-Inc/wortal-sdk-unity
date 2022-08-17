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
        public void ShowInterstitialAd(Placement type, string description)
        {
            string adUnitId = Wortal.Settings.LinkInterstitialId;
            if (string.IsNullOrEmpty(adUnitId))
            {
                Debug.LogError("[Wortal] Link interstitial AdUnit ID missing or invalid. No ads will be shown.");
                return;
            }

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
                adUnitId,
                description,
                BeforeAdCallback,
                AfterAdCallback,
                NoShowCallback);
        }

        public void ShowRewardedAd(string description)
        {
            string adUnitId = Wortal.Settings.LinkRewardedId;
            if (string.IsNullOrEmpty(adUnitId))
            {
                Debug.LogError("[Wortal] Link rewarded AdUnit ID missing or invalid. No ads will be shown.");
                return;
            }

            ShowRewardedAdLink(
                adUnitId,
                description,
                BeforeAdCallback,
                AfterAdCallback,
                AdDismissedCallback,
                AdViewedCallback,
                NoShowCallback);
        }

        [DllImport("__Internal")]
        private static extern void ShowInterstitialAdLink(string type, string adUnitId, string description,
                                                             IAdProvider.BeforeAdDelegate beforeAdCallback,
                                                             IAdProvider.AfterAdDelegate afterAdCallback,
                                                             IAdProvider.NoShowDelegate noShowDelegate);

        [DllImport("__Internal")]
        private static extern void ShowRewardedAdLink(string adUnitId, string description, IAdProvider.BeforeAdDelegate beforeAdCallback,
                                                            IAdProvider.AfterAdDelegate afterAdCallback,
                                                            IAdProvider.AdDismissedDelegate adDismissedDelegate,
                                                            IAdProvider.AdViewedDelegate adViewedDelegate,
                                                            IAdProvider.NoShowDelegate noShowDelegate);

        [MonoPInvokeCallback(typeof(IAdProvider.BeforeAdDelegate))]
        private static void BeforeAdCallback()
        {
            Debug.Log("[Wortal] BeforeAdCallback");
            Wortal.CallBeforeAd();
        }

        [MonoPInvokeCallback(typeof(IAdProvider.AfterAdDelegate))]
        private static void AfterAdCallback()
        {
            Debug.Log("[Wortal] AfterAdCallback");
            Wortal.CallAfterAd();
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
            Debug.Log("[Wortal] NoShowCallback");
            Wortal.CallAfterAd();
        }
    }
}
