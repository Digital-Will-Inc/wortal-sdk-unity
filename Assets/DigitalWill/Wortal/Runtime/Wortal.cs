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
        public static Action<WortalError> WortalError;
        public static Action OnPause;

        /// <summary>
        /// Ads API
        /// </summary>
        public static WortalAds Ads { get; private set; }
        /// <summary>
        /// Analytics API
        /// </summary>
        public static WortalAnalytics Analytics { get; private set; }
        /// <summary>
        /// Context API
        /// </summary>
        public static WortalContext Context { get; private set; }
        /// <summary>
        /// In-App Purchasing API
        /// </summary>
        public static WortalIAP IAP { get; private set; }
        /// <summary>
        /// Leaderboard API
        /// </summary>
        public static WortalLeaderboard Leaderboard { get; private set; }
        /// <summary>
        /// Player API
        /// </summary>
        public static WortalPlayer Player { get; private set; }
        /// <summary>
        /// Session API
        /// </summary>
        public static WortalSession Session { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            Debug.Log("[Wortal] Initializing Unity SDK..");
            Ads = new WortalAds();
            Analytics = new WortalAnalytics();
            Context = new WortalContext();
            IAP = new WortalIAP();
            Leaderboard = new WortalLeaderboard();
            Player = new WortalPlayer();
            Session = new WortalSession();
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

        [DllImport("__Internal")]
        private static extern void OnPauseJS(Action callback);
    }

#region Error Handling

    [Serializable]
    public struct WortalError
    {
        /// <summary>
        /// Error code. This can be compared to <see cref="WortalErrorCodes"/> for determining the type of error.
        /// </summary>
        [JsonProperty("code")]
        public string Code;
        /// <summary>
        /// Details about the error.
        /// </summary>
        [JsonProperty("message")]
        public string Message;
        /// <summary>
        /// Any context provided about the error, such as the calling method that caught the error.
        /// </summary>
        [JsonProperty("context")]
        public string Context;
    }

    /// <summary>
    /// Types of error codes that can be returned by the Wortal SDK.
    /// </summary>
    [Serializable]
    public static class WortalErrorCodes
    {
        /// <summary>
        /// Function or feature is not currently supported on the platform currently being played on.
        /// </summary>
        public const string NOT_SUPPORTED = "NOT_SUPPORTED";
        /// <summary>
        ///The client does not support the current operation. This may be due to lack of support on the client version
        /// or platform, or because the operation is not allowed for the game or player.
        /// </summary>
        public const string CLIENT_UNSUPPORTED_OPERATION = "CLIENT_UNSUPPORTED_OPERATION";
        /// <summary>
        /// The requested operation is invalid or the current game state. This may include requests that violate
        /// limitations, such as exceeding storage thresholds, or are not available in a certain state, such as making
        /// a context-specific request in a solo context.
        /// </summary>
        public const string INVALID_OPERATION = "INVALID_OPERATION";
        /// <summary>
        /// The parameter(s) passed to the API are invalid. Could indicate an incorrect type, invalid number of
        /// arguments, or a semantic issue (for example, passing an unserializable object to a serializing function).
        /// </summary>
        public const string INVALID_PARAM = "INVALID_PARAM";
        /// <summary>
        /// No leaderboard with the requested name was found. Either the leaderboard does not exist yet, or the name
        /// did not match any registered leaderboard configuration for the game.
        /// </summary>
        public const string LEADERBOARD_NOT_FOUND = "LEADERBOARD_NOT_FOUND";
        /// <summary>
        /// Attempted to write to a leaderboard that's associated with a context other than the one the game is
        /// currently being played in.
        /// </summary>
        public const string LEADERBOARD_WRONG_CONTEXT = "LEADERBOARD_WRONG_CONTEXT";
        /// <summary>
        /// The client experienced an issue with a network request. This is likely due to a transient issue,
        /// such as the player's internet connection dropping.
        /// </summary>
        public const string NETWORK_FAILURE = "NETWORK_FAILURE";
        /// <summary>
        /// The client has not completed setting up payments or is not accepting payments API calls.
        /// </summary>
        public const string PAYMENTS_NOT_INITIALIZED = "PAYMENTS_NOT_INITIALIZED";
        /// <summary>
        /// Represents a rejection due an existing request that conflicts with this one. For example, we will reject
        /// any calls that would surface a Facebook UI when another request that depends on a Facebook UI is pending.
        /// </summary>
        public const string PENDING_REQUEST = "PENDING_REQUEST";
        /// <summary>
        /// Some APIs or operations are being called too often. This is likely due to the game calling a particular
        /// API an excessive amount of times in a very short period. Reducing the rate of requests should cause this
        /// error to go away.
        /// </summary>
        public const string RATE_LIMITED = "RATE_LIMITED";
        /// <summary>
        /// The game attempted to perform a context switch into the current context.
        /// </summary>
        public const string SAME_CONTEXT = "SAME_CONTEXT";
        /// <summary>
        /// An unknown or unspecified issue occurred. This is the default error code returned when the client does
        /// not specify a code.
        /// </summary>
        public const string UNKNOWN = "UNKNOWN";
        /// <summary>
        /// The user made a choice that resulted in a rejection. For example, if the game calls up the Context Switch
        /// dialog and the player closes it, this error code will be included in the promise rejection.
        /// </summary>
        public const string USER_INPUT = "USER_INPUT";
        /// <summary>
        /// Unknown error that the provider SDK did not specify a code for.
        /// </summary>
        public const string RETHROW_FROM_PLATFORM = "RETHROW_FROM_PLATFORM";
    }

#endregion
}
