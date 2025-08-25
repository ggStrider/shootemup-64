using System;
using System.Collections.Generic;

namespace Internal.Core.Reactive
{
    public class ReactiveList<T>
    {
        public ReactiveList(int size)
        {
            _list = new List<T>(size);
        }

        public ReactiveList(List<T> source)
        {
            CopyList(source);
        }

        private List<T> _list;

        public List<T> List
        {
            get => _list;
            set
            {
                CopyList(value);
            }
        }

        public T this[int index]
        {
            get => _list[index];
            set
            {
                SetValueInIndex(index, value);
            }
        }

        /// <summary>
        /// Invokes when value in List changed
        /// T1 = Old Value; T2 = New Value, T3(int) = where was replaced (index)
        /// </summary>
        public event Action<T, T, int> OnListValueChanged;

        /// <summary>
        /// Invokes when variable value changed
        /// T1 = Old Array; T2 = New Array
        /// </summary>
        public event Action<List<T>, List<T>> OnListReplaced;

        private void CopyList(List<T> source, bool isSilent = false)
        {
            var oldList = _list != null ? new List<T>(_list) : new List<T>();
            _list = new List<T>(source);

            if (!isSilent) OnListReplaced?.Invoke(oldList, _list);
        }

        private void SetValueInIndex(int index, T newValue, bool isSilent = false)
        {
            if (EqualityComparer<T>.Default.Equals(_list[index], newValue)) return;

            var oldValue = _list[index];
            _list[index] = newValue;

            if (!isSilent)
                OnListValueChanged?.Invoke(oldValue, newValue, index);
        }

        public void CopyListSilent(List<T> source) =>
            CopyList(source, isSilent: true);

        public void SetValueInIndexSilent(int index, T newValue) =>
            SetValueInIndex(index, newValue);
    }
}