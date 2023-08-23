using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Filter used when searching for connected players.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ConnectedPlayerFilter
    {
        /// <summary>
        /// All friends.
        /// </summary>
        ALL,
        /// <summary>
        /// Only friends who have played this game before.
        /// </summary>
        INCLUDE_PLAYERS,
        /// <summary>
        /// Only friends who haven't played this game before.
        /// </summary>
        INCLUDE_NON_PLAYERS,
        /// <summary>
        /// Only friends who haven't been sent an in-game message before.
        /// </summary>
        NEW_INVITATIONS_ONLY,
    }
}
