using System;
using System.Threading;
using System.Threading.Tasks;

namespace Secs4Net
{
    public sealed class InMemoryConnector : IHsmsConnector
    {
        public event EventHandler<ConnectionState>? ConnectionChanged;
        public ConnectionState State => ConnectionState.Selected;
        public PipeDecoder PipeDecoder { get; }
        public InMemoryConnector(PipeDecoder pipeDecoder)
        {
            PipeDecoder = pipeDecoder;
        }

        public async ValueTask<int> WriteAsync(ReadOnlyMemory<byte> source, CancellationToken cancellationToken)
        {
            // assume the 'PipeDecoder.Input' here is another connector's input
            var result = await PipeDecoder.Input.WriteAsync(source, cancellationToken);
            return source.Length;
        }
    }
}
