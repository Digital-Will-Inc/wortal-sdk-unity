using System;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class AndroidWortalAchievements : IWortalAchievements
    {
        public bool IsSupported => false;

        public void GetAchievements(Action<Achievement[]> onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[Android Platform] IWortalAchievements.GetAchievements() called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "GetAchievements implementation"
            });
        }

        public void UnlockAchievement(string achievementID, Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[Android Platform] IWortalAchievements.UnlockAchievement({achievementID}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "UnlockAchievement implementation"
            });
        }

        public void ShowAchievements(Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[Android Platform] IWortalAchievements.ShowAchievements() called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "ShowAchievements implementation"
            });
        }
    }
}