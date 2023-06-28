using System;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Result of a scheduled notification.
    /// </summary>
    [Serializable]
    public struct NotificationScheduleResult
    {
        /// <summary>
        /// ID of the scheduled notification.
        /// </summary>
        [JsonProperty("id")]
        public string ID;
        /// <summary>
        /// Whether the notification was successfully scheduled.
        /// </summary>
        [JsonProperty("success")]
        public bool Success;
    }
}
