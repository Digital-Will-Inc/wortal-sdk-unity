using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// A filter that may be applied to an inviteAsync operation. If no results are returned with the filters, then the filters will not be applied.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum InviteFilter
    {
        /// <summary>
        /// Prefer to only surface contexts the game has not been played in before. This can include players who have not played the game before.
        /// </summary>
        NEW_CONTEXT_ONLY,
        /// <summary>
        /// Prefer to only surface people who have not played the game before.
        /// </summary>
        NEW_PLAYERS_ONLY,
        /// <summary>
        /// Prefer to only surface contexts the game has been played in before.
        /// </summary>
        EXISTING_CONTEXT_ONLY,
        /// <summary>
        /// Prefer to only surface people who have played the game before.
        /// </summary>
        EXISTING_PLAYERS_ONLY,
    }
}
