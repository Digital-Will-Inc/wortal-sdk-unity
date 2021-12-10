using System;
using UnityEngine;

namespace DigitalWill.Services
{
    /// <summary>
    /// Audio extension class that holds an audio source for a specific language.
    /// </summary>
    [Serializable]
    public class LocalizedAudio
    {
        /// <summary>
        /// Audio language code.
        /// </summary>
        public LanguageCode LanguageCode;

        /// <summary>
        /// Audio clip to play for this language.
        /// </summary>
        public AudioClip Audio;
    }
}
