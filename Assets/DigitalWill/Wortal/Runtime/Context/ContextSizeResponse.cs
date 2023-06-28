using System;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Response object for the Context.IsSizeBetween API.
    /// </summary>
    [Serializable]
    public struct ContextSizeResponse
    {
        [JsonProperty("answer")]
        public bool Answer;
        [JsonProperty("maxSize")]
        public int MaxSize;
        [JsonProperty("minSize")]
        public int MinSize;
    }
}
