using System;
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class WebGLWortalLeaderboard : IWortalLeaderboard
    {
        private static Action<Leaderboard> _getLeaderboardCallback;
        private static Action<LeaderboardEntry> _sendScoreCallback;
        private static Action<LeaderboardEntry[]> _getEntriesCallback;
        private static Action<LeaderboardEntry> _getPlayerEntryCallback;
        private static Action<int> _getEntryCountCallback;
        private static Action<LeaderboardEntry[]> _getConnectedPlayerEntriesCallback;
        private static Action<WortalError> _errorCallback;

        public bool IsSupported => true; // Changed to true since we're implementing the functionality

        public void GetLeaderboardAsync(string name, Action<Leaderboard> onSuccess, Action<WortalError> onError)
        {
            _getLeaderboardCallback = onSuccess;
            _errorCallback = onError;
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.LeaderboardGetJS(name, LeaderboardGetCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Leaderboard.GetLeaderboardAsync({name})");
            var leaderboard = new Leaderboard
            {
                Id = name,
                Name = name,
            };
            LeaderboardGetCallback(JsonConvert.SerializeObject(leaderboard));
#endif
        }

        public void SendScoreAsync(string name, int score, Action<LeaderboardEntry> onSuccess, Action<WortalError> onError)
        {
            SendScoreAsync(name, score, "", onSuccess, onError);
        }

        public void SendScoreAsync(string name, int score, string details, Action<LeaderboardEntry> onSuccess, Action<WortalError> onError)
        {
            _sendScoreCallback = onSuccess;
            _errorCallback = onError;
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.LeaderboardSendEntryJS(name, score, details, LeaderboardSendEntryCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Leaderboard.SendScoreAsync({name}, {score}, {details})");
            var entry = new LeaderboardEntry
            {
                Player = new Player
                {
                    ID = "player1",
                    Name = "Player",
                    Photo = "data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==",
                    IsFirstPlay = false,
                    DaysSinceFirstPlay = 0,
                },
                Details = details,
                FormattedScore = $"{score} points",
                Rank = 1,
                Score = score,
                Timestamp = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            };
            LeaderboardSendEntryCallback(JsonConvert.SerializeObject(entry));
#endif
        }

        public void GetEntriesAsync(string name, int count, int offset, Action<LeaderboardEntry[]> onSuccess, Action<WortalError> onError)
        {
            _getEntriesCallback = onSuccess;
            _errorCallback = onError;
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.LeaderboardGetEntriesJS(name, count, offset, LeaderboardGetEntriesCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Leaderboard.GetEntriesAsync({name}, {count}, {offset})");
            var entry = new LeaderboardEntry
            {
                Player = new Player
                {
                    ID = "player1",
                    Name = "Player",
                    Photo = "data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==",
                    IsFirstPlay = false,
                    DaysSinceFirstPlay = 0,
                },
                Details = "",
                FormattedScore = "100 points",
                Rank = 1,
                Score = 100,
                Timestamp = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            };
            LeaderboardEntry[] entries = { entry };
            LeaderboardGetEntriesCallback(JsonConvert.SerializeObject(entries));
#endif
        }

        public void GetPlayerEntryAsync(string name, Action<LeaderboardEntry> onSuccess, Action<WortalError> onError)
        {
            _getPlayerEntryCallback = onSuccess;
            _errorCallback = onError;
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.LeaderboardGetPlayerEntryJS(name, LeaderboardGetPlayerEntryCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Leaderboard.GetPlayerEntryAsync({name})");
            var entry = new LeaderboardEntry
            {
                Player = new Player
                {
                    ID = "player1",
                    Name = "Player",
                    Photo = "data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==",
                    IsFirstPlay = false,
                    DaysSinceFirstPlay = 0,
                },
                Details = "",
                FormattedScore = "100 points",
                Rank = 1,
                Score = 100,
                Timestamp = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            };
            LeaderboardGetPlayerEntryCallback(JsonConvert.SerializeObject(entry));
#endif
        }

        public void GetEntryCountAsync(string name, int count, Action<LeaderboardEntry[]> onSuccess, Action<WortalError> onError)
        {
            // Note: This method signature seems incorrect in your interface. 
            // Based on the original implementation, this should return an int count, not LeaderboardEntry[]
            // For now, implementing as requested in the interface
            _getEntriesCallback = onSuccess;
            _errorCallback = onError;
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.LeaderboardGetEntriesJS(name, count, 0, LeaderboardGetEntriesCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Leaderboard.GetEntryCountAsync({name}, {count})");
            var entry = new LeaderboardEntry
            {
                Player = new Player
                {
                    ID = "player1",
                    Name = "Player",
                    Photo = "data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==",
                    IsFirstPlay = false,
                    DaysSinceFirstPlay = 0,
                },
                Details = "",
                FormattedScore = "100 points",
                Rank = 1,
                Score = 100,
                Timestamp = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            };
            LeaderboardEntry[] entries = { entry };
            LeaderboardGetEntriesCallback(JsonConvert.SerializeObject(entries));
#endif
        }

        public void GetConnectedPlayerEntriesAsync(string name, int count, int offset, Action<LeaderboardEntry[]> onSuccess, Action<WortalError> onError)
        {
            _getConnectedPlayerEntriesCallback = onSuccess;
            _errorCallback = onError;
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.LeaderboardGetConnectedPlayersEntriesJS(name, count, offset, LeaderboardGetConnectedPlayersEntriesCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Leaderboard.GetConnectedPlayerEntriesAsync({name}, {count}, {offset})");
            var entry = new LeaderboardEntry
            {
                Player = new Player
                {
                    ID = "player1",
                    Name = "Player",
                    Photo = "data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==",
                    IsFirstPlay = false,
                    DaysSinceFirstPlay = 0,
                },
                Details = "",
                FormattedScore = "100 points",
                Rank = 1,
                Score = 100,
                Timestamp = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            };
            LeaderboardEntry[] entries = { entry };
            LeaderboardGetConnectedPlayersEntriesCallback(JsonConvert.SerializeObject(entries));
#endif
        }

        /// <summary>
        /// Gets the total number of entries in the leaderboard.
        /// This is the correct signature for getting entry count.
        /// </summary>
        /// <param name="name">Name of the leaderboard.</param>
        /// <param name="callback">Callback with the entry count.</param>
        /// <param name="errorCallback">Error callback event with WortalError describing the error.</param>
        public void GetEntryCount(string name, Action<int> callback, Action<WortalError> errorCallback)
        {
            _getEntryCountCallback = callback;
            _errorCallback = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.LeaderboardGetEntryCountJS(name, LeaderboardGetEntryCountCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Leaderboard.GetEntryCount({name})");
            LeaderboardGetEntryCountCallback(1);
#endif
        }

        #region Callback Methods

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void LeaderboardGetCallback(string leaderboard)
        {
            Leaderboard leaderboardObj;

            try
            {
                leaderboardObj = JsonConvert.DeserializeObject<Leaderboard>(leaderboard);
            }
            catch (Exception e)
            {
                WortalError error = new WortalError
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message,
                    Context = "LeaderboardGetCallback"
                };

                _errorCallback?.Invoke(error);
                return;
            }

            _getLeaderboardCallback?.Invoke(leaderboardObj);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void LeaderboardSendEntryCallback(string entry)
        {
            LeaderboardEntry entryObj;

            try
            {
                entryObj = JsonConvert.DeserializeObject<LeaderboardEntry>(entry);
            }
            catch (Exception e)
            {
                WortalError error = new WortalError
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message,
                    Context = "LeaderboardSendEntryCallback"
                };

                _errorCallback?.Invoke(error);
                return;
            }

            _sendScoreCallback?.Invoke(entryObj);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void LeaderboardGetEntriesCallback(string entries)
        {
            LeaderboardEntry[] entriesObj;

            try
            {
                entriesObj = JsonConvert.DeserializeObject<LeaderboardEntry[]>(entries);
            }
            catch (Exception e)
            {
                WortalError error = new WortalError
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message,
                    Context = "LeaderboardGetEntriesCallback"
                };

                _errorCallback?.Invoke(error);
                return;
            }

            _getEntriesCallback?.Invoke(entriesObj);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void LeaderboardGetPlayerEntryCallback(string entry)
        {
            LeaderboardEntry entryObj;

            try
            {
                entryObj = JsonConvert.DeserializeObject<LeaderboardEntry>(entry);
            }
            catch (Exception e)
            {
                WortalError error = new WortalError
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message,
                    Context = "LeaderboardGetPlayerEntryCallback"
                };

                _errorCallback?.Invoke(error);
                return;
            }

            _getPlayerEntryCallback?.Invoke(entryObj);
        }

        [MonoPInvokeCallback(typeof(Action<int>))]
        private static void LeaderboardGetEntryCountCallback(int count)
        {
            _getEntryCountCallback?.Invoke(count);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void LeaderboardGetConnectedPlayersEntriesCallback(string entries)
        {
            LeaderboardEntry[] entriesObj;

            try
            {
                entriesObj = JsonConvert.DeserializeObject<LeaderboardEntry[]>(entries);
            }
            catch (Exception e)
            {
                WortalError error = new WortalError
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message,
                    Context = "LeaderboardGetConnectedPlayersEntriesCallback"
                };

                _errorCallback?.Invoke(error);
                return;
            }

            _getConnectedPlayerEntriesCallback?.Invoke(entriesObj);
        }

        #endregion Callback Methods
    }
}
