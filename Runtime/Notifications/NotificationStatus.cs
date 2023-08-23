using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Status types of a scheduled notification.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum NotificationStatus
    {
        SCHEDULED,
        SENT,
        CANCELED,
        UNKNOWN,
    }
}
