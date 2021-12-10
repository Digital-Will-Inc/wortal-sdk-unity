using System;
using UnityEngine;

namespace DigitalWill.Services
{
    /// <summary>
    /// Basic audio service.
    /// </summary>
    public class SoupAudioService : IAudioService
    {
#pragma warning disable 67
        public event Action<bool> AudioToggled;
        public event Action<float> VolumeMasterChanged;
        public event Action<float> VolumeBGMChanged;
        public event Action<float> VolumeSFXChanged;
#pragma warning restore 67

        public bool IsAudioOn { get; private set; }
        public float VolumeMaster { get; private set; }
        public float VolumeBGM { get; private set; }
        public float VolumeSFX { get; private set; }

        public SoupAudioService()
        {
            IsAudioOn = true;
            VolumeMaster = 1f;
            VolumeBGM = 1f;
            VolumeSFX = 1f;
        }

        public void ToggleAudio(bool status)
        {
            IsAudioOn = status;
            AudioToggled?.Invoke(IsAudioOn);
        }

        public void SetVolumeMaster(float volume)
        {
            VolumeMaster = Mathf.Clamp01(volume);
            VolumeMasterChanged?.Invoke(VolumeMaster);
        }

        public void SetVolumeBGM(float volume)
        {
            VolumeBGM = Mathf.Clamp01(volume);
            VolumeBGMChanged?.Invoke(VolumeBGM);
        }

        public void SetVolumeSFX(float volume)
        {
            VolumeSFX = Mathf.Clamp01(volume);
            VolumeSFXChanged?.Invoke(VolumeSFX);
        }
    }
}
