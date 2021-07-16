using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace Secs4Net
{
    unsafe partial class Item
    {
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
    }
}
