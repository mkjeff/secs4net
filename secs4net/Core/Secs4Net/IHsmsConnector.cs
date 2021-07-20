using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Secs4Net
{
    public interface IHsmsConnector : IEncodedBuffer
    {
        event EventHandler<ConnectionState>? ConnectionChanged;
        int T5 => 10000;
        int T7 => 10000;
        int T8 => 5000;
        int LinkTestInterval => 60000;
        ConnectionState State { get; }
        bool IsActive => false;
        IPAddress IpAddress => IPAddress.Loopback;
        int Port => -1;
        string DeviceIpAddress => string.Empty;

        PipeDecoder PipeDecoder { get; }

        void Reconnect() { }
        Task Start(CancellationToken cancellation) => Task.CompletedTask;

        internal Task HandleControlMessagesAsync(IAsyncEnumerable<MessageHeader> controlMessages, CancellationToken cancellation) => Task.CompletedTask;
    }
}
