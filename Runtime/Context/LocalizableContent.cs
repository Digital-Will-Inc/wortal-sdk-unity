using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Used to pass localized key-value pairs for content being used in the context API.
    /// </summary>
    /// <remarks>If no localizable content is required then pass only the Default string.</remarks>
    /// <example><code>
    /// var content = new LocalizableContent
    /// {
    ///     Default = "Play",
    ///     Localizations = new Dictionary&lt;string, string&gt;
    ///     {
    ///         {"en_US", "Play"},
    ///         {"ja_JP", "プレイ"},
    ///     }
    /// }
    /// </code></example>
    [Serializable]
    public struct LocalizableContent
    {
        /// <summary>
        /// Text that will be used if no matching locale is found.
        /// </summary>
        [JsonProperty("default", NullValueHandling = NullValueHandling.Ignore)]
        public string Default;
        /// <summary>
        /// Key value pairs of localized strings.
        /// </summary>
        [JsonProperty("localizations", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Localizations;
    }
}
