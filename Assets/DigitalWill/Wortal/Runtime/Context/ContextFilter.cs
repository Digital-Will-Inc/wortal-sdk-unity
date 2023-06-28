using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Available options for the ContextFilter type in the SDK Core.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ContextFilter
    {
        /// <summary>
        /// Ignores this parameter and does not send to the SDK.
        /// </summary>
        NONE,
        /// <summary>
        /// Only enlists contexts that the current player is in, but never participated in (e.g. a new context created by a friend).
        /// </summary>
        NEW_CONTEXT_ONLY,
        /// <summary>
        /// Enlists contexts that the current player has participated before.
        /// </summary>
        INCLUDE_EXISTING_CHALLENGES,
        /// <summary>
        /// Only enlists friends who haven't played this game before.
        /// </summary>
        NEW_PLAYERS_ONLY,
        /// <summary>
        /// Only enlists friends who haven't been sent an in-game message before. This filter can be fine-tuned with `hoursSinceInvitation` parameter.
        /// </summary>
        NEW_INVITATIONS_ONLY,
    }
}
