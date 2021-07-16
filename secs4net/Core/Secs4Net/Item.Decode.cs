using Microsoft.Toolkit.HighPerformance;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Secs4Net
{
    partial class Item
    {
        public static Item Decode(Span<byte> bytes)
        {
            var index = 0;
            return DecodeFromFullBuffer(bytes, ref index);
        }

        internal static void DecodeFormatAndLengthByteCount(Span<byte> bytes, ref int index, out SecsFormat format, out byte lengthByteCount)
        {
            var formatAndLengthByte = bytes.DangerousGetReferenceAt(index);
            format = (SecsFormat)(formatAndLengthByte & 0xFC);
            lengthByteCount = (byte)(formatAndLengthByte & 3);
            index++;
            //return (format, lengthByteCount);
        }

        internal static void DecodeDataLength(Span<byte> sourceBytes, ref int index, out int dataLength)
        {
            Span<byte> itemLengthBytes = stackalloc byte[4];
            sourceBytes.CopyTo(itemLengthBytes);
            itemLengthBytes.Slice(0, sourceBytes.Length).Reverse();
            dataLength = BitConverter.ToInt32(itemLengthBytes); // max to 3 byte dataLength
            index += sourceBytes.Length;
        }

        internal static Item DecodeFromFullBuffer(Span<byte> bytes, ref int index)
        {
            DecodeFormatAndLengthByteCount(bytes, ref index, out var format, out var lengthByteCount);
            DecodeDataLength(bytes.Slice(index, lengthByteCount), ref index, out var dataLength);

            if (format == SecsFormat.List)
            {
                if (dataLength == 0)
                {
                    return L();
                }

                var list = new List<Item>(dataLength);
                for (var i = 0; i < dataLength; i++)
                {
                    list.Add(DecodeFromFullBuffer(bytes, ref index));
                }

                return L(list);
            }
            var item = DecodeNonListItem(format, bytes.Slice(index, dataLength));
            index += dataLength;
            return item;
        }

        internal static Item DecodeNonListItem(SecsFormat format, Span<byte> bytes)
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

            static T[] Decode<T>(Span<byte> bytes) where T : unmanaged
            {
                var elmSize = Unsafe.SizeOf<T>();
                bytes.Reverse(elmSize);
                var values = new T[bytes.Length / elmSize];
                bytes.CopyTo(values.AsSpan().AsBytes());
                return values;
            }

            static Item ThrowHelper(SecsFormat format) => throw new ArgumentException(@"Invalid format", nameof(format));
        }
    }
}
