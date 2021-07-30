using Microsoft.Toolkit.HighPerformance;
using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Secs4Net
{
    partial class Item
    {
        private class MemoryItem<T> : Item where T : unmanaged
        {
            private readonly Memory<T> _value;

            public MemoryItem(SecsFormat format, Memory<T> value) : base(format, value.Length) 
                => _value = value;

            public sealed override ref TResult FirstValue<TResult>()
            {
                if (_value.Length == 0 || _value.Length * Unsafe.SizeOf<T>() < Unsafe.SizeOf<TResult>())
                {
                    ThrowHelper();
                }

                return ref Unsafe.As<T, TResult>(ref _value.Span.DangerousGetReferenceAt(0));

                [DoesNotReturn]
                static void ThrowHelper() => throw new IndexOutOfRangeException($"The item is empty or data length less than sizeof({typeof(T).Name})");
            }

            public sealed override ref readonly TResult FirstValueOrDefault<TResult>(TResult defaultValue = default)
            {
                if (_value.Length == 0 || _value.Length * Unsafe.SizeOf<T>() < Unsafe.SizeOf<TResult>())
                {
                    return ref new ReadOnlyRef<TResult>(defaultValue).Value;
                }

                return ref Unsafe.As<T, TResult>(ref _value.Span.DangerousGetReferenceAt(0));
            }

            public sealed override Memory<TResult> GetMemory<TResult>() 
                => _value.Cast<T, TResult>();

            public sealed override ReadOnlyMemory<TResult> GetReadOnlyMemory<TResult>()
                => _value.Cast<T, TResult>();

            public sealed override void EncodeTo(IBufferWriter<byte> buffer)
            {
                if (_value.IsEmpty)
                {
                    EncodeEmptyItem(Format, buffer);
                    return;
                }

                var valueAsBytes = _value.Span.AsBytes();
                var byteLength = valueAsBytes.Length;
                EncodeItemHeader(Format, byteLength, buffer);
                var span = buffer.GetSpan(byteLength).Slice(0, byteLength);
                valueAsBytes.CopyTo(span);
                span.Reverse(Unsafe.SizeOf<T>());
                buffer.Advance(byteLength);
            }

            private protected sealed override bool IsEquals(Item other)
                => base.IsEquals(other) && _value.Span.SequenceEqual(Unsafe.As<MemoryItem<T>>(other)._value.Span);
        }
    }
}
