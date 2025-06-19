using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace DigitalWill.WortalSDK
{
    public class iOSWortalAuthentication : IWortalAuthentication
    {
        public bool IsSupported => true;

        public void Authenticate(Action<AuthResponse> onSuccess, Action<WortalError> onError)
        {
#if UNITY_IOS
            Debug.Log("[iOS] Authenticating with Apple Game Center...");

            // Authenticate with Game Center directly using Social API
            // Unity automatically uses GameCenter implementation on iOS
            Social.localUser.Authenticate((success) =>
            {
                if (success)
                {
                    Debug.Log("[iOS] Apple Game Center authentication successful");
                    Debug.Log($"[iOS] User ID: {Social.localUser.id}");
                    Debug.Log($"[iOS] User Name: {Social.localUser.userName}");

                    var authResponse = new AuthResponse
                    {
                        Status = AuthStatus.SUCCESS,
                        UserID = Social.localUser.id ?? string.Empty,
                        UserName = Social.localUser.userName ?? "Game Center User",
                        Token = GenerateGameCenterToken(),
                        Provider = "GameCenter"
                    };

                    onSuccess?.Invoke(authResponse);
                }
                else
                {
                    Debug.LogWarning("[iOS] Apple Game Center authentication failed");

                    var error = new WortalError
                    {
                        Code = "AUTHENTICATION_FAILED",
                        Message = "Apple Game Center authentication failed. User may have cancelled or Game Center is not available.",
                        Context = "iOSWortalAuthentication.Authenticate"
                    };

                    onError?.Invoke(error);
                }
            });
#else
            Debug.LogError("[iOS] Game Center authentication called on non-iOS platform");

            var error = new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "Game Center authentication is only supported on iOS platform",
                Context = "iOSWortalAuthentication.Authenticate"
            };

            onError?.Invoke(error);
#endif
        }

        public void LinkAccount(Action<bool> onSuccess, Action<WortalError> onError)
        {
#if UNITY_IOS
            Debug.Log("[iOS] Linking Apple Game Center account...");

            if (Social.localUser.authenticated)
            {
                // Account is already authenticated and linked
                Debug.Log("[iOS] Apple Game Center account already linked");
                onSuccess?.Invoke(true);
            }
            else
            {
                // Try to authenticate first, then consider it linked
                Debug.Log("[iOS] Attempting to authenticate for account linking...");
                
                Authenticate(
                    (authResponse) =>
                    {
                        Debug.Log("[iOS] Apple Game Center account linked successfully through authentication");
                        onSuccess?.Invoke(true);
                    },
                    (error) =>
                    {
                        Debug.LogError($"[iOS] Failed to link Apple Game Center account: {error.Message}");
                        
                        var linkError = new WortalError
                        {
                            Code = "LINK_ACCOUNT_FAILED",
                            Message = $"Failed to link Game Center account: {error.Message}",
                            Context = "iOSWortalAuthentication.LinkAccount"
                        };
                        
                        onError?.Invoke(linkError);
                    }
                );
            }
#else
            Debug.LogError("[iOS] Game Center account linking called on non-iOS platform");

            var error = new WortalError
            {
                Code = "PLATFORM_NOT_SUPPORTED",
                Message = "Game Center account linking is only supported on iOS platform",
                Context = "iOSWortalAuthentication.LinkAccount"
            };

            onError?.Invoke(error);
#endif
        }

        public AuthStatus GetAuthStatus()
        {
#if UNITY_IOS
            if (Social.localUser.authenticated)
            {
                Debug.Log("[iOS] Apple Game Center authentication status: SUCCESS");
                return AuthStatus.SUCCESS;
            }
            else
            {
                Debug.Log("[iOS] Apple Game Center authentication status: NOT_AUTHENTICATED");
                return AuthStatus.NOT_AUTHENTICATED;
            }
#else
            Debug.Log("[iOS] Game Center not supported on this platform");
            return AuthStatus.NOT_SUPPORTED;
#endif
        }

        /// <summary>
        /// Generates a token for Game Center authentication.
        /// This is a placeholder implementation - you may want to implement
        /// a more sophisticated token generation based on your backend requirements.
        /// </summary>
        /// <returns>Generated token string or null if not authenticated</returns>
        private string GenerateGameCenterToken()
        {
#if UNITY_IOS
            if (Social.localUser.authenticated && !string.IsNullOrEmpty(Social.localUser.id))
            {
                // Generate a simple token based on user ID and timestamp
                // In production, you might want to use a more secure token generation method
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                var tokenData = $"gc_{Social.localUser.id}_{timestamp}";
                
                // You could add additional hashing/encryption here if needed
                return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(tokenData));
            }
#endif
            return null;
        }

        /// <summary>
        /// Gets the current authenticated user information
        /// </summary>
        /// <returns>AuthResponse with current user data or null if not authenticated</returns>
        public AuthResponse? GetCurrentUser()
        {
#if UNITY_IOS
            if (Social.localUser.authenticated)
            {
                return new AuthResponse
                {
                    Status = AuthStatus.SUCCESS,
                    UserID = Social.localUser.id ?? string.Empty,
                    UserName = Social.localUser.userName ?? "Game Center User",
                    Token = GenerateGameCenterToken(),
                    Provider = "GameCenter"
                };
            }
#endif
            return null;
        }

        /// <summary>
        /// Signs out the current user from Game Center
        /// Note: Game Center doesn't have a direct sign-out method,
        /// but we can clear our local authentication state
        /// </summary>
        public void SignOut()
        {
#if UNITY_IOS
            Debug.Log("[iOS] Signing out from Game Center (clearing local state)");
            // Game Center doesn't provide a direct sign-out method
            // The user would need to sign out from Game Center in iOS Settings
            // We can only clear our local references
#else
            Debug.Log("[iOS] Sign out called on non-iOS platform");
#endif
        }
    }
}
