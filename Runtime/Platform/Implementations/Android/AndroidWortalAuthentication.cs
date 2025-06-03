using System;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class AndroidWortalAuthentication : IWortalAuthentication
    {
        public bool IsSupported => false;

        public void Authenticate(Action<AuthResponse> onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[Android Platform] IWortalAuthentication.Authenticate() called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "Authenticate implementation"
            });
        }

        public void LinkAccount(Action<bool> onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[Android Platform] IWortalAuthentication.LinkAccount() called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "LinkAccount implementation"
            });
        }

        public AuthStatus GetAuthStatus()
        {
            Debug.Log("[Android Platform] IWortalAuthentication.GetAuthStatus() called - Not implemented");
            return AuthStatus.CANCEL;
        }
    }
}