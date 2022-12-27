using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
	/// <summary>
	/// Player API
	/// </summary>
	public class WortalPlayer
    {
        private static Action<IDictionary<string, object>> _getDataCallback;
        private static Action _setDataCallback;
        private static Action<WortalPlayer[]> _getConnectedPlayersCallback;
        private static Action<string, string> _getSignedPlayerInfoCallback;

#region Public API
        /// <summary>
        /// Gets the player's ID from the platform.
        /// </summary>
        /// <returns>The player's ID.</returns>
        public string GetID()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return PlayerGetIDJS();
#else
            Debug.Log("[Wortal] Mock Player.GetID()");
            return "player1";
#endif
        }

        /// <summary>
        /// Gets the player's name on the platform.
        /// </summary>
        /// <returns>The player's name.</returns>
        public string GetName()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return PlayerGetNameJS();
#else
            Debug.Log("[Wortal] Mock Player.GetName()");
            return "Player";
#endif
        }

        /// <summary>
        /// Gets the player's photo from the platform.
        /// </summary>
        /// <returns>URL of base64 image for the player's photo.</returns>
        public string GetPhoto()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return PlayerGetPhotoJS();
#else
            Debug.Log("[Wortal] Mock Player.GetPhoto()");
            return "data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==";
#endif
        }

        /// <summary>
        /// Checks whether this is the first time the player has played this game.
        /// </summary>
        /// <returns>True if it is the first play. Some platforms always return true.</returns>
        public bool IsFirstPlay()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return PlayerIsFirstPlayJS();
#else
            Debug.Log("[Wortal] Mock Player.IsFirstPlay()");
            return false;
#endif
        }

        /// <summary>
        /// Gets the game data with the specific keys from the platform's storage.
        /// </summary>
        /// <param name="keys">Array of keys for the data to get.</param>
        /// <param name="callback">Callback with the player's data. Data can be any type and is stored in IDictionary&lt;string, object&gt;</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Player.GetData(new[] { "items", "lives" },
        ///     data =>
        ///     {
        ///         // Check the return values types before casting or operating on them.
        ///         foreach (KeyValuePair&lt;string, object&gt; kvp in data)
        ///         {
        ///             Debug.Log("Key name: " + kvp.Key);
        ///             Debug.Log("Value type: " + kvp.Value.GetType());
        ///         }
        ///         // Nested objects should de-serialize as IDictionary&lt;string, object&gt;
        ///         var items = (Dictionary&lt;string, object&gt;)data["items"];
        ///         Debug.Log("Coins: " + items["coins"]);
        ///     },
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        ///</code></example>
        public void GetData(string[] keys, Action<IDictionary<string, object>> callback, Action<WortalError> errorCallback)
        {
            _getDataCallback = callback;
            Wortal.WortalError = errorCallback;
            string keysStr = string.Join("|", keys);
#if UNITY_WEBGL && !UNITY_EDITOR
            PlayerGetDataJS(keysStr, PlayerGetDataCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Player.GetData({keys})");
            Dictionary<string, object> data = new()
            {
                {
                    "items", new Dictionary<string, int>
                    {
                        { "coins", 100 },
                        { "boosters", 2 },
                    }
                },
                { "lives", 3 },
            };
            PlayerGetDataCallback(JsonConvert.SerializeObject(data));
#endif
        }

        /// <summary>
        /// Uploads game data to the platform's storage. Max size is 1MB.
        /// </summary>
        /// <param name="data">Key-value pairs of the data to upload. Nullable values will remove the data.</param>
        /// <param name="callback">Void callback that's fired when the JS promise resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Dictionary&lt;string, object&gt; data = new()
        /// {
        ///    { "items", new Dictionary&lt;string, int&gt;
        ///        {
        ///            { "coins", 100 },
        ///            { "boosters", 2 },
        ///        }
        ///    },
        ///    { "lives", 3 },
        /// };
        /// Wortal.Player.SetData(data,
        ///     () => Debug.Log("Data set"),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        ///</code></example>
        public void SetData(IDictionary<string, object> data, Action callback, Action<WortalError> errorCallback)
        {
            _setDataCallback = callback;
            Wortal.WortalError = errorCallback;
            string dataObj = JsonConvert.SerializeObject(data);
#if UNITY_WEBGL && !UNITY_EDITOR
            PlayerSetDataJS(dataObj, PlayerSetDataCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Player.SetData({data})");
            PlayerSetDataCallback();
#endif
        }

        /// <summary>
        /// Gets the friends of the player who have also played this game before.
        /// </summary>
        /// <param name="payload">Options for the friends to get.</param>
        /// <param name="callback">Callback with array of matching players. Fired when the JS promise resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// var payload = new WortalPlayer.PlayerPayload
        /// {
        ///    Filter = WortalPlayer.PlayerFilter.ALL,
        /// };
        /// Wortal.Player.GetConnectedPlayers(payload,
        ///     players => Debug.Log(players[0].GetName()),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        ///</code></example>
        public void GetConnectedPlayers(PlayerPayload payload, Action<WortalPlayer[]> callback, Action<WortalError> errorCallback)
        {
            _getConnectedPlayersCallback = callback;
            Wortal.WortalError = errorCallback;
            string payloadStr = JsonConvert.SerializeObject(payload);
#if UNITY_WEBGL && !UNITY_EDITOR
            PlayerGetConnectedPlayersJS(payloadStr, PlayerGetConnectedPlayersCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Player.GetConnectedPlayers({payload})");
            var player = new Player
            {
                ID = "player1",
                Name = "Player",
                Photo = "data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==",
                IsFirstPlay = false,
                DaysSinceFirstPlay = 0,
            };
            Player[] players = { player };
            PlayerGetConnectedPlayersCallback(JsonConvert.SerializeObject(players));
#endif
        }

        /// <summary>
        /// Gets a signed player object that includes the player ID and signature for validation. This can be used to
        /// send something to a backend server for validation, such as game or purchase data.
        /// </summary>
        /// <param name="callback">Callback with the player ID and signature. Fired when the JS promise resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Player.GetSignedPlayerInfo(
        ///     (id, signature) => Debug.Log("ID: " + id + "\nSignature: " + signature),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        ///</code></example>
        public void GetSignedPlayerInfo(Action<string, string> callback, Action<WortalError> errorCallback)
        {
            _getSignedPlayerInfoCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            PlayerGetSignedPlayerInfoJS(PlayerGetSignedPlayerInfoCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock Player.GetSignedPlayerInfo()");
            PlayerGetSignedPlayerInfoCallback("player1", "some-signature");
#endif
        }
#endregion Public API

#region JSlib Interface
        [DllImport("__Internal")]
        private static extern string PlayerGetIDJS();

        [DllImport("__Internal")]
        private static extern string PlayerGetNameJS();

        [DllImport("__Internal")]
        private static extern string PlayerGetPhotoJS();

        [DllImport("__Internal")]
        private static extern bool PlayerIsFirstPlayJS();

        [DllImport("__Internal")]
        private static extern void PlayerGetDataJS(string keys, Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void PlayerSetDataJS(string data, Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void PlayerGetConnectedPlayersJS(string payload, Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void PlayerGetSignedPlayerInfoJS(Action<string, string> callback, Action<string> errorCallback);

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void PlayerGetDataCallback(string data)
        {
            // Data is Record<string, unknown> in JS.
            IDictionary<string, object> dataObj = JsonConvert.DeserializeObject<JObject>(data).ToDictionary();
            _getDataCallback?.Invoke(dataObj);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void PlayerSetDataCallback()
        {
            _setDataCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void PlayerGetConnectedPlayersCallback(string players)
        {
            WortalPlayer[] playersObj = JsonConvert.DeserializeObject<WortalPlayer[]>(players);
            _getConnectedPlayersCallback?.Invoke(playersObj);
        }

        [MonoPInvokeCallback(typeof(Action<string, string>))]
        private static void PlayerGetSignedPlayerInfoCallback(string playerId, string signature)
        {
            _getSignedPlayerInfoCallback?.Invoke(playerId, signature);
        }
#endregion JSlib Interface

#region Types
        /// <summary>
        /// Represents a player in the game. To access info about the current player, use the Wortal.Player API.
        /// This is used to access info about other players such as friends or leaderboard entries.
        /// </summary>
        [Serializable]
        public struct Player
        {
            /// <summary>
            /// ID of the player. This is platform-dependent.
            /// </summary>
            [JsonProperty("id")]
            public string ID;

            /// <summary>
            /// Name of the player.
            /// </summary>
            [JsonProperty("name")]
            public string Name;

            /// <summary>
            /// Data URL for the player's photo.
            /// </summary>
            [JsonProperty("photo")]
            public string Photo;

            /// <summary>
            /// Is this the first time the player has played this game or not.
            /// </summary>
            [JsonProperty("isFirstPlay", NullValueHandling = NullValueHandling.Ignore)]
            public bool IsFirstPlay;

            /// <summary>
            /// Days since the first time the player has played this game.
            /// </summary>
            [JsonProperty("daysSinceFirstPlay", DefaultValueHandling = DefaultValueHandling.Ignore)]
            public int DaysSinceFirstPlay;
        }

        /// <summary>
        ///
        /// </summary>
        [Serializable]
        public struct PlayerPayload
        {
            /// <summary>
            /// Specify where to start fetch the friend list.
            /// This parameter only applies when NEW_INVITATIONS_ONLY filter is used.
            /// When not specified with NEW_INVITATIONS_ONLY filter, default cursor is 0.
            /// </summary>
            [JsonProperty("cursor", DefaultValueHandling = DefaultValueHandling.Ignore)]
            public int Cursor;

            /// <summary>
            /// Filter to be applied to the friend list.
            /// </summary>
            /// <remarks>Use <see cref="WortalPlayer.PlayerFilter"/> here.</remarks>
            [JsonProperty("filter", NullValueHandling = NullValueHandling.Ignore)]
            public string Filter;

            /// <summary>
            /// Specify how long a friend should be filtered out after the current player sends them a message.
            /// This parameter only applies when NEW_INVITATIONS_ONLY filter is used.
            /// When not specified, it will filter out any friend who has been sent a message.
            /// </summary>
            [JsonProperty("hoursSinceInvitation", DefaultValueHandling = DefaultValueHandling.Ignore)]
            public int HoursSinceInvitation;

            /// <summary>
            /// Specify how many friends to be returned in the friend list.
            /// This parameter only applies when NEW_INVITATIONS_ONLY filter is used.
            /// When not specified with NEW_INVITATIONS_ONLY filter, default cursor is 25.
            /// </summary>
            [JsonProperty("size", DefaultValueHandling = DefaultValueHandling.Ignore)]
            public int Size;
        }

        /// <summary>
        /// Filter used when searching for connected players.
        /// </summary>
        [Serializable]
        public static class PlayerFilter
        {
            /// <summary>
            /// All friends.
            /// </summary>
            public const string ALL = "ALL";
            /// <summary>
            /// Only friends who have played this game before.
            /// </summary>
            public const string INCLUDE_PLAYERS = "INCLUDE_PLAYERS";
            /// <summary>
            /// Only friends who haven't played this game before.
            /// </summary>
            public const string INCLUDE_NON_PLAYERS = "INCLUDE_NON_PLAYERS";
            /// <summary>
            /// Only friends who haven't been sent an in-game message before.
            /// </summary>
            public const string NEW_INVITATIONS_ONLY = "NEW_INVITATIONS_ONLY";
        }
#endregion Types
    }
}
