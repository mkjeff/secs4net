using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Secs4Net;

partial class Item
{
    [DebuggerTypeProxy(typeof(ItemDebugView))]
    [SkipLocalsInit]
    private sealed class ListItem : Item
    {
        private readonly Item[] _value;

        internal ListItem(SecsFormat format, Item[] value)
            : base(format)
            => _value = value;

        public sealed override void Dispose()
        {
            foreach (var a in _value)
            {
                a.Dispose();
            }
        }

        public sealed override int Count => _value.Length;

        public sealed override Item this[int index]
        {
            get => _value[index];
            set => _value[index] = value;
        }

        public sealed override Item[] Items => _value;

        public sealed override void EncodeTo(IBufferWriter<byte> buffer)
        {
            var arr = _value;
            if (arr.Length == 0)
            {
                EncodeEmptyItem(Format, buffer);
                return;
            }

            EncodeItemHeader(Format, arr.Length, buffer);
            foreach (var item in arr)
            {
                item.EncodeTo(buffer);
            }
        }

        private protected sealed override bool IsEquals(Item other)
            => Format == other.Format && IsListEquals(_value, Unsafe.As<ListItem>(other)!._value);

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool IsListEquals(Item[] listLeft, Item[] listRight)
        {
            if (listLeft.Length != listRight.Length)
            {
                return false;
            }

            for (int i = 0, count = listLeft.Length; i < count; i++)
            {
                if (!listLeft[i].IsEquals(listRight[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private sealed class ItemDebugView
        {
            private readonly ListItem _item;
            public ItemDebugView(ListItem item)
            {
                _item = item;
                EncodedBytes = new EncodedByteDebugView(item);
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public Item[] Items => _item._value;

            public EncodedByteDebugView EncodedBytes { get; }
        }
    }
}
