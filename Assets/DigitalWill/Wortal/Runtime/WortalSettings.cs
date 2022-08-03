using UnityEngine;

namespace DigitalWill
{
    /// <summary>
    /// Settings for the wortal.
    /// </summary>
    public class WortalSettings : ScriptableObject
    {
        [Header("Platform Config")]
        [SerializeField] private Platform _platform;

        [Header("Link Configuration")]
        [SerializeField] private string _linkInterstitialId;
        [SerializeField] private string _linkRewardedId;

        [Header("Localization")]
        [SerializeField] private Language _defaultLanguage = Language.English;
        [SerializeField] private Language[] _supportedLanguages = { Language.English, Language.Japanese };

        public Platform Platform => _platform;
        public string LinkInterstitialId => _linkInterstitialId;
        public string LinkRewardedId => _linkRewardedId;
        public Language DefaultLanguage => _defaultLanguage;
        public Language[] SupportedLanguages => _supportedLanguages;
    }

    public enum Platform
    {
        Debug,
        AdSense,
        Link,
        Viber,
    }
}
