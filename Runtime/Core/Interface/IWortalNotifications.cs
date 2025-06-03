using System;
using DigitalWill.WortalSDK;

namespace DigitalWill.WortalSDK.Core
{
    /// <summary>
    /// Interface for notifications functionality across platforms
    /// </summary>
    public interface IWortalNotifications
    {
        /// <summary>
        /// Schedule a notification to be delivered to the player at a later time.
        /// </summary>
        /// <param name="payload">Object defining the notification to be scheduled.</param>
        /// <param name="callback">Callback that contains the result of the scheduled notification.</param>
        /// <param name="errorCallback">Error callback event with WortalError describing the error.</param>
        void Schedule(NotificationPayload payload, Action<NotificationScheduleResult> callback, Action<WortalError> errorCallback);

        /// <summary>
        /// Gets the history of scheduled notifications for the past 30 days.
        /// </summary>
        /// <param name="callback">Callback that contains an array of notifications scheduled.</param>
        /// <param name="errorCallback">Error callback event with WortalError describing the error.</param>
        void GetHistory(Action<ScheduledNotification[]> callback, Action<WortalError> errorCallback);

        /// <summary>
        /// Cancels a scheduled notification.
        /// </summary>
        /// <param name="id">ID of the notification to cancel.</param>
        /// <param name="callback">Callback that returns true if the notification was cancelled successfully, false otherwise.</param>
        /// <param name="errorCallback">Error callback event with WortalError describing the error.</param>
        void Cancel(string id, Action<bool> callback, Action<WortalError> errorCallback);

        /// <summary>
        /// Cancels all scheduled notifications.
        /// </summary>
        /// <param name="callback">Callback that returns true if the notifications were cancelled successfully, false otherwise.</param>
        /// <param name="errorCallback">Error callback event with WortalError describing the error.</param>
        void CancelAll(Action<bool> callback, Action<WortalError> errorCallback);

        /// <summary>
        /// Cancels all scheduled notifications with a specific label.
        /// </summary>
        /// <param name="label">Optional label of the notification category to cancel. If this is used then only notifications with the specified label will be cancelled.</param>
        /// <param name="callback">Callback that returns true if the notifications were cancelled successfully, false otherwise.</param>
        /// <param name="errorCallback">Error callback event with WortalError describing the error.</param>
        void CancelAll(string label, Action<bool> callback, Action<WortalError> errorCallback);

        /// <summary>
        /// Checks if notification features are supported on this platform
        /// </summary>
        bool IsSupported { get; }
    }
}