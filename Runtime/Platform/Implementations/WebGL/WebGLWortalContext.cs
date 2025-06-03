using System;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class WebGLWortalContext : IWortalContext
    {
        public bool IsSupported => false;

        public string GetID()
        {
            Debug.Log("[WebGL Platform] IWortalContext.GetID() called - Not implemented");
            return null;
        }

        public ContextType GetType()
        {
            Debug.Log("[WebGL Platform] IWortalContext.GetType() called - Not implemented");
            return ContextType.SOLO;
        }

        public void GetPlayersAsync(Action<ContextPlayer[]> onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[WebGL Platform] IWortalContext.GetPlayersAsync() called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on WebGL platform",
                Context = "GetPlayersAsync implementation"
            });
        }

        public void InviteAsync(InvitePayload payload, Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[WebGL Platform] IWortalContext.InviteAsync({payload}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on WebGL platform",
                Context = "InviteAsync implementation"
            });
        }

        public void ShareAsync(SharePayload payload, Action<bool> onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[WebGL Platform] IWortalContext.ShareAsync({payload}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on WebGL platform",
                Context = "ShareAsync implementation"
            });
        }

        public void ShareLinkAsync(LinkSharePayload payload, Action<bool> onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[WebGL Platform] IWortalContext.ShareLinkAsync({payload}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on WebGL platform",
                Context = "ShareLinkAsync implementation"
            });
        }

        public void UpdateAsync(UpdatePayload payload, Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[WebGL Platform] IWortalContext.UpdateAsync({payload}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on WebGL platform",
                Context = "UpdateAsync implementation"
            });
        }

        public void SwitchAsync(string contextID, Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[WebGL Platform] IWortalContext.SwitchAsync({contextID}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on WebGL platform",
                Context = "SwitchAsync implementation"
            });
        }

        public void ChooseAsync(ContextChoosePayload options, Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[WebGL Platform] IWortalContext.ChooseAsync({options}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on WebGL platform",
                Context = "ChooseAsync implementation"
            });
        }

        public void CreateAsync(string playerID, Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[WebGL Platform] IWortalContext.CreateAsync({playerID}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on WebGL platform",
                Context = "CreateAsync implementation"
            });
        }
    }
}