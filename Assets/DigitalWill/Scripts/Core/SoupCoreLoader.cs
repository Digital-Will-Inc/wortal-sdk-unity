using System;
using UnityEngine;

namespace DigitalWill.Core
{
    /// <summary>
    /// Loads the SoupCore prefab at runtime. This prefab contains <see cref="SoupComponent"/> which is critical
    /// for Soup to work.
    /// </summary>
    /// <remarks>This prefab is marked DDOL so you can attach other persistent scripts to it such as GameManagers.</remarks>
    public class SoupCoreLoader : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void LoadCore()
        {
            try
            {
                var soup = Instantiate(Resources.Load("SoupCore")) as GameObject;

                if (soup != null)
                {
                    if (soup.TryGetComponent(typeof(SoupComponent), out Component comp))
                    {
                        Soup.Log("SoupCoreLoader: SoupCore loaded.");
                    }
                    else
                    {
                        soup.AddComponent<SoupComponent>();
                        Soup.LogWarning("SoupCoreLoader: SoupCore prefab found with no SoupComponent. Adding component..");
                    }
                }

                DontDestroyOnLoad(soup);
            }
            catch (Exception)
            {
                var soup = new GameObject("SoupCore");
                soup.AddComponent<SoupComponent>();
                DontDestroyOnLoad(soup);
                Soup.LogError("#CRITICAL# SoupCoreLoader: SoupCore prefab was not found in Resources directory. Creating new Soup object with default settings..");
            }

            Soup.Log("SoupCoreLoader: SoupCore initializing..");
        }
    }
}
