using System;
using System.Collections.Generic;
using System.Linq;

namespace Secs4Net
{
    public sealed class ListFormat : IFormat<SecsItem>
    {
        public const SecsFormat Format = SecsFormat.List;

        private static readonly Pool<SecsItem<ListFormat, SecsItem>> ListItemPool
            = new Pool<SecsItem<ListFormat, SecsItem>>(p => new ListItem(p));

        private static readonly Pool<SecsItem<ListFormat, SecsItem>> PooledListItemPool =
            new Pool<SecsItem<ListFormat, SecsItem>>(p => new PooledListItem(p));

        public static readonly SecsItem Empty = new ListItem();

        /// <summary>
        /// Create ListItem
        /// </summary>
        /// <param name="value">dynamic allocated item collection</param>
        /// <returns></returns>
        public static SecsItem Create(IEnumerable<SecsItem> value)
        {
            var secsItems = value as SecsItem[] ?? value.ToArray();
            return secsItems.Length == 0 ? Empty : Create(secsItems);
        }

        /// <summary>
        /// Create ListItem
        /// </summary>
        /// <param name="value">dynamic allocated item array</param>
        /// <returns></returns>
        public static SecsItem Create(SecsItem[] value)
        {
            var item = ListItemPool.Acquire();
            item.SetValue(new ArraySegment<SecsItem>(value));
            return item;
        }

        /// <summary>
        /// Create PooledListItem
        /// </summary>
        /// <param name="value">item list from pool</param>
        /// <returns></returns>
        internal static SecsItem Create(ArraySegment<SecsItem> value)
        {
            var item = PooledListItemPool.Acquire();
            item.SetValue(value);
            return item;
        }

        public static SecsItem Create(SecsItem v0)
        {
            var result = SecsItemArrayPool.Pool.Rent(1);
            result[0] = v0;
            return Create(new ArraySegment<SecsItem>(result, 0, 1));
        }

        public static SecsItem Create(SecsItem v0, SecsItem v1)
        {
            var result = SecsItemArrayPool.Pool.Rent(2);
            result[0] = v0;
            result[1] = v1;
            return Create(new ArraySegment<SecsItem>(result, 0, 2));
        }

        public static SecsItem Create(SecsItem v0, SecsItem v1, SecsItem v2)
        {
            var result = SecsItemArrayPool.Pool.Rent(3);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            return Create(new ArraySegment<SecsItem>(result, 0, 3));
        }

        public static SecsItem Create(SecsItem v0, SecsItem v1, SecsItem v2, SecsItem v3)
        {
            var result = SecsItemArrayPool.Pool.Rent(4);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            result[3] = v3;
            return Create(new ArraySegment<SecsItem>(result, 0, 4));
        }

        public static SecsItem Create(SecsItem v0, SecsItem v1, SecsItem v2, SecsItem v3, SecsItem v4)
        {
            var result = SecsItemArrayPool.Pool.Rent(5);
            result[0] = v0;
            result[1] = v1;
            result[2] = v2;
            result[3] = v3;
            result[4] = v4;
            return Create(new ArraySegment<SecsItem>(result, 0, 5));
        }

        public static SecsItem Create(SecsItem v0, SecsItem v1, SecsItem v2, SecsItem v3, SecsItem v4, SecsItem v5)
        {
            var result = SecsItemArrayPool.Pool.Rent(6);
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
            var result = SecsItemArrayPool.Pool.Rent(7);
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
            var result = SecsItemArrayPool.Pool.Rent(8);
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
            var result = SecsItemArrayPool.Pool.Rent(9);
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
            var result = SecsItemArrayPool.Pool.Rent(10);
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
}
