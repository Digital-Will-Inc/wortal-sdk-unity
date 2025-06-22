using System;
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class WebGLWortalContext : IWortalContext
    {
        private static Action<ContextPlayer[]> _getPlayersCallback;
        private static Action _inviteCallback;
        private static Action<bool> _shareCallback;
        private static Action<bool> _shareLinkCallback;
        private static Action _updateCallback;
        private static Action _switchCallback;
        private static Action _chooseCallback;
        private static Action _createCallback;
        private static Action<WortalError> _errorCallback;

        public bool IsSupported => true; // Changed to true since we're implementing the functionality

        public string GetID()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return PluginManager.ContextGetIdJS();
#else
            Debug.Log("[Wortal] Mock Context.GetID");
            return "mock-context-id";
#endif
        }

        public ContextType GetType()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            string typeString = PluginManager.ContextGetTypeJS();
            if (Enum.TryParse<ContextType>(typeString, true, out ContextType contextType))
            {
                return contextType;
            }
            return ContextType.SOLO;
#else
            Debug.Log("[Wortal] Mock Context.GetType");
            return ContextType.SOLO;
#endif
        }

        public void GetPlayersAsync(Action<ContextPlayer[]> onSuccess, Action<WortalError> onError)
        {
            _getPlayersCallback = onSuccess;
            _errorCallback = onError;
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.ContextGetPlayersJS(ContextGetPlayersCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock Context.GetPlayersAsync()");
            var player = new ContextPlayer
            {
                id = "player1",
                name = "Player",
                photo = "data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==",
            };
            var players = new[] { player };
            onSuccess?.Invoke(players);
#endif
        }

        public void InviteAsync(InvitePayload payload, Action onSuccess, Action<WortalError> onError)
        {
            _inviteCallback = onSuccess;
            _errorCallback = onError;
            string payloadObj = JsonConvert.SerializeObject(payload);
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.ContextInviteJS(payloadObj, ContextInviteCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Context.InviteAsync({payload})");
            onSuccess?.Invoke();
#endif
        }

        public void ShareAsync(SharePayload payload, Action<bool> onSuccess, Action<WortalError> onError)
        {
            _shareCallback = onSuccess;
            _errorCallback = onError;
            string payloadObj = JsonConvert.SerializeObject(payload);
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.ContextShareJS(payloadObj, ContextShareCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Context.ShareAsync({payload})");
            onSuccess?.Invoke(true);
#endif
        }

        public void ShareLinkAsync(LinkSharePayload payload, Action<bool> onSuccess, Action<WortalError> onError)
        {
            _shareLinkCallback = onSuccess;
            _errorCallback = onError;
            string payloadObj = JsonConvert.SerializeObject(payload);
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.ContextShareLinkJS(payloadObj, ContextShareLinkCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Context.ShareLinkAsync({payload})");
            onSuccess?.Invoke(true);
#endif
        }

        public void UpdateAsync(UpdatePayload payload, Action onSuccess, Action<WortalError> onError)
        {
            _updateCallback = onSuccess;
            _errorCallback = onError;
            string payloadObj = JsonConvert.SerializeObject(payload);
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.ContextUpdateJS(payloadObj, ContextUpdateCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Context.UpdateAsync({payload})");
            onSuccess?.Invoke();
#endif
        }

        public void SwitchAsync(string contextID, Action onSuccess, Action<WortalError> onError)
        {
            _switchCallback = onSuccess;
            _errorCallback = onError;
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.ContextSwitchJS(contextID, ContextSwitchCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Context.SwitchAsync({contextID})");
            onSuccess?.Invoke();
#endif
        }

        public void ChooseAsync(ContextChoosePayload options, Action onSuccess, Action<WortalError> onError)
        {
            _chooseCallback = onSuccess;
            _errorCallback = onError;
            string payloadObj = JsonConvert.SerializeObject(options);
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.ContextChooseJS(payloadObj, ContextChooseCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Context.ChooseAsync({options})");
            onSuccess?.Invoke();
#endif
        }

        public void CreateAsync(string playerID, Action onSuccess, Action<WortalError> onError)
        {
            _createCallback = onSuccess;
            _errorCallback = onError;
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.ContextCreateJS(playerID, ContextCreateCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Context.CreateAsync({playerID})");
            onSuccess?.Invoke();
#endif
        }

        #region Callback Methods

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void ContextGetPlayersCallback(string players)
        {
            ContextPlayer[] playersObj;

            try
            {
                // Convert WortalPlayer[] to ContextPlayer[] if needed
                var wortalPlayers = JsonConvert.DeserializeObject<ContextPlayer[]>(players);
                playersObj = new ContextPlayer[wortalPlayers.Length];

                for (int i = 0; i < wortalPlayers.Length; i++)
                {
                    playersObj[i] = new ContextPlayer
                    {
                        id = wortalPlayers[i].id,
                        name = wortalPlayers[i].name,
                        photo = wortalPlayers[i].photo,
                    };
                }
            }
            catch (Exception e)
            {
                WortalError error = new WortalError
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message,
                    Context = "ContextGetPlayersCallback"
                };

                _errorCallback?.Invoke(error);
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
            // Convert int result to bool (shareResult > 0 means success)
            _shareCallback?.Invoke(shareResult > 0);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void ContextShareLinkCallback()
        {
            _shareLinkCallback?.Invoke(true);
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

        #endregion Callback Methods
    }
}
