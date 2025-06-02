using System;
#if UNITY_WEBGL
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;
#endif
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
        private static Action _inviteCallback;
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
#if UNITY_WEBGL
            return ContextGetIdJS();
#elif UNITY_EDITOR
            Debug.Log("[Wortal] Mock Context.GetID");
            return "mock-id";
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Context.GetID not supported on Android. Returning null.");
            return null;
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Context.GetID not supported on iOS. Returning null.");
            return null;
#else
            Debug.LogWarning("[Wortal] Context.GetID not supported on this platform. Returning null.");
            return null;
#endif
        }

        /// <summary>
        /// Gets the type of the current context.
        /// </summary>
        /// <returns>The <see cref="ContextType"/> of the current context.</returns>
        public new string GetType()
        {
#if UNITY_WEBGL
            return ContextGetTypeJS();
#elif UNITY_EDITOR
            Debug.Log("[Wortal] Mock Context.GetType");
            return ContextType.SOLO.ToString();
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Context.GetType not supported on Android. Returning SOLO.");
            return ContextType.SOLO.ToString();
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Context.GetType not supported on iOS. Returning SOLO.");
            return ContextType.SOLO.ToString();
#else
            Debug.LogWarning("[Wortal] Context.GetType not supported on this platform. Returning SOLO.");
            return ContextType.SOLO.ToString();
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
#if UNITY_WEBGL
            ContextGetPlayersJS(ContextGetPlayersCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log("[Wortal] Mock Context.GetPlayers()");
            var player = new Player
            {
                ID = "player1",
                Name = "Player",
                Photo = "data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==",
                IsFirstPlay = false,
                DaysSinceFirstPlay = 0,
            };
            // Player array needs to be WortalPlayer for the callback.
            // The mock was creating Player[] and then serializing/deserializing.
            // For simplicity, let's create WortalPlayer[] directly for the mock.
            WortalPlayer wortalPlayer = new WortalPlayer
            {
                ID = player.ID,
                Name = player.Name,
                Photo = player.Photo,
                IsFirstPlay = player.IsFirstPlay,
            };
            _getPlayersCallback?.Invoke(new WortalPlayer[] { wortalPlayer });
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Context.GetPlayers not supported on Android. Returning empty array.");
            _getPlayersCallback?.Invoke(Array.Empty<WortalPlayer>());
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Context.GetPlayers not supported on iOS. Returning empty array.");
            _getPlayersCallback?.Invoke(Array.Empty<WortalPlayer>());
#else
            Debug.LogWarning("[Wortal] Context.GetPlayers not supported on this platform. Returning empty array.");
            _getPlayersCallback?.Invoke(Array.Empty<WortalPlayer>());
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
        public void Choose(ChoosePayload payload, Action callback, Action<WortalError> errorCallback)
        {
            _chooseCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            string payloadObj = JsonConvert.SerializeObject(payload);
            ContextChooseJS(payloadObj, ContextChooseCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log($"[Wortal] Mock Context.Choose({JsonConvert.SerializeObject(payload)})");
            _chooseCallback?.Invoke();
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Context.Choose not supported on Android.");
            _chooseCallback?.Invoke();
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Context.Choose not supported on iOS.");
            _chooseCallback?.Invoke();
#else
            Debug.LogWarning($"[Wortal] Context.Choose not supported on this platform.");
            _chooseCallback?.Invoke();
#endif
        }

        /// <summary>
        /// This invokes a dialog to let the user invite one or more people to the game. A blob of data can be attached to the
        /// invite which every game session launched from the invite will be able to access from Wortal.session.getEntryPointData().
        /// This data must be less than or equal to 1000 characters when stringified. The user may choose to cancel the action
        /// and close the dialog, and the returned promise will resolve when the dialog is closed regardless of whether the user
        /// actually invited people or not. The sections included in the dialog can be customized by using the sections parameter.
        /// This can specify which sections to include, how many results to include in each section, and what order the sections
        /// should appear in. The last section will include as many results as possible. If no sections are specified, the
        /// default section settings will be applied. The filters parameter allows for filtering the results. If no results are
        /// returned when the filters are applied, the results will be generated without the filters.
        /// </summary>
        /// <param name="payload">Object defining the invite message.</param>
        /// <param name="callback">Void callback event triggered when the async JS function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Context.Invite(payload,
        ///     () => Debug.Log("Invite sent!"),
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
        public void Invite(InvitePayload payload, Action callback, Action<WortalError> errorCallback)
        {
            _inviteCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            string payloadObj = JsonConvert.SerializeObject(payload);
            ContextInviteJS(payloadObj, ContextInviteCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log($"[Wortal] Mock Context.Invite({JsonConvert.SerializeObject(payload)})");
            _inviteCallback?.Invoke();
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Context.Invite not supported on Android.");
            _inviteCallback?.Invoke();
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Context.Invite not supported on iOS.");
            _inviteCallback?.Invoke();
#else
            Debug.LogWarning($"[Wortal] Context.Invite not supported on this platform.");
            _inviteCallback?.Invoke();
#endif
        }

        /// <summary>
        /// This invokes a dialog to let the user share specified content, as a post on the user's timeline, for example.
        /// A blob of data can be attached to the share which every game session launched from the share will be able to access
        /// from Wortal.session.getEntryPointData(). This data must be less than or equal to 1000 characters when stringified.
        /// The user may choose to cancel the share action and close the dialog, and the returned promise will resolve when the
        /// dialog is closed regardless if the user actually shared the content or not.
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
        public void Share(SharePayload payload, Action<int> callback, Action<WortalError> errorCallback)
        {
            _shareCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            string payloadObj = JsonConvert.SerializeObject(payload);
            ContextShareJS(payloadObj, ContextShareCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log($"[Wortal] Mock Context.Share({JsonConvert.SerializeObject(payload)})");
            _shareCallback?.Invoke(1);
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Context.Share not supported on Android. Returning 0 shares.");
            _shareCallback?.Invoke(0);
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Context.Share not supported on iOS. Returning 0 shares.");
            _shareCallback?.Invoke(0);
#else
            Debug.LogWarning($"[Wortal] Context.Share not supported on this platform. Returning 0 shares.");
            _shareCallback?.Invoke(0);
#endif
        }

        /// <summary>
        /// This invokes a dialog that contains a custom game link that users can copy to their clipboard, or share.
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
        public void ShareLink(LinkSharePayload payload, Action callback, Action<WortalError> errorCallback)
        {
            _shareLinkCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            string payloadObj = JsonConvert.SerializeObject(payload);
            ContextShareLinkJS(payloadObj, ContextShareLinkCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log($"[Wortal] Mock Context.ShareLinkAsync({JsonConvert.SerializeObject(payload)})");
            _shareLinkCallback?.Invoke();
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Context.ShareLink not supported on Android.");
            _shareLinkCallback?.Invoke();
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Context.ShareLink not supported on iOS.");
            _shareLinkCallback?.Invoke();
#else
            Debug.LogWarning($"[Wortal] Context.ShareLink not supported on this platform.");
            _shareLinkCallback?.Invoke();
#endif
        }

        /// <summary>
        /// Posts an update to the current context. Will send a message to the chat thread of the current context.
        /// When players launch the game from this message, those game sessions will be able to access the specified blob
        /// of data through Wortal.session.getEntryPointData().
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
        public void Update(UpdatePayload payload, Action callback, Action<WortalError> errorCallback)
        {
            _updateCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            string payloadObj = JsonConvert.SerializeObject(payload);
            ContextUpdateJS(payloadObj, ContextUpdateCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log($"[Wortal] Mock Context.Update({JsonConvert.SerializeObject(payload)})");
            _updateCallback?.Invoke();
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Context.Update not supported on Android.");
            _updateCallback?.Invoke();
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Context.Update not supported on iOS.");
            _updateCallback?.Invoke();
#else
            Debug.LogWarning($"[Wortal] Context.Update not supported on this platform.");
            _updateCallback?.Invoke();
#endif
        }

        /// <inheritdoc cref="Create(string,System.Action,System.Action{DigitalWill.WortalSDK.WortalError})"/>
        public void Create(string[] playerIds, Action callback, Action<WortalError> errorCallback)
        {
            _createCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            string idsStr = string.Join("|", playerIds);
            ContextCreateGroupJS(idsStr, ContextCreateCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log($"[Wortal] Mock Context.CreateGroup({playerIds.Length})");
            _createCallback?.Invoke();
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Context.Create (group) not supported on Android.");
            _createCallback?.Invoke();
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Context.Create (group) not supported on iOS.");
            _createCallback?.Invoke();
#else
            Debug.LogWarning($"[Wortal] Context.Create (group) not supported on this platform.");
            _createCallback?.Invoke();
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
#if UNITY_WEBGL
            ContextCreateJS(playerId, ContextCreateCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log($"[Wortal] Mock Context.Create({playerId})");
            _createCallback?.Invoke();
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Context.Create (single) not supported on Android.");
            _createCallback?.Invoke();
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Context.Create (single) not supported on iOS.");
            _createCallback?.Invoke();
#else
            Debug.LogWarning($"[Wortal] Context.Create (single) not supported on this platform.");
            _createCallback?.Invoke();
#endif
        }

        /// <inheritdoc cref="Switch(string,DigitalWill.WortalSDK.SwitchPayload,System.Action,System.Action{DigitalWill.WortalSDK.WortalError})"/>
        public void Switch(string contextId, SwitchPayload payload, Action callback, Action<WortalError> errorCallback)
        {
            _switchCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL
            string payloadObj = JsonConvert.SerializeObject(payload);
            ContextSwitchWithPayloadJS(contextId, payloadObj, ContextSwitchCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log($"[Wortal] Mock Context.Switch({contextId}, {JsonConvert.SerializeObject(payload)})");
            _switchCallback?.Invoke();
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Context.Switch (with payload) not supported on Android.");
            _switchCallback?.Invoke();
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Context.Switch (with payload) not supported on iOS.");
            _switchCallback?.Invoke();
#else
            Debug.LogWarning($"[Wortal] Context.Switch (with payload) not supported on this platform.");
            _switchCallback?.Invoke();
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
#if UNITY_WEBGL
            ContextSwitchJS(contextId, ContextSwitchCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
            Debug.Log($"[Wortal] Mock Context.Switch({contextId})");
            _switchCallback?.Invoke();
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Context.Switch not supported on Android.");
            _switchCallback?.Invoke();
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Context.Switch not supported on iOS.");
            _switchCallback?.Invoke();
#else
            Debug.LogWarning($"[Wortal] Context.Switch not supported on this platform.");
            _switchCallback?.Invoke();
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
#if UNITY_WEBGL
            string result = ContextIsSizeBetweenJS(min, max);
            return JsonConvert.DeserializeObject<ContextSizeResponse>(result);
#elif UNITY_EDITOR
            Debug.Log($"[Wortal] Mock Context.IsSizeBetween({min}, {max})");
            return new ContextSizeResponse
            {
                Answer = true, // Mocking a positive response
                MinSize = min,
                MaxSize = max,
            };
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Context.IsSizeBetween({min}, {max}) not supported on Android. Returning false.");
            return new ContextSizeResponse { Answer = false, MinSize = min, MaxSize = max };
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Context.IsSizeBetween({min}, {max}) not supported on iOS. Returning false.");
            return new ContextSizeResponse { Answer = false, MinSize = min, MaxSize = max };
#else
            Debug.LogWarning($"[Wortal] Context.IsSizeBetween({min}, {max}) not supported on this platform. Returning false.");
            return new ContextSizeResponse { Answer = false, MinSize = min, MaxSize = max };
#endif
        }

#endregion Public API
#region JSlib Interface

#if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern string ContextGetIdJS();

        [DllImport("__Internal")]
        private static extern string ContextGetTypeJS();

        [DllImport("__Internal")]
        private static extern void ContextGetPlayersJS(Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void ContextChooseJS(string payload, Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void ContextInviteJS(string payload, Action callback, Action<string> errorCallback);

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
        private static extern void ContextSwitchWithPayloadJS(string contextId, string payload, Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern string ContextIsSizeBetweenJS(int min, int max);
#endif

#if UNITY_WEBGL
        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void ContextGetPlayersCallback(string players)
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

            _getPlayersCallback?.Invoke(playersObj);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void ContextChooseCallback()
        {
            _chooseCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void ContextInviteCallback()
        {
            _inviteCallback?.Invoke();
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
#endif

#endregion JSlib Interface
    }
}
