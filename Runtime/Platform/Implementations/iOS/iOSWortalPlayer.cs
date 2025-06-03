using System;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class iOSWortalPlayer : IWortalPlayer
    {
        public bool IsSupported => false;

        public string GetID()
        {
            Debug.Log("[iOS Platform] IWortalPlayer.GetID() called - Not implemented");
            return "ios_player_id";
        }

        public string GetName()
        {
            Debug.Log("[iOS Platform] IWortalPlayer.GetName() called - Not implemented");
            return "iOS Player";
        }

        public string GetPhoto()
        {
            Debug.Log("[iOS Platform] IWortalPlayer.GetPhoto() called - Not implemented");
            return null;
        }

        public bool IsFirstPlay()
        {
            Debug.Log("[iOS Platform] IWortalPlayer.IsFirstPlay() called - Not implemented");
            return false;
        }

        public void GetConnectedPlayersAsync(GetConnectedPlayersPayload payload, Action<IWortalPlayer[]> onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[iOS Platform] IWortalPlayer.GetConnectedPlayersAsync({payload}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "GetConnectedPlayersAsync implementation"
            });
        }

        public void GetSignedPlayerInfoAsync(Action<string, string> onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[iOS Platform] IWortalPlayer.GetSignedPlayerInfoAsync() called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "GetSignedPlayerInfoAsync implementation"
            });
        }

        public void CanSubscribeBotAsync(Action<bool> onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[iOS Platform] IWortalPlayer.CanSubscribeBotAsync() called - Not implemented");
            onSuccess?.Invoke(false);
        }

        public void SubscribeBotAsync(Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[iOS Platform] IWortalPlayer.SubscribeBotAsync() called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "SubscribeBotAsync implementation"
            });
        }

        public void GetDataAsync(string[] keys, Action<PlayerData> onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[iOS Platform] IWortalPlayer.GetDataAsync({string.Join(", ", keys)}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "GetDataAsync implementation"
            });
        }

        public void SetDataAsync(PlayerData data, Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[iOS Platform] IWortalPlayer.SetDataAsync({data}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "SetDataAsync implementation"
            });
        }

        public void FlushDataAsync(Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[iOS Platform] IWortalPlayer.FlushDataAsync() called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "FlushDataAsync implementation"
            });
        }

        public void GetStatsAsync(string[] keys, Action<PlayerStats> onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[iOS Platform] IWortalPlayer.GetStatsAsync({string.Join(", ", keys)}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "GetStatsAsync implementation"
            });
        }

        public void SetStatsAsync(PlayerStats stats, Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[iOS Platform] IWortalPlayer.SetStatsAsync({stats}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "SetStatsAsync implementation"
            });
        }

        public void IncrementStatsAsync(PlayerStats increments, Action<PlayerStats> onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[iOS Platform] IWortalPlayer.IncrementStatsAsync({increments}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "IncrementStatsAsync implementation"
            });
        }

        public void GetASIDAsync(Action<string> onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[iOS Platform] IWortalPlayer.GetASIDAsync() called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "GetASIDAsync implementation"
            });
        }

        public void GetSignedASIDAsync(Action<string, string> onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[iOS Platform] IWortalPlayer.GetSignedASIDAsync() called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "GetSignedASIDAsync implementation"
            });
        }
    }
}