using DigitalWill.Core;
using TMPro;
using UnityEngine;

namespace DigitalWill.Services
{
    /// <summary>
    /// Loops through a language dictionary and displays every value. This is useful when building a dynamic
    /// TMP_FontAsset and you need to expose every character used to build the atlas.
    /// </summary>
    public class FontAtlasBuilder : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private TMP_Text _characterDumpText;
        [SerializeField] private LanguageCode _languageToBuild;

        private void Start()
        {
            _characterDumpText.font = Soup.Localization.Font;
            BuildAtlas(_languageToBuild);
        }

        private void BuildAtlas(LanguageCode language)
        {
            var dict = Soup.Localization.GetDictionaryByLanguage(language);

            foreach (var entry in dict)
            {
                _characterDumpText.text = entry.Value;
            }
        }
    }
}
