#if UNITY_EDITOR
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace DigitalWill.Wortal.Editor
{
    /// <summary>
    /// Imports the dependency package for the wortal plugin.
    /// </summary>
    [InitializeOnLoad]
    public static class WortalInstaller
    {
        private const string LOG_PREFIX = "[WortalInstaller] ";
        private const string PACKAGE_NAME = "co.digitalwill.wortal";
        private const string DEPENDENCIES_PATH = "Packages/co.digitalwill.wortal/Import/";
        private const string TEMPLATE_PATH = "PROJECT:Wortal";
        private const string WARNING_TITLE = "WARNING: Reinstall Wortal package";
        private const string WARNING_DESC = "This will reinstall the Wortal package, which may overwrite previously saved settings and configuration for the package. It will also change some project settings. Are you sure you want to proceed?";

        static WortalInstaller()
        {
            Events.registeredPackages += OnPackageEvent;
        }

        private static void OnPackageEvent(PackageRegistrationEventArgs diff)
        {
            PackageInfo wortal = diff.added.FirstOrDefault(p => p.name == PACKAGE_NAME);
            if (wortal != null)
            {
                Install();
            }
        }

        [MenuItem("DigitalWill/Reinstall Wortal")]
        private static void Reinstall()
        {
            if (EditorUtility.DisplayDialog(WARNING_TITLE, WARNING_DESC, "Reinstall", "Cancel"))
            {
                Install();
            }
        }

        private static void Install()
        {
            Debug.Log(LOG_PREFIX + "Starting wortal plugin install..");

            string[] packages = Directory.GetFiles(DEPENDENCIES_PATH);

            for (int i = 0; i < packages.Length; i++)
            {
                if (packages[i].EndsWith(".unitypackage"))
                {
                    AssetDatabase.ImportPackage(packages[i], false);
                    Debug.Log(LOG_PREFIX + $"Importing {packages[i]}..");
                }
            }

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
#endif
