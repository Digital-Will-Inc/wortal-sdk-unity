using System;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class WebGLWortalLeaderboard : IWortalLeaderboard
    {
        public bool IsSupported => false;

        public void GetLeaderboardAsync(string name, Action<Leaderboard> onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[WebGL Platform] IWortalLeaderboard.GetLeaderboardAsync({name}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on WebGL platform",
                Context = "GetLeaderboardAsync implementation"
            });
        }

        public void SendScoreAsync(string name, int score, Action<LeaderboardEntry> onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[WebGL Platform] IWortalLeaderboard.SendScoreAsync({name}, {score}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on WebGL platform",
                Context = "SendScoreAsync implementation"
            });
        }

        public void SendScoreAsync(string name, int score, string details, Action<LeaderboardEntry> onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[WebGL Platform] IWortalLeaderboard.SendScoreAsync({name}, {score}, {details}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on WebGL platform",
                Context = "SendScoreAsync implementation"
            });
        }

        public void GetEntriesAsync(string name, int count, int offset, Action<LeaderboardEntry[]> onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[WebGL Platform] IWortalLeaderboard.GetEntriesAsync({name}, {count}, {offset}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on WebGL platform",
                Context = "GetEntriesAsync implementation"
            });
        }

        public void GetPlayerEntryAsync(string name, Action<LeaderboardEntry> onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[WebGL Platform] IWortalLeaderboard.GetPlayerEntryAsync({name}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on WebGL platform",
                Context = "GetPlayerEntryAsync implementation"
            });
        }

        public void GetEntryCountAsync(string name, int count, Action<LeaderboardEntry[]> onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[WebGL Platform] IWortalLeaderboard.GetEntryCountAsync({name}, {count}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on WebGL platform",
                Context = "GetEntryCountAsync implementation"
            });
        }

        public void GetConnectedPlayerEntriesAsync(string name, int count, int offset, Action<LeaderboardEntry[]> onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[WebGL Platform] IWortalLeaderboard.GetConnectedPlayerEntriesAsync({name}, {count}, {offset}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on WebGL platform",
                Context = "GetConnectedPlayerEntriesAsync implementation"
            });
        }
    }
}