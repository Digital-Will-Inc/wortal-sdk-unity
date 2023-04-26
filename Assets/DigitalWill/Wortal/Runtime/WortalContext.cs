using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Context API
    /// </summary>
    public class WortalContext
    {
        private static Action<WortalPlayer[]> _getPlayersCallback;
        private static Action _chooseCallback;
        private static Action<int> _shareCallback;
        private static Action _shareLinkCallback;
        private static Action _updateCallback;
        private static Action _createCallback;
        private static Action _switchCallback;

#region Public API

        /// <summary>
        /// Gets the ID of the current context.
        /// </summary>
        /// <returns>String ID of the current context if one exists. Null if the player is playing solo or
        /// if the game is being played on a platform that does not currently support context.</returns>
        public string GetID()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return ContextGetIdJS();
#else
            Debug.Log("[Wortal] Mock Context.GetID");
            return "mock-id";
#endif
        }

        /// <summary>
        /// Gets the type of the current context.
        /// </summary>
        /// <returns>The <see cref="ContextType"/> of the current context.</returns>
        public string GetType()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return ContextGetTypeJS();
#else
            Debug.Log("[Wortal] Mock Context.GetType");
            return ContextType.SOLO;
#endif
        }

        /// <summary>
        /// Gets an array of WortalPlayer objects containing information about active players in the current context
        /// (people who played the game in the current context in the last 90 days). This may include the current player.
        /// </summary>
        /// <param name="callback">Callback with array of matching players. Fired when the JS promise resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Context.GetPlayers(
        ///     players => Debug.Log(players[0].GetName()),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>NETWORK_FAILURE</li>
        /// <li>CLIENT_UNSUPPORTED_OPERATION</li>
        /// <li>INVALID_OPERATION</li>
        /// </ul></throws>
        public void GetPlayers(Action<WortalPlayer[]> callback, Action<WortalError> errorCallback)
        {
            _getPlayersCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            ContextGetPlayersJS(ContextGetPlayersCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Context.GetPlayers()");
            var player = new WortalPlayer.Player
            {
                ID = "player1",
                Name = "Player",
                Photo = "data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==",
                IsFirstPlay = false,
                DaysSinceFirstPlay = 0,
            };
            WortalPlayer.Player[] players = { player };
            ContextGetPlayersCallback(JsonConvert.SerializeObject(players));
#endif
        }

        /// <summary>
        /// Opens a context selection dialog for the player. If the player selects an available context, the client will attempt
        /// to switch into that context, and resolve if successful. Otherwise, if the player exits the menu or the client fails
        /// to switch into the new context, this function will reject.
        /// </summary>
        /// <param name="payload">Object defining the options for the context choice.</param>
        /// <param name="callback">Void callback event triggered when the async JS function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Context.Choose(payload,
        ///     () => Debug.Log("New context: " + Wortal.Context.GetID()),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>INVALID_PARAM</li>
        /// <li>SAME_CONTEXT</li>
        /// <li>NETWORK_FAILURE</li>
        /// <li>USER_INPUT</li>
        /// <li>PENDING_REQUEST</li>
        /// <li>CLIENT_UNSUPPORTED_OPERATION</li>
        /// </ul></throws>
        public void Choose(ContextPayload payload, Action callback, Action<WortalError> errorCallback)
        {
            _chooseCallback = callback;
            Wortal.WortalError = errorCallback;
            string payloadObj = JsonConvert.SerializeObject(payload);
#if UNITY_WEBGL && !UNITY_EDITOR
            ContextChooseJS(payloadObj, ContextChooseCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Context.Choose({payload})");
            ContextChooseCallback();
#endif
        }

        /// <summary>
        /// Shares a message to the player's friends. Will trigger a UI for the player to choose which friends to share with.
        /// </summary>
        /// <param name="payload">Object defining the share message.</param>
        /// <param name="callback">Callback event that contains int with number of friends this was shared with. Fired after JS async function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Context.Share(payload,
        ///     shareResult => Debug.Log("Number of shares: " + shareResult),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>INVALID_PARAM</li>
        /// <li>NETWORK_FAILURE</li>
        /// <li>PENDING_REQUEST</li>
        /// <li>CLIENT_UNSUPPORTED_OPERATION</li>
        /// <li>INVALID_OPERATION</li>
        /// </ul></throws>
        public void Share(ContextPayload payload, Action<int> callback, Action<WortalError> errorCallback)
        {
            _shareCallback = callback;
            Wortal.WortalError = errorCallback;
            string payloadObj = JsonConvert.SerializeObject(payload);
#if UNITY_WEBGL && !UNITY_EDITOR
            ContextShareJS(payloadObj, ContextShareCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Context.Share({payload})");
            ContextShareCallback(1);
#endif
        }

        /// <summary>
        /// his invokes a dialog that contains a custom game link that users can copy to their clipboard, or share.
        /// A blob of data can be attached to the custom link - game sessions initiated from the link will be able to access the
        /// data through Wortal.session.getEntryPointData(). This data should be less than or equal to 1000 characters when
        /// stringified. The provided text and image will be used to generate the link preview, with the game name as the title
        /// of the preview. The text is recommended to be less than 44 characters. The image is recommended to either be a square
        /// or of the aspect ratio 1.91:1. The returned promise will resolve when the dialog is closed regardless if the user
        /// actually shared the link or not.
        /// </summary>
        /// <param name="payload">Object defining the payload for the custom link.</param>
        /// <param name="callback">Callback that is fired when the dialog is closed.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Context.ShareLink(payload,
        ///     shareResult => Debug.Log("Number of shares: " + shareResult),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>INVALID_PARAM</li>
        /// <li>NETWORK_FAILURE</li>
        /// <li>PENDING_REQUEST</li>
        /// <li>INVALID_OPERATION</li>
        /// </ul></throws>
        public void ShareLink(ContextPayload payload, Action callback, Action<WortalError> errorCallback)
        {
            _shareLinkCallback = callback;
            Wortal.WortalError = errorCallback;
            string payloadObj = JsonConvert.SerializeObject(payload);
#if UNITY_WEBGL && !UNITY_EDITOR
            ContextShareLinkJS(payloadObj, ContextShareLinkCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Context.ShareLinkAsync({payload})");
            ContextShareLinkCallback();
#endif
        }

        /// <summary>
        /// Posts an update to the current context. Will send a message to the chat thread of the current context.
        /// </summary>
        /// <param name="payload">Object defining the update message.</param>
        /// <param name="callback">Void callback event triggered when the async JS function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Context.Update(payload,
        ///     () => Debug.Log("Update sent"),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>INVALID_PARAM</li>
        /// <li>PENDING_REQUEST</li>
        /// <li>INVALID_OPERATION</li>
        /// </ul></throws>
        public void Update(ContextPayload payload, Action callback, Action<WortalError> errorCallback)
        {
            _updateCallback = callback;
            Wortal.WortalError = errorCallback;
            string payloadObj = JsonConvert.SerializeObject(payload);
#if UNITY_WEBGL && !UNITY_EDITOR
            ContextUpdateJS(payloadObj, ContextUpdateCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Context.Update({payload})");
            ContextUpdateCallback();
#endif
        }

        /// <inheritdoc cref="Create(string,System.Action,System.Action{DigitalWill.WortalSDK.WortalError})"/>
        public void Create(string[] playerIds, Action callback, Action<WortalError> errorCallback)
        {
            _createCallback = callback;
            Wortal.WortalError = errorCallback;
            string idsStr = string.Join("|", playerIds);
#if UNITY_WEBGL && !UNITY_EDITOR
            ContextCreateGroupJS(idsStr, ContextCreateCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Context.CreateGroup({playerIds.Length})");
            ContextCreateCallback();
#endif
        }

        /// <summary>
        /// <p>Attempts to create a context between the current player and a specified player or a list of players. This API
        /// supports 3 use cases: 1) When the input is a single playerID, it attempts to create or switch into a context between
        /// a specified player and the current player 2) When the input is a list of connected playerIDs, it attempts to create
        /// a context containing all the players 3) When there's no input, a friend picker will be loaded to ask the player to
        /// create a context with friends to play with.</p>
        ///<p>For each of these cases, the returned promise will reject if any of the players listed are not Connected Players
        /// of the current player, or if the player denies the request to enter the new context. Otherwise, the promise will
        /// resolve when the game has switched into the new context.</p>
        /// </summary>
        /// <param name="playerId">ID of player to create a context with.</param>
        /// <param name="callback">Void callback event triggered when the async JS function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Context.Create("somePlayerId",
        ///     () => Debug.Log("New context: " + Wortal.Context.GetID(),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>INVALID_PARAM</li>
        /// <li>SAME_CONTEXT</li>
        /// <li>NETWORK_FAILURE</li>
        /// <li>USER_INPUT</li>
        /// <li>PENDING_REQUEST</li>
        /// <li>CLIENT_UNSUPPORTED_OPERATION</li>
        /// </ul></throws>
        public void Create(string playerId, Action callback, Action<WortalError> errorCallback)
        {
            _createCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            ContextCreateJS(playerId, ContextCreateCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Context.Create({playerId})");
            ContextCreateCallback();
#endif
        }

        /// <summary>
        /// Request a switch into a specific context. If the player does not have permission to enter that context, or if the
        /// player does not provide permission for the game to enter that context, this will reject. Otherwise, the promise will
        /// resolve when the game has switched into the specified context.
        /// </summary>
        /// <param name="contextId">ID of the context to switch to.</param>
        /// <param name="callback">Void callback event triggered when the async JS function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Context.Switch("someContextId",
        ///     () => Debug.Log("New context: " + Wortal.Context.GetID(),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>INVALID_PARAM</li>
        /// <li>SAME_CONTEXT</li>
        /// <li>NETWORK_FAILURE</li>
        /// <li>USER_INPUT</li>
        /// <li>PENDING_REQUEST</li>
        /// <li>CLIENT_UNSUPPORTED_OPERATION</li>
        /// </ul></throws>
        public void Switch(string contextId, Action callback, Action<WortalError> errorCallback)
        {
            _switchCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            ContextSwitchJS(contextId, ContextSwitchCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Context.Switch({contextId})");
            ContextSwitchCallback();
#endif
        }

        /// <summary>
        /// This function determines whether the number of participants in the current game context is between a given minimum
        /// and maximum, inclusive. If one of the bounds is null only the other bound will be checked against. It will always
        /// return the original result for the first call made in a context in a given game play session. Subsequent calls,
        /// regardless of arguments, will return the answer to the original query until a context change occurs and the query
        /// result is reset.
        /// </summary>
        /// <param name="min">Minimum number of players in context.</param>
        /// <param name="max">Maximum number of players in context.</param>
        /// <example><code>
        /// var result = Wortal.Context.IsSizeBetween(2, 4);
        /// Debug.Log("IsSizeBetween(2, 4): " + result.Answer);
        /// </code></example>
        /// <returns>Object with the result of the check. Null if not supported.</returns>
        public ContextSizeResponse IsSizeBetween(int min = 0, int max = 0)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            string result = ContextIsSizeBetweenJS(min, max);
            return JsonConvert.DeserializeObject<ContextSizeResponse>(result);
#else
            Debug.Log($"[Wortal] Mock Context.IsSizeBetween({min}, {max})");
            return new ContextSizeResponse
            {
                Answer = true,
                MinSize = min,
                MaxSize = max,
            };
#endif
        }

#endregion Public API
#region JSlib Interface

        [DllImport("__Internal")]
        private static extern string ContextGetIdJS();

        [DllImport("__Internal")]
        private static extern string ContextGetTypeJS();

        [DllImport("__Internal")]
        private static extern void ContextGetPlayersJS(Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void ContextChooseJS(string payload, Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void ContextShareJS(string payload, Action<int> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void ContextShareLinkJS(string payload, Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void ContextUpdateJS(string payload, Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void ContextCreateJS(string playerId, Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void ContextCreateGroupJS(string playerIds, Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void ContextSwitchJS(string contextId, Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern string ContextIsSizeBetweenJS(int min, int max);

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void ContextGetPlayersCallback(string players)
        {
            WortalPlayer[] playersObj = JsonConvert.DeserializeObject<WortalPlayer[]>(players);
            _getPlayersCallback?.Invoke(playersObj);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void ContextChooseCallback()
        {
            _chooseCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<int>))]
        private static void ContextShareCallback(int shareResult)
        {
            _shareCallback?.Invoke(shareResult);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void ContextShareLinkCallback()
        {
            _shareLinkCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void ContextUpdateCallback()
        {
            _updateCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void ContextCreateCallback()
        {
            _createCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void ContextSwitchCallback()
        {
            _switchCallback?.Invoke();
        }

#endregion JSlib Interface
#region Type Constants

        /// <summary>
        /// Available options for the ContextFilter type in the SDK Core.
        /// </summary>
        [Serializable]
        public static class ContextFilter
        {
            /// <summary>
            /// Only enlists contexts that the current player is in, but never participated in (e.g. a new context created by a friend).
            /// </summary>
            public const string NEW_CONTEXT_ONLY = "NEW_CONTEXT_ONLY";
            /// <summary>
            /// Enlists contexts that the current player has participated before.
            /// </summary>
            public const string INCLUDE_EXISTING_CHALLENGES = "INCLUDE_EXISTING_CHALLENGES";
            /// <summary>
            /// Only enlists friends who haven't played this game before.
            /// </summary>
            public const string NEW_PLAYERS_ONLY = "NEW_PLAYERS_ONLY";
            /// <summary>
            /// Only enlists friends who haven't been sent an in-game message before. This filter can be fine-tuned with `hoursSinceInvitation` parameter.
            /// </summary>
            public const string NEW_INVITATIONS_ONLY = "NEW_INVITATIONS_ONLY";
        }

        /// <summary>
        /// Available options for the ContextType type in the SDK Core.
        /// </summary>
        [Serializable]
        public static class ContextType
        {
            public const string SOLO = "SOLO";
            public const string THREAD = "THREAD";
            public const string GROUP = "GROUP";
            public const string POST = "POST";
        }

        /// <summary>
        /// Response object for the Context.IsSizeBetween API.
        /// </summary>
        [Serializable]
        public struct ContextSizeResponse
        {
            [JsonProperty("answer")]
            public bool Answer;
            [JsonProperty("maxSize")]
            public int MaxSize;
            [JsonProperty("minSize")]
            public int MinSize;
        }

        /// <summary>
        /// Available options for the Strategy type in the SDK Core.
        /// </summary>
        [Serializable]
        public static class StrategyType
        {
            /// <summary>
            /// Will be sent immediately.
            /// </summary>
            public const string IMMEDIATE = "IMMEDIATE";
            /// <summary>
            /// When the game session ends, the latest payload will be sent.
            /// </summary>
            public const string LAST = "LAST";
            /// <summary>
            /// Will be sent immediately, and also discard any pending `LAST` payloads in the same session.
            /// </summary>
            public const string IMMEDIATE_CLEAR = "IMMEDIATE_CLEAR";
        }

        /// <summary>
        /// Available options for the UI type in the SDK Core.
        /// </summary>
        [Serializable]
        public static class UIType
        {
            /// <summary>
            /// Serial contact card with share and skip button.
            /// </summary>
            public const string DEFAULT = "DEFAULT";
            /// <summary>
            /// Selectable contact list.
            /// </summary>
            public const string MULTIPLE = "MULTIPLE";
        }

        /// <summary>
        /// Available options for the Intent type in the SDK Core.
        /// </summary>
        [Serializable]
        public static class IntentType
        {
            public const string INVITE = "INVITE";
            public const string REQUEST = "REQUEST";
            public const string CHALLENGE = "CHALLENGE";
            public const string SHARE = "SHARE";
        }

        /// <summary>
        /// Available options for the Notifications type in the SDK Core.
        /// </summary>
        [Serializable]
        public static class NotificationsType
        {
            public const string NO_PUSH = "NO_PUSH";
            public const string PUSH = "PUSH";
        }

        /// <summary>
        /// Available options for the ShareDestination type in the SDK Core.
        /// </summary>
        [Serializable]
        public static class ShareDestination
        {
            public const string NEWSFEED = "NEWSFEED";
            public const string GROUP = "GROUP";
            public const string COPY_LINK = "COPY_LINK";
            public const string MESSENGER = "MESSENGER";
        }

#endregion Type Constants
    }

#region Payload Objects

    /// <summary>
    /// Payload used for methods in the Context API.
    /// <example><code>
    /// var payload = new ContextPayload
    /// {
    ///     Image = "dataURLToBase64Image",
    ///     Text = new LocalizableContent
    ///     {
    ///         Default = "Play",
    ///         Localizations = new Dictionary&lt;string, string&gt;
    ///         {
    ///             {"en_US", "Play"},
    ///             {"ja_JP", "プレイ"},
    ///         },
    ///     },
    ///     Data = new Dictionary&lt;string, object&gt;
    ///     {
    ///         {"current_level", 1},
    ///     },
    /// };
    /// </code></example>
    /// </summary>
    [Serializable]
    public struct ContextPayload
    {
        /// <summary>
        /// URL of base64 encoded image to be displayed. This is required for the payload to be sent.
        /// </summary>
        [JsonProperty("image")]
        public string Image;
        /// <summary>
        /// Message body. This is required for the payload to be sent.
        /// </summary>
        [JsonProperty("text")]
        public LocalizableContent Text;
        /// <summary>
        /// Text of the call-to-action button.
        /// </summary>
        [JsonProperty("caption", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public LocalizableContent Caption;
        /// <summary>
        /// Text of the call-to-action button.
        /// </summary>
        [JsonProperty("cta", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public LocalizableContent CTA;
        /// <summary>
        /// Object passed to any session launched from this context message.
        /// Its size must be less than or equal to 1000 chars when stringified.
        /// It can be accessed from <code>Wortal.Context.GetEntryPointData()</code>.
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Data;
        /// <summary>
        /// An array of filters to be applied to the friend list. Only the first filter is currently used.
        /// </summary>
        /// <remarks>Use <see cref="WortalContext.ContextFilter"/> here.</remarks>
        [JsonProperty("filters", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Filters;
        /// <summary>
        /// Context maximum size.
        /// </summary>
        [JsonProperty("maxSize", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int MaxSize;
        /// <summary>
        /// Context minimum size.
        /// </summary>
        [JsonProperty("minSize", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int MinSize;
        /// <summary>
        /// Specify how long a friend should be filtered out after the current player sends them a message.
        /// This parameter only applies when `NEW_INVITATIONS_ONLY` filter is used.
        /// When not specified, it will filter out any friend who has been sent a message.
        /// </summary>
        [JsonProperty("hoursSinceInvitation", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int HoursSinceInvitation;
        /// <summary>
        /// Optional customizable text field in the share UI.
        /// This can be used to describe the incentive a user can get from sharing.
        /// </summary>
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public LocalizableContent Description;
        /// <summary>
        /// Message format to be used. There's no visible difference among the available options.
        /// </summary>
        /// <remarks>Use <see cref="WortalContext.IntentType"/> here.</remarks>
        [JsonProperty("intent", NullValueHandling = NullValueHandling.Ignore)]
        public string Intent;
        /// <summary>
        /// Optional property to switch share UI mode.
        /// </summary>
        /// <remarks>Use <see cref="WortalContext.UIType"/> here.</remarks>
        [JsonProperty("ui", NullValueHandling = NullValueHandling.Ignore)]
        public string UI;
        /// <summary>
        /// Defines the minimum number of players to be selected to start sharing.
        /// </summary>
        [JsonProperty("minShare", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int MinShare;
        /// <summary>
        /// Defines how the update message should be delivered.
        /// </summary>
        /// <remarks>Use <see cref="WortalContext.StrategyType"/> here.</remarks>
        [JsonProperty("strategy", NullValueHandling = NullValueHandling.Ignore)]
        public string Strategy;
        /// <summary>
        /// Specifies if the message should trigger push notification.
        /// </summary>
        /// <remarks>Use <see cref="WortalContext.NotificationsType"/> here.</remarks>
        [JsonProperty("notifications", NullValueHandling = NullValueHandling.Ignore)]
        public string Notifications;
        /// <summary>
        /// Specifies where the share should appear.
        /// </summary>
        /// <remarks>Use <see cref="WortalContext.ShareDestination"/> here.</remarks>
        [JsonProperty("shareDestination", NullValueHandling = NullValueHandling.Ignore)]
        public string ShareDestination;
        /// <summary>
        /// Should the player switch context or not.
        /// </summary>
        [JsonProperty("switchContext", NullValueHandling = NullValueHandling.Ignore)]
        public bool SwitchContext;
        /// <summary>
        /// Not used.
        /// </summary>
        [JsonProperty("action", NullValueHandling = NullValueHandling.Ignore)]
        public string Action;
        /// <summary>
        /// Not used.
        /// </summary>
        [JsonProperty("template", NullValueHandling = NullValueHandling.Ignore)]
        public string Template;
    }

    /// <summary>
    /// Used to pass localized key-value pairs for content being used in the context API.
    /// </summary>
    /// <remarks>If no localizable content is required then pass only the Default string.</remarks>
    /// <example><code>
    /// var content = new LocalizableContent
    /// {
    ///     Default = "Play",
    ///     Localizations = new Dictionary&lt;string, string&gt;
    ///     {
    ///         {"en_US", "Play"},
    ///         {"ja_JP", "プレイ"},
    ///     }
    /// }
    /// </code></example>
    [Serializable]
    public struct LocalizableContent
    {
        /// <summary>
        /// Text that will be used if no matching locale is found.
        /// </summary>
        [JsonProperty("default", NullValueHandling = NullValueHandling.Ignore)]
        public string Default;
        /// <summary>
        /// Key value pairs of localized strings.
        /// </summary>
        [JsonProperty("localizations", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Localizations;
    }

#endregion Payload Objects
}
