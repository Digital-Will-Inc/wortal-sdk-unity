using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Represents a custom link to be shared by the user.
    /// </summary>
    [Serializable]
    public struct LinkSharePayload
    {
        /// <summary>
        /// A base64 encoded image to be shown for the link preview. The image is recommended to either be a square or of
        /// the aspect ratio 1.91:1.
        /// </summary>
        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Image;
        /// <summary>
        /// A text description for the link preview. Recommended to be less than 44 characters.
        /// </summary>
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public LocalizableContent Text;
        /// <summary>
        /// A blob of data to associate with the shared link. All game sessions launched from the share will be able to
        /// access this blob through <code>Wortal.session.getEntryPointData()</code>.
        /// </summary>
        [JsonProperty("data")]
        public Dictionary<string, object> Data;
    }
}
