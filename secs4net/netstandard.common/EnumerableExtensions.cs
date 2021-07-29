#if !NET
using System.Collections.Generic;

namespace System.Linq
{
    public static class EnumerableExt
    {
        public static IEnumerable<T[]> Chunk<T>(this IEnumerable<T> items, int maxItems)
        {
            return items.Select((item, inx) => new { item, inx })
                        .GroupBy(x => x.inx / maxItems)
                        .Select(g => g.Select(x => x.item).ToArray());
        }
    }
}
#endif
