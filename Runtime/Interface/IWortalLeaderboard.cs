using System;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Interface for leaderboard functionality across platforms
    /// </summary>
    public interface IWortalLeaderboard
    {
        /// <summary>
        /// Gets a leaderboard by name
        /// </summary>
        /// <param name="name">Name of the leaderboard</param>
        /// <param name="onSuccess">Callback with leaderboard data</param>
        /// <param name="onError">Callback for leaderboard errors</param>
        void GetLeaderboardAsync(string name, Action<Leaderboard> onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Sends a score to a leaderboard
        /// </summary>
        /// <param name="name">Name of the leaderboard</param>
        /// <param name="score">Score to submit</param>
        /// <param name="onSuccess">Callback with leaderboard entry</param>
        /// <param name="onError">Callback for score submission errors</param>
        void SendScoreAsync(string name, int score, Action<LeaderboardEntry> onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Sends a score to a leaderboard with additional details
        /// </summary>
        /// <param name="name">Name of the leaderboard</param>
        /// <param name="score">Score to submit</param>
        /// <param name="details">Additional score details</param>
        /// <param name="onSuccess">Callback with leaderboard entry</param>
        /// <param name="onError">Callback for score submission errors</param>
        void SendScoreAsync(string name, int score, string details, Action<LeaderboardEntry> onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Gets entries from a leaderboard
        /// </summary>
        /// <param name="name">Name of the leaderboard</param>
        /// <param name="count">Number of entries to retrieve</param>
        /// <param name="offset">Offset for pagination</param>
        /// <param name="onSuccess">Callback with leaderboard entries</param>
        /// <param name="onError">Callback for retrieval errors</param>
        void GetEntriesAsync(string name, int count, int offset, Action<LeaderboardEntry[]> onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Gets the player's entry from a leaderboard
        /// </summary>
        /// <param name="name">Name of the leaderboard</param>
        /// <param name="onSuccess">Callback with player's entry</param>
        /// <param name="onError">Callback for retrieval errors</param>
        void GetPlayerEntryAsync(string name, Action<LeaderboardEntry> onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Gets entries around the player's entry
        /// </summary>
        /// <param name="name">Name of the leaderboard</param>
        /// <param name="count">Number of entries to retrieve</param>
        /// <param name="onSuccess">Callback with entries around player</param>
        /// <param name="onError">Callback for retrieval errors</param>
        void GetEntryCountAsync(string name, int count, Action<LeaderboardEntry[]> onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Gets connected player entries from a leaderboard
        /// </summary>
        /// <param name="name">Name of the leaderboard</param>
        /// <param name="count">Number of entries to retrieve</param>
        /// <param name="offset">Offset for pagination</param>
        /// <param name="onSuccess">Callback with connected player entries</param>
        /// <param name="onError">Callback for retrieval errors</param>
        void GetConnectedPlayerEntriesAsync(string name, int count, int offset, Action<LeaderboardEntry[]> onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Checks if leaderboards are supported on this platform
        /// </summary>
        bool IsSupported { get; }
    }
}
