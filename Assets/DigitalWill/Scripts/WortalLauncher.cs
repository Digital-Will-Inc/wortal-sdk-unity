using System;
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

                if (wortal != null)
                {
                    if (wortal.TryGetComponent(typeof(Wortal), out Component comp))
                    {
                        Debug.Log("WortalLoader: Wortal prefab loaded.");
                    }
                    else
                    {
                        wortal.AddComponent<Wortal>();
                        Debug.LogWarning("WortalLoader: Wortal prefab found with no Wortal. Adding component..");
                    }
                }

                DontDestroyOnLoad(wortal);
            }
            catch (Exception)
            {
                var wortal = new GameObject("Wortal");
                wortal.AddComponent<Wortal>();
                DontDestroyOnLoad(wortal);
                Debug.LogError("#CRITICAL# WortalLoader: Wortal prefab was not found in Resources directory. Creating new prefab with default settings..");
            }

            Debug.Log("WortalLoader: Wortal initializing..");
        }
    }
}
