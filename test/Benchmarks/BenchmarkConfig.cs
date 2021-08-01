using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using System.IO;

namespace benchmark
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
