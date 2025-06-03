using System;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class WebGLWortalSession : IWortalSession
    {
        public bool IsSupported => true;
        public bool IsAudioEnabled => true;

        public string GetGameID()
        {
            Debug.Log("[WebGL Platform] IWortalSession.GetGameID() called - Not implemented");
            return "WebGL_game_id";
        }

        public Platform GetPlatform()
        {
            Debug.Log("[WebGL Platform] IWortalSession.GetPlatform() called");
            return Platform.WebGL;
        }

        public Device GetDevice()
        {
            Debug.Log("[WebGL Platform] IWortalSession.GetDevice() called");
            return Device.DESKTOP;
        }

        public Orientation GetOrientation()
        {
            Debug.Log("[WebGL Platform] IWortalSession.GetOrientation() called");
            return Screen.width > Screen.height ? Orientation.LANDSCAPE : Orientation.PORTRAIT;
        }

        public string GetLocale()
        {
            Debug.Log("[WebGL Platform] IWortalSession.GetLocale() called");
            return Application.systemLanguage.ToString();
        }

        public TrafficSource GetTrafficSource()
        {
            Debug.Log("[WebGL Platform] IWortalSession.GetTrafficSource() called - Not implemented");
            return default(TrafficSource); // or just: return default;
        }

        public void GetEntryPointAsync(Action<string> onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[WebGL Platform] IWortalSession.GetEntryPointAsync() called - Not implemented");
            onSuccess?.Invoke("menu");
        }

        public object GetEntryPointData()
        {
            Debug.Log("[WebGL Platform] IWortalSession.GetEntryPointData() called - Not implemented");
            return null;
        }

        public void StartGame(Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log("[WebGL Platform] IWortalSession.StartGame() called");
            onSuccess?.Invoke();
        }

        public void SetLoadingProgress(int progress)
        {
            Debug.Log($"[WebGL Platform] IWortalSession.SetLoadingProgress({progress}) called");
        }

        public void SetSessionData(object data)
        {
            Debug.Log($"[WebGL Platform] IWortalSession.SetSessionData({data}) called");
        }

        public object GetSessionData()
        {
            Debug.Log("[WebGL Platform] IWortalSession.GetSessionData() called - Not implemented");
            return null;
        }

        public void GameReady()
        {
            Debug.Log("[WebGL Platform] IWortalSession.GameReady() called");
        }

        public void GameplayStart()
        {
            Debug.Log("[WebGL Platform] IWortalSession.GameplayStart() called");
        }

        public void GameplayStop()
        {
            Debug.Log("[WebGL Platform] IWortalSession.GameplayStop() called");
        }

        public void HappyTime()
        {
            Debug.Log("[WebGL Platform] IWortalSession.HappyTime() called");
        }

        public void OnAudioStatusChange(Action<bool> onAudioEnabled)
        {
            Debug.Log("[WebGL Platform] IWortalSession.OnAudWebGLtatusChange() called - Not implemented");
        }
    }
}