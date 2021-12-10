namespace DigitalWill.Services
{
    /// <summary>
    /// Types of game service interfaces and their corresponding implementations.
    /// </summary>
    public static class ServiceType
    {
        /// <summary>
        /// Handles in-game audio functions.
        /// </summary>
        public enum AudioType
        {
            /// <summary>
            /// Null service, empty implementation disables the service.
            /// </summary>
            None,
            /// <summary>
            /// Soup provides audio services.
            /// </summary>
            SoupAudio
        }

        /// <summary>
        /// Handles in-game localization.
        /// </summary>
        public enum LocalizationType
        {
            /// <summary>
            /// Null service, empty implementation disables the service.
            /// </summary>
            None,
            /// <summary>
            /// Soup provides localization services.
            /// </summary>
            SoupLocalization
        }

        /// <summary>
        /// Manages game and player data locally.
        /// </summary>
        public enum LocalDataType
        {
            /// <summary>
            /// Null service, empty implementation disables the service.
            /// </summary>
            None,
            /// <summary>
            /// Soup provides local data handling services.
            /// </summary>
            SoupLocalData
        }
    }
}
