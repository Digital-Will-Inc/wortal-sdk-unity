using System;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class iOSWortalContext : IWortalContext
    {
        public bool IsSupported => false;

        public string GetID()
        {
            Debug.Log("[iOS Platform] IWortalContext.GetID() called - Not implemented");
            return null;
        }

        public ContextType GetType()
        {
            Debug.Log("[iOS Platform] IWortalContext.GetType() called - Not implemented");
            return ContextType.SOLO;
        }

        public void GetPlayersAsync(Action<ContextPlayer[]> onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[iOS Platform] IWortalContext.GetPlayersAsync() called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "GetPlayersAsync implementation"
            });
        }

        public void InviteAsync(InvitePayload payload, Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[iOS Platform] IWortalContext.InviteAsync({payload}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "InviteAsync implementation"
            });
        }

        public void ShareAsync(SharePayload payload, Action<bool> onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[iOS Platform] IWortalContext.ShareAsync({payload}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "ShareAsync implementation"
            });
        }

        public void ShareLinkAsync(LinkSharePayload payload, Action<bool> onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[iOS Platform] IWortalContext.ShareLinkAsync({payload}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "ShareLinkAsync implementation"
            });
        }

        public void UpdateAsync(UpdatePayload payload, Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[iOS Platform] IWortalContext.UpdateAsync({payload}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "UpdateAsync implementation"
            });
        }

        public void SwitchAsync(string contextID, Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[iOS Platform] IWortalContext.SwitchAsync({contextID}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "SwitchAsync implementation"
            });
        }

        public void ChooseAsync(ContextChoosePayload options, Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[iOS Platform] IWortalContext.ChooseAsync({options}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "ChooseAsync implementation"
            });
        }

        public void CreateAsync(string playerID, Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[iOS Platform] IWortalContext.CreateAsync({playerID}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "CreateAsync implementation"
            });
        }
    }
}