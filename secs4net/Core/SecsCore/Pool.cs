using System;
using System.Collections.Generic;
using System.Threading;
using static System.Diagnostics.Debug;

namespace Secs4Net
{
    internal enum LoadingMode { Eager, Lazy, LazyExpanding };

    internal enum PoolAccessMode { FIFO, LIFO, Circular };

    internal interface IPool<in T> : IDisposable
    {
        void Release(T item);
        bool IsDisposed { get; }
    }

    internal sealed class Pool<T> : IPool<T>
    {
        private readonly Func<IPool<T>, T> _factory;
        private readonly LoadingMode _loadingMode;
        private readonly IItemStore _itemStore;
        private readonly int _size;
        private int _count;
        private readonly Semaphore _sync;
        public bool IsDisposed { get; private set; }

        public Pool(int size, Func<IPool<T>, T> factory,
            LoadingMode loadingMode = LoadingMode.Lazy, PoolAccessMode poolAccessMode = PoolAccessMode.FIFO)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size), size,
                    "Argument 'size' must be greater than zero.");
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            _size = size;
            _factory = factory;
            _sync = new Semaphore(size, size);
            _loadingMode = loadingMode;
            _itemStore = CreateItemStore(poolAccessMode, size);
            if (loadingMode == LoadingMode.Eager)
            {
                PreloadItems();
            }
        }

        public T Acquire()
        {
            _sync.WaitOne();
            switch (_loadingMode)
            {
                case LoadingMode.Eager:
                    return AcquireEager();
                case LoadingMode.Lazy:
                    return AcquireLazy();
                default:
                    Assert(_loadingMode == LoadingMode.LazyExpanding,
                        "Unknown LoadingMode encountered in Acquire method.");
                    return AcquireLazyExpanding();
            }
        }

        public void Release(T item)
        {
            lock (_itemStore)
            {
                _itemStore.Store(item);
            }
            _sync.Release();
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
                lock (_itemStore)
                {
                    while (_itemStore.Count > 0)
                    {
                        var disposable = (IDisposable)_itemStore.Fetch();
                        disposable.Dispose();
                    }
                }
            }
            _sync.Close();
        }

        #region Acquisition

        private T AcquireEager()
        {
            lock (_itemStore)
            {
                return _itemStore.Fetch();
            }
        }

        private T AcquireLazy()
        {
            lock (_itemStore)
            {
                if (_itemStore.Count > 0)
                {
                    return _itemStore.Fetch();
                }
            }
            Interlocked.Increment(ref _count);
            return _factory(this);
        }

        private T AcquireLazyExpanding()
        {
            bool shouldExpand = false;
            if (_count < _size)
            {
                int newCount = Interlocked.Increment(ref _count);
                if (newCount <= _size)
                {
                    shouldExpand = true;
                }
                else
                {
                    // Another thread took the last spot - use the store instead
                    Interlocked.Decrement(ref _count);
                }
            }
            if (shouldExpand)
            {
                return _factory(this);
            }
            else
            {
                lock (_itemStore)
                {
                    return _itemStore.Fetch();
                }
            }
        }

        private void PreloadItems()
        {
            for (int i = 0; i < _size; i++)
            {
                T item = _factory(this);
                _itemStore.Store(item);
            }
            _count = _size;
        }

        #endregion

        #region Collection Wrappers

        private interface IItemStore
        {
            T Fetch();
            void Store(T item);
            int Count { get; }
        }

        private static IItemStore CreateItemStore(PoolAccessMode mode, int capacity)
        {
            switch (mode)
            {
                case PoolAccessMode.FIFO:
                    return new QueueStore(capacity);
                case PoolAccessMode.LIFO:
                    return new StackStore(capacity);
                default:
                    Assert(mode == PoolAccessMode.Circular,
                        "Invalid AccessMode in CreateItemStore");
                    return new CircularStore(capacity);
            }
        }

        private class QueueStore : Queue<T>, IItemStore
        {
            public QueueStore(int capacity) : base(capacity)
            {
            }

            public T Fetch() => Dequeue();

            public void Store(T item)
            {
                Enqueue(item);
            }
        }

        private class StackStore : Stack<T>, IItemStore
        {
            public StackStore(int capacity) : base(capacity)
            {
            }

            public T Fetch() => Pop();

            public void Store(T item)
            {
                Push(item);
            }
        }

        private class CircularStore : IItemStore
        {
            private readonly List<Slot> _slots;
            private int _freeSlotCount;
            private int _position = -1;

            public CircularStore(int capacity)
            {
                _slots = new List<Slot>(capacity);
            }

            public T Fetch()
            {
                if (Count == 0)
                    throw new InvalidOperationException("The buffer is empty.");

                int startPosition = _position;
                do
                {
                    Advance();
                    var slot = _slots[_position];
                    if (!slot.IsInUse)
                    {
                        slot.IsInUse = true;
                        --_freeSlotCount;
                        return slot.Item;
                    }
                } while (startPosition != _position);
                throw new InvalidOperationException("No free slots.");
            }

            public void Store(T item)
            {
                var slot = _slots.Find(s => object.Equals(s.Item, item));
                if (slot == null)
                {
                    slot = new Slot(item);
                    _slots.Add(slot);
                }
                slot.IsInUse = false;
                ++_freeSlotCount;
            }

            public int Count => _freeSlotCount;

            private void Advance()
            {
                _position = (_position + 1) % _slots.Count;
            }

            private class Slot
            {
                public Slot(T item)
                {
                    Item = item;
                }

                public T Item { get; }
                public bool IsInUse { get; set; }
            }
        }

        #endregion
    }
}
