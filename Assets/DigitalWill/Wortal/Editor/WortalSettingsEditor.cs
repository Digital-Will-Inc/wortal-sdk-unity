using DigitalWill.H5Portal;
using UnityEditor;

namespace DigitalWill.Wortal.Editor
{
    /// <summary>
    /// Editor for Wortal settings.
    /// </summary>
    [CustomEditor(typeof(WortalSettings))]
    public class WortalSettingsEditor : UnityEditor.Editor
    {
        [MenuItem("DigitalWill/Wortal/Wortal Config")]
        private static void ShowSettingsMenu()
        {
            Selection.activeObject = H5Portal.Wortal.Settings;
        }
    }
}
