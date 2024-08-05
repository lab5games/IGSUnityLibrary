
namespace IGS.Unity.Pool
{
    public interface IObjectPool<T> where T : class
    {
        // get an instance from the pool
        T Get();
        // returns a PooledObject that will automatically recycle the instance to the pool when it is disposed
        PooledObject<T> Get(out T obj);
        // recycle the instace back to the pool
        void Recycle(T obj);
        // removed all pooled items
        void Clear();
    }
}
