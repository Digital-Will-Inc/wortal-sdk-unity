using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Types of achievements.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AchievementType
    {
        /// <summary>
        /// The achievement is unlocked when the user reaches a certain value.
        /// </summary>
        SINGLE,
        /// <summary>
        /// The achievement is unlocked when the user reaches a certain value, but the value can be increased. Ex: Win 5 games, Win 10 games, etc.
        /// </summary>
        INCREMENTAL,
    }
}
