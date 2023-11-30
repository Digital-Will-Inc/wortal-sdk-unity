using System;
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;
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
            string payloadJson = JsonConvert.SerializeObject(payload);
#if UNITY_WEBGL && !UNITY_EDITOR
            StatsGetStatsJS(level, payloadJson, StatsGetStatsCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock Stats.GetStats()");
            callback?.Invoke(new Stats[]
            {
                new()
                {
                    Level = "Mock Level",
                    Value = 100,
                    Period = StatPeriod.WEEKLY,
                },
                new()
                {
                    Level = "Mock Level",
                    Value = 150,
                    Period = StatPeriod.ALLTIME,
                },
            });
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
            string payloadJson = JsonConvert.SerializeObject(payload);
#if UNITY_WEBGL && !UNITY_EDITOR
            StatsPostStatsJS(level, value, payloadJson, StatsPostStatsCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Stats.PostStats() + {level} + {value} + {payloadJson}");
            callback?.Invoke();
#endif
        }

#endregion Public API
#region JSlib Interface

        [DllImport("__Internal")]
        private static extern void StatsGetStatsJS(string level, string payload, Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void StatsPostStatsJS(string level, int value, string payload, Action callback, Action<string> errorCallback);

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void StatsGetStatsCallback(string statsJson)
        {
            Stats[] stats = JsonConvert.DeserializeObject<Stats[]>(statsJson);
            _getStatsCallback?.Invoke(stats);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void StatsPostStatsCallback()
        {
            _postStatsCallback?.Invoke();
        }


#endregion JSlib Interface
    }
}
