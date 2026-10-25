using FluentAssertions;
using Microsoft.Extensions.Options;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Secs4Net.UnitTests;

public class HsmsConnectionTeardownTests
{
    private sealed class SilentLogger : ISecsGemLogger
    {
        public void Debug(string msg) { }
        public void Info(string msg) { }
        public void Warning(string msg) { }
        public void Error(string msg, SecsMessage? message, Exception? ex) { }
    }

    private static int FreeTcpPort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }

    private static byte[] ControlMessage(byte sType, uint systemBytes)
    {
        var frame = new byte[14];
        BinaryPrimitives.WriteUInt32BigEndian(frame, 10);
        BinaryPrimitives.WriteUInt16BigEndian(frame.AsSpan(4), 0xFFFF);
        frame[9] = sType;
        BinaryPrimitives.WriteUInt32BigEndian(frame.AsSpan(10), systemBytes);
        return frame;
    }

    private static async Task WaitUntilAsync(Func<bool> condition, TimeSpan timeout)
    {
        using var cts = new CancellationTokenSource(timeout);
        while (!condition())
        {
            await Task.Delay(10, cts.Token);
        }
    }

    /// <summary>
    /// A lost link and disposal can tear the connection down at the same time: the receive loop sees the peer
    /// close and calls <see cref="HsmsConnection.Reconnect"/> while the owner disposes the connection. Both reach
    /// <c>Disconnect()</c>; the pipe decoder's cancellation source and the socket must each be released exactly once,
    /// and disposal must run to the end (it also stops control-message processing and the T7/T8 timers).
    /// </summary>
    [Fact]
    public async Task Reconnect_And_DisposeAsync_Racing_Do_Not_Throw()
    {
        const int Rounds = 100;
        var failures = new List<string>();

        for (var round = 1; round <= Rounds; round++)
        {
            var port = FreeTcpPort();
            var options = Options.Create(new SecsGemOptions
            {
                IsActive = false,
                IpAddress = "127.0.0.1",
                Port = port,
                T5 = 1000,
                T6 = 5000,
                T7 = 10000,
                T8 = 5000,
                LinkTestInterval = 60000,
            });

            var connection = new HsmsConnection(options, new SilentLogger());
            var selected = 0;
            connection.ConnectionChanged += (_, state) =>
            {
                if (state == ConnectionState.Selected)
                {
                    Interlocked.Exchange(ref selected, 1);
                }
            };

            using var stop = new CancellationTokenSource();
            connection.Start(stop.Token);

            using var host = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await host.ConnectAsync(IPAddress.Loopback, port);
            await host.SendAsync(ControlMessage(sType: 1, systemBytes: (uint)round)); // Select.req
            await WaitUntilAsync(() => Volatile.Read(ref selected) == 1, TimeSpan.FromSeconds(5));

            using var gate = new Barrier(2);
            var reconnect = Task.Run(() =>
            {
                gate.SignalAndWait();
                connection.Reconnect();
            });
            var dispose = Task.Run(async () =>
            {
                gate.SignalAndWait();
                await connection.DisposeAsync();
            });

            foreach (var (name, path) in new[] { ("Reconnect", reconnect), ("DisposeAsync", dispose) })
            {
                try
                {
                    await path;
                }
                catch (Exception ex)
                {
                    failures.Add($"round {round} {name}: {ex.GetType().Name} {ex.Message}");
                }
            }

            stop.Cancel();
        }

        failures.Should().BeEmpty();
    }
}
