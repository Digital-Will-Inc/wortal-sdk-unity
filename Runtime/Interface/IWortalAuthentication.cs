using System;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Interface for authentication functionality across platforms
    /// </summary>
    public interface IWortalAuthentication
    {
        /// <summary>
        /// Authenticates the player on the current platform
        /// </summary>
        /// <param name="onSuccess">Callback for successful authentication</param>
        /// <param name="onError">Callback for authentication errors</param>
        void Authenticate(Action<AuthResponse> onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Links the current account with platform-specific account
        /// </summary>
        /// <param name="onSuccess">Callback for successful linking</param>
        /// <param name="onError">Callback for linking errors</param>
        void LinkAccount(Action<bool> onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Gets the current authentication status
        /// </summary>
        AuthStatus GetAuthStatus();

        /// <summary>
        /// Checks if authentication is supported on this platform
        /// </summary>
        bool IsSupported { get; }
    }
}
