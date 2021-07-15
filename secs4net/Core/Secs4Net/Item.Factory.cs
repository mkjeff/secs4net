using Microsoft.Toolkit.HighPerformance;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Secs4Net
{
    unsafe partial class Item
    {
        private unsafe Item(SecsFormat format, IEnumerable values, delegate*<Item, IBufferWriter<byte>, void> encoder)
        {
            Format = format;
            _values = values;
            _encode = encoder;
        }

        private static void EncodeList(Item item, IBufferWriter<byte> buffer)
        {
            var list = Unsafe.As<IReadOnlyList<Item>>(item._values);
            EncodeItemHeader(item.Format, list.Count, buffer);

            for (int i = 0; i < list.Count; i++)
            {
                list[i].EncodeTo(buffer);
            }
        }

        //private static void EncodeArray<T>(Item item, IBufferWriter<byte> buffer) where T : unmanaged
        //{
        //    var arr = Unsafe.As<T[]>(item._values);
        //    var byteLength = Unsafe.SizeOf<T>() * arr.Length;
        //    EncodeItemHeader(item.Format, byteLength, buffer);
        //    var span = buffer.GetSpan(sizeHint: byteLength);
        //    MemoryMarshal.AsBytes(arr.AsSpan()).CopyTo(span);
        //    span.Reverse(Unsafe.SizeOf<T>());
        //    buffer.Advance(byteLength);
        //}

        private static void EncodeArray(Item item, IBufferWriter<byte> buffer)
        {
            var arr = Unsafe.As<Array>(item._values);
            var byteLength = Buffer.ByteLength(arr);
            EncodeItemHeader(item.Format, byteLength, buffer);
            var span = buffer.GetSpan(sizeHint: byteLength).Slice(0, byteLength);
            MemoryMarshal.CreateReadOnlySpan(ref MemoryMarshal.GetArrayDataReference(arr), byteLength).CopyTo(span);
            span.Reverse(byteLength / arr.Length);
            buffer.Advance(byteLength);
        }

        private static void EncodeString(Item item, IBufferWriter<byte> buffer)
        {
            var str = Unsafe.As<string>(item._values);
            var encoder = item.Format == SecsFormat.ASCII ? Encoding.ASCII : Jis8Encoding;
            var bytelength = encoder.GetByteCount(str);
            EncodeItemHeader(item.Format, bytelength, buffer);
            var span = buffer.GetSpan(sizeHint: bytelength).Slice(0, bytelength);
            buffer.Advance(encoder.GetBytes(str, span));
        }

        private static void EncodeEmptyItem(Item item, IBufferWriter<byte> buffer)
        {
            var span = buffer.GetSpan(sizeHint: 2);
            span[0] = (byte)((byte)item.Format | 1);
            span[1] = 0;
            buffer.Advance(2);
        }

        public static Item L(IReadOnlyList<Item> items) => items.Count > 0 ? new Item(SecsFormat.List, items, &EncodeList) : L();
        public static Item L(IEnumerable<Item> items) => items.Any() ? L(items.ToList()) : L();
        public static Item L(params Item[] items) => items.Length > 0 ? L((IReadOnlyList<Item>)items) : L();

        public static Item B(params byte[] value) => value.Length > 0 ? new Item(SecsFormat.Binary, value, &EncodeArray) : B();
        public static Item B(IEnumerable<byte> value) => value.Any() ? B(value.ToArray()) : B();

        public static Item U1(params byte[] value) => value.Length > 0 ? new Item(SecsFormat.U1, value, &EncodeArray) : U1();
        public static Item U1(IEnumerable<byte> value) => value.Any() ? U1(value.ToArray()) : U1();

        public static Item U2(params ushort[] value) => value.Length > 0 ? new Item(SecsFormat.U2, value, &EncodeArray) : U2();
        public static Item U2(IEnumerable<ushort> value) => value.Any() ? U2(value.ToArray()) : U2();

        public static Item U4(params uint[] value) => value.Length > 0 ? new Item(SecsFormat.U4, value, &EncodeArray) : U4();
        public static Item U4(IEnumerable<uint> value) => value.Any() ? U4(value.ToArray()) : U4();

        public static Item U8(params ulong[] value) => value.Length > 0 ? new Item(SecsFormat.U8, value, &EncodeArray) : U8();
        public static Item U8(IEnumerable<ulong> value) => value.Any() ? U8(value.ToArray()) : U8();

        public static Item I1(params sbyte[] value) => value.Length > 0 ? new Item(SecsFormat.I1, value, &EncodeArray) : I1();
        public static Item I1(IEnumerable<sbyte> value) => value.Any() ? I1(value.ToArray()) : I1();

        public static Item I2(params short[] value) => value.Length > 0 ? new Item(SecsFormat.I2, value, &EncodeArray) : I2();
        public static Item I2(IEnumerable<short> value) => value.Any() ? I2(value.ToArray()) : I2();

        public static Item I4(params int[] value) => value.Length > 0 ? new Item(SecsFormat.I4, value, &EncodeArray) : I4();
        public static Item I4(IEnumerable<int> value) => value.Any() ? I4(value.ToArray()) : I4();

        public static Item I8(params long[] value) => value.Length > 0 ? new Item(SecsFormat.I8, value, &EncodeArray) : I8();
        public static Item I8(IEnumerable<long> value) => value.Any() ? I8(value.ToArray()) : I8();

        public static Item F4(params float[] value) => value.Length > 0 ? new Item(SecsFormat.F4, value, &EncodeArray) : F4();
        public static Item F4(IEnumerable<float> value) => value.Any() ? F4(value.ToArray()) : F4();

        public static Item F8(params double[] value) => value.Length > 0 ? new Item(SecsFormat.F8, value, &EncodeArray) : F8();
        public static Item F8(IEnumerable<double> value) => value.Any() ? F8(value.ToArray()) : F8();

        public static Item Boolean(params bool[] value) => value.Length > 0 ? new Item(SecsFormat.Boolean, value, &EncodeArray) : Boolean();
        public static Item Boolean(IEnumerable<bool> value) => value.Any() ? Boolean(value.ToArray()) : Boolean();

        public static Item A(string? value) => string.IsNullOrEmpty(value) ? A() : new Item(SecsFormat.ASCII, value, &EncodeString);

        public static Item J(string? value) => string.IsNullOrEmpty(value) ? J() : new Item(SecsFormat.JIS8, value, &EncodeString);

        public static Item L() => EmptyL;
        public static Item B() => EmptyBinary;
        public static Item U1() => EmptyU1;
        public static Item U2() => EmptyU2;
        public static Item U4() => EmptyU4;
        public static Item U8() => EmptyU8;
        public static Item I1() => EmptyI1;
        public static Item I2() => EmptyI2;
        public static Item I4() => EmptyI4;
        public static Item I8() => EmptyI8;
        public static Item F4() => EmptyF4;
        public static Item F8() => EmptyF8;
        public static Item Boolean() => EmptyBoolean;
        public static Item A() => EmptyA;
        public static Item J() => EmptyJ;

        /// <summary>
        /// Encode Item header
        /// </summary>
        /// <param name="count">List item count or value bytes length</param>
        private static void EncodeItemHeader(SecsFormat format, int count, IBufferWriter<byte> buffer)
        {
            var lengthSpan = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<int, byte>(ref count), sizeof(int));
            if (count <= 0xff)
            {//	1 byte
                var span = buffer.GetSpan(sizeHint: 2);
                span.DangerousGetReferenceAt(0) = (byte)((byte)format | 1);
                span.DangerousGetReferenceAt(1) = lengthSpan.DangerousGetReferenceAt(0);
                buffer.Advance(2);
                return;
            }
            if (count <= 0xffff)
            {//	2 byte
                var span = buffer.GetSpan(sizeHint: 3);
                span.DangerousGetReferenceAt(0) = (byte)((byte)format | 2);
                span.DangerousGetReferenceAt(1) = lengthSpan.DangerousGetReferenceAt(1);
                span.DangerousGetReferenceAt(2) = lengthSpan.DangerousGetReferenceAt(0);
                buffer.Advance(3);
                return;
            }
            if (count <= 0xffffff)
            {//	3 byte
                var span = buffer.GetSpan(sizeHint: 4);
                span.DangerousGetReferenceAt(0) = (byte)((byte)format | 3);
                span.DangerousGetReferenceAt(1) = lengthSpan.DangerousGetReferenceAt(2);
                span.DangerousGetReferenceAt(2) = lengthSpan.DangerousGetReferenceAt(1);
                span.DangerousGetReferenceAt(3) = lengthSpan.DangerousGetReferenceAt(0);
                buffer.Advance(4);
                return;
            }
            throw new ArgumentOutOfRangeException(nameof(count), count, $@"Item data length:{count} is overflow");
        }
    }
}
