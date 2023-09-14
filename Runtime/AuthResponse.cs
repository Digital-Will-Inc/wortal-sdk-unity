using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Response from the Wortal authentication API.
    /// </summary>
    [Serializable]
    public struct AuthResponse
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("status")]
        public AuthStatus Status;
    }
}
