using System;
using AOT;
using Newtonsoft.Json;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class WebGLWortalAchievements : IWortalAchievements
    {
        private static Action<Achievement[]> _getAchievementsCallback;
        private static Action _unlockAchievementCallback;
        private static Action _showAchievementsCallback;
        private static Action<WortalError> _errorCallback;

        public bool IsSupported => true;

        /// <summary>
        /// Gets a player's achievements. This method returns all achievements, regardless of whether they are unlocked or not.
        /// </summary>
        public void GetAchievements(Action<Achievement[]> onSuccess, Action<WortalError> onError)
        {
            _getAchievementsCallback = onSuccess;
            _errorCallback = onError;

#if UNITY_WEBGL && !UNITY_EDITOR
            // Use PluginManager instead of direct DllImport
            PluginManager.AchievementsGetAchievements(AchievementsGetAchievementsCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[WebGL Platform] Mock Achievements.GetAchievements()");
            onSuccess?.Invoke(new Achievement[]
            {
                new()
                {
                    Name = "Mock Achievement",
                    Description = "Mock Description",
                    Id = "Mock_ID",
                    IsUnlocked = false,
                    Type = AchievementType.SINGLE,
                },
                new()
                {
                    Name = "Mock Achievement 2",
                    Description = "Mock Description 2",
                    Id = "Mock_ID_2",
                    IsUnlocked = true,
                    Type = AchievementType.SINGLE,
                },
            });
#endif
        }

        /// <summary>
        /// Unlocks an achievement for the player. This method will only unlock the achievement if it has not already been unlocked.
        /// </summary>
        public void UnlockAchievement(string achievementID, Action onSuccess, Action<WortalError> onError)
        {
            _unlockAchievementCallback = onSuccess;
            _errorCallback = onError;

#if UNITY_WEBGL && !UNITY_EDITOR
            // Use PluginManager instead of direct DllImport
            PluginManager.AchievementsUnlockAchievement(achievementID, AchievementsUnlockAchievementCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[WebGL Platform] Mock Achievements.UnlockAchievement({achievementID})");
            onSuccess?.Invoke();
#endif
        }

        public void ShowAchievements(Action onSuccess, Action<WortalError> onError)
        {
            _showAchievementsCallback = onSuccess;
            _errorCallback = onError;

#if UNITY_WEBGL && !UNITY_EDITOR
            Debug.Log("[WebGL Platform] [ShowAchievements] Currently not supported in WebGL");
#else
            Debug.Log("[WebGL Platform] Mock Achievements.ShowAchievements()");
            onSuccess?.Invoke();
#endif
        }

        #region Callbacks - NO MORE DllImport HERE

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void AchievementsGetAchievementsCallback(string achievementsJson)
        {
            Achievement[] achievements;

            try
            {
                achievements = JsonConvert.DeserializeObject<Achievement[]>(achievementsJson);
            }
            catch (Exception e)
            {
                WortalError error = new()
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message,
                };

                _errorCallback?.Invoke(error);
                return;
            }

            _getAchievementsCallback?.Invoke(achievements);
        }

        [MonoPInvokeCallback(typeof(Action<bool>))]
        private static void AchievementsUnlockAchievementCallback(bool success)
        {
            if (success)
            {
                _unlockAchievementCallback?.Invoke();
            }
            else
            {
                _errorCallback?.Invoke(new WortalError
                {
                    Code = "UNLOCK_FAILED",
                    Message = "Failed to unlock achievement",
                    Context = "UnlockAchievement operation"
                });
            }
        }

        #endregion
    }
}
