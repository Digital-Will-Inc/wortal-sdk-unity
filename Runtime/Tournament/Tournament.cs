using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Represents a tournament for the game.
    /// </summary>
    [Serializable]
    public struct Tournament
    {
        /// <summary>
        /// ID of the tournament.
        /// </summary>
        [JsonProperty("id")]
        public string ID;
        /// <summary>
        /// Context ID the tournament is associated with.
        /// </summary>
        [JsonProperty("contextID")]
        public string ContextID;
        /// <summary>
        /// Unix timestamp of when the tournament ends.
        /// </summary>
        [JsonProperty("endTime")]
        public long EndTime;
        /// <summary>
        /// Optional title of the tournament.
        /// </summary>
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title;
        /// <summary>
        /// Optional payload attached to the tournament.
        /// </summary>
        [JsonProperty("payload", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Payload;

        /// <summary>
        /// Constructor for creating a tournament. This is used for mock tournaments and debugging.
        /// Will create a random ID and ContextID.
        /// </summary>
        /// <param name="payload">Payload to create the tournament with.</param>
        public Tournament(CreateTournamentPayload payload)
        {
            ID = UnityEngine.Random.Range(100000, 999999).ToString();
            ContextID = UnityEngine.Random.Range(100000, 999999).ToString();
            EndTime = payload.Config.EndTime;
            Title = payload.Config.Title;
            Payload = payload.Data;
        }
    }
}
