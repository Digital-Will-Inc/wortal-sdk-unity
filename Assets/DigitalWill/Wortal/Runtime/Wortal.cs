using System;
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Wortal API
    /// </summary>
    public static class Wortal
    {
        private static Action _performHapticFeedbackCallback;

#region Public API

        public static Action<WortalError> WortalError;
        public static Action OnPause;

        /// <summary>
        /// Ads API
        /// </summary>
        public static WortalAds Ads { get; } = new();
        /// <summary>
        /// Analytics API
        /// </summary>
        public static WortalAnalytics Analytics { get; } = new();
        /// <summary>
        /// Context API
        /// </summary>
        public static WortalContext Context { get; } = new();
        /// <summary>
        /// In-App Purchasing API
        /// </summary>
        public static WortalIAP IAP { get; } = new();
        /// <summary>
        /// Leaderboard API
        /// </summary>
        public static WortalLeaderboard Leaderboard { get; } = new();
        /// <summary>
        /// Notifications API
        /// </summary>
        public static WortalNotifications Notifications { get; } = new();
        /// <summary>
        /// Player API
        /// </summary>
        public static WortalPlayer Player { get; } = new();
        /// <summary>
        /// Session API
        /// </summary>
        public static WortalSession Session { get; } = new();

        /// <summary>
        /// Get the list of APIs supported by the current platform.
        /// </summary>
        /// <returns>String array containing all APIs supported.</returns>
        /// <example><code>
        /// string[] supportedAPIs = Wortal.GetSupportedAPIs();
        /// int index = Array.IndexOf(supportedAPIs, "iap.makePurchaseAsync");
        /// IAPShopButton.SetActive(index > -1);
        /// </code></example>
        public static string[] GetSupportedAPIs()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return JsonConvert.DeserializeObject<string[]>(GetSupportedAPIsJS());
#else
            Debug.Log("[Wortal] Mock GetSupportedAPIs()");
            return new[]
            {
                "mock.API",
                "mock.API2",
            };
#endif
        }

        /// <summary>
        /// Perform a haptic feedback effect.
        /// </summary>
        /// <param name="callback">Callback for after the request was made. If the device is not supported then its
        /// possible the underlying Promise is left pending and this callback is never reached. Do not depend on this callback.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        public static void PerformHapticFeedback(Action callback, Action<WortalError> errorCallback)
        {
            _performHapticFeedbackCallback = callback;
            WortalError = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            PerformHapticFeedbackJS(PerformHapticFeedbackCallback, WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock PerformHapticFeedback()");
#endif
        }

#endregion Public API
#region Internal

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            Debug.Log("[Wortal] Initializing Unity SDK..");
#if UNITY_WEBGL && !UNITY_EDITOR
            OnPauseJS(OnPauseCallback);
#endif
            Debug.Log("[Wortal] Unity SDK initialization complete.");
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        public static void WortalErrorCallback(string error)
        {
            var wortalError = JsonConvert.DeserializeObject<WortalError>(error);
            WortalError?.Invoke(wortalError);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnPauseCallback()
        {
            OnPause?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void PerformHapticFeedbackCallback()
        {
            _performHapticFeedbackCallback?.Invoke();
        }

        [DllImport("__Internal")]
        private static extern void OnPauseJS(Action callback);

        [DllImport("__Internal")]
        private static extern string GetSupportedAPIsJS();

        [DllImport("__Internal")]
        private static extern void PerformHapticFeedbackJS(Action callback, Action<string> errorCallback);

#endregion Internal
    }
}
