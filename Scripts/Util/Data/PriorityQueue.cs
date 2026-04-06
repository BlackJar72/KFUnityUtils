using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


// FIXME: THis should just be in kfutils, not RPG specfic
namespace kfutils {


    /// <summary>
    /// A simple priority queue implemented as a binary heap.
    /// 
    /// While recent version of .NET have a standard priority queue, 
    /// these version are not supported by Unity; with Unity there 
    /// is not pre-made standard priority, and priority queues have 
    /// many uses. 
    /// 
    /// In the past I used an free prioriy queue implementation from 
    /// a developer going by Blue Raja: 
    /// 
    /// https://github.com/BlueRaja/High-Speed-Priority-Queue-for-C-Sharp
    /// 
    /// However, I wanted my own that I fully understood and could better 
    /// predict the behavior of, as the one mentioned above and the one 
    /// I was used to from Java did not always seem to produce the same 
    /// result. Also, it seems like a good learning exercise to improve 
    /// my knowledge of algorithms.
    /// 
    /// Examples of algorithms using priority queues include A* pathfinding, 
    /// heep sort, Prims least spanning tree algorithm, Hoffman compression,
    /// and various others (only considering those already invented). Both 
    /// Doomlike Dungeons and Caverns of Evil use priority queues, through 
    /// custum A* implementations, as part of their quality control pass in 
    /// generating dungeons / levels.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityQueue<T> : IEnumerable<T> where T : IComparable 
    {
        private T[] data;
        private int count;
        
        private const int MIN_SIZE = 16;
        private readonly int minSize;

        public int Count => count;
        public bool IsEmpty => count < 1;
        public bool NotEmpty => count > 0;

        public bool IsReadOnly => false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        private int GetParent(int i) => (i - 1) / 2;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        private int GetLeft(int i) => (i * 2) + 1;  // Left child
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        private int GetRight(int i) => (i * 2) + 2; // Right child
        


        public PriorityQueue(int minSize = MIN_SIZE)
        {
            this.minSize = minSize;
            data = new T[minSize];
            count = 0;
        }


        private void Expand()
        {
            T[] bigger = new T[data.Length * 2];
            Array.Copy(data, 0, bigger, 0, data.Length);
            data = bigger;
        }


        private void Shrink()
        {            
            T[] smaller = new T[Math.Max(data.Length / 2, minSize)];
            Array.Copy(data, 0, smaller, 0, smaller.Length);
            data = smaller;        
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        public T Peek() => data[0];


        public void Add(T item)
        {
            if(count >= data.Length) Expand();
            data[count] = item;
            MoveUp(count);
            count++;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        public void Push(T item) => Add(item);


        public T Pop()
        {
            if(count < 1) return default(T);
            T result = data[0];
            --count;
            data[0] = data[count];
            MoveDown(0);
            if((count < (data.Length / 4)) && (data.Length > minSize)) Shrink();
            return result;
        }


        public void Clear()
        {
            count = 0;
            if(data.Length > minSize) data = new T[minSize];
        }


        private void MoveUp(int i)
        {
            T tmp = data[i];
            for(int parent; (i > 0) && (tmp.CompareTo(data[parent = GetParent(i)]) < 0); i = parent)
            {
                data[i] = data[parent];
            }
            data[i] = tmp;
        } 


        private void MoveDown(int i)
        {
            T tmp = data[i];
            int rightChild;
            for(int child; (child = GetLeft(i)) < count; i = child)
            {
                rightChild = child + 1;
                if((rightChild < count) && (data[rightChild].CompareTo(data[child]) < 0)) child = rightChild;
                if(data[child].CompareTo(tmp) > -1) break;
                data[i] = data[child];
            }
            data[i] = tmp;
        }


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


        /****************************************************************************/
        /*        METHODS BELOW WILL MOST LLIKELY ONLY BE USED FOR TESTING          */
        /****************************************************************************/


        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < count; i++ )
            {
                yield return data[i];
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


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

    }

}

