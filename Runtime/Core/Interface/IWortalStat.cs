using System;
using DigitalWill.WortalSDK;

namespace DigitalWill.WortalSDK.Core
{
    /// <summary>
    /// Interface for stats functionality across platforms
    /// </summary>
    public interface IWortalStats
    {
        /// <summary>
        /// Gets a player's stats.
        /// </summary>
        /// <param name="level">The name of the level to get stats for.</param>
        /// <param name="payload">Payload with additional details about the stats.</param>
        /// <param name="callback">Callback with the stats. Fired after async function resolves.</param>
        /// <param name="errorCallback">Error callback event with WortalError describing the error.</param>
        void GetStats(string level, GetStatsPayload payload, Action<Stats[]> callback, Action<WortalError> errorCallback);

        /// <summary>
        /// Posts a player's stats.
        /// </summary>
        /// <param name="level">The name of the level the stats are for.</param>
        /// <param name="value">The value of the stat.</param>
        /// <param name="payload">Payload with additional details about the stats.</param>
        /// <param name="callback">Callback. Fired after async function resolves.</param>
        /// <param name="errorCallback">Error callback event with WortalError describing the error.</param>
        void PostStats(string level, int value, PostStatsPayload payload, Action callback, Action<WortalError> errorCallback);

        /// <summary>
        /// Checks if stats features are supported on this platform
        /// </summary>
        bool IsSupported { get; }
    }
}