#if UNITY_LOCALIZATION
using System.Collections.Generic;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Utility class for payload templates.
    /// </summary>
    public static class TemplateUtils
    {
        /// <summary>
        /// Gets a dictionary of locale codes and localized strings for the given <see cref="LocalizedString"/>.
        /// This is used for populating <see cref="LocalizableContent"/>.
        /// </summary>
        /// <param name="localizedString">String to get the dictionary for.</param>
        /// <returns>Dictionary with all localized strings based on the locales in the project.</returns>
        public static Dictionary<string, string> GetLocalizationDictionary(LocalizedString localizedString)
        {
            Dictionary<string, string> localizationDictionary = new();
            foreach (Locale locale in LocalizationSettings.AvailableLocales.Locales)
            {
                localizationDictionary.Add(locale.Identifier.Code, localizedString.GetLocalizedString(locale.Identifier.Code));
            }

            return localizationDictionary;
        }
    }
}
#endif
