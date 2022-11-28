using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            string data = SessionGetEntryPointDataJS();
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
            SessionGetEntryPointJS(SessionGetEntryPointCallback, Wortal.WortalErrorCallback);
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
            SessionSetSessionDataJS(dataJson);
        }

        /// <summary>
        /// Gets the locale the player is using.
        /// </summary>
        /// <returns>Locale in BCP47 format. http://www.ietf.org/rfc/bcp/bcp47.txt</returns>
        public string GetLocale()
        {
            return SessionGetLocaleJS();
        }

        /// <summary>
        /// Gets the traffic source info for the game.
        /// </summary>
        /// <returns>Traffic source info with the parameters that are attached to the game's URL.</returns>
        public TrafficSource GetTrafficSource()
        {
            string source = SessionGetTrafficSourceJS();
            return JsonConvert.DeserializeObject<TrafficSource>(source);
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
#endregion Types
	}
}
