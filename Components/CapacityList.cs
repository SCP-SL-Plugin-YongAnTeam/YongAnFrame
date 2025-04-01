using System;
using System.Collections;
using System.Collections.Generic;

namespace YongAnFrame.Components
{
    public class CapacityList<T>(int capacity, Action modify = null) : ICollection<T>, IEnumerable<T>, IEnumerable
    {
        private readonly List<T> list = new(capacity);
        private readonly Action modify = modify;

        public int Capacity { get; set; } = capacity;
        /// <inheritdoc/>
        public int Count => list.Count;
        /// <inheritdoc/>
        public bool IsReadOnly => false;

        public T this[int index]
        {
            get
            {
                if (Count > index)
                {
                    return list[index];
                }
                return default;
            }
            set
            {
                list[index] = value;
            }
        }

        /// <inheritdoc/>
        public void Add(T item)
        {
            if (Capacity > list.Count)
            {
                list.Add(item);
            }
            else
            {
                list.RemoveAt(0);
                list.Add(item);
            }
            modify?.Invoke();
        }

        /// <inheritdoc/>
        public bool Remove(T item)
        {
            bool v = list.Remove(item);
            modify?.Invoke();
            return v;
        }

        /// <inheritdoc/>
        public IEnumerator GetEnumerator() => list.GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => list.GetEnumerator();

        /// <inheritdoc/>
        public void Clear()
        {
            list.Clear();
            modify?.Invoke();
        }

        /// <inheritdoc/>
        public bool Contains(T item) => list.Contains(item);

        /// <inheritdoc/>
        public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

        /// <inheritdoc/>
        public int IndexOf(T item) => list.IndexOf(item);

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
            modify?.Invoke();
        }
    }
}
