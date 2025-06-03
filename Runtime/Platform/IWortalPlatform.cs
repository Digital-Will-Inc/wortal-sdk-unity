using System;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Main platform abstraction interface that provides access to all Wortal SDK features
    /// </summary>
    public interface IWortalPlatform
    {
        /// <summary>
        /// Gets the platform type
        /// </summary>
        WortalPlatformType PlatformType { get; }

        /// <summary>
        /// Gets the authentication implementation for this platform
        /// </summary>
        IWortalAuthentication Authentication { get; }

        /// <summary>
        /// Gets the achievements implementation for this platform
        /// </summary>
        IWortalAchievements Achievements { get; }

        /// <summary>
        /// Gets the ads implementation for this platform
        /// </summary>
        IWortalAds Ads { get; }

        /// <summary>
        /// Gets the analytics implementation for this platform
        /// </summary>
        IWortalAnalytics Analytics { get; }

        /// <summary>
        /// Gets the context implementation for this platform
        /// </summary>
        IWortalContext Context { get; }

        /// <summary>
        /// Gets the IAP implementation for this platform
        /// </summary>
        IWortalIAP IAP { get; }

        /// <summary>
        /// Gets the leaderboard implementation for this platform
        /// </summary>
        IWortalLeaderboard Leaderboard { get; }

        /// <summary>
        /// Gets the stats implementation for this platform
        /// </summary>
        IWortalNotifications Notifications { get; }

        /// <summary>
        /// Gets the player implementation for this platform
        /// </summary>
        IWortalPlayer Player { get; }

        /// <summary>
        /// Gets the session implementation for this platform
        /// </summary>
        IWortalSession Session { get; }

        /// <summary>
        /// Gets the stats implementation for this platform
        /// </summary>
        IWortalStats Stats { get; }

        /// <summary>
        /// Gets the tournament implementation for this platform
        /// </summary>
        IWortalTournament Tournament { get; }

        /// <summary>
        /// Initializes the platform
        /// </summary>
        /// <param name="onSuccess">Callback for successful initialization</param>
        /// <param name="onError">Callback for initialization errors</param>
        void Initialize(Action onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Checks if the platform is initialized
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Gets the supported APIs for this platform
        /// </summary>
        string[] GetSupportedAPIs();
    }
}
