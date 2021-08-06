using Microsoft.Toolkit.HighPerformance;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Secs4Net
{
    partial class Item
    {
        private sealed class ListItem : Item
        {
            private readonly Item[] _value;

            internal ListItem(SecsFormat format, Item[] value)
                : base(format)
                => _value = value;

            public sealed override void Dispose()
            {
                for (int i = 0; i < _value.Length; i++)
                {
                    _value.DangerousGetReferenceAt(i).Dispose();
                }

                GC.SuppressFinalize(this);
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

            public sealed override IEnumerator<Item> GetEnumerator() => ((IEnumerable<Item>)_value).GetEnumerator();

            public sealed override void EncodeTo(IBufferWriter<byte> buffer)
            {
                if (_value.Length == 0)
                {
                    EncodeEmptyItem(Format, buffer);
                    return;
                }

                EncodeItemHeader(Format, _value.Length, buffer);
                for (int i = 0; i < _value.Length; i++)
                {
                    _value.DangerousGetReferenceAt(i).EncodeTo(buffer);
                }
            }

            private protected sealed override bool IsEquals(Item other)
                => Format == other.Format && IsListEquals(_value, Unsafe.As<ListItem>(other)!._value);

            static bool IsListEquals(Item[] listLeft, Item[] listRight)
            {
                for (int i = 0, count = listLeft.Length; i < count; i++)
                {
                    if (!listLeft.DangerousGetReferenceAt(i).IsEquals(listRight.DangerousGetReferenceAt(i)))
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
