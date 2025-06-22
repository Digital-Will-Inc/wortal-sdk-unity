using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class WebGLWortalPlayer : IWortalPlayer
    {
        private static Action<IDictionary<string, object>> _getDataCallback;
        private static Action _setDataCallback;
        private static Action _flushDataCallback;
        private static Action<IWortalPlayer[]> _getConnectedPlayersCallback;
        private static Action<string, string> _getSignedPlayerInfoCallback;
        private static Action<string> _getASIDCallback;
        private static Action<string, string> _getSignedASIDCallback;
        private static Action<bool> _canSubscribeBotCallback;
        private static Action _subscribeBotCallback;
        private static Action<WortalError> _errorCallback;

        public bool IsSupported => true; // Changed to true since we're implementing the functionality

        public string GetID()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return PluginManager.PlayerGetIDJS();
#else
            Debug.Log("[Wortal] Mock Player.GetID()");
            return "player1";
#endif
        }

        public string GetName()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return PluginManager.PlayerGetNameJS();
#else
            Debug.Log("[Wortal] Mock Player.GetName()");
            return "Player";
#endif
        }

        public string GetPhoto()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return PluginManager.PlayerGetPhotoJS();
#else
            Debug.Log("[Wortal] Mock Player.GetPhoto()");
            return "data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==";
#endif
        }

        public bool IsFirstPlay()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return PluginManager.PlayerIsFirstPlayJS();
#else
            Debug.Log("[Wortal] Mock Player.IsFirstPlay()");
            return false;
#endif
        }

        public void GetConnectedPlayersAsync(GetConnectedPlayersPayload payload, Action<IWortalPlayer[]> onSuccess, Action<WortalError> onError)
        {
            _getConnectedPlayersCallback = onSuccess;
            _errorCallback = onError;
            string payloadStr = JsonConvert.SerializeObject(payload);
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.PlayerGetConnectedPlayersJS(payloadStr, PlayerGetConnectedPlayersCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Player.GetConnectedPlayersAsync({payload})");
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

        public void GetSignedPlayerInfoAsync(Action<string, string> onSuccess, Action<WortalError> onError)
        {
            _getSignedPlayerInfoCallback = onSuccess;
            _errorCallback = onError;
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.PlayerGetSignedPlayerInfoJS(PlayerGetSignedPlayerInfoCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock Player.GetSignedPlayerInfo()");
            PlayerGetSignedPlayerInfoCallback("player1", "some-signature");
#endif
        }

        public void CanSubscribeBotAsync(Action<bool> onSuccess, Action<WortalError> onError)
        {
            _canSubscribeBotCallback = onSuccess;
            _errorCallback = onError;
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.PlayerCanSubscribeBotJS(PlayerCanSubscribeBotCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock Player.CanSubscribeBot()");
            PlayerCanSubscribeBotCallback(true);
#endif
        }

        public void SubscribeBotAsync(Action onSuccess, Action<WortalError> onError)
        {
            _subscribeBotCallback = onSuccess;
            _errorCallback = onError;
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.PlayerSubscribeBotJS(PlayerSubscribeBotCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock Player.SubscribeBot()");
            PlayerSubscribeBotCallback();
#endif
        }

        public void GetDataAsync(string[] keys, Action<PlayerData> onSuccess, Action<WortalError> onError)
        {
            // Convert PlayerData callback to IDictionary callback for compatibility with original implementation
            _getDataCallback = (data) =>
            {
                var playerData = new PlayerData();
                playerData.data = new Dictionary<string, object>(data);
                onSuccess?.Invoke(playerData);
            };
            _errorCallback = onError;
            string keysStr = string.Join("|", keys);
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.PlayerGetDataJS(keysStr, PlayerGetDataCallback, Wortal.WortalErrorCallback);
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

        public void SetDataAsync(PlayerData data, Action onSuccess, Action<WortalError> onError)
        {
            _setDataCallback = onSuccess;
            _errorCallback = onError;
            string dataObj = JsonConvert.SerializeObject(data.data);
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.PlayerSetDataJS(dataObj, PlayerSetDataCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log($"[Wortal] Mock Player.SetData({data})");
            PlayerSetDataCallback();
#endif
        }

        public void FlushDataAsync(Action onSuccess, Action<WortalError> onError)
        {
            _flushDataCallback = onSuccess;
            _errorCallback = onError;
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.PlayerFlushDataJS(PlayerFlushDataCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock Player.FlushData()");
            PlayerFlushDataCallback();
#endif
        }

        public void GetStatsAsync(string[] keys, Action<PlayerStats> onSuccess, Action<WortalError> onError)
        {
            // Stats functionality is not in the original WortalPlayer, so we'll implement as mock for now
            Debug.Log($"[Wortal] Mock Player.GetStatsAsync({string.Join(", ", keys)})");
            var stats = new PlayerStats();
            foreach (var key in keys)
            {
                stats.SetStat(key, 0);
            }
            onSuccess?.Invoke(stats);
        }

        public void SetStatsAsync(PlayerStats stats, Action onSuccess, Action<WortalError> onError)
        {
            // Stats functionality is not in the original WortalPlayer, so we'll implement as mock for now
            Debug.Log($"[Wortal] Mock Player.SetStatsAsync({stats})");
            onSuccess?.Invoke();
        }

        public void IncrementStatsAsync(PlayerStats increments, Action<PlayerStats> onSuccess, Action<WortalError> onError)
        {
            // Stats functionality is not in the original WortalPlayer, so we'll implement as mock for now
            Debug.Log($"[Wortal] Mock Player.IncrementStatsAsync({increments})");
            onSuccess?.Invoke(increments);
        }

        public void GetASIDAsync(Action<string> onSuccess, Action<WortalError> onError)
        {
            _getASIDCallback = onSuccess;
            _errorCallback = onError;
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.PlayerGetASIDJS(PlayerGetASIDCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock Player.GetASID()");
            PlayerGetASIDCallback("player1");
#endif
        }

        public void GetSignedASIDAsync(Action<string, string> onSuccess, Action<WortalError> onError)
        {
            _getSignedASIDCallback = onSuccess;
            _errorCallback = onError;
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.PlayerGetSignedASIDJS(PlayerGetSignedASIDCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock Player.GetSignedASID()");
            PlayerGetSignedASIDCallback("player1", "some-signature");
#endif
        }

        #region Callback Methods

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
                WortalError error = new WortalError
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message,
                    Context = "PlayerGetDataCallback"
                };

                _errorCallback?.Invoke(error);
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
            IWortalPlayer[] playersObj;

            try
            {
                // Convert Player[] to IWortalPlayer[] for interface compatibility
                var playerArray = JsonConvert.DeserializeObject<WebGLWortalPlayer[]>(players);
                playersObj = new IWortalPlayer[playerArray.Length];

                for (int i = 0; i < playerArray.Length; i++)
                {
                    playersObj[i] = playerArray[i];
                }
            }
            catch (Exception e)
            {
                WortalError error = new WortalError
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message,
                    Context = "PlayerGetConnectedPlayersCallback"
                };

                _errorCallback?.Invoke(error);
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

        #endregion Callback Methods
    }
}
