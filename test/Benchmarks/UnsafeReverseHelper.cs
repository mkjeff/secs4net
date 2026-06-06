using Secs4Net.Extensions;
using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Benchmarks;

internal static class UnsafeReverseHelper
{
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
            rStart = BinaryPrimitives.ReadSingleBigEndian(rStart.AsReadOnlyBytes());
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
            rStart = BinaryPrimitives.ReadDoubleBigEndian(rStart.AsReadOnlyBytes());
            rStart = ref Unsafe.Add(ref rStart, 1u);
        }
    }
}
