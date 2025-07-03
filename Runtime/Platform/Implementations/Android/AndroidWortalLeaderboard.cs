using System;
using System.Reflection;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class AndroidWortalLeaderboard : IWortalLeaderboard
    {
        private static Type _playGamesPlatformType;
        private static object _playGamesInstance;

        static AndroidWortalLeaderboard()
        {
            InitializeGooglePlayGamesReflection();
        }

        public bool IsSupported => _playGamesPlatformType != null;

        private static void InitializeGooglePlayGamesReflection()
        {
            try
            {
                _playGamesPlatformType = Type.GetType("GooglePlayGames.PlayGamesPlatform, GooglePlayGames");

                if (_playGamesPlatformType != null)
                {
                    var instanceProperty = _playGamesPlatformType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
                    _playGamesInstance = instanceProperty?.GetValue(null);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[Android] Google Play Games not available: {e.Message}");
            }
        }

        public void GetLeaderboardAsync(string name, Action<Leaderboard> onSuccess, Action<WortalError> onError)
        {
            if (!IsSupported)
            {
                Debug.LogWarning("[Android] Google Play Games not available");
                onError?.Invoke(new WortalError
                {
                    Code = "DEPENDENCY_MISSING",
                    Message = "Google Play Games SDK not found",
                    Context = "GetLeaderboardAsync"
                });
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                // Get platform-specific leaderboard ID
                var platformLeaderboardId = WortalSettings.Instance.GetPlatformLeaderboardId(name);
                
                Debug.Log($"[Android] Loading leaderboard: {name} -> {platformLeaderboardId}");

                if (string.IsNullOrEmpty(platformLeaderboardId))
                {
                    Debug.LogError($"[Android] No Google Play Games ID found for leaderboard: {name}");
                    onError?.Invoke(new WortalError
                    {
                        Code = "LEADERBOARD_NOT_CONFIGURED",
                        Message = $"Leaderboard '{name}' is not configured for Google Play Games",
                        Context = "GetLeaderboardAsync"
                    });
                    return;
                }

                var loadScoresMethod = _playGamesPlatformType.GetMethod("LoadScores", new[] { typeof(string), typeof(Action<>).MakeGenericType(typeof(object[])) });
                
                Action<object[]> scoresCallback = (gpgScores) =>
                {
                    if (gpgScores == null)
                    {
                        Debug.LogWarning($"[Android] Failed to load leaderboard: {name}");
                        onError?.Invoke(new WortalError
                        {
                            Code = "LOAD_FAILED",
                            Message = $"Failed to load leaderboard {name}",
                            Context = "GetLeaderboardAsync"
                        });
                        return;
                    }

                    Debug.Log($"[Android] Loaded {gpgScores.Length} scores from leaderboard: {name}");

                    var entries = new LeaderboardEntry[gpgScores.Length];
                    for (int i = 0; i < gpgScores.Length; i++)
                    {
                        var score = gpgScores[i];
                        var scoreType = score.GetType();
                        
                        var playerId = (string)scoreType.GetProperty("userID")?.GetValue(score) ?? "";
                        var player = new Player
                        {
                            ID = playerId,
                            Name = Social.localUser != null && Social.localUser.id == playerId
                                ? (Social.localUser.userName ?? "Google Play User")
                                : "Google Play User"
                        };

                        entries[i] = new LeaderboardEntry
                        {
                            Player = player,
                            Rank = (int)(scoreType.GetProperty("rank")?.GetValue(score) ?? 0),
                            Score = (int)((long)(scoreType.GetProperty("value")?.GetValue(score) ?? 0)),
                            FormattedScore = scoreType.GetProperty("formattedValue")?.GetValue(score)?.ToString() ?? "",
                            Timestamp = (int)((long)(scoreType.GetProperty("date")?.GetValue(score) ?? 0)),
                            Details = null
                        };
                    }

                    var leaderboard = new Leaderboard
                    {
                        Id = platformLeaderboardId,
                        Name = name,
                    };

                    onSuccess?.Invoke(leaderboard);
                };

                loadScoresMethod?.Invoke(_playGamesInstance, new object[] { platformLeaderboardId, scoresCallback });
            }
            catch (Exception e)
            {
                Debug.LogError($"[Android] Error loading leaderboard: {e.Message}");
                onError?.Invoke(new WortalError
                {
                    Code = "LOAD_ERROR",
                    Message = $"Error loading leaderboard: {e.Message}",
                    Context = "GetLeaderboardAsync"
                });
            }
#else
            Debug.LogWarning($"[Android] GetLeaderboardAsync called on non-Android platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "GetLeaderboardAsync is only supported on Android platform",
                Context = "GetLeaderboardAsync"
            });
#endif
        }

        public void SendScoreAsync(string name, int score, Action<LeaderboardEntry> onSuccess, Action<WortalError> onError)
        {
            if (!IsSupported)
            {
                Debug.LogWarning("[Android] Google Play Games not available");
                onError?.Invoke(new WortalError
                {
                    Code = "DEPENDENCY_MISSING",
                    Message = "Google Play Games SDK not found",
                    Context = "SendScoreAsync"
                });
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                // Get platform-specific leaderboard ID
                var platformLeaderboardId = WortalSettings.Instance.GetPlatformLeaderboardId(name);
                
                Debug.Log($"[Android] Submitting score {score} to leaderboard: {name} -> {platformLeaderboardId}");

                if (string.IsNullOrEmpty(platformLeaderboardId))
                {
                    Debug.LogError($"[Android] No Google Play Games ID found for leaderboard: {name}");
                    onError?.Invoke(new WortalError
                    {
                        Code = "LEADERBOARD_NOT_CONFIGURED",
                        Message = $"Leaderboard '{name}' is not configured for Google Play Games",
                        Context = "SendScoreAsync"
                    });
                    return;
                }

                var reportScoreMethod = _playGamesPlatformType.GetMethod("ReportScore", new[] { typeof(long), typeof(string), typeof(Action<bool>) });
                
                Action<bool> scoreCallback = (success) =>
                {
                    if (success)
                    {
                        var playerId = Social.localUser.id ?? "";
                        var player = new Player
                        {
                            ID = playerId,
                            Name = Social.localUser != null && Social.localUser.id == playerId
                                ? (Social.localUser.userName ?? "Google Play User")
                                : "Google Play User"
                        };

                        var entry  = new LeaderboardEntry
                        {
                            Player = player,
                            Rank = 0, // Rank unknown after submission
                            Score = score,
                            FormattedScore = score.ToString(),
                            Details = null
                        };

                        onSuccess?.Invoke(entry);
                    }
                    else
                    {
                        Debug.LogError($"[Android] Failed to submit score {score} to leaderboard: {name}");
                        onError?.Invoke(new WortalError
                        {
                            Code = "SUBMIT_FAILED",
                            Message = $"Failed to submit score {score} to leaderboard {name}",
                            Context = "SendScoreAsync"
                        });
                    }
                };

                reportScoreMethod?.Invoke(_playGamesInstance, new object[] { (long)score, platformLeaderboardId, scoreCallback });
            }
            catch (Exception e)
            {
                Debug.LogError($"[Android] Error submitting score: {e.Message}");
                onError?.Invoke(new WortalError
                {
                    Code = "SUBMIT_ERROR",
                    Message = $"Error submitting score: {e.Message}",
                    Context = "SendScoreAsync"
                });
            }
#else
            Debug.LogWarning($"[Android] SendScoreAsync called on non-Android platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "SendScoreAsync is only supported on Android platform",
                Context = "SendScoreAsync"
            });
#endif
        }

        public void SendScoreAsync(string name, int score, string details, Action<LeaderboardEntry> onSuccess, Action<WortalError> onError)
        {
            // Details parameter is ignored on Google Play Games
            Debug.Log($"[Android] SendScoreAsync with details - details parameter ignored on Google Play Games");
            SendScoreAsync(name, score, onSuccess, onError);
        }

        public void GetEntriesAsync(string name, int count, int offset, Action<LeaderboardEntry[]> onSuccess, Action<WortalError> onError)
        {
            Debug.LogWarning($"[Android] GetEntriesAsync with pagination is not supported on Google Play Games");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "GetEntriesAsync with pagination is not supported on Google Play Games. Use GetLeaderboardAsync instead.",
                Context = "GetEntriesAsync"
            });
        }

        public void GetPlayerEntryAsync(string name, Action<LeaderboardEntry> onSuccess, Action<WortalError> onError)
        {
            if (!IsSupported)
            {
                Debug.LogWarning("[Android] Google Play Games not available");
                onError?.Invoke(new WortalError
                {
                    Code = "DEPENDENCY_MISSING",
                    Message = "Google Play Games SDK not found",
                    Context = "GetPlayerEntryAsync"
                });
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                // Get platform-specific leaderboard ID
                var platformLeaderboardId = WortalSettings.Instance.GetPlatformLeaderboardId(name);
                
                Debug.Log($"[Android] Getting player entry for leaderboard: {name} -> {platformLeaderboardId}");

                if (string.IsNullOrEmpty(platformLeaderboardId))
                {
                    Debug.LogError($"[Android] No Google Play Games ID found for leaderboard: {name}");
                    onError?.Invoke(new WortalError
                    {
                        Code = "LEADERBOARD_NOT_CONFIGURED",
                        Message = $"Leaderboard '{name}' is not configured for Google Play Games",
                        Context = "GetPlayerEntryAsync"
                    });
                    return;
                }

                var loadScoresMethod = _playGamesPlatformType.GetMethod("LoadScores", new[] { typeof(string), typeof(Action<>).MakeGenericType(typeof(object[])) });
                
                Action<object[]> scoresCallback = (scores) =>
                {
                    if (scores == null)
                    {
                        Debug.LogWarning($"[Android] Failed to load player entry for leaderboard: {name}");
                        onError?.Invoke(new WortalError
                        {
                            Code = "LOAD_FAILED",
                            Message = $"Failed to load player entry for leaderboard {name}",
                            Context = "GetPlayerEntryAsync"
                        });
                        return;
                    }

                    // Find the local player's score
                    foreach (var score in scores)
                    {
                        var scoreType = score.GetType();
                        var userID = (string)scoreType.GetProperty("userID")?.GetValue(score);
                        
                        if (userID == Social.localUser.id)
                        {
                            Debug.Log($"[Android] Found player entry for leaderboard: {name}");

                            var playerId = userID;
                            var player = new Player
                            {
                                ID = playerId,
                                Name = Social.localUser != null && Social.localUser.id == playerId
                                    ? (Social.localUser.userName ?? "Google Play User")
                                    : "Google Play User"
                            };

                            var entry  = new LeaderboardEntry
                            {
                                Player = player,
                                Rank = (int)(scoreType.GetProperty("rank")?.GetValue(score) ?? 0),
                                Score = (int)((long)(scoreType.GetProperty("value")?.GetValue(score) ?? 0)),
                                FormattedScore = scoreType.GetProperty("value")?.GetValue(score)?.ToString() ?? "",
                                Details = null
                            };
                            onSuccess?.Invoke(entry);
                            return;
                        }
                    }

                    Debug.LogWarning($"[Android] Player entry not found in leaderboard: {name}");
                    onError?.Invoke(new WortalError
                    {
                        Code = "NOT_FOUND",
                        Message = "Player entry not found in leaderboard",
                        Context = "GetPlayerEntryAsync"
                    });
                };

                loadScoresMethod?.Invoke(_playGamesInstance, new object[] { platformLeaderboardId, scoresCallback });
            }
            catch (Exception e)
            {
                Debug.LogError($"[Android] Error getting player entry: {e.Message}");
                onError?.Invoke(new WortalError
                {
                    Code = "GET_ENTRY_ERROR",
                    Message = $"Error getting player entry: {e.Message}",
                    Context = "GetPlayerEntryAsync"
                });
            }
#else
            Debug.LogWarning($"[Android] GetPlayerEntryAsync called on non-Android platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "GetPlayerEntryAsync is only supported on Android platform",
                Context = "GetPlayerEntryAsync"
            });
#endif
        }

        public void GetEntryCountAsync(string name, int count, Action<LeaderboardEntry[]> onSuccess, Action<WortalError> onError)
        {
            Debug.LogWarning($"[Android] GetEntryCountAsync is not supported on Google Play Games");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "GetEntryCountAsync is not supported on Google Play Games. Use GetLeaderboardAsync instead.",
                Context = "GetEntryCountAsync"
            });
        }

        public void GetConnectedPlayerEntriesAsync(string name, int count, int offset, Action<LeaderboardEntry[]> onSuccess, Action<WortalError> onError)
        {
            Debug.LogWarning($"[Android] GetConnectedPlayerEntriesAsync is not supported on Google Play Games");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "GetConnectedPlayerEntriesAsync is not supported on Google Play Games. Use GetLeaderboardAsync instead.",
                Context = "GetConnectedPlayerEntriesAsync"
            });
        }
    }
}
