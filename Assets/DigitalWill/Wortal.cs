using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace DigitalWill
{
    /// <summary>
    /// Handles interaction between Unity and WortalPlugin.jslib.
    /// </summary>
    public class Wortal : MonoBehaviour
    {
        [Header("Wortal Properties")]
        [Tooltip("Default language to fall back on if the browser language cannot be detected or parsed. Also can be used if the detected language is not supported.")]
        [SerializeField] private LanguageCode _defaultLanguageCode = LanguageCode.EN;

        /// <summary>
        /// Subscribe to be notified when the language code has been parsed and set.
        /// </summary>
        public event Action<LanguageCode> LanguageCodeSet;

        /// <summary>
        /// Sets the 2-letter ISO language code of the language received from the browser.
        /// </summary>
        public LanguageCode LanguageCode { get; private set; }

        [DllImport("__Internal")]
        private static extern string GetBrowserLanguage();

        private void Awake()
        {
            var language = GetBrowserLanguage();
            ParseLanguageToCode(language);
            Debug.Log($"Preferred language: {language}.");
        }

        private void ParseLanguageToCode(string language)
        {
            string firstTwoLetters;

            if (!string.IsNullOrEmpty(language) && language.Length >= 2)
            {
                firstTwoLetters = language.Substring(0, 2).ToUpper();
                Debug.Log($"Language parsed to code: {firstTwoLetters}.");
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
            Debug.Log($"LanguageCode set to {LanguageCode}");
        }
    }
}
