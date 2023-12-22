using BenchmarkDotNet.Running;
using Benchmarks;

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly)
            .Run(args, new BenchmarkConfig());
