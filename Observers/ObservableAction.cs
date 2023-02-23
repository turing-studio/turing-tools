using System;

namespace Turing.Tools.Observers
{
    public class ObservableAction<T> : IObservableAction<T> where T : Delegate
    {
        public T Action { get; private set; }

        public void AddObserver(T observer)
        {
            Action = Delegate.Combine(Action, observer) as T;
        }
        
        public void RemoveObserver(T observer)
        {
            Action = Delegate.Remove(Action, observer) as T;
        }
    }
}