using System;

namespace DigitalWill.Services
{
    /// <summary>
    /// Base interface for audio services.
    /// </summary>
    public interface IAudioService : IGameService
    {
        /// <summary>
        /// Subscribe to be notified when the audio is toggled on or off.
        /// </summary>
        event Action<bool> AudioToggled;
        /// <summary>
        /// Subscribe to be notified when the master volume has changed.
        /// </summary>
        event Action<float> VolumeMasterChanged;
        /// <summary>
        /// Subscribe to be notified when the background music volume has changed.
        /// </summary>
        event Action<float> VolumeBGMChanged;
        /// <summary>
        /// Subscribe to be notified when the sound effects volume has changed.
        /// </summary>
        event Action<float> VolumeSFXChanged;

        /// <summary>
        /// Is the global audio currently enabled or not.
        /// </summary>
        bool IsAudioOn { get; }
        /// <summary>
        /// Master volume level.
        /// </summary>
        float VolumeMaster { get; }
        /// <summary>
        /// Volume level of the background music.
        /// </summary>
        float VolumeBGM { get; }
        /// <summary>
        /// Volume level of the sound effects.
        /// </summary>
        float VolumeSFX { get; }

        /// <summary>
        /// Toggles the audio on and off.
        /// </summary>
        /// <param name="status">Status of the audio to set.</param>
        void ToggleAudio(bool status);

        /// <summary>
        /// Sets the master volume level.
        /// </summary>
        /// <param name="volume">Float between 0 and 1.0f that represents the volume level.</param>
        void SetVolumeMaster(float volume);

        /// <summary>
        /// Sets the volume level of the background music.
        /// </summary>
        /// <param name="volume">Float between 0 and 1.0f that represents the volume level.</param>
        void SetVolumeBGM(float volume);

        /// <summary>
        /// Sets the volume level of the sound effects.
        /// </summary>
        /// <param name="volume">Float between 0 and 1.0f that represents the volume level.</param>
        void SetVolumeSFX(float volume);
    }
}
