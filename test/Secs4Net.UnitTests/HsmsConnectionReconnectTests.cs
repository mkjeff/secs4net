using FluentAssertions;
using Microsoft.Extensions.Options;
using System;
using System.Buffers.Binary;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Secs4Net.UnitTests;

public class HsmsConnectionReconnectTests
{
    private sealed class ErrorLogger : ISecsGemLogger
    {
        public ConcurrentQueue<string> Errors { get; } = new();

        public void Debug(string msg) { }
        public void Info(string msg) { }
        public void Warning(string msg) { }

        public void Error(string msg, SecsMessage? message, Exception? ex)
        {
            if (ex is not null)
            {
                Errors.Enqueue($"{msg} {ex.GetType().Name}: {ex.Message}");
            }
        }
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

    /// <summary>
    /// The receive pipe and its decoder are shared by every link of one <see cref="HsmsConnection"/>.
    /// When a link is lost, the decoder's consumer and producer are cancelled, but the next link must not start
    /// reading the pipe until they have actually finished. Otherwise a host that reconnects within a few milliseconds
    /// makes the new consumer fail with "Reading is already in progress", and its error handler calls
    /// <see cref="HsmsConnection.Reconnect"/>, dropping the host that has just been accepted.
    /// <para>
    /// To make the window as narrow as possible, the next host connects first and waits in the listen backlog,
    /// then the current link is lost. The accept for the next host returns right after the retry.
    /// </para>
    /// </summary>
    [Theory]
    [InlineData("close", 1000)]    // the host closes its socket: the producer reads 0 bytes
    [InlineData("separate", 300)]  // the host sends Separate.req
    public async Task New_Link_Receives_After_Immediate_Reconnect(string how, int cycles)
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

        var logger = new ErrorLogger();
        await using var connection = new HsmsConnection(options, logger);
        using var stop = new CancellationTokenSource();
        connection.Start(stop.Token);

        var current = await SelectAsync(port, 1, connected: null);
        for (var cycle = 2; cycle <= cycles; cycle++)
        {
            var next = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await next.ConnectAsync(IPAddress.Loopback, port);

            if (how == "close")
            {
                current.Close();
            }
            else
            {
                await current.SendAsync(ControlMessage(sType: 9, systemBytes: (uint)cycle * 2 + 1_000_000)); // Separate.req
            }

            var previous = current;
            try
            {
                current = await SelectAsync(port, (uint)cycle, connected: next);
            }
            catch (Exception ex)
            {
                next.Dispose();
                previous.Dispose();
                throw new Xunit.Sdk.XunitException(
                    $"{how} cycle {cycle}: {ex.Message} errors: {string.Join(" | ", logger.Errors)}");
            }

            previous.Dispose();
            logger.Errors.Should().BeEmpty($"{how} cycle {cycle}");
        }

        current.Dispose();
        stop.Cancel();
    }

    /// <summary>Selects, then exchanges one Linktest to show that the receive path of this link is alive.</summary>
    private static async Task<Socket> SelectAsync(int port, uint id, Socket? connected)
    {
        var host = connected ?? new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        if (connected is null)
        {
            await host.ConnectAsync(IPAddress.Loopback, port);
        }

        await host.SendAsync(ControlMessage(sType: 1, systemBytes: id * 2)); // Select.req
        (await ReceiveControlAsync(host, id, "Select.rsp"))[9].Should().Be(2);
        await host.SendAsync(ControlMessage(sType: 5, systemBytes: id * 2 + 1)); // Linktest.req
        (await ReceiveControlAsync(host, id, "Linktest.rsp"))[9].Should().Be(6);
        return host;
    }

    private static async Task<byte[]> ReceiveControlAsync(Socket host, uint id, string what)
    {
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(3));
        var frame = new byte[14];
        var read = 0;
        while (read < frame.Length)
        {
            var n = await host.ReceiveAsync(frame.AsMemory(read), SocketFlags.None, timeout.Token);
            if (n == 0)
            {
                throw new InvalidOperationException($"cycle {id}: the connection dropped the new host before {what}");
            }

            read += n;
        }

        return frame;
    }
}
