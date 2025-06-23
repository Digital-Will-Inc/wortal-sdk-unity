using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace DigitalWill.WortalSDK
{
    public class iOSWortalAuthentication : IWortalAuthentication
    {
        public bool IsSupported => Application.platform == RuntimePlatform.IPhonePlayer;

        public void Authenticate(Action<AuthResponse> onSuccess, Action<WortalError> onError)
        {
#if UNITY_IOS && !UNITY_EDITOR
            Debug.Log("[iOS] Authenticating with Apple Game Center...");

            // Check if already authenticated from silent login
            if (Social.localUser.authenticated)
            {
                Debug.Log("[iOS] Using existing Game Center authentication");
                var existingAuth = CreateAuthResponse();
                onSuccess?.Invoke(existingAuth);
                return;
            }

            // Authenticate with Game Center
            Social.localUser.Authenticate((success) =>
            {
                if (success)
                {
                    Debug.Log("[iOS] Apple Game Center authentication successful");
                    Debug.Log($"[iOS] User ID: {Social.localUser.id}");
                    Debug.Log($"[iOS] User Name: {Social.localUser.userName}");

                    var authResponse = CreateAuthResponse();
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
#if UNITY_IOS && !UNITY_EDITOR
            Debug.Log("[iOS] Linking Apple Game Center account...");

            if (Social.localUser.authenticated)
            {
                Debug.Log("[iOS] Apple Game Center account already linked");
                onSuccess?.Invoke(true);
            }
            else
            {
                Debug.Log("[iOS] Attempting to authenticate for account linking...");
                
                Authenticate(
                    (authResponse) =>
                    {
                        Debug.Log("[iOS] Apple Game Center account linked successfully");
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
#if UNITY_IOS && !UNITY_EDITOR
            if (Social.localUser.authenticated)
            {
                return AuthStatus.SUCCESS;
            }
            else
            {
                return AuthStatus.NOT_AUTHENTICATED;
            }
#else
            return AuthStatus.NOT_SUPPORTED;
#endif
        }

        /// <summary>
        /// Check if user is already authenticated (useful for silent login check)
        /// </summary>
        public bool IsAlreadyAuthenticated()
        {
#if UNITY_IOS && !UNITY_EDITOR
            return Social.localUser.authenticated;
#else
            return false;
#endif
        }

        /// <summary>
        /// Creates a standardized auth response
        /// </summary>
        private AuthResponse CreateAuthResponse()
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

        /// <summary>
        /// Generates a token for Game Center authentication
        /// </summary>
        private string GenerateGameCenterToken()
        {
#if UNITY_IOS && !UNITY_EDITOR
            if (Social.localUser.authenticated && !string.IsNullOrEmpty(Social.localUser.id))
            {
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                var tokenData = $"gc_{Social.localUser.id}_{timestamp}";
                return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(tokenData));
            }
#endif
            return null;
        }

        /// <summary>
        /// Gets the current authenticated user information
        /// </summary>
        public AuthResponse? GetCurrentUser()
        {
#if UNITY_IOS && !UNITY_EDITOR
            if (Social.localUser.authenticated)
            {
                return CreateAuthResponse();
            }
#endif
            return null;
        }

        /// <summary>
        /// Signs out from Game Center (clears local state)
        /// </summary>
        public void SignOut()
        {
#if UNITY_IOS && !UNITY_EDITOR
            Debug.Log("[iOS] Clearing Game Center local authentication state");
            // Game Center doesn't provide direct sign-out
            // User must sign out from iOS Settings > Game Center
#else
            Debug.Log("[iOS] Sign out called on non-iOS platform");
#endif
        }
    }
}
