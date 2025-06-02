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
        /// Session API
        /// </summary>
        public class WortalSession
        {
                private static Action<string> _getEntryPointCallback;
                private static Action<Orientation> _onOrientationChangeCallback;
                private static Action _switchGameCallback;
                private static Action<bool> _onAudioStatusChangeCallback;

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
#if UNITY_WEBGL
            string data = SessionGetEntryPointDataJS();
            return JsonConvert.DeserializeObject<JObject>(data).ToDictionary();
#elif UNITY_EDITOR
                        Debug.Log("[Wortal] Mock Session.GetEntryPointData()");
                        Dictionary<string, object> dataObj = new() // Changed to object for direct return
            {
                { "link", "share" },
                { "friendID", "player2" },
            };
            return dataObj;
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Session.GetEntryPointData not supported on Android. Returning empty dictionary.");
            return new Dictionary<string, object>();
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Session.GetEntryPointData not supported on iOS. Returning empty dictionary.");
            return new Dictionary<string, object>();
#else
            Debug.LogWarning("[Wortal] Session.GetEntryPointData not supported on this platform. Returning empty dictionary.");
            return new Dictionary<string, object>();
#endif
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
#if UNITY_WEBGL
            SessionGetEntryPointJS(SessionGetEntryPointCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
                        Debug.Log("[Wortal] Mock Session.GetEntryPoint()");
                        _getEntryPointCallback?.Invoke("social-share");
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Session.GetEntryPoint not supported on Android. Returning unknown_entry_point.");
            _getEntryPointCallback?.Invoke("unknown_entry_point");
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Session.GetEntryPoint not supported on iOS. Returning unknown_entry_point.");
            _getEntryPointCallback?.Invoke("unknown_entry_point");
#else
            Debug.LogWarning("[Wortal] Session.GetEntryPoint not supported on this platform. Returning unknown_entry_point.");
            _getEntryPointCallback?.Invoke("unknown_entry_point");
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
#if UNITY_WEBGL
                        string dataJson = JsonConvert.SerializeObject(data);
            SessionSetSessionDataJS(dataJson);
#elif UNITY_EDITOR
                        string dataJsonToLog = JsonConvert.SerializeObject(data);
                        Debug.Log($"[Wortal] Mock Session.SetSessionData({dataJsonToLog})");
#elif UNITY_ANDROID
            Debug.LogWarning($"[Wortal] Session.SetSessionData not supported on Android.");
#elif UNITY_IOS
            Debug.LogWarning($"[Wortal] Session.SetSessionData not supported on iOS.");
#else
            Debug.LogWarning($"[Wortal] Session.SetSessionData not supported on this platform.");
#endif
                }

                /// <summary>
                /// Gets the locale the player is using.
                /// </summary>
                /// <returns>Locale in BCP47 format. http://www.ietf.org/rfc/bcp/bcp47.txt</returns>
                public string GetLocale()
                {
#if UNITY_WEBGL
            return SessionGetLocaleJS();
#elif UNITY_EDITOR
                        Debug.Log("[Wortal] Mock Session.GetLocale()");
                        return "en-US";
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Session.GetLocale not supported on Android. Returning en-US.");
            return "en-US";
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Session.GetLocale not supported on iOS. Returning en-US.");
            return "en-US";
#else
            Debug.LogWarning("[Wortal] Session.GetLocale not supported on this platform. Returning en-US.");
            return "en-US";
#endif
                }

                /// <summary>
                /// Gets the traffic source info for the game.
                /// </summary>
                /// <returns>Traffic source info with the parameters that are attached to the game's URL.</returns>
                public TrafficSource GetTrafficSource()
                {
#if UNITY_WEBGL
            string source = SessionGetTrafficSourceJS();
            return JsonConvert.DeserializeObject<TrafficSource>(source);
#elif UNITY_EDITOR
                        Debug.Log("[Wortal] Mock Session.GetTrafficSource()");
                        // Return a new TrafficSource object directly for the mock
                        return new TrafficSource { { "utm_source", "mock-source" } };
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Session.GetTrafficSource not supported on Android. Returning empty TrafficSource.");
            return new TrafficSource();
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Session.GetTrafficSource not supported on iOS. Returning empty TrafficSource.");
            return new TrafficSource();
#else
            Debug.LogWarning("[Wortal] Session.GetTrafficSource not supported on this platform. Returning empty TrafficSource.");
            return new TrafficSource();
#endif
                }

                /// <summary>
                /// Gets the platform the game is running on. This is useful for platform specific code.
                /// For example, if you want to show a different social share asset on Facebook than on Link.
                /// </summary>
                /// <returns><see cref="Platform"/> the game is running on.</returns>
                public Platform GetPlatform()
                {
#if UNITY_WEBGL
            return (Platform)Enum.Parse(typeof(Platform), SessionGetPlatformJS());
#elif UNITY_EDITOR
                        Debug.Log("[Wortal] Mock Session.GetPlatform()"); // Corrected log message
                        return Platform.debug;
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Session.GetPlatform not supported on Android. Returning Platform.debug.");
            return Platform.debug; // Or a new "Unknown" enum if it exists
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Session.GetPlatform not supported on iOS. Returning Platform.debug.");
            return Platform.debug; // Or a new "Unknown" enum if it exists
#else
            Debug.LogWarning("[Wortal] Session.GetPlatform not supported on this platform. Returning Platform.debug.");
            return Platform.debug; // Or a new "Unknown" enum if it exists
#endif
                }

                /// <summary>
                /// Gets the device the player is using. This is useful for device specific code.
                /// </summary>
                /// <returns>Device the player is using.</returns>
                public Device GetDevice()
                {
#if UNITY_WEBGL
            return (Device)Enum.Parse(typeof(Device), SessionGetDeviceJS());
#elif UNITY_EDITOR
                        Debug.Log("[Wortal] Mock Session.GetDevice()"); // Corrected log message
                        int random = UnityEngine.Random.Range(0, 3);
                        return (Device)random;
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Session.GetDevice not supported on Android. Returning Device.DESKTOP.");
            return Device.DESKTOP;
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Session.GetDevice not supported on iOS. Returning Device.DESKTOP.");
            return Device.DESKTOP;
#else
            Debug.LogWarning("[Wortal] Session.GetDevice not supported on this platform. Returning Device.DESKTOP.");
            return Device.DESKTOP;
#endif
                }

                /// <summary>
                /// Gets the orientation of the device the player is using. This is useful for determining how to display the game.
                /// </summary>
                /// <returns>Orientation of the device the player is using.</returns>
                public Orientation GetOrientation()
                {
#if UNITY_WEBGL
            return (Orientation)Enum.Parse(typeof(Orientation), SessionGetOrientationJS().ToUpper());
#elif UNITY_EDITOR
                        Debug.Log("[Wortal] Mock Session.GetOrientation()"); // Corrected log message
                        int random = UnityEngine.Random.Range(0, 2);
                        return (Orientation)random;
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Session.GetOrientation not supported on Android. Returning Orientation.PORTRAIT.");
            return Orientation.PORTRAIT;
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Session.GetOrientation not supported on iOS. Returning Orientation.PORTRAIT.");
            return Orientation.PORTRAIT;
#else
            Debug.LogWarning("[Wortal] Session.GetOrientation not supported on this platform. Returning Orientation.PORTRAIT.");
            return Orientation.PORTRAIT;
#endif
                }

                /// <summary>
                /// Assigns a callback to be invoked when the orientation of the device changes.
                /// </summary>
                /// <param name="callback">Callback fired when the device orientation changes. Includes the current <see cref="Orientation"/>.</param>
                public void OnOrientationChange(Action<Orientation> callback)
                {
                        _onOrientationChangeCallback = callback;
#if UNITY_WEBGL
            SessionOnOrientationChangeJS(SessionOnOrientationChangeCallback);
#elif UNITY_EDITOR
                        Debug.Log("[Wortal] Mock Session.OnOrientationChange()");
                        int random = UnityEngine.Random.Range(0, 2);
                        _onOrientationChangeCallback?.Invoke((Orientation)random);
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Session.OnOrientationChange not supported on Android. Invoking with Orientation.PORTRAIT.");
            _onOrientationChangeCallback?.Invoke(Orientation.PORTRAIT);
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Session.OnOrientationChange not supported on iOS. Invoking with Orientation.PORTRAIT.");
            _onOrientationChangeCallback?.Invoke(Orientation.PORTRAIT);
#else
            Debug.LogWarning("[Wortal] Session.OnOrientationChange not supported on this platform. Invoking with Orientation.PORTRAIT.");
            _onOrientationChangeCallback?.Invoke(Orientation.PORTRAIT);
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
#if UNITY_WEBGL
            SessionSwitchGameJS(SessionSwitchGameCallback, Wortal.WortalErrorCallback);
#elif UNITY_EDITOR
                        Debug.Log("[Wortal] Mock Session.SwitchGame()");
                        _switchGameCallback?.Invoke();
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Session.SwitchGame not supported on Android.");
            // Potentially invoke errorCallback here if the API implies an error for non-support
            // For now, just invoking success callback as per other placeholders.
            _switchGameCallback?.Invoke();
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Session.SwitchGame not supported on iOS.");
            _switchGameCallback?.Invoke();
#else
            Debug.LogWarning("[Wortal] Session.SwitchGame not supported on this platform.");
            _switchGameCallback?.Invoke();
#endif
                }

                /// <summary>
                /// The HappyTime method can be called on various player achievements (beating a boss, reaching a high score, etc.).
                /// It makes the website celebrate (for example by launching some confetti). There is no need to call this when a level
                /// is completed, or an item is obtained.
                /// </summary>
                public void HappyTime()
                {
#if UNITY_WEBGL
            SessionHappyTimeJS();
#elif UNITY_EDITOR
                        Debug.Log("[Wortal] Mock Session.HappyTime()");
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Session.HappyTime not supported on Android.");
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Session.HappyTime not supported on iOS.");
#else
            Debug.LogWarning("[Wortal] Session.HappyTime not supported on this platform.");
#endif
                }

                /// <summary>
                /// Tracks the start of a gameplay session, including resuming play after a break.
                /// Call whenever the player starts playing or resumes playing after a break
                /// (menu/loading/achievement screen, game paused, etc.).
                /// </summary>
                public void GameplayStart()
                {
#if UNITY_WEBGL
            SessionGameplayStartJS();
#elif UNITY_EDITOR
                        Debug.Log("[Wortal] Mock Session.GameplayStart()");
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Session.GameplayStart not supported on Android.");
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Session.GameplayStart not supported on iOS.");
#else
            Debug.LogWarning("[Wortal] Session.GameplayStart not supported on this platform.");
#endif
                }

                /// <summary>
                /// Tracks the end of a gameplay session, including pausing play or opening a menu.
                /// Call on every game break (entering a menu, switching level, pausing the game, ...)
                /// </summary>
                public void GameplayStop()
                {
#if UNITY_WEBGL
            SessionGameplayStopJS();
#elif UNITY_EDITOR
                        Debug.Log("[Wortal] Mock Session.GameplayStop()");
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Session.GameplayStop not supported on Android.");
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Session.GameplayStop not supported on iOS.");
#else
            Debug.LogWarning("[Wortal] Session.GameplayStop not supported on this platform.");
#endif
                }

                /// <summary>
                /// Returns whether the audio is enabled for the player.
                /// </summary>
                /// <returns>True if audio is enabled, false if it is disabled.</returns>
                public bool IsAudioEnabled()
                {
#if UNITY_WEBGL // Corrected from Unity_WEBGL
        return SessionIsAudioEnabledJS();
#elif UNITY_EDITOR
                        Debug.Log("[Wortal] Mock Session.IsAudioEnabled()");
                        return true;
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Session.IsAudioEnabled not supported on Android. Returning true.");
            return true;
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Session.IsAudioEnabled not supported on iOS. Returning true.");
            return true;
#else
            Debug.LogWarning("[Wortal] Session.IsAudioEnabled not supported on this platform. Returning true.");
            return true;
#endif
                }

                /// <summary>
                /// Assigns a callback to be invoked when the audio status of the player changes.
                /// </summary>
                /// <param name="callback">Callback to be invoked when the audio status of the player changes.</param>
                public void OnAudioStatusChange(Action<bool> callback)
                {
                        _onAudioStatusChangeCallback = callback;
#if UNITY_WEBGL
        SessionOnAudioStatusChangeJS(SessionOnAudioStatusChangeCallback);
#elif UNITY_EDITOR
                        Debug.Log("[Wortal] Mock Session.OnAudioStatusChange()");
                        int random = UnityEngine.Random.Range(0, 2); // Range is exclusive, so (0, 2) gives 0 or 1.
                        _onAudioStatusChangeCallback?.Invoke(random == 1);
#elif UNITY_ANDROID
            Debug.LogWarning("[Wortal] Session.OnAudioStatusChange not supported on Android. Invoking with true.");
            _onAudioStatusChangeCallback?.Invoke(true);
#elif UNITY_IOS
            Debug.LogWarning("[Wortal] Session.OnAudioStatusChange not supported on iOS. Invoking with true.");
            _onAudioStatusChangeCallback?.Invoke(true);
#else
            Debug.LogWarning("[Wortal] Session.OnAudioStatusChange not supported on this platform. Invoking with true.");
            _onAudioStatusChangeCallback?.Invoke(true);
#endif
                }

                #endregion Public API
                #region JSlib Interface

#if UNITY_WEBGL
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
                private static extern bool SessionIsAudioEnabledJS(); // Corrected return type based on usage

                [DllImport("__Internal")]
                private static extern void SessionOnAudioStatusChangeJS(Action<int> callback);
#endif

#if UNITY_WEBGL
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
#endif

                #endregion JSlib Interface
        }
}
