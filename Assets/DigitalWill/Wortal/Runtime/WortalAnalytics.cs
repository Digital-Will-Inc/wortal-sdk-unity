using System.Runtime.InteropServices;

namespace DigitalWill
{
    /// <summary>
    /// Analytics handler for the Wortal.
    /// </summary>
    internal class WortalAnalytics
    {
        public void LevelStartEvent(string level)
        {
            LogLevelStart(level);
        }

        public void LevelEndEvent(string level, string score)
        {
            LogLevelEnd(level, score);
        }

        public void LevelUpEvent(string level)
        {
            LogLevelUp(level);
        }

        public void ScoreEvent(string score)
        {
            LogScore(score);
        }

        public void GameChoiceEvent(string decision, string choice)
        {
            LogGameChoice(decision, choice);
        }

        [DllImport("__Internal")]
        private static extern void LogLevelStart(string level);

        [DllImport("__Internal")]
        private static extern void LogLevelEnd(string level, string score);

        [DllImport("__Internal")]
        private static extern void LogLevelUp(string level);

        [DllImport("__Internal")]
        private static extern void LogScore(string score);

        [DllImport("__Internal")]
        private static extern void LogGameChoice(string decision, string choice);
    }
}
