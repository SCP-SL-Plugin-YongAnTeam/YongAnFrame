using Exiled.API.Features;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
