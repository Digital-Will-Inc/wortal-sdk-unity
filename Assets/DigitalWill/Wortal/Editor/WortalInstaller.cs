using UnityEditor;
using UnityEngine;

namespace DigitalWill.WortalEditor
{
    /// <summary>
    /// Sets the project settings required to use the Wortal plugin.
    /// </summary>
    public static class WortalInstaller
    {
        private const string LOG_PREFIX = "[WortalInstaller] ";
        private const string TEMPLATE_PATH = "PROJECT:Wortal";

        [MenuItem("DigitalWill/Wortal/Set Project Settings")]
        private static void Install()
        {
            Debug.Log(LOG_PREFIX + "Starting wortal plugin install..");

            PlayerSettings.runInBackground = true;
            Debug.Log(LOG_PREFIX + "Setting player settings..");

            PlayerSettings.WebGL.template = TEMPLATE_PATH;
            Debug.Log(LOG_PREFIX + "Setting WebGL template..");

            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
            Debug.Log(LOG_PREFIX + "Disabling compression..");

            Debug.Log(LOG_PREFIX + "Installation finished.");
        }
    }
}
