using Microsoft.Toolkit.HighPerformance;
using Microsoft.Toolkit.HighPerformance.Buffers;
using Secs4Net.Extensions;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
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
            Span<byte> itemLengthBytes = stackalloc byte[4];
            sourceBytes.CopyTo(itemLengthBytes);
            itemLengthBytes.Slice(0, (int)sourceBytes.Length).Reverse();
            dataLength = Unsafe.ReadUnaligned<int>(ref itemLengthBytes.DangerousGetReference()); // max to 3 byte dataLength
        }

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

                var list = new List<Item>(dataLength);
                for (var i = 0; i < dataLength; i++)
                {
#if DEBUG
                    Debug.Assert(list.Count < dataLength);
#endif
                    list.Add(DecodeFromFullBuffer(ref bytes));
                }

                return L(list);
            }

            var dataItemBytes = bytes.Slice(0, dataLength);
            var item = DecodeDataItem(format, dataItemBytes);
            bytes = bytes.Slice(dataItemBytes.End);
            return item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Item DecodeDataItem(SecsFormat format, in ReadOnlySequence<byte> bytes)
        {
            var length = bytes.Length;
            return format switch
            {
                SecsFormat.ASCII => length switch
                {
                    0 => A(),
                    >= 512 => CreateLazyStringItem(bytes, format),
                    _ => CreateStringItem(bytes, format, Encoding.ASCII),
                },
                SecsFormat.JIS8 => length switch
                {
                    0 => J(),
                    >= 512 => CreateLazyStringItem(bytes, format),
                    _ => CreateStringItem(bytes, format, Jis8Encoding),
                },
                SecsFormat.Boolean => length switch
                {
                    0 => Boolean(),
                    >= 1024 => Boolean(DecodeWithOwner<bool>(bytes)),
                    _ => Boolean(Decode<bool>(bytes)),
                },
                SecsFormat.Binary => length switch
                {
                    0 => B(),
                    >= 1024 => B(DecodeWithOwner<byte>(bytes)),
                    _ => B(Decode<byte>(bytes)),
                },
                SecsFormat.U1 => length switch
                {
                    0 => U1(),
                    >= 1024 => U1(DecodeWithOwner<byte>(bytes)),
                    _ => U1(Decode<byte>(bytes)),
                },
                SecsFormat.U2 => length switch
                {
                    0 => U2(),
                    >= 1024 => U2(DecodeWithOwner<ushort>(bytes).ReverseEndianness()),
                    _ => U2(Decode<ushort>(bytes).ReverseEndianness()),
                },
                SecsFormat.U4 => length switch
                {
                    0 => U4(),
                    >= 1024 => U4(DecodeWithOwner<uint>(bytes).ReverseEndianness()),
                    _ => U4(Decode<uint>(bytes).ReverseEndianness()),
                },
                SecsFormat.U8 => length switch
                {
                    0 => U8(),
                    >= 1024 => U8(DecodeWithOwner<ulong>(bytes).ReverseEndianness()),
                    _ => U8(Decode<ulong>(bytes).ReverseEndianness()),
                },
                SecsFormat.I1 => length switch
                {
                    0 => I1(),
                    >= 1024 => I1(DecodeWithOwner<sbyte>(bytes)),
                    _ => I1(Decode<sbyte>(bytes)),
                },
                SecsFormat.I2 => length switch
                {
                    0 => I2(),
                    >= 1024 => I2(DecodeWithOwner<short>(bytes).ReverseEndianness()),
                    _ => I2(Decode<short>(bytes).ReverseEndianness()),
                },
                SecsFormat.I4 => length switch
                {
                    0 => I4(),
                    >= 1024 => I4(DecodeWithOwner<int>(bytes).ReverseEndianness()),
                    _ => I4(Decode<int>(bytes).ReverseEndianness()),
                },
                SecsFormat.I8 => length switch
                {
                    0 => I8(),
                    >= 1024 => I8(DecodeWithOwner<long>(bytes).ReverseEndianness()),
                    _ => I8(Decode<long>(bytes).ReverseEndianness()),
                },
                SecsFormat.F4 => length switch
                {
                    0 => F4(),
                    >= 1024 => F4(DecodeWithOwner<float>(bytes).ReverseEndianness()),
                    _ => F4(Decode<float>(bytes).ReverseEndianness()),
                },
                SecsFormat.F8 => length switch
                {
                    0 => F8(),
                    >= 1024 => F8(DecodeWithOwner<double>(bytes).ReverseEndianness()),
                    _ => F8(Decode<double>(bytes).ReverseEndianness()),
                },
                _ => ThrowHelper(format),
            };

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static LazyStringItem CreateLazyStringItem(in ReadOnlySequence<byte> bytes, SecsFormat format)
            {
                var owner = MemoryOwner<byte>.Allocate((int)bytes.Length);
                bytes.CopyTo(owner.Memory.Span);
                return new LazyStringItem(format, owner);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static StringItem CreateStringItem(in ReadOnlySequence<byte> bytes, SecsFormat format, Encoding encoding)
            {
                using var spanOwner = SpanOwner<byte>.Allocate((int)bytes.Length);
                bytes.CopyTo(spanOwner.Span);
                return new StringItem(format, StringPool.Shared.GetOrAdd(spanOwner.Span, encoding));
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

            [DoesNotReturn]
            static Item ThrowHelper(SecsFormat format) => throw new ArgumentOutOfRangeException(nameof(format), format, @"Invalid format");
        }
    }
}
