using UnityEngine;

namespace FTKingdom.Utils
{
    public class BaseSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
#if UNITY_EDITOR
        protected static bool SHOW_DEBUG = false;
#endif
        private static T _instance = null;
        protected static T BaseInstance
        {
            get
            {
                if (_instance == null)
                {
                    var instances = FindObjectsByType(typeof(T), FindObjectsSortMode.None);
                    if (instances.Length > 0)
                    {
                        _instance = instances[0] as T;
#if UNITY_EDITOR
                        if (instances.Length > 1 && SHOW_DEBUG)
                        {
                            Debug.LogError($"There is multiple instances of {typeof(T)} in this scene. This can lead to errors.");

                            for (int i = 0; i < instances.Length; i++)
                            {
                                GameObject instanceGameObject = (instances[i] as T).gameObject;
                                Debug.LogError($"{typeof(T)} ({i + 1}): {instanceGameObject.name}", instanceGameObject);
                            }
                        }
#endif
                    }
                }

                return _instance;
            }
        }
    }
}