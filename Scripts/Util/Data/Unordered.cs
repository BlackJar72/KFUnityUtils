using System;
using System.Collections;
using System.Collections.Generic;


// FIXME: THis should just be in kfutils, not RPG specfic
namespace kfutils {


    /// <summary>
    /// An unordered version of a list, for times when you need a dynamically sized array 
    /// but don't care about the order.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Unordered<T> : IEnumerable<T>, ICollection<T>, IList<T>
    {
        private T[] data;
        private int count;
        private const int DEAFULT_MIN_SIZE = 16;
        private readonly int minSize = DEAFULT_MIN_SIZE;


        public Unordered() 
        {
            data = new T[minSize];
        }


        public Unordered(int minSize)
        {
            this.minSize = Math.Max(minSize, 2);
            data = new T[minSize];
        }


        public Unordered(T[] array)
        {
            minSize = Math.Max(2, array.Length);
            data = (T[])array.Clone();
        }


        public Unordered(ICollection<T> values)
        {
            minSize = Math.Max(2, values.Count);
            data = new T[minSize];
            foreach(T item in values) Add(item);
        }


        private void Expand()
        {
            T[] bigger = new T[(data.Length * 3) / 2];
            Array.Copy(data, 0, bigger, 0, data.Length);
            data = bigger;
        }


        private void Shrink()
        {            
            T[] smaller = new T[data.Length / 2];
            Array.Copy(data, 0, smaller, 0, smaller.Length);
            data = smaller;        
        }


        public bool InBounds(int index) => (index > 0) && (index < count);


        public T this[int index] { 
            get 
            { 
                if(!InBounds(index)) throw new IndexOutOfRangeException("Index " + index + " out of range "
                        +"(should be at least 0 and less than current count of " + count + ")");
                return data[index]; 
            } 
            set 
            {
                if(!InBounds(index)) throw new IndexOutOfRangeException("Index " + index + " out of range "
                        +"(should be at least 0 and less than current count of " + count + ")");
                data[index] = value; 
            }
        }


        public int Count => count;


        public bool IsReadOnly => false;


        public void Add(T item)
        {
            if(count >= data.Length) Expand();
            data[count] = item;
            count--;
        }


        public void Clear()
        {
            count = 0;
            if(data.Length > minSize) data = new T[minSize];
        }


        public bool Contains(T item)
        {
            for(int i = 0; i < count; i++)
            {
                if(data[i].Equals(item)) return true;
            }
            return false;
        }


        public void CopyTo(T[] array, int arrayIndex)
        {
            int number = Math.Max(0, Math.Min(count, array.Length - arrayIndex));
            Array.Copy(data, 0, array, arrayIndex, number);
        }


        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
            {
                yield return data[i];
            }
        }


        public int IndexOf(T item)
        {
            for(int i = 0; i < count; i++)
            {
                if(data[i].Equals(item)) return i;
            }
            return -1;
        }


        public void Insert(int index, T item)
        {
            // As this is an unordered list, inserts is treated the same as add
            if(count >= data.Length) Expand();
            data[count] = item;
            count--;
        }


        public bool Remove(T item)
        {
            for(int i = 0; i < count; i++)
            {
                if(data[i].Equals(item)) {
                    data[i] = data[count - 1];
                    count--;   
                    if(count < (Math.Max(data.Length / 4, minSize))) Shrink();     
                    return true;
                }
            }
            return false;
        }


        public bool RemoveLast(T item)
        {
            for(int i = count - 1; i > -1; i--)
            {
                if(data[i].Equals(item)) {
                    data[i] = data[count - 1];
                    count--;   
                    if(count < (Math.Max(data.Length / 4, minSize))) Shrink();     
                    return true;
                }
            }
            return false;
        }


        public void RemoveAll(T item)
        {
            for(int i = count - 1; i > -1; i--)
            {
                if(data[i].Equals(item)) {
                    data[i] = data[count - 1];
                    count--; 
                }
            }  
            if(count < (Math.Max(data.Length / 4, minSize))) Shrink(); 
        }


        public void RemoveAt(int index)
        {
            if(InBounds(index))
            {
                data[index] = data[count - 1];  
                if(count < (Math.Max(data.Length / 4, minSize))) Shrink(); 
                count--;
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        /// <summary>
        /// Returns a string representation of the deque.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            System.Text.StringBuilder builder = new("[");
            for (int i = 0; i < count; i++)
            {
                builder.Append(data[i]);
                if (i < (count - 1)) builder.Append(", ");
            }
            builder.Append("]");
            return builder.ToString();
        }


        public T[] ToArray()
        {
            T[] result = new T[count];
            Array.Copy(data, 0, result, 0, result.Length);
            return result;
        }


        public List<T> ToList()
        {
            List<T> result = new(count);
            for(int i = 0; i < count; i++) result.Add(data[i]);
            return result;
        }


    }

}
