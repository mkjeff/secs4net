using System;
using System.Net;
using System.Threading.Tasks;

namespace Secs4Net
{
    public interface ISecsGem
    {
        bool LinkTestEnable { get; set; }
        int LinkTestInterval { get; set; }
        int T3 { get; set; }
        int T5 { get; set; }
        int T6 { get; set; }
        int T7 { get; set; }
        int T8 { get; set; }
        ISecsGemLogger Logger { get; set; }

        int DecoderBufferSize { get; }
        ushort DeviceId { get; set; }
        string DeviceIpAddress { get; }
        IPAddress IpAddress { get; }
        int Port { get; }

        bool IsActive { get; }
        bool IsDisposed { get; }
        ConnectionState State { get; }


        event EventHandler<ConnectionState> ConnectionChanged;
        event Action<PrimaryMessageWrapper> PrimaryMessageReceived;

        void Dispose();
        Task<SecsMessage> SendAsync(SecsMessage message, bool autoDispose = true);
        void Start();
    }
}