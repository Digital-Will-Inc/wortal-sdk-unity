using System;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Represents a player in the game. To access info about the current player, use the Wortal.Player API.
    /// This is used to access info about other players such as friends or leaderboard entries.
    /// </summary>
    [Serializable]
    public struct Player
    {
        /// <summary>
        /// ID of the player. This is platform-dependent.
        /// </summary>
        [JsonProperty("id")]
        public string ID;
        /// <summary>
        /// Name of the player.
        /// </summary>
        [JsonProperty("name")]
        public string Name;
        /// <summary>
        /// Data URL for the player's photo.
        /// </summary>
        [JsonProperty("photo")]
        public string Photo;
        /// <summary>
        /// Is this the first time the player has played this game or not.
        /// </summary>
        [JsonProperty("isFirstPlay", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsFirstPlay;
        /// <summary>
        /// Days since the first time the player has played this game.
        /// </summary>
        [JsonProperty("daysSinceFirstPlay", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int DaysSinceFirstPlay;
    }
}
