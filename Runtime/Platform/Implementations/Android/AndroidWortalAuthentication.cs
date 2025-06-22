using System;
using System.Reflection;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class AndroidWortalAuthentication : IWortalAuthentication
    {
        private static Type _playGamesPlatformType;
        private static object _playGamesInstance;
        private static Type _signInStatusType;

        static AndroidWortalAuthentication()
        {
            InitializeGooglePlayGamesReflection();
        }

        public bool IsSupported => _playGamesPlatformType != null;

        private static void InitializeGooglePlayGamesReflection()
        {
            try
            {
                _playGamesPlatformType = Type.GetType("GooglePlayGames.PlayGamesPlatform, GooglePlayGames");
                _signInStatusType = Type.GetType("GooglePlayGames.BasicApi.SignInStatus, GooglePlayGames.BasicApi");

                if (_playGamesPlatformType != null)
                {
                    var instanceProperty = _playGamesPlatformType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
                    _playGamesInstance = instanceProperty?.GetValue(null);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[Android] Google Play Games not available: {e.Message}");
            }
        }

        public void Authenticate(Action<AuthResponse> onSuccess, Action<WortalError> onError)
        {
            if (!IsSupported)
            {
                onError?.Invoke(new WortalError
                {
                    Code = "DEPENDENCY_MISSING",
                    Message = "Google Play Games SDK not found. Please import Google Play Games to use authentication.",
                    Context = "AndroidWortalAuthentication.Authenticate"
                });
                return;
            }

            try
            {
                // Check if already authenticated
                var isAuthenticatedMethod = _playGamesPlatformType.GetMethod("IsAuthenticated");
                var isAuthenticated = (bool)isAuthenticatedMethod.Invoke(_playGamesInstance, null);

                if (!isAuthenticated)
                {
                    // Activate platform
                    var activateMethod = _playGamesPlatformType.GetMethod("Activate", BindingFlags.Public | BindingFlags.Static);
                    activateMethod?.Invoke(null, null);
                }

                // Authenticate
                var authenticateMethod = _playGamesPlatformType.GetMethod("Authenticate", new[] { typeof(Action<>).MakeGenericType(_signInStatusType) });

                Action<object> authCallback = (signInStatus) =>
                {
                    // Get SUCCESS enum value
                    var successValue = Enum.Parse(_signInStatusType, "Success");

                    if (signInStatus.Equals(successValue))
                    {
                        var authResponse = new AuthResponse
                        {
                            Status = AuthStatus.SUCCESS,
                            UserID = Social.localUser.id,
                            UserName = Social.localUser.userName,
                            Token = GenerateGooglePlayToken(),
                            Provider = "GooglePlayGames"
                        };

                        onSuccess?.Invoke(authResponse);
                    }
                    else
                    {
                        onError?.Invoke(new WortalError
                        {
                            Code = "AUTHENTICATION_FAILED",
                            Message = $"Google Play Games authentication failed: {signInStatus}",
                            Context = "AndroidWortalAuthentication.Authenticate"
                        });
                    }
                };

                authenticateMethod.Invoke(_playGamesInstance, new object[] { authCallback });
            }
            catch (Exception e)
            {
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
            if (!IsSupported)
            {
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
                var isAuthenticatedMethod = _playGamesPlatformType.GetMethod("IsAuthenticated");
                var isAuthenticated = (bool)isAuthenticatedMethod.Invoke(_playGamesInstance, null);

                if (isAuthenticated)
                {
                    onSuccess?.Invoke(true);
                }
                else
                {
                    Authenticate(
                        (authResponse) => onSuccess?.Invoke(true),
                        (error) => onError?.Invoke(error)
                    );
                }
            }
            catch (Exception e)
            {
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
                return AuthStatus.NOT_SUPPORTED;

            try
            {
                var isAuthenticatedMethod = _playGamesPlatformType.GetMethod("IsAuthenticated");
                var isAuthenticated = (bool)isAuthenticatedMethod.Invoke(_playGamesInstance, null);
                return isAuthenticated ? AuthStatus.SUCCESS : AuthStatus.NOT_AUTHENTICATED;
            }
            catch
            {
                return AuthStatus.NOT_SUPPORTED;
            }
        }

        private string GenerateGooglePlayToken()
        {
            return $"gpg_token_{Social.localUser.id}_{DateTime.UtcNow.Ticks}";
        }
    }
}
