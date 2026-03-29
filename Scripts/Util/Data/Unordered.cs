using System;
using System.Collections;
using System.Collections.Generic;


// FIXME: THis should just be in kfutils, not RPG specfic
namespace kfutils {


    /// <summary>
    /// An unordered version of a list, for times when you need a dynamically sized array 
    /// but don't care about the order.
    /// 
    /// This is primarily for use cases where all items must be access in some way (read, processesed, 
    /// modified, etc.) but is which the order is not important. E.g, with a list of game objects which 
    /// need to run certain code once per frame without the order of which run its code first, especially 
    /// where entries can be added or removed.
    /// 
    /// If and item might be removed during iteration, the developer must remember to acount for this by 
    /// only advancing the index when nothing is removed, OR alternately iterating backward so the entries 
    /// moved from the end will already have been processed.  (Do not use both approachs together as the  
    /// moved entry then be process again.)
    /// 
    /// This will remove items by copying the last item into the index of the removed item and decrimenting 
    /// the count.  This swapping can greatly reduce the number of copies from that needed for traditional 
    /// ordered dynamic array types like Lists, which must copy all subsequent entries to maintain oder. 
    /// 
    /// It should fascillitate faster removal, especially with long data sets, in situation when the 
    /// oder is not important but it must also function like a list. 
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


        private Unordered(int minSize, int size)
        {
            this.minSize = Math.Max(minSize, 2);
            data = new T[Math.Max(minSize, size)];
        }


        private void Expand()
        {
            T[] bigger = new T[(data.Length * 3) / 2];
            Array.Copy(data, 0, bigger, 0, data.Length);
            data = bigger;
        }


        private void Shrink()
        {            
            T[] smaller = new T[Math.Max(data.Length / 2, minSize)];
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
            // Iterating from the end, as the allows some action such as removals; 
            // remember, the order is not supposed to mater for this type, so neither 
            // should starting point.
            for (int i = count - 1; i > -1; i--)
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
            --count;
            data[count] = item;
        }


        public bool Remove(T item)
        {
            for(int i = 0; i < count; i++)
            {
                if(data[i].Equals(item)) {
                    --count;
                    data[i] = data[count];
                    if((count < (data.Length / 4)) && (data.Length > minSize)) Shrink();     
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
                    --count;
                    data[i] = data[count];
                    if((count < (data.Length / 4)) && (data.Length > minSize)) Shrink();     
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
                    --count;
                    data[i] = data[count];
                }
            } 
            if((count < (data.Length / 4)) && (data.Length > minSize)) Shrink(); 
        }


        public void RemoveAll(Predicate<T> predicate)
        {
            for(int i = count - 1; i > -1; i--)
            {
                if(predicate(data[i])) {
                    --count;
                    data[i] = data[count];
                }
            }   
            if((count < (data.Length / 4)) && (data.Length > minSize)) Shrink(); 
        }


        public void RemoveAt(int index)
        {
            if(InBounds(index))
            {
                --count;
                data[index] = data[count];                   
                if((count < (data.Length / 4)) && (data.Length > minSize)) Shrink(); 
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        /// <summary>
        /// Returns a string representation of the contents of the Unordered list.
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


        /// <summary>
        /// Creates a shallow copy of the Unordered list.
        /// </summary>
        /// <returns></returns>
        public Unordered<T> Clone()
        {
            Unordered<T> result = new(minSize, data.Length);
            Array.Copy(data, 0, result.data, 0, count);
            return result;
        }


    }

}
