using System;
using System.Threading;
using System.Threading.Tasks;

namespace Secs4Net
{
    public sealed class PipeConnection : IHsmsConnection
    {
        public event EventHandler<ConnectionState>? ConnectionChanged;
        public ConnectionState State => ConnectionState.Selected;
        public PipeDecoder PipeDecoder { get; }
        public PipeConnection(PipeDecoder pipeDecoder)
        {
            PipeDecoder = pipeDecoder;
            AsyncHelper.LongRunningAsync(() => PipeDecoder.StartAsync(CancellationToken.None));
        }

        public async ValueTask<int> SendAsync(ReadOnlyMemory<byte> source, CancellationToken cancellationToken)
        {
            // assume the 'PipeDecoder.Input' here is another connector's input
            var result = await PipeDecoder.Input.WriteAsync(source, cancellationToken);
            return source.Length;
        }
    }
}
