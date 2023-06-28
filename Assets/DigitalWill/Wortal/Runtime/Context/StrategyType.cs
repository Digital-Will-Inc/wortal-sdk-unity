using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Available options for the Strategy type in the SDK Core.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StrategyType
    {
        /// <summary>
        /// Ignores this parameter and does not send to the SDK.
        /// </summary>
        NONE,
        /// <summary>
        /// Will be sent immediately.
        /// </summary>
        IMMEDIATE,
        /// <summary>
        /// When the game session ends, the latest payload will be sent.
        /// </summary>
        LAST,
        /// <summary>
        /// Will be sent immediately, and also discard any pending `LAST` payloads in the same session.
        /// </summary>
        IMMEDIATE_CLEAR,
    }
}
