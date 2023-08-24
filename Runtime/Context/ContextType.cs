using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Available options for the ContextType type in the SDK Core.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ContextType
    {
        /// <summary>
        /// Ignores this parameter and does not send to the SDK.
        /// </summary>
        NONE,
        /// <summary>
        /// Default context, where the player is the only participant.
        /// </summary>
        SOLO,
        /// <summary>
        /// A chat thread.
        /// </summary>
        THREAD,
        /// <summary>
        /// A Facebook group - FB only.
        /// </summary>
        GROUP,
        /// <summary>
        /// A Facebook post - FB only
        /// </summary>
        POST,
    }
}
