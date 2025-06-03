using System;
using System.Collections.Generic;
using DigitalWill.WortalSDK;

namespace DigitalWill.WortalSDK.Core
{
    /// <summary>
    /// Interface for analytics functionality across platforms
    /// </summary>
    public interface IWortalAnalytics
    {
        /// <summary>
        /// Logs the start of a level
        /// </summary>
        /// <param name="level">Level identifier</param>
        void LogLevelStart(string level);

        /// <summary>
        /// Logs the end of a level
        /// </summary>
        /// <param name="level">Level identifier</param>
        /// <param name="score">Score achieved</param>
        /// <param name="wasCompleted">Whether the level was completed</param>
        void LogLevelEnd(string level, string score, bool wasCompleted);

        /// <summary>
        /// Logs the level up event
        /// </summary>
        /// <param name="level">New level reached</param>
        void LogLevelUp(int level);

        /// <summary>
        /// Logs a score event
        /// </summary>
        /// <param name="score">Score value</param>
        void LogScore(int score);

        /// <summary>
        /// Logs a tutorial begin event
        /// </summary>
        void LogTutorialStart();

        /// <summary>
        /// Logs a tutorial complete event
        /// </summary>
        void LogTutorialEnd();

        /// <summary>
        /// Logs a game choice event
        /// </summary>
        /// <param name="decision">The decision made</param>
        /// <param name="choice">The choice selected</param>
        void LogGameChoice(string decision, string choice);

        /// <summary>
        /// Logs a social invite event
        /// </summary>
        /// <param name="placement">Where the invite was sent from</param>
        void LogSocialInvite(string placement);

        /// <summary>
        /// Logs a social share event
        /// </summary>
        /// <param name="placement">Where the share was initiated from</param>
        void LogSocialShare(string placement);

        /// <summary>
        /// Logs a purchase event
        /// </summary>
        /// <param name="productID">ID of the purchased product</param>
        /// <param name="details">Additional purchase details</param>
        void LogPurchase(string productID, string details);

        /// <summary>
        /// Logs a purchase with subscription details
        /// </summary>
        /// <param name="productID">ID of the purchased product</param>
        /// <param name="details">Additional purchase details</param>
        void LogPurchaseSubscription(string productID, string details);

        /// <summary>
        /// Logs a custom event
        /// </summary>
        /// <param name="eventName">Name of the custom event</param>
        /// <param name="parameters">Event parameters</param>
        void LogCustomEvent(string eventName, Dictionary<string, object> parameters);

        /// <summary>
        /// Checks if analytics are supported on this platform
        /// </summary>
        bool IsSupported { get; }
    }
}
