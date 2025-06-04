using System;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Represents an achievement for the game.
    /// </summary>
    [Serializable]
    public struct Achievement
    {
        /// <summary>
        /// ID of the achievement.
        /// </summary>
        [JsonProperty("id")]
        public string Id;
        /// <summary>
        /// Name of the achievement.
        /// </summary>
        [JsonProperty("name")]
        public string Name;
        /// <summary>
        /// Whether the achievement is unlocked.
        /// </summary>
        [JsonProperty("isUnlocked")]
        public bool IsUnlocked;
        /// <summary>
        /// The description of the achievement.
        /// </summary>
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description;
        /// <summary>
        /// The type of the achievement.
        /// </summary>
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public AchievementType Type;
    }
}
