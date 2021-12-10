using DigitalWill.Services;
using UnityEngine;

namespace DigitalWill.Core
{
    /// <summary>
    /// Provides access to Soup features.
    /// </summary>
    public static class Soup
    {
        private static bool _isInit;

        /// <summary>
        /// Returns the currently registered IAudioService.
        /// </summary>
        public static IAudioService Audio { get; private set; }

        /// <summary>
        /// Returns the currently registered ILocalDataService.
        /// </summary>
        public static ILocalDataService LocalData { get; private set; }

        /// <summary>
        /// Returns the currently registered ILocalizationService.
        /// </summary>
        public static ILocalizationService Localization { get; private set; }

        /// <summary>
        /// Log info about Soup.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public static void Log(string message)
        {
            SoupLog.Log(message, LogType.Log);
        }

        /// <summary>
        /// Log warning about Soup.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public static void LogWarning(string message)
        {
            SoupLog.Log(message, LogType.Warning);
        }

        /// <summary>
        /// Log error about Soup.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public static void LogError(string message)
        {
            SoupLog.Log(message, LogType.Error);
        }

        public static void Init()
        {
            if (_isInit)
            {
                LogWarning("Soup: Already initialized.");
                return;
            }

            Audio = GameServices.I.Get<IAudioService>();
            LocalData = GameServices.I.Get<ILocalDataService>();
            Localization = GameServices.I.Get<ILocalizationService>();

            _isInit = true;
        }
    }
}
