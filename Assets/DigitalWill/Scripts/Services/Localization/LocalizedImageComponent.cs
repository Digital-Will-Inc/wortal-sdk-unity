using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalWill.Services
{
    /// <summary>
    /// Handles changing image sprites based on the current localization settings.
    /// </summary>
    public class LocalizedImageComponent : MonoBehaviour
    {
        [Header("Localization Properties")]
        [Tooltip("The default sprite that will be displayed here.")]
        [SerializeField] private Sprite _defaultSprite;
        [Tooltip("List of localized image assets with their corresponding language codes.")]
        [SerializeField] private List<LocalizedImage> _localizedImages;

        private Image _image;
        private ILocalizationService _localization;

        private void Awake()
        {
            _localization = GameServices.I.Get<ILocalizationService>();
            _image = GetComponent<Image>();
        }

        private void Start()
        {
            _localization.LanguageChanged += OnLanguageChanged;

            if (_defaultSprite == null)
            {
                _defaultSprite = _image.sprite;
            }
        }

        private void OnDestroy()
        {
            _localization.LanguageChanged -= OnLanguageChanged;
        }

        private void OnLanguageChanged(LanguageCode code)
        {
            if (_localizedImages == null || _localizedImages.Count == 0)
            {
                return;
            }

            Sprite newSprite = _defaultSprite;

            for (int i = 0; i < _localizedImages.Count; i++)
            {
                if (_localizedImages[i].LanguageCode != code)
                {
                    continue;
                }

                newSprite = _localizedImages[i].Sprite;
                break;
            }

            _image.sprite = newSprite;
        }
    }
}
