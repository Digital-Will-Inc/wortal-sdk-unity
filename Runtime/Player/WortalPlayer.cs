using System;
using System.Collections.Generic;
#if UNITY_WEBGL
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#endif
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
        private static Action _flushDataCallback;
        private static Action<WortalPlayer[]> _getConnectedPlayersCallback;
        private static Action<string, string> _getSignedPlayerInfoCallback;
        private static Action<string> _getASIDCallback;
        private static Action<string, string> _getSignedASIDCallback;
        private static Action<bool> _canSubscribeBotCallback;
        private static Action _subscribeBotCallback;

#region Public API

        /// <summary>
        /// Gets the player's ID from the platform.
        /// </summary>
        /// <returns>The player's ID.</returns>
        public string GetID()
        {
#if UNITY_WEBGL
            return PlayerGetIDJS();
#elif UNITY_EDITOR
            Debug.Log("[Wortal] Mock Player.GetID()");
            return "player1";
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Player.GetID not supported on Android. Returning unknown_player_id.");
            return "unknown_player_id";
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Player.GetID not supported on iOS. Returning unknown_player_id.");
            return "unknown_player_id";
#else
            Debug.LogWarning("[Wortal] Player.GetID not supported on this platform. Returning unknown_player_id.");
            return "unknown_player_id";
#endif
        }

        /// <summary>
        /// Gets the player's name on the platform.
        /// </summary>
        /// <returns>The player's name.</returns>
        public string GetName()
        {
#if UNITY_WEBGL
            return PlayerGetNameJS();
#elif UNITY_EDITOR
            Debug.Log("[Wortal] Mock Player.GetName()");
            return "Player";
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Player.GetName not supported on Android. Returning Unknown Player.");
            return "Unknown Player";
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Player.GetName not supported on iOS. Returning Unknown Player.");
            return "Unknown Player";
#else
            Debug.LogWarning("[Wortal] Player.GetName not supported on this platform. Returning Unknown Player.");
            return "Unknown Player";
#endif
        }

        /// <summary>
        /// Gets the player's photo from the platform.
        /// </summary>
        /// <returns>URL of base64 image for the player's photo.</returns>
        public string GetPhoto()
        {
#if UNITY_WEBGL
            return PlayerGetPhotoJS();
#elif UNITY_EDITOR
            Debug.Log("[Wortal] Mock Player.GetPhoto()");
            return "data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==";
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Player.GetPhoto not supported on Android. Returning null.");
            return null;
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Player.GetPhoto not supported on iOS. Returning null.");
            return null;
#else
            Debug.LogWarning("[Wortal] Player.GetPhoto not supported on this platform. Returning null.");
            return null;
#endif
        }

        /// <summary>
        /// Checks whether this is the first time the player has played this game.
        /// </summary>
        /// <returns>True if it is the first play. Some platforms always return true.</returns>
        public bool IsFirstPlay()
        {
#if UNITY_WEBGL
            return PlayerIsFirstPlayJS();
#elif UNITY_EDITOR
            Debug.Log("[Wortal] Mock Player.IsFirstPlay()");
            return false;
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Player.IsFirstPlay not supported on Android. Returning true.");
            return true;
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Player.IsFirstPlay not supported on iOS. Returning true.");
            return true;
#else
            Debug.LogWarning("[Wortal] Player.IsFirstPlay not supported on this platform. Returning true.");
            return true;
#endif
        }

        /// <summary>
        /// Retrieve data from the designated cloud storage of the current player. Please note that JSON objects stored as
        /// string values would be returned back as JSON objects.
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
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>INVALID_PARAM</li>
        /// <li>NETWORK_FAILURE</li>
        /// <li>CLIENT_UNSUPPORTED_OPERATION</li>
        /// </ul></throws>
        public void GetData(string[] keys, Action<IDictionary<string, object>> callback, Action<WortalError> errorCallback)
        {
            _getDataCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            string keysStr = string.Join("|", keys);
            PlayerGetDataJS(keysStr, PlayerGetDataCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log($"[Wortal] Mock Player.GetData({string.Join(", ", keys)})");
            Dictionary<string, object> data = new();
            foreach (string key in keys)
            {
                if (key == "items")
                {
                    data.Add("items", new Dictionary<string, int> { { "coins", 100 }, { "boosters", 2 } });
                }
                else if (key == "lives")
                {
                    data.Add("lives", 3);
                }
                // Add more mock data as needed for other keys
            }
            _getDataCallback?.Invoke(data); // Directly invoke with dictionary
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Player.GetData({string.Join(", ", keys)}) not supported on Android. Returning empty dictionary.");
            _getDataCallback?.Invoke(new Dictionary<string, object>());
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Player.GetData({string.Join(", ", keys)}) not supported on iOS. Returning empty dictionary.");
            _getDataCallback?.Invoke(new Dictionary<string, object>());
#else
            Debug.LogWarning($"[Wortal] Player.GetData({string.Join(", ", keys)}) not supported on this platform. Returning empty dictionary.");
            _getDataCallback?.Invoke(new Dictionary<string, object>());
#endif
        }

        /// <summary>
        /// Set data to be saved to the designated cloud storage of the current player. The game can store up to 1MB of data
        /// for each unique player.
        /// </summary>
        /// <param name="data">An object containing a set of key-value pairs that should be persisted to cloud storage. The object must
        /// contain only serializable values - any non-serializable values will cause the entire modification to be rejected.</param>
        /// <param name="callback">Void callback that is fired when the JS promise resolves. NOTE: The promise resolving does not
        /// necessarily mean that the input has already been persisted. Rather, it means that the data was valid and has been
        /// scheduled to be saved. It also guarantees that all values that were set are now available in player.getDataAsync.</param>
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
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>INVALID_PARAM</li>
        /// <li>NETWORK_FAILURE</li>
        /// <li>PENDING_REQUEST</li>
        /// <li>CLIENT_UNSUPPORTED_OPERATION</li>
        /// </ul></throws>
        public void SetData(IDictionary<string, object> data, Action callback, Action<WortalError> errorCallback)
        {
            _setDataCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            string dataObj = JsonConvert.SerializeObject(data);
            PlayerSetDataJS(dataObj, PlayerSetDataCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            // Serialize data for logging purposes only if Newtonsoft.Json is available.
            string dataJson = JsonConvert.SerializeObject(data);
            Debug.Log($"[Wortal] Mock Player.SetData({dataJson})");
            _setDataCallback?.Invoke();
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Player.SetData not supported on Android.");
            _setDataCallback?.Invoke();
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Player.SetData not supported on iOS.");
            _setDataCallback?.Invoke();
#else
            Debug.LogWarning($"[Wortal] Player.SetData not supported on this platform.");
            _setDataCallback?.Invoke();
#endif
        }

        /// <summary>
        /// Flushes any unsaved data to the platform's storage. This function is expensive, and should primarily be used for
        /// critical changes where persistence needs to be immediate and known by the game. Non-critical changes should rely on
        /// the platform to persist them in the background.
        /// NOTE: Calls to player.setDataAsync will be rejected while this function's result is pending.
        /// </summary>
        /// <param name="callback">Void callback that is fired when the JS promise resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Player.FlushData(
        ///     () => Debug.Log("Data flushed"),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>INVALID_PARAM</li>
        /// <li>NETWORK_FAILURE</li>
        /// <li>PENDING_REQUEST</li>
        /// <li>CLIENT_UNSUPPORTED_OPERATION</li>
        /// </ul></throws>
        public void FlushData(Action callback, Action<WortalError> errorCallback)
        {
            _flushDataCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            PlayerFlushDataJS(PlayerFlushDataCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log("[Wortal] Mock Player.FlushData()");
            _flushDataCallback?.Invoke();
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Player.FlushData not supported on Android.");
            _flushDataCallback?.Invoke();
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Player.FlushData not supported on iOS.");
            _flushDataCallback?.Invoke();
#else
            Debug.LogWarning("[Wortal] Player.FlushData not supported on this platform.");
            _flushDataCallback?.Invoke();
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
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>NETWORK_FAILURE</li>
        /// <li>CLIENT_UNSUPPORTED_OPERATION</li>
        /// </ul></throws>
        public void GetConnectedPlayers(GetConnectedPlayersPayload payload, Action<WortalPlayer[]> callback, Action<WortalError> errorCallback)
        {
            _getConnectedPlayersCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            string payloadStr = JsonConvert.SerializeObject(payload);
            PlayerGetConnectedPlayersJS(payloadStr, PlayerGetConnectedPlayersCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            string payloadJson = JsonConvert.SerializeObject(payload);
            Debug.Log($"[Wortal] Mock Player.GetConnectedPlayers({payloadJson})");
            var wortalPlayer = new WortalPlayer // Changed from Player to WortalPlayer
            {
                ID = "connectedPlayer1",
                Name = "Connected Player 1",
                Photo = "data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==",
                IsFirstPlay = false,
                // DaysSinceFirstPlay is not a property of WortalPlayer, remove if it was from an old Player struct
            };
            _getConnectedPlayersCallback?.Invoke(new WortalPlayer[] { wortalPlayer });
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Player.GetConnectedPlayers not supported on Android. Returning empty array.");
            _getConnectedPlayersCallback?.Invoke(Array.Empty<WortalPlayer>());
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Player.GetConnectedPlayers not supported on iOS. Returning empty array.");
            _getConnectedPlayersCallback?.Invoke(Array.Empty<WortalPlayer>());
#else
            Debug.LogWarning($"[Wortal] Player.GetConnectedPlayers not supported on this platform. Returning empty array.");
            _getConnectedPlayersCallback?.Invoke(Array.Empty<WortalPlayer>());
#endif
        }

        /// <summary>
        /// Gets a signed player object that includes the player ID and signature for validation. This can be used to
        /// send something to a backend server for validation, such as game or purchase data.
        /// </summary>
        /// <param name="callback">Callback with the player ID and signature. Fired when the JS promise resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <remarks>
        /// Server side validation can be done by following these steps:
        ///
        /// <li>Split the signature into two parts delimited by the `.` character.</li>
        /// <li>Decode the first part with base64url encoding, which should be a hash.</li>
        /// <li>Decode the second part with base64url encoding, which should be a string representation of an JSON object.</li>
        /// <li>Hash the second part string using HMAC SHA-256 and the app secret, check if it is identical to the hash from step 2.</li>
        /// <li>Optionally, developer can also validate the timestamp to see if the request is made recently.</li>
        /// </remarks>
        /// <example><code>
        /// Wortal.Player.GetSignedPlayerInfo(
        ///     (id, signature) => Debug.Log("ID: " + id + "\nSignature: " + signature),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>INVALID_PARAM</li>
        /// <li>NETWORK_FAILURE</li>
        /// <li>CLIENT_UNSUPPORTED_OPERATION</li>
        /// </ul></throws>
        public void GetSignedPlayerInfo(Action<string, string> callback, Action<WortalError> errorCallback)
        {
            _getSignedPlayerInfoCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            PlayerGetSignedPlayerInfoJS(PlayerGetSignedPlayerInfoCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log("[Wortal] Mock Player.GetSignedPlayerInfo()");
            _getSignedPlayerInfoCallback?.Invoke("mockPlayerIdSigned", "mockSignature");
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Player.GetSignedPlayerInfo not supported on Android. Returning nulls.");
            _getSignedPlayerInfoCallback?.Invoke(null, null);
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Player.GetSignedPlayerInfo not supported on iOS. Returning nulls.");
            _getSignedPlayerInfoCallback?.Invoke(null, null);
#else
            Debug.LogWarning("[Wortal] Player.GetSignedPlayerInfo not supported on this platform. Returning nulls.");
            _getSignedPlayerInfoCallback?.Invoke(null, null);
#endif
        }

        /// <summary>
        /// A unique identifier for the player. This is the standard Facebook Application-Scoped ID which is used for all Graph
        /// API calls. If your game shares an AppID with a native game this is the ID you will see in the native game too.
        /// </summary>
        /// <param name="callback">Callback with the ASID. Fired when the JS promise resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Player.GetASID(
        ///     id => Debug.Log("ASID: " + asid),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>RETHROW_FROM_PLATFORM</li>
        /// </ul></throws>
        public void GetASID(Action<string> callback, Action<WortalError> errorCallback)
        {
            _getASIDCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            PlayerGetASIDJS(PlayerGetASIDCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log("[Wortal] Mock Player.GetASID()");
            _getASIDCallback?.Invoke("mockASID");
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Player.GetASID not supported on Android. Returning null.");
            _getASIDCallback?.Invoke(null);
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Player.GetASID not supported on iOS. Returning null.");
            _getASIDCallback?.Invoke(null);
#else
            Debug.LogWarning("[Wortal] Player.GetASID not supported on this platform. Returning null.");
            _getASIDCallback?.Invoke(null);
#endif
        }

        /// <summary>
        /// A unique identifier for the player. This is the standard Facebook Application-Scoped ID which is used for all Graph
        /// API calls. If your game shares an AppID with a native game this is the ID you will see in the native game too.
        /// </summary>
        /// <param name="callback">Callback with the ASID and signature. Fired when the JS promise resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <remarks>
        /// A signature to verify this object indeed comes from Facebook. The string is base64url encoded and signed with an
        /// HMAC version of your App Secret, based on the OAuth 2.0 spec.
        ///
        /// You can validate it with the following 4 steps:
        ///
        /// <li>Split the signature into two parts delimited by the '.' character.</li>
        /// <li>Decode the first part (the encoded signature) with base64url encoding.</li>
        /// <li>Decode the second part (the response payload) with base64url encoding, which should be a string
        /// representation of a JSON object that has the following fields: ** algorithm - always equals to HMAC-SHA256
        /// ** issued_at - a unix timestamp of when this response was issued. ** asid - the app-scoped user id of the player.</li>
        /// <li>Hash the whole response payload string using HMAC SHA-256 and your app secret and confirm that it is equal to the encoded signature.</li>
        /// <li>You may also wish to validate the issued_at timestamp in the response payload to ensure the request was made recently.</li>
        /// <br/>
        /// Signature validation should only happen on your server. Never do it on the client side as it will compromise your app secret key.
        /// </remarks>
        /// <example><code>
        /// Wortal.Player.GetSignedASID(
        ///     (asid, signature) => Debug.Log("ASID: " + asid + "\nSignature: " + signature),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>RETHROW_FROM_PLATFORM</li>
        /// </ul></throws>
        public void GetSignedASID(Action<string, string> callback, Action<WortalError> errorCallback)
        {
            _getSignedASIDCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            PlayerGetSignedASIDJS(PlayerGetSignedASIDCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log("[Wortal] Mock Player.GetSignedASID()"); // Corrected log message
            _getSignedASIDCallback?.Invoke("mockSignedASID", "mockSignatureForASID");
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Player.GetSignedASID not supported on Android. Returning nulls.");
            _getSignedASIDCallback?.Invoke(null, null);
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Player.GetSignedASID not supported on iOS. Returning nulls.");
            _getSignedASIDCallback?.Invoke(null, null);
#else
            Debug.LogWarning("[Wortal] Player.GetSignedASID not supported on this platform. Returning nulls.");
            _getSignedASIDCallback?.Invoke(null, null);
#endif
        }

        /// <summary>
        /// Checks if the current user can subscribe to the game's bot.
        /// </summary>
        /// <param name="callback">Callback with the result of the check. Fired when the JS promise resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Player.CanSubscribeBot(
        ///     result => Debug.Log("Can subscribe bot: " + result),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>RATE_LIMITED</li>
        /// <li>INVALID_OPERATION</li>
        /// <li>CLIENT_UNSUPPORTED_OPERATION</li>
        /// </ul></throws>
        public void CanSubscribeBot(Action<bool> callback, Action<WortalError> errorCallback)
        {
            _canSubscribeBotCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            PlayerCanSubscribeBotJS(PlayerCanSubscribeBotCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log("[Wortal] Mock Player.CanSubscribeBot()");
            _canSubscribeBotCallback?.Invoke(true);
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Player.CanSubscribeBot not supported on Android. Returning false.");
            _canSubscribeBotCallback?.Invoke(false);
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Player.CanSubscribeBot not supported on iOS. Returning false.");
            _canSubscribeBotCallback?.Invoke(false);
#else
            Debug.LogWarning("[Wortal] Player.CanSubscribeBot not supported on this platform. Returning false.");
            _canSubscribeBotCallback?.Invoke(false);
#endif
        }

        /// <summary>
        /// Request that the player subscribe the bot associated to the game. The API will reject if the subscription fails -
        /// else, the player will subscribe the game bot.
        /// </summary>
        /// <param name="callback">Callback fired when the JS promise resolves. Promise is resolved if the player
        /// successfully subscribed to the bot.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Player.SubscribeBot(
        ///     () => Debug.Log("Subscribed to bot!"),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>INVALID_PARAM</li>
        /// <li>PENDING_REQUEST</li>
        /// <li>CLIENT_UNSUPPORTED_OPERATION</li>
        /// </ul></throws>
        public void SubscribeBot(Action callback, Action<WortalError> errorCallback)
        {
            _subscribeBotCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            PlayerSubscribeBotJS(PlayerSubscribeBotCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log("[Wortal] Mock Player.SubscribeBot()");
            _subscribeBotCallback?.Invoke();
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Player.SubscribeBot not supported on Android.");
            _subscribeBotCallback?.Invoke();
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Player.SubscribeBot not supported on iOS.");
            _subscribeBotCallback?.Invoke();
#else
            Debug.LogWarning("[Wortal] Player.SubscribeBot not supported on this platform.");
            _subscribeBotCallback?.Invoke();
#endif
        }

#endregion Public API
#region JSlib Interface

#if UNITY_WEBGL
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
        private static extern void PlayerFlushDataJS(Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void PlayerGetConnectedPlayersJS(string payload, Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void PlayerGetSignedPlayerInfoJS(Action<string, string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void PlayerGetASIDJS(Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void PlayerGetSignedASIDJS(Action<string, string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void PlayerCanSubscribeBotJS(Action<bool> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void PlayerSubscribeBotJS(Action callback, Action<string> errorCallback);
#endif

#if UNITY_WEBGL
        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void PlayerGetDataCallback(string data)
        {
            // Data is Record<string, unknown> in JS.
            IDictionary<string, object> dataObj;

            try
            {
                dataObj = JsonConvert.DeserializeObject<JObject>(data).ToDictionary();
            }
            catch (Exception e)
            {
                WortalError error = new()
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message,
                };

                Wortal.WortalError?.Invoke(error);
                return;
            }

            _getDataCallback?.Invoke(dataObj);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void PlayerSetDataCallback()
        {
            _setDataCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void PlayerFlushDataCallback()
        {
            _flushDataCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void PlayerGetConnectedPlayersCallback(string players)
        {
            WortalPlayer[] playersObj;

            try
            {
                playersObj = JsonConvert.DeserializeObject<WortalPlayer[]>(players);
            }
            catch (Exception e)
            {
                WortalError error = new()
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message,
                };

                Wortal.WortalError?.Invoke(error);
                return;
            }

            _getConnectedPlayersCallback?.Invoke(playersObj);
        }

        [MonoPInvokeCallback(typeof(Action<string, string>))]
        private static void PlayerGetSignedPlayerInfoCallback(string playerId, string signature)
        {
            _getSignedPlayerInfoCallback?.Invoke(playerId, signature);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void PlayerGetASIDCallback(string asid)
        {
            _getASIDCallback?.Invoke(asid);
        }

        [MonoPInvokeCallback(typeof(Action<string, string>))]
        private static void PlayerGetSignedASIDCallback(string asid, string signature)
        {
            _getSignedASIDCallback?.Invoke(asid, signature);
        }

        [MonoPInvokeCallback(typeof(Action<bool>))]
        private static void PlayerCanSubscribeBotCallback(bool result)
        {
            _canSubscribeBotCallback?.Invoke(result);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void PlayerSubscribeBotCallback()
        {
            _subscribeBotCallback?.Invoke();
        }
#endif

#endregion JSlib Interface
    }
}
