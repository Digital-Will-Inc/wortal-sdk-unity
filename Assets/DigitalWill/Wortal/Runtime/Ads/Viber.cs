using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace DigitalWill
{
	/// <summary>
	/// Provides ads for games deployed to Viber Play.
	/// </summary>
	public class Viber : IAdProvider
	{
        private delegate void BeforeAdDelegate();
        private delegate void AfterAdDelegate();
        private delegate void AdDismissedDelegate();
        private delegate void AdViewedDelegate();
        private delegate void NoShowDelegate();

        public void ShowInterstitialAd(Placement type, string description)
        {
            Debug.Log("[Wortal] Ads currently not supported on Viber.");
            Wortal.CallBeforeAd();
            Wortal.CallAdDone();

            //TODO: finish implementation when Viber ads are supported
/*
            string adUnitId = "id";

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

            ShowInterstitialAdViber(
                typeArg,
                adUnitId,
                description,
                BeforeAdCallback,
                AfterAdCallback,
                NoShowCallback);
*/
        }

        public void ShowRewardedAd(string description)
        {
            Debug.Log("[Wortal] Ads currently not supported on Viber.");
            Wortal.CallBeforeAd();
            Wortal.CallAdDismissed();
            Wortal.CallAdDone();

            //TODO: finish implementation when Viber ads are supported
/*
            string adUnitId = "id";

            ShowRewardedAdViber(
                adUnitId,
                description,
                BeforeAdCallback,
                AfterAdCallback,
                AdDismissedCallback,
                AdViewedCallback,
                NoShowCallback);
*/
        }

        [DllImport("__Internal")]
        private static extern void ShowInterstitialAdViber(string type, string adUnitId, string description,
                                                             BeforeAdDelegate beforeAdCallback,
                                                             AfterAdDelegate afterAdCallback,
                                                             NoShowDelegate noShowDelegate);

        [DllImport("__Internal")]
        private static extern void ShowRewardedAdViber(string adUnitId, string description, BeforeAdDelegate beforeAdCallback,
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
