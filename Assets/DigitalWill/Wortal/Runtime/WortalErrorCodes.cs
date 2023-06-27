namespace DigitalWill.WortalSDK
{
    public enum WortalErrorCodes
    {
        /// <summary>
        /// Function or feature is not currently supported on the platform currently being played on.
        /// </summary>
        NOT_SUPPORTED,
        /// <summary>
        ///The client does not support the current operation. This may be due to lack of support on the client version
        /// or platform, or because the operation is not allowed for the game or player.
        /// </summary>
        CLIENT_UNSUPPORTED_OPERATION,
        /// <summary>
        /// The requested operation is invalid or the current game state. This may include requests that violate
        /// limitations, such as exceeding storage thresholds, or are not available in a certain state, such as making
        /// a context-specific request in a solo context.
        /// </summary>
        INVALID_OPERATION,
        /// <summary>
        /// The parameter(s) passed to the API are invalid. Could indicate an incorrect type, invalid number of
        /// arguments, or a semantic issue (for example, passing an unserializable object to a serializing function).
        /// </summary>
        INVALID_PARAM,
        /// <summary>
        /// No leaderboard with the requested name was found. Either the leaderboard does not exist yet, or the name
        /// did not match any registered leaderboard configuration for the game.
        /// </summary>
        LEADERBOARD_NOT_FOUND,
        /// <summary>
        /// Attempted to write to a leaderboard that's associated with a context other than the one the game is
        /// currently being played in.
        /// </summary>
        LEADERBOARD_WRONG_CONTEXT,
        /// <summary>
        /// The client experienced an issue with a network request. This is likely due to a transient issue,
        /// such as the player's internet connection dropping.
        /// </summary>
        NETWORK_FAILURE,
        /// <summary>
        /// The client has not completed setting up payments or is not accepting payments API calls.
        /// </summary>
        PAYMENTS_NOT_INITIALIZED,
        /// <summary>
        /// Represents a rejection due an existing request that conflicts with this one. For example, we will reject
        /// any calls that would surface a Facebook UI when another request that depends on a Facebook UI is pending.
        /// </summary>
        PENDING_REQUEST,
        /// <summary>
        /// Some APIs or operations are being called too often. This is likely due to the game calling a particular
        /// API an excessive amount of times in a very short period. Reducing the rate of requests should cause this
        /// error to go away.
        /// </summary>
        RATE_LIMITED,
        /// <summary>
        /// The game attempted to perform a context switch into the current context.
        /// </summary>
        SAME_CONTEXT,
        /// <summary>
        /// An unknown or unspecified issue occurred. This is the default error code returned when the client does
        /// not specify a code.
        /// </summary>
        UNKNOWN,
        /// <summary>
        /// The user made a choice that resulted in a rejection. For example, if the game calls up the Context Switch
        /// dialog and the player closes it, this error code will be included in the promise rejection.
        /// </summary>
        USER_INPUT,
        /// <summary>
        /// Unknown error that the provider SDK did not specify a code for.
        /// </summary>
        RETHROW_FROM_PLATFORM,
        /// <summary>
        /// The requested operation has failed. This is the default error code returned when a web request fails. The
        /// status code for the failed request will be included in the error message, if one was provided.
        /// </summary>
        OPERATION_FAILED,
    }
}
