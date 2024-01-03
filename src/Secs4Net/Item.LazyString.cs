using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;

namespace Secs4Net;

partial class Item
{
    [DebuggerTypeProxy(typeof(ItemDebugView))]
    [SkipLocalsInit]
    private sealed class LazyStringItem : Item
    {
        private readonly IMemoryOwner<byte> _owner;
        private readonly Lazy<string> _value;

        internal LazyStringItem(SecsFormat format, IMemoryOwner<byte> owner)
            : base(format)
        {
            _owner = owner;
            _value = new Lazy<string>(() =>
            {
                var encoding = Format == SecsFormat.ASCII ? Encoding.ASCII : JIS8Encoding;
                return encoding.GetString(_owner.Memory.Span);
            }, isThreadSafe: false);
        }

        public override void Dispose()
            => _owner.Dispose();

        public override int Count
            => _value.Value.Length;

        public override string GetString()
            => _value.Value;

        public override void EncodeTo(IBufferWriter<byte> buffer)
        {
            ReadOnlySpan<byte> bytes = _owner.Memory.Span;
            if (bytes.IsEmpty)
            {
                EncodeEmptyItem(Format, buffer);
                return;
            }
            EncodeItemHeader(Format, bytes.Length, buffer);
            buffer.Write(bytes);
        }

        private protected override bool IsEquals(Item other)
            => Format == other.Format && _value.Value.Equals(other.GetString(), StringComparison.Ordinal);

        private sealed class ItemDebugView(LazyStringItem item)
        {
            public string Value => item._value.Value;
            public EncodedByteDebugView EncodedBytes { get; } = new EncodedByteDebugView(item);
        }
    }
}
