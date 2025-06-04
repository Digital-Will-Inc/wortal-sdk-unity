using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Represents settings used for Tournament.Create.
    /// </summary>
    [Serializable]
    public struct CreateTournamentPayload
    {
        /// <summary>
        /// An integer value representing the player's score which will be the first score in the tournament.
        /// </summary>
        [JsonProperty("initialScore")]
        public int InitialScore;
        /// <summary>
        /// An object holding configurations for the tournament.
        /// </summary>
        [JsonProperty("config")]
        public CreateTournamentConfig Config;
        /// <summary>
        /// Optional payload to attach to the tournament.
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Data;
    }
}
