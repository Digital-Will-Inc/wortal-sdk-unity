using System;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Payload for getting player stats.
    /// </summary>
    [Serializable]
    public struct GetStatsPayload
    {
        /// <summary>
        /// The period of time over which the stat is tracked.
        /// </summary>
        [JsonProperty("period", NullValueHandling = NullValueHandling.Ignore)]
        public StatPeriod Period;
    }
}
