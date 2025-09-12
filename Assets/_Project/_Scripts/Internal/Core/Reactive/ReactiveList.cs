using System;
using System.Collections.Generic;
using UnityEngine;

namespace Internal.Core.Reactive
{
    [Serializable]
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

        public ReactiveList()
        {
            _list = new();
        }

        [SerializeField] private List<T> _list;

        public List<T> List
        {
            get => _list;
            set { CopyList(value); }
        }

        public T this[int index]
        {
            get => _list[index];
            set { SetValueInIndex(index, value); }
        }

        /// <summary>
        /// Invokes when value in List changed
        /// T1 = Old Value; T2 = New Value, T3(int) = where was replaced (index)
        /// </summary>
        public event Action<T, T, int> OnItemChanged;

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
                OnItemChanged?.Invoke(oldValue, newValue, index);
        }

        public void CopyListSilent(List<T> source) =>
            CopyList(source, isSilent: true);

        public void SetValueInIndexSilent(int index, T newValue) =>
            SetValueInIndex(index, newValue);

        /// <summary>
        /// Invokes when an item is added to the list.
        /// T1(int) = Index of the added item; T2(T) = The item added to the list.
        /// </summary>
        public event Action<int, T> OnItemAdded;

        public event Action<T> OnItemRemoved;

        /// Adds an item to the list and optionally triggers the OnListCountChanged event.
        /// <param name="item">
        /// The item of type T to be added to the list.
        /// </param>
        /// <param name="isSilent">
        /// A boolean value indicating whether the action should suppress triggering the OnListCountChanged event.
        /// If true, the event will not be triggered; otherwise, it will.
        /// </param>
        public void Add(T item, bool isSilent = false)
        {
            _list.Add(item);
            var index = _list.Count - 1;

            if (!isSilent)
            {
                OnItemAdded?.Invoke(index, item);
                // OnListReplaced?.Invoke(new List<T>(_list.GetRange(0, index)), new List<T>(_list));
            }
        }

        public void Remove(T item, bool isSilent = false)
        {
            var removed = _list.Remove(item);
            
            if (removed && !isSilent)
            {
                OnItemRemoved?.Invoke(item);
            }
        }

        public static implicit operator List<T>(ReactiveList<T> list)
        {
            return list.List;
        }

        public int Count => _list.Count;

        public bool Contains(T obj)
        {
            return List.Contains(obj);
        }
    }
}