using System.Net;

namespace Secs4Net;

public interface ISecsConnection
{
    public event EventHandler<ConnectionState>? ConnectionChanged;

    /// <summary>
    /// Connection state
    /// </summary>
    ConnectionState State { get; }

    /// <summary>
    /// Is Active or Passive mode
    /// </summary>
    bool IsActive { get; }

    /// <summary>
    /// When <see cref="IsActive"/> is <see langword="true"/> the IP address will be treated remote device's IP address, 
    /// opposite the connection will bind on this IP address as Passive mode.
    /// </summary>
    IPAddress IpAddress { get; }

    /// <summary>
    /// When <see cref="IsActive"/> is <see langword="true"/> the port number will be treated remote device's TCP port number, 
    /// opposite the connection will bind on the port number as Passive mode.
    /// </summary>
    int Port { get; }

    /// <summary>
    /// Remote device's IP address.<br/>
    /// In Active mode, it is the same as IPAddress property<br/>
    /// In Passive mode, remote IP Address can be resolved successfully only 
    /// when <see cref="State"/> is <see cref="ConnectionState.Connected"/> or <see cref="ConnectionState.Selected"/>, 
    /// otherwise, return "N/A"
    /// </summary>
    string DeviceIpAddress { get; }
    bool LinkTestEnabled { get; set; }
    void Start(CancellationToken cancellation);
    void Reconnect();

    internal Task SendAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken);
    internal IAsyncEnumerable<(MessageHeader header, Item? rootItem)> GetDataMessages(CancellationToken cancellation);
}
