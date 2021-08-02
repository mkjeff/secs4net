using System;
using System.Threading;
using System.Threading.Tasks;

namespace Secs4Netb.Benchmark
{
    internal static class AsyncHelper
    {
        public static Task LongRunningAsync(Func<Task> func, CancellationToken cancellation = default)
            => Task.Factory.StartNew(func, cancellation,
                TaskCreationOptions.LongRunning | TaskCreationOptions.PreferFairness | TaskCreationOptions.RunContinuationsAsynchronously, TaskScheduler.Default).Unwrap();
    }
}
