using System.Buffers;
using System.Runtime.CompilerServices;
using static System.Runtime.CompilerServices.MethodImplOptions;

namespace Secs4Net;

partial class Item
{
    public static Item L(IEnumerable<Item> items) => L(items.ToArray());
    public static Item L(params Item[] items) => items.Length > 0 ? new ListItem(items) : EmptyL;

    public static Item A(string? value) => string.IsNullOrEmpty(value) ? EmptyA : new StringItem(SecsFormat.ASCII, value);
    public static Item J(string? value) => string.IsNullOrEmpty(value) ? EmptyJ : new StringItem(SecsFormat.JIS8, value);

    [MethodImpl(AggressiveInlining)] public static Item B(params byte[] value) => B(value.AsMemory());
    [MethodImpl(AggressiveInlining)] public static Item B(IEnumerable<byte> value) => B(value.ToArray());
    [MethodImpl(AggressiveInlining)] public static Item B(Memory<byte> value) => new MemoryItem<byte>(SecsFormat.Binary, value);
    [MethodImpl(AggressiveInlining)] public static Item B(IMemoryOwner<byte> valueOwner) => new MemoryOwnerItem<byte>(SecsFormat.Binary, valueOwner);

    [MethodImpl(AggressiveInlining)] public static Item U1(params byte[] value) => U1(value.AsMemory());
    [MethodImpl(AggressiveInlining)] public static Item U1(IEnumerable<byte> value) => U1(value.ToArray());
    [MethodImpl(AggressiveInlining)] public static Item U1(Memory<byte> value) => new MemoryItem<byte>(SecsFormat.U1, value);
    [MethodImpl(AggressiveInlining)] public static Item U1(IMemoryOwner<byte> valueOwner) => new MemoryOwnerItem<byte>(SecsFormat.U1, valueOwner);

    [MethodImpl(AggressiveInlining)] public static Item U2(params ushort[] value) => U2(value.AsMemory());
    [MethodImpl(AggressiveInlining)] public static Item U2(IEnumerable<ushort> value) => U2(value.ToArray());
    [MethodImpl(AggressiveInlining)] public static Item U2(Memory<ushort> value) => new MemoryItem<ushort>(SecsFormat.U2, value);
    [MethodImpl(AggressiveInlining)] public static Item U2(IMemoryOwner<ushort> valueOwner) => new MemoryOwnerItem<ushort>(SecsFormat.U2, valueOwner);

    [MethodImpl(AggressiveInlining)] public static Item U4(params uint[] value) => U4(value.AsMemory());
    [MethodImpl(AggressiveInlining)] public static Item U4(IEnumerable<uint> value) => U4(value.ToArray());
    [MethodImpl(AggressiveInlining)] public static Item U4(Memory<uint> value) => new MemoryItem<uint>(SecsFormat.U4, value);
    [MethodImpl(AggressiveInlining)] public static Item U4(IMemoryOwner<uint> valueOwner) => new MemoryOwnerItem<uint>(SecsFormat.U4, valueOwner);

    [MethodImpl(AggressiveInlining)] public static Item U8(params ulong[] value) => U8(value.AsMemory());
    [MethodImpl(AggressiveInlining)] public static Item U8(IEnumerable<ulong> value) => U8(value.ToArray());
    [MethodImpl(AggressiveInlining)] public static Item U8(Memory<ulong> value) => new MemoryItem<ulong>(SecsFormat.U8, value);
    [MethodImpl(AggressiveInlining)] public static Item U8(IMemoryOwner<ulong> valueOwner) => new MemoryOwnerItem<ulong>(SecsFormat.U8, valueOwner);

    [MethodImpl(AggressiveInlining)] public static Item I1(params sbyte[] value) => I1(value.AsMemory());
    [MethodImpl(AggressiveInlining)] public static Item I1(IEnumerable<sbyte> value) => I1(value.ToArray());
    [MethodImpl(AggressiveInlining)] public static Item I1(Memory<sbyte> value) => new MemoryItem<sbyte>(SecsFormat.I1, value);
    [MethodImpl(AggressiveInlining)] public static Item I1(IMemoryOwner<sbyte> valueOwner) => new MemoryOwnerItem<sbyte>(SecsFormat.I1, valueOwner);

    [MethodImpl(AggressiveInlining)] public static Item I2(params short[] value) => I2(value.AsMemory());
    [MethodImpl(AggressiveInlining)] public static Item I2(IEnumerable<short> value) => I2(value.ToArray());
    [MethodImpl(AggressiveInlining)] public static Item I2(Memory<short> value) => new MemoryItem<short>(SecsFormat.I2, value);
    [MethodImpl(AggressiveInlining)] public static Item I2(IMemoryOwner<short> valueOwner) => new MemoryOwnerItem<short>(SecsFormat.I2, valueOwner);

    [MethodImpl(AggressiveInlining)] public static Item I4(params int[] value) => I4(value.AsMemory());
    [MethodImpl(AggressiveInlining)] public static Item I4(IEnumerable<int> value) => I4(value.ToArray());
    [MethodImpl(AggressiveInlining)] public static Item I4(Memory<int> value) => new MemoryItem<int>(SecsFormat.I4, value);
    [MethodImpl(AggressiveInlining)] public static Item I4(IMemoryOwner<int> valueOwner) => new MemoryOwnerItem<int>(SecsFormat.I4, valueOwner);

    [MethodImpl(AggressiveInlining)] public static Item I8(params long[] value) => I8(value.AsMemory());
    [MethodImpl(AggressiveInlining)] public static Item I8(IEnumerable<long> value) => I8(value.ToArray());
    [MethodImpl(AggressiveInlining)] public static Item I8(Memory<long> value) => new MemoryItem<long>(SecsFormat.I8, value);
    [MethodImpl(AggressiveInlining)] public static Item I8(IMemoryOwner<long> valueOwner) => new MemoryOwnerItem<long>(SecsFormat.I8, valueOwner);

    [MethodImpl(AggressiveInlining)] public static Item F4(params float[] value) => F4(value.AsMemory());
    [MethodImpl(AggressiveInlining)] public static Item F4(IEnumerable<float> value) => F4(value.ToArray());
    [MethodImpl(AggressiveInlining)] public static Item F4(Memory<float> value) => new MemoryItem<float>(SecsFormat.F4, value);
    [MethodImpl(AggressiveInlining)] public static Item F4(IMemoryOwner<float> valueOwner) => new MemoryOwnerItem<float>(SecsFormat.F4, valueOwner);

    [MethodImpl(AggressiveInlining)] public static Item F8(params double[] value) => F8(value.AsMemory());
    [MethodImpl(AggressiveInlining)] public static Item F8(IEnumerable<double> value) => F8(value.ToArray());
    [MethodImpl(AggressiveInlining)] public static Item F8(Memory<double> value) => new MemoryItem<double>(SecsFormat.F8, value);
    [MethodImpl(AggressiveInlining)] public static Item F8(IMemoryOwner<double> valueOwner) => new MemoryOwnerItem<double>(SecsFormat.F8, valueOwner);

    [MethodImpl(AggressiveInlining)] public static Item Boolean(params bool[] value) => Boolean(value.AsMemory());
    [MethodImpl(AggressiveInlining)] public static Item Boolean(IEnumerable<bool> value) => Boolean(value.ToArray());
    [MethodImpl(AggressiveInlining)] public static Item Boolean(Memory<bool> value) => new MemoryItem<bool>(SecsFormat.Boolean, value);
    [MethodImpl(AggressiveInlining)] public static Item Boolean(IMemoryOwner<bool> valueOwner) => new MemoryOwnerItem<bool>(SecsFormat.Boolean, valueOwner);

    [MethodImpl(AggressiveInlining)] public static Item L() => EmptyL;
    [MethodImpl(AggressiveInlining)] public static Item A() => EmptyA;
    [MethodImpl(AggressiveInlining)] public static Item J() => EmptyJ;
    [MethodImpl(AggressiveInlining)] public static Item B() => EmptyBinary;
    [MethodImpl(AggressiveInlining)] public static Item U1() => EmptyU1;
    [MethodImpl(AggressiveInlining)] public static Item U2() => EmptyU2;
    [MethodImpl(AggressiveInlining)] public static Item U4() => EmptyU4;
    [MethodImpl(AggressiveInlining)] public static Item U8() => EmptyU8;
    [MethodImpl(AggressiveInlining)] public static Item I1() => EmptyI1;
    [MethodImpl(AggressiveInlining)] public static Item I2() => EmptyI2;
    [MethodImpl(AggressiveInlining)] public static Item I4() => EmptyI4;
    [MethodImpl(AggressiveInlining)] public static Item I8() => EmptyI8;
    [MethodImpl(AggressiveInlining)] public static Item F4() => EmptyF4;
    [MethodImpl(AggressiveInlining)] public static Item F8() => EmptyF8;
    [MethodImpl(AggressiveInlining)] public static Item Boolean() => EmptyBoolean;

    private static readonly Item EmptyL = new ListItem([]);
    private static readonly Item EmptyA = new StringItem(SecsFormat.ASCII, string.Empty);
    private static readonly Item EmptyJ = new StringItem(SecsFormat.JIS8, string.Empty);
    private static readonly Item EmptyBoolean = Boolean(Memory<bool>.Empty);
    private static readonly Item EmptyBinary = B(Memory<byte>.Empty);
    private static readonly Item EmptyU1 = U1(Memory<byte>.Empty);
    private static readonly Item EmptyU2 = U2(Memory<ushort>.Empty);
    private static readonly Item EmptyU4 = U4(Memory<uint>.Empty);
    private static readonly Item EmptyU8 = U8(Memory<ulong>.Empty);
    private static readonly Item EmptyI1 = I1(Memory<sbyte>.Empty);
    private static readonly Item EmptyI2 = I2(Memory<short>.Empty);
    private static readonly Item EmptyI4 = I4(Memory<int>.Empty);
    private static readonly Item EmptyI8 = I8(Memory<long>.Empty);
    private static readonly Item EmptyF4 = F4(Memory<float>.Empty);
    private static readonly Item EmptyF8 = F8(Memory<double>.Empty);
}
