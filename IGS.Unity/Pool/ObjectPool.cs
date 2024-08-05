using System;
using System.Collections.Generic;

namespace IGS.Unity.Pool
{
    public class ObjectPool<T> : IDisposable, IObjectPool<T> where T : class
    {
        internal readonly List<T> _pooledItems = null;
        readonly Func<T> _onCreate;
        readonly Action<T> _onGet;
        readonly Action<T> _onRecycle;
        readonly Action<T> _onRelease;
        readonly int _maxSize;
        T _lastRecycledObject = null;
        

        public int CountAll { get; private set; }

        public int CountInactive { get { return _pooledItems.Count + (_lastRecycledObject.IsNull() ? 0 : 1); } }

        public int CountActive { get { return CountAll - CountInactive; } }

        public ObjectPool(Func<T> onCreate, Action<T> onGet = null, Action<T> onRecycle = null, Action<T> onRelease = null, int defaultSize = 10, int maxSize = 1000)
        {
            if(onCreate == null)
                throw new ArgumentNullException("onCreate");

            if(maxSize <= 0)
                throw new ArgumentException("Max size must be greater than 0", "maxSize");

            // initialize
            _pooledItems = new List<T>(defaultSize <= 0 ? 10 : defaultSize);
            _onCreate = onCreate;
            _onGet = onGet;
            _onRecycle = onRecycle;
            _onRelease = onRelease;
            _maxSize = maxSize;
        }

        public T Get()
        {
            T element = null;

            if(_lastRecycledObject != null)
            {
                element = _lastRecycledObject;
                _lastRecycledObject = null;
            }
            else if(_pooledItems.Count == 0)
            {
                element = _onCreate();
                CountAll++;
            }
            else
            {
                int lastIndx = _pooledItems.Count - 1;
                element = _pooledItems[lastIndx];
                _pooledItems.RemoveAt(lastIndx);
            }

            if(_onGet != null)
                _onGet(element);

            return element;
        }

        public PooledObject<T> Get(out T obj)
        {
            obj = Get();
            return new PooledObject<T>(obj, this);
        }

        public void Recycle(T obj)
        {
            if(HasElement(obj))
            {
                throw new InvalidOperationException("Trying to recycle an object that has already been recycled");
            }

            if(_onRecycle != null)
                _onRecycle(obj);

            if(_lastRecycledObject.IsNull())
            {
                _lastRecycledObject = obj;
            }
            else if(CountInactive < _maxSize)
            {
                _pooledItems.Add(obj);
            }
            else
            {
                CountAll--;

                if(_onRelease != null)
                    _onRelease(obj);
            }
        }

        public void Clear()
        {
            if(_onRelease != null)
            {
                _pooledItems.ForEach(x => _onRelease(x));

                if(_lastRecycledObject != null)
                    _onRelease(_lastRecycledObject);
            }

            _lastRecycledObject = null;
            _pooledItems.Clear();
            CountAll = 0;
        }

        public void Dispose()
        {
            Clear();
        }

        internal bool HasElement(T element)
        {
            return _lastRecycledObject.SafeEquals(element) || _pooledItems.Contains(element);
        }
    }
}
