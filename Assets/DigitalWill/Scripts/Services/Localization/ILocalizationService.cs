using System;
using System.Collections.Generic;
using TMPro;

namespace DigitalWill.Services
{
    /// <summary>
    /// Base interface for localization services.
    /// </summary>
    public interface ILocalizationService : IGameService
    {
        /// <summary>
        /// Subscribe to be notified when the game language changes.
        /// </summary>
        event Action<LanguageCode> LanguageChanged;
        /// <summary>
        /// Subscribe to be notified when the game font changes.
        /// </summary>
        event Action<LanguageCode> FontChanged;

        /// <summary>
        /// Font currently used in the game.
        /// </summary>
        TMP_FontAsset Font { get; }

        /// <summary>
        /// Initializes the service.
        /// </summary>
        /// <param name="code">Language code to initialize with.</param>
        void Init(LanguageCode code);

        /// <summary>
        /// Sets the language of the game.
        /// </summary>
        /// <param name="languageCode">String value of the 2-letter language code representing the language name.</param>
        void SetLanguage(string languageCode);

        /// <summary>
        /// Sets the language of the game.
        /// </summary>
        /// <param name="languageCode">LanguageCode enum of the language to set.</param>
        void SetLanguage(LanguageCode languageCode);

        /// <summary>
        /// Gets the localized value for the given key,
        /// </summary>
        /// <param name="key">Language file key for the value to get.</param>
        /// <param name="parameters">Optional parameters to pass.</param>
        /// <returns>Localized value string of the key provided.</returns>
        string GetValue(string key, string[] parameters = null);

        /// <summary>
        /// Gets the entire dictionary of a given language.
        /// </summary>
        /// <param name="languageCode">LanguageCode key of the dictionary to get.</param>
        /// <returns>Dictionary of the given language.</returns>
        /// <remarks>Useful for building TMP_FontAsset atlases with different fonts.</remarks>
        Dictionary<string, string> GetDictionaryByLanguage(LanguageCode languageCode);
    }
}
