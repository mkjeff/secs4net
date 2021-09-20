using System.Buffers;
using System.Diagnostics;
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

        public sealed override int Count
            => _value.Length;

        public sealed override string GetString()
            => _value;

        public sealed override void EncodeTo(IBufferWriter<byte> buffer)
        {
            if (_value.Length == 0)
            {
                EncodeEmptyItem(Format, buffer);
                return;
            }

            var encoder = Format == SecsFormat.ASCII ? Encoding.ASCII : Jis8Encoding;
            var bytelength = encoder.GetByteCount(_value);
            EncodeItemHeader(Format, bytelength, buffer);
            var length = encoder.GetBytes(_value, buffer.GetSpan(bytelength));
            buffer.Advance(bytelength);
#if DEBUG
            Debug.Assert(length == bytelength);
#endif
        }

        private protected sealed override bool IsEquals(Item other)
            => Format == other.Format && _value.Equals(other.GetString(), StringComparison.Ordinal);

        private sealed class ItemDebugView
        {
            private readonly StringItem _item; 

            public ItemDebugView(StringItem item)
            {
                _item = item;
                EncodedBytes = new EncodedByteDebugView(item);
            }
            public string Value => _item._value;
            public EncodedByteDebugView EncodedBytes { get; }
        }
    }
}
