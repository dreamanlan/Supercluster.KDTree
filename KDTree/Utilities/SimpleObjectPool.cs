using System;
using System.Collections.Generic;

namespace Supercluster.KDTree.Utilities
{
    public class SimpleObjectPool<T> where T : new()
    {
        public SimpleObjectPool()
        {
            m_UnusedObjects = new Queue<T>();
        }
        public SimpleObjectPool(int initPoolSize)
        {
            m_UnusedObjects = new Queue<T>(initPoolSize);
            Init(initPoolSize);
        }
        public void Init(int initPoolSize)
        {
            for (int i = 0; i < initPoolSize; ++i) {
                T t = new T();
                Recycle(t);
            }
        }
        public T Alloc()
        {
            if (m_UnusedObjects.Count > 0) {
                var t = m_UnusedObjects.Dequeue();
                m_HashCodes.Remove(t.GetHashCode());
                return t;
            }
            else {
                T t = new T();
                return t;
            }
        }
        public void Recycle(T t)
        {
            if (null != t && m_UnusedObjects.Count < m_PoolSize) {
                int hashCode = t.GetHashCode();
                if (!m_HashCodes.Contains(hashCode)) {
                    m_HashCodes.Add(hashCode);
                    m_UnusedObjects.Enqueue(t);
                }
            }
        }
        public void Clear()
        {
            m_HashCodes.Clear();
            m_UnusedObjects.Clear();
        }
        public int Count
        {
            get {
                return m_UnusedObjects.Count;
            }
        }

        private HashSet<int> m_HashCodes = new HashSet<int>();
        private Queue<T> m_UnusedObjects = new Queue<T>();
        private int m_PoolSize = 4096;
    }
    public class SimpleArrayPool<T>
    {
        public SimpleArrayPool(int arraySize)
        {
            m_ArraySize = arraySize;
            m_UnusedObjects = new Queue<T[]>();
        }
        public SimpleArrayPool(int arraySize, int initPoolSize)
        {
            m_ArraySize = arraySize;
            m_UnusedObjects = new Queue<T[]>(initPoolSize);
            Init(initPoolSize);
        }
        public void Init(int initPoolSize)
        {
            for (int i = 0; i < initPoolSize; ++i) {
                T[] t = new T[m_ArraySize];
                Recycle(t);
            }
        }
        public T[] Alloc()
        {
            if (m_UnusedObjects.Count > 0) {
                var t = m_UnusedObjects.Dequeue();
                m_HashCodes.Remove(t.GetHashCode());
                return t;
            }
            else {
                T[] t = new T[m_ArraySize];
                return t;
            }
        }
        public void Recycle(T[] t)
        {
            if (null != t && m_UnusedObjects.Count < m_PoolSize) {
                int hashCode = t.GetHashCode();
                if (!m_HashCodes.Contains(hashCode)) {
                    m_HashCodes.Add(hashCode);
                    m_UnusedObjects.Enqueue(t);
                }
            }
        }
        public void Clear()
        {
            m_HashCodes.Clear();
            m_UnusedObjects.Clear();
        }
        public int Count
        {
            get {
                return m_UnusedObjects.Count;
            }
        }

        private readonly int m_ArraySize = 2;
        private HashSet<int> m_HashCodes = new HashSet<int>();
        private Queue<T[]> m_UnusedObjects = new Queue<T[]>();
        private int m_PoolSize = 4096;
    }
}
