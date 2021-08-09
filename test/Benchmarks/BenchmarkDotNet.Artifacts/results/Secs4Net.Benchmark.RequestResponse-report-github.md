``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1151 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.6.21355.2
  [Host]     : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-PSNALV : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-DJMMZE : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|     Method |              Runtime | Count |        Mean |      Error |     StdDev |      Median |        Ratio | RatioSD |   Gen 0 |  Gen 1 | Gen 2 | Allocated |
|----------- |--------------------- |------ |------------:|-----------:|-----------:|------------:|-------------:|--------:|--------:|-------:|------:|----------:|
| **Sequential** |             **.NET 6.0** |    **16** |   **171.93 μs** |   **3.289 μs** |   **5.673 μs** |   **171.41 μs** |     **baseline** |        **** |  **1.9531** |      **-** |     **-** |     **18 KB** |
| Sequential | .NET Framework 4.7.2 |    16 |   376.73 μs |  34.970 μs | 101.454 μs |   337.50 μs | 1.79x slower |   0.44x |  5.3711 | 0.9766 |     - |     34 KB |
|            |                      |       |             |            |            |             |              |         |         |        |       |           |
|   Parallel |             .NET 6.0 |    16 |    63.67 μs |   1.262 μs |   2.823 μs |    63.83 μs |     baseline |         |  1.8311 |      - |     - |     15 KB |
|   Parallel | .NET Framework 4.7.2 |    16 |   191.87 μs |  15.479 μs |  44.162 μs |   182.75 μs | 2.87x slower |   0.71x |  3.4180 | 0.4883 |     - |     24 KB |
|            |                      |       |             |            |            |             |              |         |         |        |       |           |
| **Sequential** |             **.NET 6.0** |    **64** |   **711.78 μs** |  **13.618 μs** |  **12.072 μs** |   **713.06 μs** |     **baseline** |        **** |  **8.7891** |      **-** |     **-** |     **73 KB** |
| Sequential | .NET Framework 4.7.2 |    64 | 1,921.77 μs | 179.980 μs | 527.850 μs | 1,954.10 μs | 1.45x slower |   0.06x | 21.4844 | 2.9297 |     - |    136 KB |
|            |                      |       |             |            |            |             |              |         |         |        |       |           |
|   Parallel |             .NET 6.0 |    64 |   223.09 μs |   6.109 μs |  17.821 μs |   222.37 μs |     baseline |         |  7.3242 | 0.4883 |     - |     62 KB |
|   Parallel | .NET Framework 4.7.2 |    64 |   736.16 μs |  72.573 μs | 208.226 μs |   682.14 μs | 3.31x slower |   0.98x | 13.6719 | 1.9531 |     - |     98 KB |
