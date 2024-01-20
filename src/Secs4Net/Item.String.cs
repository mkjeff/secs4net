using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;

namespace Secs4Net;

partial class Item
{
    [DebuggerTypeProxy(typeof(ItemDebugView))]
    [SkipLocalsInit]
    private sealed class StringItem : Item
    {
        private readonly string _value;

        internal StringItem(SecsFormat format, string value)
            : base(format)
            => _value = value;

        public override int Count
            => _value.Length;

        public override string GetString()
            => _value;

        public override void EncodeTo(IBufferWriter<byte> buffer)
        {
            if (_value.Length == 0)
            {
                EncodeEmptyItem(Format, buffer);
                return;
            }

            var encoder = Format == SecsFormat.ASCII ? ASCIIEncoding : JIS8Encoding;
            var byteLength = encoder.GetByteCount(_value);
            EncodeItemHeader(Format, byteLength, buffer);
            var length = encoder.GetBytes(_value, buffer.GetSpan(byteLength));
            buffer.Advance(byteLength);
            Debug.Assert(length == byteLength);
        }

        private protected override bool IsEquals(Item other)
            => Format == other.Format && _value.Equals(other.GetString(), StringComparison.Ordinal);

        private sealed class ItemDebugView(Item.StringItem item)
        {
            public string Value => item._value;
            public EncodedByteDebugView EncodedBytes { get; } = new EncodedByteDebugView(item);
        }
    }
}
