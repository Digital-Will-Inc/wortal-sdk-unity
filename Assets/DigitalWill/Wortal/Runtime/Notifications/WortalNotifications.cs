using System;
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class WortalNotifications
    {
        private static Action<NotificationScheduleResult> _scheduleCallback;
        private static Action<ScheduledNotification[]> _getHistoryCallback;
        private static Action<bool> _cancelCallback;
        private static Action<bool> _cancelAllCallback;

#region Public API

        /// <summary>
        /// Schedule a notification to be delivered to the player at a later time.
        /// </summary>
        /// <param name="payload">Object defining the notification to be scheduled.</param>
        /// <param name="callback">Callback that contains the result of the scheduled notification.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Notifications.Schedule(payload,
        ///     result => Debug.Log(result.id),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>INVALID_PARAM</li>
        /// <li>OPERATION_FAILED</li>
        /// </ul></throws>
        public void Schedule(NotificationPayload payload, Action<NotificationScheduleResult> callback, Action<WortalError> errorCallback)
        {
            _scheduleCallback = callback;
            Wortal.WortalError = errorCallback;
            string payloadObj = JsonConvert.SerializeObject(payload);
#if UNITY_WEBGL && !UNITY_EDITOR
            Debug.Log($"[Wortal] Notifications.Schedule({payloadObj})");
            NotificationsScheduleJS(payloadObj, NotificationsScheduleCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Notifications.Schedule({payload})");
            var result = new NotificationScheduleResult
            {
                ID = "mock.ID",
                Success = true,
            };
            NotificationsScheduleCallback(JsonConvert.SerializeObject(result));
#endif
        }

        /// <summary>
        /// Gets the history of scheduled notifications for the past 30 days.
        /// </summary>
        /// <param name="callback">Callback that contains an array of notifications scheduled.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Notifications.GetHistory(
        ///     results => Debug.Log(results[0].id),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>OPERATION_FAILED</li>
        /// </ul></throws>
        public void GetHistory(Action<ScheduledNotification[]> callback, Action<WortalError> errorCallback)
        {
            _getHistoryCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            NotificationsGetHistoryJS(NotificationsGetHistoryCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock Notifications.GetHistory()");
            ScheduledNotification[] result = {
                new()
                {
                    ID = "mock.ID",
                    Status = NotificationStatus.SCHEDULED,
                    CreatedTime = "0000000",
                },
            };
            NotificationsGetHistoryCallback(JsonConvert.SerializeObject(result));
#endif
        }

        /// <summary>
        /// Cancels a scheduled notification.
        /// </summary>
        /// <param name="id">ID of the notification to cancel.</param>
        /// <param name="callback">Callback that returns true if the notification was cancelled successfully, false otherwise.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Notifications.Cancel(id,
        ///     result => Debug.Log(result),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>INVALID_PARAM</li>
        /// <li>OPERATION_FAILED</li>
        /// </ul></throws>
        public void Cancel(string id, Action<bool> callback, Action<WortalError> errorCallback)
        {
            _cancelCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            NotificationsCancelJS(id, NotificationsCancelCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock Notifications.Cancel()");
            NotificationsCancelCallback(true);
#endif
        }

        /// <inheritdoc cref="CancelAll(string,System.Action{System.bool},System.Action{DigitalWill.WortalSDK.WortalError})"/>
        public void CancelAll(Action<bool> callback, Action<WortalError> errorCallback)
        {
            _cancelAllCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            NotificationsCancelAllJS(NotificationsCancelAllCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock Notifications.CancelAll()");
            NotificationsCancelAllCallback(true);
#endif
        }

        /// <summary>
        /// Cancels all scheduled notifications.
        /// </summary>
        /// <param name="label">Optional label of the notification category to cancel. If this is used then only notifications with the
        /// specified label will be cancelled.</param>
        /// <param name="callback">Callback that returns true if the notifications were cancelled successfully, false otherwise.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Notifications.CancelAll(label,
        ///     result => Debug.Log(result),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>OPERATION_FAILED</li>
        /// </ul></throws>
        public void CancelAll(string label, Action<bool> callback, Action<WortalError> errorCallback)
        {
            _cancelAllCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            NotificationsCancelAllLabelJS(label, NotificationsCancelAllCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock Notifications.CancelAll()");
            NotificationsCancelAllCallback(true);
#endif
        }

#endregion Public API
#region JSlib Interface

        [DllImport("__Internal")]
        private static extern void NotificationsScheduleJS(string payload, Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void NotificationsGetHistoryJS(Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void NotificationsCancelJS(string id, Action<bool> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void NotificationsCancelAllJS(Action<bool> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void NotificationsCancelAllLabelJS(string label, Action<bool> callback, Action<string> errorCallback);

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void NotificationsScheduleCallback(string result)
        {
            NotificationScheduleResult resultObj = JsonConvert.DeserializeObject<NotificationScheduleResult>(result);
            _scheduleCallback?.Invoke(resultObj);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void NotificationsGetHistoryCallback(string result)
        {
            ScheduledNotification[] resultObj = JsonConvert.DeserializeObject<ScheduledNotification[]>(result);
            _getHistoryCallback?.Invoke(resultObj);
        }

        [MonoPInvokeCallback(typeof(Action<bool>))]
        private static void NotificationsCancelCallback(bool result)
        {
            _cancelCallback?.Invoke(result);
        }

        [MonoPInvokeCallback(typeof(Action<bool>))]
        private static void NotificationsCancelAllCallback(bool result)
        {
            _cancelAllCallback?.Invoke(result);
        }

#endregion JSlib Interface
    }
}
