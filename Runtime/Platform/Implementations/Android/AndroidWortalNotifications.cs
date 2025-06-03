using System;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class AndroidWortalNotifications : IWortalNotifications
    {
        public bool IsSupported => false;

        public void Schedule(NotificationPayload payload, Action<NotificationScheduleResult> callback, Action<WortalError> errorCallback)
        {
            Debug.Log($"[Android Platform] IWortalNotifications.Schedule({payload}) called - Not implemented");
            errorCallback?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "Schedule implementation"
            });
        }

        public void GetHistory(Action<ScheduledNotification[]> callback, Action<WortalError> errorCallback)
        {
            Debug.Log("[Android Platform] IWortalNotifications.GetHistory() called - Not implemented");
            errorCallback?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "GetHistory implementation"
            });
        }

        public void Cancel(string id, Action<bool> callback, Action<WortalError> errorCallback)
        {
            Debug.Log($"[Android Platform] IWortalNotifications.Cancel({id}) called - Not implemented");
            errorCallback?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "Cancel implementation"
            });
        }

        public void CancelAll(Action<bool> callback, Action<WortalError> errorCallback)
        {
            Debug.Log("[Android Platform] IWortalNotifications.CancelAll() called - Not implemented");
            errorCallback?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "CancelAll implementation"
            });
        }

        public void CancelAll(string label, Action<bool> callback, Action<WortalError> errorCallback)
        {
            Debug.Log($"[Android Platform] IWortalNotifications.CancelAll({label}) called - Not implemented");
            errorCallback?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "CancelAll implementation"
            });
        }
    }
}