using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Secs4Net
{
    public readonly struct ValueArray<T>
    {
        private readonly Array _array;
        private readonly int _length;

        internal ValueArray(Array src)
        {
            _array = src;
            _length = Buffer.ByteLength(_array) / Unsafe.SizeOf<T>();
        }

        public readonly bool IsEmpty => _length == 0;

        public readonly int Length => _length;

        /// <summary>
        /// Note: this operation will not valid the bound range of array. You need to make sure the parameter <paramref name="index"/> is valid before accessing
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ref readonly T this[int index] => ref Unsafe.Add(ref Unsafe.As<byte, T>(ref MemoryMarshal.GetArrayDataReference(_array)), index);

        public readonly T[] ToArray() => AsSpan().ToArray();

        public readonly ReadOnlySpan<T> AsSpan() => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<byte, T>(ref MemoryMarshal.GetArrayDataReference(_array)), _length);
    }
}
