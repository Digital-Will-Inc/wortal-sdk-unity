using System;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Payload used to post a player's stats.
    /// </summary>
    [Serializable]
    public struct PostStatsPayload
    {
        /// <summary>
        /// The period of time over which the stat is tracked.
        /// </summary>
        [JsonProperty("period", NullValueHandling = NullValueHandling.Ignore)]
        public StatPeriod Period;
        /// <summary>
        /// The type of stat this value represents.
        /// </summary>
        [JsonProperty("valueType", NullValueHandling = NullValueHandling.Ignore)]
        public StatValueType ValueType;
        /// <summary>
        /// Whether a lower value is a better value for this stat. Ex: time to complete a level.
        /// </summary>
        [JsonProperty("lowerIsBetter", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsLowerBetter;
    }
}
