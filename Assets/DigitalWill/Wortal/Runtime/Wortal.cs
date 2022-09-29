using System;
using UnityEngine;

namespace DigitalWill
{
    /// <summary>
    /// Contains utility functions for interfacing with the game portal and player's browser.
    /// </summary>
    public static class Wortal
    {
        private static WortalAds _ads;
        private static WortalAnalytics _analytics;

        /// <summary>
        /// An ad was requested and successfully returned. This is fired before the ad is shown so it can be used
        /// for pausing the game, muting audio, etc.
        /// </summary>
        public static event Action BeforeAd;
        /// <summary>
        /// An ad request has finished. This does not guarantee an ad was shown, only that the request to the provider
        /// has finished and the player can resume the game now.
        /// </summary>
        public static event Action AfterAd;
        /// <summary>
        /// A rewarded ad was successfully viewed and the player should be given a reward.
        /// </summary>
        public static event Action RewardPlayer;
        /// <summary>
        /// A rewarded ad was dismissed and the player should not receive a reward.
        /// </summary>
        public static event Action RewardSkipped;

        /// <summary>
        /// Shows an interstitial ad. Game should be paused on the BeforeAd event and resumed on the AfterAd event.
        /// </summary>
        /// <param name="type">Type of ad placement.</param>
        /// <param name="description">Description of the ad placement. Ex: "NextLevel"</param>
        public static void ShowInterstitialAd(Placement type, string description)
        {
            _ads.ShowInterstitialAd(type, description);
        }

        /// <summary>
        /// Shows a rewarded ad. Game should be paused on the BeforeAd event and resumed on the AfterAd event.
        /// Player should be rewarded on the RewardPlayer event and not on the RewardSkipped event.
        /// </summary>
        /// <param name="description">Description of the ad placement. Ex: "ReviveAndContinue"</param>
        public static void ShowRewardedAd(string description)
        {
            _ads.ShowRewardedAd(description);
        }

        /// <summary>
        /// Logs the start of a level. Also starts ticking the level timer.
        /// </summary>
        /// <param name="level">Level being played.</param>
        public static void LogLevelStart(string level)
        {
            _analytics.LevelStartEvent(level);
        }

        /// <summary>
        /// Logs the end of a level. If the level name is the same as the previous call to LogLevelStart then
        /// this will add the time spent in the level to the analytics data.
        /// </summary>
        /// <param name="level">Level that was played.</param>
        /// <param name="score">Score the player achieved.</param>
        public static void LogLevelEnd(string level, string score)
        {
            _analytics.LevelEndEvent(level, score);
        }

        /// <summary>
        /// Logs the player leveling up.
        /// </summary>
        /// <param name="level">Level the player achieved.</param>
        public static void LogLevelUp(string level)
        {
            _analytics.LevelUpEvent(level);
        }

        /// <summary>
        /// Logs the player's score.
        /// </summary>
        /// <param name="score">Score the player achieved.</param>
        public static void LogScore(string score)
        {
            _analytics.ScoreEvent(score);
        }

        /// <summary>
        /// Logs a game choice the player has made. This can be a powerful tool for understanding how players
        /// are interacting with the game and help with balancing the game.
        /// </summary>
        /// <param name="decision">Decision the player was faced with.</param>
        /// <param name="choice">Choice the player made.</param>
        public static void LogGameChoice(string decision, string choice)
        {
            _analytics.GameChoiceEvent(decision, choice);
        }

        internal static void InvokeBeforeAd() => BeforeAd?.Invoke();
        internal static void InvokeAfterAd() => AfterAd?.Invoke();
        internal static void InvokeRewardSkipped() => RewardSkipped?.Invoke();
        internal static void InvokeRewardPlayer() => RewardPlayer?.Invoke();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            _ads = new WortalAds();
            _analytics = new WortalAnalytics();
            Debug.Log("Wortal SDK for Unity initialized.");
        }
    }
}
