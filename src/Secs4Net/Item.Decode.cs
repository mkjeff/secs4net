using Microsoft.Toolkit.HighPerformance;
using Microsoft.Toolkit.HighPerformance.Buffers;
using Secs4Net.Extensions;
using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace Secs4Net
{
    partial class Item
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void DecodeFormatAndLengthByteCount(byte formatAndLengthByte, out SecsFormat format, out byte lengthByteCount)
        {
            format = (SecsFormat)(formatAndLengthByte & 0b11111100);
            lengthByteCount = (byte)(formatAndLengthByte & 0b00000011);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void DecodeDataLength(in ReadOnlySequence<byte> sourceBytes, out int dataLength)
        {
            dataLength = 0;
            var lengthBytes = dataLength.AsBytes();
            sourceBytes.CopyTo(lengthBytes);
            lengthBytes.Slice(0, (int)sourceBytes.Length).Reverse();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Item DecodeFromFullBuffer(ref ReadOnlySequence<byte> bytes)
        {
#if NET
            DecodeFormatAndLengthByteCount(bytes.FirstSpan.DangerousGetReferenceAt(0), out var format, out var lengthByteCount);
#else
            DecodeFormatAndLengthByteCount(bytes.First.Span.DangerousGetReferenceAt(0), out var format, out var lengthByteCount);
#endif

            var dataLengthSeq = bytes.Slice(1, lengthByteCount);
            DecodeDataLength(dataLengthSeq, out var dataLength);
            bytes = bytes.Slice(dataLengthSeq.End);

            if (format == SecsFormat.List)
            {
                if (dataLength == 0)
                {
                    return L();
                }

                var items = new Item[dataLength];
                for (var i = 0; i < items.Length; i++)
                {
                    items.DangerousGetReferenceAt(i) = DecodeFromFullBuffer(ref bytes);
                }

                return L(items);
            }

            var dataItemBytes = bytes.Slice(0, dataLength);
            var item = DecodeDataItem(format, dataItemBytes);
            bytes = bytes.Slice(dataItemBytes.End);
            return item;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static Item DecodeDataItem(SecsFormat format, in ReadOnlySequence<byte> bytes)
        {
            var length = bytes.Length;
            return format switch
            {
                SecsFormat.ASCII => length switch
                {
                    0 => A(),
                    >= 512 => DecodeLazyStringItem(bytes, format),
                    _ => A(DecodeString(bytes, Encoding.ASCII)),
                },
                SecsFormat.JIS8 => length switch
                {
                    0 => J(),
                    >= 512 => DecodeLazyStringItem(bytes, format),
                    _ => J(DecodeString(bytes, Jis8Encoding)),
                },
                SecsFormat.Boolean => length switch
                {
                    0 => Boolean(),
                    >= 1024 => new MemoryOwnerItem<bool>(format, DecodeWithOwner<bool>(bytes)),
                    _ => new MemoryItem<bool>(format, Decode<bool>(bytes)),
                },
                SecsFormat.Binary => length switch
                {
                    0 => B(),
                    >= 1024 => new MemoryOwnerItem<byte>(format, DecodeWithOwner<byte>(bytes)),
                    _ => new MemoryItem<byte>(format, Decode<byte>(bytes)),
                },
                SecsFormat.U1 => length switch
                {
                    0 => U1(),
                    >= 1024 => new MemoryOwnerItem<byte>(format, DecodeWithOwner<byte>(bytes)),
                    _ => new MemoryItem<byte>(format, Decode<byte>(bytes)),
                },
                SecsFormat.U2 => length switch
                {
                    0 => U2(),
                    >= 1024 => DecodeU2WithOwner(bytes),
                    _ => DecodeU2(bytes),
                },
                SecsFormat.U4 => length switch
                {
                    0 => U4(),
                    >= 1024 => DecodeU4WithOwner(bytes),
                    _ => DecodeU4(bytes),
                },
                SecsFormat.U8 => length switch
                {
                    0 => U8(),
                    >= 1024 => DecodeU8WithOwner(bytes),
                    _ => DecodeU8(bytes),
                },
                SecsFormat.I1 => length switch
                {
                    0 => I1(),
                    >= 1024 => new MemoryOwnerItem<sbyte>(format, DecodeWithOwner<sbyte>(bytes)),
                    _ => new MemoryItem<sbyte>(format, Decode<sbyte>(bytes)),
                },
                SecsFormat.I2 => length switch
                {
                    0 => I2(),
                    >= 1024 => DecodeI2WithOwner(bytes),
                    _ => DecodeI2(bytes),
                },
                SecsFormat.I4 => length switch
                {
                    0 => I4(),
                    >= 1024 => DecodeI4WithOwner(bytes),
                    _ => DecodeI4(bytes),
                },
                SecsFormat.I8 => length switch
                {
                    0 => I8(),
                    >= 1024 => DecodeI8WithOwner(bytes),
                    _ => DecodeI8(bytes),
                },
                SecsFormat.F4 => length switch
                {
                    0 => F4(),
                    >= 1024 => DecodeF4WithOwner(bytes),
                    _ => DecodeF4(bytes),
                },
                SecsFormat.F8 => length switch
                {
                    0 => F8(),
                    >= 1024 => DecodeF8WithOwner(bytes),
                    _ => DecodeF8(bytes),
                },
                _ => ThrowHelper(format),
            };

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static LazyStringItem DecodeLazyStringItem(in ReadOnlySequence<byte> bytes, SecsFormat format)
            {
                var owner = MemoryOwner<byte>.Allocate((int)bytes.Length);
                bytes.CopyTo(owner.Memory.Span);
                return new LazyStringItem(format, owner);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static string DecodeString(in ReadOnlySequence<byte> bytes, Encoding encoding)
            {
                using var spanOwner = SpanOwner<byte>.Allocate((int)bytes.Length);
                bytes.CopyTo(spanOwner.Span);
                return StringPool.Shared.GetOrAdd(spanOwner.Span, encoding);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [SkipLocalsInit]
            static Memory<T> Decode<T>(in ReadOnlySequence<byte> bytes) where T : unmanaged
            {
                Memory<T> valueMemory = new T[bytes.Length / Unsafe.SizeOf<T>()];
                bytes.CopyTo(valueMemory.Span.AsBytes());
                return valueMemory;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static IMemoryOwner<T> DecodeWithOwner<T>(in ReadOnlySequence<byte> bytes) where T : unmanaged
            {
                var valueMemoryOwner = MemoryOwner<T>.Allocate((int)(bytes.Length / Unsafe.SizeOf<T>()));
                bytes.CopyTo(valueMemoryOwner.Span.AsBytes());
                return valueMemoryOwner;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static Item DecodeU2(in ReadOnlySequence<byte> bytes)
            {
                var value = Decode<ushort>(bytes);
                value.Span.ReverseEndianness();
                return new MemoryItem<ushort>(SecsFormat.U2, value);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static Item DecodeU2WithOwner(in ReadOnlySequence<byte> bytes)
            {
                var valueOwner = DecodeWithOwner<ushort>(bytes);
                valueOwner.Memory.Span.ReverseEndianness();
                return new MemoryOwnerItem<ushort>(SecsFormat.U2, valueOwner);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static Item DecodeU4(in ReadOnlySequence<byte> bytes)
            {
                var value = Decode<uint>(bytes);
                value.Span.ReverseEndianness();
                return new MemoryItem<uint>(SecsFormat.U4, value);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static Item DecodeU4WithOwner(in ReadOnlySequence<byte> bytes)
            {
                var valueOwner = DecodeWithOwner<uint>(bytes);
                valueOwner.Memory.Span.ReverseEndianness();
                return new MemoryOwnerItem<uint>(SecsFormat.U4, valueOwner);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static Item DecodeU8(in ReadOnlySequence<byte> bytes)
            {
                var value = Decode<ulong>(bytes);
                value.Span.ReverseEndianness();
                return new MemoryItem<ulong>(SecsFormat.U8, value);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static Item DecodeU8WithOwner(in ReadOnlySequence<byte> bytes)
            {
                var valueOwner = DecodeWithOwner<ulong>(bytes);
                valueOwner.Memory.Span.ReverseEndianness();
                return new MemoryOwnerItem<ulong>(SecsFormat.U8, valueOwner);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static Item DecodeI2(in ReadOnlySequence<byte> bytes)
            {
                var value = Decode<short>(bytes);
                value.Span.ReverseEndianness();
                return new MemoryItem<short>(SecsFormat.I2, value);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static Item DecodeI2WithOwner(in ReadOnlySequence<byte> bytes)
            {
                var valueOwner = DecodeWithOwner<short>(bytes);
                valueOwner.Memory.Span.ReverseEndianness();
                return new MemoryOwnerItem<short>(SecsFormat.I2, valueOwner);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static Item DecodeI4(in ReadOnlySequence<byte> bytes)
            {
                var value = Decode<int>(bytes);
                value.Span.ReverseEndianness();
                return new MemoryItem<int>(SecsFormat.I4, value);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static Item DecodeI4WithOwner(in ReadOnlySequence<byte> bytes)
            {
                var valueOwner = DecodeWithOwner<int>(bytes);
                valueOwner.Memory.Span.ReverseEndianness();
                return new MemoryOwnerItem<int>(SecsFormat.I4, valueOwner);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static Item DecodeI8(in ReadOnlySequence<byte> bytes)
            {
                var value = Decode<long>(bytes);
                value.Span.ReverseEndianness();
                return new MemoryItem<long>(SecsFormat.I8, value);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static Item DecodeI8WithOwner(in ReadOnlySequence<byte> bytes)
            {
                var valueOwner = DecodeWithOwner<long>(bytes);
                valueOwner.Memory.Span.ReverseEndianness();
                return new MemoryOwnerItem<long>(SecsFormat.I8, valueOwner);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static Item DecodeF4(in ReadOnlySequence<byte> bytes)
            {
                var value = Decode<float>(bytes);
                value.Span.ReverseEndianness();
                return new MemoryItem<float>(SecsFormat.F4, value);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static Item DecodeF4WithOwner(in ReadOnlySequence<byte> bytes)
            {
                var valueOwner = DecodeWithOwner<float>(bytes);
                valueOwner.Memory.Span.ReverseEndianness();
                return new MemoryOwnerItem<float>(SecsFormat.F4, valueOwner);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static Item DecodeF8(in ReadOnlySequence<byte> bytes)
            {
                var value = Decode<double>(bytes);
                value.Span.ReverseEndianness();
                return new MemoryItem<double>(SecsFormat.F8, value);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static Item DecodeF8WithOwner(in ReadOnlySequence<byte> bytes)
            {
                var valueOwner = DecodeWithOwner<double>(bytes);
                valueOwner.Memory.Span.ReverseEndianness();
                return new MemoryOwnerItem<double>(SecsFormat.F8, valueOwner);
            }

            [DoesNotReturn]
            static Item ThrowHelper(SecsFormat format) => throw new ArgumentOutOfRangeException(nameof(format), format, @"Invalid format");
        }
    }
}
