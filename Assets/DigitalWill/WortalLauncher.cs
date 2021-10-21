using UnityEngine;

namespace DigitalWill
{
    /// <summary>
    /// Handles auto-loading and persisting the Wortal prefab.
    /// </summary>
    public class WortalLauncher : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LoadWortal()
        {
            try
            {
                var wortal = Instantiate(Resources.Load("Wortal")) as GameObject;
                DontDestroyOnLoad(wortal);
                Debug.Log("Initializing Wortal prefab.");
            }
            catch (System.ArgumentException e)
            {
                Debug.LogError("CRITICAL: Wortal prefab was not found in Resources directory. \n" + e);
            }
        }
    }
}
