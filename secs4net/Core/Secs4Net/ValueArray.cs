using Microsoft.Toolkit.HighPerformance;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Secs4Net
{
    public readonly struct ValueArray<T>
    {
        private readonly Memory<byte> _array;
        public readonly int _length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ValueArray(Memory<byte> src)
        {
            _array = src;
            _length = src.Length / Unsafe.SizeOf<T>();
        }

        public readonly bool IsEmpty => _length == 0;
        public readonly int Length => _length;

        /// <summary>
        /// Note: this operation will not valid the bound range of array. You need to make sure the parameter <paramref name="index"/> is valid before accessing
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ref T this[int index] => ref Unsafe.Add(ref Unsafe.As<byte, T>(ref _array.Span.DangerousGetReferenceAt(0)), index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly T[] ToArray() => AsSpan().ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ReadOnlySpan<T> AsSpan() => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<byte, T>(ref _array.Span.DangerousGetReferenceAt(0)), _length);
    }
}
