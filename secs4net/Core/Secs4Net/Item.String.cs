using System.Buffers;
using System.Text;

namespace Secs4Net
{
    partial class Item
    {
        private sealed class StringItem : Item
        {
            private readonly string _value;
            public unsafe StringItem(SecsFormat format, string value) : base(format, value.Length)
            {
                _value = value;
            }

            public override string GetString() => _value;

            public override void EncodeTo(IBufferWriter<byte> buffer)
            {
                if (Count == 0)
                {
                    EncodeEmptyItem(Format, buffer);
                    return;
                }

                var encoder = Format == SecsFormat.ASCII ? Encoding.ASCII : Jis8Encoding;
                var bytelength = encoder.GetByteCount(_value);
                EncodeItemHeader(Format, bytelength, buffer);
                var span = buffer.GetSpan(sizeHint: bytelength).Slice(0, bytelength);
                buffer.Advance(encoder.GetBytes(_value, span));
            }
        }
    }
}
