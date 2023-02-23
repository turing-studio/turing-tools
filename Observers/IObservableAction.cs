using System;

namespace Turing.Tools.Observers
{
    public interface IObservableAction<T> where T : Delegate
    {
        void AddObserver(T observer);
        void RemoveObserver(T observer);
    }
}