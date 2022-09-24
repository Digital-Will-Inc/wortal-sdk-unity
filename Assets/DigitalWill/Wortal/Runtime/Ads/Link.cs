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
            string adUnitId = Wortal.LinkInterstitialId;
            if (string.IsNullOrEmpty(adUnitId))
            {
                Debug.LogError("[Wortal] Link interstitial AdUnit ID missing or invalid. No ads will be shown.");
                Wortal.InvokeAfterAd();
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

            ShowInterstitialAd(
                typeArg,
                adUnitId,
                description,
                BeforeAdCallback,
                AfterAdCallback,
                NoShowCallback);
        }

        public void ShowRewardedAd(string description)
        {
            string adUnitId = Wortal.LinkRewardedId;
            if (string.IsNullOrEmpty(adUnitId))
            {
                Debug.LogError("[Wortal] Link rewarded AdUnit ID missing or invalid. No ads will be shown.");
                Wortal.InvokeAfterAd();
                return;
            }

            RequestRewardedAd(
                adUnitId,
                description,
                BeforeAdCallback,
                AfterAdCallback,
                AdDismissedCallback,
                AdViewedCallback,
                NoShowCallback);
        }

        [DllImport("__Internal")]
        private static extern void ShowInterstitialAd(string type, string adUnitId, string description,
                                                             IAdProvider.BeforeAdDelegate beforeAdCallback,
                                                             IAdProvider.AfterAdDelegate afterAdCallback,
                                                             IAdProvider.NoShowDelegate noShowDelegate);

        [DllImport("__Internal")]
        private static extern void RequestRewardedAd(string adUnitId, string description, IAdProvider.BeforeAdDelegate beforeAdCallback,
                                                     IAdProvider.AfterAdDelegate afterAdCallback,
                                                     IAdProvider.AdDismissedDelegate adDismissedDelegate,
                                                     IAdProvider.AdViewedDelegate adViewedDelegate,
                                                     IAdProvider.NoShowDelegate noShowDelegate);

        [MonoPInvokeCallback(typeof(IAdProvider.BeforeAdDelegate))]
        private static void BeforeAdCallback()
        {
            Debug.Log("[Wortal] BeforeAdCallback");
            Wortal.InvokeBeforeAd();
        }

        [MonoPInvokeCallback(typeof(IAdProvider.AfterAdDelegate))]
        private static void AfterAdCallback()
        {
            Debug.Log("[Wortal] AfterAdCallback");
            Wortal.InvokeAfterAd();
        }

        [MonoPInvokeCallback(typeof(IAdProvider.AdDismissedDelegate))]
        private static void AdDismissedCallback()
        {
            Debug.Log("[Wortal] AdDismissedCallback");
            Wortal.InvokeRewardSkipped();
        }

        [MonoPInvokeCallback(typeof(IAdProvider.AdViewedDelegate))]
        private static void AdViewedCallback()
        {
            Debug.Log("[Wortal] AdViewedCallback");
            Wortal.InvokeRewardPlayer();
        }

        [MonoPInvokeCallback(typeof(IAdProvider.NoShowDelegate))]
        private static void NoShowCallback()
        {
            Debug.Log("[Wortal] NoShowCallback");
            Wortal.InvokeAfterAd();
        }
    }
}
