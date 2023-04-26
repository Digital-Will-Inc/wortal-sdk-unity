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
        /// <returns><see cref="WortalSession.Platform"/> the game is running on.</returns>
        public string GetPlatform()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return SessionGetPlatformJS();
#else
            Debug.Log("[Wortal] Mock Session.SessionGetPlatform()");
            return Platform.DEBUG;
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

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void SessionGetEntryPointCallback(string entryPoint)
        {
            _getEntryPointCallback?.Invoke(entryPoint);
        }

#endregion JSlib Interface
#region Types

        /// <summary>
        /// Traffic source info.
        /// </summary>
        [Serializable]
        public struct TrafficSource
        {
            /// <summary>
            /// Entry point of the session.
            /// </summary>
            [JsonProperty("['r_entrypoint']", NullValueHandling = NullValueHandling.Ignore)]
            public string EntryPoint;
            /// <summary>
            /// UTM source tag.
            /// </summary>
            [JsonProperty("['utm_source']", NullValueHandling = NullValueHandling.Ignore)]
            public string UTMSource;
        }

        /// <summary>
        /// Different platforms the game can be launched from.
        /// </summary>
        [Serializable]
        public static class Platform
        {
            public const string WORTAL = "wortal";
            public const string LINK = "link";
            public const string VIBER = "viber";
            public const string GAME_DISTRIBUTION = "gd";
            public const string FACEBOOK = "facebook";
            public const string DEBUG = "debug";
        }

#endregion Types
    }
}
