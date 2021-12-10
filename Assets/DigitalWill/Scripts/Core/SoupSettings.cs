// ReSharper disable InconsistentNaming
using DigitalWill.Services;
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
        public string PACKAGE_VERSION = "2.0.0";
        public string PACKAGE_REPO = "https://github.com/Digital-Will-Inc/sdk-soup";
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

        public enum SoupLogLevel
        {
            Info,
            Warning,
            Error
        }
    }
}
