using System;
using System.Collections.Concurrent;

namespace Secs4Net
{
    internal enum PoolAccessMode { FIFO, LIFO };

    internal sealed class Pool<T> 
    {
        private readonly Func<Pool<T>, T> _factory;
        private readonly IItemStore _itemStore;

        public Pool(Func<Pool<T>, T> factory, PoolAccessMode poolAccessMode = PoolAccessMode.FIFO)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            _factory = factory;
            _itemStore = poolAccessMode == PoolAccessMode.FIFO
                             ? (IItemStore) new QueueStore()
                             : new StackStore();
        }

        public T Rent()
        {
            T item;
            if (_itemStore.Count <= 0 || !_itemStore.TryRent(out item))
                item = _factory(this);
            return item;
        }

        public void Return(T item)
        {
            _itemStore.Return(item);
        }

        public void Reset()
        {
            while (!_itemStore.IsEmpty)
            {
                T item;
                _itemStore.TryRent(out item);
            }
        }

        private interface IItemStore
        {
            bool TryRent(out T item);
            void Return(T item);
            int Count { get; }
            bool IsEmpty { get; }
        }

        private class QueueStore : ConcurrentQueue<T>, IItemStore
        {
            public bool TryRent(out T item) => TryDequeue(out item);

            public void Return(T item) => Enqueue(item);
        }

        private class StackStore : ConcurrentStack<T>, IItemStore
        {
            public bool TryRent(out T item) => TryPop(out item);

            public void Return(T item) => Push(item);
        }
    }
}
