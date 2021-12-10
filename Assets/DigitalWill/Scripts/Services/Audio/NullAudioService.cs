using System;

namespace DigitalWill.Services
{
    /// <summary>
    /// Empty service for stub testing or disabling the service.
    /// </summary>
    public class NullAudioService : IAudioService
    {
#pragma warning disable 67
        public event Action<bool> AudioToggled;
        public event Action<float> VolumeMasterChanged;
        public event Action<float> VolumeBGMChanged;
        public event Action<float> VolumeSFXChanged;
#pragma warning restore 67
        public bool IsAudioOn => false;
        public float VolumeMaster => -1;
        public float VolumeBGM => -1;
        public float VolumeSFX => -1;
        public void ToggleAudio(bool status) { }
        public void SetVolumeMaster(float volume) { }
        public void SetVolumeBGM(float volume) { }
        public void SetVolumeSFX(float volume) { }
    }
}
