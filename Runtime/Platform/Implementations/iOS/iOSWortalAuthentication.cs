using System;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class iOSWortalAuthentication : IWortalAuthentication
    {
        public bool IsSupported => false;

        public void Authenticate(Action<AuthResponse> onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[iOS Platform] IWortalAuthentication.Authenticate() called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "Authenticate implementation"
            });
        }

        public void LinkAccount(Action<bool> onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[iOS Platform] IWortalAuthentication.LinkAccount() called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "LinkAccount implementation"
            });
        }

        public AuthStatus GetAuthStatus()
        {
            Debug.Log("[iOS Platform] IWortalAuthentication.GetAuthStatus() called - Not implemented");
            return AuthStatus.CANCEL;
        }
    }
}