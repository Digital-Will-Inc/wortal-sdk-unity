// ReSharper disable CheckNamespace
using DigitalWill.Core;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace DigitalWill
{
    /// <summary>
    /// Custom editor for Soup settings to be used in the <see cref="SoupEditorWindow"/>. Removes the default
    /// inspector view of the settings and allows for opening the editor window via the inspector or project browser.
    /// </summary>
    [CustomEditor(typeof(SoupSettings))]
    public class SoupSettingsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Removes the inspector view of the settings asset and provides a button to launch the editor window.
            if (GUILayout.Button("Open"))
            {
                SoupEditorWindow.ShowSoupWindow();
            }
        }
    }

    /// <summary>
    /// Handles opening SoupSettings asset from project browser. Double-clicking asset will launch editor window.
    /// </summary>
    public class SoupAssetHandler
    {
        [OnOpenAsset]
        public static bool OpenEditor(int instanceId, int line)
        {
            var settings = EditorUtility.InstanceIDToObject(instanceId) as SoupSettings;

            if (settings != null)
            {
                SoupEditorWindow.ShowSoupWindow();
                return true;
            }

            return false;
        }
    }
}
