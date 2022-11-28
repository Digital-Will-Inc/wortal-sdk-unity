using System.Runtime.InteropServices;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Analytics API
    /// </summary>
    public class WortalAnalytics
    {
#region Public API
        /// <summary>
        /// Logs the start of a level.
        /// </summary>
        /// <param name="level">Name of the level.</param>
        /// <example><code>
        /// Wortal.Analytics.LogLevelStart("Level 3");
        /// </code></example>
        public void LogLevelStart(string level)
        {
            LogLevelStartJS(level);
        }

        /// <summary>
        /// Logs the end of a level.
        /// To ensure the level timer is recorded the level name must match the name passed into the
        /// previous logLevelStart call. If it does not match then the timer will be logged at 0.
        /// </summary>
        /// <param name="level">Name of the level.</param>
        /// <param name="score">Score the player achieved.</param>
        /// <param name="wasCompleted">Was the level completed or not.</param>
        /// <example><code>
        /// Wortal.Analytics.LogLevelEnd("Level 3", "100", true);
        /// </code></example>
        public void LogLevelEnd(string level, string score, bool wasCompleted)
        {
            LogLevelEndJS(level, score, wasCompleted);
        }

        /// <summary>
        /// Logs the player achieving a new level.
        /// </summary>
        /// <param name="level">Level the player achieved.</param>
        /// <example><code>
        /// Wortal.Analytics.LogLevelUp("Level 7");
        /// </code></example>
        public void LogLevelUp(string level)
        {
            LogLevelUpJS(level);
        }

        /// <summary>
        /// Logs the player's score.
        /// </summary>
        /// <param name="score">Score the player achieved.</param>
        /// <example><code>
        /// Wortal.Analytics.LogScore("100");
        /// </code></example>
        public void LogScore(string score)
        {
            LogScoreJS(score);
        }

        /// <summary>
        /// Logs the start of a tutorial.
        /// </summary>
        /// <param name="tutorial">Name of the tutorial.</param>
        /// <example><code>
        /// Wortal.Analytics.LogTutorialStart("First Play");
        /// </code></example>
        public void LogTutorialStart(string tutorial)
        {
            LogTutorialStartJS(tutorial);
        }

        /// <summary>
        /// Logs the end of a tutorial.
        /// To ensure the level timer is recorded the tutorial name must match the name passed into the
        /// previous logTutorialStart call. If it does not match then the timer will be logged at 0.
        /// </summary>
        /// <param name="tutorial">Name of the tutorial.</param>
        /// <param name="wasCompleted">Was the tutorial completed.</param>
        /// <example><code>
        /// Wortal.Analytics.LogTutorialEnd("First Play", true);
        /// </code></example>
        public void LogTutorialEnd(string tutorial, bool wasCompleted)
        {
            LogTutorialEndJS(tutorial, wasCompleted);
        }

        /// <summary>
        /// Logs a choice the player made in the game. This can be a powerful tool for balancing the game and understanding
        /// what content the players are interacting with the most.
        /// </summary>
        /// <param name="decision">Decision the player was faced with.</param>
        /// <param name="choice">Choice the player made.</param>
        /// <example><code>
        /// Wortal.Analytics.LogGameChoice("Character", "Blue");
        /// </code></example>
        public void LogGameChoice(string decision, string choice)
        {
            LogGameChoiceJS(decision, choice);
        }
#endregion Public API

#region JSlib Interface
        [DllImport("__Internal")]
        private static extern void LogLevelStartJS(string level);

        [DllImport("__Internal")]
        private static extern void LogLevelEndJS(string level, string score, bool wasCompleted);

        [DllImport("__Internal")]
        private static extern void LogLevelUpJS(string level);

        [DllImport("__Internal")]
        private static extern void LogScoreJS(string score);

        [DllImport("__Internal")]
        private static extern void LogTutorialStartJS(string tutorial);

        [DllImport("__Internal")]
        private static extern void LogTutorialEndJS(string tutorial, bool wasCompleted);

        [DllImport("__Internal")]
        private static extern void LogGameChoiceJS(string decision, string choice);
#endregion JSlib Interface
    }
}
