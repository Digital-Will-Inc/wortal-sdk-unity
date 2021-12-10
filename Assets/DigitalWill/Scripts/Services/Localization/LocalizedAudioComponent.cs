using System.Collections.Generic;
using UnityEngine;

namespace DigitalWill.Services
{
    /// <summary>
    /// Handles changing audio sources based on the current localization settings.
    /// </summary>
    /// <seealso cref="ILocalizationService"/>
    public class LocalizedAudioComponent : MonoBehaviour
    {
        [Header("Localization Properties")]
        [Tooltip("The default audio that will be played here.")]
        [SerializeField] private AudioClip _defaultClip;
        [Tooltip("List of localized audio assets with their corresponding language codes.")]
        [SerializeField] private List<LocalizedAudio> _localizedClips;

        private AudioSource _audioSource;
        private ILocalizationService _localization;

        private void Awake()
        {
            _localization = GameServices.I.Get<ILocalizationService>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            _localization.LanguageChanged += OnLanguageChanged;

            if (_defaultClip == null)
            {
                _defaultClip = _audioSource.clip;
            }
        }

        private void OnDestroy()
        {
            _localization.LanguageChanged -= OnLanguageChanged;
        }

        private void OnLanguageChanged(LanguageCode code)
        {
            if (_localizedClips == null || _localizedClips.Count == 0)
            {
                return;
            }

            AudioClip newClip = _defaultClip;

            for (int i = 0; i < _localizedClips.Count; i++)
            {
                if (_localizedClips[i].LanguageCode != code)
                {
                    continue;
                }

                newClip = _localizedClips[i].Audio;
                break;
            }

            _audioSource.clip = newClip;
        }
    }
}
