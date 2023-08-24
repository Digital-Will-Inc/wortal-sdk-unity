using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Represents content used to share a tournament.
    /// </summary>
    [Serializable]
    public struct ShareTournamentPayload
    {
        /// <summary>
        /// An integer value representing the player's latest score.
        /// </summary>
        [JsonProperty("score")]
        public int Score;
        /// <summary>
        /// A blob of data to attach to the update. Must be less than or equal to 1000 characters when stringified.
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Data;
    }
}
