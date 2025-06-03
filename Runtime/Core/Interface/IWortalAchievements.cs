using System;
using DigitalWill.WortalSDK;

namespace DigitalWill.WortalSDK.Core
{
    /// <summary>
    /// Interface for achievements functionality across platforms
    /// </summary>
    public interface IWortalAchievements
    {
        /// <summary>
        /// Gets all achievements for the game
        /// </summary>
        /// <param name="onSuccess">Callback with achievement data</param>
        /// <param name="onError">Callback for errors</param>
        void GetAchievements(Action<Achievement[]> onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Unlocks an achievement
        /// </summary>
        /// <param name="achievementID">ID of the achievement to unlock</param>
        /// <param name="onSuccess">Callback for successful unlock</param>
        /// <param name="onError">Callback for errors</param>
        void UnlockAchievement(string achievementID, Action onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Shows the achievements UI
        /// </summary>
        /// <param name="onSuccess">Callback for successful UI display</param>
        /// <param name="onError">Callback for errors</param>
        void ShowAchievements(Action onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Checks if achievements are supported on this platform
        /// </summary>
        bool IsSupported { get; }
    }
}
