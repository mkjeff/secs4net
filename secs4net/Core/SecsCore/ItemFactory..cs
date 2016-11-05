using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace Secs4Net
{
    public interface IFormat<T>
    {
    }

    public sealed class ListFormat : IFormat<Item>
    {
        public const SecsFormat Format = SecsFormat.List;
        public static readonly Item Empty = new ListItem();

        public static Item Create(IEnumerable<Item> value) 
            => !value.Any() ? Empty : Create(value.ToArray());

        public static Item Create(Item[] value)
            => ItemPool<ListItem, ListFormat, Item>.Acquire(new ArraySegment<Item>(value));

        public static Item Create(ArraySegment<Item> value)
            => ItemPool<PooledListItem, ListFormat, Item>.Acquire(value);

        public static Item Create(Item v0)
        {
            var result = ArrayPool<Item>.Shared.Rent(1);
            result[0] = v0;
            return Create(new ArraySegment<Item>(result, 0, 1));
        }

        public static Item Create(Item v0, Item v1)
        {
            var result = ArrayPool<Item>.Shared.Rent(2);
            result[0] = v0;
            result[1] = v1;
            return Create(new ArraySegment<Item>(result, 0, 2));
        }

        public static Item Create(Item v0, Item v1, Item v2)
        {
            var result = ArrayPool<Item>.Shared.Rent(3);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            return Create(new ArraySegment<Item>(result, 0, 3));
        }

        public static Item Create(Item v0, Item v1, Item v2, Item v3)
        {
            var result = ArrayPool<Item>.Shared.Rent(4);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            result[3] = v3;
            return Create(new ArraySegment<Item>(result, 0, 4));
        }

        public static Item Create(Item v0, Item v1, Item v2, Item v3, Item v4)
        {
            var result = ArrayPool<Item>.Shared.Rent(5);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            result[3] = v3;
            result[4] = v4;
            return Create(new ArraySegment<Item>(result, 0, 5));
        }

        public static Item Create(Item v0, Item v1, Item v2, Item v3, Item v4, Item v5)
        {
            var result = ArrayPool<Item>.Shared.Rent(6);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            result[3] = v3;
            result[4] = v4;
            result[5] = v5;
            return Create(new ArraySegment<Item>(result, 0, 6));
        }

        public static Item Create(Item v0, Item v1, Item v2, Item v3, Item v4, Item v5, Item v6)
        {
            var result = ArrayPool<Item>.Shared.Rent(7);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            result[3] = v3;
            result[4] = v4;
            result[5] = v5;
            result[6] = v6;
            return Create(new ArraySegment<Item>(result, 0, 7));
        }
    }

    public abstract class StringFormat<TFormat> : IFormat<string> where TFormat : IFormat<string>
    {
        public static readonly Item Empty = new StringItem<TFormat>();
        public static Item Create(string str)
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
        public static readonly Item Empty = new PooledValueItem<TFormat, TValue>();

        internal ValueTypeFormat()
        {
        }

        public static Item Create(IEnumerable<TValue> value) 
            => !value.Any() ? Empty : Create(value.ToArray());

        public static Item Create(TValue[] value)
            => ItemPool<ValueTypeItem<TFormat, TValue>, TFormat, TValue>.Acquire(new ArraySegment<TValue>(value));

        public static Item Create(ArraySegment<TValue> value)
            => ItemPool<PooledValueItem<TFormat, TValue>, TFormat, TValue>.Acquire(value);

        public static Item Create(TValue v0)
        {
            var result = ArrayPool<TValue>.Shared.Rent(1);
            result[0] = v0;
            return Create(new ArraySegment<TValue>(result, 0, 1));
        }

        public static Item Create(TValue v0, TValue v1)
        {
            var result = ArrayPool<TValue>.Shared.Rent(2);
            result[0] = v0;
            result[1] = v1;
            return Create(new ArraySegment<TValue>(result, 0, 2));
        }

        public static Item Create(TValue v0, TValue v1, TValue v2)
        {
            var result = ArrayPool<TValue>.Shared.Rent(3);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            return Create(new ArraySegment<TValue>(result, 0, 3));
        }

        public static Item Create(TValue v0, TValue v1, TValue v2, TValue v3)
        {
            var result = ArrayPool<TValue>.Shared.Rent(4);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            result[3] = v3;
            return Create(new ArraySegment<TValue>(result, 0, 4));
        }

        public static Item Create(TValue v0, TValue v1, TValue v2, TValue v3, TValue v4)
        {
            var result = ArrayPool<TValue>.Shared.Rent(5);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            result[3] = v3;
            result[4] = v4;
            return Create(new ArraySegment<TValue>(result, 0, 5));
        }

        public static Item Create(TValue v0, TValue v1, TValue v2, TValue v3, TValue v4, TValue v5)
        {
            var result = ArrayPool<TValue>.Shared.Rent(6);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            result[3] = v3;
            result[4] = v4;
            result[5] = v5;
            return Create(new ArraySegment<TValue>(result, 0, 6));
        }

        public static Item Create(TValue v0, TValue v1, TValue v2, TValue v3, TValue v4, TValue v5, TValue v6)
        {
            var result = ArrayPool<TValue>.Shared.Rent(7);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            result[3] = v3;
            result[4] = v4;
            result[5] = v5;
            result[6] = v6;
            return Create(new ArraySegment<TValue>(result, 0, 7));
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
