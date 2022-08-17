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
        private static WortalSettings _settings;

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
        public static event Action AdViewed;
        /// <summary>
        /// A rewarded ad was dismissed and the player should not receive a reward.
        /// </summary>
        public static event Action AdDismissed;
        /// <summary>
        /// Subscribe to be notified when the language has been parsed and set.
        /// </summary>
        public static event Action<Language> LanguageSet;

        /// <summary>
        /// Has the Language been set yet or not.
        /// </summary>
        public static bool IsLanguageSet { get; private set; }
        /// <summary>
        /// Language to be used for localization.
        /// </summary>
        public static Language Language { get; private set; }

        /// <summary>
        /// Settings asset for the wortal.
        /// </summary>
        public static WortalSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    InitSettings();
                }

                return _settings;
            }
        }

        /// <summary>
        /// Shows an interstitial ad.
        /// </summary>
        /// <param name="type">Type of ad placement.</param>
        /// <param name="name">Name of the ad placement.</param>
        public static void ShowInterstitialAd(Placement type, string name)
        {
            _ads.ShowInterstitialAd(type, name);
        }

        /// <summary>
        /// Shows a rewarded ad.
        /// </summary>
        /// <param name="name">Name of the ad to be shown.</param>
        public static void ShowRewardedAd(string name)
        {
            _ads.ShowRewardedAd(name);
        }

        /// <summary>
        /// Opens a link to a website with the URL provided. Will open in a new browser tab.
        /// </summary>
        /// <param name="url">URL to open.</param>
        internal static void OpenWebLink(string url)
        {
            // We make this internal so it hopefully isn't abused by anyone. It is used to link to an app store
            // page for a game to offer players the mobile version. We don't want games to be opening random
            // webpages via our plugin for obvious security reasons.
            OpenLink(url);
        }

        internal static void CallBeforeAd() => BeforeAd?.Invoke();
        internal static void CallAfterAd() => AfterAd?.Invoke();
        internal static void CallAdDismissed() => AdDismissed?.Invoke();
        internal static void CallAdViewed() => AdViewed?.Invoke();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            _ads = ParsePlatform(GetPlatform()) switch
            {
                Platform.AdSense => new AdSense(),
                Platform.Link => new Link(),
                Platform.Viber => new Viber(),
                Platform.Debug => new DebugAds(),
                _ => new DebugAds(),
            };

            Language = LanguageUtil.GetLanguage(GetBrowserLanguage());
            LanguageSet?.Invoke(Language);
            IsLanguageSet = true;
            Debug.Log(LOG_PREFIX + $"Preferred language: {Language.ToString()}.");
        }

        private static void InitSettings()
        {
            try
            {
                _settings = Resources.Load<WortalSettings>("WortalSettings");
            }
            catch (Exception e)
            {
                Debug.LogError(LOG_PREFIX + $"Failed to initialize. WortalSettings are missing. \n{e}");
            }
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
        private static extern string GetBrowserLanguage();

        [DllImport("__Internal")]
        private static extern void GetLinkAdUnitIds(Action<string, string> callback);

        [DllImport("__Internal")]
        private static extern void OpenLink(string url);

        [MonoPInvokeCallback(typeof(Action<string, string>))]
        private static void LinkAdUnitCallback(string interstitialId, string rewardedId)
        {
            if (string.IsNullOrEmpty(interstitialId) || string.IsNullOrEmpty(rewardedId))
            {
                Debug.LogWarning(LOG_PREFIX + "Link AdUnit IDs invalid or missing. Ad calls will not be made.");
            }

            _settings.LinkInterstitialId = interstitialId;
            _settings.LinkRewardedId = rewardedId;
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
