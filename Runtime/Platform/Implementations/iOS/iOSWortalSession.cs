using System;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class iOSWortalSession : IWortalSession
    {
        public bool IsSupported => true;
        public bool IsAudioEnabled => true;

        public string GetGameID()
        {
            Debug.Log("[iOS Platform] IWortalSession.GetGameID() called - Not implemented");
            return "ios_game_id";
        }

        public Platform GetPlatform()
        {
            Debug.Log("[iOS Platform] IWortalSession.GetPlatform() called");
            return Platform.iOS;
        }

        public Device GetDevice()
        {
            Debug.Log("[iOS Platform] IWortalSession.GetDevice() called");
            return Device.IOS;
        }

        public Orientation GetOrientation()
        {
            Debug.Log("[iOS Platform] IWortalSession.GetOrientation() called");
            return Screen.width > Screen.height ? Orientation.LANDSCAPE : Orientation.PORTRAIT;
        }

        public string GetLocale()
        {
            Debug.Log("[iOS Platform] IWortalSession.GetLocale() called");
            return Application.systemLanguage.ToString();
        }

        public TrafficSource GetTrafficSource()
        {
            Debug.Log("[iOS Platform] IWortalSession.GetTrafficSource() called - Not implemented");
            return default(TrafficSource); // or just: return default;
        }

        public void GetEntryPointAsync(Action<string> onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[iOS Platform] IWortalSession.GetEntryPointAsync() called - Not implemented");
            onSuccess?.Invoke("menu");
        }

        public object GetEntryPointData()
        {
            Debug.Log("[iOS Platform] IWortalSession.GetEntryPointData() called - Not implemented");
            return null;
        }

        public void StartGame(Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[iOS Platform] IWortalSession.StartGame() called");
            onSuccess?.Invoke();
        }

        public void SetLoadingProgress(int progress)
        {
            Debug.Log($"[iOS Platform] IWortalSession.SetLoadingProgress({progress}) called");
        }

        public void SetSessionData(object data)
        {
            Debug.Log($"[iOS Platform] IWortalSession.SetSessionData({data}) called");
        }

        public object GetSessionData()
        {
            Debug.Log("[iOS Platform] IWortalSession.GetSessionData() called - Not implemented");
            return null;
        }

        public void GameReady()
        {
            Debug.Log("[iOS Platform] IWortalSession.GameReady() called");
        }

        public void GameplayStart()
        {
            Debug.Log("[iOS Platform] IWortalSession.GameplayStart() called");
        }

        public void GameplayStop()
        {
            Debug.Log("[iOS Platform] IWortalSession.GameplayStop() called");
        }

        public void HappyTime()
        {
            Debug.Log("[iOS Platform] IWortalSession.HappyTime() called");
        }

        public void OnAudioStatusChange(Action<bool> onAudioEnabled)
        {
            Debug.Log("[iOS Platform] IWortalSession.OnAudioStatusChange() called - Not implemented");
        }
    }
}