using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Context API
    /// </summary>
    public class WortalContext
    {
        private static Action _chooseCallback;
        private static Action<int> _shareCallback;
        private static Action _updateCallback;
        private static Action _createCallback;
        private static Action _switchCallback;

#region Public API
        /// <summary>
        /// Gets the ID of the current context.
        /// </summary>
        /// <returns>String ID of the current context if one exists. Null if the player is playing solo. Empty string if the
        /// game is being played on a platform that does not currently support context.</returns>
        public string GetID()
        {
            return ContextGetIdJS();
        }

        /// <summary>
        /// Opens the platform UI to select friends to invite and play with.
        /// </summary>
        /// <param name="payload">Object defining the options for the context choice.</param>
        /// <param name="callback">Void callback event triggered when the async JS function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Context.ChooseAsync(payload,
        ///     () => Debug.Log("New context: " + Wortal.Context.GetID()),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        public void Choose(ContextPayload payload, Action callback, Action<WortalError> errorCallback)
        {
            _chooseCallback = callback;
            Wortal.WortalError = errorCallback;
            string payloadObj = JsonConvert.SerializeObject(payload);
            ContextChooseJS(payloadObj, ContextChooseCallback, Wortal.WortalErrorCallback);
        }

        /// <summary>
        /// Shares a message to the player's friends. Will trigger a UI for the player to choose which friends to share with.
        /// </summary>
        /// <param name="payload">Object defining the share message.</param>
        /// <param name="callback">Callback event that contains int with number of friends this was shared with. Fired after JS async function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Context.ShareAsync(payload,
        ///     shareResult => Debug.Log("Number of shares: " + shareResult),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        public void Share(ContextPayload payload, Action<int> callback, Action<WortalError> errorCallback)
        {
            _shareCallback = callback;
            Wortal.WortalError = errorCallback;
            string payloadObj = JsonConvert.SerializeObject(payload);
            ContextShareJS(payloadObj, ContextShareCallback, Wortal.WortalErrorCallback);
        }

        /// <summary>
        /// Posts an update to the current context. Will send a message to the chat thread of the current context.
        /// </summary>
        /// <param name="payload">Object defining the update message.</param>
        /// <param name="callback">Void callback event triggered when the async JS function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Context.UpdateAsync(payload,
        ///     () => Debug.Log("Update sent"),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        public void Update(ContextPayload payload, Action callback, Action<WortalError> errorCallback)
        {
            _updateCallback = callback;
            Wortal.WortalError = errorCallback;
            string payloadObj = JsonConvert.SerializeObject(payload);
            ContextUpdateJS(payloadObj, ContextUpdateCallback, Wortal.WortalErrorCallback);
        }

        /// <summary>
        /// Creates a context with the given player ID.
        /// </summary>
        /// <param name="playerId">ID of player to create a context with.</param>
        /// <param name="callback">Void callback event triggered when the async JS function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Context.Create("somePlayerId",
        ///     () => Debug.Log("New context: " + Wortal.Context.GetID(),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        public void Create(string playerId, Action callback, Action<WortalError> errorCallback)
        {
            _createCallback = callback;
            Wortal.WortalError = errorCallback;
            ContextCreateJS(playerId, ContextCreateCallback, Wortal.WortalErrorCallback);
        }

        /// <summary>
        /// Switches the current context to the context with the given ID.
        /// </summary>
        /// <param name="contextId">ID of the context to switch to.</param>
        /// <param name="callback">Void callback event triggered when the async JS function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Context.Switch("someContextId",
        ///     () => Debug.Log("New context: " + Wortal.Context.GetID(),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        public void Switch(string contextId, Action callback, Action<WortalError> errorCallback)
        {
            _switchCallback = callback;
            Wortal.WortalError = errorCallback;
            ContextSwitchJS(contextId, ContextSwitchCallback, Wortal.WortalErrorCallback);
        }
#endregion Public API

#region JSlib Interface
        [DllImport("__Internal")]
        private static extern string ContextGetIdJS();

        [DllImport("__Internal")]
        private static extern void ContextChooseJS(string payload, Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void ContextShareJS(string payload, Action<int> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void ContextUpdateJS(string payload, Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void ContextCreateJS(string playerId, Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void ContextSwitchJS(string contextId, Action callback, Action<string> errorCallback);

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
        /// Available options for the Strategy type in the SDK Core.
        /// </summary>
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
        public static class NotificationsType
        {
            public const string NO_PUSH = "NO_PUSH";
            public const string PUSH = "PUSH";
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
        /// <remarks>Use <see cref="ContextFilter"/> here.</remarks>
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
        /// <remarks>Use <see cref="IntentType"/> here.</remarks>
        [JsonProperty("intent", NullValueHandling = NullValueHandling.Ignore)]
        public string Intent;

        /// <summary>
        /// Optional property to switch share UI mode.
        /// </summary>
        /// <remarks>Use <see cref="UIType"/> here.</remarks>
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
        /// <remarks>Use <see cref="StrategyType"/> here.</remarks>
        [JsonProperty("strategy", NullValueHandling = NullValueHandling.Ignore)]
        public string Strategy;

        /// <summary>
        /// Specifies if the message should trigger push notification.
        /// </summary>
        /// <remarks>Use <see cref="NotificationsType"/> here.</remarks>
        [JsonProperty("notifications", NullValueHandling = NullValueHandling.Ignore)]
        public string Notifications;

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
