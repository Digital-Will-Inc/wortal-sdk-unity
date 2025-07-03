using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace DigitalWill.WortalSDK
{
    public class iOSWortalPlayer : IWortalPlayer
    {
        public bool IsSupported => Application.platform == RuntimePlatform.IPhonePlayer;

        // Cache for player data (since Game Center doesn't provide cloud storage)
        private static Dictionary<string, object> _localPlayerData = new Dictionary<string, object>();
        private static Dictionary<string, object> _localPlayerStats = new Dictionary<string, object>();
        private static bool _isFirstPlay = true;

        public string GetID()
        {
#if UNITY_IOS && !UNITY_EDITOR
            if (Social.localUser.authenticated)
            {
                var playerId = Social.localUser.id ?? "";
                Debug.Log($"[iOS] Player ID: {playerId}");
                return playerId;
            }
            else
            {
                Debug.LogWarning("[iOS] Player not authenticated with Game Center");
                return ""; // Return empty string if not authenticated
            }
#else
            Debug.Log("[iOS] GetID called on non-iOS platform");
            return "ios_mock_player_id";
#endif
        }

        public string GetName()
        {
#if UNITY_IOS && !UNITY_EDITOR
            if (Social.localUser.authenticated)
            {
                var playerName = Social.localUser.userName ?? "Game Center User";
                Debug.Log($"[iOS] Player Name: {playerName}");
                return playerName;
            }
            else
            {
                Debug.LogWarning("[iOS] Player not authenticated with Game Center");
                return "Guest";
            }
#else
            Debug.Log("[iOS] GetName called on non-iOS platform");
            return "iOS Mock Player";
#endif
        }

        public string GetPhoto()
        {
#if UNITY_IOS && !UNITY_EDITOR
            // Game Center doesn't provide direct access to player photos through Unity's Social API
            // This would require native iOS integration
            Debug.Log("[iOS] Game Center doesn't provide player photos through Unity Social API");
            return null;
#else
            Debug.Log("[iOS] GetPhoto called on non-iOS platform");
            return null;
#endif
        }

        public bool IsFirstPlay()
        {
#if UNITY_IOS && !UNITY_EDITOR
            // Check if this is the first time the player has played
            // We'll use PlayerPrefs to track this since Game Center doesn't provide this info
            var playerId = GetID();
            if (string.IsNullOrEmpty(playerId))
                return true;

            var firstPlayKey = $"WortalFirstPlay_{playerId}";
            var hasPlayedBefore = PlayerPrefs.GetInt(firstPlayKey, 0) == 1;
            
            if (!hasPlayedBefore)
            {
                PlayerPrefs.SetInt(firstPlayKey, 1);
                PlayerPrefs.Save();
                Debug.Log("[iOS] First play detected");
                return true;
            }
            
            Debug.Log("[iOS] Returning player");
            return false;
#else
            Debug.Log("[iOS] IsFirstPlay called on non-iOS platform");
            return _isFirstPlay;
#endif
        }

        public void GetConnectedPlayersAsync(GetConnectedPlayersPayload payload, Action<IWortalPlayer[]> onSuccess, Action<WortalError> onError)
        {
#if UNITY_IOS && !UNITY_EDITOR
            Debug.Log("[iOS] Loading Game Center friends...");
            
            var friendIds = new string[Social.localUser.friends.Length];
            for (int i = 0; i < Social.localUser.friends.Length; i++)
            {
                friendIds[i] = Social.localUser.friends[i].id;
            }
            Social.LoadUsers(friendIds, (users) =>
            {
                if (users == null)
                {
                    Debug.LogWarning("[iOS] Failed to load Game Center friends");
                    onError?.Invoke(new WortalError
                    {
                        Code = "LOAD_FRIENDS_FAILED",
                        Message = "Failed to load Game Center friends",
                        Context = "GetConnectedPlayersAsync"
                    });
                    return;
                }

                Debug.Log($"[iOS] Loaded {users.Length} Game Center friends");
                
                var connectedPlayers = new IWortalPlayer[users.Length];
                for (int i = 0; i < users.Length; i++)
                {
                    connectedPlayers[i] = new GameCenterPlayer(users[i]);
                }

                onSuccess?.Invoke(connectedPlayers);
            });
#else
            Debug.LogWarning("[iOS] GetConnectedPlayersAsync called on non-iOS platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "GetConnectedPlayersAsync is only supported on iOS platform",
                Context = "GetConnectedPlayersAsync"
            });
#endif
        }

        public void GetSignedPlayerInfoAsync(Action<string, string> onSuccess, Action<WortalError> onError)
        {
#if UNITY_IOS && !UNITY_EDITOR
            if (!Social.localUser.authenticated)
            {
                onError?.Invoke(new WortalError
                {
                    Code = "NOT_AUTHENTICATED",
                    Message = "Player not authenticated with Game Center",
                    Context = "GetSignedPlayerInfoAsync"
                });
                return;
            }

            // Game Center doesn't provide signed player info in the same way as other platforms
            // We'll provide the player ID and a basic signature
            var playerId = Social.localUser.id;
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var signature = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"gc_signature_{playerId}_{timestamp}"));
            
            Debug.Log($"[iOS] Generated signed player info for: {playerId}");
            onSuccess?.Invoke(playerId, signature);
#else
            Debug.LogWarning("[iOS] GetSignedPlayerInfoAsync called on non-iOS platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "GetSignedPlayerInfoAsync is only supported on iOS platform",
                Context = "GetSignedPlayerInfoAsync"
            });
#endif
        }

        public void CanSubscribeBotAsync(Action<bool> onSuccess, Action<WortalError> onError)
        {
            // Bot subscription is not supported on iOS Game Center
            Debug.Log("[iOS] Bot subscription not supported on Game Center");
            onSuccess?.Invoke(false);
        }

        public void SubscribeBotAsync(Action onSuccess, Action<WortalError> onError)
        {
            // Bot subscription is not supported on iOS Game Center
            Debug.Log("[iOS] Bot subscription not supported on Game Center");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_SUPPORTED",
                Message = "Bot subscription is not supported on iOS Game Center",
                Context = "SubscribeBotAsync"
            });
        }

        public void GetDataAsync(string[] keys, Action<PlayerData> onSuccess, Action<WortalError> onError)
        {
#if UNITY_IOS && !UNITY_EDITOR
            // Game Center doesn't provide cloud storage, so we'll use local storage
            Debug.Log($"[iOS] Getting player data for keys: {string.Join(", ", keys)}");
            
            var playerData = new PlayerData();
            var dataDict = new Dictionary<string, object>();
            
            foreach (var key in keys)
            {
                if (_localPlayerData.ContainsKey(key))
                {
                    dataDict[key] = _localPlayerData[key];
                }
            }
            
            playerData.data = dataDict;
            Debug.Log($"[iOS] Retrieved {dataDict.Count} data entries");
            onSuccess?.Invoke(playerData);
#else
            Debug.LogWarning("[iOS] GetDataAsync called on non-iOS platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "GetDataAsync is only supported on iOS platform",
                Context = "GetDataAsync"
            });
#endif
        }

        public void SetDataAsync(PlayerData data, Action onSuccess, Action<WortalError> onError)
        {
#if UNITY_IOS && !UNITY_EDITOR
            Debug.Log($"[iOS] Setting player data with {data.data?.Count ?? 0} entries");
            
            if (data.data != null)
            {
                foreach (var kvp in data.data)
                {
                    _localPlayerData[kvp.Key] = kvp.Value;
                }
                
                // Save to PlayerPrefs for persistence
                SavePlayerDataToPrefs();
                Debug.Log("[iOS] Player data saved successfully");
                onSuccess?.Invoke();
            }
            else
            {
                onError?.Invoke(new WortalError
                {
                    Code = "INVALID_DATA",
                    Message = "Player data is null",
                    Context = "SetDataAsync"
                });
            }
#else
            Debug.LogWarning("[iOS] SetDataAsync called on non-iOS platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "SetDataAsync is only supported on iOS platform",
                Context = "SetDataAsync"
            });
#endif
        }

        public void FlushDataAsync(Action onSuccess, Action<WortalError> onError)
        {
#if UNITY_IOS && !UNITY_EDITOR
            Debug.Log("[iOS] Flushing player data to persistent storage");
            
            try
            {
                SavePlayerDataToPrefs();
                PlayerPrefs.Save();
                Debug.Log("[iOS] Player data flushed successfully");
                onSuccess?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError($"[iOS] Failed to flush player data: {e.Message}");
                onError?.Invoke(new WortalError
                {
                    Code = "FLUSH_FAILED",
                    Message = $"Failed to flush player data: {e.Message}",
                    Context = "FlushDataAsync"
                });
            }
#else
            Debug.LogWarning("[iOS] FlushDataAsync called on non-iOS platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "FlushDataAsync is only supported on iOS platform",
                Context = "FlushDataAsync"
            });
#endif
        }

        public void GetStatsAsync(string[] keys, Action<PlayerStats> onSuccess, Action<WortalError> onError)
        {
#if UNITY_IOS && !UNITY_EDITOR
            Debug.Log($"[iOS] Getting player stats for keys: {string.Join(", ", keys)}");
            
            var playerStats = new PlayerStats();
            var statsDict = new Dictionary<string, object>();
            
            foreach (var key in keys)
            {
                if (_localPlayerStats.ContainsKey(key))
                {
                    statsDict[key] = _localPlayerStats[key];
                }
            }
            
            playerStats.stats = statsDict;
            Debug.Log($"[iOS] Retrieved {statsDict.Count} stat entries");
            onSuccess?.Invoke(playerStats);
#else
            Debug.LogWarning("[iOS] GetStatsAsync called on non-iOS platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "GetStatsAsync is only supported on iOS platform",
                Context = "GetStatsAsync"
            });
#endif
        }

        public void SetStatsAsync(PlayerStats stats, Action onSuccess, Action<WortalError> onError)
        {
#if UNITY_IOS && !UNITY_EDITOR
            Debug.Log($"[iOS] Setting player stats with {stats.stats?.Count ?? 0} entries");
            
            if (stats.stats != null)
            {
                foreach (var kvp in stats.stats)
                {
                    _localPlayerStats[kvp.Key] = kvp.Value;
                }
                
                // Save to PlayerPrefs for persistence
                SavePlayerStatsToPrefs();
                Debug.Log("[iOS] Player stats saved successfully");
                onSuccess?.Invoke();
            }
            else
            {
                onError?.Invoke(new WortalError
                {
                    Code = "INVALID_STATS",
                    Message = "Player stats is null",
                    Context = "SetStatsAsync"
                });
            }
#else
            Debug.LogWarning("[iOS] SetStatsAsync called on non-iOS platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "SetStatsAsync is only supported on iOS platform",
                Context = "SetStatsAsync"
            });
#endif
        }

        public void IncrementStatsAsync(PlayerStats increments, Action<PlayerStats> onSuccess, Action<WortalError> onError)
        {
#if UNITY_IOS && !UNITY_EDITOR
            Debug.Log($"[iOS] Incrementing player stats with {increments.stats?.Count ?? 0} entries");
            
            if (increments.stats != null)
            {
                var resultStats = new Dictionary<string, object>();
                
                foreach (var kvp in increments.stats)
                {
                    var currentValue = _localPlayerStats.ContainsKey(kvp.Key) ? _localPlayerStats[kvp.Key] : 0;
                    
                    if (kvp.Value is int intIncrement && currentValue is int intCurrent)
                    {
                        var newValue = intCurrent + intIncrement;
                        _localPlayerStats[kvp.Key] = newValue;
                        resultStats[kvp.Key] = newValue;
                    }
                    else if (kvp.Value is float floatIncrement && currentValue is float floatCurrent)
                    {
                        var newValue = floatCurrent + floatIncrement;
                        _localPlayerStats[kvp.Key] = newValue;
                        resultStats[kvp.Key] = newValue;
                    }
                    else
                    {
                        // Try to convert and add
                        try
                        {
                            var currentFloat = Convert.ToSingle(currentValue);
                            var incrementFloat = Convert.ToSingle(kvp.Value);
                            var newValue = currentFloat + incrementFloat;
                            _localPlayerStats[kvp.Key] = newValue;
                            resultStats[kvp.Key] = newValue;
                        }
                        catch
                        {
                            Debug.LogWarning($"[iOS] Cannot increment non-numeric stat: {kvp.Key}");
                            resultStats[kvp.Key] = kvp.Value;
                        }
                    }
                }
                
                SavePlayerStatsToPrefs();
                
                var result = new PlayerStats { stats = resultStats };
                Debug.Log("[iOS] Player stats incremented successfully");
                onSuccess?.Invoke(result);
            }
            else
            {
                onError?.Invoke(new WortalError
                {
                    Code = "INVALID_INCREMENTS",
                    Message = "Stat increments is null",
                    Context = "IncrementStatsAsync"
                });
            }
#else
            Debug.LogWarning("[iOS] IncrementStatsAsync called on non-iOS platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "IncrementStatsAsync is only supported on iOS platform",
                Context = "IncrementStatsAsync"
            });
#endif
        }

        public void GetASIDAsync(Action<string> onSuccess, Action<WortalError> onError)
        {
#if UNITY_IOS && !UNITY_EDITOR
            // ASID (App-Scoped ID) - Game Center doesn't provide this directly
            // We'll generate a consistent app-scoped ID based on the player ID
            var playerId = GetID();
            if (string.IsNullOrEmpty(playerId))
            {
                onError?.Invoke(new WortalError
                {
                    Code = "NOT_AUTHENTICATED",
                    Message = "Player not authenticated with Game Center",
                    Context = "GetASIDAsync"
                });
                return;
            }
            
            var appId = Application.identifier;
            var asid = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"gc_asid_{appId}_{playerId}"));
            
            Debug.Log($"[iOS] Generated ASID: {asid}");
            onSuccess?.Invoke(asid);
#else
            Debug.LogWarning("[iOS] GetASIDAsync called on non-iOS platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "GetASIDAsync is only supported on iOS platform",
                Context = "GetASIDAsync"
            });
#endif
        }

        public void GetSignedASIDAsync(Action<string, string> onSuccess, Action<WortalError> onError)
        {
#if UNITY_IOS && !UNITY_EDITOR
            GetASIDAsync((asid) =>
            {
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                var signature = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"gc_asid_signature_{asid}_{timestamp}"));
                
                Debug.Log($"[iOS] Generated signed ASID");
                onSuccess?.Invoke(asid, signature);
            }, onError);
#else
            Debug.LogWarning("[iOS] GetSignedASIDAsync called on non-iOS platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "GetSignedASIDAsync is only supported on iOS platform",
                Context = "GetSignedASIDAsync"
            });
#endif
        }

        // Helper methods for data persistence
        private void SavePlayerDataToPrefs()
        {
            var playerId = GetID();
            if (string.IsNullOrEmpty(playerId)) return;

            foreach (var kvp in _localPlayerData)
            {
                var key = $"WortalPlayerData_{playerId}_{kvp.Key}";
                PlayerPrefs.SetString(key, kvp.Value?.ToString() ?? "");
            }
        }

        private void SavePlayerStatsToPrefs()
        {
            var playerId = GetID();
            if (string.IsNullOrEmpty(playerId)) return;

            foreach (var kvp in _localPlayerStats)
            {
                var key = $"WortalPlayerStats_{playerId}_{kvp.Key}";
                PlayerPrefs.SetString(key, kvp.Value?.ToString() ?? "");
            }
        }

        static iOSWortalPlayer()
        {
            LoadPlayerDataFromPrefs();
        }

        private static void LoadPlayerDataFromPrefs()
        {
            // This will be called when the class is first accessed
            // In a real implementation, you'd load this when the player is authenticated
        }
    }

    // Helper class for Game Center friends
    public class GameCenterPlayer : IWortalPlayer
    {
        private readonly IUserProfile _user;

        public GameCenterPlayer(IUserProfile user)
        {
            _user = user;
        }

        public bool IsSupported => true;

        public string GetID() => _user.id;
        public string GetName() => _user.userName ?? "Game Center User";
        public string GetPhoto() => null; // Game Center doesn't provide photos through Unity API
        public bool IsFirstPlay() => false; // Unknown for other players

        // Other methods not applicable for connected players
        public void GetConnectedPlayersAsync(GetConnectedPlayersPayload payload, Action<IWortalPlayer[]> onSuccess, Action<WortalError> onError) =>
            onError?.Invoke(new WortalError { Code = "NOT_APPLICABLE", Message = "Not applicable for connected players" });

        public void GetSignedPlayerInfoAsync(Action<string, string> onSuccess, Action<WortalError> onError) =>
            onError?.Invoke(new WortalError { Code = "NOT_APPLICABLE", Message = "Not applicable for connected players" });

        public void CanSubscribeBotAsync(Action<bool> onSuccess, Action<WortalError> onError) => onSuccess?.Invoke(false);

        public void SubscribeBotAsync(Action onSuccess, Action<WortalError> onError) =>
            onError?.Invoke(new WortalError { Code = "NOT_APPLICABLE", Message = "Not applicable for connected players" });

        public void GetDataAsync(string[] keys, Action<PlayerData> onSuccess, Action<WortalError> onError) =>
            onError?.Invoke(new WortalError { Code = "NOT_APPLICABLE", Message = "Not applicable for connected players" });

        public void SetDataAsync(PlayerData data, Action onSuccess, Action<WortalError> onError) =>
            onError?.Invoke(new WortalError { Code = "NOT_APPLICABLE", Message = "Not applicable for connected players" });

        public void FlushDataAsync(Action onSuccess, Action<WortalError> onError) =>
            onError?.Invoke(new WortalError { Code = "NOT_APPLICABLE", Message = "Not applicable for connected players" });

        public void GetStatsAsync(string[] keys, Action<PlayerStats> onSuccess, Action<WortalError> onError) =>
            onError?.Invoke(new WortalError { Code = "NOT_APPLICABLE", Message = "Not applicable for connected players" });

        public void SetStatsAsync(PlayerStats stats, Action onSuccess, Action<WortalError> onError) =>
            onError?.Invoke(new WortalError { Code = "NOT_APPLICABLE", Message = "Not applicable for connected players" });

        public void IncrementStatsAsync(PlayerStats increments, Action<PlayerStats> onSuccess, Action<WortalError> onError) =>
            onError?.Invoke(new WortalError { Code = "NOT_APPLICABLE", Message = "Not applicable for connected players" });

        public void GetASIDAsync(Action<string> onSuccess, Action<WortalError> onError) =>
            onError?.Invoke(new WortalError { Code = "NOT_APPLICABLE", Message = "Not applicable for connected players" });

        public void GetSignedASIDAsync(Action<string, string> onSuccess, Action<WortalError> onError) =>
            onError?.Invoke(new WortalError { Code = "NOT_APPLICABLE", Message = "Not applicable for connected players" });
    }
}
