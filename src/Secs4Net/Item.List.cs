using System.Buffers;
using System.Runtime.CompilerServices;

namespace Secs4Net;

partial class Item
{
    [DebuggerTypeProxy(typeof(ItemDebugView))]
    [SkipLocalsInit]
    private sealed class ListItem : Item
    {
        private readonly Item[] _value;

        internal ListItem(Item[] value)
            : base(SecsFormat.List)
            => _value = value;

        public override void Dispose()
        {
            foreach (ref readonly var a in _value.AsSpan())
            {
                a.Dispose();
            }
        }

        public override int Count => _value.Length;

        public override Item this[int index]
        {
            get => _value[index];
            set => _value[index] = value;
        }

        public override Item[] Items => _value;

        public override void EncodeTo(IBufferWriter<byte> buffer)
        {
            var arr = _value.AsSpan();
            if (arr.Length == 0)
            {
                EncodeEmptyItem(Format, buffer);
                return;
            }

            EncodeItemHeader(Format, arr.Length, buffer);
            foreach (ref readonly var item in arr)
            {
                item.EncodeTo(buffer);
            }
        }

        private protected override bool IsEquals(Item other)
            => Format == other.Format && IsListEquals(_value, Unsafe.As<ListItem>(other)._value);

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

        private sealed class ItemDebugView(ListItem item)
        {

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public Item[] Items => item._value;

            public EncodedByteDebugView EncodedBytes { get; } = new EncodedByteDebugView(item);
        }
    }
}
