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
            private readonly IList<Item> _value;

            public ListItem(SecsFormat format, IList<Item> value) : base(format, value.Count)
                => _value = value;

            public sealed override void Dispose()
            {
                for (int i = 0; i < _value.Count; i++)
                {
                    _value[i].Dispose();
                }
                GC.SuppressFinalize(this);
            }

            public sealed override Item this[int index]
            {
                get => _value[index];
                set => _value[index] = value;
            }

            public sealed override IEnumerable<Item> Slice(int start, int length)
            {
                if (start < 0 || start + length > _value.Count)
                {
                    throw new IndexOutOfRangeException($"Item.Count is {_value.Count}, but Slice(start: {start}, length: {length})");
                }
                return _value.Skip(start).Take(length);
            }

            public sealed override IEnumerator<Item> GetEnumerator() => _value.GetEnumerator();

            public sealed override void EncodeTo(IBufferWriter<byte> buffer)
            {
                if (_value.Count == 0)
                {
                    EncodeEmptyItem(Format, buffer);
                    return;
                }

                EncodeItemHeader(Format, _value.Count, buffer);
                for (int i = 0; i < _value.Count; i++)
                {
                    _value[i].EncodeTo(buffer);
                }
            }

            private protected sealed override bool IsEquals(Item other)
                => base.IsEquals(other) && IsListEquals(_value, Unsafe.As<ListItem>(other)._value);

            static bool IsListEquals(IList<Item> listLeft, IList<Item> listRight)
            {
                for (int i = 0, count = listLeft.Count; i < count; i++)
                {
                    if (!listLeft[i].IsEquals(listRight[i]))
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
