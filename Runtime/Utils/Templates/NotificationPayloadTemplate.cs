#if UNITY_LOCALIZATION
using System;
using UnityEngine;
using UnityEngine.Localization;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Template for an notification to be scheduled.
    /// </summary>
    [CreateAssetMenu(fileName = "NotificationPayloadTemplate", menuName = "Wortal/Notification Payload Template")]
    public class NotificationPayloadTemplate : ScriptableObject
    {
#region Private Members

        [Header("Required Properties")]
        [Tooltip("The title of the notification.")]
        [SerializeField]
        private LocalizedString _title;
        [Tooltip("The body of the notification.")]
        [SerializeField]
        private LocalizedString _body;

        [Header("Optional Properties")]
        [Tooltip("URL to the icon of the notification. Defaults to game icon on Wortal if not provided.")]
        [SerializeField]
        private string _mediaURL;
        [Tooltip("Label used to categorize notifications.")]
        [SerializeField]
        private string _label;
        [Tooltip("Time from now (in seconds) to send the notification. Limited between 300 (5 minutes) and 6480000 (75 days). Limit of 5 pending scheduled notifications per recipient. Default is 1 day (86400 seconds).")]
        [SerializeField]
        private int _scheduleInterval = 86400;

#endregion Private Members
#region Public API

        /// <summary>
        /// Gets the payload object that can be passed into Wortal.Notification API calls.
        /// </summary>
        /// <returns>Payload to be passed into Notification API.</returns>
        /// <exception cref="MissingMemberException">Thrown if any required properties are missing or null.</exception>
        public NotificationPayload GetPayload()
        {
            if (_title.IsEmpty)
            {
                throw new MissingMemberException("[Wortal] Title is required for the notification payload to be sent.");
            }

            if (_body.IsEmpty)
            {
                throw new MissingMemberException("[Wortal] Body is required for the notification payload to be sent.");
            }

            var payload = new NotificationPayload
            {
                Title = _title.GetLocalizedString(),
                Body = _body.GetLocalizedString(),
                MediaURL = _mediaURL,
                Label = _label,
                ScheduleInterval = _scheduleInterval,
            };

            return payload;
        }

#endregion Public API
    }
}
#endif
