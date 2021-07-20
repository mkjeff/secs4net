using System;
using System.Threading;
using System.Threading.Tasks;

namespace Secs4Net
{
    public interface IEncodedBuffer
    {
        public ValueTask<int> WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken);
    }
}
