using System;
using System.Collections.Generic;
using System.Linq;

namespace Turing.Tools.Observers
{
    public class ImposableAction<T> : IImposableAction<T> where T: Delegate
    {
        public T Action => _actions.LastOrDefault();

        private readonly List<T> _actions = new List<T>();

        public void SetAction(T action)
        {
            _actions.Remove(action);
            _actions.Add(action);
        }

        public void RemoveAction(T action)
        {
            _actions.Remove(action);
        }
    }
}