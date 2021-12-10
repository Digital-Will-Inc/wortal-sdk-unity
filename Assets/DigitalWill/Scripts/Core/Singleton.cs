using UnityEngine;

namespace DigitalWill.Core
{
    /// <summary>
    /// Singleton to provide global access to a single instance of a class.
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        /// <summary>
        /// Returns the instance of the singleton. Creates a new one if one is not found.
        /// </summary>
        public static T I
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                // Search for existing instances.
                var objs = FindObjectsOfType(typeof(T)) as T[];

                // Assign the first instance we find.
                if (objs != null && objs.Length > 0)
                {
                    _instance = objs[0];
                }

                // We should only ever have one instance.
                if (objs != null && objs.Length > 1)
                {
                    Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
                }

                // If we've found an instance then we return it.
                if (_instance != null)
                {
                    return _instance;
                }

                // Create a new instance if we didn't find one.
                var obj = new GameObject {hideFlags = HideFlags.HideAndDontSave};
                _instance = obj.AddComponent<T>();

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.LogError("There is already an instance of " + typeof(T).Name + " in the scene.");
            }
            else
            {
                _instance = this as T;
            }
        }
    }
}
