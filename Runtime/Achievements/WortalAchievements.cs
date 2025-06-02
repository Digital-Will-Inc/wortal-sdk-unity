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
    /// Achievements API for Wortal.
    /// </summary>
    public class WortalAchievements
    {
        private static Action<Achievement[]> _getAchievementsCallback;
        private static Action<bool> _unlockAchievementCallback;

#region Public API

        /// <summary>
        /// Gets a player's achievements. This method returns all achievements, regardless of whether they are unlocked or not.
        /// </summary>
        /// <param name="callback">Callback with the achievements. Fired after JS async function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Achievements.GetAchievements(achievements =>
        /// {
        ///     foreach (Achievement achievement in achievements)
        ///     {
        ///         Debug.Log(achievement.Name);
        ///     }
        /// },
        /// error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// </ul></throws>
        public void GetAchievements(Action<Achievement[]> callback, Action<WortalError> errorCallback)
        {
            _getAchievementsCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            AchievementsGetAchievementsJS(AchievementsGetAchievementsCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log("[Wortal] Mock Achievements.GetAchievements()");
            callback?.Invoke(new Achievement[]
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
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Achievements.GetAchievements not supported on Android. Returning empty array.");
            callback?.Invoke(Array.Empty<Achievement>());
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Achievements.GetAchievements not supported on iOS. Returning empty array.");
            callback?.Invoke(Array.Empty<Achievement>());
#else
            Debug.LogWarning("[Wortal] Achievements.GetAchievements not supported on this platform. Returning empty array.");
            callback?.Invoke(Array.Empty<Achievement>());
#endif
        }

        /// <summary>
        /// Unlocks an achievement for the player. This method will only unlock the achievement if it has not already been unlocked.
        /// </summary>
        /// <param name="achievementID">The ID of the achievement to unlock.</param>
        /// <param name="callback">Callback with the unlock result. Fired after JS async function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Achievements.UnlockAchievement("achievementID",
        ///     success => Debug.Log("Achievement Unlocked: " + success),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>INVALID_PARAMS</li>
        /// <li>ACHIEVEMENT_NOT_FOUND</li>
        /// </ul></throws>
        public void UnlockAchievement(string achievementID, Action<bool> callback, Action<WortalError> errorCallback)
        {
            _unlockAchievementCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            AchievementsUnlockAchievementJS(achievementID, AchievementsUnlockAchievementCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log($"[Wortal] Mock Achievements.UnlockAchievement({achievementID})");
            callback?.Invoke(true); // Mock success
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Achievements.UnlockAchievement({achievementID}) not supported on Android. Returning false.");
            callback?.Invoke(false);
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Achievements.UnlockAchievement({achievementID}) not supported on iOS. Returning false.");
            callback?.Invoke(false);
#else
            Debug.LogWarning($"[Wortal] Achievements.UnlockAchievement({achievementID}) not supported on this platform. Returning false.");
            callback?.Invoke(false);
#endif
        }

#endregion Public API
#region JSlib Interface

#if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void AchievementsGetAchievementsJS(Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void AchievementsUnlockAchievementJS(string achievementID, Action<bool> callback, Action<string> errorCallback);
#endif

#if UNITY_WEBGL
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

                Wortal.WortalError?.Invoke(error);
                return;
            }

            _getAchievementsCallback?.Invoke(achievements);
        }

        [MonoPInvokeCallback(typeof(Action<bool>))]
        private static void AchievementsUnlockAchievementCallback(bool success)
        {
            _unlockAchievementCallback?.Invoke(success);
        }
#endif

#endregion JSlib Interface
    }
}
