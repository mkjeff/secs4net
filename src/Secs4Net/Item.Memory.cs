using Microsoft.Toolkit.HighPerformance;
using Secs4Net.Extensions;
using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Secs4Net
{
    partial class Item
    {
        private class MemoryItem<T> : Item where T : unmanaged, IEquatable<T>
        {
            private protected virtual Memory<T> Value { get; }

            internal MemoryItem(SecsFormat format, Memory<T> value) : base(format)
                => Value = value;

            public sealed override int Count => Value.Length;

            public sealed override ref TResult FirstValue<TResult>()
            {
                var memory = Value;
                if (memory.Length == 0 || memory.Length * Unsafe.SizeOf<T>() < Unsafe.SizeOf<TResult>())
                {
                    ThrowHelper();
                }

                return ref Unsafe.As<T, TResult>(ref memory.Span.DangerousGetReferenceAt(0));

                [DoesNotReturn]
                static void ThrowHelper() => throw new IndexOutOfRangeException($"The item is empty or data length less than sizeof({typeof(T).Name})");
            }

            public sealed override TResult FirstValueOrDefault<TResult>(TResult defaultValue = default)
            {
                var memory = Value;
                if (memory.Length == 0 || memory.Length * Unsafe.SizeOf<T>() < Unsafe.SizeOf<TResult>())
                {
                    return defaultValue;
                }

                return Unsafe.As<T, TResult>(ref memory.Span.DangerousGetReferenceAt(0));
            }

            public sealed override Memory<TResult> GetMemory<TResult>()
                => Value.Cast<T, TResult>();

            public sealed override ReadOnlyMemory<TResult> GetReadOnlyMemory<TResult>()
                => Value.Cast<T, TResult>();

            public sealed override void EncodeTo(IBufferWriter<byte> buffer)
            {
                var memory = Value;
                if (memory.IsEmpty)
                {
                    EncodeEmptyItem(Format, buffer);
                    return;
                }

                var valueAsBytes = memory.Span.AsBytes();
                var byteLength = valueAsBytes.Length;
                EncodeItemHeader(Format, byteLength, buffer);
                var span = buffer.GetSpan(byteLength).Slice(0, byteLength);
                valueAsBytes.CopyTo(span);
                span.Reverse(Unsafe.SizeOf<T>());
                buffer.Advance(byteLength);
            }

            private protected sealed override bool IsEquals(Item other)
                => Format == other.Format && Value.Span.SequenceEqual(Unsafe.As<MemoryItem<T>>(other).Value.Span);
        }
    }
}
