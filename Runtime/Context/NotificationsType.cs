using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Available options for the Notifications type in the SDK Core.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum NotificationsType
    {
        /// <summary>
        /// Ignores this parameter and does not send to the SDK.
        /// </summary>
        NONE,
        NO_PUSH,
        PUSH,
    }
}
