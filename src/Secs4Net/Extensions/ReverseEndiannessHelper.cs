using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace Secs4Net.Extensions;

public unsafe static class ReverseEndiannessHelper<T> where T : unmanaged
{
    public static readonly delegate*<Span<T>, void> Reverse;

    static ReverseEndiannessHelper()
    {
        var t = typeof(T);
        if (t == typeof(ushort))
        {
            Reverse = (delegate*<Span<T>, void>)ReverseHelper.ReverseUInt16;
        }
        else if (t == typeof(uint))
        {
            Reverse = (delegate*<Span<T>, void>)ReverseHelper.ReverseUInt32;
        }
        else if (t == typeof(ulong))
        {
            Reverse = (delegate*<Span<T>, void>)ReverseHelper.ReverseUInt64;
        }
        else if (t == typeof(short))
        {
            Reverse = (delegate*<Span<T>, void>)ReverseHelper.ReverseInt16;
        }
        else if (t == typeof(int))
        {
            Reverse = (delegate*<Span<T>, void>)ReverseHelper.ReverseInt32;
        }
        else if (t == typeof(long))
        {
            Reverse = (delegate*<Span<T>, void>)ReverseHelper.ReverseInt64;
        }
        else if (t == typeof(float))
        {
            Reverse = (delegate*<Span<T>, void>)ReverseHelper.ReverseSingle;
        }
        else if (t == typeof(double))
        {
            Reverse = (delegate*<Span<T>, void>)ReverseHelper.ReverseDouble;
        }
        else
        {
            Reverse = &ReverseNothing;
        }

        static void ReverseNothing(Span<T> bytes) { }
    }
}

public unsafe static class ReverseHelper
{
    internal static readonly delegate*<Span<ushort>, void> ReverseUInt16 = &ReverseEndianness;
    internal static readonly delegate*<Span<uint>, void> ReverseUInt32 = &ReverseEndianness;
    internal static readonly delegate*<Span<ulong>, void> ReverseUInt64 = &ReverseEndianness;
    internal static readonly delegate*<Span<short>, void> ReverseInt16 = &ReverseEndianness;
    internal static readonly delegate*<Span<int>, void> ReverseInt32 = &ReverseEndianness;
    internal static readonly delegate*<Span<long>, void> ReverseInt64 = &ReverseEndianness;
    internal static readonly delegate*<Span<float>, void> ReverseSingle = &ReverseEndianness;
    internal static readonly delegate*<Span<double>, void> ReverseDouble = &ReverseEndianness;


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReverseEndianness(this Span<short> span)
    {
        ref var rStart = ref MemoryMarshal.GetReference(span);
        ref var rEnd = ref Unsafe.Add(ref rStart, span.Length);
        while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
        {
            rStart = BinaryPrimitives.ReverseEndianness(rStart);
            rStart = ref Unsafe.Add(ref rStart, 1u);
        }
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReverseEndianness(this Span<ushort> span)
    {
        ref var rStart = ref MemoryMarshal.GetReference(span);
        ref var rEnd = ref Unsafe.Add(ref rStart, span.Length);
        while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
        {
            rStart = BinaryPrimitives.ReverseEndianness(rStart);
            rStart = ref Unsafe.Add(ref rStart, 1u);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReverseEndianness(this Span<int> span)
    {
        ref var rStart = ref MemoryMarshal.GetReference(span);
        ref var rEnd = ref Unsafe.Add(ref rStart, span.Length);
        while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
        {
            rStart = BinaryPrimitives.ReverseEndianness(rStart);
            rStart = ref Unsafe.Add(ref rStart, 1u);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReverseEndianness(this Span<uint> span)
    {
        ref var rStart = ref MemoryMarshal.GetReference(span);
        ref var rEnd = ref Unsafe.Add(ref rStart, span.Length);
        while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
        {
            rStart = BinaryPrimitives.ReverseEndianness(rStart);
            rStart = ref Unsafe.Add(ref rStart, 1u);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReverseEndianness(this Span<long> span)
    {
        ref var rStart = ref MemoryMarshal.GetReference(span);
        ref var rEnd = ref Unsafe.Add(ref rStart, span.Length);
        while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
        {
            rStart = BinaryPrimitives.ReverseEndianness(rStart);
            rStart = ref Unsafe.Add(ref rStart, 1u);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReverseEndianness(this Span<ulong> span)
    {
        ref var rStart = ref MemoryMarshal.GetReference(span);
        ref var rEnd = ref Unsafe.Add(ref rStart, span.Length);
        while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
        {
            rStart = BinaryPrimitives.ReverseEndianness(rStart);
            rStart = ref Unsafe.Add(ref rStart, 1u);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReverseEndianness(this Span<float> span)
    {
        ref var rStart = ref MemoryMarshal.GetReference(span);
        ref var rEnd = ref Unsafe.Add(ref rStart, span.Length);
        while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
        {
            ReverseEndianness(ref rStart);
            rStart = ref Unsafe.Add(ref rStart, 1u);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReverseEndianness(this Span<double> span)
    {
        ref var rStart = ref MemoryMarshal.GetReference(span);
        ref var rEnd = ref Unsafe.Add(ref rStart, span.Length);
        while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
        {
            ReverseEndianness(ref rStart);
            rStart = ref Unsafe.Add(ref rStart, 1u);
        }
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ReverseEndianness(ref float value)
    {
#if NET
        value = BinaryPrimitives.ReadSingleBigEndian(value.AsReadOnlyBytes());
#else
        value = ReadSingleBigEndian(value.AsReadOnlyBytes());
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ReverseEndianness(ref double value)
    {
#if NET
        value = BinaryPrimitives.ReadDoubleBigEndian(value.AsReadOnlyBytes());
#else
        value = ReadDoubleBigEndian(value.AsReadOnlyBytes());
#endif
    }

#if !NET
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float ReadSingleBigEndian(ReadOnlySpan<byte> source)
        => Int32BitsToSingle(BinaryPrimitives.ReverseEndianness(MemoryMarshal.Read<int>(source)));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float Int32BitsToSingle(int value)
        => *(float*)&value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double ReadDoubleBigEndian(ReadOnlySpan<byte> source)
        => BitConverter.Int64BitsToDouble(BinaryPrimitives.ReverseEndianness(MemoryMarshal.Read<long>(source)));
#endif
}
