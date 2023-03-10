using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Turing.Tools.Extensions;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace Turing.Tools.ObjectPooling
{
    /// <summary>
    /// Poolable.
    /// Make sure only one Poolable object is attacted to the GameObject
    /// </summary>
    [System.Serializable]
    public abstract class Poolable<T> : MonoBehaviour where T : Component
    {
        private class ObjectPool : IPool<T>
        {
            private T _original;
            private Queue<Poolable<T>> _objects = new Queue<Poolable<T>>();
            private const int MinSize = 5;

            private Transform _poolParent;

            public void SetOriginal(T original)
            {
                _original = original;
            }

            public T Get()
            {
                while (_objects.Count < MinSize)
                {
                    if (!_poolParent)
                        _poolParent = new GameObject(_original.GetType().Name + " Pool").transform;
                    Add(Instantiate(_original.gameObject, _poolParent).GetComponent<Poolable<T>>());
                }

                Poolable<T> obj = null;
                do
                    obj = _objects.Dequeue();
                while (obj == null);
                obj._isInPool = false;
                return obj as T;
            }

            public void Clear()
            {
                while (_objects.Count > 0)
                    Destroy(_objects.Dequeue());
            }

            public void Add(Poolable<T> obj)
            {
                _objects.Enqueue(obj);
                obj._isInPool = true;
            }

            public void DeleteFromPool(Poolable<T> obj)
            {
                var queue = new Queue<Poolable<T>>();
                while (_objects.Count > 0)
                {
                    var o = _objects.Dequeue();
                    int first = o.gameObject.GetInstanceID();
                    int second = obj.gameObject.GetInstanceID();
                    if (first != second)
                        queue.Enqueue(o);
                }

                _objects = queue;
            }
        }

        private bool _isInPool;

        private static ObjectPool _pool = new ObjectPool();

        public static IPool<T> Pool => _pool;

        /// <summary>
        /// Revert object to start state.
        /// </summary>
        public abstract void Reset();

        public void Remove()
        {
            if (_isInPool)
                throw new InvalidOperationException("Object is already in the pool");
            _pool.Add(this);
        }

        private void OnDestroy()
        {
            //_pool.DeleteFromPool(this);
        }
    }
}