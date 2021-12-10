using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DigitalWill.Core
{
    /// <summary>
    /// Handles Soup settings and some SDK-specific functions. Service classes that do not inherit from MonoBehaviour
    /// but wish to use coroutines may call the instance of this class to invoke StartCoroutine.
    /// </summary>
    /// <remarks>Settings should only be edited from the editor window and never at runtime.</remarks>
    public class SoupComponent : Singleton<SoupComponent>
    {
        private const string SETTINGS_PATH = "Assets/DigitalWill/Resources/SoupSettings.asset";

        private static SoupSettings _settings;

        /// <summary>
        /// Settings used to initialize Soup. These should only be edited in the Soup Settings editor window.
        /// </summary>
        public static SoupSettings SoupSettings
        {
            get
            {
                if (_settings == null)
                {
                    InitSoup();
                }

                return _settings;
            }
        }

        private static void InitSoup()
        {
            try
            {
                _settings = Resources.Load<SoupSettings>("SoupSettings");

#if UNITY_EDITOR
                if (_settings == null)
                {
                    if (!Directory.Exists(Application.dataPath + "/DigitalWill/Resources"))
                    {
                        Directory.CreateDirectory(Application.dataPath + "/DigitalWill/Resources");
                        Soup.LogWarning("SoupComponent: Detected missing DigitalWill/Resources directory. creating now.");
                    }

                    // We found a settings file, but for some reason it didn't load properly. It might be corrupt.
                    // We will just delete it and create a new one with default values.
                    if (File.Exists(SETTINGS_PATH))
                    {
                        AssetDatabase.DeleteAsset(SETTINGS_PATH);
                        AssetDatabase.Refresh();
                        Soup.LogWarning("SoupComponent: Detected a potentially corrupt SoupSettings file. Deleting now.");
                    }

                    var asset = ScriptableObject.CreateInstance<SoupSettings>();
                    AssetDatabase.CreateAsset(asset, SETTINGS_PATH);

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    Soup.LogWarning("SoupComponent: SoupSettings file was missing. Created a new one.");

                    _settings = asset;
                    Selection.activeObject = asset;
                }
#endif
            }
            catch(Exception e)
            {
                Soup.LogError($"SoupComponent: Failed to initialize. \n{e}");
            }
        }
    }
}
