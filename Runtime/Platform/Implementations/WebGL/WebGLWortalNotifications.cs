using System;
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class WebGLWortalNotifications : IWortalNotifications
    {
        private static Action<NotificationScheduleResult> _scheduleCallback;
        private static Action<ScheduledNotification[]> _getHistoryCallback;
        private static Action<bool> _cancelCallback;
        private static Action<bool> _cancelAllCallback;
        private static Action<WortalError> _errorCallback;

        public bool IsSupported => true; // Changed to true since we're implementing the functionality

        public void Schedule(NotificationPayload payload, Action<NotificationScheduleResult> callback, Action<WortalError> errorCallback)
        {
            _scheduleCallback = callback;
            _errorCallback = errorCallback;
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

        public void GetHistory(Action<ScheduledNotification[]> callback, Action<WortalError> errorCallback)
        {
            _getHistoryCallback = callback;
            _errorCallback = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            NotificationsGetHistoryJS(NotificationsGetHistoryCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock Notifications.GetHistory()");
            ScheduledNotification[] result = {
                new ScheduledNotification
                {
                    ID = "mock.ID",
                    Status = NotificationStatus.SCHEDULED,
                    CreatedTime = "0000000",
                },
            };
            NotificationsGetHistoryCallback(JsonConvert.SerializeObject(result));
#endif
        }

        public void Cancel(string id, Action<bool> callback, Action<WortalError> errorCallback)
        {
            _cancelCallback = callback;
            _errorCallback = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            NotificationsCancelJS(id, NotificationsCancelCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock Notifications.Cancel()");
            NotificationsCancelCallback(true);
#endif
        }

        public void CancelAll(Action<bool> callback, Action<WortalError> errorCallback)
        {
            _cancelAllCallback = callback;
            _errorCallback = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            NotificationsCancelAllJS(NotificationsCancelAllCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock Notifications.CancelAll()");
            NotificationsCancelAllCallback(true);
#endif
        }

        public void CancelAll(string label, Action<bool> callback, Action<WortalError> errorCallback)
        {
            _cancelAllCallback = callback;
            _errorCallback = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            NotificationsCancelAllLabelJS(label, NotificationsCancelAllCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock Notifications.CancelAll()");
            NotificationsCancelAllCallback(true);
#endif
        }

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

        #endregion JSlib Interface

        #region Callback Methods

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
                WortalError error = new WortalError
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message,
                    Context = "NotificationsScheduleCallback"
                };

                _errorCallback?.Invoke(error);
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
                WortalError error = new WortalError
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message,
                    Context = "NotificationsGetHistoryCallback"
                };

                _errorCallback?.Invoke(error);
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

        #endregion Callback Methods
    }
}
