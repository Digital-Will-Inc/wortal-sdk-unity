using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

namespace DigitalWill
{
    /// <summary>
    /// Wortal class controls the wortal prefab and provides an interface to WortalPlugin.jslib.
    /// </summary>
    public class Wortal : MonoBehaviour
    {
        [Header("Wortal Properties")]
        [Tooltip("Default language to fall back on if the browser language cannot be detected or parsed. Also can be used if the detected language is not supported.")]
        [SerializeField] private LanguageCode _defaultLanguageCode = LanguageCode.EN;
        [Tooltip("String to test language parsing in Editor. Only the first two letters will be parsed and they should correspond to a language code. Example: JA, de, etc.")]
        [SerializeField] private string _testLanguage = "ja";

        #pragma warning disable 67
        /// <summary>
        /// Subscribe to be notified when an ad timeout occurs. This is helpful to escape out of error conditions
        /// and avoid infinite waiting loops for non-existent ads.
        /// </summary>
        public static event Action AdTimeout;

        /// <summary>
        /// Subscribe to be notified when the language code has been parsed and set.
        /// </summary>
        public static event Action<LanguageCode> LanguageCodeSet;
        #pragma warning restore 67

        /// <summary>
        /// Has the LanguageCode been set yet or not.
        /// </summary>
        public static bool IsLanguageCodeSet { get; private set; }

        /// <summary>
        /// Sets the 2-letter ISO language code of the language received from the browser.
        /// </summary>
        public static LanguageCode LanguageCode { get; private set; }

        #if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern string GetBrowserLanguage();
        #endif

        private void Awake()
        {
            #if UNITY_WEBGL && !UNITY_EDITOR
            var language = GetBrowserLanguage();
            #else
            var language = _testLanguage;
            #endif

            AdSense.AdCalled += OnAdCalled;
            ParseLanguageToCode(language);
            Debug.Log($"Preferred language: {language}.");
        }

        private void OnDestroy()
        {
            AdSense.AdCalled -= OnAdCalled;
        }

        private void OnAdCalled()
        {
            StartCoroutine(CheckForAdTimeout());
        }

        private IEnumerator CheckForAdTimeout()
        {
            yield return new WaitForSeconds(0.5f);

            if (AdSense.IsAdAvailable)
            {
                yield break;
            }

            AdTimeout?.Invoke();
            Debug.Log("Ad timeout reached.");
        }

        private void ParseLanguageToCode(string language)
        {
            string firstTwoLetters;

            if (!string.IsNullOrEmpty(language) && language.Length >= 2)
            {
                firstTwoLetters = language.Substring(0, 2).ToUpper();
            }
            else
            {
                LanguageCode = _defaultLanguageCode;
                Debug.LogWarning("Language could not be parsed. Using system default.");
                return;
            }

            switch (firstTwoLetters)
            {
                case "EN":
                    LanguageCode = LanguageCode.EN;
                    break;
                case "JA":
                    LanguageCode = LanguageCode.JA;
                    break;
                case "ES":
                    LanguageCode = LanguageCode.ES;
                    break;
                case "FR":
                    LanguageCode = LanguageCode.FR;
                    break;
                case "DE":
                    LanguageCode = LanguageCode.DE;
                    break;
                case "IT":
                    LanguageCode = LanguageCode.IT;
                    break;
                case "PT":
                    LanguageCode = LanguageCode.PT;
                    break;
                case "RU":
                    LanguageCode = LanguageCode.RU;
                    break;
                case "PL":
                    LanguageCode = LanguageCode.PL;
                    break;
                case "ZH":
                    LanguageCode = LanguageCode.ZH;
                    break;
                case "KO":
                    LanguageCode = LanguageCode.KO;
                    break;
                case "AR":
                    LanguageCode = LanguageCode.AR;
                    break;
                default:
                    LanguageCode = _defaultLanguageCode;
                    Debug.LogWarning("No supported language code was found. Using system default.");
                    break;
            }

            LanguageCodeSet?.Invoke(LanguageCode);
            IsLanguageCodeSet = true;
            Debug.Log($"LanguageCode set to {LanguageCode}");
        }
    }
}
