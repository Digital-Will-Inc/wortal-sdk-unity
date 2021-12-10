using System;
using UnityEngine;

namespace DigitalWill.Services
{
    /// <summary>
    /// Image extensions class that holds an image for a specific language.
    /// </summary>
    [Serializable]
    public class LocalizedImage
    {
        /// <summary>
        /// Image language code.
        /// </summary>
        public LanguageCode LanguageCode;

        /// <summary>
        /// Image to display for this language.
        /// </summary>
        public Sprite Sprite;
    }
}
