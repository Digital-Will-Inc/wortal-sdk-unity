using System;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class AndroidWortalSession : IWortalSession
    {
        public bool IsSupported => true;
        public bool IsAudioEnabled => true;

        public string GetGameID()
        {
            Debug.Log("[Android Platform] IWortalSession.GetGameID() called - Not implemented");
            return "Android_game_id";
        }

        public Platform GetPlatform()
        {
            Debug.Log("[Android Platform] IWortalSession.GetPlatform() called");
            return Platform.Android;
        }

        public Device GetDevice()
        {
            Debug.Log("[Android Platform] IWortalSession.GetDevice() called");
            return Device.ANDROID;
        }

        public Orientation GetOrientation()
        {
            Debug.Log("[Android Platform] IWortalSession.GetOrientation() called");
            return Screen.width > Screen.height ? Orientation.LANDSCAPE : Orientation.PORTRAIT;
        }

        public string GetLocale()
        {
            Debug.Log("[Android Platform] IWortalSession.GetLocale() called");
            return Application.systemLanguage.ToString();
        }

        public TrafficSource GetTrafficSource()
        {
            Debug.Log("[Android Platform] IWortalSession.GetTrafficSource() called - Not implemented");
            return default(TrafficSource); // or just: return default;
        }

        public void GetEntryPointAsync(Action<string> onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[Android Platform] IWortalSession.GetEntryPointAsync() called - Not implemented");
            onSuccess?.Invoke("menu");
        }

        public object GetEntryPointData()
        {
            Debug.Log("[Android Platform] IWortalSession.GetEntryPointData() called - Not implemented");
            return null;
        }

        public void StartGame(Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[Android Platform] IWortalSession.StartGame() called");
            onSuccess?.Invoke();
        }

        public void SetLoadingProgress(int progress)
        {
            Debug.Log($"[Android Platform] IWortalSession.SetLoadingProgress({progress}) called");
        }

        public void SetSessionData(object data)
        {
            Debug.Log($"[Android Platform] IWortalSession.SetSessionData({data}) called");
        }

        public object GetSessionData()
        {
            Debug.Log("[Android Platform] IWortalSession.GetSessionData() called - Not implemented");
            return null;
        }

        public void GameReady()
        {
            Debug.Log("[Android Platform] IWortalSession.GameReady() called");
        }

        public void GameplayStart()
        {
            Debug.Log("[Android Platform] IWortalSession.GameplayStart() called");
        }

        public void GameplayStop()
        {
            Debug.Log("[Android Platform] IWortalSession.GameplayStop() called");
        }

        public void HappyTime()
        {
            Debug.Log("[Android Platform] IWortalSession.HappyTime() called");
        }

        public void OnAudioStatusChange(Action<bool> onAudioEnabled)
        {
            Debug.Log("[Android Platform] IWortalSession.OnAudAndroidtatusChange() called - Not implemented");
        }
    }
}