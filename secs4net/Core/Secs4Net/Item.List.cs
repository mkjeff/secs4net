using System.Buffers;
using System.Collections.Generic;

namespace Secs4Net
{
    partial class Item
    {
        private sealed class ListItem : Item
        {
            private readonly IList<Item> _value;
            public unsafe ListItem(SecsFormat format, IList<Item> value) : base(format, value.Count)
            {
                _value = value;
            }

            public override Item this[int index]
            {
                get => _value[index];
                set => _value[index] = value;
            }

            public override IEnumerator<Item> GetEnumerator() => _value.GetEnumerator();

            public override void EncodeTo(IBufferWriter<byte> buffer)
            {
                if (Count == 0)
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
        }
    }
}
