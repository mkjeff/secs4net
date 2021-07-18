using System.Net;

namespace Secs4Net.Options
{
    public class SecsGemOptions
    {
        public ushort DeviceId { get; set; }
        public bool IsActive { get; set; }
        public IPAddress? IpAddress { get; set; }
        public int Port { get; set; }
        public int T3 { get; set; } = 45000;
        public int T5 { get; set; } = 10000;
        public int T6 { get; set; } = 5000;
        public int T7 { get; set; } = 10000;
        public int T8 { get; set; } = 5000;
        public int LinkTestInterval { get; set; } = 60000;
        public int DecoderBufferSize { get; set; } = 0x4000;
    }
}
