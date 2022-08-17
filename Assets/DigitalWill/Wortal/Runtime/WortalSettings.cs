using UnityEngine;

namespace DigitalWill
{
    /// <summary>
    /// Settings for the wortal.
    /// </summary>
    public class WortalSettings : ScriptableObject
    {
        [Header("Localization")]
        [SerializeField] private bool _enableLanguageCheck = true;
        [SerializeField] private Language _defaultLanguage = Language.English;
        [SerializeField] private Language[] _supportedLanguages = { Language.English, Language.Japanese };

        public string LinkInterstitialId { get; set; }
        public string LinkRewardedId { get; set; }
        public bool EnableLanguageCheck => _enableLanguageCheck;
        public Language DefaultLanguage => _defaultLanguage;
        public Language[] SupportedLanguages => _supportedLanguages;
    }
}
