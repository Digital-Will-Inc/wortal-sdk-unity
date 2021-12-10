// ReSharper disable InconsistentNaming
using System.Collections.Generic;
using DigitalWill.Services;
using TMPro;
using UnityEngine;

namespace DigitalWill.Core
{
    /// <summary>
    /// <para>Settings file for the Soup package containing service types and configurations. Settings are configured in
    /// the Soup editor window and managed by <see cref="SoupComponent"/>.</para>
    /// <para>Only one settings asset should exist in a project and it should not be modified at runtime. Settings
    /// are set in the editor and loaded in at runtime. See repo docs for more information.</para>
    /// </summary>
    public class SoupSettings : ScriptableObject
    {
#region Package Properties
        public string PACKAGE_VERSION = "1.2.0";
        public string PACKAGE_REPO = "https://github.com/Digital-Will-Inc/unity-wortal-plugin";
#endregion

#region Soup Properties
        public SoupLogLevel LogLevel = SoupLogLevel.Info;
        public bool SaveLogsToFile;
#endregion

#region Service Types
        [Tooltip("Manages in-game audio controls.")]
        public ServiceType.AudioType Audio;
        [Tooltip("Manages locally-owned player and game data.")]
        public ServiceType.LocalDataType LocalData;
        [Tooltip("Localizes game content for different languages.")]
        public ServiceType.LocalizationType Localization;
#endregion

#region Soup Localization
        [Tooltip("Default language to be displayed in the game.")]
        public LanguageCode DefaultLanguage;
        [Tooltip("Debug language to test localization and custom fonts with.")]
        public LanguageCode DebugLanguage;
        [Tooltip("Default font to use in the game.")]
        public TMP_FontAsset DefaultFont;
        [Tooltip("Custom fonts to be used with localized languages.")]
        public List<LocalizedFont> CustomFonts;
        [Tooltip("Adds the Font Atlas Builder to the game for debug building of dynamic TMP_FontAssets.")]
        public bool UseFontAtlasBuilder;
#endregion

        public enum SoupLogLevel
        {
            Info,
            Warning,
            Error
        }
    }
}
