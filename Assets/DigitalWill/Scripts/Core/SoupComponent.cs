using System;
using System.Collections;
using System.IO;
using DigitalWill.Services;
using UnityEditor;
using UnityEngine;

namespace DigitalWill.Core
{
    /// <summary>
    /// Handles Soup settings and some SDK-specific functions. Service classes that do not inherit from MonoBehaviour
    /// but wish to use coroutines may call the instance of this class to invoke StartCoroutine.
    /// </summary>
    /// <remarks>Settings should only be edited from the editor window and never at runtime.</remarks>
    public class SoupComponent : Singleton<SoupComponent>
    {
        private const string SETTINGS_PATH = "Assets/DigitalWill/Resources/SoupSettings.asset";

        private static SoupSettings _settings;

#pragma warning disable 67
        /// <summary>
        /// Subscribe to be notified when an ad timeout occurs. This is helpful to escape out of error conditions
        /// and avoid infinite waiting loops for non-existent ads.
        /// </summary>
        public event Action AdTimeout;

        /// <summary>
        /// Subscribe to be notified when the language code has been parsed and set.
        /// </summary>
        public event Action<LanguageCode> LanguageCodeSet;
#pragma warning restore 67

        /// <summary>
        /// Has the LanguageCode been set yet or not.
        /// </summary>
        public bool IsLanguageCodeSet { get; private set; }

        /// <summary>
        /// Sets the 2-letter ISO language code of the language received from the browser.
        /// </summary>
        public LanguageCode LanguageCode { get; private set; }

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern string GetBrowserLanguage();

        [DllImport("__Internal")]
        private static extern void OpenLink(string url);
#endif

        protected override void Awake()
        {
            base.Awake();

#if UNITY_WEBGL && !UNITY_EDITOR
            var language = GetBrowserLanguage();
#else
            var language = SoupSettings.DebugLanguage.ToString();
#endif

            AdSense.AdCalled += OnAdCalled;
            ParseLanguageToCode(language);
            Soup.Log($"Preferred language: {language}.");
        }

        private void OnDestroy()
        {
            AdSense.AdCalled -= OnAdCalled;
        }

        /// <summary>
        /// Settings used to initialize Soup. These should only be edited in the Soup Settings editor window.
        /// </summary>
        public static SoupSettings SoupSettings
        {
            get
            {
                if (_settings == null)
                {
                    InitSoup();
                }

                return _settings;
            }
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
            Soup.Log("Ad timeout reached.");
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
                LanguageCode = SoupSettings.DefaultLanguage;
                Soup.LogWarning("Language could not be parsed. Using system default.");
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
                    LanguageCode = SoupSettings.DefaultLanguage;
                    Soup.LogWarning("No supported language code was found. Using system default.");
                    break;
            }

            LanguageCodeSet?.Invoke(LanguageCode);
            IsLanguageCodeSet = true;
            Soup.Log($"LanguageCode set to {LanguageCode}");
        }

        private static void InitSoup()
        {
            try
            {
                _settings = Resources.Load<SoupSettings>("SoupSettings");

#if UNITY_EDITOR
                if (_settings == null)
                {
                    if (!Directory.Exists(Application.dataPath + "/DigitalWill/Resources"))
                    {
                        Directory.CreateDirectory(Application.dataPath + "/DigitalWill/Resources");
                        Soup.LogWarning("SoupComponent: Detected missing DigitalWill/Resources directory. creating now.");
                    }

                    // We found a settings file, but for some reason it didn't load properly. It might be corrupt.
                    // We will just delete it and create a new one with default values.
                    if (File.Exists(SETTINGS_PATH))
                    {
                        AssetDatabase.DeleteAsset(SETTINGS_PATH);
                        AssetDatabase.Refresh();
                        Soup.LogWarning("SoupComponent: Detected a potentially corrupt SoupSettings file. Deleting now.");
                    }

                    var asset = ScriptableObject.CreateInstance<SoupSettings>();
                    AssetDatabase.CreateAsset(asset, SETTINGS_PATH);

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    Soup.LogWarning("SoupComponent: SoupSettings file was missing. Created a new one.");

                    _settings = asset;
                    Selection.activeObject = asset;
                }
#endif
            }
            catch(Exception e)
            {
                Soup.LogError($"SoupComponent: Failed to initialize. \n{e}");
            }
        }
    }
}
