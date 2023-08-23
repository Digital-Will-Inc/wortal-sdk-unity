using System;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
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

        public WortalErrorCodes ErrorCode => (WortalErrorCodes)Enum.Parse(typeof(WortalErrorCodes), Code);

        public override string ToString()
        {
            return $"Code: {Code}, Message: {Message}, Context: {Context}";
        }
    }
}
