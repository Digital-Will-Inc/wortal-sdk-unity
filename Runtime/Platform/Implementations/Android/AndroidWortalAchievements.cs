using System;
using System.Reflection;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class AndroidWortalAchievements : IWortalAchievements
    {
        private static Type _playGamesPlatformType;
        private static object _playGamesInstance;

        static AndroidWortalAchievements()
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

        public void GetAchievements(Action<Achievement[]> onSuccess, Action<WortalError> onError)
        {
            if (!IsSupported)
            {
                Debug.LogWarning("[Android] Google Play Games not available");
                onError?.Invoke(new WortalError
                {
                    Code = "DEPENDENCY_MISSING",
                    Message = "Google Play Games SDK not found",
                    Context = "GetAchievements"
                });
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                Debug.Log("[Android] Loading achievements from Google Play Games...");
                
                var loadAchievementsMethod = _playGamesPlatformType.GetMethod("LoadAchievements", new[] { typeof(Action<>).MakeGenericType(typeof(object[])) });
                
                Action<object[]> achievementsCallback = (gpgAchievements) =>
                {
                    if (gpgAchievements == null)
                    {
                        Debug.LogWarning("[Android] Failed to load achievements from Google Play Games");
                        onError?.Invoke(new WortalError
                        {
                            Code = "LOAD_FAILED",
                            Message = "Failed to load achievements from Google Play Games",
                            Context = "GetAchievements"
                        });
                        return;
                    }

                    Debug.Log($"[Android] Loaded {gpgAchievements.Length} achievements from Google Play Games");

                    var wortalAchievements = new Achievement[gpgAchievements.Length];
                    for (int i = 0; i < gpgAchievements.Length; i++)
                    {
                        var gpgAchievement = gpgAchievements[i];
                        var achievementType = gpgAchievement.GetType();
                        
                        wortalAchievements[i] = new Achievement
                        {
                            Id = (string)achievementType.GetProperty("id")?.GetValue(gpgAchievement),
                            Name = (string)achievementType.GetProperty("name")?.GetValue(gpgAchievement) ?? "",
                            Description = (string)achievementType.GetProperty("description")?.GetValue(gpgAchievement) ?? "",
                            IsUnlocked = (bool)(achievementType.GetProperty("completed")?.GetValue(gpgAchievement) ?? false),
                            Type = AchievementType.SINGLE
                        };
                    }

                    onSuccess?.Invoke(wortalAchievements);
                };

                loadAchievementsMethod?.Invoke(_playGamesInstance, new object[] { achievementsCallback });
            }
            catch (Exception e)
            {
                Debug.LogError($"[Android] Error loading achievements: {e.Message}");
                onError?.Invoke(new WortalError
                {
                    Code = "LOAD_ERROR",
                    Message = $"Error loading achievements: {e.Message}",
                    Context = "GetAchievements"
                });
            }
#else
            Debug.LogWarning("[Android] GetAchievements called on non-Android platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "GetAchievements is only supported on Android platform",
                Context = "GetAchievements"
            });
#endif
        }

        public void UnlockAchievement(string achievementID, Action onSuccess, Action<WortalError> onError)
        {
            if (!IsSupported)
            {
                Debug.LogWarning("[Android] Google Play Games not available");
                onError?.Invoke(new WortalError
                {
                    Code = "DEPENDENCY_MISSING",
                    Message = "Google Play Games SDK not found",
                    Context = "UnlockAchievement"
                });
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                // Get platform-specific achievement ID
                var platformAchievementId = WortalSettings.Instance.GetPlatformAchievementId(achievementID);
                
                Debug.Log($"[Android] Unlocking achievement: {achievementID} -> {platformAchievementId}");

                if (string.IsNullOrEmpty(platformAchievementId))
                {
                    Debug.LogError($"[Android] No Google Play Games ID found for achievement: {achievementID}");
                    onError?.Invoke(new WortalError
                    {
                        Code = "ACHIEVEMENT_NOT_CONFIGURED",
                        Message = $"Achievement '{achievementID}' is not configured for Google Play Games",
                        Context = "UnlockAchievement"
                    });
                    return;
                }

                var reportProgressMethod = _playGamesPlatformType.GetMethod("ReportProgress", new[] { typeof(string), typeof(double), typeof(Action<bool>) });
                
                Action<bool> progressCallback = (success) =>
                {
                    if (success)
                    {
                        Debug.Log($"[Android] Successfully unlocked achievement: {achievementID}");
                        onSuccess?.Invoke();
                    }
                    else
                    {
                        Debug.LogError($"[Android] Failed to unlock achievement: {achievementID}");
                        onError?.Invoke(new WortalError
                        {
                            Code = "UNLOCK_FAILED",
                            Message = $"Failed to unlock achievement {achievementID}",
                            Context = "UnlockAchievement"
                        });
                    }
                };

                reportProgressMethod?.Invoke(_playGamesInstance, new object[] { platformAchievementId, 100.0, progressCallback });
            }
            catch (Exception e)
            {
                Debug.LogError($"[Android] Error unlocking achievement: {e.Message}");
                onError?.Invoke(new WortalError
                {
                    Code = "UNLOCK_ERROR",
                    Message = $"Error unlocking achievement: {e.Message}",
                    Context = "UnlockAchievement"
                });
            }
#else
            Debug.LogWarning("[Android] UnlockAchievement called on non-Android platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "UnlockAchievement is only supported on Android platform",
                Context = "UnlockAchievement"
            });
#endif
        }

        public void ShowAchievements(Action onSuccess, Action<WortalError> onError)
        {
            if (!IsSupported)
            {
                Debug.LogWarning("[Android] Google Play Games not available");
                onError?.Invoke(new WortalError
                {
                    Code = "DEPENDENCY_MISSING",
                    Message = "Google Play Games SDK not found",
                    Context = "ShowAchievements"
                });
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            Debug.Log("[Android] ShowAchievements - Google Play Games doesn't provide direct UI access through Unity");
            // Google Play Games doesn't provide a direct method to show achievements UI through Unity's Social API
            // This would require native Android integration or Google Play Games specific UI calls
            
            Debug.Log("[Android] Achievement UI not directly available through Unity's Social API");
            onSuccess?.Invoke();
#else
            Debug.LogWarning("[Android] ShowAchievements called on non-Android platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "ShowAchievements is only supported on Android platform",
                Context = "ShowAchievements"
            });
#endif
        }
    }
}
