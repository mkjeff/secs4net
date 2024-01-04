using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Options;
using Secs4Net;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Benchmarks;

[Config(typeof(BenchmarkConfig))]
//[EtwProfiler]
[MemoryDiagnoser]
public class RequestResponse
{
    private static readonly SecsMessage ping = new(s: 1, f: 13)
    {
        SecsItem = A("Ping"),
    };

    private static readonly SecsMessage pong = new(s: 1, f: 14, replyExpected: false)
    {
        SecsItem = A("Pong"),
    };

    private CancellationTokenSource _cts;
    private SecsGem _secsGem1;
    private SecsGem _secsGem2;
    private HsmsConnection _connection1;
    private HsmsConnection _connection2;

    [Params(16, 64)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var logger = new Logger();

        _connection1 = new HsmsConnection(Options.Create(new SecsGemOptions
        {
            IsActive = true,
            DeviceId = 0,
            T3 = 60000,
        }), logger);

        _connection2 = new HsmsConnection(Options.Create(new SecsGemOptions
        {
            IsActive = false,
            DeviceId = 0,
            T3 = 60000,
        }), logger);

        var options = Options.Create(new SecsGemOptions
        {
            DeviceId = 0,
        });
        _secsGem1 = new SecsGem(options, _connection1, logger);
        _secsGem2 = new SecsGem(options, _connection2, logger);

        _cts = new CancellationTokenSource();
        _connection1.Start(_cts.Token);
        _connection2.Start(_cts.Token);

        SpinWait.SpinUntil(() => _connection2.State == ConnectionState.Selected);

        Task.Run(async () =>
        {
            await foreach (var a in _secsGem2.GetPrimaryMessageAsync(_cts.Token))
            {
                using var primaryMessage = a.PrimaryMessage;
                await a.TryReplyAsync(pong, _cts.Token);
            }
        });
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _secsGem1?.Dispose();
        _secsGem2?.Dispose();
        if (_connection1 is not null)
        {
            await _connection1.DisposeAsync();
        }

        if (_connection2 is not null)
        {
            await _connection2.DisposeAsync();
        }
    }

    [Benchmark(Description = "Sequential")]
    public async Task<int> SequentialSendAsync()
    {
        for (int i = 0; i < Count; i++)
        {
            using var pong = await _secsGem1.SendAsync(ping);
        }
        return Count;
    }

    [Benchmark(Description = "Parallel")]
    public async Task<int> ParallelSendAsync()
    {
        var tasks = new Task<SecsMessage>[Count];
        for (int i = 0; i < Count; i++)
        {
            tasks[i] = _secsGem1.SendAsync(ping);
        }
        var replies = await Task.WhenAll(tasks).ConfigureAwait(false);

        Array.ForEach(replies, a => a.Dispose());
        return replies.Length;
    }

    private sealed class Logger : ISecsGemLogger
    {
        public void Debug(string msg) { }
        public void Error(string msg) { }
        public void Error(string msg, Exception ex) { }
        public void Error(string msg, SecsMessage message, Exception ex) { }
        public void Info(string msg) { }
        public void MessageIn(SecsMessage msg, int id) { }
        public void MessageOut(SecsMessage msg, int id) { }
        public void Warning(string msg) { }
    }
}
