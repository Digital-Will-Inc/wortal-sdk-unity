using System;
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class WebGLWortalStats : IWortalStats
    {
        private static Action<Stats[]> _getStatsCallback;
        private static Action _postStatsCallback;
        private static Action<WortalError> _errorCallback;

        public bool IsSupported => true; // Changed to true since we're implementing the functionality

        public void GetStats(string level, GetStatsPayload payload, Action<Stats[]> callback, Action<WortalError> errorCallback)
        {
            _getStatsCallback = callback;
            _errorCallback = errorCallback;
            string payloadJson = JsonConvert.SerializeObject(payload);
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.StatsGetStatsJS(level, payloadJson, StatsGetStatsCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Stats.GetStats({level}, {payloadJson})");
            var stats = new Stats[]
            {
                new()
                {
                    Level = level,
                    Value = 100,
                    Period = StatPeriod.WEEKLY,
                },
                new()
                {
                    Level = level,
                    Value = 150,
                    Period = StatPeriod.ALLTIME,
                },
            };
            StatsGetStatsCallback(JsonConvert.SerializeObject(stats));
#endif
        }

        public void PostStats(string level, int value, PostStatsPayload payload, Action callback, Action<WortalError> errorCallback)
        {
            _postStatsCallback = callback;
            _errorCallback = errorCallback;
            string payloadJson = JsonConvert.SerializeObject(payload);
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.StatsPostStatsJS(level, value, payloadJson, StatsPostStatsCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Stats.PostStats({level}, {value}, {payloadJson})");
            StatsPostStatsCallback();
#endif
        }

        #region Callback Methods

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void StatsGetStatsCallback(string statsJson)
        {
            Stats[] stats;

            try
            {
                stats = JsonConvert.DeserializeObject<Stats[]>(statsJson);
            }
            catch (Exception e)
            {
                WortalError error = new WortalError
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message,
                    Context = "StatsGetStatsCallback"
                };

                _errorCallback?.Invoke(error);
                return;
            }

            _getStatsCallback?.Invoke(stats);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void StatsPostStatsCallback()
        {
            _postStatsCallback?.Invoke();
        }

        #endregion Callback Methods
    }
}
