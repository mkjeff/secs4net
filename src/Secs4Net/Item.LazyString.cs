using Microsoft.Toolkit.HighPerformance.Buffers;
using Secs4Net.Extensions;
using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;

namespace Secs4Net
{
    partial class Item
    {
        private sealed class LazyStringItem : Item
        {
            private readonly IMemoryOwner<byte> _owner;
            private readonly Lazy<string> _value;

            public LazyStringItem(SecsFormat format, IMemoryOwner<byte> owner) : base(format, GetEncoding(format).GetCharCount(owner.Memory.Span))
            {
                _owner = owner;
                _value = new Lazy<string>(() =>
                {
                    var encoding = GetEncoding(format);
                    var span = _owner.Memory.Span;
                    return span.Length > 512 ? encoding.GetString(span) : StringPool.Shared.GetOrAdd(span, encoding);
                });
            }

            public sealed override void Dispose()
                => _owner.Dispose();

            public sealed override string GetString()
                => _value.Value;

            public sealed override void EncodeTo(IBufferWriter<byte> buffer)
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

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static Encoding GetEncoding(SecsFormat format)
                => format == SecsFormat.ASCII ? Encoding.ASCII : Jis8Encoding;

            private protected sealed override bool IsEquals(Item other)
                => base.IsEquals(other) && _value.Value.Equals(other.GetString(), StringComparison.Ordinal);
        }
    }
}
