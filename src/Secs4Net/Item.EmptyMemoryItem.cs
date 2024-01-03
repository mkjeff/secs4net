using System.Buffers;

namespace Secs4Net;

partial class Item
{
    private sealed class EmptyItem : Item
    {
        internal EmptyItem(SecsFormat format)
            : base(format)
        {
        }

        public override int Count => 0;
        public override void EncodeTo(IBufferWriter<byte> buffer) => EncodeEmptyItem(Format, buffer);
        private protected override bool IsEquals(Item other) => Format == other.Format && other.Count is 0;
    }
}

