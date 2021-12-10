using System;
using System.Collections.Generic;
using TMPro;

namespace DigitalWill.Services
{
    /// <summary>
    /// Empty service for stub testing or disabling the service.
    /// </summary>
    public class NullLocalizationService : ILocalizationService
    {
#pragma warning disable 67
        public event Action<LanguageCode> LanguageChanged;
        public event Action<LanguageCode> FontChanged;
#pragma warning restore 67
        public TMP_FontAsset Font => null;
        public void Init(LanguageCode code) { }
        public void SetLanguage(string languageCode) { }
        public void SetLanguage(LanguageCode languageCode) { }
        public string GetValue(string key, string[] parameters = null) { return null; }
        public Dictionary<string, string> GetDictionaryByLanguage(LanguageCode languageCode) { return null; }
    }
}
