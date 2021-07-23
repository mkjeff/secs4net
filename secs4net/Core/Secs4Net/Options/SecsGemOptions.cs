using System.Net;

namespace Secs4Net
{
    public record SecsGemOptions
    {
        public ushort DeviceId { get; init; }
        public bool IsActive { get; init; }
        public string IpAddress { get; init; }
        public int Port { get; init; }
        public int T3 { get; init; } = 45000;
        public int T5 { get; init; } = 10000;
        public int T6 { get; init; } = 5000;
        public int T7 { get; init; } = 10000;
        public int T8 { get; init; } = 5000;
        public int LinkTestInterval { get; init; } = 60000;
        public int SocketReceiveBufferSize { get; init; } = 65535;
        public int SocketSendBufferInitialSize { get; init; } = 65535;
    }
}
