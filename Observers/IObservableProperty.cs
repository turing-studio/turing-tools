using System;

namespace Turing.Tools.Observers
{
    public interface IObservableProperty<T> : IObservableAction<Action<T>>
    {
        T Value { get; }
    }
}