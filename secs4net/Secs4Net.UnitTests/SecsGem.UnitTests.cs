using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Secs4Net.UnitTests.Extensions;
using System;
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
        private readonly IHsmsConnection connector1;
        private readonly IHsmsConnection connector2;

        public SecsGemUnitTests()
        {
            var pipe1 = new Pipe();
            var pipe2 = new Pipe();
            connector1 = new PipeConnection(new PipeDecoder(pipe1.Reader, input: pipe2.Writer));
            connector2 = new PipeConnection(new PipeDecoder(pipe2.Reader, input: pipe1.Writer));
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
            _ = Task.Run(async () =>
            {
                var msg = await secsGem2.GetPrimaryMessageAsync(cts.Token).FirstAsync(cts.Token);
                msg.PrimaryMessage.Should().BeEquivalentTo(ping);
                await msg.TryReplyAsync(pong);
            });

            var reply = await secsGem1.SendAsync(ping, cts.Token);
            reply.Should().BeEquivalentTo(pong);
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

            sendAsync.Should().Throw<SecsException>().WithMessage(Resources.S9F1);
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

            Func<Task> sendAsync = async () =>
            {
                var reply = await secsGem1.SendAsync(ping, cts.Token);
                reply.Should().NotBeEquivalentTo(pong);
            };

            sendAsync.Should().Throw<SecsException>().WithMessage(Resources.T3Timeout);
        }
    }
}
