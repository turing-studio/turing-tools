using System;

namespace Turing.Tools.Observers
{
    public interface IImposableAction<T> where T : Delegate
    {
        void SetAction(T action);
        void RemoveAction(T action);
    }
}