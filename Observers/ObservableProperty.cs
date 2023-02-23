using System;

namespace Turing.Tools.Observers
{
    public class ObservableProperty<T> : IObservableProperty<T>
    {
        private Action<T> Action { get; set; }

        public void AddObserver(Action<T> observer)
        {
            Action = Delegate.Combine(Action, observer) as Action<T>;
        }
        
        public void RemoveObserver(Action<T> observer)
        {
            Action = Delegate.Remove(Action, observer) as Action<T>;
        }
        
        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                if (_value != null && _value.Equals(value) || _value == null && value == null) 
                    return;
                _value = value;
                Action?.Invoke(_value);
            }
        }

        public void Set(T value) => Value = value;

        public static implicit operator T(ObservableProperty<T> o)
        {
            return o._value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}