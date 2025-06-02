using System;
#if UNITY_WEBGL
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;
#endif
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
#if UNITY_WEBGL
            string payloadObj = JsonConvert.SerializeObject(payload);
            Debug.Log($"[Wortal] Notifications.Schedule({payloadObj})");
            NotificationsScheduleJS(payloadObj, NotificationsScheduleCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            string payloadJson = JsonConvert.SerializeObject(payload); // Keep for debug log
            Debug.Log($"[Wortal] Mock Notifications.Schedule({payloadJson})");
            var result = new NotificationScheduleResult
            {
                ID = Guid.NewGuid().ToString(), // More realistic mock ID
                Success = true,
            };
            _scheduleCallback?.Invoke(result);
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Notifications.Schedule not supported on Android. Returning Success = false.");
            _scheduleCallback?.Invoke(new NotificationScheduleResult { Success = false });
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Notifications.Schedule not supported on iOS. Returning Success = false.");
            _scheduleCallback?.Invoke(new NotificationScheduleResult { Success = false });
#else
            Debug.LogWarning($"[Wortal] Notifications.Schedule not supported on this platform. Returning Success = false.");
            _scheduleCallback?.Invoke(new NotificationScheduleResult { Success = false });
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
#if UNITY_WEBGL
            NotificationsGetHistoryJS(NotificationsGetHistoryCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log("[Wortal] Mock Notifications.GetHistory()");
            ScheduledNotification[] result = {
                new()
                {
                    ID = Guid.NewGuid().ToString(),
                    Status = NotificationStatus.SCHEDULED,
                    CreatedTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                    Label = "mockLabel1"
                },
                new()
                {
                    ID = Guid.NewGuid().ToString(),
                    Status = NotificationStatus.DELIVERED,
                    CreatedTime = (DateTimeOffset.UtcNow.ToUnixTimeSeconds() - 86400).ToString(), // Yesterday
                    Label = "mockLabel2"
                },
            };
            _getHistoryCallback?.Invoke(result);
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Notifications.GetHistory not supported on Android. Returning empty array.");
            _getHistoryCallback?.Invoke(Array.Empty<ScheduledNotification>());
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Notifications.GetHistory not supported on iOS. Returning empty array.");
            _getHistoryCallback?.Invoke(Array.Empty<ScheduledNotification>());
#else
            Debug.LogWarning("[Wortal] Notifications.GetHistory not supported on this platform. Returning empty array.");
            _getHistoryCallback?.Invoke(Array.Empty<ScheduledNotification>());
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
#if UNITY_WEBGL
            NotificationsCancelJS(id, NotificationsCancelCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log($"[Wortal] Mock Notifications.Cancel({id})");
            _cancelCallback?.Invoke(true);
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Notifications.Cancel({id}) not supported on Android. Returning false.");
            _cancelCallback?.Invoke(false);
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Notifications.Cancel({id}) not supported on iOS. Returning false.");
            _cancelCallback?.Invoke(false);
#else
            Debug.LogWarning($"[Wortal] Notifications.Cancel({id}) not supported on this platform. Returning false.");
            _cancelCallback?.Invoke(false);
#endif
        }

        /// <inheritdoc cref="CancelAll(string,System.Action{System.bool},System.Action{DigitalWill.WortalSDK.WortalError})"/>
        public void CancelAll(Action<bool> callback, Action<WortalError> errorCallback)
        {
            _cancelAllCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            NotificationsCancelAllJS(NotificationsCancelAllCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log("[Wortal] Mock Notifications.CancelAll() (no label)");
            _cancelAllCallback?.Invoke(true);
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Notifications.CancelAll() (no label) not supported on Android. Returning false.");
            _cancelAllCallback?.Invoke(false);
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Notifications.CancelAll() (no label) not supported on iOS. Returning false.");
            _cancelAllCallback?.Invoke(false);
#else
            Debug.LogWarning("[Wortal] Notifications.CancelAll() (no label) not supported on this platform. Returning false.");
            _cancelAllCallback?.Invoke(false);
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
#if UNITY_WEBGL
            NotificationsCancelAllLabelJS(label, NotificationsCancelAllCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log($"[Wortal] Mock Notifications.CancelAll(label: {label})");
            _cancelAllCallback?.Invoke(true);
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Notifications.CancelAll(label: {label}) not supported on Android. Returning false.");
            _cancelAllCallback?.Invoke(false);
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Notifications.CancelAll(label: {label}) not supported on iOS. Returning false.");
            _cancelAllCallback?.Invoke(false);
#else
            Debug.LogWarning($"[Wortal] Notifications.CancelAll(label: {label}) not supported on this platform. Returning false.");
            _cancelAllCallback?.Invoke(false);
#endif
        }

#endregion Public API
#region JSlib Interface

#if UNITY_WEBGL
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
#endif

#if UNITY_WEBGL
        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void NotificationsScheduleCallback(string result)
        {
            NotificationScheduleResult resultObj;

            try
            {
                resultObj = JsonConvert.DeserializeObject<NotificationScheduleResult>(result);
            }
            catch (Exception e)
            {
                WortalError error = new()
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message,
                };

                Wortal.WortalError?.Invoke(error);
                return;
            }

            _scheduleCallback?.Invoke(resultObj);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void NotificationsGetHistoryCallback(string result)
        {
            ScheduledNotification[] resultObj;

            try
            {
                resultObj = JsonConvert.DeserializeObject<ScheduledNotification[]>(result);
            }
            catch (Exception e)
            {
                WortalError error = new()
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message,
                };

                Wortal.WortalError?.Invoke(error);
                return;
            }

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
#endif

#endregion JSlib Interface
    }
}
