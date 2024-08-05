using System;

namespace IGS.Unity.Pool
{
    public struct PooledObject<T> : IDisposable where T : class
    {
        T _object;
        IObjectPool<T> _pool;

        internal PooledObject(T obj, IObjectPool<T> pool)
        {
            _object = obj;
            _pool = pool;
        }

        public void Dispose()
        {
            _pool.Recycle(_object);
        }
    }
}
