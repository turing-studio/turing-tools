using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Turing.Tools.Singletons
{
    /// <summary>
    /// Singleton pattern. Instance is persistent, i.e. not destroyed on load.
    /// New instance cannot be set until current instance exists.
    /// Object using component of this type should not have other singleton components.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PersistentMonoSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (!Application.isPlaying)
                    return null;
                if (_instance != null)
                    return _instance;
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                    _instance = new GameObject().AddComponent<T>();
                _instance.gameObject.name = _instance.GetType().Name;
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
                _instance = this as T;
            if (_instance != this)
                Destroy(gameObject);
            if (_instance == this)
                DontDestroyOnLoad(gameObject);
        }
    }
}