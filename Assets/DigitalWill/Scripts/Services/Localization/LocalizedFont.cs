using System;
using TMPro;
using UnityEngine;

namespace DigitalWill.Services
{
    /// <summary>
    /// Font extension class that provides configurable properties for specific fonts.
    /// </summary>
    [Serializable]
    public class LocalizedFont
    {
        /// <summary>
        /// Font language code.
        /// </summary>
        public LanguageCode LanguageCode;
        /// <summary>
        /// Font
        /// </summary>
        public TMP_FontAsset Font;
        /// <summary>
        /// True, when components should use custom line spacing.
        /// </summary>
        public bool CustomLineSpacing = false;
        /// <summary>
        /// Custom line spacing value.
        /// </summary>
        public float LineSpacing = 1.0f;
        /// <summary>
        /// True, when components should use custom font size.
        /// </summary>
        public bool CustomFontSizeOffset = false;
        /// <summary>
        /// Custom font size offset in percents.
        /// e.g. 55, -10
        /// </summary>
        public int FontSizeOffsetPercent = 0;
        /// <summary>
        /// True, when components should use custom alignment.
        /// </summary>
        public bool CustomAlignment = false;
        /// <summary>
        /// Custom alignment value.
        /// </summary>
        public TextAlignment Alignment = TextAlignment.Left;
    }
}
