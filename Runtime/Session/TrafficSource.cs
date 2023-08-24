using System;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Traffic source info.
    /// </summary>
    [Serializable]
    public struct TrafficSource
    {
        /// <summary>
        /// Entry point of the session.
        /// </summary>
        [JsonProperty("['r_entrypoint']", NullValueHandling = NullValueHandling.Ignore)]
        public string EntryPoint;
        /// <summary>
        /// UTM source tag.
        /// </summary>
        [JsonProperty("['utm_source']", NullValueHandling = NullValueHandling.Ignore)]
        public string UTMSource;
    }
}
