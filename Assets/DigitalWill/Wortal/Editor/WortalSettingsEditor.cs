using DigitalWill.H5Portal;
using UnityEditor;

namespace DigitalWill.Wortal.Editor
{
    /// <summary>
    ///
    /// </summary>
    [CustomEditor(typeof(WortalSettings))]
    public class WortalSettingsEditor : UnityEditor.Editor
    {
        [MenuItem("DigitalWill/Wortal Settings")]
        private static void ShowSettingsMenu()
        {
            Selection.activeObject = H5Portal.Wortal.Settings;
        }
    }
}
