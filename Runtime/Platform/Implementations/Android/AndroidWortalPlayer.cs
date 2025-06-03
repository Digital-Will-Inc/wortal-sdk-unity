using System;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class AndroidWortalPlayer : IWortalPlayer
    {
        public bool IsSupported => false;

        public string GetID()
        {
            Debug.Log("[Android Platform] IWortalPlayer.GetID() called - Not implemented");
            return "Android_player_id";
        }

        public string GetName()
        {
            Debug.Log("[Android Platform] IWortalPlayer.GetName() called - Not implemented");
            return "Android Player";
        }

        public string GetPhoto()
        {
            Debug.Log("[Android Platform] IWortalPlayer.GetPhoto() called - Not implemented");
            return null;
        }

        public bool IsFirstPlay()
        {
            Debug.Log("[Android Platform] IWortalPlayer.IsFirstPlay() called - Not implemented");
            return false;
        }

        public void GetConnectedPlayersAsync(GetConnectedPlayersPayload payload, Action<IWortalPlayer[]> onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[Android Platform] IWortalPlayer.GetConnectedPlayersAsync({payload}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "GetConnectedPlayersAsync implementation"
            });
        }

        public void GetSignedPlayerInfoAsync(Action<string, string> onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[Android Platform] IWortalPlayer.GetSignedPlayerInfoAsync() called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "GetSignedPlayerInfoAsync implementation"
            });
        }

        public void CanSubscribeBotAsync(Action<bool> onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[Android Platform] IWortalPlayer.CanSubscribeBotAsync() called - Not implemented");
            onSuccess?.Invoke(false);
        }

        public void SubscribeBotAsync(Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[Android Platform] IWortalPlayer.SubscribeBotAsync() called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "SubscribeBotAsync implementation"
            });
        }

        public void GetDataAsync(string[] keys, Action<PlayerData> onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[Android Platform] IWortalPlayer.GetDataAsync({string.Join(", ", keys)}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "GetDataAsync implementation"
            });
        }

        public void SetDataAsync(PlayerData data, Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[Android Platform] IWortalPlayer.SetDataAsync({data}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "SetDataAsync implementation"
            });
        }

        public void FlushDataAsync(Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[Android Platform] IWortalPlayer.FlushDataAsync() called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "FlushDataAsync implementation"
            });
        }

        public void GetStatsAsync(string[] keys, Action<PlayerStats> onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[Android Platform] IWortalPlayer.GetStatsAsync({string.Join(", ", keys)}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "GetStatsAsync implementation"
            });
        }

        public void SetStatsAsync(PlayerStats stats, Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[Android Platform] IWortalPlayer.SetStatsAsync({stats}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "SetStatsAsync implementation"
            });
        }

        public void IncrementStatsAsync(PlayerStats increments, Action<PlayerStats> onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[Android Platform] IWortalPlayer.IncrementStatsAsync({increments}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "IncrementStatsAsync implementation"
            });
        }

        public void GetASIDAsync(Action<string> onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[Android Platform] IWortalPlayer.GetASIDAsync() called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "GetASIDAsync implementation"
            });
        }

        public void GetSignedASIDAsync(Action<string, string> onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[Android Platform] IWortalPlayer.GetSignedASIDAsync() called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on Android platform",
                Context = "GetSignedASIDAsync implementation"
            });
        }
    }
}