using System;
using System.Collections;
using System.Collections.Generic;

namespace YongAnFrame.Components
{
    /// <summary>
    /// 容量列表
    /// </summary>
    /// <typeparam name="T">存储</typeparam>
    /// <param name="capacity">容量</param>
    /// <param name="modify">修改委托</param>
    public class CapacityList<T>(int capacity, Action modify = null) : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
    {
        private readonly List<T> list = new(capacity);
        private readonly Action modify = modify;

        /// <summary>
        /// 获取容量
        /// </summary>
        public int Capacity { get; } = capacity;
        /// <inheritdoc/>
        public int Count => list.Count;
        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
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
        IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

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

        /// <inheritdoc/>
        public void Insert(int index, T item)
        {
            list.Insert(index, item);
            modify?.Invoke();
        }
    }
}
