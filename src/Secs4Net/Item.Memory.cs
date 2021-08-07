using Microsoft.Toolkit.HighPerformance;
using Secs4Net.Extensions;
using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Secs4Net
{
    partial class Item
    {
        private class MemoryItem<T> : Item where T : unmanaged, IEquatable<T>
        {
            private static readonly unsafe delegate*<Span<byte>, void> ReverseEndianness;

            static unsafe MemoryItem()
            {
                var type = typeof(T);
                if (type == typeof(ushort))
                {
                    ReverseEndianness = &ReverseUInt16;
                }
                else if (type == typeof(uint))
                {
                    ReverseEndianness = &ReverseUInt32;
                }
                else if (type == typeof(ulong))
                {
                    ReverseEndianness = &ReverseUInt64;
                }
                else if (type == typeof(short))
                {
                    ReverseEndianness = &ReverseInt16;
                }
                else if (type == typeof(int))
                {
                    ReverseEndianness = &ReverseInt32;
                }
                else if (type == typeof(long))
                {
                    ReverseEndianness = &ReverseInt64;
                }
                else if (type == typeof(float))
                {
                    ReverseEndianness = &ReverseSingle;
                }
                else if (type == typeof(double))
                {
                    ReverseEndianness = &ReverseDouble;
                }
                else
                {
                    ReverseEndianness = &ReverseNothing;
                }

                static void ReverseNothing(Span<byte> bytes) { }
                static void ReverseUInt16(Span<byte> bytes) => bytes.Cast<ushort>().ReverseEndianness();
                static void ReverseUInt32(Span<byte> bytes) => bytes.Cast<uint>().ReverseEndianness();
                static void ReverseUInt64(Span<byte> bytes) => bytes.Cast<ulong>().ReverseEndianness();
                static void ReverseInt16(Span<byte> bytes) => bytes.Cast<short>().ReverseEndianness();
                static void ReverseInt32(Span<byte> bytes) => bytes.Cast<int>().ReverseEndianness();
                static void ReverseInt64(Span<byte> bytes) => bytes.Cast<long>().ReverseEndianness();
                static void ReverseSingle(Span<byte> bytes) => bytes.Cast<float>().ReverseEndianness();
                static void ReverseDouble(Span<byte> bytes) => bytes.Cast<double>().ReverseEndianness();
            }

            private protected virtual ReadOnlyMemory<T> Value { get; }

            internal MemoryItem(SecsFormat format, Memory<T> value)
                : base(format)
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
                => MemoryMarshal.AsMemory(Value.Cast<T, TResult>());

            public sealed override ReadOnlyMemory<TResult> GetReadOnlyMemory<TResult>()
                => Value.Cast<T, TResult>();

            public sealed override unsafe void EncodeTo(IBufferWriter<byte> buffer)
            {
                var memory = Value;
                if (memory.IsEmpty)
                {
                    EncodeEmptyItem(Format, buffer);
                    return;
                }

                var valueByteSpan = memory.Span.AsBytes();
                var byteLength = valueByteSpan.Length;

                EncodeItemHeader(Format, byteLength, buffer);

                var bufferByteSpan = buffer.GetSpan(byteLength).Slice(0, byteLength);
                valueByteSpan.CopyTo(bufferByteSpan);
                ReverseEndianness(bufferByteSpan);
                buffer.Advance(byteLength);
            }

            private protected sealed override bool IsEquals(Item other)
                => Format == other.Format && Value.Span.SequenceEqual(Unsafe.As<MemoryItem<T>>(other)!.Value.Span);
        }
    }
}
