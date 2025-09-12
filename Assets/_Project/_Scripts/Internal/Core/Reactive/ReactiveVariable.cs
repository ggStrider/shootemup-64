using System;
using System.Collections.Generic;
using UnityEngine;

namespace Internal.Core.Reactive
{
    [Serializable]
    public class ReactiveVariable<T> : IDisposable
    {
        [SerializeField] private T _value;
        
        public ReactiveVariable(T value = default)
        {
            _value = value;
        }
        
        public T Value
        {
            get => _value;
            set
            {
                SetValue(value);
            }
        }

        /// <summary>
        /// Invokes when variable value changed.
        /// T1 = Old Value; T2 = New Value
        /// </summary>
        public event Action<T, T> OnValueChanged;

        private void SetValue(T value, bool silentSet = false)
        {
            if (EqualityComparer<T>.Default.Equals(_value, value)) return;

            var oldValue = _value;
            _value = value;

            if (!silentSet) OnValueChanged?.Invoke(oldValue, value);
        }

        public void SetValueSilent(T newValue) => SetValue(newValue, silentSet: true);

        public void Dispose()
        {
            OnValueChanged = null;
        }

        public static implicit operator T(ReactiveVariable<T> reactiveVariable)
        {
            return reactiveVariable.Value;
        }
    }
}