using System;
using System.Collections.Concurrent;

namespace Secs4Net
{
    internal enum PoolAccessMode { FIFO, LIFO };


    internal sealed class Pool<T> 
    {
        private readonly Func<Pool<T>, T> _factory;
        private readonly IItemStore _itemStore;
        public bool IsDisposed { get; private set; }

        public Pool(int size, Func<Pool<T>, T> factory, PoolAccessMode poolAccessMode = PoolAccessMode.FIFO)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            _factory = factory;

            _itemStore = poolAccessMode == PoolAccessMode.FIFO
                             ? (IItemStore) new QueueStore()
                             : new StackStore();
        }

        public T Acquire()
        {
            T item;
            if (_itemStore.Count > 0)
            {
                _itemStore.Fetch(out item);
            }
            else
            {
                item = _factory(this);
            }
            return item;
        }

        public void Release(T item)
        {
            _itemStore.Store(item);
        }

        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            IsDisposed = true;
            if (typeof(IDisposable).IsAssignableFrom(typeof(T)))
            {
                while (_itemStore.Count > 0)
                {
                    T disposable;
                    _itemStore.Fetch(out disposable);
                    ((IDisposable) disposable).Dispose();
                }
            }
        }

        #region Collection Wrappers

        private interface IItemStore
        {
            void Fetch(out T item);
            void Store(T item);
            int Count { get; }
        }

        private static IItemStore CreateItemStore(PoolAccessMode mode)
        {
            switch (mode)
            {
                case PoolAccessMode.FIFO:
                    return new QueueStore();
                //case PoolAccessMode.LIFO:
                default:
                    return new StackStore();
            }
        }

        private class QueueStore : ConcurrentQueue<T>, IItemStore
        {
            public void Fetch(out T item) => TryDequeue(out item);

            public void Store(T item)
            {
                Enqueue(item);
            }
        }

        private class StackStore : ConcurrentStack<T>, IItemStore
        {
            public void Fetch(out T item) => TryPop(out item);

            public void Store(T item)
            {
                Push(item);
            }
        }

        #endregion
    }
}
