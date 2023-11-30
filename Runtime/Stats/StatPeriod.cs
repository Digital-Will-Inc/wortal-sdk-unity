using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Different periods of time stats are tracked by.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatPeriod
    {
        ALLTIME,
        DAILY,
        MONTHLY,
        WEEKLY,
    }
}
