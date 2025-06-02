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
    /// Leaderboard API
    /// </summary>
    public class WortalLeaderboard
    {
        private static Action<Leaderboard> _getLeaderboardCallback;
        private static Action<LeaderboardEntry> _sendEntryCallback;
        private static Action<LeaderboardEntry[]> _getEntriesCallback;
        private static Action<LeaderboardEntry> _getPlayerEntryCallback;
        private static Action<int> _getEntryCountCallback;
        private static Action<LeaderboardEntry[]> _getConnectedPlayersEntriesCallback;

#region Public API

        /// <summary>
        /// Gets the leaderboard with the given name. Access the leaderboard API via the Leaderboard returned here.
        /// </summary>
        /// <param name="name">Name of the leaderboard.</param>
        /// <param name="callback">Callback with the leaderboard. Fired after JS async function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Leaderboard.GetLeaderboard("global",
        ///     board => Debug.Log("Leaderboard: " + board.Name),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>LEADERBOARD_NOT_FOUND</li>
        /// <li>NETWORK_FAILURE</li>
        /// <li>CLIENT_UNSUPPORTED_OPERATION</li>
        /// <li>INVALID_OPERATION</li>
        /// <li>INVALID_PARAM</li>
        /// </ul></throws>
        public void GetLeaderboard(string name, Action<Leaderboard> callback, Action<WortalError> errorCallback)
        {
            _getLeaderboardCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            LeaderboardGetJS(name, LeaderboardGetCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log($"[Wortal] Mock Leaderboard.GetLeaderboard({name})");
            var leaderboard = new Leaderboard
            {
                Id = name, // Use the requested name for mock
                Name = name, // Use the requested name for mock
                ContextID = "mockContextID" // Added for completeness if needed
            };
            _getLeaderboardCallback?.Invoke(leaderboard);
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Leaderboard.GetLeaderboard({name}) not supported on Android. Returning null.");
            _getLeaderboardCallback?.Invoke(null);
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Leaderboard.GetLeaderboard({name}) not supported on iOS. Returning null.");
            _getLeaderboardCallback?.Invoke(null);
#else
            Debug.LogWarning($"[Wortal] Leaderboard.GetLeaderboard({name}) not supported on this platform. Returning null.");
            _getLeaderboardCallback?.Invoke(null);
#endif
        }

        /// <inheritdoc cref="SendEntry(string,int,string,System.Action{DigitalWill.WortalSDK.LeaderboardEntry},System.Action{DigitalWill.WortalSDK.WortalError})"/>
        public void SendEntry(string name, int score, Action<LeaderboardEntry> callback, Action<WortalError> errorCallback)
        {
            SendEntry(name, score, "", callback, errorCallback);
        }

        /// <summary>
        /// Sends an entry to be added to the leaderboard, or updated if already existing. Will only update if the score
        /// is a higher than the player's previous entry.
        /// </summary>
        /// <param name="name">Name of the leaderboard.</param>
        /// <param name="score">Score for the entry.</param>
        /// <param name="details">Additional details about the entry.</param>
        /// <param name="callback">Callback with new entry if one was created, updated entry if the score is higher, or the old entry if no new
        /// high score was achieved. Fired after JS async function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Leaderboard.SendEntry("global", 100,
        ///     entry => Debug.Log("Score: " + entry.Score),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>LEADERBOARD_WRONG_CONTEXT</li>
        /// <li>NETWORK_FAILURE</li>
        /// <li>CLIENT_UNSUPPORTED_OPERATION</li>
        /// <li>INVALID_OPERATION</li>
        /// <li>INVALID_PARAM</li>
        /// <li>RATE_LIMITED</li>
        /// </ul></throws>
        public void SendEntry(string name, int score, string details, Action<LeaderboardEntry> callback, Action<WortalError> errorCallback)
        {
            _sendEntryCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            LeaderboardSendEntryJS(name, score, details, LeaderboardSendEntryCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log($"[Wortal] Mock Leaderboard.SendEntry({name}, {score}, {details})");
            var entry = new LeaderboardEntry
            {
                Player = new WortalPlayer // Changed from Player to WortalPlayer for consistency
                {
                    ID = "mockPlayerID",
                    Name = "Mock Player",
                    Photo = "data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==",
                    IsFirstPlay = false,
                },
                Details = details,
                FormattedScore = score + " points",
                Rank = 1, // Mock rank
                Score = score,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };
            _sendEntryCallback?.Invoke(entry);
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Leaderboard.SendEntry({name}, {score}, {details}) not supported on Android. Returning null.");
            _sendEntryCallback?.Invoke(null);
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Leaderboard.SendEntry({name}, {score}, {details}) not supported on iOS. Returning null.");
            _sendEntryCallback?.Invoke(null);
#else
            Debug.LogWarning($"[Wortal] Leaderboard.SendEntry({name}, {score}, {details}) not supported on this platform. Returning null.");
            _sendEntryCallback?.Invoke(null);
#endif
        }

        /// <inheritdoc cref="GetEntries(string,int,int,System.Action{DigitalWill.WortalSDK.LeaderboardEntry[]},System.Action{DigitalWill.WortalSDK.WortalError})"/>
        public void GetEntries(string name, int count, Action<LeaderboardEntry[]> callback, Action<WortalError> errorCallback)
        {
            GetEntries(name, count, 0, callback, errorCallback);
        }

        /// <summary>
        /// Gets a list of leaderboard entries in the leaderboard.
        /// </summary>
        /// <param name="name">Name of the leaderboard.</param>
        /// <param name="count">Number of entries to get.</param>
        /// <param name="offset">Offset from the first entry (top rank) to start the count from. Default is 0.</param>
        /// <param name="callback">Callback with an array of leaderboard entries. Fired after JS async function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Leaderboard.GetEntries("global", 10,
        ///     entries => Debug.Log("Score: " + entries[0].Score),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>INVALID_PARAM</li>
        /// <li>NETWORK_FAILURE</li>
        /// <li>RATE_LIMITED</li>
        /// </ul></throws>
        public void GetEntries(string name, int count, int offset, Action<LeaderboardEntry[]> callback, Action<WortalError> errorCallback)
        {
            _getEntriesCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            LeaderboardGetEntriesJS(name, count, offset,
                LeaderboardGetEntriesCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log($"[Wortal] Mock Leaderboard.GetEntries({name}, {count}, {offset})");
            var entries = new System.Collections.Generic.List<LeaderboardEntry>();
            for (int i = 0; i < count; i++)
            {
                entries.Add(new LeaderboardEntry
                {
                    Player = new WortalPlayer
                    {
                        ID = $"mockPlayerID_{offset + i}",
                        Name = $"Mock Player {offset + i}",
                        Photo = "data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==",
                        IsFirstPlay = false,
                    },
                    Details = $"Details for rank {offset + i + 1}",
                    FormattedScore = (1000 - (offset + i) * 10) + " points",
                    Rank = offset + i + 1,
                    Score = 1000 - (offset + i) * 10,
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - (ulong)(offset + i * 60)
                });
            }
            _getEntriesCallback?.Invoke(entries.ToArray());
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Leaderboard.GetEntries({name}, {count}, {offset}) not supported on Android. Returning empty array.");
            _getEntriesCallback?.Invoke(Array.Empty<LeaderboardEntry>());
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Leaderboard.GetEntries({name}, {count}, {offset}) not supported on iOS. Returning empty array.");
            _getEntriesCallback?.Invoke(Array.Empty<LeaderboardEntry>());
#else
            Debug.LogWarning($"[Wortal] Leaderboard.GetEntries({name}, {count}, {offset}) not supported on this platform. Returning empty array.");
            _getEntriesCallback?.Invoke(Array.Empty<LeaderboardEntry>());
#endif
        }

        /// <summary>
        /// Gets the player's entry in the leaderboard.
        /// </summary>
        /// <param name="name">Name of the leaderboard.</param>
        /// <param name="callback">Callback with the leaderboard entry for the player. Fired after JS async function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Leaderboard.GetPlayerEntry("global",
        ///     entry => Debug.Log("Score: " + entry.Score),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>INVALID_PARAM</li>
        /// <li>INVALID_OPERATION</li>
        /// <li>NETWORK_FAILURE</li>
        /// <li>RATE_LIMITED</li>
        /// </ul></throws>
        public void GetPlayerEntry(string name, Action<LeaderboardEntry> callback, Action<WortalError> errorCallback)
        {
            _getPlayerEntryCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            LeaderboardGetPlayerEntryJS(name, LeaderboardGetPlayerEntryCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log($"[Wortal] Mock Leaderboard.GetPlayerEntry({name})");
            var entry = new LeaderboardEntry
            {
                Player = new WortalPlayer // Assuming this is the current player
                {
                    ID = "currentPlayerMockID",
                    Name = "Current Mock Player",
                    Photo = "data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==",
                    IsFirstPlay = false,
                },
                Details = "Player-specific details",
                FormattedScore = "550 points", // Mock score
                Rank = 5, // Mock rank
                Score = 550,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - 3600 // An hour ago
            };
            _getPlayerEntryCallback?.Invoke(entry);
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Leaderboard.GetPlayerEntry({name}) not supported on Android. Returning null.");
            _getPlayerEntryCallback?.Invoke(null);
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Leaderboard.GetPlayerEntry({name}) not supported on iOS. Returning null.");
            _getPlayerEntryCallback?.Invoke(null);
#else
            Debug.LogWarning($"[Wortal] Leaderboard.GetPlayerEntry({name}) not supported on this platform. Returning null.");
            _getPlayerEntryCallback?.Invoke(null);
#endif
        }

        /// <summary>
        /// Gets the total number of entries in the leaderboard.
        /// </summary>
        /// <param name="name">Name of the leaderboard.</param>
        /// <param name="callback">Callback with the entry count. Fired after JS async function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Leaderboard.GetEntryCount("global",
        ///     count => Debug.Log("Count: " + count),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>INVALID_PARAM</li>
        /// <li>NETWORK_FAILURE</li>
        /// <li>RATE_LIMITED</li>
        /// </ul></throws>
        public void GetEntryCount(string name, Action<int> callback, Action<WortalError> errorCallback)
        {
            _getEntryCountCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            LeaderboardGetEntryCountJS(name, LeaderboardGetEntryCountCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log($"[Wortal] Mock Leaderboard.GetEntryCount({name})");
            _getEntryCountCallback?.Invoke(100); // Mocking a larger count
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Leaderboard.GetEntryCount({name}) not supported on Android. Returning 0.");
            _getEntryCountCallback?.Invoke(0);
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Leaderboard.GetEntryCount({name}) not supported on iOS. Returning 0.");
            _getEntryCountCallback?.Invoke(0);
#else
            Debug.LogWarning($"[Wortal] Leaderboard.GetEntryCount({name}) not supported on this platform. Returning 0.");
            _getEntryCountCallback?.Invoke(0);
#endif
        }

        /// <inheritdoc cref="GetConnectedPlayersEntries(string,int,int,System.Action{DigitalWill.WortalSDK.LeaderboardEntry[]},System.Action{DigitalWill.WortalSDK.WortalError})"/>
        public void GetConnectedPlayersEntries(string name, int count, Action<LeaderboardEntry[]> callback, Action<WortalError> errorCallback)
        {
            GetConnectedPlayersEntries(name, count, 0, callback, errorCallback);
        }

        /// <summary>
        /// Gets a list of leaderboard entries of connected players in the leaderboard.
        /// </summary>
        /// <param name="name">Name of the leaderboard.</param>
        /// <param name="count">Number of entries to get.</param>
        /// <param name="offset">Offset from the first entry (top rank) to start the count from. Default is 0.</param>
        /// <param name="callback">Callback with an array of leaderboard entries. Fired after JS async function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Leaderboard.GetConnectedPlayersEntries("global", 10, 0,
        ///     entries => Debug.Log("Entries: " + entries.Length),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>INVALID_PARAM</li>
        /// <li>NETWORK_FAILURE</li>
        /// <li>RATE_LIMITED</li>
        /// </ul></throws>
        public void GetConnectedPlayersEntries(string name, int count, int offset, Action<LeaderboardEntry[]> callback, Action<WortalError> errorCallback)
        {
            _getConnectedPlayersEntriesCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            LeaderboardGetConnectedPlayersEntriesJS(name, count, offset,
                LeaderboardGetConnectedPlayersEntriesCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log($"[Wortal] Mock Leaderboard.GetConnectedPlayersEntries({name}, {count}, {offset})");
            var entries = new System.Collections.Generic.List<LeaderboardEntry>();
            // Mock a smaller list for connected players, distinct from general GetEntries
            int mockCount = Math.Min(count, 5); // Max 5 connected player entries for mock
            for (int i = 0; i < mockCount; i++)
            {
                entries.Add(new LeaderboardEntry
                {
                    Player = new WortalPlayer
                    {
                        ID = $"connectedPlayerID_{offset + i}",
                        Name = $"Connected Player {offset + i}",
                        Photo = "data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==",
                        IsFirstPlay = false,
                    },
                    Details = $"Connected player details for rank {offset + i + 1}",
                    FormattedScore = (800 - (offset + i) * 15) + " points", // Different scoring pattern
                    Rank = offset + i + 1,
                    Score = 800 - (offset + i) * 15,
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - (ulong)(offset + i * 120) // Different time pattern
                });
            }
            _getConnectedPlayersEntriesCallback?.Invoke(entries.ToArray()); // Corrected to use _getConnectedPlayersEntriesCallback
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Leaderboard.GetConnectedPlayersEntries({name}, {count}, {offset}) not supported on Android. Returning empty array.");
            _getConnectedPlayersEntriesCallback?.Invoke(Array.Empty<LeaderboardEntry>());
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Leaderboard.GetConnectedPlayersEntries({name}, {count}, {offset}) not supported on iOS. Returning empty array.");
            _getConnectedPlayersEntriesCallback?.Invoke(Array.Empty<LeaderboardEntry>());
#else
            Debug.LogWarning($"[Wortal] Leaderboard.GetConnectedPlayersEntries({name}, {count}, {offset}) not supported on this platform. Returning empty array.");
            _getConnectedPlayersEntriesCallback?.Invoke(Array.Empty<LeaderboardEntry>());
#endif
        }

#endregion Public API
#region JSlib Interface

#if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void LeaderboardGetJS(string name, Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void LeaderboardSendEntryJS(string name, int score, string details, Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void LeaderboardGetEntriesJS(string name, int count, int offset, Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void LeaderboardGetPlayerEntryJS(string name, Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void LeaderboardGetEntryCountJS(string name, Action<int> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void LeaderboardGetConnectedPlayersEntriesJS(string name, int count, int offset, Action<string> callback, Action<string> errorCallback);
#endif

#if UNITY_WEBGL
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
                WortalError error = new()
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message
                };

                Wortal.WortalError?.Invoke(error);
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
                WortalError error = new()
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message
                };

                Wortal.WortalError?.Invoke(error);
                return;
            }

            _sendEntryCallback?.Invoke(entryObj);
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
                WortalError error = new()
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message
                };

                Wortal.WortalError?.Invoke(error);
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
                WortalError error = new()
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message
                };

                Wortal.WortalError?.Invoke(error);
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
                WortalError error = new()
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message
                };

                Wortal.WortalError?.Invoke(error);
                return;
            }

            _getConnectedPlayersEntriesCallback?.Invoke(entriesObj);
        }
#endif

#endregion JSlib Interface
    }
}
