using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace DigitalWill.WortalEditor
{
    /// <summary>
    /// Sets the project settings required to use the Wortal plugin.
    /// </summary>
    public static class WortalInstaller
    {
        private const string LOG_PREFIX = "[WortalConfig] ";
        private const string TEMPLATE_PATH = "PROJECT:Wortal";

        [MenuItem("DigitalWill/Wortal/Set Project Settings")]
        private static void Install()
        {
            Debug.Log(LOG_PREFIX + "Starting Wortal SDK plugin configuration..");

            // AFG requires this.
            PlayerSettings.runInBackground = true;
            Debug.Log(LOG_PREFIX + "Setting player settings..");

            // We need the provided template that includes the SDK Core and instance.js.
            PlayerSettings.WebGL.template = TEMPLATE_PATH;
            Debug.Log(LOG_PREFIX + "Setting WebGL template..");

            // Some platforms do not support the decompression fallback extension type.
            PlayerSettings.WebGL.decompressionFallback = false;
            Debug.Log(LOG_PREFIX + "Disabling compression fallback..");

            // This is necessary to prevent code stripping that will break some methods that return
            // a serialized array of objects, such as Leaderboard.GetConnectedPlayersEntries and IAP.GetCatalog.
#if UNITY_2022_1_OR_NEWER
            PlayerSettings.SetIl2CppCodeGeneration(NamedBuildTarget.WebGL, Il2CppCodeGeneration.OptimizeSize);
#else
            EditorUserBuildSettings.il2CppCodeGeneration = Il2CppCodeGeneration.OptimizeSize;
#endif
            Debug.Log(LOG_PREFIX + "Setting IL2CPP code generation strategy..");

            Debug.Log(LOG_PREFIX + "Installation finished.");
        }

        [InitializeOnLoadMethod]
        private static void CheckSettings()
        {
            if (!PlayerSettings.runInBackground)
            {
                Debug.LogWarning(LOG_PREFIX + "PlayerSettings.runInBackground should be true.");
            }
            if (PlayerSettings.WebGL.template != TEMPLATE_PATH)
            {
                Debug.LogWarning(LOG_PREFIX + "Wortal WebGL template should be used.");
            }
            if (PlayerSettings.WebGL.decompressionFallback)
            {
                Debug.LogWarning(LOG_PREFIX + "Wortal SDK does not currently support decompression fallback");
            }
#if UNITY_2022_1_OR_NEWER
            if (PlayerSettings.GetIl2CppCodeGeneration(NamedBuildTarget.WebGL) != Il2CppCodeGeneration.OptimizeSize)
            {
                Debug.LogWarning(LOG_PREFIX + "IL2CPP code generation should be set to OptimizeSize. Faster (smaller) builds.");
            }
#else
            if (EditorUserBuildSettings.il2CppCodeGeneration != Il2CppCodeGeneration.OptimizeSize)
            {
                Debug.LogWarning(LOG_PREFIX + "IL2CPP code generation should be set to OptimizeSize. Faster (smaller) builds.");
            }
#endif
        }
    }
}
