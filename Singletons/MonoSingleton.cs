using System;
using UnityEngine;

namespace Turing.Tools.Singletons
{
    /// <summary>
    /// Singleton pattern. Instance can be destroyed.
    /// New instance cannot be set until current instance exists.
    /// Object using component of this type should not have other singleton components.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        private static bool _destroyed;

        public static T Instance
        {
            get
            {
                if (_instance == null && !_destroyed)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject();
                        _instance = obj.AddComponent<T>();
                    }

                    _instance.gameObject.name = _instance.GetType().Name;
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
                _instance = this as T;
            // Destroy self if not _instance
            else if (this != _instance)
                Destroy(this.gameObject);
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _destroyed = true;
        }
    }
}