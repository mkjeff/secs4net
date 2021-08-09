using Microsoft.Toolkit.HighPerformance;
using PooledAwait;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Secs4Net.Extensions
{
    public static class SecsExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ChunkedSpan<T> Chunk<T>(ref this Span<T> span, int count) => new(span, count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ChunkedReadOnlySpan<T> Chunk<T>(ref this ReadOnlySpan<T> span, int count) => new(span, count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ChunkedMemory<T> Chunk<T>(this Memory<T> memory, int count) => new(memory, count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ChunkedReadOnlyMemory<T> Chunk<T>(this ReadOnlyMemory<T> memory, int count) => new(memory, count);

        public static IEnumerable<Memory<T>> AsEnumerable<T>(this ChunkedMemory<T> source)
        {
            foreach (var m in source)
            {
                yield return m;
            }
        }

        public static IEnumerable<ReadOnlyMemory<T>> AsEnumerable<T>(this ChunkedReadOnlyMemory<T> source)
        {
            foreach (var m in source)
            {
                yield return m;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetName(this SecsFormat format)
        {
            return format switch
            {
                SecsFormat.List => nameof(SecsFormat.List),
                SecsFormat.Binary => nameof(SecsFormat.Binary),
                SecsFormat.Boolean => nameof(SecsFormat.Boolean),
                SecsFormat.ASCII => nameof(SecsFormat.ASCII),
                SecsFormat.JIS8 => nameof(SecsFormat.JIS8),
                SecsFormat.I8 => nameof(SecsFormat.I8),
                SecsFormat.I1 => nameof(SecsFormat.I1),
                SecsFormat.I2 => nameof(SecsFormat.I2),
                SecsFormat.I4 => nameof(SecsFormat.I4),
                SecsFormat.F8 => nameof(SecsFormat.F8),
                SecsFormat.F4 => nameof(SecsFormat.F4),
                SecsFormat.U8 => nameof(SecsFormat.U8),
                SecsFormat.U1 => nameof(SecsFormat.U1),
                SecsFormat.U2 => nameof(SecsFormat.U2),
                SecsFormat.U4 => nameof(SecsFormat.U4),
                _ => ThrowHelper(format),
            };

            [DoesNotReturn]
            static string ThrowHelper(SecsFormat format) => throw new ArgumentOutOfRangeException(nameof(format), (int)format, "Invalid enum value");
        }

#if NET
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> AsBytes<T>(this ref T value) where T : unmanaged
            => MemoryMarshal.CreateSpan(ref Unsafe.As<T, byte>(ref value), Unsafe.SizeOf<T>());
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Span<byte> AsBytes<T>(this ref T value) where T : unmanaged
            => new(Unsafe.AsPointer(ref value), Unsafe.SizeOf<T>());
#endif

#if NET
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Span<T> Cast<T>(this Span<byte> bytes) where T : unmanaged
            => MemoryMarshal.CreateSpan(ref Unsafe.As<byte, T>(ref MemoryMarshal.GetReference(bytes)), bytes.Length / Unsafe.SizeOf<T>());
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe Span<T> Cast<T>(this Span<byte> bytes) where T : unmanaged
            => new(Unsafe.AsPointer(ref MemoryMarshal.GetReference(bytes)), bytes.Length / Unsafe.SizeOf<T>());
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseEndianness(this Span<short> span)
        {
            ref var rStart = ref span.DangerousGetReferenceAt(0);
            ref var rEnd = ref span.DangerousGetReferenceAt(span.Length);
            while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
            {
                rStart = BinaryPrimitives.ReverseEndianness(rStart);
                rStart = ref Unsafe.Add(ref rStart, 1);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseEndianness(this Span<ushort> span)
        {
            ref var rStart = ref span.DangerousGetReferenceAt(0);
            ref var rEnd = ref span.DangerousGetReferenceAt(span.Length);
            while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
            {
                rStart = BinaryPrimitives.ReverseEndianness(rStart);
                rStart = ref Unsafe.Add(ref rStart, 1);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseEndianness(this Span<int> span)
        {
            ref var rStart = ref span.DangerousGetReferenceAt(0);
            ref var rEnd = ref span.DangerousGetReferenceAt(span.Length);
            while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
            {
                rStart = BinaryPrimitives.ReverseEndianness(rStart);
                rStart = ref Unsafe.Add(ref rStart, 1);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseEndianness(this Span<uint> span)
        {
            ref var rStart = ref span.DangerousGetReferenceAt(0);
            ref var rEnd = ref span.DangerousGetReferenceAt(span.Length);
            while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
            {
                rStart = BinaryPrimitives.ReverseEndianness(rStart);
                rStart = ref Unsafe.Add(ref rStart, 1);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseEndianness(this Span<long> span)
        {
            ref var rStart = ref span.DangerousGetReferenceAt(0);
            ref var rEnd = ref span.DangerousGetReferenceAt(span.Length);
            while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
            {
                rStart = BinaryPrimitives.ReverseEndianness(rStart);
                rStart = ref Unsafe.Add(ref rStart, 1);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseEndianness(this Span<ulong> span)
        {
            ref var rStart = ref span.DangerousGetReferenceAt(0);
            ref var rEnd = ref span.DangerousGetReferenceAt(span.Length);
            while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
            {
                rStart = BinaryPrimitives.ReverseEndianness(rStart);
                rStart = ref Unsafe.Add(ref rStart, 1);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseEndianness(this Span<float> span)
        {
            ref var rStart = ref span.DangerousGetReferenceAt(0);
            ref var rEnd = ref span.DangerousGetReferenceAt(span.Length);
            while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
            {
#if NET
                rStart = BinaryPrimitives.ReadSingleBigEndian(rStart.AsBytes());
#else
                rStart = ReadSingleBigEndian(rStart.AsBytes());
#endif
                rStart = ref Unsafe.Add(ref rStart, 1);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseEndianness(this Span<double> span)
        {
            ref var rStart = ref span.DangerousGetReferenceAt(0);
            ref var rEnd = ref span.DangerousGetReferenceAt(span.Length);
            while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
            {
#if NET
                rStart = BinaryPrimitives.ReadDoubleBigEndian(rStart.AsBytes());
#else
                rStart = ReadDoubleBigEndian(rStart.AsBytes());
#endif
                rStart = ref Unsafe.Add(ref rStart, 1);
            }
        }

#if NETSTANDARD
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ReadSingleBigEndian(ReadOnlySpan<byte> source)
        {
            return Int32BitsToSingle(BinaryPrimitives.ReverseEndianness(MemoryMarshal.Read<int>(source)));
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            unsafe static float Int32BitsToSingle(int value)
                => *(float*)&value;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ReadDoubleBigEndian(ReadOnlySpan<byte> source)
            => BitConverter.Int64BitsToDouble(BinaryPrimitives.ReverseEndianness(MemoryMarshal.Read<long>(source)));
#endif

        internal static string GetDebugString(this Item item, int maxCount)
        {
            var sb = new StringBuilder();
            sb.Append(item.Format.GetName()).Append('[').Append(item.Count).Append("]: ");
            return item.Format switch
            {
                SecsFormat.List => sb.Append("...").ToString(),
                SecsFormat.ASCII or SecsFormat.JIS8 => sb.Append(item.GetString()).ToString(),
                SecsFormat.Binary => AppendBinary(sb, item.GetReadOnlyMemory<byte>().Span, maxCount).ToString(),
                SecsFormat.Boolean => AppendMemoryItem<bool>(sb, item, maxCount).ToString(),
                SecsFormat.I1 => AppendMemoryItem<sbyte>(sb, item, maxCount).ToString(),
                SecsFormat.I2 => AppendMemoryItem<short>(sb, item, maxCount).ToString(),
                SecsFormat.I4 => AppendMemoryItem<int>(sb, item, maxCount).ToString(),
                SecsFormat.I8 => AppendMemoryItem<long>(sb, item, maxCount).ToString(),
                SecsFormat.U1 => AppendMemoryItem<byte>(sb, item, maxCount).ToString(),
                SecsFormat.U2 => AppendMemoryItem<ushort>(sb, item, maxCount).ToString(),
                SecsFormat.U4 => AppendMemoryItem<uint>(sb, item, maxCount).ToString(),
                SecsFormat.U8 => AppendMemoryItem<ulong>(sb, item, maxCount).ToString(),
                SecsFormat.F4 => AppendMemoryItem<float>(sb, item, maxCount).ToString(),
                SecsFormat.F8 => AppendMemoryItem<double>(sb, item, maxCount).ToString(),
                _ => sb.ToString(),
            };

            static StringBuilder AppendMemoryItem<T>(StringBuilder sb, Item item, int maxCount) where T : unmanaged
            {
                var arrary = item.GetReadOnlyMemory<T>().Span;
                if (arrary.IsEmpty)
                {
                    return sb;
                }

                var len = Math.Min(arrary.Length, maxCount);
                for (int i = 0; i < len - 1; i++)
                {
                    sb.Append(arrary.DangerousGetReferenceAt(i).ToString()).Append(' ');
                }

                sb.Append(arrary.DangerousGetReferenceAt(len - 1).ToString());
                if (len < arrary.Length)
                {
                    sb.Append(" ...");
                }

                return sb;
            }

            static StringBuilder AppendBinary(StringBuilder sb, ReadOnlySpan<byte> array, int maxCount)
            {
                if (array.IsEmpty)
                {
                    return sb;
                }

                var len = Math.Min(array.Length, maxCount);
                for (int i = 0; i < len - 1; i++)
                {
                    AppendHexChars(sb, array.DangerousGetReferenceAt(i));
                    sb.Append(' ');
                }

                AppendHexChars(sb, array.DangerousGetReferenceAt(len - 1));
                if (len < array.Length)
                {
                    sb.Append(" ...");
                }

                return sb;

                static void AppendHexChars(StringBuilder sb, byte num)
                {
                    var hex1 = Math.DivRem(num, 0x10, out var hex0);
                    sb.Append("0x").Append(GetHexChar(hex1)).Append(GetHexChar(hex0));
                }

                static char GetHexChar(int i) => i < 10 ? (char)(i + 0x30) : (char)(i - 10 + 0x41);
            }
        }

        internal static async Task WithCancellation(this Task task, CancellationToken cancellationToken)
        {
            var tcs = ValueTaskCompletionSource<object?>.Create();

            // This disposes the registration as soon as one of the tasks trigger
            using (cancellationToken.Register(static state => ((TaskCompletionSource<object?>)state!).TrySetResult(null), tcs))
            {
                var resultTask = await Task.WhenAny(task, tcs.Task).ConfigureAwait(false);
                if (resultTask == tcs.Task)
                {
                    // Operation cancelled
                    throw new OperationCanceledException(cancellationToken);
                }

                await task;
            }
        }

        internal static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            var tcs = ValueTaskCompletionSource<object?>.Create();

            // This disposes the registration as soon as one of the tasks trigger
            using (cancellationToken.Register(static state => ((TaskCompletionSource<object?>)state!).TrySetResult(null), tcs))
            {
                var resultTask = await Task.WhenAny(task, tcs.Task).ConfigureAwait(false);
                if (resultTask == tcs.Task)
                {
                    // Operation cancelled
                    throw new OperationCanceledException(cancellationToken);
                }

                return await task;
            }
        }
    }
}
