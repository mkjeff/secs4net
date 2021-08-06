using Microsoft.Toolkit.HighPerformance;
using Secs4Net.Extensions;
using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace Secs4Net.Benchmarks
{
    /// <summary>
    /// Even use function pointer, still slower than BinaryPrimitives directly method call.
    /// Check out the result of benchmark <see cref="ReverseEndianness"/>
    /// </summary>
    public static unsafe class ReverseEndiannessHelper<T> where T : unmanaged
    {
        public static readonly delegate*<T, T> Reverse;

        static ReverseEndiannessHelper()
        {
            var t = typeof(T);
            if (sizeof(T) == 1)
            {
                Reverse = &Dummy;
            }
            else if (t == typeof(ushort))
            {
                Reverse = (delegate*<T, T>)typeof(BinaryPrimitives).GetMethod(nameof(BinaryPrimitives.ReverseEndianness), new[] { typeof(ushort) })!.MethodHandle.GetFunctionPointer();
            }
            else if (t == typeof(uint))
            {
                Reverse = (delegate*<T, T>)typeof(BinaryPrimitives).GetMethod(nameof(BinaryPrimitives.ReverseEndianness), new[] { typeof(uint) })!.MethodHandle.GetFunctionPointer();
            }
            else if (t == typeof(ulong))
            {
                Reverse = (delegate*<T, T>)typeof(BinaryPrimitives).GetMethod(nameof(BinaryPrimitives.ReverseEndianness), new[] { typeof(ulong) })!.MethodHandle.GetFunctionPointer();
            }
            else if (t == typeof(short))
            {
                Reverse = (delegate*<T, T>)typeof(BinaryPrimitives).GetMethod(nameof(BinaryPrimitives.ReverseEndianness), new[] { typeof(short) })!.MethodHandle.GetFunctionPointer();
            }
            else if (t == typeof(int))
            {
                Reverse = (delegate*<T, T>)typeof(BinaryPrimitives).GetMethod(nameof(BinaryPrimitives.ReverseEndianness), new[] { typeof(int) })!.MethodHandle.GetFunctionPointer();
            }
            else if (t == typeof(long))
            {
                Reverse = (delegate*<T, T>)typeof(BinaryPrimitives).GetMethod(nameof(BinaryPrimitives.ReverseEndianness), new[] { typeof(long) })!.MethodHandle.GetFunctionPointer();
            }
            else if (t == typeof(float))
            {
                Reverse = (delegate*<T, T>)typeof(ReverseHelper).GetMethod(nameof(ReverseHelper.ReverseEndiannessSingle))!.MethodHandle.GetFunctionPointer();
            }
            else if (t == typeof(double))
            {
                Reverse = (delegate*<T, T>)typeof(ReverseHelper).GetMethod(nameof(ReverseHelper.ReverseEndiannessDouble))!.MethodHandle.GetFunctionPointer();
            }
            else
            {
                throw new InvalidOperationException($"Invalid type argurment {typeof(T).Name}");
            }
        }

        private static T Dummy(T value) => value;
    }

    static class ReverseHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe float ReverseEndiannessSingle(float value)
        {
#if NET
            return BinaryPrimitives.ReadSingleBigEndian(value.AsBytes());
#else
            return SecsExtension.ReadSingleBigEndian(value.AsBytes());
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ReverseEndiannessDouble(double value)
        {
#if NET
            return BinaryPrimitives.ReadDoubleBigEndian(value.AsBytes());
#else
            return SecsExtension.ReadDoubleBigEndian(value.AsBytes());
#endif
        }
    }
}
