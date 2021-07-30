using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Secs4Net.UnitTests.Extensions;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Secs4Net.Item;
namespace Secs4Net.UnitTests
{
    public class SecsGemUnitTests
    {
        private readonly PipeConnection connector1;
        private readonly PipeConnection connector2;

        public SecsGemUnitTests()
        {
            var pipe1 = new Pipe(new PipeOptions(
                //readerScheduler: PipeScheduler.Inline, writerScheduler: PipeScheduler.Inline,
                useSynchronizationContext: false));
            var pipe2 = new Pipe(new PipeOptions(
                //readerScheduler: PipeScheduler.Inline, writerScheduler: PipeScheduler.Inline,
                useSynchronizationContext: false));
            connector1 = new PipeConnection(decoderReader: pipe1.Reader, decoderInput: pipe2.Writer);
            connector2 = new PipeConnection(decoderReader: pipe2.Reader, decoderInput: pipe1.Writer);
        }

        [Fact]
        public async Task SecsGem_SendAsync_And_Return_Secondary_Message()
        {
            var options = Options.Create(new SecsGemOptions
            {
                SocketReceiveBufferSize = 32,
                DeviceId = 0,
            });
            using var secsGem1 = new SecsGem(options, connector1, Substitute.For<ISecsGemLogger>());
            using var secsGem2 = new SecsGem(options, connector2, Substitute.For<ISecsGemLogger>());

            var ping = new SecsMessage(s: 1, f: 13)
            {
                SecsItem = A("Ping"),
            };

            var pong = new SecsMessage(s: 1, f: 14, replyExpected: false)
            {
                SecsItem = A("Pong"),
            };

            using var cts = new CancellationTokenSource();
            _ = connector1.StartAsync(cts.Token);
            _ = connector2.StartAsync(cts.Token);
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
            using var secsGem1 = new SecsGem(options1, connector1, Substitute.For<ISecsGemLogger>());
            using var secsGem2 = new SecsGem(options2, connector2, Substitute.For<ISecsGemLogger>());

            var ping = new SecsMessage(s: 1, f: 13)
            {
                SecsItem = A("Ping"),
            };

            using var cts = new CancellationTokenSource();
            _ = connector1.StartAsync(cts.Token);
            _ = connector2.StartAsync(cts.Token);

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
            using var secsGem1 = new SecsGem(options1, connector1, Substitute.For<ISecsGemLogger>());
            using var secsGem2 = new SecsGem(options1, connector2, Substitute.For<ISecsGemLogger>());

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
        public void SecsGem_SendAsync_With_A_Large_Number_Of_Messages_At_Once()
        {
            var options1 = Options.Create(new SecsGemOptions
            {
                IsActive = true,
                DeviceId = 0,
                T3 = 60000,
            });

            var options2 = Options.Create(new SecsGemOptions
            {
                IsActive = false,
                DeviceId = 0,
                T3 = 60000,
            });

            //var connector1 = new HsmsConnection(options1, Substitute.For<ISecsGemLogger>());
            //var connector2 = new HsmsConnection(options2, Substitute.For<ISecsGemLogger>());
            var secsGem1 = new SecsGem(options1, connector1, Substitute.For<ISecsGemLogger>());
            var secsGem2 = new SecsGem(options2, connector2, Substitute.For<ISecsGemLogger>());

            using var cts = new CancellationTokenSource();

            _ = connector1.StartAsync(cts.Token);
            _ = connector2.StartAsync(cts.Token);

            SpinWait.SpinUntil(() => connector1.State == ConnectionState.Selected && connector1.State == ConnectionState.Selected);

            _ = AsyncHelper.LongRunningAsync(async () =>
            {
                var pong = new SecsMessage(s: 1, f: 14, replyExpected: false)
                {
                    SecsItem = A("Pong"),
                };

                await foreach (var a in secsGem2.GetPrimaryMessageAsync(cts.Token))
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
                    totalTasks.Add(secsGem1.SendAsync(ping, cts.Token).AsTask());
                }
                await Task.WhenAll(totalTasks.ToArray());
            };

            sendAsync.Should().NotThrowAsync();
        }
    }
}
