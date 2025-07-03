using System;
using System.Reflection;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class AndroidWortalAuthentication : IWortalAuthentication
    {
        private static Type _playGamesPlatformType;
        private static Type _signInStatusType;

        static AndroidWortalAuthentication()
        {
            InitializeGooglePlayGamesReflection();
        }

        public bool IsSupported => _playGamesPlatformType != null;

        private static object GetPlayGamesInstance()
        {
            if (_playGamesPlatformType == null)
            {
                Debug.LogWarning("[Android] PlayGamesPlatformType is null, cannot get instance");
                return null;
            }

            try
            {
                var instanceProperty = _playGamesPlatformType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
                if (instanceProperty == null)
                {
                    Debug.LogWarning("[Android] Instance property not found on PlayGamesPlatform");
                    return null;
                }

                var instance = instanceProperty.GetValue(null);
                Debug.Log($"[Android] PlayGames instance retrieved: {instance != null}");
                return instance;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Android] Error getting PlayGames instance: {e.Message}");
                return null;
            }
        }

        private static void InitializeGooglePlayGamesReflection()
        {
            try
            {
                Debug.Log("[Android] Starting Google Play Games reflection initialization...");

                _playGamesPlatformType = Type.GetType("GooglePlayGames.PlayGamesPlatform, GooglePlayGames");
                _signInStatusType = Type.GetType("GooglePlayGames.BasicApi.SignInStatus, GooglePlayGames.BasicApi");

                Debug.Log($"[Android] PlayGamesPlatformType found: {_playGamesPlatformType != null}");
                Debug.Log($"[Android] SignInStatusType found: {_signInStatusType != null}");

                if (_playGamesPlatformType != null)
                {
                    Debug.Log("[Android] Google Play Games SDK successfully initialized");
                }
                else
                {
                    Debug.LogWarning("[Android] Google Play Games SDK not found - authentication will not be supported");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[Android] Google Play Games initialization failed: {e.Message}");
                Debug.LogError($"[Android] Stack trace: {e.StackTrace}");
            }
        }

        public bool IsAlreadyAuthenticated()
        {
            if (!IsSupported)
            {
                Debug.LogWarning("[Android] Google Play Games not supported, returning false for IsAlreadyAuthenticated");
                return false;
            }

            try
            {
                var playGamesInstance = GetPlayGamesInstance();
                if (playGamesInstance == null)
                {
                    Debug.LogWarning("[Android] PlayGames instance is null, cannot check authentication status");
                    return false;
                }

                var isAuthenticatedMethod = _playGamesPlatformType.GetMethod("IsAuthenticated");
                if (isAuthenticatedMethod == null)
                {
                    Debug.LogError("[Android] IsAuthenticated method not found");
                    return false;
                }

                var result = (bool)isAuthenticatedMethod.Invoke(playGamesInstance, null);
                Debug.Log($"[Android] IsAlreadyAuthenticated result: {result}");
                return result;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Android] Error checking authentication status: {e.Message}");
                return false;
            }
        }

        public void Authenticate(Action<AuthResponse> onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[Android] Starting authentication process...");

            if (!IsSupported)
            {
                Debug.LogError("[Android] Google Play Games SDK not supported");
                onError?.Invoke(new WortalError
                {
                    Code = "DEPENDENCY_MISSING",
                    Message = "Google Play Games SDK not found. Please import Google Play Games to use authentication.",
                    Context = "AndroidWortalAuthentication.Authenticate"
                });
                return;
            }

            var playGamesInstance = GetPlayGamesInstance();
            if (playGamesInstance == null)
            {
                Debug.LogError("[Android] PlayGames instance is null");
                onError?.Invoke(new WortalError
                {
                    Code = "PLATFORM_NOT_READY",
                    Message = "Google Play Games instance not found. Please ensure Google Play Games is initialized.",
                    Context = "AndroidWortalAuthentication.Authenticate"
                });
                return;
            }

            // Check if already authenticated from silent login
            if (IsAlreadyAuthenticated())
            {
                Debug.Log("[Android] Using existing Google Play Games authentication");
                var authResponse = new AuthResponse
                {
                    Status = AuthStatus.SUCCESS,
                    UserID = Social.localUser.id,
                    UserName = Social.localUser.userName,
                    Token = GenerateGooglePlayToken(),
                    Provider = "GooglePlayGames"
                };
                onSuccess?.Invoke(authResponse);
                return;
            }

            try
            {
                Debug.Log("[Android] Starting new authentication flow...");

                // Check if already authenticated (double-check)
                var isAuthenticatedMethod = _playGamesPlatformType.GetMethod("IsAuthenticated");
                if (isAuthenticatedMethod == null)
                {
                    throw new Exception("IsAuthenticated method not found");
                }

                var isAuthenticated = (bool)isAuthenticatedMethod.Invoke(playGamesInstance, null);
                Debug.Log($"[Android] Current authentication status: {isAuthenticated}");

                if (!isAuthenticated)
                {
                    Debug.Log("[Android] Activating Google Play Games platform...");
                    // Activate platform
                    var activateMethod = _playGamesPlatformType.GetMethod("Activate", BindingFlags.Public | BindingFlags.Static);
                    if (activateMethod != null)
                    {
                        activateMethod.Invoke(null, null);
                        Debug.Log("[Android] Platform activated");
                    }
                    else
                    {
                        Debug.LogWarning("[Android] Activate method not found, continuing without activation");
                    }
                }

                // Authenticate
                Debug.Log("[Android] Calling authentication method...");
                var authenticateMethod = _playGamesPlatformType.GetMethod("Authenticate", new[] { typeof(Action<>).MakeGenericType(_signInStatusType) });

                if (authenticateMethod == null)
                {
                    throw new Exception("Authenticate method not found");
                }

                Action<object> authCallback = (signInStatus) =>
                {
                    Debug.Log($"[Android] Authentication callback received: {signInStatus}");

                    try
                    {
                        // Get SUCCESS enum value
                        var successValue = Enum.Parse(_signInStatusType, "Success");
                        Debug.Log($"[Android] Success enum value: {successValue}");

                        if (signInStatus.Equals(successValue))
                        {
                            Debug.Log("[Android] Authentication successful!");
                            var authResponse = new AuthResponse
                            {
                                Status = AuthStatus.SUCCESS,
                                UserID = Social.localUser.id,
                                UserName = Social.localUser.userName,
                                Token = GenerateGooglePlayToken(),
                                Provider = "GooglePlayGames"
                            };

                            Debug.Log($"[Android] Auth response - UserID: {authResponse.UserID}, UserName: {authResponse.UserName}");
                            onSuccess?.Invoke(authResponse);
                        }
                        else
                        {
                            Debug.LogError($"[Android] Authentication failed with status: {signInStatus}");
                            onError?.Invoke(new WortalError
                            {
                                Code = "AUTHENTICATION_FAILED",
                                Message = $"Google Play Games authentication failed: {signInStatus}",
                                Context = "AndroidWortalAuthentication.Authenticate"
                            });
                        }
                    }
                    catch (Exception callbackException)
                    {
                        Debug.LogError($"[Android] Error in authentication callback: {callbackException.Message}");
                        onError?.Invoke(new WortalError
                        {
                            Code = "AUTHENTICATION_CALLBACK_ERROR",
                            Message = $"Authentication callback failed: {callbackException.Message}",
                            Context = "AndroidWortalAuthentication.Authenticate"
                        });
                    }
                };

                authenticateMethod.Invoke(playGamesInstance, new object[] { authCallback });
                Debug.Log("[Android] Authentication method invoked, waiting for callback...");
            }
            catch (Exception e)
            {
                Debug.LogError($"[Android] Authentication error: {e.Message}");
                Debug.LogError($"[Android] Stack trace: {e.StackTrace}");
                onError?.Invoke(new WortalError
                {
                    Code = "AUTHENTICATION_ERROR",
                    Message = $"Authentication failed: {e.Message}",
                    Context = "AndroidWortalAuthentication.Authenticate"
                });
            }
        }

        public void LinkAccount(Action<bool> onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[Android] Starting link account process...");

            if (!IsSupported)
            {
                Debug.LogError("[Android] Google Play Games SDK not supported for link account");
                onError?.Invoke(new WortalError
                {
                    Code = "DEPENDENCY_MISSING",
                    Message = "Google Play Games SDK not found",
                    Context = "AndroidWortalAuthentication.LinkAccount"
                });
                return;
            }

            try
            {
                var playGamesInstance = GetPlayGamesInstance();
                if (playGamesInstance == null)
                {
                    Debug.LogError("[Android] PlayGames instance is null for link account");
                    onError?.Invoke(new WortalError
                    {
                        Code = "PLATFORM_NOT_READY",
                        Message = "Google Play Games instance not available",
                        Context = "AndroidWortalAuthentication.LinkAccount"
                    });
                    return;
                }

                var isAuthenticatedMethod = _playGamesPlatformType.GetMethod("IsAuthenticated");
                if (isAuthenticatedMethod == null)
                {
                    throw new Exception("IsAuthenticated method not found");
                }

                var isAuthenticated = (bool)isAuthenticatedMethod.Invoke(playGamesInstance, null);
                Debug.Log($"[Android] Link account - current auth status: {isAuthenticated}");

                if (isAuthenticated)
                {
                    Debug.Log("[Android] Account already linked");
                    onSuccess?.Invoke(true);
                }
                else
                {
                    Debug.Log("[Android] Account not authenticated, starting authentication for linking...");
                    Authenticate(
                        (authResponse) =>
                        {
                            Debug.Log("[Android] Account linked successfully");
                            onSuccess?.Invoke(true);
                        },
                        (error) =>
                        {
                            Debug.LogError($"[Android] Account linking failed: {error.Message}");
                            onError?.Invoke(error);
                        }
                    );
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[Android] Link account error: {e.Message}");
                onError?.Invoke(new WortalError
                {
                    Code = "LINK_ACCOUNT_ERROR",
                    Message = e.Message,
                    Context = "AndroidWortalAuthentication.LinkAccount"
                });
            }
        }

        public AuthStatus GetAuthStatus()
        {
            if (!IsSupported)
            {
                Debug.LogWarning("[Android] Google Play Games not supported, returning NOT_SUPPORTED");
                return AuthStatus.NOT_SUPPORTED;
            }

            try
            {
                var playGamesInstance = GetPlayGamesInstance();
                if (playGamesInstance == null)
                {
                    Debug.LogWarning("[Android] PlayGames instance is null, returning NOT_SUPPORTED");
                    return AuthStatus.NOT_SUPPORTED;
                }

                var isAuthenticatedMethod = _playGamesPlatformType.GetMethod("IsAuthenticated");
                if (isAuthenticatedMethod == null)
                {
                    Debug.LogError("[Android] IsAuthenticated method not found");
                    return AuthStatus.NOT_SUPPORTED;
                }

                var isAuthenticated = (bool)isAuthenticatedMethod.Invoke(playGamesInstance, null);
                var status = isAuthenticated ? AuthStatus.SUCCESS : AuthStatus.NOT_AUTHENTICATED;
                Debug.Log($"[Android] GetAuthStatus result: {status}");
                return status;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Android] Error getting auth status: {e.Message}");
                return AuthStatus.NOT_SUPPORTED;
            }
        }

        private string GenerateGooglePlayToken()
        {
            var token = $"gpg_token_{Social.localUser.id}_{DateTime.UtcNow.Ticks}";
            Debug.Log($"[Android] Generated token: {token}");
            return token;
        }
    }
}
