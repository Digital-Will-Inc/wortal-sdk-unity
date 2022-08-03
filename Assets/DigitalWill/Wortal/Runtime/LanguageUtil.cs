using System.Collections.Generic;
using UnityEngine;

namespace DigitalWill
{
    /// <summary>
    /// Utilities for localization support.
    /// </summary>
    internal class LanguageUtil
    {
        /// <summary>
        /// Gets the <see cref="Language"/> for the game based on the player's preferred browser language.
        /// </summary>
        /// <param name="code">Language code from the player's browser.</param>
        /// <returns>Language the game should be played in. Returns the default language if the given language was not found or not supported.</returns>
        public static Language GetLanguage(string code)
        {
            Language language = Wortal.Settings.DefaultLanguage;

            if (!string.IsNullOrEmpty(code) && code.Length >= 2)
            {
                string languageCode = code.Substring(0, 2).ToLower();

                if (languageCode == "zh")
                {
                    language = HandleChineseLocales(code);
                }
                else
                {
                    if (!_languages.TryGetValue(languageCode, out language))
                    {
                        Debug.LogWarning($"[Wortal] Failed to find language for language code: {languageCode}");
                    }
                }
            }

            if (!IsLanguageSupported(language))
            {
                Debug.LogWarning($"[Wortal] Language not supported: {language.ToString()}. Using default language.");
                language = Wortal.Settings.DefaultLanguage;
            }

            return language;
        }

        private static bool IsLanguageSupported(Language language)
        {
            for (int i = 0; i < Wortal.Settings.SupportedLanguages.Length; i++)
            {
                if (Wortal.Settings.SupportedLanguages[i] == language)
                {
                    return true;
                }
            }

            return false;
        }

        private static Language HandleChineseLocales(string code)
        {
            string locale = null;
            if (code.Length >= 5)
            {
                locale = code.Substring(3, 2).ToLower();
            }

            if (locale == null || locale == "cn" || locale == "sg")
            {
                return Language.ChineseSimplified;
            }

            if (locale == "tw" || locale == "hk" || locale == "mo")
            {
                return Language.ChineseTraditional;
            }

            Debug.LogWarning($"[Wortal] Failed to find Chinese locale: {locale}");
            return Language.ChineseSimplified;
        }

        private static readonly Dictionary<string, Language> _languages = new Dictionary<string, Language>()
        {
            { "en", Language.English },
            { "ja", Language.Japanese },
            { "es", Language.Spanish },
            { "fr", Language.French },
            { "de", Language.German },
            { "it", Language.Italian },
            { "pt", Language.Portuguese },
            { "ko", Language.Korean },
            { "zhcn", Language.ChineseSimplified },
            { "zhtw", Language.ChineseTraditional },
            { "ru", Language.Russian },
            { "pl", Language.Polish },
            { "tr", Language.Turkish },
            { "ar", Language.Arabic },
            { "th", Language.Thai },
            { "vi", Language.Vietnamese },
            { "id", Language.Indonesian },
            { "ms", Language.Malay },
            { "tl", Language.Tagalog },
            { "da", Language.Danish },
            { "sv", Language.Swedish },
            { "nl", Language.Dutch },
            { "fi", Language.Finnish },
            { "no", Language.Norwegian },
            { "ua", Language.Ukrainian },
            { "ro", Language.Romanian },
            { "bg", Language.Bulgarian },
            { "hu", Language.Hungarian },
            { "hr", Language.Croatian },
            { "cs", Language.Czech },
            { "he", Language.Hebrew },
            { "hi", Language.Hindi },
            { "ur", Language.Urdu },
            { "ne", Language.Nepali },
            { "fa", Language.Farsi },
        };
    }
}
