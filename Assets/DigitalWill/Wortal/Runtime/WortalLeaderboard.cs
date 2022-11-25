using System;
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
	/// <summary>
	/// Leaderboard API
	/// </summary>
	public class WortalLeaderboard
    {
        private static Action<Leaderboard> _getLeaderboardCallback;
        private static Action<LeaderboardEntry> _sendEntryCallback;
        private static Action<LeaderboardEntry[]> _getEntriesCallback;
        private static Action<LeaderboardEntry> _getPlayerEntryCallback;
        private static Action<int> _getEntryCountCallback;
        private static Action<LeaderboardEntry[]> _getConnectedPlayersEntriesCallback;

#region Public API
        /// <summary>
        /// Gets the leaderboard with the given name. Access the leaderboard API via the Leaderboard returned here.
        /// </summary>
        /// <param name="name">Name of the leaderboard.</param>
        /// <param name="callback">Callback with the leaderboard. Fired after JS async function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Leaderboard.GetLeaderboard("global",
        ///     board => Debug.Log("Leaderboard: " + board.Name),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        public void GetLeaderboard(string name, Action<Leaderboard> callback, Action<WortalError> errorCallback)
        {
            _getLeaderboardCallback = callback;
            Wortal.WortalError = errorCallback;
            LeaderboardGetJS(name, LeaderboardGetCallback, Wortal.WortalErrorCallback);
        }

        /// <inheritdoc cref="SendEntry(string,int,string,System.Action{DigitalWill.WortalSDK.WortalLeaderboard.LeaderboardEntry},System.Action{DigitalWill.WortalSDK.WortalError})"/>
        public void SendEntry(string name, int score, Action<LeaderboardEntry> callback, Action<WortalError> errorCallback)
        {
            SendEntry(name, score, "", callback, errorCallback);
        }

        /// <summary>
        /// Sends an entry to be added to the leaderboard, or updated if already existing. Will only update if the score
        /// is a higher than the player's previous entry.
        /// </summary>
        /// <param name="name">Name of the leaderboard.</param>
        /// <param name="score">Score for the entry.</param>
        /// <param name="details">Additional details about the entry.</param>
        /// <param name="callback">Callback with new entry if one was created, updated entry if the score is higher, or the old entry if no new
        /// high score was achieved. Fired after JS async function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Leaderboard.SendEntry("global", 100,
        ///     entry => Debug.Log("Score: " + entry.Score),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        public void SendEntry(string name, int score, string details, Action<LeaderboardEntry> callback, Action<WortalError> errorCallback)
        {
            _sendEntryCallback = callback;
            Wortal.WortalError = errorCallback;
            LeaderboardSendEntryJS(name, score, details, LeaderboardSendEntryCallback, Wortal.WortalErrorCallback);
        }

        /// <inheritdoc cref="GetEntries(string,int,int,System.Action{DigitalWill.WortalSDK.WortalLeaderboard.LeaderboardEntry[]},System.Action{DigitalWill.WortalSDK.WortalError})"/>
        public void GetEntries(string name, int count, Action<LeaderboardEntry[]> callback, Action<WortalError> errorCallback)
        {
            GetEntries(name, count, 0, callback, errorCallback);
        }

        /// <summary>
        /// Gets a list of leaderboard entries in the leaderboard.
        /// </summary>
        /// <param name="name">Name of the leaderboard.</param>
        /// <param name="count">Number of entries to get.</param>
        /// <param name="offset">Offset from the first entry (top rank) to start the count from. Default is 0.</param>
        /// <param name="callback">Callback with an array of leaderboard entries. Fired after JS async function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Leaderboard.GetEntries("global", 10,
        ///     entries => Debug.Log("Score: " + entries[0].Score),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        public void GetEntries(string name, int count, int offset, Action<LeaderboardEntry[]> callback, Action<WortalError> errorCallback)
        {
            _getEntriesCallback = callback;
            Wortal.WortalError = errorCallback;
            LeaderboardGetEntriesJS(name, count, offset,
                LeaderboardGetEntriesCallback, Wortal.WortalErrorCallback);
        }

        /// <summary>
        /// Gets the player's entry in the leaderboard.
        /// </summary>
        /// <param name="name">Name of the leaderboard.</param>
        /// <param name="callback">Callback with the leaderboard entry for the player. Fired after JS async function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Leaderboard.GetPlayerEntry("global",
        ///     entry => Debug.Log("Score: " + entry.Score),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        public void GetPlayerEntry(string name, Action<LeaderboardEntry> callback, Action<WortalError> errorCallback)
        {
            _getPlayerEntryCallback = callback;
            Wortal.WortalError = errorCallback;
            LeaderboardGetPlayerEntryJS(name, LeaderboardGetPlayerEntryCallback, Wortal.WortalErrorCallback);
        }

        /// <summary>
        /// Gets the total number of entries in the leaderboard.
        /// </summary>
        /// <param name="name">Name of the leaderboard.</param>
        /// <param name="callback">Callback with the entry count. Fired after JS async function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Leaderboard.GetEntryCount("global",
        ///     count => Debug.Log("Count: " + count),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        public void GetEntryCount(string name, Action<int> callback, Action<WortalError> errorCallback)
        {
            _getEntryCountCallback = callback;
            Wortal.WortalError = errorCallback;
            LeaderboardGetEntryCountJS(name, LeaderboardGetEntryCountCallback, Wortal.WortalErrorCallback);
        }

        /// <inheritdoc cref="GetConnectedPlayersEntries(string,int,int,System.Action{DigitalWill.WortalSDK.WortalLeaderboard.LeaderboardEntry[]},System.Action{DigitalWill.WortalSDK.WortalError})"/>
        public void GetConnectedPlayersEntries(string name, int count, Action<LeaderboardEntry[]> callback, Action<WortalError> errorCallback)
        {
            GetConnectedPlayersEntries(name, count, 0, callback, errorCallback);
        }

        /// <summary>
        /// Gets a list of leaderboard entries of connected players in the leaderboard.
        /// </summary>
        /// <param name="name">Name of the leaderboard.</param>
        /// <param name="count">Number of entries to get.</param>
        /// <param name="offset">Offset from the first entry (top rank) to start the count from. Default is 0.</param>
        /// <param name="callback">Callback with an array of leaderboard entries. Fired after JS async function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Leaderboard.GetConnectedPlayersEntries("global",
        ///     entries => Debug.Log("Entries: " + entries.Length),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        public void GetConnectedPlayersEntries(string name, int count, int offset, Action<LeaderboardEntry[]> callback, Action<WortalError> errorCallback)
        {
            _getConnectedPlayersEntriesCallback = callback;
            Wortal.WortalError = errorCallback;
            LeaderboardGetConnectedPlayersEntriesJS(name, count, offset,
                LeaderboardGetConnectedPlayersEntriesCallback, Wortal.WortalErrorCallback);
        }
#endregion Public API

#region JSlib Interface
        [DllImport("__Internal")]
        private static extern void LeaderboardGetJS(string name, Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void LeaderboardSendEntryJS(string name, int score, string details, Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void LeaderboardGetEntriesJS(string name, int count, int offset, Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void LeaderboardGetPlayerEntryJS(string name, Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void LeaderboardGetEntryCountJS(string name, Action<int> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void LeaderboardGetConnectedPlayersEntriesJS(string name, int count, int offset, Action<string> callback, Action<string> errorCallback);

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void LeaderboardGetCallback(string leaderboard)
        {
            Leaderboard leaderboardObj = JsonConvert.DeserializeObject<Leaderboard>(leaderboard);
            _getLeaderboardCallback?.Invoke(leaderboardObj);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void LeaderboardSendEntryCallback(string entry)
        {
            LeaderboardEntry entryObj = JsonConvert.DeserializeObject<LeaderboardEntry>(entry);
            _sendEntryCallback?.Invoke(entryObj);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void LeaderboardGetEntriesCallback(string entries)
        {
            LeaderboardEntry[] entriesObj = JsonConvert.DeserializeObject<LeaderboardEntry[]>(entries);
            _getEntriesCallback?.Invoke(entriesObj);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void LeaderboardGetPlayerEntryCallback(string entry)
        {
            LeaderboardEntry entryObj = JsonConvert.DeserializeObject<LeaderboardEntry>(entry);
            _getPlayerEntryCallback?.Invoke(entryObj);
        }

        [MonoPInvokeCallback(typeof(Action<int>))]
        private static void LeaderboardGetEntryCountCallback(int count)
        {
            _getEntryCountCallback?.Invoke(count);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void LeaderboardGetConnectedPlayersEntriesCallback(string entries)
        {
            LeaderboardEntry[] entriesObj = JsonConvert.DeserializeObject<LeaderboardEntry[]>(entries);
            _getConnectedPlayersEntriesCallback?.Invoke(entriesObj);
        }
#endregion JSlib Interface

#region Types
        /// <summary>
        /// Represents a leaderboard for the game.
        /// </summary>
        [Serializable]
        public struct Leaderboard
        {
            /// <summary>
            /// ID of the leaderboard.
            /// </summary>
            [JsonProperty("id")]
            public string Id;

            /// <summary>
            /// Name of the leaderboard.
            /// </summary>
            [JsonProperty("name")]
            public string Name;

            /// <summary>
            /// Context ID of the leaderboard, if one exits.
            /// </summary>
            [JsonProperty("contextId", NullValueHandling = NullValueHandling.Ignore)]
            public string ContextId;
        }

        /// <summary>
        /// A single entry on a leaderboard.
        /// </summary>
        [Serializable]
        public struct LeaderboardEntry
        {
            /// <summary>
            /// Player that made this entry.
            /// </summary>
            [JsonProperty("player")]
            public WortalPlayer.Player Player;

            /// <summary>
            /// Rank of this entry in the leaderboard.
            /// </summary>
            [JsonProperty("rank")]
            public int Rank;

            /// <summary>
            /// Score achieved in this entry.
            /// </summary>
            [JsonProperty("score")]
            public int Score;

            /// <summary>
            /// Score of this entry with optional formatting. Ex: '100 points'
            /// </summary>
            [JsonProperty("formattedScore")]
            public string FormattedScore;

            /// <summary>
            /// Timestamp of when this entry was made.
            /// </summary>
            [JsonProperty("timestamp")]
            public int Timestamp;

            /// <summary>
            /// Optional details about this entry.
            /// </summary>
            [JsonProperty("details", NullValueHandling = NullValueHandling.Ignore)]
            public string Details;
        }
#endregion Types
	}
}
