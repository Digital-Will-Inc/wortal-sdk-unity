using System;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class iOSWortalStats : IWortalStats
    {
        public bool IsSupported => false;

        public void GetStats(string level, GetStatsPayload payload, Action<Stats[]> callback, Action<WortalError> errorCallback)
        {
            Debug.Log($"[iOS Platform] IWortalStats.GetStats({level}, {payload}) called - Not implemented");
            errorCallback?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "GetStats implementation"
            });
        }

        public void PostStats(string level, int value, PostStatsPayload payload, Action callback, Action<WortalError> errorCallback)
        {
            Debug.Log($"[iOS Platform] IWortalStats.PostStats({level}, {value}, {payload}) called - Not implemented");
            errorCallback?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "PostStats implementation"
            });
        }
    }
}