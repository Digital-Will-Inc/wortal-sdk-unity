using System;
using DigitalWill.WortalSDK;

namespace DigitalWill.WortalSDK.Core
{
    /// <summary>
    /// Interface for session functionality across platforms
    /// </summary>
    public interface IWortalSession
    {
        /// <summary>
        /// Gets the current game ID
        /// </summary>
        /// <returns>Game ID</returns>
        string GetGameID();

        /// <summary>
        /// Gets the current platform
        /// </summary>
        /// <returns>Platform type</returns>
        Platform GetPlatform();

        /// <summary>
        /// Gets the device information
        /// </summary>
        /// <returns>Device info</returns>
        Device GetDevice();

        /// <summary>
        /// Gets the orientation of the game
        /// </summary>
        /// <returns>Game orientation</returns>
        Orientation GetOrientation();

        /// <summary>
        /// Gets the locale of the current session
        /// </summary>
        /// <returns>Locale string</returns>
        string GetLocale();

        /// <summary>
        /// Gets the traffic source of the session
        /// </summary>
        /// <returns>Traffic source</returns>
        TrafficSource GetTrafficSource();

        /// <summary>
        /// Gets the entry point of the session
        /// </summary>
        /// <param name="onSuccess">Callback with entry point</param>
        /// <param name="onError">Callback for retrieval errors</param>
        void GetEntryPointAsync(Action<string> onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Gets the entry point data
        /// </summary>
        /// <returns>Entry point data</returns>
        object GetEntryPointData();

        /// <summary>
        /// Starts the game (primarily for WebGL)
        /// </summary>
        /// <param name="onSuccess">Callback for successful game start</param>
        /// <param name="onError">Callback for game start errors</param>
        void StartGame(Action onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Sets the loading progress
        /// </summary>
        /// <param name="progress">Loading progress (0-100)</param>
        void SetLoadingProgress(int progress);

        /// <summary>
        /// Sets the session data
        /// </summary>
        /// <param name="data">Session data to set</param>
        void SetSessionData(object data);

        /// <summary>
        /// Gets the session data
        /// </summary>
        /// <returns>Session data</returns>
        object GetSessionData();

        /// <summary>
        /// Called when the game is ready
        /// </summary>
        void GameReady();

        /// <summary>
        /// Called when gameplay starts
        /// </summary>
        void GameplayStart();

        /// <summary>
        /// Called when gameplay stops
        /// </summary>
        void GameplayStop();

        /// <summary>
        /// Called when the game is happy (good moment for ads/prompts)
        /// </summary>
        void HappyTime();

        /// <summary>
        /// Checks if audio is enabled for the session
        /// </summary>
        /// <returns>True if audio is enabled, false otherwise</returns>
        bool IsAudioEnabled { get; }

        /// <summary>
        /// Callback for audio status change
        /// </summary>
        /// <param name="onAudioEnabled">Callback with audio enabled status</param>
        void OnAudioStatusChange(Action<bool> onAudioEnabled);

        /// <summary>
        /// Checks if session features are supported on this platform
        /// </summary>
        bool IsSupported { get; }
    }
}
