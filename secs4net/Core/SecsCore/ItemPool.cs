using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Secs4Net
{
    internal sealed class PooledWrappedItem<TItem, TFormat, TValue> : Item<TFormat, TValue>
        where TItem : Item<TFormat, TValue>, new()
        where TFormat : IFormat<TValue>
    {
        private readonly TItem _item;
        private readonly IPool<Item<TFormat, TValue>> _pool;

        internal PooledWrappedItem(IPool<Item<TFormat, TValue>> pool)
        {
            if (pool == null)
                throw new ArgumentNullException(nameof(pool));

            _pool = pool;
            _item = new TItem();
        }

        public override void Dispose()
        {
            if (_pool.IsDisposed)
            {
                _item.Dispose();
            }
            else
            {
                _pool.Release(this);
            }
        }

        internal override void SetValue(ArraySegment<TValue> itemValue)
        {
            _item.SetValue(itemValue);
        }

        internal override void SetValue(string itemValue)
        {
            _item.SetValue(itemValue);
        }

        protected internal override ArraySegment<byte> GetEncodedData() => _item.GetEncodedData();
        public override int Count => _item.Count;
        public override bool IsMatch(Item target) => _item.IsMatch(target);
        public override string GetString() => _item.GetString();
        public override T GetValue<T>() => _item.GetValue<T>();
        public override T[] GetValues<T>() => _item.GetValues<T>();
        public override IReadOnlyList<Item> Items => _item.Items;
        public override IEnumerable Values => _item.Values;
        public override string ToString() => _item.ToString();
        public override bool Equals(object obj) => _item.Equals(obj);
        public override int GetHashCode() => _item.GetHashCode();
    }

    internal static class ItemPool<TItem, TFormat, TValue>
        where TItem : Item<TFormat, TValue>, new()
        where TFormat : IFormat<TValue>
    {
        private static readonly Pool<Item<TFormat, TValue>> Default
            = new Pool<Item<TFormat, TValue>>(1000, p => new PooledWrappedItem<TItem, TFormat, TValue>(p));

        public static TItem Acquire(ArraySegment<TValue> value)
        {
            var valueTypeItem = Default.Acquire();
            valueTypeItem.SetValue(value);
            return Unsafe.As<TItem>(valueTypeItem);
        }

        public static TItem Acquire(string value)
        {
            var valueTypeItem = Default.Acquire();
            valueTypeItem.SetValue(value);
            return Unsafe.As<TItem>(valueTypeItem);
        }
    }
}
