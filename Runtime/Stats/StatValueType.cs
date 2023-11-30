using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Different types of values that can represent a stat.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatValueType
    {
        NUMBER,
        TIME,
    }
}
