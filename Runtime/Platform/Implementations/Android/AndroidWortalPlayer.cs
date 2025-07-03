using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace DigitalWill.WortalSDK
{
    public class AndroidWortalPlayer : IWortalPlayer
    {
        private static Type _playGamesPlatformType;
        private static object _playGamesInstance;

        // Cache for player data (since Google Play Games doesn't provide cloud storage by default)
        private static Dictionary<string, object> _localPlayerData = new Dictionary<string, object>();
        private static Dictionary<string, object> _localPlayerStats = new Dictionary<string, object>();

        static AndroidWortalPlayer()
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

        public string GetID()
        {
            if (!IsSupported)
            {
                Debug.LogWarning("[Android] Google Play Games not available");
                return "android_mock_player_id";
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                var isAuthenticatedMethod = _playGamesPlatformType.GetMethod("IsAuthenticated");
                var isAuthenticated = (bool)isAuthenticatedMethod.Invoke(_playGamesInstance, null);
                
                if (isAuthenticated)
                {
                    var playerId = Social.localUser.id ?? "";
                    Debug.Log($"[Android] Player ID: {playerId}");
                    return playerId;
                }
                else
                {
                    Debug.LogWarning("[Android] Player not authenticated with Google Play Games");
                    return "";
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[Android] Error getting player ID: {e.Message}");
                return "";
            }
#else
            Debug.Log("[Android] GetID called on non-Android platform");
            return "android_mock_player_id";
#endif
        }

        public string GetName()
        {
            if (!IsSupported)
            {
                Debug.LogWarning("[Android] Google Play Games not available");
                return "Android Mock Player";
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                var isAuthenticatedMethod = _playGamesPlatformType.GetMethod("IsAuthenticated");
                var isAuthenticated = (bool)isAuthenticatedMethod.Invoke(_playGamesInstance, null);
                
                if (isAuthenticated)
                {
                    var playerName = Social.localUser.userName ?? "Google Play User";
                    Debug.Log($"[Android] Player Name: {playerName}");
                    return playerName;
                }
                else
                {
                    Debug.LogWarning("[Android] Player not authenticated with Google Play Games");
                    return "Guest";
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[Android] Error getting player name: {e.Message}");
                return "Google Play User";
            }
#else
            Debug.Log("[Android] GetName called on non-Android platform");
            return "Android Mock Player";
#endif
        }

        public string GetPhoto()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            // Google Play Games doesn't provide direct access to player photos through Unity's Social API
            // This would require native Android integration or Google Play Games specific API calls
            Debug.Log("[Android] Google Play Games doesn't provide player photos through Unity Social API");
            return null;
#else
            Debug.Log("[Android] GetPhoto called on non-Android platform");
            return null;
#endif
        }

        public bool IsFirstPlay()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            // Check if this is the first time the player has played
            var playerId = GetID();
            if (string.IsNullOrEmpty(playerId))
                return true;

            var firstPlayKey = $"WortalFirstPlay_{playerId}";
            var hasPlayedBefore = PlayerPrefs.GetInt(firstPlayKey, 0) == 1;
            
            if (!hasPlayedBefore)
            {
                PlayerPrefs.SetInt(firstPlayKey, 1);
                PlayerPrefs.Save();
                Debug.Log("[Android] First play detected");
                return true;
            }
            
            Debug.Log("[Android] Returning player");
            return false;
#else
            Debug.Log("[Android] IsFirstPlay called on non-Android platform");
            return true;
#endif
        }

        public void GetConnectedPlayersAsync(GetConnectedPlayersPayload payload, Action<IWortalPlayer[]> onSuccess, Action<WortalError> onError)
        {
            if (!IsSupported)
            {
                Debug.LogWarning("[Android] Google Play Games not available");
                onError?.Invoke(new WortalError
                {
                    Code = "DEPENDENCY_MISSING",
                    Message = "Google Play Games SDK not found",
                    Context = "GetConnectedPlayersAsync"
                });
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                Debug.Log("[Android] Loading Google Play Games friends...");

                var friendIds = new List<string>();
                foreach (var friend in Social.localUser.friends)
                {
                    if (friend != null && !string.IsNullOrEmpty(friend.id))
                    {
                        friendIds.Add(friend.id);
                    }
                }

                Social.LoadUsers(friendIds.ToArray(), (users) =>
                {
                    if (users == null)
                    {
                        Debug.LogWarning("[Android] Failed to load Google Play Games friends");
                        onError?.Invoke(new WortalError
                        {
                            Code = "LOAD_FRIENDS_FAILED",
                            Message = "Failed to load Google Play Games friends",
                            Context = "GetConnectedPlayersAsync"
                        });
                        return;
                    }

                    Debug.Log($"[Android] Loaded {users.Length} Google Play Games friends");
                    
                    var connectedPlayers = new IWortalPlayer[users.Length];
                    for (int i = 0; i < users.Length; i++)
                    {
                        connectedPlayers[i] = new GooglePlayGamesPlayer(users[i]);
                    }

                    onSuccess?.Invoke(connectedPlayers);
                });
            }
            catch (Exception e)
            {
                Debug.LogError($"[Android] Error loading connected players: {e.Message}");
                onError?.Invoke(new WortalError
                {
                    Code = "LOAD_FRIENDS_ERROR",
                    Message = $"Error loading connected players: {e.Message}",
                    Context = "GetConnectedPlayersAsync"
                });
            }
#else
            Debug.LogWarning("[Android] GetConnectedPlayersAsync called on non-Android platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "GetConnectedPlayersAsync is only supported on Android platform",
                Context = "GetConnectedPlayersAsync"
            });
#endif
        }

        public void GetSignedPlayerInfoAsync(Action<string, string> onSuccess, Action<WortalError> onError)
        {
            if (!IsSupported)
            {
                Debug.LogWarning("[Android] Google Play Games not available");
                onError?.Invoke(new WortalError
                {
                    Code = "DEPENDENCY_MISSING",
                    Message = "Google Play Games SDK not found",
                    Context = "GetSignedPlayerInfoAsync"
                });
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                var isAuthenticatedMethod = _playGamesPlatformType.GetMethod("IsAuthenticated");
                var isAuthenticated = (bool)isAuthenticatedMethod.Invoke(_playGamesInstance, null);
                
                if (!isAuthenticated)
                {
                    onError?.Invoke(new WortalError
                    {
                        Code = "NOT_AUTHENTICATED",
                        Message = "Player not authenticated with Google Play Games",
                        Context = "GetSignedPlayerInfoAsync"
                    });
                    return;
                }

                // Google Play Games provides server auth codes if configured
                // For now, we'll provide the player ID and a basic signature
                var playerId = Social.localUser.id;
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                var signature = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"gpg_signature_{playerId}_{timestamp}"));
                
                Debug.Log($"[Android] Generated signed player info for: {playerId}");
                onSuccess?.Invoke(playerId, signature);
            }
            catch (Exception e)
            {
                Debug.LogError($"[Android] Error getting signed player info: {e.Message}");
                onError?.Invoke(new WortalError
                {
                    Code = "GET_SIGNED_INFO_ERROR",
                    Message = $"Error getting signed player info: {e.Message}",
                    Context = "GetSignedPlayerInfoAsync"
                });
            }
#else
            Debug.LogWarning("[Android] GetSignedPlayerInfoAsync called on non-Android platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "GetSignedPlayerInfoAsync is only supported on Android platform",
                Context = "GetSignedPlayerInfoAsync"
            });
#endif
        }

        public void CanSubscribeBotAsync(Action<bool> onSuccess, Action<WortalError> onError)
        {
            // Bot subscription is not supported on Google Play Games
            Debug.Log("[Android] Bot subscription not supported on Google Play Games");
            onSuccess?.Invoke(false);
        }

        public void SubscribeBotAsync(Action onSuccess, Action<WortalError> onError)
        {
            // Bot subscription is not supported on Google Play Games
            Debug.Log("[Android] Bot subscription not supported on Google Play Games");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_SUPPORTED",
                Message = "Bot subscription is not supported on Google Play Games",
                Context = "SubscribeBotAsync"
            });
        }

        public void GetDataAsync(string[] keys, Action<PlayerData> onSuccess, Action<WortalError> onError)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            // Google Play Games doesn't provide cloud storage by default, so we'll use local storage
            Debug.Log($"[Android] Getting player data for keys: {string.Join(", ", keys)}");
            
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
            Debug.Log($"[Android] Retrieved {dataDict.Count} data entries");
            onSuccess?.Invoke(playerData);
#else
            Debug.LogWarning("[Android] GetDataAsync called on non-Android platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "GetDataAsync is only supported on Android platform",
                Context = "GetDataAsync"
            });
#endif
        }

        public void SetDataAsync(PlayerData data, Action onSuccess, Action<WortalError> onError)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            Debug.Log($"[Android] Setting player data with {data.data?.Count ?? 0} entries");
            
            if (data.data != null)
            {
                foreach (var kvp in data.data)
                {
                    _localPlayerData[kvp.Key] = kvp.Value;
                }
                
                // Save to PlayerPrefs for persistence
                SavePlayerDataToPrefs();
                Debug.Log("[Android] Player data saved successfully");
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
            Debug.LogWarning("[Android] SetDataAsync called on non-Android platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "SetDataAsync is only supported on Android platform",
                Context = "SetDataAsync"
            });
#endif
        }

        public void FlushDataAsync(Action onSuccess, Action<WortalError> onError)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            Debug.Log("[Android] Flushing player data to persistent storage");
            
            try
            {
                SavePlayerDataToPrefs();
                PlayerPrefs.Save();
                Debug.Log("[Android] Player data flushed successfully");
                onSuccess?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError($"[Android] Failed to flush player data: {e.Message}");
                onError?.Invoke(new WortalError
                {
                    Code = "FLUSH_FAILED",
                    Message = $"Failed to flush player data: {e.Message}",
                    Context = "FlushDataAsync"
                });
            }
#else
            Debug.LogWarning("[Android] FlushDataAsync called on non-Android platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "FlushDataAsync is only supported on Android platform",
                Context = "FlushDataAsync"
            });
#endif
        }

        public void GetStatsAsync(string[] keys, Action<PlayerStats> onSuccess, Action<WortalError> onError)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            Debug.Log($"[Android] Getting player stats for keys: {string.Join(", ", keys)}");
            
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
            Debug.Log($"[Android] Retrieved {statsDict.Count} stat entries");
            onSuccess?.Invoke(playerStats);
#else
            Debug.LogWarning("[Android] GetStatsAsync called on non-Android platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "GetStatsAsync is only supported on Android platform",
                Context = "GetStatsAsync"
            });
#endif
        }

        public void SetStatsAsync(PlayerStats stats, Action onSuccess, Action<WortalError> onError)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            Debug.Log($"[Android] Setting player stats with {stats.stats?.Count ?? 0} entries");
            
            if (stats.stats != null)
            {
                foreach (var kvp in stats.stats)
                {
                    _localPlayerStats[kvp.Key] = kvp.Value;
                }
                
                // Save to PlayerPrefs for persistence
                SavePlayerStatsToPrefs();
                Debug.Log("[Android] Player stats saved successfully");
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
            Debug.LogWarning("[Android] SetStatsAsync called on non-Android platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "SetStatsAsync is only supported on Android platform",
                Context = "SetStatsAsync"
            });
#endif
        }

        public void IncrementStatsAsync(PlayerStats increments, Action<PlayerStats> onSuccess, Action<WortalError> onError)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            Debug.Log($"[Android] Incrementing player stats with {increments.stats?.Count ?? 0} entries");
            
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
                            Debug.LogWarning($"[Android] Cannot increment non-numeric stat: {kvp.Key}");
                            resultStats[kvp.Key] = kvp.Value;
                        }
                    }
                }
                
                SavePlayerStatsToPrefs();
                
                var result = new PlayerStats { stats = resultStats };
                Debug.Log("[Android] Player stats incremented successfully");
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
            Debug.LogWarning("[Android] IncrementStatsAsync called on non-Android platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "IncrementStatsAsync is only supported on Android platform",
                Context = "IncrementStatsAsync"
            });
#endif
        }

        public void GetASIDAsync(Action<string> onSuccess, Action<WortalError> onError)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            // ASID (App-Scoped ID) - Google Play Games doesn't provide this directly
            // We'll generate a consistent app-scoped ID based on the player ID
            var playerId = GetID();
            if (string.IsNullOrEmpty(playerId))
            {
                onError?.Invoke(new WortalError
                {
                    Code = "NOT_AUTHENTICATED",
                    Message = "Player not authenticated with Google Play Games",
                    Context = "GetASIDAsync"
                });
                return;
            }
            
            var appId = Application.identifier;
            var asid = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"gpg_asid_{appId}_{playerId}"));
            
            Debug.Log($"[Android] Generated ASID: {asid}");
            onSuccess?.Invoke(asid);
#else
            Debug.LogWarning("[Android] GetASIDAsync called on non-Android platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "GetASIDAsync is only supported on Android platform",
                Context = "GetASIDAsync"
            });
#endif
        }

        public void GetSignedASIDAsync(Action<string, string> onSuccess, Action<WortalError> onError)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            GetASIDAsync((asid) =>
            {
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                var signature = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"gpg_asid_signature_{asid}_{timestamp}"));
                
                Debug.Log($"[Android] Generated signed ASID");
                onSuccess?.Invoke(asid, signature);
            }, onError);
#else
            Debug.LogWarning("[Android] GetSignedASIDAsync called on non-Android platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "GetSignedASIDAsync is only supported on Android platform",
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
    }

    // Helper class for Google Play Games friends
    public class GooglePlayGamesPlayer : IWortalPlayer
    {
        private readonly IUserProfile _user;

        public GooglePlayGamesPlayer(IUserProfile user)
        {
            _user = user;
        }

        public bool IsSupported => true;

        public string GetID() => _user.id;
        public string GetName() => _user.userName ?? "Google Play User";
        public string GetPhoto() => null; // Google Play Games doesn't provide photos through Unity API
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
