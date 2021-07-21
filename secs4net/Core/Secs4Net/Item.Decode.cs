using Microsoft.Toolkit.HighPerformance;
using Microsoft.Toolkit.HighPerformance.Buffers;
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
        internal static void DecodeFormatAndLengthByteCount(byte formatAndLengthByte, out SecsFormat format, out byte lengthByteCount)
        {
            format = (SecsFormat)(formatAndLengthByte & 0xFC);
            lengthByteCount = (byte)(formatAndLengthByte & 3);
        }

        internal static void DecodeDataLength(in ReadOnlySequence<byte> sourceBytes, out int dataLength)
        {
            Span<byte> itemLengthBytes = stackalloc byte[4];
            sourceBytes.CopyTo(itemLengthBytes);
            itemLengthBytes.Slice(0, (int)sourceBytes.Length).Reverse();
            dataLength = BitConverter.ToInt32(itemLengthBytes); // max to 3 byte dataLength
        }

        internal static Item DecodeFromFullBuffer(ref ReadOnlySequence<byte> bytes)
        {
            DecodeFormatAndLengthByteCount(bytes.FirstSpan[0], out var format, out var lengthByteCount);
            bytes = bytes.Slice(1);

            var dataLengthSeq = bytes.Slice(0, lengthByteCount);
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
                    Debug.Assert(list.Count < dataLength);
                    list.Add(DecodeFromFullBuffer(ref bytes));
                }

                return L(list);
            }

            var dataItemBytes = bytes.Slice(0, dataLength);
            var item = DecodeDataItem(format, dataItemBytes);
            bytes = bytes.Slice(dataItemBytes.End);
            return item;
        }

        internal static Item DecodeDataItem(SecsFormat format, in ReadOnlySequence<byte> bytes)
        {
            var length = bytes.Length; 
            return format switch
            {
                SecsFormat.ASCII => length switch
                {
                    0 => A(),
                    > 512 => A(Encoding.ASCII.GetString(bytes)),
                    _ => A(DecodeStringWithPooled(bytes, Encoding.ASCII)),
                },
                SecsFormat.JIS8 => length switch
                {
                    0 => J(),
                    > 512 => J(Jis8Encoding.GetString(bytes)),
                    _ => J(DecodeStringWithPooled(bytes, Jis8Encoding)),
                },
                SecsFormat.Boolean => length switch
                {
                    0 => Boolean(),
                    > 1024 => Boolean(DecodeWithOwner<bool>(bytes)),
                    _ => Boolean(Decode<bool>(bytes)),
                },
                SecsFormat.Binary => length switch
                {
                    0 => B(),
                    > 1024 => B(DecodeWithOwner<byte>(bytes)),
                    _ => B(Decode<byte>(bytes)),
                },
                SecsFormat.U1 => length switch
                {
                    0 => U1(),
                    > 1024 => U1(DecodeWithOwner<byte>(bytes)),
                    _ => U1(Decode<byte>(bytes)),
                },
                SecsFormat.U2 => length switch
                {
                    0 => U2(),
                    > 1024 => U2(DecodeWithOwner<ushort>(bytes)),
                    _ => U2(Decode<ushort>(bytes)),
                },
                SecsFormat.U4 => length switch
                {
                    0 => U4(),
                    > 1024 => U4(DecodeWithOwner<uint>(bytes)),
                    _ => U4(Decode<uint>(bytes)),
                },
                SecsFormat.U8 => length switch
                {
                    0 => U8(),
                    > 1024 => U8(DecodeWithOwner<ulong>(bytes)),
                    _ => U8(Decode<ulong>(bytes)),
                },
                SecsFormat.I1 => length switch
                {
                    0 => I1(),
                    > 1024 => I1(DecodeWithOwner<sbyte>(bytes)),
                    _ => I1(Decode<sbyte>(bytes)),
                },
                SecsFormat.I2 => length switch
                {
                    0 => I2(),
                    > 1024 => I2(DecodeWithOwner<short>(bytes)),
                    _ => I2(Decode<short>(bytes)),
                },
                SecsFormat.I4 => length switch
                {
                    0 => I4(),
                    > 1024 => I4(DecodeWithOwner<int>(bytes)),
                    _ => I4(Decode<int>(bytes)),
                },
                SecsFormat.I8 => length switch
                {
                    0 => I8(),
                    > 1024 => I8(DecodeWithOwner<long>(bytes)),
                    _ => I8(Decode<long>(bytes)),
                },
                SecsFormat.F4 => length switch
                {
                    0 => F4(),
                    > 1024 => F4(DecodeWithOwner<float>(bytes)),
                    _ => F4(Decode<float>(bytes)),
                },
                SecsFormat.F8 => length switch
                {
                    0 => F8(),
                    > 1024 => F8(DecodeWithOwner<double>(bytes)),
                    _ => F8(Decode<double>(bytes)),
                },
                _ => ThrowHelper(format),
            };

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static T[] Decode<T>(in ReadOnlySequence<byte> bytes) where T : unmanaged
            {
                var elmSize = Unsafe.SizeOf<T>();
                var values = new T[bytes.Length / elmSize];
                var valueAsBytesSpan = values.AsSpan().AsBytes();
                bytes.CopyTo(valueAsBytesSpan);
                valueAsBytesSpan.Reverse(elmSize);
                return values;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static string DecodeStringWithPooled(in ReadOnlySequence<byte> bytes, Encoding encoding)
            {
                using var spanOwner = SpanOwner<byte>.Allocate((int)bytes.Length);
                bytes.CopyTo(spanOwner.Span);
                return StringPool.Shared.GetOrAdd(spanOwner.Span, encoding);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static MemoryOwner<T> DecodeWithOwner<T>(in ReadOnlySequence<byte> bytes) where T : unmanaged
            {
                var elmSize = Unsafe.SizeOf<T>();
                var values = MemoryOwner<T>.Allocate((int)(bytes.Length / elmSize));
                var valueAsBytesSpan = values.Memory.Span.AsBytes();
                bytes.CopyTo(valueAsBytesSpan);
                valueAsBytesSpan.Reverse(elmSize);
                return values;
            }

            [DoesNotReturn]
            static Item ThrowHelper(SecsFormat format) => throw new ArgumentOutOfRangeException(nameof(format), format, @"Invalid format");
        }
    }
}
