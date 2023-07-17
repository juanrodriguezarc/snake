using UnityEngine;

namespace Snake.Utility
{

    /// <summary>
    /// Simple singleton implementation for MonoBehaviours.
    ///  This is a globally accessible class that exists in the scene, but only once.
    /// </summary>

    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }
        protected virtual void Awake() => Instance = this as T;

        protected virtual void OnApplicationQuit()
        {
            Instance = null;
            Destroy(gameObject);
        }
    }

    public abstract class SingletonPersistent<T> : Singleton<T> where T : MonoBehaviour
    {

        protected override void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
            base.Awake();
        }
    }
}

