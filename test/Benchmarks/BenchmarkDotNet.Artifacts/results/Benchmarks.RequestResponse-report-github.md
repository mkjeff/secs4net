```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3007/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  Job-ERYFHQ : .NET 6.0.26 (6.0.2623.60508), X64 RyuJIT AVX2
  Job-GVMCZA : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  Job-BVWEYE : .NET Framework 4.8.1 (4.8.9181.0), X64 RyuJIT VectorSize=256


```
| Method     | Runtime            | Count | Mean        | Error     | StdDev      | Median      | Ratio        | RatioSD | Gen0    | Gen1    | Allocated | Alloc Ratio |
|----------- |------------------- |------ |------------:|----------:|------------:|------------:|-------------:|--------:|--------:|--------:|----------:|------------:|
| **Sequential** | **.NET 6.0**           | **16**    |    **628.5 μs** |  **12.56 μs** |    **26.76 μs** |    **627.7 μs** | **1.92x faster** |   **0.07x** |  **0.9766** |       **-** |  **21.05 KB** |  **5.65x less** |
| Sequential | .NET 8.0           | 16    |    866.4 μs |  17.09 μs |    41.27 μs |    871.7 μs | 1.50x faster |   0.18x |  0.9766 |       - |  20.79 KB |  5.72x less |
| Sequential | .NET Framework 4.8 | 16    |  1,235.3 μs |   6.95 μs |     6.51 μs |  1,232.5 μs |     baseline |         | 18.5547 |  1.9531 | 118.99 KB |             |
|            |                    |       |             |           |             |             |              |         |         |         |           |             |
| Parallel   | .NET 6.0           | 16    |  1,406.4 μs | 221.29 μs |   652.48 μs |  1,394.3 μs | 4.46x faster |   3.74x |       - |       - |  19.12 KB |  6.11x less |
| Parallel   | .NET 8.0           | 16    |    474.7 μs |  13.03 μs |    37.82 μs |    476.4 μs | 8.85x faster |   2.53x |  0.9766 |       - |  18.94 KB |  6.17x less |
| Parallel   | .NET Framework 4.8 | 16    |  4,230.2 μs | 399.77 μs | 1,178.73 μs |  3,956.0 μs |     baseline |         | 15.6250 |  7.8125 | 116.88 KB |             |
|            |                    |       |             |           |             |             |              |         |         |         |           |             |
| **Sequential** | **.NET 6.0**           | **64**    |  **3,687.4 μs** |  **73.49 μs** |   **188.37 μs** |  **3,690.2 μs** | **1.62x faster** |   **0.72x** |  **3.9063** |       **-** |  **83.67 KB** |  **5.95x less** |
| Sequential | .NET 8.0           | 64    |  3,294.2 μs | 123.77 μs |   364.94 μs |  3,418.5 μs | 1.77x faster |   0.66x |       - |       - |  82.67 KB |  6.02x less |
| Sequential | .NET Framework 4.8 | 64    |  5,889.9 μs | 805.71 μs | 2,375.65 μs |  5,411.4 μs |     baseline |         | 78.1250 |  7.8125 |  497.6 KB |             |
|            |                    |       |             |           |             |             |              |         |         |         |           |             |
| Parallel   | .NET 6.0           | 64    |  1,585.1 μs |  36.65 μs |   105.16 μs |  1,562.8 μs | 7.78x faster |   1.32x |  3.9063 |       - |  74.31 KB |  5.78x less |
| Parallel   | .NET 8.0           | 64    |  1,908.2 μs | 285.32 μs |   827.75 μs |  1,638.1 μs | 7.63x faster |   3.28x |  3.9063 |       - |  72.17 KB |  5.95x less |
| Parallel   | .NET Framework 4.8 | 64    | 12,267.3 μs | 593.38 μs | 1,740.28 μs | 12,084.2 μs |     baseline |         | 62.5000 | 15.6250 | 429.38 KB |             |
