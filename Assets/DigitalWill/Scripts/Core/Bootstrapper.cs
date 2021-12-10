using DigitalWill.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DigitalWill.Core
{
    /// <summary>
    /// Boot component that handles loading in the first scene after the ServiceBootstrapper signals its LoadingDone
    /// event. Can be modified to check the IsLoadingDone property and load from that instead. Handles showing the iOS
    /// tracking consent dialog if applicable.
    /// </summary>
    public class Bootstrapper : MonoBehaviour
    {
        private void Start()
        {
            ServiceBootstrapper.LoadingDone += OnLoadingDone;
        }

        private void OnDestroy()
        {
            ServiceBootstrapper.LoadingDone -= OnLoadingDone;
        }

        private void OnLoadingDone()
        {
            // Load the scene at buildIndex 1. You can change this to fit your game's needs.
            SceneManager.LoadScene(1);
        }
    }
}
