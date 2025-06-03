// Runtime/Core/Data/WortalError.cs
using System;

namespace DigitalWill.WortalSDK.Core
{
    /// <summary>
    /// Represents an error in the Wortal SDK
    /// </summary>
    [Serializable]
    public class WortalError
    {
        /// <summary>
        /// Error code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Additional error details
        /// </summary>
        public object Details { get; set; }

        public override string ToString()
        {
            return $"WortalError: {Code} - {Message}";
        }
    }

    /// <summary>
    /// Standard error codes used throughout the SDK
    /// </summary>
    public static class WortalErrorCodes
    {
        public const string INITIALIZATION_ERROR = "INITIALIZATION_ERROR";
        public const string NOT_SUPPORTED = "NOT_SUPPORTED";
        public const string INVALID_CONFIGURATION = "INVALID_CONFIGURATION";
        public const string AUTHENTICATION_FAILED = "AUTHENTICATION_FAILED";
        public const string NETWORK_ERROR = "NETWORK_ERROR";
        public const string INVALID_PARAMETER = "INVALID_PARAMETER";
        public const string OPERATION_CANCELLED = "OPERATION_CANCELLED";
        public const string PERMISSION_DENIED = "PERMISSION_DENIED";
        public const string RATE_LIMITED = "RATE_LIMITED";
        public const string SERVICE_UNAVAILABLE = "SERVICE_UNAVAILABLE";
        public const string UNKNOWN_ERROR = "UNKNOWN_ERROR";
    }
}
