﻿using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;

namespace Secs4Net
{
    partial class Item
    {
        private sealed class StringItem : Item
        {
            private readonly string _value;

            public StringItem(SecsFormat format, string value) : base(format, value.Length) 
                => _value = value;

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
                var span = buffer.GetSpan(bytelength).Slice(0, bytelength);
                buffer.Advance(encoder.GetBytes(_value, span));
            }

            private protected sealed override bool IsEquals(Item other) 
                => base.IsEquals(other) && _value.Equals(Unsafe.As<StringItem>(other)._value, System.StringComparison.Ordinal);
        }
    }
}
