using UnityEngine;

namespace Capybara
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static readonly object _lock = new object();

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindFirstObjectByType<T>();

                        if (FindObjectsByType<T>(FindObjectsSortMode.None).Length > 1)
                        {
                            Debug.LogError("[Singleton] Something went really wrong - " +
                                           "there should never be more than 1 singleton! " +
                                           "Reopening the scene might fix it.");
                        }
                    }
                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }
            else
            {
                Debug.LogError("Ey too many instances mateeeeeee: " + this.GetType().Name);
                Destroy(gameObject); // Optionally destroy this instance if a duplicate is found
            }
        }
    }
}