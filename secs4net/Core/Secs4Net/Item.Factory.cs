using Microsoft.Toolkit.HighPerformance.Buffers;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace Secs4Net
{
    partial class Item
    {
        public static Item L(IList<Item> items) => items.Count > 0 ? new ListItem(SecsFormat.List, items) : L();
        public static Item L(IEnumerable<Item> items) => items.Any() ? L(items.ToList()) : L();
        public static Item L(params Item[] items) => items.Length > 0 ? L((IList<Item>)items) : L();
        
        public static Item A(string? value) => string.IsNullOrEmpty(value) ? A() : new StringItem(SecsFormat.ASCII, value);
        public static Item J(string? value) => string.IsNullOrEmpty(value) ? J() : new StringItem(SecsFormat.JIS8, value);

        public static Item B(params byte[] value) => value.Length > 0 ? new MemoryItem<byte>(SecsFormat.Binary, value) : B();
        public static Item B(IEnumerable<byte> value) => value.Any() ? B(value.ToArray()) : B();
        public static Item B(MemoryOwner<byte> valueOwner) => new MemoryOwnerItem<byte>(SecsFormat.Binary, valueOwner);

        public static Item U1(params byte[] value) => value.Length > 0 ? new MemoryItem<byte>(SecsFormat.U1, value) : U1();
        public static Item U1(IEnumerable<byte> value) => value.Any() ? U1(value.ToArray()) : U1();
        public static Item U1(MemoryOwner<byte> valueOwner) => new MemoryOwnerItem<byte>(SecsFormat.U1, valueOwner);

        public static Item U2(params ushort[] value) => value.Length > 0 ? new MemoryItem<ushort>(SecsFormat.U2, value) : U2();
        public static Item U2(IEnumerable<ushort> value) => value.Any() ? U2(value.ToArray()) : U2(); 
        public static Item U2(MemoryOwner<ushort> valueOwner) => new MemoryOwnerItem<ushort>(SecsFormat.U2, valueOwner);

        public static Item U4(params uint[] value) => value.Length > 0 ? new MemoryItem<uint>(SecsFormat.U4, value) : U4();
        public static Item U4(IEnumerable<uint> value) => value.Any() ? U4(value.ToArray()) : U4();
        public static Item U4(MemoryOwner<uint> valueOwner) => new MemoryOwnerItem<uint>(SecsFormat.U4, valueOwner);

        public static Item U8(params ulong[] value) => value.Length > 0 ? new MemoryItem<ulong>(SecsFormat.U8, value) : U8();
        public static Item U8(IEnumerable<ulong> value) => value.Any() ? U8(value.ToArray()) : U8();
        public static Item U8(MemoryOwner<ulong> valueOwner) => new MemoryOwnerItem<ulong>(SecsFormat.U8, valueOwner);

        public static Item I1(params sbyte[] value) => value.Length > 0 ? new MemoryItem<sbyte>(SecsFormat.I1, value) : I1();
        public static Item I1(IEnumerable<sbyte> value) => value.Any() ? I1(value.ToArray()) : I1();
        public static Item I1(MemoryOwner<sbyte> valueOwner) => new MemoryOwnerItem<sbyte>(SecsFormat.I1, valueOwner);

        public static Item I2(params short[] value) => value.Length > 0 ? new MemoryItem<short>(SecsFormat.I2, value) : I2();
        public static Item I2(IEnumerable<short> value) => value.Any() ? I2(value.ToArray()) : I2();
        public static Item I2(MemoryOwner<short> valueOwner) => new MemoryOwnerItem<short>(SecsFormat.I2, valueOwner);

        public static Item I4(params int[] value) => value.Length > 0 ? new MemoryItem<int>(SecsFormat.I4, value) : I4();
        public static Item I4(IEnumerable<int> value) => value.Any() ? I4(value.ToArray()) : I4();
        public static Item I4(MemoryOwner<int> valueOwner) => new MemoryOwnerItem<int>(SecsFormat.I4, valueOwner);

        public static Item I8(params long[] value) => value.Length > 0 ? new MemoryItem<long>(SecsFormat.I8, value) : I8();
        public static Item I8(IEnumerable<long> value) => value.Any() ? I8(value.ToArray()) : I8();
        public static Item I8(MemoryOwner<long> valueOwner) => new MemoryOwnerItem<long>(SecsFormat.I8, valueOwner);

        public static Item F4(params float[] value) => value.Length > 0 ? new MemoryItem<float>(SecsFormat.F4, value) : F4();
        public static Item F4(IEnumerable<float> value) => value.Any() ? F4(value.ToArray()) : F4();
        public static Item F4(MemoryOwner<float> valueOwner) => new MemoryOwnerItem<float>(SecsFormat.F4, valueOwner);

        public static Item F8(params double[] value) => value.Length > 0 ? new MemoryItem<double>(SecsFormat.F8, value) : F8();
        public static Item F8(IEnumerable<double> value) => value.Any() ? F8(value.ToArray()) : F8();
        public static Item F8(MemoryOwner<double> valueOwner) => new MemoryOwnerItem<double>(SecsFormat.F8, valueOwner);

        public static Item Boolean(params bool[] value) => value.Length > 0 ? new MemoryItem<bool>(SecsFormat.Boolean, value) : Boolean();
        public static Item Boolean(IEnumerable<bool> value) => value.Any() ? Boolean(value.ToArray()) : Boolean();
        public static Item Boolean(MemoryOwner<bool> valueOwner) => new MemoryOwnerItem<bool>(SecsFormat.Boolean, valueOwner);

        public static Item L() => EmptyL;
        public static Item A() => EmptyA;
        public static Item J() => EmptyJ;
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
    }
}
