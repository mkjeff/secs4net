using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Microsoft.Extensions.Options;
using System;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace Secs4Net.Benchmark;

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

    [Params(16, 64)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var pipe1 = new Pipe(new PipeOptions(useSynchronizationContext: false));
        var pipe2 = new Pipe(new PipeOptions(useSynchronizationContext: false));
        var connection1 = new PipeConnection(decoderReader: pipe1.Reader, decoderInput: pipe2.Writer);
        var connection2 = new PipeConnection(decoderReader: pipe2.Reader, decoderInput: pipe1.Writer);

        var options = Options.Create(new SecsGemOptions
        {
            DeviceId = 0,
        });
        var logger = new Logger();
        _secsGem1 = new SecsGem(options, connection1, logger);
        _secsGem2 = new SecsGem(options, connection2, logger);

        _cts = new CancellationTokenSource();
        Task.Run(()=> connection1.StartAsync(_cts.Token));
        Task.Run(() => connection2.StartAsync(_cts.Token));
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
    public void Cleanup()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _secsGem1?.Dispose();
        _secsGem2?.Dispose();
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
            tasks[i] = _secsGem1.SendAsync(ping).AsTask();
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
