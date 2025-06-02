using System;
#if UNITY_WEBGL
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;
#endif
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Stats API for Wortal.
    /// </summary>
    public class WortalStats
    {
        private static Action<Stats[]> _getStatsCallback;
        private static Action _postStatsCallback;

#region Public API

        /// <summary>
        /// Gets a player's stats.
        /// </summary>
        /// <param name="level">The name of the level to get stats for.</param>
        /// <param name="payload">Payload with additional details about the stats.</param>
        /// <param name="callback">Callback with the stats. Fired after JS async function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Stats.GetStats(
        ///     "Level 1",
        ///     new GetStatsPayload { Period = StatPeriod.ALLTIME },
        ///     stats =>
        ///     {
        ///         foreach (Stats stat in stats)
        ///         {
        ///             Debug.Log(stat.Name);
        ///         }
        ///     },
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>INVALID_PARAMS</li>
        /// </ul></throws>
        public void GetStats(string level, GetStatsPayload payload, Action<Stats[]> callback, Action<WortalError> errorCallback)
        {
            _getStatsCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            string payloadJson = JsonConvert.SerializeObject(payload);
            StatsGetStatsJS(level, payloadJson, StatsGetStatsCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            string payloadJsonForLog = JsonConvert.SerializeObject(payload);
            Debug.Log($"[Wortal] Mock Stats.GetStats({level}, {payloadJsonForLog})");
            _getStatsCallback?.Invoke(new Stats[]
            {
                new()
                {
                    Level = level, // Use requested level
                    Value = 100,
                    Period = payload.Period ?? StatPeriod.ALLTIME, // Use requested period or default
                },
                new()
                {
                    Level = level,
                    Value = (payload.Period == StatPeriod.DAILY) ? 50 : 150, // Different value for daily
                    Period = payload.Period ?? StatPeriod.ALLTIME,
                },
            });
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Stats.GetStats({level}) not supported on Android. Returning empty array.");
            _getStatsCallback?.Invoke(Array.Empty<Stats>());
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Stats.GetStats({level}) not supported on iOS. Returning empty array.");
            _getStatsCallback?.Invoke(Array.Empty<Stats>());
#else
            Debug.LogWarning($"[Wortal] Stats.GetStats({level}) not supported on this platform. Returning empty array.");
            _getStatsCallback?.Invoke(Array.Empty<Stats>());
#endif
        }

        /// <summary>
        /// Posts a player's stats.
        /// </summary>
        /// <param name="level">The name of the level the stats are for.</param>
        /// <param name="value">The value of the stat.</param>
        /// <param name="payload">Payload with additional details about the stats.</param>
        /// <param name="callback">Callback. Fired after JS async function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Stats.PostStats(
        ///     "Level 1",
        ///     100,
        ///     new PostStatsPayload { Period = StatPeriod.DAILY },
        ///     callback => Debug.Log("Stats posted!"),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// };
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>INVALID_PARAMS</li>
        /// </ul></throws>
        public void PostStats(string level, int value, PostStatsPayload payload, Action callback, Action<WortalError> errorCallback)
        {
            _postStatsCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            string payloadJson = JsonConvert.SerializeObject(payload);
            StatsPostStatsJS(level, value, payloadJson, StatsPostStatsCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            string payloadJsonForLog = JsonConvert.SerializeObject(payload);
            Debug.Log($"[Wortal] Mock Stats.PostStats({level}, {value}, {payloadJsonForLog})");
            _postStatsCallback?.Invoke();
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Stats.PostStats({level}, {value}) not supported on Android.");
            _postStatsCallback?.Invoke();
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Stats.PostStats({level}, {value}) not supported on iOS.");
            _postStatsCallback?.Invoke();
#else
            Debug.LogWarning($"[Wortal] Stats.PostStats({level}, {value}) not supported on this platform.");
            _postStatsCallback?.Invoke();
#endif
        }

#endregion Public API
#region JSlib Interface

#if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void StatsGetStatsJS(string level, string payload, Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void StatsPostStatsJS(string level, int value, string payload, Action callback, Action<string> errorCallback);
#endif

#if UNITY_WEBGL
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
                WortalError error = new()
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message
                };

                Wortal.WortalError?.Invoke(error);
                return;
            }

            _getStatsCallback?.Invoke(stats);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void StatsPostStatsCallback()
        {
            _postStatsCallback?.Invoke();
        }
#endif

#endregion JSlib Interface
    }
}
