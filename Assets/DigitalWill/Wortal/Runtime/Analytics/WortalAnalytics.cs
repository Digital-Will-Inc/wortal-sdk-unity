using System.Runtime.InteropServices;
using UnityEngine;

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
        /// <throws><ul>
        /// <li>INVALID_PARAM</li>
        /// </ul></throws>
        public void LogLevelStart(string level)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LogLevelStartJS(level);
#else
            Debug.Log($"[Wortal] Mock Analytics.LogLevelStart({level})");
#endif
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
        /// <throws><ul>
        /// <li>INVALID_PARAM</li>
        /// </ul></throws>
        public void LogLevelEnd(string level, string score, bool wasCompleted)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LogLevelEndJS(level, score, wasCompleted);
#else
            Debug.Log($"[Wortal] Mock Analytics.LogLevelEnd({level}, {score}, {wasCompleted})");
#endif
        }

        /// <summary>
        /// Logs the player achieving a new level.
        /// </summary>
        /// <param name="level">Level the player achieved.</param>
        /// <example><code>
        /// Wortal.Analytics.LogLevelUp("Level 7");
        /// </code></example>
        /// <throws><ul>
        /// <li>INVALID_PARAM</li>
        /// </ul></throws>
        public void LogLevelUp(string level)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LogLevelUpJS(level);
#else
            Debug.Log($"[Wortal] Mock Analytics.LogLevelUp({level})");
#endif
        }

        /// <summary>
        /// Logs the player's score.
        /// </summary>
        /// <param name="score">Score the player achieved.</param>
        /// <example><code>
        /// Wortal.Analytics.LogScore("100");
        /// </code></example>
        /// <throws><ul>
        /// <li>INVALID_PARAM</li>
        /// </ul></throws>
        public void LogScore(string score)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LogScoreJS(score);
#else
            Debug.Log($"[Wortal] Mock Analytics.LogScore({score})");
#endif
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
#if UNITY_WEBGL && !UNITY_EDITOR
            LogTutorialStartJS(tutorial);
#else
            Debug.Log($"[Wortal] Mock Analytics.LogTutorialStart({tutorial})");
#endif
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
#if UNITY_WEBGL && !UNITY_EDITOR
            LogTutorialEndJS(tutorial, wasCompleted);
#else
            Debug.Log($"[Wortal] Mock Analytics.LogTutorialEnd({tutorial}, {wasCompleted})");
#endif
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
        /// <throws><ul>
        /// <li>INVALID_PARAM</li>
        /// </ul></throws>
        public void LogGameChoice(string decision, string choice)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LogGameChoiceJS(decision, choice);
#else
            Debug.Log($"[Wortal] Mock Analytics.LogGameChoice({decision}, {choice})");
#endif
        }

        /// <summary>
        /// Logs the player's purchase of an in-app product.
        /// </summary>
        /// <param name="productID">ID of the product the player purchased.</param>
        /// <param name="data">Additional details about the purchase.</param>
        /// <example><code>
        /// Wortal.Analytics.LogPurchase("game.gems.100", "100 gems from shop sale");
        /// </code></example>
        /// <throws><ul>
        /// <li>INVALID_PARAM</li>
        /// </ul></throws>
        public void LogPurchase(string productID, string data = null)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LogPurchaseJS(productID, data);
#else
            Debug.Log($"[Wortal] Mock Analytics.LogPurchase({productID} / {data})");
#endif
        }

        /// <summary>
        /// Logs the player's purchase of an in-app subscription.
        /// </summary>
        /// <param name="productID">ID of the subscription product the player purchased.</param>
        /// <param name="data">Additional details about the purchase.</param>
        /// <example><code>
        /// Wortal.Analytics.LogPurchaseSubscription("game.seasonpass", "Season pass from level up reward UI");
        /// </code></example>
        /// <throws><ul>
        /// <li>INVALID_PARAM</li>
        /// </ul></throws>
        public void LogPurchaseSubscription(string productID, string data = null)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LogPurchaseSubscriptionJS(productID, data);
#else
            Debug.Log($"[Wortal] Mock Analytics.LogPurchaseSubscription({productID} / {data})");
#endif
        }

        /// <summary>
        /// Logs the player's social invite.
        /// </summary>
        /// <param name="placement">Placement of the invite.</param>
        /// <example><code>
        /// Wortal.Analytics.LogSocialInvite("Leaderboard View");
        /// </code></example>
        public void LogSocialInvite(string placement)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LogSocialInviteJS(placement);
#else
            Debug.Log($"[Wortal] Mock Analytics.LogSocialInvite({placement})");
#endif
        }

        /// <summary>
        /// Logs the player's social share.
        /// </summary>
        /// <param name="placement">Placement of the share.</param>
        /// <example><code>
        /// Wortal.Analytics.LogSocialShare("Game Over UI");
        /// </code></example>
        public void LogSocialShare(string placement)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LogSocialShareJS(placement);
#else
            Debug.Log($"[Wortal] Mock Analytics.LogSocialShare({placement})");
#endif
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

        [DllImport("__Internal")]
        private static extern void LogPurchaseJS(string productID, string data);

        [DllImport("__Internal")]
        private static extern void LogPurchaseSubscriptionJS(string productID, string data);

        [DllImport("__Internal")]
        private static extern void LogSocialInviteJS(string placement);

        [DllImport("__Internal")]
        private static extern void LogSocialShareJS(string placement);

#endregion JSlib Interface
    }
}
