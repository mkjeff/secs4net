using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Secs4Net.UnitTests;

public class SecsGemUnitTests
{
    private readonly PipeConnection pipeConnection1;
    private readonly PipeConnection pipeConnection2;
    private readonly IOptions<SecsGemOptions> optionsActive = Options.Create(new SecsGemOptions
    {
        IsActive = true,
        DeviceId = 0,
        T3 = 60000,
    });

    private readonly IOptions<SecsGemOptions> optionsPassive = Options.Create(new SecsGemOptions
    {
        IsActive = false,
        DeviceId = 0,
        T3 = 60000,
    });

    public SecsGemUnitTests()
    {
        var pipe1 = new Pipe(new PipeOptions(useSynchronizationContext: false));
        var pipe2 = new Pipe(new PipeOptions(useSynchronizationContext: false));
        pipeConnection1 = new PipeConnection(decoderReader: pipe1.Reader, decoderInput: pipe2.Writer);
        pipeConnection2 = new PipeConnection(decoderReader: pipe2.Reader, decoderInput: pipe1.Writer);
    }

    [Fact]
    public async Task SecsGem_SendAsync_And_Return_Secondary_Message()
    {
        var options = Options.Create(new SecsGemOptions
        {
            SocketReceiveBufferSize = 32,
            DeviceId = 0,
        });
        using var secsGem1 = new SecsGem(options, pipeConnection1, Substitute.For<ISecsGemLogger>());
        using var secsGem2 = new SecsGem(options, pipeConnection2, Substitute.For<ISecsGemLogger>());

        var ping = new SecsMessage(s: 1, f: 13)
        {
            SecsItem = A("Ping"),
        };

        var pong = new SecsMessage(s: 1, f: 14, replyExpected: false)
        {
            SecsItem = A("Pong"),
        };

        using var cts = new CancellationTokenSource();
        _ = pipeConnection1.StartAsync(cts.Token);
        _ = pipeConnection2.StartAsync(cts.Token);
        _ = Task.Run(async () =>
        {
            var msg = await secsGem2.GetPrimaryMessageAsync(cts.Token).FirstAsync(cts.Token);
            msg.PrimaryMessage.Should().BeEquivalentTo(ping);
            await msg.TryReplyAsync(pong);
        });

        var reply = await secsGem1.SendAsync(ping, cts.Token);
        reply.Should().NotBeNull().And.BeEquivalentTo(pong);
    }

    [Fact]
    public void SecsGem_SendAsync_With_Different_Device_Id()
    {
        var options1 = Options.Create(new SecsGemOptions
        {
            SocketReceiveBufferSize = 32,
            DeviceId = 0,
        });
        var options2 = Options.Create(new SecsGemOptions
        {
            SocketReceiveBufferSize = 32,
            DeviceId = 1,
        });
        using var secsGem1 = new SecsGem(options1, pipeConnection1, Substitute.For<ISecsGemLogger>());
        using var secsGem2 = new SecsGem(options2, pipeConnection2, Substitute.For<ISecsGemLogger>());

        var ping = new SecsMessage(s: 1, f: 13)
        {
            SecsItem = A("Ping"),
        };

        using var cts = new CancellationTokenSource();
        _ = pipeConnection1.StartAsync(cts.Token);
        _ = pipeConnection2.StartAsync(cts.Token);

        var receiver = Substitute.For<Action<SecsMessage>>();
        _ = Task.Run(async () =>
        {
            await foreach (var a in secsGem2.GetPrimaryMessageAsync(cts.Token))
            {
                // can't receive any message, reply S9F1 internally
                receiver(a.PrimaryMessage);
            }
        });

        Func<Task> sendAsync = async () =>
        {
            var reply = await secsGem1.SendAsync(ping, cts.Token);
        };

        sendAsync.Should().ThrowAsync<SecsException>().WithMessage(Resources.S9F1);
        receiver.DidNotReceive();
    }

    [Fact]
    public void SecsGem_SendAsync_With_T3_Timeout()
    {
        var options1 = Options.Create(new SecsGemOptions
        {
            SocketReceiveBufferSize = 32,
            DeviceId = 0,
            T3 = 500,
        });
        using var secsGem1 = new SecsGem(options1, pipeConnection1, Substitute.For<ISecsGemLogger>());
        using var secsGem2 = new SecsGem(options1, pipeConnection2, Substitute.For<ISecsGemLogger>());

        var ping = new SecsMessage(s: 1, f: 13)
        {
            SecsItem = A("Ping"),
        };
        var pong = new SecsMessage(s: 1, f: 14, replyExpected: false)
        {
            SecsItem = A("Pong"),
        };

        using var cts = new CancellationTokenSource();

        _ = Task.Run(async () =>
        {
            await foreach (var a in secsGem2.GetPrimaryMessageAsync(cts.Token))
            {
                await Task.Delay(TimeSpan.FromMilliseconds(options1.Value.T3 + 100)); // process delay over T3
                await a.TryReplyAsync(pong);
            }
        });

        var receiver = Substitute.For<Action>();
        Func<Task> sendAsync = async () =>
        {
            var reply = await secsGem1.SendAsync(ping, cts.Token);
            receiver();
        };

        sendAsync.Should().ThrowAsync<SecsException>().WithMessage(Resources.T3Timeout);
        receiver.DidNotReceive();
    }

    [Fact]
    public void SecsGem_PipeConnection_SendAsync_With_A_Large_Number_Of_Messages_At_Once()
    {
        using var cts = new CancellationTokenSource();
        SendAsyncManyMessagesAtOnce(pipeConnection1, pipeConnection2, cts.Token);
    }

    [Fact]
    public void SecsGem_HsmsConnection_SendAsync_With_A_Large_Number_Of_Messages_At_Once()
    {
        using var connection1 = new HsmsConnection(optionsActive, Substitute.For<ISecsGemLogger>());
        using var connection2 = new HsmsConnection(optionsPassive, Substitute.For<ISecsGemLogger>());
        using var cts = new CancellationTokenSource();

        _ = connection1.StartAsync(cts.Token);
        _ = connection2.StartAsync(cts.Token);

        SpinWait.SpinUntil(() => pipeConnection1.State == ConnectionState.Selected && pipeConnection1.State == ConnectionState.Selected);

        SendAsyncManyMessagesAtOnce(connection1, connection2, cts.Token);
    }

    private void SendAsyncManyMessagesAtOnce(ISecsConnection connection1, ISecsConnection connection2, CancellationToken cancellation)
    {
        using var secsGem1 = new SecsGem(optionsActive, connection1, Substitute.For<ISecsGemLogger>());
        using var secsGem2 = new SecsGem(optionsPassive, connection2, Substitute.For<ISecsGemLogger>());

        _ = Task.Run(async () =>
        {
            var pong = new SecsMessage(s: 1, f: 14, replyExpected: false)
            {
                SecsItem = A("Pong"),
            };

            await foreach (var a in secsGem2.GetPrimaryMessageAsync(cancellation))
            {
                await a.TryReplyAsync(pong);
            }
        });

        Func<Task> sendAsync = async () =>
        {
            var ping = new SecsMessage(s: 1, f: 13)
            {
                SecsItem = A("Ping"),
            };

            var sendCount = 10000;
            var totalTasks = new List<Task<SecsMessage>>(capacity: sendCount);
            for (var g = 0; g < sendCount; g++)
            {
                totalTasks.Add(secsGem1.SendAsync(ping, cancellation));
            }
            await Task.WhenAll(totalTasks.ToArray());
        };

        sendAsync.Should().NotThrowAsync();
    }
}
