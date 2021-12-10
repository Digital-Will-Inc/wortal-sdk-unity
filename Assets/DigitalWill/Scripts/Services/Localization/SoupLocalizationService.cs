using System;
using System.Collections.Generic;
using DigitalWill.Core;
using TMPro;
using UnityEngine;

namespace DigitalWill.Services
{
    /// <summary>
    /// Localization service using a CSV with localized values.
    /// </summary>
    /// <remarks>CSV should be named LanguageFile.csv and placed in the Resources folder. Values should be tab-separated and keys should start with @.</remarks>
    public class SoupLocalizationService : ILocalizationService
    {
        private const LanguageCode DEFAULT_LANGUAGE = LanguageCode.EN;
        private const string PREFS_KEY = "game_language";
        private const string LANG_FILE = "LanguageFile";

#pragma warning disable 67
        public event Action<LanguageCode> LanguageChanged;
        public event Action<LanguageCode> FontChanged;
#pragma warning restore 67

        private readonly List<LanguageCode> _availableLanguages = new List<LanguageCode>();
        private Dictionary<LanguageCode, Dictionary<string, string>> _localizedDB;
        private Dictionary<string, string> _workingDB;

        private LanguageCode _currentLanguage;
        private TextAsset _languageFile;

        public TMP_FontAsset Font { get; private set; }

        public void Init(LanguageCode code)
        {
            _localizedDB = InitDictionary();

            try
            {
                SetLanguage(code);
                SetFont(code);
            }
            catch
            {
                Soup.LogError($"Failed to localize for {code}. Falling back to default language.");
                SetLanguage(DEFAULT_LANGUAGE);
                SetFont(DEFAULT_LANGUAGE);
            }
        }

        public void SetLanguage(string languageCode)
        {
            SetLanguage((LanguageCode)Enum.Parse(typeof(LanguageCode), languageCode));
        }

        public void SetLanguage(LanguageCode languageCode)
        {
            if (_localizedDB.ContainsKey(languageCode))
            {
                _currentLanguage = languageCode;
                Soup.Log($"LocalizationService setting current language: {languageCode}.");
            }
            else
            {
                _currentLanguage = DEFAULT_LANGUAGE;
                Soup.LogError($"LocalizationService could not find language code: {nameof(languageCode)}. Falling back to default language.");
            }

            PlayerPrefs.SetString(PREFS_KEY, nameof(_currentLanguage));
            LanguageChanged?.Invoke(languageCode);

            _workingDB = _localizedDB[_currentLanguage];

            if (_currentLanguage == DEFAULT_LANGUAGE)
            {
                return;
            }

            FontChanged?.Invoke(languageCode);
            Soup.Log($"Font change signaled for {languageCode}.");
        }

        public string GetValue(string key, string[] parameters = null)
        {
            string value = _workingDB[key];

            if (string.IsNullOrEmpty(value))
            {
                if (key == "")
                {
                    Soup.LogError($"No key found for {key}");
                    return "";
                }

                Soup.LogError($"No translation found for {key}");
                return "";
            }

            if (parameters != null && parameters.Length > 0)
            {
                return string.Format(value.Replace("\\n", Environment.NewLine), parameters);
            }

            return string.Format(value.Replace("\\n", Environment.NewLine));
        }

        public Dictionary<string, string> GetDictionaryByLanguage(LanguageCode languageCode)
        {
            return _localizedDB[languageCode];
        }

        private void SetFont(LanguageCode languageCode)
        {
            Font = SoupComponent.SoupSettings.DefaultFont;
            var fonts = SoupComponent.SoupSettings.CustomFonts;

            if (fonts == null || fonts.Count < 1)
            {
                return;
            }

            for (int i = 0; i < fonts.Count; i++)
            {
                if (fonts[i].LanguageCode == languageCode)
                {
                    Font = fonts[i].Font;
                }
            }
        }

        private Dictionary<LanguageCode, Dictionary<string, string>> InitDictionary()
        {
            _languageFile = Resources.Load<TextAsset>(LANG_FILE);

            var languageDictionary = new Dictionary<LanguageCode, Dictionary<string, string>>();

            string[] lines = _languageFile.text.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            string[] columns = lines[0].Split('\t');
            string[] languageNames = Enum.GetNames(typeof(LanguageCode));

            foreach (string languageCode in columns)
            {
                if (Array.IndexOf(languageNames, languageCode) >= 0)
                {
                    _availableLanguages.Add((LanguageCode)Enum.Parse(typeof(LanguageCode), languageCode));
                    Soup.Log($"SoupLocalizationService: Adding language support for {languageCode}.");
                }
            }

            foreach (LanguageCode languageCode in _availableLanguages)
            {
                languageDictionary[languageCode] = new Dictionary<string, string>();
            }

            foreach (string text in lines)
            {
                if (text.StartsWith("#") || !text.StartsWith("@"))
                {
                    continue;
                }

                string[] line = text.Split('\t');

                for (int i = 0; i < _availableLanguages.Count; i++)
                {
                    languageDictionary[_availableLanguages[i]].Add(line[0], line[i + 1] != "" ? line[i + 1] : " ");
                }
            }

            return languageDictionary;
        }
    }
}
