using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Secs4Net
{
    internal class ListItem : SecsItem<ListFormat, SecsItem>
    {
        private readonly Pool<SecsItem<ListFormat, SecsItem>> _pool;
        protected ArraySegment<SecsItem> list = new ArraySegment<SecsItem>(Array.Empty<SecsItem>());

        internal ListItem(Pool<SecsItem<ListFormat, SecsItem>> pool = null)
        {
            _pool = pool;
        }

        internal override void Release()
        {
            foreach (var item in list)
                item.Release();

            _pool?.Release(this);
        }

        internal sealed override void SetValue(ArraySegment<SecsItem> items)
        {
            Debug.Assert(items.Count <= byte.MaxValue, $"List length out of range, max length: 255");
            list = items;
        }

        protected internal sealed override ArraySegment<byte> GetEncodedData()
        {
            var arr = SecsGem.EncodedBytePool.Rent(2);
            arr[0] = (byte)SecsFormat.List | 1;
            arr[1] = unchecked((byte)list.Count);
            return new ArraySegment<byte>(arr, 0, 2);
        }

        public sealed override int Count => list.Count;
        public sealed override IReadOnlyList<SecsItem> Items => list;
        public sealed override string ToString() => $"<List [{list.Count}] >";

        public sealed override bool IsMatch(SecsItem target)
        {
            if (ReferenceEquals(this, target))
                return true;

            if (Format != target.Format)
                return false;

            if (target.Count == 0)
                return true;

            if (Count != target.Count)
                return false;

            return IsMatch(list.Array,
                           Unsafe.As<ListItem>(target).list.Array,
                           Count);
        }

        private static bool IsMatch(SecsItem[] a, SecsItem[] b, int count)
        {
            for (var i = 0; i < count; i++)
                if (!a[i].IsMatch(b[i]))
                    return false;
            return true;
        }
    }

    internal sealed class PooledListItem : ListItem
    {
        internal PooledListItem(Pool<SecsItem<ListFormat, SecsItem>> pool)
            : base(pool)
        {
        }

        internal override void Release()
        {
            base.Release();
            SecsItemArrayPool.Pool.Return(list.Array);
        }
    }
}
