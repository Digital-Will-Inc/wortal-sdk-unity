using UnityEngine;
using System;
using System.Linq;

namespace DigitalWill.WortalSDK.Core
{
    /// <summary>
    /// Manages platform detection and provides the appropriate platform implementation
    /// </summary>
    public static class WortalPlatformManager
    {
        private static IWortalPlatform _currentPlatform;
        private static bool _isInitialized = false;

        /// <summary>
        /// Gets the current platform implementation
        /// </summary>
        public static IWortalPlatform CurrentPlatform
        {
            get
            {
                if (!_isInitialized)
                {
                    InitializePlatform();
                }
                return _currentPlatform;
            }
        }

        /// <summary>
        /// Gets the current platform type
        /// </summary>
        public static WortalPlatformType PlatformType { get; private set; }

        /// <summary>
        /// Initializes the platform detection and creates appropriate platform instance
        /// </summary>
        private static void InitializePlatform()
        {
            PlatformType = DetectPlatform();
            _currentPlatform = CreatePlatformImplementation(PlatformType);
            _isInitialized = true;

            Debug.Log($"[Wortal] Platform detected: {PlatformType}");
        }

        /// <summary>
        /// Detects the current platform at runtime
        /// </summary>
        private static WortalPlatformType DetectPlatform()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return WortalPlatformType.WebGL;
#elif UNITY_ANDROID && !UNITY_EDITOR
            return WortalPlatformType.Android;
#elif UNITY_IOS && !UNITY_EDITOR
            return WortalPlatformType.iOS;
#else
            // Editor or unsupported platform
            return GetEditorPlatform();
#endif
        }

        /// <summary>
        /// Gets the platform type when running in Unity Editor
        /// </summary>
        private static WortalPlatformType GetEditorPlatform()
        {
#if UNITY_EDITOR
            switch (UnityEditor.EditorUserBuildSettings.activeBuildTarget)
            {
                case UnityEditor.BuildTarget.WebGL:
                    return WortalPlatformType.WebGL;
                case UnityEditor.BuildTarget.Android:
                    return WortalPlatformType.Android;
                case UnityEditor.BuildTarget.iOS:
                    return WortalPlatformType.iOS;
                default:
                    return WortalPlatformType.WebGL; // Fallback
            }
#else
            return WortalPlatformType.WebGL;
#endif
        }

        /// <summary>
        /// Creates the appropriate platform implementation based on platform type
        /// </summary>
        private static IWortalPlatform CreatePlatformImplementation(WortalPlatformType platformType)
        {
            switch (platformType)
            {
                case WortalPlatformType.WebGL:
                    return new WebGLWortalPlatform();
                case WortalPlatformType.Android:
                    return new AndroidWortalPlatform();
                case WortalPlatformType.iOS:
                    return new iOSWortalPlatform();
                default:
                    Debug.LogWarning($"[Wortal] Unsupported platform: {platformType}, falling back to WebGL");
                    return new WebGLWortalPlatform();
            }
        }

        /// <summary>
        /// Forces reinitialization of platform detection (useful for testing)
        /// </summary>
        public static void ForceReinitialize()
        {
            _isInitialized = false;
            _currentPlatform = null;
        }
    }
}
