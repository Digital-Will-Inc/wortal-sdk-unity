using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class WebGLWortalSession : IWortalSession
    {
        private static Action<string> _getEntryPointCallback;
        private static Action<Orientation> _onOrientationChangeCallback;
        private static Action _switchGameCallback;
        private static Action<bool> _onAudioStatusChangeCallback;
        private static Action<WortalError> _errorCallback;

        public bool IsSupported => true; // Changed to true since we're implementing the functionality

        public bool IsAudioEnabled =>
#if UNITY_WEBGL && !UNITY_EDITOR
            SessionIsAudioEnabledJS();
#else
            true;
#endif

        public string GetGameID()
        {
            // Game ID is typically set in the Wortal configuration, not retrieved from session
            Debug.Log("[Wortal] Mock Session.GetGameID()");
            return "mock-game-id";
        }

        public Platform GetPlatform()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return (Platform)Enum.Parse(typeof(Platform), SessionGetPlatformJS());
#else
            Debug.Log("[Wortal] Mock Session.GetPlatform()");
            return Platform.debug;
#endif
        }

        public Device GetDevice()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return (Device)Enum.Parse(typeof(Device), SessionGetDeviceJS());
#else
            Debug.Log("[Wortal] Mock Session.GetDevice()");
            int random = UnityEngine.Random.Range(0, 3);
            return (Device)random;
#endif
        }

        public Orientation GetOrientation()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return (Orientation)Enum.Parse(typeof(Orientation), SessionGetOrientationJS());
#else
            Debug.Log("[Wortal] Mock Session.GetOrientation()");
            return Screen.width > Screen.height ? Orientation.LANDSCAPE : Orientation.PORTRAIT;
#endif
        }

        public string GetLocale()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return SessionGetLocaleJS();
#else
            Debug.Log("[Wortal] Mock Session.GetLocale()");
            return "en-US";
#endif
        }

        public TrafficSource GetTrafficSource()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            string source = SessionGetTrafficSourceJS();
            return JsonConvert.DeserializeObject<TrafficSource>(source);
#else
            Debug.Log("[Wortal] Mock Session.GetTrafficSource()");
            Dictionary<string, string> sourceObj = new()
            {
                { "['utm_source']", "some-source" },
            };
            string source = JsonConvert.SerializeObject(sourceObj);
            return JsonConvert.DeserializeObject<TrafficSource>(source);
#endif
        }

        public void GetEntryPointAsync(Action<string> onSuccess, Action<WortalError> onError)
        {
            _getEntryPointCallback = onSuccess;
            _errorCallback = onError;
#if UNITY_WEBGL && !UNITY_EDITOR
            SessionGetEntryPointJS(SessionGetEntryPointCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock Session.GetEntryPoint()");
            SessionGetEntryPointCallback("social-share");
#endif
        }

        public object GetEntryPointData()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            string data = SessionGetEntryPointDataJS();
            return JsonConvert.DeserializeObject<JObject>(data).ToDictionary();
#else
            Debug.Log("[Wortal] Mock Session.GetEntryPointData()");
            Dictionary<string, string> dataObj = new()
            {
                { "link", "share" },
                { "friendID", "player2" },
            };
            return dataObj;
#endif
        }

        public void StartGame(Action onSuccess, Action<WortalError> onError)
        {
            // StartGame is typically handled by the Wortal initialization, not a separate call
            Debug.Log("[Wortal] Session.StartGame()");
            onSuccess?.Invoke();
        }

        public void SetLoadingProgress(int progress)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            // This would typically be handled by Wortal.SetLoadingProgress if available
            Debug.Log($"[Wortal] Session.SetLoadingProgress({progress})");
#else
            Debug.Log($"[Wortal] Mock Session.SetLoadingProgress({progress})");
#endif
        }

        public void SetSessionData(object data)
        {
            IDictionary<string, object> dataDict;
            if (data is IDictionary<string, object> dict)
            {
                dataDict = dict;
            }
            else
            {
                // Convert object to dictionary if needed
                string jsonString = JsonConvert.SerializeObject(data);
                dataDict = JsonConvert.DeserializeObject<JObject>(jsonString).ToDictionary();
            }

            string dataJson = JsonConvert.SerializeObject(dataDict);
#if UNITY_WEBGL && !UNITY_EDITOR
            SessionSetSessionDataJS(dataJson);
#else
            Debug.Log($"[Wortal] Mock Session.SetSessionData({data})");
#endif
        }

        public object GetSessionData()
        {
            // Session data is typically set, not retrieved. This might not be available in the original API
            Debug.Log("[Wortal] Session.GetSessionData() - Not available in original API");
            return null;
        }

        public void GameReady()
        {
            // GameReady is typically handled by Wortal initialization
            Debug.Log("[Wortal] Session.GameReady()");
        }

        public void GameplayStart()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            SessionGameplayStartJS();
#else
            Debug.Log("[Wortal] Mock Session.GameplayStart()");
#endif
        }

        public void GameplayStop()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            SessionGameplayStopJS();
#else
            Debug.Log("[Wortal] Mock Session.GameplayStop()");
#endif
        }

        public void HappyTime()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            SessionHappyTimeJS();
#else
            Debug.Log("[Wortal] Mock Session.HappyTime()");
#endif
        }

        public void OnAudioStatusChange(Action<bool> onAudioEnabled)
        {
            _onAudioStatusChangeCallback = onAudioEnabled;
#if UNITY_WEBGL && !UNITY_EDITOR
            SessionOnAudioStatusChangeJS(SessionOnAudioStatusChangeCallback);
#else
            Debug.Log("[Wortal] Mock Session.OnAudioStatusChange()");
            int random = UnityEngine.Random.Range(0, 2);
            _onAudioStatusChangeCallback?.Invoke(random == 1);
#endif
        }

        /// <summary>
        /// Assigns a callback to be invoked when the orientation of the device changes.
        /// </summary>
        /// <param name="callback">Callback fired when the device orientation changes.</param>
        public void OnOrientationChange(Action<Orientation> callback)
        {
            _onOrientationChangeCallback = callback;
#if UNITY_WEBGL && !UNITY_EDITOR
            SessionOnOrientationChangeJS(SessionOnOrientationChangeCallback);
#else
            Debug.Log("[Wortal] Mock Session.OnOrientationChange()");
            int random = UnityEngine.Random.Range(0, 2);
            _onOrientationChangeCallback?.Invoke((Orientation)random);
#endif
        }

        /// <summary>
        /// Request to switch to another game.
        /// </summary>
        /// <param name="callback">Success callback.</param>
        /// <param name="errorCallback">Error callback.</param>
        public void SwitchGame(Action callback, Action<WortalError> errorCallback)
        {
            _switchGameCallback = callback;
            _errorCallback = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            SessionSwitchGameJS(SessionSwitchGameCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock Session.SwitchGame()");
            SessionSwitchGameCallback();
#endif
        }

        #region JSlib Interface

        [DllImport("__Internal")]
        private static extern string SessionGetEntryPointDataJS();

        [DllImport("__Internal")]
        private static extern void SessionGetEntryPointJS(Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void SessionSetSessionDataJS(string data);

        [DllImport("__Internal")]
        private static extern string SessionGetLocaleJS();

        [DllImport("__Internal")]
        private static extern string SessionGetTrafficSourceJS();

        [DllImport("__Internal")]
        private static extern string SessionGetPlatformJS();

        [DllImport("__Internal")]
        private static extern string SessionGetDeviceJS();

        [DllImport("__Internal")]
        private static extern string SessionGetOrientationJS();

        [DllImport("__Internal")]
        private static extern void SessionOnOrientationChangeJS(Action<string> callback);

        [DllImport("__Internal")]
        private static extern void SessionSwitchGameJS(Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void SessionHappyTimeJS();

        [DllImport("__Internal")]
        private static extern void SessionGameplayStartJS();

        [DllImport("__Internal")]
        private static extern void SessionGameplayStopJS();

        [DllImport("__Internal")]
        private static extern bool SessionIsAudioEnabledJS();

        [DllImport("__Internal")]
        private static extern void SessionOnAudioStatusChangeJS(Action<int> callback);

        #endregion JSlib Interface

        #region Callback Methods

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void SessionGetEntryPointCallback(string entryPoint)
        {
            _getEntryPointCallback?.Invoke(entryPoint);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void SessionOnOrientationChangeCallback(string orientation)
        {
            _onOrientationChangeCallback?.Invoke((Orientation)Enum.Parse(typeof(Orientation), orientation.ToUpper()));
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void SessionSwitchGameCallback()
        {
            _switchGameCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<int>))]
        private static void SessionOnAudioStatusChangeCallback(int isEnabled)
        {
            bool isAudio = isEnabled == 1;
            _onAudioStatusChangeCallback?.Invoke(isAudio);
        }

        #endregion Callback Methods
    }
}
