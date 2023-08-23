using System;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Notification that has been scheduled.
    /// </summary>
    [Serializable]
    public struct ScheduledNotification
    {
        /// <summary>
        /// The ID of the notification.
        /// </summary>
        [JsonProperty("id")]
        public string ID;
        /// <summary>
        /// Status of the notification.
        /// </summary>
        [JsonProperty("status")]
        public NotificationStatus Status;
        /// <summary>
        /// Label for the notification category.
        /// </summary>
        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Label;
        /// <summary>
        /// Time the notification was scheduled on. This is not the time the notification will be sent on.
        /// </summary>
        [JsonProperty("createdTime")]
        public string CreatedTime;
    }
}
