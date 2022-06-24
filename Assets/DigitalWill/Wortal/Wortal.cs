using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace DigitalWill.H5Portal
{
    /// <summary>
    /// Contains utility functions for interfacing with the game portal and player's browser.
    /// </summary>
    public static class Wortal
    {
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
        public static event Action AdDone;
        /// <summary>
        /// A timeout was reached before an ad was shown. This is possibly due to ad blockers or other browser extensions.
        /// </summary>
        public static event Action AdTimedOut;
        /// <summary>
        /// A rewarded ad was successfully viewed and the player should be given a reward.
        /// </summary>
        public static event Action RewardedAdViewed;
        /// <summary>
        /// A rewarded ad was dismissed and the player should not receive a reward.
        /// </summary>
        public static event Action RewardedAdDismissed;
        /// <summary>
        /// Subscribe to be notified when the language code has been parsed and set.
        /// </summary>
        public static event Action<string> LanguageCodeSet;

        /// <summary>
        /// Has the LanguageCode been set yet or not.
        /// </summary>
        public static bool IsLanguageCodeSet { get; private set; }
        /// <summary>
        /// Sets the 2-letter ISO language code of the language received from the browser.
        /// </summary>
        public static string LanguageCode { get; private set; }
        /// <summary>
        /// The default language code to fallback to if the player's preferred language is not supported or not found.
        /// </summary>
        public static string DefaultLanguageCode { get; set; } = "EN";
        /// <summary>
        /// Was an ad successfully returned or not. This gets set in the beforeAd callback and checked in Wortal.cs
        /// on a timer to ensure we don't get stuck in an infinite loop waiting for an due to an error.
        /// </summary>
        public static bool IsAdAvailable { get; internal set; }

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
        /// Requests a rewarded ad. Should be called before <see cref="ShowRewardedAd"/>.
        /// </summary>
        /// <param name="name">Name of the ad to be shown.</param>
        public static void RequestRewardedAd(string name)
        {
            _ads.RequestRewardedAd(name);
        }

        /// <summary>
        /// Shows a rewarded ad.
        /// </summary>
        public static void ShowRewardedAd()
        {
            _ads.ShowRewardedAd();
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

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            //TODO: implement platform check once it is available in Wortal SDK
            _ads = new AdSense();

            string language = GetBrowserLanguage();
            ParseLanguageCode(language);
            Debug.Log($"[Wortal] Preferred language: {LanguageCode}.");
        }

        [DllImport("__Internal")]
        private static extern string GetBrowserLanguage();
        [DllImport("__Internal")]
        private static extern void OpenLink(string url);

        private static void ParseLanguageCode(string language)
        {
            string firstTwoLetters;
            if (!string.IsNullOrEmpty(language) && language.Length >= 2)
            {
                firstTwoLetters = language.Substring(0, 2).ToUpper();
            }
            else
            {
                firstTwoLetters = DefaultLanguageCode;
                Debug.LogWarning("[Wortal] Language could not be parsed. Using system default.");
            }

            LanguageCode = firstTwoLetters;
            LanguageCodeSet?.Invoke(LanguageCode);
            IsLanguageCodeSet = true;
        }
    }
}
