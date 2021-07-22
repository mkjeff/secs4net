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
        /// Note: This operation is faster because of bypassing index boundary validation.
        /// You need to make sure the parameter <paramref name="index"/> is less than <see cref="Length"/> before accessing.
        /// A safer, but slower option is using <see cref="ReadOnlySpan{T}"/> returns from <see cref="AsSpan"/> method.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ref T this[int index] => ref Unsafe.Add(ref Unsafe.As<byte, T>(ref _array.Span.DangerousGetReference()), index);

        public readonly T[] ToArray() => AsSpan().ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ReadOnlySpan<T> AsSpan() => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<byte, T>(ref _array.Span.DangerousGetReference()), _length);
    }
}
