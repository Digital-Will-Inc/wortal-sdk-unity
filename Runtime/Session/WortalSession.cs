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
    /// Session API
    /// </summary>
    public class WortalSession
    {
        private static Action<string> _getEntryPointCallback;
        private static Action<Orientation> _onOrientationChangeCallback;
        private static Action _switchGameCallback;

#region Public API

        /// <summary>
        /// Gets the data bound to the entry point.
        /// </summary>
        /// <example><code>
        /// var data = Wortal.Session.GetEntryPointData();
        /// foreach (KeyValuePair&lt;string, object&gt; kvp in data)
        /// {
        ///     Debug.Log("Key name: " + kvp.Key);
        ///     Debug.Log("Value type: " + kvp.Value.GetType());
        /// }
        /// </code></example>
        public IDictionary<string, object> GetEntryPointData()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            string data = SessionGetEntryPointDataJS();
#else
            Debug.Log("[Wortal] Mock Session.GetEntryPointData()");
            Dictionary<string, string> dataObj = new()
            {
                { "link", "share" },
                { "friendID", "player2" },
            };
            string data = JsonConvert.SerializeObject(dataObj);
#endif
            return JsonConvert.DeserializeObject<JObject>(data).ToDictionary();
        }

        /// <summary>
        /// Gets the entry point of where the game started from.
        /// </summary>
        /// <example><code>
        /// Wortal.Session.GetEntryPoint(
        ///     entryPoint => Debug.Log(entryPoint),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>NOT_SUPPORTED</li>
        /// <li>RETHROW_FROM_PLATFORM</li>
        /// </ul></throws>
        public void GetEntryPoint(Action<string> callback, Action<WortalError> errorCallback)
        {
            _getEntryPointCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            SessionGetEntryPointJS(SessionGetEntryPointCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock Session.GetEntryPoint()");
            SessionGetEntryPointCallback("social-share");
#endif
        }

        /// <summary>
        /// Sets the data for this session. This is not persistent and is only used to populate webhook events.
        /// </summary>
        /// <param name="data">Data to store for this session.</param>
        /// <example><code>
        /// Dictionary&lt;string, object&gt; data = new()
        /// {
        ///     { "referrerId", "friend1" },
        /// };
        /// Wortal.Session.SetSessionData(data);
        /// </code></example>
        public void SetSessionData(IDictionary<string, object> data)
        {
            string dataJson = JsonConvert.SerializeObject(data);
#if UNITY_WEBGL && !UNITY_EDITOR
            SessionSetSessionDataJS(dataJson);
#else
            Debug.Log($"[Wortal] Mock Session.SetSessionData({data})");
#endif
        }

        /// <summary>
        /// Gets the locale the player is using.
        /// </summary>
        /// <returns>Locale in BCP47 format. http://www.ietf.org/rfc/bcp/bcp47.txt</returns>
        public string GetLocale()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return SessionGetLocaleJS();
#else
            Debug.Log("[Wortal] Mock Session.GetLocale()");
            return "en-US";
#endif
        }

        /// <summary>
        /// Gets the traffic source info for the game.
        /// </summary>
        /// <returns>Traffic source info with the parameters that are attached to the game's URL.</returns>
        public TrafficSource GetTrafficSource()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            string source = SessionGetTrafficSourceJS();
#else
            Debug.Log("[Wortal] Mock Session.GetTrafficSource()");
            Dictionary<string, string> sourceObj = new()
            {
                { "['utm_source']", "some-source" },
            };
            string source = JsonConvert.SerializeObject(sourceObj);
#endif
            return JsonConvert.DeserializeObject<TrafficSource>(source);
        }

        /// <summary>
        /// Gets the platform the game is running on. This is useful for platform specific code.
        /// For example, if you want to show a different social share asset on Facebook than on Link.
        /// </summary>
        /// <returns><see cref="Platform"/> the game is running on.</returns>
        public Platform GetPlatform()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return (Platform)Enum.Parse(typeof(Platform), SessionGetPlatformJS());
#else
            Debug.Log("[Wortal] Mock Session.SessionGetPlatform()");
            return Platform.debug;
#endif
        }

        /// <summary>
        /// Gets the device the player is using. This is useful for device specific code.
        /// </summary>
        /// <returns>Device the player is using.</returns>
        public Device GetDevice()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return (Device)Enum.Parse(typeof(Device), SessionGetDeviceJS());
#else
            Debug.Log("[Wortal] Mock Session.SessionGetDevice()");
            int random = UnityEngine.Random.Range(0, 3);
            return (Device)random;
#endif
        }

        /// <summary>
        /// Gets the orientation of the device the player is using. This is useful for determining how to display the game.
        /// </summary>
        /// <returns>Orientation of the device the player is using.</returns>
        public Orientation GetOrientation()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return (Orientation)Enum.Parse(typeof(Orientation), SessionGetOrientationJS());
#else
            Debug.Log("[Wortal] Mock Session.SessionGetOrientation()");
            int random = UnityEngine.Random.Range(0, 2);
            return (Orientation)random;
#endif
        }

        /// <summary>
        /// Assigns a callback to be invoked when the orientation of the device changes.
        /// </summary>
        /// <param name="callback">Callback fired when the device orientation changes. Includes the current <see cref="Orientation"/>.</param>
        public void OnOrientationChange(Action<Orientation> callback)
        {
            _onOrientationChangeCallback = callback;
#if UNITY_WEBGL && !UNITY_EDITOR
            SessionOnOrientationChangeJS(SessionOnOrientationChangeCallback);
#else
            Debug.Log("[Wortal] Mock Session.OnOrientationChange()");
            int random = UnityEngine.Random.Range(0, 2);
            _onOrientationChangeCallback((Orientation)random);
#endif
        }

        /// <summary>
        /// Request to switch to another game. The API will reject if the switch fails - else, the client will load the new game.
        /// </summary>
        /// <param name="callback">Void callback event triggered when the async JS function resolves.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Session.SwitchGame("someGameId",
        ///     () => Debug.Log("Switched game successfully"),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>INVALID_PARAMS</li>
        /// <li>USER_INPUT</li>
        /// <li>PENDING_REQUEST</li>
        /// <li>CLIENT_REQUIRES_UPDATE</li>
        /// <li>NOT_SUPPORTED</li>
        /// </ul></throws>
        public void SwitchGame(Action callback, Action<WortalError> errorCallback)
        {
            _switchGameCallback = callback;
            Wortal.WortalError = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            SessionSwitchGameJS(SessionSwitchGameCallback, Wortal.WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock Session.SwitchGame()");
            SessionSwitchGameCallback();
#endif
        }

        /// <summary>
        /// The HappyTime method can be called on various player achievements (beating a boss, reaching a high score, etc.).
        /// It makes the website celebrate (for example by launching some confetti). There is no need to call this when a level
        /// is completed, or an item is obtained.
        /// </summary>
        public void HappyTime()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            SessionHappyTimeJS();
#else
            Debug.Log("[Wortal] Mock Session.HappyTime()");
#endif
        }

        /// <summary>
        /// Tracks the start of a gameplay session, including resuming play after a break.
        /// Call whenever the player starts playing or resumes playing after a break
        /// (menu/loading/achievement screen, game paused, etc.).
        /// </summary>
        public void GameplayStart()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            SessionGameplayStartJS();
#else
            Debug.Log("[Wortal] Mock Session.GameplayStart()");
#endif
        }

        /// <summary>
        /// Tracks the end of a gameplay session, including pausing play or opening a menu.
        /// Call on every game break (entering a menu, switching level, pausing the game, ...)
        /// </summary>
        public void GameplayStop()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            SessionGameplayStopJS();
#else
            Debug.Log("[Wortal] Mock Session.GameplayStop()");
#endif
        }

#endregion Public API
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

#endregion JSlib Interface
    }
}
