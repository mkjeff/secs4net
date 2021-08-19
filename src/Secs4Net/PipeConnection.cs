using PooledAwait;
using System.IO.Pipelines;
using System.Net;

namespace Secs4Net;

public sealed class PipeConnection : ISecsConnection
{
    private readonly SemaphoreSlim _sendLock = new(initialCount: 1);
    private readonly PipeDecoder _decoder;
    private readonly int _chunkSize;

    public PipeConnection(PipeReader decoderReader, PipeWriter decoderInput, int chunkSize = 0)
    {
        _decoder = new PipeDecoder(decoderReader, decoderInput);
        _chunkSize = chunkSize;
    }

    public Task StartAsync(CancellationToken cancellation)
        => AsyncHelper.LongRunningAsync(() => _decoder.StartAsync(cancellation), cancellation);

    ValueTask ISecsConnection.SendAsync(ReadOnlyMemory<byte> source, CancellationToken cancellation)
        => SendAsync(source, cancellation);

    private async PooledValueTask SendAsync(ReadOnlyMemory<byte> source, CancellationToken cancellation)
    {
        await _sendLock.WaitAsync(cancellation).ConfigureAwait(false);
        try
        {
            if (_chunkSize <= 0)
            {
                _ = await _decoder.Input.WriteAsync(source, cancellation).ConfigureAwait(false);
            }
            else
            {
                foreach (var chunk in source.Chunk(_chunkSize))
                {
                    _ = await _decoder.Input.WriteAsync(chunk, cancellation).ConfigureAwait(false);
                }
            }
        }
        finally
        {
            _sendLock.Release();
        }
    }

    IAsyncEnumerable<(MessageHeader header, Item? rootItem)> ISecsConnection.GetDataMessages(CancellationToken cancellation)
        => _decoder.GetDataMessages(cancellation);

    bool ISecsConnection.LinkTestEnabled { get; set; }
    public ConnectionState State { get; } = ConnectionState.Selected;
    bool ISecsConnection.IsActive { get; }
    IPAddress ISecsConnection.IpAddress { get; } = IPAddress.Any;
    int ISecsConnection.Port { get; }
    string ISecsConnection.DeviceIpAddress { get; } = string.Empty;
    void ISecsConnection.Reconnect() { }
    event EventHandler<ConnectionState>? ISecsConnection.ConnectionChanged { add { } remove { } }
}
