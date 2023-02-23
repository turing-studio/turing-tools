using UnityEngine;

namespace Turing.Tools.ObjectPooling
{
    public interface IPool<T> where T: Component
    {
        void SetOriginal(T original);
        T Get();
        void Clear();
    }
}