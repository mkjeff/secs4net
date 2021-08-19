namespace Secs4Net;

internal static class AsyncHelper
{
    public static Task LongRunningAsync(Func<Task> func, CancellationToken cancellation = default)
        => Task.Factory.StartNew(func, cancellation,
            TaskCreationOptions.LongRunning | TaskCreationOptions.PreferFairness | TaskCreationOptions.RunContinuationsAsynchronously, TaskScheduler.Default).Unwrap();
}
