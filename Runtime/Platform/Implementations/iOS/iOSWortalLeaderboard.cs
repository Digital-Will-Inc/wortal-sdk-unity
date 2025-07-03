using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace DigitalWill.WortalSDK
{
    public class iOSWortalLeaderboard : IWortalLeaderboard
    {
        public bool IsSupported => Application.platform == RuntimePlatform.IPhonePlayer;

        public void GetLeaderboardAsync(string name, Action<Leaderboard> onSuccess, Action<WortalError> onError)
        {
#if UNITY_IOS && !UNITY_EDITOR
            // Get platform-specific leaderboard ID
            var platformLeaderboardId = WortalSettings.Instance.GetPlatformLeaderboardId(name);
            
            Debug.Log($"[iOS] Loading leaderboard: {name} -> {platformLeaderboardId}");

            if (string.IsNullOrEmpty(platformLeaderboardId))
            {
                Debug.LogError($"[iOS] No Game Center ID found for leaderboard: {name}");
                onError?.Invoke(new WortalError
                {
                    Code = "LEADERBOARD_NOT_CONFIGURED",
                    Message = $"Leaderboard '{name}' is not configured for iOS Game Center",
                    Context = "GetLeaderboardAsync"
                });
                return;
            }

            Social.LoadScores(platformLeaderboardId, (scores) =>
            {
                if (scores == null)
                {
                    Debug.LogWarning($"[iOS] Failed to load leaderboard: {name}");
                    onError?.Invoke(new WortalError
                    {
                        Code = "LOAD_FAILED",
                        Message = $"Failed to load leaderboard {name}",
                        Context = "GetLeaderboardAsync"
                    });
                    return;
                }

                Debug.Log($"[iOS] Loaded {scores.Length} scores from leaderboard: {name}");

                var entries = new LeaderboardEntry[scores.Length];
                for (int i = 0; i < scores.Length; i++)
                {
                    var score = scores[i];

                    var playerId = score.userID ?? Social.localUser.id;
                    var player = new Player
                    {
                        ID = playerId,
                        Name = Social.localUser.userName ?? "Game Center User",
                    };
                    entries[i] = new LeaderboardEntry
                    {
                        Player = player,
                        Score = (int)score.value,
                        Rank = score.rank,
                        FormattedScore = score.formattedValue ?? score.value.ToString(),
                        Timestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds, // Game Center does not provide timestamp, using current time
                        Details = null // Game Center does not support details
                    };
                }

                var leaderboard = new Leaderboard
                {
                    Id = platformLeaderboardId,
                    Name = name
                };

                onSuccess?.Invoke(leaderboard);
            });
#else
            Debug.LogWarning($"[iOS] GetLeaderboardAsync called on non-iOS platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "GetLeaderboardAsync is only supported on iOS platform",
                Context = "GetLeaderboardAsync"
            });
#endif
        }

        public void SendScoreAsync(string name, int score, Action<LeaderboardEntry> onSuccess, Action<WortalError> onError)
        {
#if UNITY_IOS && !UNITY_EDITOR
            // Get platform-specific leaderboard ID
            var platformLeaderboardId = WortalSettings.Instance.GetPlatformLeaderboardId(name);
            
            Debug.Log($"[iOS] Submitting score {score} to leaderboard: {name} -> {platformLeaderboardId}");

            if (string.IsNullOrEmpty(platformLeaderboardId))
            {
                Debug.LogError($"[iOS] No Game Center ID found for leaderboard: {name}");
                onError?.Invoke(new WortalError
                {
                    Code = "LEADERBOARD_NOT_CONFIGURED",
                    Message = $"Leaderboard '{name}' is not configured for iOS Game Center",
                    Context = "SendScoreAsync"
                });
                return;
            }

            Social.ReportScore(score, platformLeaderboardId, (success) =>
            {
                if (success)
                {
                    Debug.Log($"[iOS] Successfully submitted score {score} to leaderboard: {name}");
                    var playerId = Social.localUser.id;
                    var player = new Player
                    {
                        ID = playerId,
                        Name = Social.localUser.userName ?? "Game Center User",
                    };

                    var entry = new LeaderboardEntry
                    {
                        Player = player,
                        Score = score,
                        Rank = 0, // Rank will be updated when leaderboard is fetched
                        FormattedScore = score.ToString(),
                        Timestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds, // Game Center does not provide timestamp, using current time
                        Details = null // Game Center does not support details
                    };
                    onSuccess?.Invoke(entry);
                }
                else
                {
                    Debug.LogError($"[iOS] Failed to submit score {score} to leaderboard: {name}");
                    onError?.Invoke(new WortalError
                    {
                        Code = "SUBMIT_FAILED",
                        Message = $"Failed to submit score {score} to leaderboard {name}",
                        Context = "SendScoreAsync"
                    });
                }
            });
#else
            Debug.LogWarning($"[iOS] SendScoreAsync called on non-iOS platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "SendScoreAsync is only supported on iOS platform",
                Context = "SendScoreAsync"
            });
#endif
        }

        public void SendScoreAsync(string name, int score, string details, Action<LeaderboardEntry> onSuccess, Action<WortalError> onError)
        {
            // Details parameter is ignored on iOS Game Center
            Debug.Log($"[iOS] SendScoreAsync with details - details parameter ignored on Game Center");
            SendScoreAsync(name, score, onSuccess, onError);
        }

        public void GetEntriesAsync(string name, int count, int offset, Action<LeaderboardEntry[]> onSuccess, Action<WortalError> onError)
        {
            Debug.LogWarning($"[iOS] GetEntriesAsync with pagination is not supported on iOS Game Center");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "GetEntriesAsync with pagination is not supported on iOS Game Center. Use GetLeaderboardAsync instead.",
                Context = "GetEntriesAsync"
            });
        }

        public void GetPlayerEntryAsync(string name, Action<LeaderboardEntry> onSuccess, Action<WortalError> onError)
        {
#if UNITY_IOS && !UNITY_EDITOR
            // Get platform-specific leaderboard ID
            var platformLeaderboardId = WortalSettings.Instance.GetPlatformLeaderboardId(name);
            
            Debug.Log($"[iOS] Getting player entry for leaderboard: {name} -> {platformLeaderboardId}");

            if (string.IsNullOrEmpty(platformLeaderboardId))
            {
                Debug.LogError($"[iOS] No Game Center ID found for leaderboard: {name}");
                onError?.Invoke(new WortalError
                {
                    Code = "LEADERBOARD_NOT_CONFIGURED",
                    Message = $"Leaderboard '{name}' is not configured for iOS Game Center",
                    Context = "GetPlayerEntryAsync"
                });
                return;
            }

            Social.LoadScores(platformLeaderboardId, (scores) =>
            {
                if (scores == null)
                {
                    Debug.LogWarning($"[iOS] Failed to load player entry for leaderboard: {name}");
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
                    if (score.userID == Social.localUser.id)
                    {
                        Debug.Log($"[iOS] Found player entry for leaderboard: {name}");
                        var playerId = score.userID ?? Social.localUser.id;
                        var player = new Player
                        {
                            ID = playerId,
                            Name = Social.localUser.userName ?? "Game Center User",
                        };

                        var entry = new LeaderboardEntry
                        {
                            Player = player,
                            Score = (int)score.value,
                            Rank = score.rank,
                            FormattedScore = score.formattedValue ?? score.value.ToString(),
                            Timestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds, // Game Center does not provide timestamp, using current time
                            Details = null // Game Center does not support details
                        };
                        onSuccess?.Invoke(entry);
                        return;
                    }
                }

                Debug.LogWarning($"[iOS] Player entry not found in leaderboard: {name}");
                onError?.Invoke(new WortalError
                {
                    Code = "NOT_FOUND",
                    Message = "Player entry not found in leaderboard",
                    Context = "GetPlayerEntryAsync"
                });
            });
#else
            Debug.LogWarning($"[iOS] GetPlayerEntryAsync called on non-iOS platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "GetPlayerEntryAsync is only supported on iOS platform",
                Context = "GetPlayerEntryAsync"
            });
#endif
        }

        public void GetEntryCountAsync(string name, int count, Action<LeaderboardEntry[]> onSuccess, Action<WortalError> onError)
        {
            Debug.LogWarning($"[iOS] GetEntryCountAsync is not supported on iOS Game Center");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "GetEntryCountAsync is not supported on iOS Game Center. Use GetLeaderboardAsync instead.",
                Context = "GetEntryCountAsync"
            });
        }

        public void GetConnectedPlayerEntriesAsync(string name, int count, int offset, Action<LeaderboardEntry[]> onSuccess, Action<WortalError> onError)
        {
            Debug.LogWarning($"[iOS] GetConnectedPlayerEntriesAsync is not supported on iOS Game Center");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "GetConnectedPlayerEntriesAsync is not supported on iOS Game Center. Use GetLeaderboardAsync instead.",
                Context = "GetConnectedPlayerEntriesAsync"
            });
        }
    }
}
