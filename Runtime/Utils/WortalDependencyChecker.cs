using System;
using System.Linq;
using System.Reflection;
using UnityEngine;


namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Checks for platform-specific dependencies and provides user guidance
    /// </summary>
    public static class WortalDependencyChecker
    {
        public static bool HasGooglePlayGames
        {
            get
            {
                // Try multiple detection methods
                return CheckGooglePlayGamesAssembly() || CheckGooglePlayGamesTypes() || CheckGooglePlayGamesNamespace();
            }
        }

        private static bool CheckGooglePlayGamesAssembly()
        {
            try
            {
                // Check if the assembly exists
                var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
                return assemblies.Any(assembly =>
                    assembly.GetName().Name.Contains("GooglePlayGames") ||
                    assembly.GetName().Name.Contains("Google.Play.Games"));
            }
            catch
            {
                return false;
            }
        }

        private static bool CheckGooglePlayGamesTypes()
        {
            try
            {
                // Try different possible type names and assemblies
                var typeNames = new[]
                {
                    "GooglePlayGames.PlayGamesPlatform, GooglePlayGames",
                    "GooglePlayGames.PlayGamesPlatform, Assembly-CSharp",
                    "GooglePlayGames.PlayGamesPlatform",
                    "GooglePlayGames.PlayGamesPlatform, Google.Play.Games",
                    "GooglePlayGames.PlayGamesPlatform, com.google.play.games"
                };

                foreach (var typeName in typeNames)
                {
                    var type = Type.GetType(typeName);
                    if (type != null)
                    {
                        Debug.Log($"[Wortal] Found Google Play Games type: {typeName}");
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private static bool CheckGooglePlayGamesNamespace()
        {
            try
            {
                // Check all loaded assemblies for GooglePlayGames namespace
                var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    try
                    {
                        var types = assembly.GetTypes();
                        if (types.Any(t => t.Namespace != null && t.Namespace.StartsWith("GooglePlayGames")))
                        {
                            Debug.Log($"[Wortal] Found GooglePlayGames namespace in assembly: {assembly.GetName().Name}");
                            return true;
                        }
                    }
                    catch (ReflectionTypeLoadException)
                    {
                        // Some assemblies might not load completely, skip them
                        continue;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool HasAppleGameKit =>
            Type.GetType("UnityEngine.SocialPlatforms.GameCenter.GameCenterPlatform, UnityEngine.GameCenterModule") != null;

        /// <summary>
        /// Check Android-specific dependencies
        /// </summary>
        public static void CheckAndroidDependencies()
        {
            var settings = WortalSettings.Instance;

            if (settings.enableGooglePlayGames && !HasGooglePlayGames)
            {
                Debug.LogWarning("[Wortal] Google Play Games SDK not found but is enabled in settings. " +
                               "Android authentication and social features will be limited.\n" +
                               "To enable full functionality:\n" +
                               "1. Open Window > Package Manager\n" +
                               "2. Search for 'Google Play Games'\n" +
                               "3. Install the package\n" +
                               "Or disable Google Play Games in Wortal Settings if not needed.");
            }
            else if (HasGooglePlayGames)
            {
                if (settings.enableDebugLogging)
                    Debug.Log("[Wortal] Google Play Games SDK detected and ready.");
            }
        }

        /// <summary>
        /// Check iOS-specific dependencies
        /// </summary>
        public static void CheckiOSDependencies()
        {
            var settings = WortalSettings.Instance;

            if (settings.enableAppleGameCenter && !HasAppleGameKit)
            {
                Debug.LogWarning("[Wortal] Apple Game Center not available but is enabled in settings. " +
                               "iOS authentication and social features will be limited.\n" +
                               "Game Center should be available by default on iOS builds.");
            }
            else if (HasAppleGameKit)
            {
                if (settings.enableDebugLogging)
                    Debug.Log("[Wortal] Apple Game Center detected and ready.");
            }
        }

        /// <summary>
        /// Check all platform dependencies based on current build target
        /// </summary>
        public static void CheckAllDependencies()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            CheckAndroidDependencies();
#elif UNITY_IOS && !UNITY_EDITOR
            CheckiOSDependencies();
#elif UNITY_EDITOR
            // In editor, check for all platforms
            CheckAndroidDependencies();
            CheckiOSDependencies();
#endif
        }

        /// <summary>
        /// Get dependency status for UI display
        /// </summary>
        public static DependencyStatus GetDependencyStatus()
        {
            return new DependencyStatus
            {
                HasGooglePlayGames = HasGooglePlayGames,
                HasAppleGameKit = HasAppleGameKit,
                AndroidReady = !WortalSettings.Instance.enableGooglePlayGames || HasGooglePlayGames,
                iOSReady = !WortalSettings.Instance.enableAppleGameCenter || HasAppleGameKit
            };
        }

        /// <summary>
        /// Runtime initialization - called automatically
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void RuntimeDependencyCheck()
        {
            if (WortalSettings.Instance.enableDebugLogging)
            {
                CheckAllDependencies();
            }
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void DebugDependencyDetection()
        {
            Debug.Log("=== Wortal Dependency Debug ===");

            // List all assemblies
            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            Debug.Log($"Total assemblies loaded: {assemblies.Length}");

            foreach (var assembly in assemblies)
            {
                var name = assembly.GetName().Name;
                if (name.Contains("Google") || name.Contains("Play") || name.Contains("Games"))
                {
                    Debug.Log($"Found Google-related assembly: {name}");

                    try
                    {
                        var types = assembly.GetTypes();
                        var googleTypes = types.Where(t => t.Namespace != null &&
                            (t.Namespace.Contains("Google") || t.Namespace.Contains("Play"))).ToArray();

                        foreach (var type in googleTypes.Take(5)) // Show first 5
                        {
                            Debug.Log($"  - Type: {type.FullName}");
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log($"  - Could not load types: {e.Message}");
                    }
                }
            }

            // Test specific type lookups
            var testTypes = new[]
            {
        "GooglePlayGames.PlayGamesPlatform",
        "GooglePlayGames.PlayGamesPlatform, GooglePlayGames",
        "GooglePlayGames.PlayGamesPlatform, Assembly-CSharp",
        "GooglePlayGames.PlayGamesPlatform, com.google.play.games"
    };

            foreach (var typeName in testTypes)
            {
                var type = Type.GetType(typeName);
                Debug.Log($"Type lookup '{typeName}': {(type != null ? "FOUND" : "NOT FOUND")}");
            }
        }

    }

    /// <summary>
    /// Dependency status information
    /// </summary>
    [System.Serializable]
    public class DependencyStatus
    {
        public bool HasGooglePlayGames;
        public bool HasAppleGameKit;
        public bool AndroidReady;
        public bool iOSReady;
    }
}
