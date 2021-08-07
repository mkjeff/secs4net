``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1151 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.6.21355.2
  [Host]     : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-GAZESS : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-HKKFQD : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|     Method |              Runtime | Count |        Mean |      Error |     StdDev |        Ratio | RatioSD |   Gen 0 |  Gen 1 | Gen 2 | Allocated |
|----------- |--------------------- |------ |------------:|-----------:|-----------:|-------------:|--------:|--------:|-------:|------:|----------:|
| **Sequential** |             **.NET 6.0** |    **16** |   **198.70 μs** |   **3.074 μs** |   **2.876 μs** | **1.37x faster** |   **0.04x** |  **2.1973** |      **-** |     **-** |     **18 KB** |
| Sequential | .NET Framework 4.7.2 |    16 |   274.55 μs |   4.697 μs |   6.585 μs |     baseline |         |  4.8828 | 0.9766 |     - |     33 KB |
|            |                      |       |             |            |            |              |         |         |        |       |           |
|   Parallel |             .NET 6.0 |    16 |    62.77 μs |   1.225 μs |   2.765 μs | 2.92x faster |   0.61x |  1.8311 |      - |     - |     15 KB |
|   Parallel | .NET Framework 4.7.2 |    16 |   184.64 μs |  11.218 μs |  31.270 μs |     baseline |         |  3.4180 | 0.4883 |     - |     25 KB |
|            |                      |       |             |            |            |              |         |         |        |       |           |
| **Sequential** |             **.NET 6.0** |    **64** |   **663.79 μs** |  **13.161 μs** |  **29.708 μs** | **2.50x faster** |   **0.75x** |  **8.7891** |      **-** |     **-** |     **73 KB** |
| Sequential | .NET Framework 4.7.2 |    64 | 1,770.39 μs | 154.869 μs | 454.205 μs |     baseline |         | 21.4844 | 2.9297 |     - |    136 KB |
|            |                      |       |             |            |            |              |         |         |        |       |           |
|   Parallel |             .NET 6.0 |    64 |   212.01 μs |   4.211 μs |  11.738 μs | 3.01x faster |   0.66x |  7.5684 | 0.4883 |     - |     62 KB |
|   Parallel | .NET Framework 4.7.2 |    64 |   634.79 μs |  46.973 μs | 133.255 μs |     baseline |         | 13.6719 | 1.9531 |     - |     97 KB |
