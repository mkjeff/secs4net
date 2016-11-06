using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Secs4Net
{
    internal static class ItemPool<TItem, TFormat, TValue>
        where TItem : SecsItem<TFormat, TValue>, new()
        where TFormat : IFormat<TValue>
    {
        private static readonly Pool<SecsItem<TFormat, TValue>> Default
            = new Pool<SecsItem<TFormat, TValue>>(1000, p => new SecsItemPooledWrapper<TItem, TFormat, TValue>(p));

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

    internal sealed class SecsItemPooledWrapper<TItem, TFormat, TValue> : SecsItem<TFormat, TValue>
        where TItem : SecsItem<TFormat, TValue>, new()
        where TFormat : IFormat<TValue>
    {
        internal readonly TItem Item;
        private readonly IPool<SecsItem<TFormat, TValue>> _pool;

        internal SecsItemPooledWrapper(IPool<SecsItem<TFormat, TValue>> pool)
        {
            if (pool == null)
                throw new ArgumentNullException(nameof(pool));

            _pool = pool;
            Item = new TItem();
        }

        public override void Dispose()
        {
            if (_pool.IsDisposed)
            {
                Item.Dispose();
            }
            else
            {
                _pool.Release(this);
            }
        }

        internal override void SetValue(ArraySegment<TValue> itemValue)
        {
            Item.SetValue(itemValue);
        }

        internal override void SetValue(string itemValue)
        {
            Item.SetValue(itemValue);
        }

        protected internal override ArraySegment<byte> GetEncodedData() => Item.GetEncodedData();
        public override int Count => Item.Count;
        public override bool IsMatch(SecsItem target) => Item.IsMatch(target);
        public override string GetString() => Item.GetString();
        public override T GetValue<T>() => Item.GetValue<T>();
        public override T[] GetValues<T>() => Item.GetValues<T>();
        public override IReadOnlyList<SecsItem> Items => Item.Items;
        public override IEnumerable Values => Item.Values;
        public override string ToString() => Item.ToString();
        public override bool Equals(object obj) => Item.Equals(obj);
        public override int GetHashCode() => Item.GetHashCode();
    }
}
