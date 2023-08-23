using System;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Payload for scheduling a notification.
    /// </summary>
    [Serializable]
    public struct NotificationPayload
    {
        /// <summary>
        /// The title of the notification.
        /// </summary>
        [JsonProperty("title")]
        public string Title;
        /// <summary>
        /// The body of the notification.
        /// </summary>
        [JsonProperty("body")]
        public string Body;
        /// <summary>
        /// URL to the icon of the notification. Defaults to game icon on Wortal if not provided.
        /// </summary>
        [JsonProperty("mediaURL", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string MediaURL;
        /// <summary>
        /// Label used to categorize notifications.
        /// </summary>
        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Label;
        /// <summary>
        /// Time from now (in seconds) to send the notification. Limited between 300 (5 minutes) and 6480000 (75 days).
        /// Limit of 5 pending scheduled notifications per recipient. Default is 1 day (86400 seconds).
        /// </summary>
        [JsonProperty("scheduleInterval", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ScheduleInterval;
    }
}
