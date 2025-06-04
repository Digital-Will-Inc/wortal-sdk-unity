using System;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// A single entry on a leaderboard.
    /// </summary>
    [Serializable]
    public struct LeaderboardEntry
    {
        /// <summary>
        /// Player that made this entry.
        /// </summary>
        [JsonProperty("player")]
        public Player Player;
        /// <summary>
        /// Rank of this entry in the leaderboard.
        /// </summary>
        [JsonProperty("rank")]
        public int Rank;
        /// <summary>
        /// Score achieved in this entry.
        /// </summary>
        [JsonProperty("score")]
        public int Score;
        /// <summary>
        /// Score of this entry with optional formatting. Ex: '100 points'
        /// </summary>
        [JsonProperty("formattedScore")]
        public string FormattedScore;
        /// <summary>
        /// Timestamp of when this entry was made.
        /// </summary>
        [JsonProperty("timestamp")]
        public int Timestamp;
        /// <summary>
        /// Optional details about this entry.
        /// </summary>
        [JsonProperty("details", NullValueHandling = NullValueHandling.Ignore)]
        public string Details;
    }
}
