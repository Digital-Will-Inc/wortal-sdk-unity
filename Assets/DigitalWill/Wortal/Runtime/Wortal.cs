using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace DigitalWill
{
    /// <summary>
    /// Contains utility functions for interfacing with the game portal and player's browser.
    /// </summary>
    public static class Wortal
    {
        private const string LOG_PREFIX = "[Wortal] ";

        private static IAdProvider _ads;

        /// <summary>
        /// An ad was requested and successfully returned. This is fired before the ad is shown so it can be used
        /// for pausing the game, muting audio, etc.
        /// </summary>
        public static event Action BeforeAd;
        /// <summary>
        /// An ad request has finished. This does not guarantee an ad was shown, only that the request to the provider
        /// has finished and the player can resume the game now.
        /// </summary>
        public static event Action AfterAd;
        /// <summary>
        /// A rewarded ad was successfully viewed and the player should be given a reward.
        /// </summary>
        public static event Action RewardPlayer;
        /// <summary>
        /// A rewarded ad was dismissed and the player should not receive a reward.
        /// </summary>
        public static event Action RewardSkipped;

        public static string LinkInterstitialId { get; private set; }
        public static string LinkRewardedId { get; private set; }

        /// <summary>
        /// Shows an interstitial ad. Game should be paused on the BeforeAd event and resumed on the AfterAd event.
        /// </summary>
        /// <param name="type">Type of ad placement.</param>
        /// <param name="description">Description of the ad placement. Ex: "NextLevel"</param>
        public static void ShowInterstitialAd(Placement type, string description)
        {
            _ads.ShowInterstitialAd(type, description);
        }

        /// <summary>
        /// Shows a rewarded ad. Game should be paused on the BeforeAd event and resumed on the AfterAd event.
        /// Player should be rewarded on the RewardPlayer event and not on the RewardSkipped event.
        /// </summary>
        /// <param name="description">Description of the ad placement. Ex: "ReviveAndContinue"</param>
        public static void ShowRewardedAd(string description)
        {
            _ads.ShowRewardedAd(description);
        }

        internal static void InvokeBeforeAd() => BeforeAd?.Invoke();
        internal static void InvokeAfterAd() => AfterAd?.Invoke();
        internal static void InvokeRewardSkipped() => RewardSkipped?.Invoke();
        internal static void InvokeRewardPlayer() => RewardPlayer?.Invoke();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            Debug.Log(LOG_PREFIX + "Initializing Wortal SDK for Unity..");
            _ads = ParsePlatform(GetPlatform()) switch
            {
                Platform.AdSense => new AdSense(),
                Platform.Link => new Link(),
                Platform.Viber => new Viber(),
                Platform.Debug => new DebugAds(),
                _ => new DebugAds(),
            };
        }

        private static Platform ParsePlatform(string platform)
        {
            switch (platform)
            {
                case "wortal":
                    return Platform.AdSense;
                case "link":
                    GetLinkAdUnitIds(LinkAdUnitCallback);
                    return Platform.Link;
                case "viber":
                    return Platform.Viber;
                default:
                    Debug.LogWarning(LOG_PREFIX + "Could not determine platform. Switching to debug mode.");
                    return Platform.Debug;
            }
        }

        [DllImport("__Internal")]
        private static extern string GetPlatform();

        [DllImport("__Internal")]
        private static extern void GetLinkAdUnitIds(Action<string, string> callback);

        [MonoPInvokeCallback(typeof(Action<string, string>))]
        private static void LinkAdUnitCallback(string interstitialId, string rewardedId)
        {
            if (string.IsNullOrEmpty(interstitialId) || string.IsNullOrEmpty(rewardedId))
                Debug.LogWarning(LOG_PREFIX + "Link AdUnit IDs invalid or missing. Ad calls will not be made.");

            LinkInterstitialId = interstitialId;
            LinkRewardedId = rewardedId;
            Debug.Log(LOG_PREFIX + "Link AdUnitIds fetched.");
        }
    }

    public enum Platform
    {
        Debug,
        AdSense,
        Link,
        Viber,
    }
}
