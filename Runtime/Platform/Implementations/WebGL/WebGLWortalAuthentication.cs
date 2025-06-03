using System;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class WebGLWortalAuthentication : IWortalAuthentication
    {
        public bool IsSupported => false;

        public void Authenticate(Action<AuthResponse> onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[WebGL Platform] IWortalAuthentication.Authenticate() called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on WebGL platform",
                Context = "Authenticate implementation"
            });
        }

        public void LinkAccount(Action<bool> onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[WebGL Platform] IWortalAuthentication.LinkAccount() called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on WebGL platform",
                Context = "LinkAccount implementation"
            });
        }

        public AuthStatus GetAuthStatus()
        {
            Debug.Log("[WebGL Platform] IWortalAuthentication.GetAuthStatus() called - Not implemented");
            return AuthStatus.CANCEL;
        }
    }
}