using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;

namespace DigitalWill.Wortal
{
    /// <summary>
    /// Contains utility functions for interfacing with the game portal and player's browser.
    /// </summary>
    public class DWPortal
    {
#pragma warning disable 67
        /// <summary>
        /// Subscribe to be notified when an ad timeout occurs. This is helpful to escape out of error conditions
        /// and avoid infinite waiting loops for non-existent ads.
        /// </summary>
        public static event Action AdTimeout;

        /// <summary>
        /// Subscribe to be notified when the language code has been parsed and set.
        /// </summary>
        public static event Action<string> LanguageCodeSet;
#pragma warning restore 67

        /// <summary>
        /// Has the LanguageCode been set yet or not.
        /// </summary>
        public static bool IsLanguageCodeSet { get; private set; }

        /// <summary>
        /// Sets the 2-letter ISO language code of the language received from the browser.
        /// </summary>
        public static string LanguageCode { get; private set; }

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern string GetBrowserLanguage();

        [DllImport("__Internal")]
        private static extern void OpenLink(string url);
#endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            string language = GetBrowserLanguage();
#else
            string language = "EN";
#endif
            ParseLanguageCode(language);
            AdSense.AdCalled += OnAdCalled;
            Debug.Log($"Preferred language: {LanguageCode}.");
        }

        /// <summary>
        /// Opens a link to a website with the URL provided. Will open in a new browser tab.
        /// </summary>
        /// <param name="url">URL to open.</param>
        public static void OpenWebLink(string url)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            OpenLink(url);
#endif
        }

        private static void OnAdCalled()
        {
            Task.Delay(500).ContinueWith(_ => CheckForAdTimeout());
        }

        private static void CheckForAdTimeout()
        {
            if (AdSense.IsAdAvailable)
            {
                return;
            }

            AdTimeout?.Invoke();
            Debug.Log("Ad timeout reached.");
        }

        private static void ParseLanguageCode(string language)
        {
            string firstTwoLetters;
            if (!string.IsNullOrEmpty(language) && language.Length >= 2)
            {
                firstTwoLetters = language.Substring(0, 2).ToUpper();
            }
            else
            {
                firstTwoLetters = "EN";
                Debug.LogWarning("Language could not be parsed. Using system default.");
            }

            LanguageCode = firstTwoLetters;
            LanguageCodeSet?.Invoke(LanguageCode);
            IsLanguageCodeSet = true;
        }
    }
}
