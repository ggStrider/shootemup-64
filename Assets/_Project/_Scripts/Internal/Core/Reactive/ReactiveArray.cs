using System;
using System.Collections.Generic;

namespace Internal.Core.Reactive
{
    public class ReactiveArray<T>
    {
        public ReactiveArray(int size)
        {
            _array = new T[size];
        }

        public ReactiveArray(T[] source)
        {
            CopyArray(source);
        }

        private T[] _array;

        public T[] Array
        {
            get => _array;
            set
            {
                CopyArray(value);
            }
        }

        public T this[int index]
        {
            get => _array[index];
            set
            {
                SetValueInIndex(index, value);
            }
        }

        /// <summary>
        /// Invokes when Array element any value changed
        /// T1 = Old Value; T2 = New Value, T3(int) = replaced index
        /// </summary>
        public event Action<T, T, int> OnArrayValueChanged;

        /// <summary>
        /// Invokes when variable value changed
        /// T1 = Old Array; T2 = New Array
        /// </summary>
        public event Action<T[], T[]> OnArrayReplaced;

        private void CopyArray(T[] source, bool isSilent = false)
        {
            var oldArray = _array != null ? new T[_array.Length] : System.Array.Empty<T>();
            if (_array != null && !isSilent) System.Array.Copy(_array, oldArray, _array.Length);

            _array = new T[source.Length];
            System.Array.Copy(source, _array, source.Length);

            if (!isSilent) OnArrayReplaced?.Invoke(oldArray, _array);
        }

        private void SetValueInIndex(int index, T newValue, bool isSilent = false)
        {
            if (EqualityComparer<T>.Default.Equals(_array[index], newValue)) return;

            var oldValue = _array[index];
            _array[index] = newValue;

            if (!isSilent)
                OnArrayValueChanged?.Invoke(oldValue, newValue, index);
        }

        public void CopyArraySilent(T[] source) =>
            CopyArray(source, isSilent: true);

        public void SetValueInIndexSilent(int index, T newValue) =>
            SetValueInIndex(index, newValue);
    }
}