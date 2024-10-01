using Exiled.API.Features;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YongAnFrame.Components
{
    public class CapacityList<T>(int capacity) : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable 
    {
        private readonly List<T> list = new(capacity);

        public int Capacity { get; set; } = capacity;

        public int Count => list.Count;

        public bool IsReadOnly => ((ICollection<T>)list).IsReadOnly;

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

            }
        }

        public int IndexOf(T item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        public void Add(T item)
        {
            if (Capacity <= list.Count)
            {
                list.Add(item);
            }
            else
            {
                list.RemoveAt(0);
                list.Add(item);
            }
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

        public bool Remove(T item)
        {
            return list.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IEnumerable<T>)list).GetEnumerator();
        }
    }
}
