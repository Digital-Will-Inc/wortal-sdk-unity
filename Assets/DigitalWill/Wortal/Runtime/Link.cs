using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace DigitalWill
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

        /// <summary>
        /// Shows an interstitial ad for the Link platform.
        /// </summary>
        /// <param name="type">Type of ad placement.</param>
        /// <param name="placementId">Ad placement ID. Not required for Link ads, value will not be used.</param>
        /// <remarks>The Link ad placement ID is set in the Wortal settings and this method derives its value from there. Any placementId passed in here will be ignored.</remarks>
        public void ShowInterstitialAd(Placement type, string placementId = null)
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
                BeforeAdCallback,
                AfterAdCallback,
                NoShowCallback);
        }

        /// <summary>
        /// Shows a rewarded ad for the Link platform.
        /// </summary>
        /// <param name="placementId">Ad placement ID. Not required for Link ads, value will not be used.</param>
        /// <remarks>The Link ad placement ID is set in the Wortal settings and this method derives its value from there. Any placementId passed in here will be ignored.</remarks>
        public void ShowRewardedAd(string placementId = null)
        {
            ShowRewardedAdLink(
                Wortal.Settings.LinkRewardedId,
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
        private static extern void TriggerAdViewedLink();

        [DllImport("__Internal")]
        private static extern void TriggerNoShowLink();

        [DllImport("__Internal")]
        private static extern void ShowInterstitialAdLink(string type, string placementId,
                                                             BeforeAdDelegate beforeAdCallback,
                                                             AfterAdDelegate afterAdCallback,
                                                             NoShowDelegate noShowDelegate);

        [DllImport("__Internal")]
        private static extern void ShowRewardedAdLink(string placementId, BeforeAdDelegate beforeAdCallback,
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
