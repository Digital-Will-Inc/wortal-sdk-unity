using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace DigitalWill.WortalSDK
{
    public class iOSWortalAchievements : IWortalAchievements
    {
        public bool IsSupported => Application.platform == RuntimePlatform.IPhonePlayer;

        public void GetAchievements(Action<Achievement[]> onSuccess, Action<WortalError> onError)
        {
#if UNITY_IOS && !UNITY_EDITOR
            Debug.Log("[iOS] Loading achievements from Game Center...");
            
            Social.LoadAchievements((achievements) =>
            {
                if (achievements == null)
                {
                    Debug.LogWarning("[iOS] Failed to load achievements from Game Center");
                    onError?.Invoke(new WortalError
                    {
                        Code = "LOAD_FAILED",
                        Message = "Failed to load achievements from Game Center",
                        Context = "GetAchievements"
                    });
                    return;
                }

                Debug.Log($"[iOS] Loaded {achievements.Length} achievements from Game Center");
                
                var wortalAchievements = new Achievement[achievements.Length];
                for (int i = 0; i < achievements.Length; i++)
                {
                    var gcAchievement = achievements[i];
                    wortalAchievements[i] = new Achievement
                    {
                        Id = gcAchievement.id,
                        Name = !string.IsNullOrEmpty(gcAchievement.title) ? gcAchievement.title : gcAchievement.id,
                        Description = gcAchievement.description ?? "",
                        IsUnlocked = gcAchievement.completed,
                        Type = AchievementType.SINGLE
                    };
                }

                onSuccess?.Invoke(wortalAchievements);
            });
#else
            Debug.LogWarning("[iOS] GetAchievements called on non-iOS platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "GetAchievements is only supported on iOS platform",
                Context = "GetAchievements"
            });
#endif
        }

        public void UnlockAchievement(string achievementID, Action onSuccess, Action<WortalError> onError)
        {
#if UNITY_IOS && !UNITY_EDITOR
            // Get platform-specific achievement ID
            var platformAchievementId = WortalSettings.Instance.GetPlatformAchievementId(achievementID);
            
            Debug.Log($"[iOS] Unlocking achievement: {achievementID} -> {platformAchievementId}");

            if (string.IsNullOrEmpty(platformAchievementId))
            {
                Debug.LogError($"[iOS] No Game Center ID found for achievement: {achievementID}");
                onError?.Invoke(new WortalError
                {
                    Code = "ACHIEVEMENT_NOT_CONFIGURED",
                    Message = $"Achievement '{achievementID}' is not configured for iOS Game Center",
                    Context = "UnlockAchievement"
                });
                return;
            }

            Social.ReportProgress(platformAchievementId, 100.0, (success) =>
            {
                if (success)
                {
                    Debug.Log($"[iOS] Successfully unlocked achievement: {achievementID}");
                    onSuccess?.Invoke();
                }
                else
                {
                    Debug.LogError($"[iOS] Failed to unlock achievement: {achievementID}");
                    onError?.Invoke(new WortalError
                    {
                        Code = "UNLOCK_FAILED",
                        Message = $"Failed to unlock achievement {achievementID}",
                        Context = "UnlockAchievement"
                    });
                }
            });
#else
            Debug.LogWarning("[iOS] UnlockAchievement called on non-iOS platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "UnlockAchievement is only supported on iOS platform",
                Context = "UnlockAchievement"
            });
#endif
        }

        public void ShowAchievements(Action onSuccess, Action<WortalError> onError)
        {
#if UNITY_IOS && !UNITY_EDITOR
            Debug.Log("[iOS] ShowAchievements - Game Center doesn't provide direct UI access");
            // Game Center doesn't provide a direct method to show achievements UI
            // The achievements UI is typically shown through the Game Center app
            // or through native iOS UI which would require additional native plugins
            
            Debug.Log("[iOS] Achievement UI not directly available through Unity's Social API");
            onSuccess?.Invoke();
#else
            Debug.LogWarning("[iOS] ShowAchievements called on non-iOS platform");
            onError?.Invoke(new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "ShowAchievements is only supported on iOS platform",
                Context = "ShowAchievements"
            });
#endif
        }
    }
}
