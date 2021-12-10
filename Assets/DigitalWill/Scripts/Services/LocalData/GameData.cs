using UnityEngine;

namespace DigitalWill.Services
{
    /// <summary>
    /// Data about the game that can be saved and loaded. Common uses might be player language, audio settings, etc.
    /// </summary>
    [System.Serializable]
    public class GameData
    {
        public LanguageCode LanguageCode;
        public bool AudioToggle;
        public float VolumeMaster;
        public float VolumeBGM;
        public float VolumeSFX;

        /// <summary>
        /// Constructs a GameData object with data about the current game settings.
        /// </summary>
        /// <param name="languageCode">Language the player is playing the game in.</param>
        /// <param name="audioToggle">Is the audio toggled on or off.</param>
        /// <param name="volumeMaster">Float range 0 to 1 representing the master volume percentage.</param>
        /// <param name="volumeBGM">Float range 0 to 1 representing the background music volume percentage.</param>
        /// <param name="volumeSFX">Float range 0 to 1 representing the sound effects volume percentage.</param>
        /// <remarks>If a volume float greater than 1 is passed in the value will be clamped to 1, which represents
        /// 100% volume in-game.</remarks>
        public GameData(LanguageCode languageCode, bool audioToggle, float volumeMaster, float volumeBGM, float volumeSFX)
        {
            LanguageCode = languageCode;
            AudioToggle = audioToggle;
            VolumeMaster = Mathf.Clamp01(volumeMaster);
            VolumeBGM = Mathf.Clamp01(volumeBGM);
            VolumeSFX = Mathf.Clamp01(volumeSFX);
        }
    }
}
