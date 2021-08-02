using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;

namespace Secs4Netb.Benchmark
{
    public class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            //WithArtifactsPath(new DirectoryInfo(Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), 
            //    "BenchmarkDotNet.Artifacts")).FullName);
            // Configure your benchmarks, see for more details: https://benchmarkdotnet.org/articles/configs/configs.html.
            //Add(Job.Dry);
            //Add(ConsoleLogger.Default);
            //Add(TargetMethodColumn.Method, StatisticColumn.Max);
            //Add(RPlotExporter.Default, CsvExporter.Default);
            //Add(EnvironmentAnalyser.Default);
            //Add(Job.Default.WithRuntime)
            AddLogger(ConsoleLogger.Default);
        }
    }
}
