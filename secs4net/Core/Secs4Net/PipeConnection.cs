using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Secs4Net
{
    public sealed class PipeConnection : ISecsConnection
    {
        bool ISecsConnection.LinkTestEnabled { get; set; }

        private readonly PipeDecoder _decoder;
        public PipeConnection(PipeDecoder pipeDecoder)
        {
            _decoder = pipeDecoder;
            AsyncHelper.LongRunningAsync(() => _decoder.StartAsync(CancellationToken.None));
        }

        async ValueTask ISecsConnection.SendAsync(ReadOnlyMemory<byte> source, CancellationToken cancellationToken)
        {
            // assume the 'PipeDecoder.Input' here is another connector's input
            _ = await _decoder.Input.WriteAsync(source, cancellationToken);
        }

        IAsyncEnumerable<SecsMessage> ISecsConnection.GetDataMessages(CancellationToken cancellation)
            => _decoder.GetDataMessages(cancellation);
    }
}
