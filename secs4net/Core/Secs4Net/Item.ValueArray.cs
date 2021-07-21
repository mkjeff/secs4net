using Microsoft.Toolkit.HighPerformance;
using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Secs4Net
{
    partial class Item
    {
        private class ValueArrayItem<T> : Item where T : unmanaged
        {
            private readonly Memory<T> _value;
            public unsafe ValueArrayItem(SecsFormat format, Memory<T> value) : base(format, value.Length)
            {
                _value = value;
            }

            public sealed override ref TResult FirstValue<TResult>()
            {
                if (_value.Length == 0 || _value.AsBytes().Length < Unsafe.SizeOf<TResult>())
                {
                    ThrowHelper();
                }

                return ref Unsafe.As<T, TResult>(ref _value.Span.DangerousGetReferenceAt(0));

                [DoesNotReturn]
                static void ThrowHelper() => throw new IndexOutOfRangeException("The item is empty or data length less than sizeof(T)");
            }

            public sealed override ref readonly TResult FirstValueOrDefault<TResult>(in TResult defaultValue = default)
            {
                if (_value.Length == 0 || _value.AsBytes().Length < Unsafe.SizeOf<TResult>())
                {
                    return ref defaultValue;
                }

                return ref Unsafe.As<T, TResult>(ref _value.Span.DangerousGetReferenceAt(0));
            }

            public sealed override ValueArray<TResult> GetValues<TResult>() => new(_value.AsBytes());

            public sealed override void EncodeTo(IBufferWriter<byte> buffer)
            {
                if (Count == 0)
                {
                    EncodeEmptyItem(Format, buffer);
                    return;
                }

                var bytes = _value.Span.AsBytes();
                var byteLength = bytes.Length;
                EncodeItemHeader(Format, byteLength, buffer);
                var span = buffer.GetSpan(sizeHint: byteLength).Slice(0, byteLength);
                bytes.CopyTo(span);
                span.Reverse(Unsafe.SizeOf<T>());
                buffer.Advance(byteLength);
            }
        }
    }
}
