using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Secs4Net
{
    internal sealed class ListItem : SecsItem<ListFormat, SecsItem>
    {
        private readonly Pool<ListItem> _pool;
        private ArraySegment<SecsItem> _items = new ArraySegment<SecsItem>(Array.Empty<SecsItem>());
        private bool _isItemsFromPool;

        internal ListItem(Pool<ListItem> pool = null) => _pool = pool;

        internal ListItem SetItems(ArraySegment<SecsItem> items, bool fromPool)
        {
            if (items.Count > byte.MaxValue)
                throw new ArgumentOutOfRangeException($"List length out of range, max length: 255");
            _items = items;
            _isItemsFromPool = fromPool;
            return this;
        }

        internal override void Release()
        {
            foreach (var item in _items)
                item.Release();

            _pool?.Return(this);

            ReturnItemArray();
        }

        private void ReturnItemArray()
        {
            if (_isItemsFromPool)
                SecsItemArrayPool.Pool.Return(_items.Array);
        }

        ~ListItem() => ReturnItemArray();

        protected override ArraySegment<byte> GetEncodedData()
        {
            var arr = SecsGem.EncodedBytePool.Rent(2);
            arr[0] = (byte)SecsFormat.List | 1;
            arr[1] = unchecked((byte)_items.Count);
            return new ArraySegment<byte>(arr, 0, 2);
        }

        public override int Count => _items.Count;
        public override IReadOnlyList<SecsItem> Items => _items;
        public override string ToString() => $"<List [{_items.Count}] >";

        public override bool IsMatch(SecsItem target)
            => ReferenceEquals(this, target)
               || Format == target.Format
               && (target.Count == 0
                   || Count == target.Count
                   && IsMatch(_items.Array,
                       Unsafe.As<ListItem>(target)
                             ._items.Array,
                       Count));

        private static bool IsMatch(SecsItem[] a, SecsItem[] b, int count)
        {
            for (var i = 0; i < count; i++)
                if (!a[i].IsMatch(b[i]))
                    return false;
            return true;
        }
    }
}
