using UnityEngine;

namespace IGS.Unity
{
    [DisallowMultipleComponent]
    public abstract class Singleton : MonoBehaviour
    {
        public static bool Quitting { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            Quitting = false;
        }

        private void OnApplicationQuit()
        {
            Quitting = true;
        }
    }

    public abstract class Singleton<T> : Singleton where T : MonoBehaviour
    {
        private static readonly object _lock = new object();
        private static bool _initializing = false;

        private static T _instance = null;

        public static T Instance
        {
            get
            {
                if(Quitting)
                {
                    GameLogger.Log(string.Format("Instance {0} will not be returned because the application is quitting", typeof(T).Name), LogFilter.Warning);
                    return null;
                }

                lock(_lock)
                {
                    // instance already found?
                    if(_instance != null)
                    {
                        return _instance;
                    }

                    _initializing = true;

                    // search for and in-scene instace of T
                    var allInstances = FindObjectsOfType<T>();
                    // found exactly one?
                    if(allInstances.Length == 1)
                    {
                        _instance = allInstances.First();   
                    }
                    // found none?
                    else if(allInstances.Length == 0)
                    {
                        _instance = new GameObject(string.Format("[Singleton] {0}", typeof(T).Name)).AddComponent<T>(); 
                    }
                    // multiple found?
                    else
                    {
                        _instance = allInstances.First();

                        // destroy the duplicates
                        for(int i=0; i<allInstances.Length; i++)
                        {
                            GameLogger.Log(string.Format("Destroying duplicate {0} on {1}", typeof(T).Name, allInstances[i].gameObject.name), LogFilter.Error);

                            Destroy(allInstances[i].gameObject);
                        }
                    }
                }

                _initializing = false;

                return _instance;
            }
        }

        private static void ConstructIfNeeded(Singleton<T> IN)
        {
            lock(_lock)
            {
                if(_instance == null && !_initializing)
                {
                    _initializing = IN as T;
                }
                else if(_instance != null && !_initializing)
                {
                    GameLogger.Log(string.Format("Destroying duplicate {0} on {1}", typeof(T).Name, IN.gameObject.name), LogFilter.Error);

                    Destroy(IN.gameObject);
                }
            }
        }

        private void Awake()
        {
            ConstructIfNeeded(this);

            OnAwake();
        }

        protected virtual void OnAwake() { }
    }
}
