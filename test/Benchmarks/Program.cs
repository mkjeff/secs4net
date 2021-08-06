using BenchmarkDotNet.Running;

namespace Secs4Net.Benchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly)
                .Run(args, new BenchmarkConfig());
        }
    }
}
