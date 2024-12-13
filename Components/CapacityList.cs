using System;
using System.Collections;
using System.Collections.Generic;

namespace YongAnFrame.Components
{
    public class CapacityList<T>(int capacity) : ICollection<T>, IEnumerable<T>, IEnumerable
    {
        private readonly List<T> list = new(capacity);

        public int Capacity { get; set; } = capacity;

        public int Count => list.Count;

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
        }

        public bool Remove(T item)
        {
            return list.Remove(item);
        }

        public IEnumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public int IndexOf(T item)
        {
            return list.IndexOf(item);
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }
    }
}
