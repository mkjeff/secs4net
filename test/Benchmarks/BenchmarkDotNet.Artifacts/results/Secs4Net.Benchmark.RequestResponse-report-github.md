``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1151 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.6.21355.2
  [Host]     : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-KDEKXM : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-MYICZP : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|     Method |              Runtime | Count |        Mean |     Error |     StdDev |        Ratio | RatioSD |   Gen 0 |  Gen 1 | Gen 2 | Allocated |
|----------- |--------------------- |------ |------------:|----------:|-----------:|-------------:|--------:|--------:|-------:|------:|----------:|
| **Sequential** |             **.NET 6.0** |    **16** |   **198.94 μs** |  **2.788 μs** |   **2.608 μs** | **1.42x faster** |   **0.05x** |  **2.1973** |      **-** |     **-** |     **18 KB** |
| Sequential | .NET Framework 4.7.2 |    16 |   396.78 μs | 37.450 μs | 108.052 μs |     baseline |         |  5.3711 | 0.9766 |     - |     34 KB |
|            |                      |       |             |           |            |              |         |         |        |       |           |
|   Parallel |             .NET 6.0 |    16 |    62.61 μs |  1.216 μs |   3.390 μs | 3.42x faster |   0.96x |  1.8311 |      - |     - |     15 KB |
|   Parallel | .NET Framework 4.7.2 |    16 |   212.97 μs | 18.616 μs |  54.303 μs |     baseline |         |  3.4180 | 0.4883 |     - |     25 KB |
|            |                      |       |             |           |            |              |         |         |        |       |           |
| **Sequential** |             **.NET 6.0** |    **64** |   **734.85 μs** | **10.313 μs** |   **9.647 μs** | **1.48x faster** |   **0.04x** |  **8.7891** |      **-** |     **-** |     **73 KB** |
| Sequential | .NET Framework 4.7.2 |    64 | 1,084.08 μs | 21.127 μs |  25.150 μs |     baseline |         | 19.5313 | 3.9063 |     - |    133 KB |
|            |                      |       |             |           |            |              |         |         |        |       |           |
|   Parallel |             .NET 6.0 |    64 |   205.79 μs |  4.515 μs |  13.171 μs | 3.43x faster |   0.93x |  7.5684 | 0.4883 |     - |     62 KB |
|   Parallel | .NET Framework 4.7.2 |    64 |   702.14 μs | 60.869 μs | 176.593 μs |     baseline |         | 13.6719 | 1.9531 |     - |     96 KB |
