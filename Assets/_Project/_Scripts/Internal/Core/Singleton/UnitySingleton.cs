using UnityEngine;

namespace Internal.Core.Singleton
{
    public class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        private void Awake()
        {
            if (AreSingletonsExists())
            {
                DestroyImmediate(this);
                return;
            }

            DontDestroyOnLoad(gameObject);
            Instance = this as T;
        }

        private bool AreSingletonsExists()
        {
            return FindObjectsByType<T>(FindObjectsSortMode.None).Length > 1;
        }
    }
}