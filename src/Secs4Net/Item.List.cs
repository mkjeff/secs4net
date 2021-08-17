using Microsoft.Toolkit.HighPerformance;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Secs4Net;

partial class Item
{
    [DebuggerTypeProxy(typeof(ItemDebugView))]
    private sealed class ListItem : Item
    {
        private readonly Item[] _value;

        internal ListItem(SecsFormat format, Item[] value)
            : base(format)
            => _value = value;

        public sealed override void Dispose()
        {
            var arr = _value;
#if NET
            ref var head = ref MemoryMarshal.GetArrayDataReference(arr);
            for (nint i = 0, count = arr.Length; i < count; i++)
            {
                Unsafe.Add(ref head, i).Dispose();
            }
#else
            for (int i = 0; i < arr.Length; i++)
            {
                arr.DangerousGetReferenceAt(i).Dispose();
            }
#endif
        }

        public sealed override int Count => _value.Length;

        public sealed override Item this[int index]
        {
            get => _value[index];
            set => _value[index] = value;
        }

        public sealed override IEnumerable<Item> Slice(int start, int length)
        {
            if (start < 0 || start + length > _value.Length)
            {
                throw new IndexOutOfRangeException($"Item.Count is {_value.Length}, but Slice(start: {start}, length: {length})");
            }
            return _value.Skip(start).Take(length);
        }

        public sealed override IEnumerator<Item> GetEnumerator()
            => ((IEnumerable<Item>)_value).GetEnumerator();

        public sealed override void EncodeTo(IBufferWriter<byte> buffer)
        {
            var arr = _value;
            if (arr.Length == 0)
            {
                EncodeEmptyItem(Format, buffer);
                return;
            }

            EncodeItemHeader(Format, arr.Length, buffer);
#if NET
            ref var head = ref MemoryMarshal.GetArrayDataReference(arr);
            for (nint i = 0, count = arr.Length; i < count; i++)
            {
                Unsafe.Add(ref head, i).EncodeTo(buffer);
            }
#else
            for (int i = 0; i < arr.Length; i++)
            {
                arr.DangerousGetReferenceAt(i).EncodeTo(buffer);
            }
#endif
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
#if NET
            ref var headLeft = ref MemoryMarshal.GetArrayDataReference(listLeft);
            ref var headRight = ref MemoryMarshal.GetArrayDataReference(listRight);
            for (nint i = 0, count = listLeft.Length; i < count; i++)
            {
                if (!Unsafe.Add(ref headLeft, i).IsEquals(Unsafe.Add(ref headRight, i)))
                {
                    return false;
                }
            }
#else
            for (int i = 0, count = listLeft.Length; i < count; i++)
            {
                if (!listLeft.DangerousGetReferenceAt(i).IsEquals(listRight.DangerousGetReferenceAt(i)))
                {
                    return false;
                }
            }
#endif
            return true;
        }

        private sealed class ItemDebugView
        {
            private readonly ListItem _item;
            public ItemDebugView(ListItem item)
            {
                _item = item;
                EncodedBytes = new EncodedByteDebugProxy(item);
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public Item[] Items => _item._value;

            public EncodedByteDebugProxy EncodedBytes { get; }
        }
    }
}
