using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Secs4Net
{
    public interface IHsmsConnection
    {
        public event EventHandler<ConnectionState>? ConnectionChanged;
        int T5 => 10000;
        int T7 => 10000;
        int T8 => 5000;
        int LinkTestInterval => 60000;
        ConnectionState State { get; }
        bool IsActive => false;
        IPAddress IpAddress => IPAddress.Loopback;
        int Port => 0;
        string DeviceIpAddress => string.Empty;
        void Reconnect() { }
        internal ValueTask<int> SendAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken);
        internal PipeDecoder PipeDecoder { get; }
    }
}
