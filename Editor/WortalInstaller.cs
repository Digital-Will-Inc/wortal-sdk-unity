using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace DigitalWill.WortalEditor
{
    /// <summary>
    /// Installs the Wortal SDK and sets the project settings.
    /// </summary>
    public static class WortalInstaller
    {
        private const string LOG_PREFIX = "[WortalConfig] ";
        private const string TEMPLATE_NAME = "PROJECT:Wortal";
        private const string TEMPLATE_PATH = "Assets/WebGLTemplates/Wortal";
        private const string TEMPLATE_SOURCE = "Packages/jp.co.digitalwill.wortal/Assets/WebGLTemplates/Wortal";

        // [MenuItem("Wortal/WebGL/Install WebGL Template")]
        internal static void InstallWebGLTemplate()
        {
            Debug.Log(LOG_PREFIX + "Installing Wortal WebGL template...");

            string destinationFolder = Path.GetFullPath(TEMPLATE_PATH);
            string sourceFolder = Path.GetFullPath(TEMPLATE_SOURCE);

            Debug.Log($"{LOG_PREFIX}Copying template from {sourceFolder}...");

            try
            {
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                foreach (string dirPath in Directory.GetDirectories(sourceFolder, "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dirPath.Replace(sourceFolder, destinationFolder));
                }

                foreach (string newPath in Directory.GetFiles(sourceFolder, "*.*", SearchOption.AllDirectories))
                {
                    File.Copy(newPath, newPath.Replace(sourceFolder, destinationFolder), true);
                }
            }
            catch (IOException exception)
            {
                Debug.LogError("Failed to copy WebGL template: " + exception.Message);
                return;
            }

            AssetDatabase.Refresh();

            Debug.Log($"{LOG_PREFIX}Setting WebGL template, old was = {PlayerSettings.WebGL.template}");

            PlayerSettings.WebGL.template = TEMPLATE_NAME;

            Debug.Log($"{LOG_PREFIX}Set WebGL template to {PlayerSettings.WebGL.template}");

            Debug.Log(LOG_PREFIX + "Wortal WebGL template installed.");
        }

        // [MenuItem("Wortal/WebGL/Set Project Settings")]
        internal static void SetProjectSettings()
        {
            Debug.Log(LOG_PREFIX + "Starting Wortal SDK plugin configuration..");

            // AFG requires this.
            PlayerSettings.runInBackground = true;
            Debug.Log(LOG_PREFIX + "Setting player settings..");

            // We need the provided template that includes the SDK Core and instance.js.
            PlayerSettings.WebGL.template = TEMPLATE_NAME;
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
        internal static void CheckSettings()
        {
            if (!PlayerSettings.runInBackground)
            {
                Debug.LogWarning(LOG_PREFIX + "PlayerSettings.runInBackground should be true.");
            }
            if (PlayerSettings.WebGL.template != TEMPLATE_NAME)
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
