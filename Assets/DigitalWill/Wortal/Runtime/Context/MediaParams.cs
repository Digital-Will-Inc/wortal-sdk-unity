using System;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Represents the media payload used by custom update and custom share.
    /// </summary>
    [Serializable]
    public struct MediaParams
    {
        /// <summary>
        /// URL of the gif to be displayed.
        /// </summary>
        [JsonProperty("gif")]
        public string GIF;
        /// <summary>
        /// URL of the video to be displayed.
        /// </summary>
        [JsonProperty("video")]
        public string Video;
    }
}
