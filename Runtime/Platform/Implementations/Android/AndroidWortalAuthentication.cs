using System;
using UnityEngine;

#if UNITY_ANDROID && !UNITY_EDITOR
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

namespace DigitalWill.WortalSDK
{
    public class AndroidWortalAuthentication : IWortalAuthentication
    {
        public bool IsSupported =>
#if UNITY_ANDROID && !UNITY_EDITOR
            true;
#else
            false;
#endif

        public void Authenticate(Action<AuthResponse> onSuccess, Action<WortalError> onError)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            Debug.Log("[Android] Authenticating with Google Play Games...");

            if (!PlayGamesPlatform.Instance.IsAuthenticated())
            {
                PlayGamesPlatform.Activate();
            }

            PlayGamesPlatform.Instance.Authenticate((success) =>
            {
                if (success == SignInStatus.Success)
                {
                    Debug.Log("[Android] Google Play Games authentication successful");
                    
                    var authResponse = new AuthResponse
                    {
                        Status = AuthStatus.SUCCESS,
                        UserID = Social.localUser.id,
                        UserName = Social.localUser.userName,
                        Token = GetGooglePlayToken(),
                        Provider = "GooglePlayGames"
                    };
                    
                    onSuccess?.Invoke(authResponse);
                }
                else
                {
                    Debug.LogWarning($"[Android] Google Play Games authentication failed: {success}");
                    
                    var error = new WortalError
                    {
                        Code = "AUTHENTICATION_FAILED",
                        Message = $"Google Play Games authentication failed: {success}",
                        Context = "AndroidWortalAuthentication.Authenticate"
                    };
                    
                    onError?.Invoke(error);
                }
            });
#else
            Debug.Log("[Android] Google Play Games not available on this platform");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_SUPPORTED",
                Message = "Google Play Games not supported on this platform",
                Context = "AndroidWortalAuthentication.Authenticate"
            });
#endif
        }

        public void LinkAccount(Action<bool> onSuccess, Action<WortalError> onError)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            Debug.Log("[Android] Linking Google Play Games account...");

            if (PlayGamesPlatform.Instance.IsAuthenticated())
            {
                Debug.Log("[Android] Google Play Games account already linked");
                onSuccess?.Invoke(true);
            }
            else
            {
                Authenticate(
                    (authResponse) =>
                    {
                        Debug.Log("[Android] Google Play Games account linked successfully");
                        onSuccess?.Invoke(true);
                    },
                    (error) =>
                    {
                        Debug.LogError($"[Android] Failed to link Google Play Games account: {error.Message}");
                        onError?.Invoke(error);
                    }
                );
            }
#else
            onError?.Invoke(new WortalError
            {
                Code = "NOT_SUPPORTED",
                Message = "Google Play Games not supported on this platform",
                Context = "AndroidWortalAuthentication.LinkAccount"
            });
#endif
        }

        public AuthStatus GetAuthStatus()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (PlayGamesPlatform.Instance.IsAuthenticated())
            {
                Debug.Log("[Android] Google Play Games authentication status: SUCCESS");
                return AuthStatus.SUCCESS;
            }
            else
            {
                Debug.Log("[Android] Google Play Games authentication status: NOT_AUTHENTICATED");
                return AuthStatus.NOT_AUTHENTICATED;
            }
#else
            return AuthStatus.NOT_SUPPORTED;
#endif
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        private string GetGooglePlayToken()
        {
            if (PlayGamesPlatform.Instance.IsAuthenticated())
            {
                return $"gpg_token_{Social.localUser.id}_{DateTime.UtcNow.Ticks}";
            }
            return null;
        }
#endif
    }
}
