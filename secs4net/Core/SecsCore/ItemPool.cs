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
            = new Pool<SecsItem<TFormat, TValue>>(1000, p => new PooledSecsItemWrapper<TItem, TFormat, TValue>(p));

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

    internal sealed class PooledSecsItemWrapper<TItem, TFormat, TValue> : SecsItem<TFormat, TValue>
        where TItem : SecsItem<TFormat, TValue>, new()
        where TFormat : IFormat<TValue>
    {
        internal readonly TItem WrappedItem;
        private readonly IPool<SecsItem<TFormat, TValue>> _pool;

        internal PooledSecsItemWrapper(IPool<SecsItem<TFormat, TValue>> pool)
        {
            if (pool == null)
                throw new ArgumentNullException(nameof(pool));

            _pool = pool;
            WrappedItem = new TItem();
        }

        internal override void ReleaseValue()
        {
            WrappedItem.ReleaseValue();
            _pool.Release(this);
        }

        internal override void SetValue(ArraySegment<TValue> itemValue)
        {
            WrappedItem.SetValue(itemValue);
        }

        internal override void SetValue(string itemValue)
        {
            WrappedItem.SetValue(itemValue);
        }

        protected internal override ArraySegment<byte> GetEncodedData() => WrappedItem.GetEncodedData();
        public override int Count => WrappedItem.Count;
        public override bool IsMatch(SecsItem target) => WrappedItem.IsMatch(target);
        public override string GetString() => WrappedItem.GetString();
        public override T GetValue<T>() => WrappedItem.GetValue<T>();
        public override T[] GetValues<T>() => WrappedItem.GetValues<T>();
        public override IReadOnlyList<SecsItem> Items => WrappedItem.Items;
        public override IEnumerable Values => WrappedItem.Values;
        public override string ToString() => WrappedItem.ToString();
        public override bool Equals(object obj) => WrappedItem.Equals(obj);
        public override int GetHashCode() => WrappedItem.GetHashCode();
    }
}
