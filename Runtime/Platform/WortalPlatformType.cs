namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Defines the supported platform types for Wortal SDK
    /// Each platform includes its respective game services
    /// </summary>
    public enum WortalPlatformType
    {
        /// <summary>
        /// WebGL platform for web games (Poki, CrazyGames, etc.)
        /// </summary>
        WebGL,

        /// <summary>
        /// Android platform with Google Play Games Services
        /// </summary>
        Android,

        /// <summary>
        /// iOS platform with Game Center
        /// </summary>
        iOS,

        /// <summary>
        /// Unity Editor (for testing)
        /// </summary>
        Editor
    }
}
