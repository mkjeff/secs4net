``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.400
  [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  Job-AITDSN : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  Job-TPKEIE : .NET Framework 4.8 (4.8.4515.0), X64 RyuJIT


```
|     Method |              Runtime | Count |        Mean |     Error |      StdDev |         Ratio | RatioSD |   Gen 0 |   Gen 1 | Allocated |
|----------- |--------------------- |------ |------------:|----------:|------------:|--------------:|--------:|--------:|--------:|----------:|
| **Sequential** |             **.NET 6.0** |    **16** |    **875.8 μs** |  **16.74 μs** |    **18.61 μs** |  **1.34x faster** |   **0.03x** |  **1.9531** |       **-** |     **20 KB** |
| Sequential | .NET Framework 4.7.2 |    16 |  1,174.8 μs |  21.70 μs |    28.96 μs |      baseline |         | 19.5313 |  3.9063 |    123 KB |
|            |                      |       |             |           |             |               |         |         |         |           |
|   Parallel |             .NET 6.0 |    16 |    622.9 μs |  39.23 μs |   114.44 μs | 10.87x faster |   2.77x |  1.9531 |       - |     18 KB |
|   Parallel | .NET Framework 4.7.2 |    16 |  6,537.8 μs | 363.41 μs | 1,054.32 μs |      baseline |         | 15.6250 |  7.8125 |    116 KB |
|            |                      |       |             |           |             |               |         |         |         |           |
| **Sequential** |             **.NET 6.0** |    **64** |  **2,260.9 μs** |  **44.23 μs** |    **70.15 μs** |  **1.73x faster** |   **0.06x** |  **7.8125** |       **-** |     **80 KB** |
| Sequential | .NET Framework 4.7.2 |    64 |  3,914.0 μs |  65.54 μs |    87.49 μs |      baseline |         | 78.1250 | 15.6250 |    489 KB |
|            |                      |       |             |           |             |               |         |         |         |           |
|   Parallel |             .NET 6.0 |    64 |  1,199.1 μs |  52.63 μs |   151.84 μs | 12.64x faster |   1.96x |  7.8125 |       - |     69 KB |
|   Parallel | .NET Framework 4.7.2 |    64 | 14,960.8 μs | 579.10 μs | 1,707.48 μs |      baseline |         | 62.5000 | 15.6250 |    430 KB |
