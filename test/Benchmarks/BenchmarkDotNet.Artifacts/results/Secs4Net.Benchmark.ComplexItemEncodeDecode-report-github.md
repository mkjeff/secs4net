``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.400
  [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  Job-AITDSN : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  Job-TPKEIE : .NET Framework 4.8 (4.8.4515.0), X64 RyuJIT


```
| Method |              Runtime | ItemCount |        Mean |    Error |   StdDev |        Ratio | RatioSD | Allocated |
|------- |--------------------- |---------- |------------:|---------:|---------:|-------------:|--------:|----------:|
| **Encode** |             **.NET 6.0** |         **0** |    **442.0 ns** |  **0.85 ns** |  **0.67 ns** | **2.74x faster** |   **0.01x** |      **40 B** |
| Encode | .NET Framework 4.7.2 |         0 |  1,210.6 ns |  4.83 ns |  4.52 ns |     baseline |         |      40 B |
|        |                      |           |             |          |          |              |         |           |
| Decode |             .NET 6.0 |         0 |  2,475.3 ns |  8.87 ns |  8.30 ns | 2.59x faster |   0.01x |     560 B |
| Decode | .NET Framework 4.7.2 |         0 |  6,417.8 ns | 13.85 ns | 12.27 ns |     baseline |         |     562 B |
|        |                      |           |             |          |          |              |         |           |
| **Encode** |             **.NET 6.0** |        **64** |  **1,353.9 ns** |  **7.88 ns** |  **7.37 ns** | **3.42x faster** |   **0.03x** |      **40 B** |
| Encode | .NET Framework 4.7.2 |        64 |  4,629.6 ns | 19.04 ns | 16.88 ns |     baseline |         |      40 B |
|        |                      |           |             |          |          |              |         |           |
| Decode |             .NET 6.0 |        64 |  4,557.0 ns | 33.11 ns | 30.97 ns | 4.61x faster |   0.03x |   7,248 B |
| Decode | .NET Framework 4.7.2 |        64 | 20,990.3 ns | 73.79 ns | 69.02 ns |     baseline |         |   7,269 B |
|        |                      |           |             |          |          |              |         |           |
| **Encode** |             **.NET 6.0** |       **128** |  **1,916.3 ns** |  **8.14 ns** |  **7.62 ns** | **3.37x faster** |   **0.02x** |      **40 B** |
| Encode | .NET Framework 4.7.2 |       128 |  6,456.2 ns | 39.97 ns | 37.38 ns |     baseline |         |      40 B |
|        |                      |           |             |          |          |              |         |           |
| Decode |             .NET 6.0 |       128 |  5,272.5 ns | 22.88 ns | 21.40 ns | 4.69x faster |   0.03x |   9,112 B |
| Decode | .NET Framework 4.7.2 |       128 | 24,712.8 ns | 98.74 ns | 87.53 ns |     baseline |         |   9,139 B |
