using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Secs4Net
{
    partial class Item
    {
        public static Item L(IEnumerable<Item> items) => L(items.ToArray());
        public static Item L(params Item[] items) => items.Length > 0 ? new ListItem(SecsFormat.List, items) : L();

        public static Item A(string? value) => string.IsNullOrEmpty(value) ? A() : new StringItem(SecsFormat.ASCII, value!);
        public static Item J(string? value) => string.IsNullOrEmpty(value) ? J() : new StringItem(SecsFormat.JIS8, value!);

        public static Item B(params byte[] value) => B(value.AsMemory());
        public static Item B(IEnumerable<byte> value) => B(value.ToArray());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Item B(ReadOnlyMemory<byte> value) => value.IsEmpty ? B() : new MemoryItem<byte>(SecsFormat.Binary, MemoryMarshal.AsMemory(value));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Item B(IMemoryOwner<byte> valueOwner) => valueOwner.Memory.IsEmpty ? B() : new MemoryOwnerItem<byte>(SecsFormat.Binary, valueOwner);

        public static Item U1(params byte[] value) => U1(value.AsMemory());
        public static Item U1(IEnumerable<byte> value) => U1(value.ToArray());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Item U1(ReadOnlyMemory<byte> value) => value.IsEmpty ? U1() : new MemoryItem<byte>(SecsFormat.U1, MemoryMarshal.AsMemory(value));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Item U1(IMemoryOwner<byte> valueOwner) => valueOwner.Memory.IsEmpty ? U1() : new MemoryOwnerItem<byte>(SecsFormat.U1, valueOwner);

        public static Item U2(params ushort[] value) => U2(value.AsMemory());
        public static Item U2(IEnumerable<ushort> value) => U2(value.ToArray());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Item U2(ReadOnlyMemory<ushort> value) => value.IsEmpty ? U2() : new MemoryItem<ushort>(SecsFormat.U2, MemoryMarshal.AsMemory(value));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Item U2(IMemoryOwner<ushort> valueOwner) => valueOwner.Memory.IsEmpty ? U2() : new MemoryOwnerItem<ushort>(SecsFormat.U2, valueOwner);

        public static Item U4(params uint[] value) => U4(value.AsMemory());
        public static Item U4(IEnumerable<uint> value) => U4(value.ToArray());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Item U4(ReadOnlyMemory<uint> value) => value.IsEmpty ? U4() : new MemoryItem<uint>(SecsFormat.U4, MemoryMarshal.AsMemory(value));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Item U4(IMemoryOwner<uint> valueOwner) => valueOwner.Memory.IsEmpty ? U4() : new MemoryOwnerItem<uint>(SecsFormat.U4, valueOwner);

        public static Item U8(params ulong[] value) => U8(value.AsMemory());
        public static Item U8(IEnumerable<ulong> value) => U8(value.ToArray());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Item U8(ReadOnlyMemory<ulong> value) => value.IsEmpty ? U8() : new MemoryItem<ulong>(SecsFormat.U8, MemoryMarshal.AsMemory(value));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Item U8(IMemoryOwner<ulong> valueOwner) => valueOwner.Memory.IsEmpty ? U8() : new MemoryOwnerItem<ulong>(SecsFormat.U8, valueOwner);

        public static Item I1(params sbyte[] value) => I1(value.AsMemory());
        public static Item I1(IEnumerable<sbyte> value) => I1(value.ToArray());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Item I1(ReadOnlyMemory<sbyte> value) => value.IsEmpty ? I1() : new MemoryItem<sbyte>(SecsFormat.I1, MemoryMarshal.AsMemory(value));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Item I1(IMemoryOwner<sbyte> valueOwner) => valueOwner.Memory.IsEmpty ? I1() : new MemoryOwnerItem<sbyte>(SecsFormat.I1, valueOwner);

        public static Item I2(params short[] value) => I2(value.AsMemory());
        public static Item I2(IEnumerable<short> value) => I2(value.ToArray());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Item I2(ReadOnlyMemory<short> value) => value.IsEmpty ? I2() : new MemoryItem<short>(SecsFormat.I2, MemoryMarshal.AsMemory(value));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Item I2(IMemoryOwner<short> valueOwner) => valueOwner.Memory.IsEmpty ? I2() : new MemoryOwnerItem<short>(SecsFormat.I2, valueOwner);

        public static Item I4(params int[] value) => I4(value.AsMemory());
        public static Item I4(IEnumerable<int> value) => I4(value.ToArray());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Item I4(ReadOnlyMemory<int> value) => value.IsEmpty ? I4() : new MemoryItem<int>(SecsFormat.I4, MemoryMarshal.AsMemory(value));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Item I4(IMemoryOwner<int> valueOwner) => valueOwner.Memory.IsEmpty ? I4() : new MemoryOwnerItem<int>(SecsFormat.I4, valueOwner);

        public static Item I8(params long[] value) => I8(value.AsMemory());
        public static Item I8(IEnumerable<long> value) => I8(value.ToArray());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Item I8(ReadOnlyMemory<long> value) => value.IsEmpty ? I8() : new MemoryItem<long>(SecsFormat.I8, MemoryMarshal.AsMemory(value));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Item I8(IMemoryOwner<long> valueOwner) => valueOwner.Memory.IsEmpty ? I8() : new MemoryOwnerItem<long>(SecsFormat.I8, valueOwner);

        public static Item F4(params float[] value) => F4(value.AsMemory());
        public static Item F4(IEnumerable<float> value) => F4(value.ToArray());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Item F4(ReadOnlyMemory<float> value) => value.IsEmpty ? F4() : new MemoryItem<float>(SecsFormat.F4, MemoryMarshal.AsMemory(value));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Item F4(IMemoryOwner<float> valueOwner) => valueOwner.Memory.IsEmpty ? F4() : new MemoryOwnerItem<float>(SecsFormat.F4, valueOwner);

        public static Item F8(params double[] value) => F8(value.AsMemory());
        public static Item F8(IEnumerable<double> value) => F8(value.ToArray());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Item F8(ReadOnlyMemory<double> value) => value.IsEmpty ? F8() : new MemoryItem<double>(SecsFormat.F8, MemoryMarshal.AsMemory(value));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Item F8(IMemoryOwner<double> valueOwner) => valueOwner.Memory.IsEmpty ? F8() : new MemoryOwnerItem<double>(SecsFormat.F8, valueOwner);

        public static Item Boolean(params bool[] value) => Boolean(value.AsMemory());
        public static Item Boolean(IEnumerable<bool> value) => Boolean(value.ToArray());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Item Boolean(ReadOnlyMemory<bool> value) => value.IsEmpty ? Boolean() : new MemoryItem<bool>(SecsFormat.Boolean, MemoryMarshal.AsMemory(value));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Item Boolean(IMemoryOwner<bool> valueOwner) => valueOwner.Memory.IsEmpty ? Boolean() : new MemoryOwnerItem<bool>(SecsFormat.Boolean, valueOwner);

        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Item L() => EmptyL;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Item A() => EmptyA;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Item J() => EmptyJ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Item B() => EmptyBinary;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Item U1() => EmptyU1;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Item U2() => EmptyU2;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Item U4() => EmptyU4;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Item U8() => EmptyU8;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Item I1() => EmptyI1;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Item I2() => EmptyI2;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Item I4() => EmptyI4;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Item I8() => EmptyI8;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Item F4() => EmptyF4;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Item F8() => EmptyF8;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Item Boolean() => EmptyBoolean;
    }
}
