using System.Collections.Generic;

namespace YongAnFrame.Components
{
    public class CapacityList<T>(int capacity)
    {
        private readonly List<T> list = new(capacity);

        public int Capacity { get; set; } = capacity;

        public int Count => list.Count;

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
    }
}
