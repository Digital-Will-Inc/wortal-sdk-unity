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
        private static Action _initializeCallback;
        private static Action _startGameCallback;
        private static Action<AuthResponse> _authenticateCallback;
        private static Action<bool> _linkAccountCallback;
        private static Action _performHapticFeedbackCallback;

#region Public API

        public static Action<WortalError> WortalError;
        public static Action OnPause;

        /// <summary>
        /// Achievements API
        /// </summary>
        public static WortalAchievements Achievements { get; } = new();
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
        /// Stats API
        /// </summary>
        public static WortalStats Stats { get; } = new();
        /// <summary>
        /// Tournament API
        /// </summary>
        public static WortalTournament Tournament { get; } = new();

        /// <summary>
        /// Returns true if the Wortal SDK has been initialized.
        /// </summary>
        public static bool IsInitialized
        {
            get
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                return IsInitializedJS();
#else
                Debug.Log("[Wortal] Mock IsInitialized()");
                return true;
#endif
            }
        }

        /// <summary>
        /// Initializes the SDK. This should be called before any other SDK functions. It is recommended to call this
        /// as soon as the script has been loaded to shorten the initialization time.
        ///
        /// NOTE: This is only available if the manual initialization option is set to true. Otherwise, the SDK will initialize automatically.
        ///
        /// PLATFORM NOTE: Only supported on Viber, Link and Facebook.
        /// </summary>
        /// <param name="callback">Void callback that is fired when the SDK has initialized successfully.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Initialize(
        ///     () => Debug.Log("SDK Initialized"),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>INITIALIZATION_ERROR</li>
        /// <li>NOT_SUPPORTED</li>
        /// </ul></throws>
        public static void Initialize(Action callback, Action<WortalError> errorCallback)
        {
            _initializeCallback = callback;
            WortalError = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            InitializeJS(InitializeCallback, WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock Initialize()");
            _initializeCallback?.Invoke();
#endif
        }

        /// <summary>
        /// This indicates that the game has finished initial loading and is ready to start. Context information will be
        /// up-to-date when the returned promise resolves. The loading screen will be removed after this is called along with
        /// the following conditions:
        /// <ul>
        /// <li>initializeAsync has been called and resolved</li>
        /// <li>setLoadingProgress has been called with a value of 100</li>
        /// </ul>
        ///
        /// NOTE: This is only available if the manual initialization option is set to true. Otherwise, the game will start automatically.
        ///
        /// PLATFORM NOTE: Only supported on Viber, Link and Facebook.
        /// </summary>
        /// <param name="callback">Void callback that is fired when the game has started.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.StartGame(
        ///     () => Debug.Log("Game Started"),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>INITIALIZATION_ERROR</li>
        /// <li>NOT_SUPPORTED</li>
        /// </ul></throws>
        public static void StartGame(Action callback, Action<WortalError> errorCallback)
        {
            _startGameCallback = callback;
            WortalError = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            StartGameJS(StartGameCallback, WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock StartGame()");
            _startGameCallback?.Invoke();
#endif
        }

        /// <summary>
        /// Starts the authentication process for the player. If the current platform has its own authentication prompt then
        /// this will be displayed.
        /// </summary>
        /// <param name="callback">Callback that is fired when the authentication process is complete. Contains the response
        /// with the authentication status.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.Authenticate(
        ///     response => Debug.Log("Authentication Complete. Status: " + response.Status),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>AUTH_IN_PROGRESS</li>
        /// <li>USER_ALREADY_AUTHENTICATED</li>
        /// <li>USER_INPUT</li>
        /// <li>NOT_SUPPORTED</li>
        /// </ul></throws>
        public static void Authenticate(Action<AuthResponse> callback, Action<WortalError> errorCallback)
        {
            _authenticateCallback = callback;
            WortalError = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            AuthenticateJS(AuthenticateCallback, WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock AuthenticateAsync()");
            var status = new AuthResponse
            {
                Status = AuthStatus.SUCCESS,
            };
            _authenticateCallback?.Invoke(status);
#endif
        }

        /// <summary>
        /// Starts the account linking process for the player. If the current platform has its own account linking prompt then
        /// this will be displayed.
        /// </summary>
        /// <param name="callback">Callback that is fired when the account linking process is complete. Contains whether
        /// the account was linked or not.</param>
        /// <param name="errorCallback">Error callback event with <see cref="WortalError"/> describing the error.</param>
        /// <example><code>
        /// Wortal.LinkAccount(
        ///     result => Debug.Log("Account Linking Complete. Linked: " + result),
        ///     error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
        /// </code></example>
        /// <throws><ul>
        /// <li>LINK_IN_PROGRESS</li>
        /// <li>USER_NOT_AUTHENTICATED</li>
        /// <li>NOT_SUPPORTED</li>
        /// </ul></throws>
        public static void LinkAccount(Action<bool> callback, Action<WortalError> errorCallback)
        {
            _linkAccountCallback = callback;
            WortalError = errorCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            LinkAccountJS(LinkAccountCallback, WortalErrorCallback);
#else
            Debug.Log("[Wortal] Mock LinkAccount()");
            _linkAccountCallback?.Invoke(true);
#endif
        }

        /// <summary>
        /// Sets the loading progress value for the game. This is required for the game to start. Failure to call this with 100
        /// once the game is fully loaded will prevent the game from starting.
        /// </summary>
        /// <param name="value">Percentage of loading complete. Range is 0 to 100.</param>
        public static void SetLoadingProgress(int value)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            SetLoadingProgressJS(value);
#else
            Debug.Log($"[Wortal] Mock SetLoadingProgress({value})");
#endif
        }

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

        [DllImport("__Internal")]
        private static extern bool IsInitializedJS();

        [DllImport("__Internal")]
        private static extern void InitializeJS(Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void StartGameJS(Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void AuthenticateJS(Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void LinkAccountJS(Action<bool> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void SetLoadingProgressJS(int value);

        [DllImport("__Internal")]
        private static extern void OnPauseJS(Action callback);

        [DllImport("__Internal")]
        private static extern string GetSupportedAPIsJS();

        [DllImport("__Internal")]
        private static extern void PerformHapticFeedbackJS(Action callback, Action<string> errorCallback);

        [MonoPInvokeCallback(typeof(Action<string>))]
        public static void WortalErrorCallback(string error)
        {
            WortalError wortalError;

            try
            {
                wortalError = JsonConvert.DeserializeObject<WortalError>(error);
            }
            catch (Exception e)
            {
                wortalError = new WortalError
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message
                };
            }

            WortalError?.Invoke(wortalError);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void InitializeCallback()
        {
            _initializeCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void StartGameCallback()
        {
            _startGameCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void AuthenticateCallback(string status)
        {
            AuthResponse authResponse;

            try
            {
                authResponse = JsonConvert.DeserializeObject<AuthResponse>(status);
            }
            catch (Exception e)
            {
                WortalError error = new()
                {
                    Code = WortalErrorCodes.SERIALIZATION_ERROR.ToString(),
                    Message = e.Message
                };

                WortalError?.Invoke(error);
                return;
            }

            _authenticateCallback?.Invoke(authResponse);
        }

        [MonoPInvokeCallback(typeof(Action<bool>))]
        private static void LinkAccountCallback(bool status)
        {
            _linkAccountCallback?.Invoke(status);
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

#endregion Internal
    }
}
