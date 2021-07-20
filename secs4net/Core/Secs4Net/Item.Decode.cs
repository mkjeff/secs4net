using Microsoft.Toolkit.HighPerformance;
using System;
using System.Buffers;
using System.Collections.Generic;
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
            return format switch
            {
                SecsFormat.ASCII => bytes.IsEmpty ? A() : A(Encoding.ASCII.GetString(bytes)),
                SecsFormat.JIS8 => bytes.IsEmpty ? J() : J(Jis8Encoding.GetString(bytes)),
                SecsFormat.Boolean => bytes.IsEmpty ? Boolean() : Boolean(Decode<bool>(bytes)),
                SecsFormat.Binary => bytes.IsEmpty ? B() : B(Decode<byte>(bytes)),
                SecsFormat.U1 => bytes.IsEmpty ? U1() : U1(Decode<byte>(bytes)),
                SecsFormat.U2 => bytes.IsEmpty ? U2() : U2(Decode<ushort>(bytes)),
                SecsFormat.U4 => bytes.IsEmpty ? U4() : U4(Decode<uint>(bytes)),
                SecsFormat.U8 => bytes.IsEmpty ? U8() : U8(Decode<ulong>(bytes)),
                SecsFormat.I1 => bytes.IsEmpty ? I1() : I1(Decode<sbyte>(bytes)),
                SecsFormat.I2 => bytes.IsEmpty ? I2() : I2(Decode<short>(bytes)),
                SecsFormat.I4 => bytes.IsEmpty ? I4() : I4(Decode<int>(bytes)),
                SecsFormat.I8 => bytes.IsEmpty ? I8() : I8(Decode<long>(bytes)),
                SecsFormat.F4 => bytes.IsEmpty ? F4() : F4(Decode<float>(bytes)),
                SecsFormat.F8 => bytes.IsEmpty ? F8() : F8(Decode<double>(bytes)),
                _ => ThrowHelper(format),
            };

            static T[] Decode<T>(in ReadOnlySequence<byte> bytes) where T : unmanaged
            {
                var elmSize = Unsafe.SizeOf<T>();
                var values = new T[bytes.Length / elmSize];
                var valueAsBytesSpan = values.AsSpan().AsBytes();
                bytes.CopyTo(values.AsSpan().AsBytes());
                valueAsBytesSpan.Reverse(elmSize);
                return values;
            }

            static Item ThrowHelper(SecsFormat format) => throw new ArgumentOutOfRangeException(nameof(format), format, @"Invalid format");
        }
    }
}
