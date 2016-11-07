using System;
using System.Collections.Generic;
using System.Linq;

namespace Secs4Net
{
    public interface IFormat<T>
    {
    }

    public sealed class ListFormat : IFormat<SecsItem>
    {
        public const SecsFormat Format = SecsFormat.List;
        public static readonly SecsItem Empty = new ListItem();

        public static SecsItem Create(IEnumerable<SecsItem> value)
            => !value.Any() ? Empty : Create(value.ToArray());

        public static SecsItem Create(SecsItem[] value)
            => ItemPool<ListItem, ListFormat, SecsItem>.Acquire(new ArraySegment<SecsItem>(value));

        internal static SecsItem Create(ArraySegment<SecsItem> value)
            => ItemPool<PooledListItem, ListFormat, SecsItem>.Acquire(value);

        public static SecsItem Create(SecsItem v0)
        {
            var result = PooledListItem.ItemListPool.Rent(1);
            result[0] = v0;
            return Create(new ArraySegment<SecsItem>(result, 0, 1));
        }

        public static SecsItem Create(SecsItem v0, SecsItem v1)
        {
            var result = PooledListItem.ItemListPool.Rent(2);
            result[0] = v0;
            result[1] = v1;
            return Create(new ArraySegment<SecsItem>(result, 0, 2));
        }

        public static SecsItem Create(SecsItem v0, SecsItem v1, SecsItem v2)
        {
            var result = PooledListItem.ItemListPool.Rent(3);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            return Create(new ArraySegment<SecsItem>(result, 0, 3));
        }

        public static SecsItem Create(SecsItem v0, SecsItem v1, SecsItem v2, SecsItem v3)
        {
            var result = PooledListItem.ItemListPool.Rent(4);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            result[3] = v3;
            return Create(new ArraySegment<SecsItem>(result, 0, 4));
        }

        public static SecsItem Create(SecsItem v0, SecsItem v1, SecsItem v2, SecsItem v3, SecsItem v4)
        {
            var result = PooledListItem.ItemListPool.Rent(5);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            result[3] = v3;
            result[4] = v4;
            return Create(new ArraySegment<SecsItem>(result, 0, 5));
        }

        public static SecsItem Create(SecsItem v0, SecsItem v1, SecsItem v2, SecsItem v3, SecsItem v4, SecsItem v5)
        {
            var result = PooledListItem.ItemListPool.Rent(6);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            result[3] = v3;
            result[4] = v4;
            result[5] = v5;
            return Create(new ArraySegment<SecsItem>(result, 0, 6));
        }

        public static SecsItem Create(SecsItem v0, SecsItem v1, SecsItem v2, SecsItem v3, SecsItem v4, SecsItem v5, SecsItem v6)
        {
            var result = PooledListItem.ItemListPool.Rent(7);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            result[3] = v3;
            result[4] = v4;
            result[5] = v5;
            result[6] = v6;
            return Create(new ArraySegment<SecsItem>(result, 0, 7));
        }

        public static SecsItem Create(SecsItem v0, SecsItem v1, SecsItem v2, SecsItem v3, SecsItem v4, SecsItem v5, SecsItem v6, SecsItem v7)
        {
            var result = PooledListItem.ItemListPool.Rent(8);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            result[3] = v3;
            result[4] = v4;
            result[5] = v5;
            result[6] = v6;
            result[7] = v7;
            return Create(new ArraySegment<SecsItem>(result, 0, 8));
        }

        public static SecsItem Create(SecsItem v0, SecsItem v1, SecsItem v2, SecsItem v3, SecsItem v4, SecsItem v5, SecsItem v6, SecsItem v7, SecsItem v8)
        {
            var result = PooledListItem.ItemListPool.Rent(9);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            result[3] = v3;
            result[4] = v4;
            result[5] = v5;
            result[6] = v6;
            result[7] = v7;
            result[8] = v8;
            return Create(new ArraySegment<SecsItem>(result, 0, 9));
        }

        public static SecsItem Create(SecsItem v0, SecsItem v1, SecsItem v2, SecsItem v3, SecsItem v4, SecsItem v5, SecsItem v6, SecsItem v7, SecsItem v8, SecsItem v9)
        {
            var result = PooledListItem.ItemListPool.Rent(10);
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
            return Create(new ArraySegment<SecsItem>(result, 0, 10));
        }
    }

    public abstract class StringFormat<TFormat> : IFormat<string> where TFormat : IFormat<string>
    {
        public static readonly SecsItem Empty = new StringItem<TFormat>();
        public static SecsItem Create(string str)
            => string.IsNullOrEmpty(str) ? Empty : ItemPool<StringItem<TFormat>, TFormat, string>.Acquire(str);

        internal StringFormat()
        {
        }
    }

    public sealed class ASCIIFormat : StringFormat<ASCIIFormat>
    {
        public const SecsFormat Format = SecsFormat.ASCII;
    }

    public sealed class JIS8Format : StringFormat<JIS8Format>
    {
        public const SecsFormat Format = SecsFormat.JIS8;
    }

    public abstract class ValueTypeFormat<TFormat, TValue> : IFormat<TValue>
        where TFormat : IFormat<TValue>
        where TValue : struct
    {
        public static readonly SecsItem Empty = new PooledValueItem<TFormat, TValue>();

        internal ValueTypeFormat()
        {
        }

        public static SecsItem Create(IEnumerable<TValue> value)
            => !value.Any() ? Empty : Create(value.ToArray());

        public static SecsItem Create(TValue[] value)
            => ItemPool<ValueItem<TFormat, TValue>, TFormat, TValue>.Acquire(new ArraySegment<TValue>(value));

        internal static SecsItem Create(ArraySegment<TValue> value)
            => ItemPool<PooledValueItem<TFormat, TValue>, TFormat, TValue>.Acquire(value);

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
