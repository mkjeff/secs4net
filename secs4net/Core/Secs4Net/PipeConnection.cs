﻿using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Secs4Net
{
    public sealed class PipeConnection : ISecsConnection
    {
        private readonly SemaphoreSlim _sendLock = new(initialCount: 1);
        private readonly PipeDecoder _decoder;

        public PipeConnection(PipeReader decoderReader, PipeWriter decoderInput) 
            => _decoder = new PipeDecoder(decoderReader, decoderInput);

        public Task StartAsync(CancellationToken cancellation) 
            => AsyncHelper.LongRunningAsync(() => _decoder.StartAsync(cancellation), cancellation);

        async ValueTask ISecsConnection.SendAsync(ReadOnlyMemory<byte> source, CancellationToken cancellationToken)
        {
            await _sendLock.WaitAsync(cancellationToken);
            try
            {
                _ = await _decoder.Input.WriteAsync(source, cancellationToken).ConfigureAwait(false);
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
}
