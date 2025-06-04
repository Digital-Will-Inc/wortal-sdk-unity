using System;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Represents the stats of a player.
    /// </summary>
    [Serializable]
    public struct Stats
    {
        /// <summary>
        /// Name of the level the stats are for.
        /// </summary>
        [JsonProperty("level")]
        public string Level;
        /// <summary>
        /// The value of the stat.
        /// </summary>
        [JsonProperty("value")]
        public int Value;
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
