using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DigitalWill.WortalEditor
{
    /// <summary>
    /// Editor for Wortal settings.
    /// </summary>
    [CustomEditor(typeof(WortalSettings))]
    public class WortalSettingsEditor : Editor
    {
        private const string LOG_PREFIX = "[Wortal] ";
        private const string SETTINGS_PATH_FULL = "Assets/DigitalWill/Wortal/Resources/WortalSettings.asset";
        private const string SETTINGS_PATH_RELATIVE = "/DigitalWill/Wortal/Resources";

        private static WortalSettings _settings;

        [MenuItem("DigitalWill/Wortal/Wortal Config")]
        private static void ShowSettingsMenu()
        {
            try
            {
                _settings = Resources.Load<WortalSettings>("WortalSettings");

                if (_settings == null)
                {
                    if (!Directory.Exists(Application.dataPath + SETTINGS_PATH_RELATIVE))
                    {
                        Directory.CreateDirectory(Application.dataPath + SETTINGS_PATH_RELATIVE);
                        Debug.Log(LOG_PREFIX + "Could not find Wortal settings directory, creating now.");
                    }

                    // We found a settings file, but for some reason it didn't load properly. It might be corrupt.
                    // We will just delete it and create a new one with default values.
                    if (File.Exists(SETTINGS_PATH_FULL))
                    {
                        AssetDatabase.DeleteAsset(SETTINGS_PATH_FULL);
                        AssetDatabase.Refresh();
                        Debug.LogWarning(LOG_PREFIX + "WortalSettings file was corrupted. Re-creating now.");
                    }

                    var asset = ScriptableObject.CreateInstance<WortalSettings>();
                    AssetDatabase.CreateAsset(asset, SETTINGS_PATH_FULL);

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    Debug.Log(LOG_PREFIX + "WortalSettings file was missing. Created a new one.");

                    _settings = asset;
                    Selection.activeObject = asset;
                }
                else
                {
                    Selection.activeObject = _settings;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(LOG_PREFIX + $"Failed to initialize. \n{e}");
            }
        }
    }
}
