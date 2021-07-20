using System;
using System.Threading;
using System.Threading.Tasks;

namespace Secs4Net
{
    internal static class AsyncHelper
    {
        public static Task LongRunningAsync(Func<Task> func, CancellationToken cancellationToken = default)
            => Task.Factory.StartNew(func, cancellationToken,
                TaskCreationOptions.LongRunning | TaskCreationOptions.PreferFairness | TaskCreationOptions.RunContinuationsAsynchronously, TaskScheduler.Default).Unwrap();
    }
}
