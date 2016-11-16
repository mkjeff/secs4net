using System;
using System.Collections.Generic;
using System.Linq;

namespace Secs4Net
{
    public abstract class ValueTypeFormat<TFormat, TValue> : IFormat<TValue>
       where TFormat : IFormat<TValue>
       where TValue : struct
    {
        private static readonly Pool<ValueItem<TFormat, TValue>> ValueItemPool
            = new Pool<ValueItem<TFormat, TValue>>(p => new ValueItem<TFormat, TValue>(p));

        public static readonly SecsItem Empty = new ValueItem<TFormat, TValue>();

        internal ValueTypeFormat()
        {
        }

        /// <summary>
        /// Create <typeparamref name="TValue"/> item
        /// </summary>
        /// <param name="value">dynamic allocated <typeparamref name="TValue"/> collection</param>
        /// <returns></returns>
        public static SecsItem Create(IEnumerable<TValue> value)
        {
            var arr = value as TValue[] ?? value.ToArray();
            return arr.Length == 0 ? Empty : Create(arr);
        }

        /// <summary>
        /// Create <typeparamref name="TValue"/> item
        /// </summary>
        /// <param name="value">dynamic allocated <typeparamref name="TValue"/> array</param>
        /// <returns></returns>
        public static SecsItem Create(TValue[] value)
        {
            var item = ValueItemPool.Acquire();
            item.SetValues(new ArraySegment<TValue>(value), fromPool: false);
            return item;
        }

        /// <summary>
        /// Create <typeparamref name="TValue"/> item
        /// </summary>
        /// <param name="value"><typeparamref name="TValue"/> item from pool</param>
        /// <returns></returns>
        internal static SecsItem Create(ArraySegment<TValue> value)
        {
            var item = ValueItemPool.Acquire();
            item.SetValues(value, fromPool: true);
            return item;
        }

        public static SecsItem Create(TValue v0)
        {
            var result = ValueTypeArrayPool<TValue>.Pool.Rent(1);
            result[0] = v0;
            return Create(new ArraySegment<TValue>(result, 0, 1));
        }

        public static SecsItem Create(TValue v0, TValue v1)
        {
            var result = ValueTypeArrayPool<TValue>.Pool.Rent(2);
            result[0] = v0;
            result[1] = v1;
            return Create(new ArraySegment<TValue>(result, 0, 2));
        }

        public static SecsItem Create(TValue v0, TValue v1, TValue v2)
        {
            var result = ValueTypeArrayPool<TValue>.Pool.Rent(3);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            return Create(new ArraySegment<TValue>(result, 0, 3));
        }

        public static SecsItem Create(TValue v0, TValue v1, TValue v2, TValue v3)
        {
            var result = ValueTypeArrayPool<TValue>.Pool.Rent(4);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            result[3] = v3;
            return Create(new ArraySegment<TValue>(result, 0, 4));
        }

        public static SecsItem Create(TValue v0, TValue v1, TValue v2, TValue v3, TValue v4)
        {
            var result = ValueTypeArrayPool<TValue>.Pool.Rent(5);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            result[3] = v3;
            result[4] = v4;
            return Create(new ArraySegment<TValue>(result, 0, 5));
        }

        public static SecsItem Create(TValue v0, TValue v1, TValue v2, TValue v3, TValue v4, TValue v5)
        {
            var result = ValueTypeArrayPool<TValue>.Pool.Rent(6);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            result[3] = v3;
            result[4] = v4;
            result[5] = v5;
            return Create(new ArraySegment<TValue>(result, 0, 6));
        }

        public static SecsItem Create(TValue v0, TValue v1, TValue v2, TValue v3, TValue v4, TValue v5, TValue v6)
        {
            var result = ValueTypeArrayPool<TValue>.Pool.Rent(7);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            result[3] = v3;
            result[4] = v4;
            result[5] = v5;
            result[6] = v6;
            return Create(new ArraySegment<TValue>(result, 0, 7));
        }

        public static SecsItem Create(TValue v0, TValue v1, TValue v2, TValue v3, TValue v4, TValue v5, TValue v6, TValue v7)
        {
            var result = ValueTypeArrayPool<TValue>.Pool.Rent(8);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            result[3] = v3;
            result[4] = v4;
            result[5] = v5;
            result[6] = v6;
            result[7] = v7;
            return Create(new ArraySegment<TValue>(result, 0, 8));
        }

        public static SecsItem Create(TValue v0, TValue v1, TValue v2, TValue v3, TValue v4, TValue v5, TValue v6, TValue v7, TValue v8)
        {
            var result = ValueTypeArrayPool<TValue>.Pool.Rent(9);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            result[3] = v3;
            result[4] = v4;
            result[5] = v5;
            result[6] = v6;
            result[7] = v7;
            result[8] = v8;
            return Create(new ArraySegment<TValue>(result, 0, 9));
        }

        public static SecsItem Create(TValue v0, TValue v1, TValue v2, TValue v3, TValue v4, TValue v5, TValue v6, TValue v7, TValue v8, TValue v9)
        {
            var result = ValueTypeArrayPool<TValue>.Pool.Rent(10);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            result[3] = v3;
            result[4] = v4;
            result[5] = v5;
            result[6] = v6;
            result[7] = v7;
            result[8] = v8;
            result[9] = v9;
            return Create(new ArraySegment<TValue>(result, 0, 10));
        }
    }

    public sealed class BooleanFormat : ValueTypeFormat<BooleanFormat, bool>
    {
        public const SecsFormat Format = SecsFormat.Boolean;
    }

    public sealed class BinaryFormat : ValueTypeFormat<BinaryFormat, byte>
    {
        public const SecsFormat Format = SecsFormat.Binary;
    }

    public sealed class F4Format : ValueTypeFormat<F4Format, float>
    {
        public const SecsFormat Format = SecsFormat.F4;
    }

    public sealed class F8Format : ValueTypeFormat<F8Format, double>
    {
        public const SecsFormat Format = SecsFormat.F8;
    }

    public sealed class I1Format : ValueTypeFormat<I1Format, sbyte>
    {
        public const SecsFormat Format = SecsFormat.I1;
    }

    public sealed class I2Format : ValueTypeFormat<I2Format, short>
    {
        public const SecsFormat Format = SecsFormat.I2;
    }

    public sealed class I4Format : ValueTypeFormat<I4Format, int>
    {
        public const SecsFormat Format = SecsFormat.I4;
    }

    public sealed class I8Format : ValueTypeFormat<I8Format, long>
    {
        public const SecsFormat Format = SecsFormat.I8;
    }

    public sealed class U1Format : ValueTypeFormat<U1Format, byte>
    {
        public const SecsFormat Format = SecsFormat.U1;
    }

    public sealed class U2Format : ValueTypeFormat<U2Format, ushort>
    {
        public const SecsFormat Format = SecsFormat.U2;
    }

    public sealed class U4Format : ValueTypeFormat<U4Format, uint>
    {
        public const SecsFormat Format = SecsFormat.U4;
    }

    public sealed class U8Format : ValueTypeFormat<U8Format, ulong>
    {
        public const SecsFormat Format = SecsFormat.U8;
    }
}
