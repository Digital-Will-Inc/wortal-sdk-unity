using System.Collections.Generic;
using DigitalWill.Core;
using TMPro;
using UnityEngine;

namespace DigitalWill.Services
{
    /// <summary>
    /// Container for custom fonts, images and audio clips that are used in localization services.
    /// </summary>
    public class LanguageAssets : Singleton<LanguageAssets>
    {
        [Header("Language Properties")]
        [SerializeField] private LanguageCode _defaultLanguage;

        [Header("Fonts")]
        [SerializeField] private TMP_FontAsset _defaultFont;
        [SerializeField] private List<LocalizedFont> _customFonts;

        public LanguageCode DefaultLanguage => _defaultLanguage;
        public TMP_FontAsset DefaultFont => _defaultFont;
        public List<LocalizedFont> CustomFonts => _customFonts;
    }
}
