using System;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Secs4Net
{
    internal class ValueItem<TFormat, TValue> : SecsItem<TFormat, TValue>
     where TFormat : IFormat<TValue>
     where TValue : struct
    {
        private readonly Pool<ValueItem<TFormat, TValue>> _pool;

        internal ValueItem(Pool<ValueItem<TFormat, TValue>> pool = null)
        {
            _pool = pool;
        }
        internal override void Release()
        {
            _pool?.Release(this);
        }

        protected sealed override ArraySegment<byte> GetEncodedData()
        {
            if (values.Count == 0)
                return EncodEmpty(Format);

            var sizeOf = Unsafe.SizeOf<TValue>();
            var bytelength = values.Count * sizeOf;
            int headerLength;
            var result = EncodeValue(Format, bytelength, out headerLength);
            Buffer.BlockCopy(values.Array, 0, result, headerLength, bytelength);
            result.Reverse(headerLength, headerLength + bytelength, sizeOf);
            return new ArraySegment<byte>(result, 0, headerLength + bytelength);
        }

        protected ArraySegment<TValue> values = new ArraySegment<TValue>(Array.Empty<TValue>());

        internal void SetValue(ArraySegment<TValue> itemValue)
        {
            values = itemValue;
        }

        public sealed override int Count => values.Count;

        public sealed override IEnumerable Values => values;

        public sealed override unsafe T GetValue<T>() => Unsafe.Read<T>(Unsafe.AsPointer(ref values.Array[0]));

        public sealed override T[] GetValues<T>() => Unsafe.As<T[]>(values.ToArray());

        public sealed override bool IsMatch(SecsItem target)
        {
            if (ReferenceEquals(this, target))
                return true;

            if (Format != target.Format)
                return false;

            if (target.Count == 0)
                return true;

            if (Count != target.Count)
                return false;

            //return memcmp(Unsafe.As<byte[]>(_values), Unsafe.As<byte[]>(target._values), Buffer.ByteLength((Array)_values)) == 0;
            return UnsafeCompare(
                values.Array,
                Unsafe.As<ValueItem<TFormat, TValue>>(target).values.Array,
                values.Count);
        }

        public sealed override string ToString()
            => $"<{Format.GetName()} [{Count}] {(Format == SecsFormat.Binary ? Unsafe.As<byte[]>(values).ToHexString() : string.Join(" ", values))} >";

        //[DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        //static extern int memcmp(byte[] b1, byte[] b2, long count);

        /// <summary>
        /// http://stackoverflow.com/questions/43289/comparing-two-byte-arrays-in-net/8808245#8808245
        /// </summary>
        private static unsafe bool UnsafeCompare(TValue[] a1, TValue[] a2, int count)
        {
            int length = count * Unsafe.SizeOf<TValue>();
            fixed (byte* p1 = Unsafe.As<byte[]>(a1), p2 = Unsafe.As<byte[]>(a2))
            {
                byte* x1 = p1, x2 = p2;
                int l = length;
                for (int i = 0; i < l / 8; i++, x1 += 8, x2 += 8)
                    if (*((long*)x1) != *((long*)x2)) return false;
                if ((l & 4) != 0) { if (*((int*)x1) != *((int*)x2)) return false; x1 += 4; x2 += 4; }
                if ((l & 2) != 0) { if (*((short*)x1) != *((short*)x2)) return false; x1 += 2; x2 += 2; }
                if ((l & 1) != 0) if (*((byte*)x1) != *((byte*)x2)) return false;
                return true;
            }
        }
    }



    internal sealed class PooledValueItem<TFormat, TValue> : ValueItem<TFormat, TValue>
        where TFormat : IFormat<TValue>
        where TValue : struct
    {
        internal PooledValueItem(Pool<ValueItem<TFormat, TValue>> pool)
            : base(pool)
        {
        }

        internal override void Release()
        {
            ValueTypeArrayPool<TValue>.Pool.Return(values.Array);
            base.Release();
        }
    }
}
