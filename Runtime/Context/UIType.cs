using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Available options for the UI type in the SDK Core.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UIType
    {
        /// <summary>
        /// Ignores this parameter and does not send to the SDK.
        /// </summary>
        NONE,
        /// <summary>
        /// Serial contact card with share and skip button.
        /// </summary>
        DEFAULT,
        /// <summary>
        /// Selectable contact list.
        /// </summary>
        MULTIPLE,
    }
}
