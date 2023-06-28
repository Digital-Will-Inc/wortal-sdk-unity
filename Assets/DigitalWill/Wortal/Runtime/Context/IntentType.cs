using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Message format to be used. There's no visible difference among the available options.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum IntentType
    {
        /// <summary>
        /// Ignores this parameter and does not send to the SDK.
        /// </summary>
        NONE,
        INVITE,
        REQUEST,
        CHALLENGE,
        SHARE,
    }
}
