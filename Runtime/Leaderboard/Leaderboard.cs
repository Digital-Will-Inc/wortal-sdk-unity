using System;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Represents a leaderboard for the game.
    /// </summary>
    [Serializable]
    public struct Leaderboard
    {
        /// <summary>
        /// ID of the leaderboard.
        /// </summary>
        [JsonProperty("id")]
        public string Id;
        /// <summary>
        /// Name of the leaderboard.
        /// </summary>
        [JsonProperty("name")]
        public string Name;
        /// <summary>
        /// Context ID of the leaderboard, if one exits.
        /// </summary>
        [JsonProperty("contextId", NullValueHandling = NullValueHandling.Ignore)]
        public string ContextId;
    }
}
