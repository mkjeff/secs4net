using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using System.Linq;

namespace Secs4Net.Benchmark;

public class BenchmarkConfig : ManualConfig
{
    public BenchmarkConfig()
    {
        this.WithCultureInfo(System.Globalization.CultureInfo.InvariantCulture);

        WithOption(ConfigOptions.DisableLogFile, true);
        WithOption(ConfigOptions.DontOverwriteResults, false);
        AddLogger(ConsoleLogger.Default);
        AddExporter(MarkdownExporter.GitHub);

        AddColumnProvider(
            DefaultColumnProviders.Descriptor,
            DefaultColumnProviders.Params,
            new SimpleColumnProvider(JobCharacteristicColumn.AllColumns.Where(c => c.ColumnName == "Runtime").ToArray()),
            DefaultColumnProviders.Statistics,
            DefaultColumnProviders.Metrics);

        SummaryStyle = BenchmarkDotNet.Reports.SummaryStyle.Default
            .WithRatioStyle(RatioStyle.Trend);
    }
}
