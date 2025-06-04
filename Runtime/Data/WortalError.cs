using System;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Represents an error returned from the Wortal SDK. You can check the ErrorCode property to determine the type
    /// of error, and the Message property for more details. The URL property will contain a link to the relevant API
    /// docs for the error.
    /// </summary>
    [Serializable]
    public struct WortalError
    {
        /// <summary>
        /// Error code. This can be compared to <see cref="WortalErrorCodes"/> for determining the type of error.
        /// </summary>
        [JsonProperty("code")]
        public string Code;
        /// <summary>
        /// Details about the error.
        /// </summary>
        [JsonProperty("message")]
        public string Message;
        /// <summary>
        /// Any context provided about the error, such as the calling method that caught the error.
        /// </summary>
        [JsonProperty("context")]
        public string Context;
        /// <summary>
        /// URL to the relevant API docs for this error.
        /// </summary>
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string URL;

        /// <summary>
        /// Returns the enum value of the parsed error code.
        /// </summary>
        public WortalErrorCodes ErrorCode => (WortalErrorCodes)Enum.Parse(typeof(WortalErrorCodes), Code);

        public override string ToString()
        {
            return $"Code: {Code}, Message: {Message}, Context: {Context}";
        }
    }
}
