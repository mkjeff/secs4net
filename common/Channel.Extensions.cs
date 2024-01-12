#if !NET
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Threading.Channels
{
    internal static class ChannelExtensions
    {
        public static async IAsyncEnumerable<T> ReadAllAsync<T>(this ChannelReader<T> reader, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            while (await reader.WaitToReadAsync(cancellationToken).ConfigureAwait(false))
            {
                while (reader.TryRead(out var item))
                {
                    yield return item;
                }
            }
        }
    }
}
#endif
