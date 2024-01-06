```

BenchmarkDotNet v0.13.11, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-ZWGTBM : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-ZQLZSO : .NET Framework 4.8.1 (4.8.9181.0), X64 RyuJIT VectorSize=256


```
| Method     | Runtime            | Count | Mean        | Error     | StdDev      | Ratio         | RatioSD | Gen0    | Gen1    | Allocated | Alloc Ratio |
|----------- |------------------- |------ |------------:|----------:|------------:|--------------:|--------:|--------:|--------:|----------:|------------:|
| **Sequential** | **.NET 8.0**           | **16**    |    **514.1 μs** |   **6.62 μs** |     **5.87 μs** |  **1.37x faster** |   **0.04x** |  **0.9766** |       **-** |  **19.79 KB** |  **6.17x less** |
| Sequential | .NET Framework 4.8 | 16    |    705.6 μs |  14.02 μs |    15.59 μs |      baseline |         | 19.5313 |  1.9531 | 122.08 KB |             |
|            |                    |       |             |           |             |               |         |         |         |           |             |
| Parallel   | .NET 8.0           | 16    |    817.6 μs |  51.73 μs |   151.72 μs |  7.29x faster |   1.96x |  0.9766 |       - |  17.32 KB |  6.53x less |
| Parallel   | .NET Framework 4.8 | 16    |  5,768.9 μs | 321.92 μs |   949.19 μs |      baseline |         | 15.6250 |  7.8125 | 113.13 KB |             |
|            |                    |       |             |           |             |               |         |         |         |           |             |
| **Sequential** | **.NET 8.0**           | **64**    |  **1,998.2 μs** |  **17.72 μs** |    **15.70 μs** |  **1.38x faster** |   **0.03x** |  **3.9063** |       **-** |  **78.67 KB** |  **6.19x less** |
| Sequential | .NET Framework 4.8 | 64    |  2,760.6 μs |  49.70 μs |    55.24 μs |      baseline |         | 78.1250 |  7.8125 | 486.88 KB |             |
|            |                    |       |             |           |             |               |         |         |         |           |             |
| Parallel   | .NET 8.0           | 64    |  1,073.7 μs |  43.21 μs |   122.58 μs | 11.55x faster |   2.35x |  3.9063 |       - |  68.08 KB |  6.23x less |
| Parallel   | .NET Framework 4.8 | 64    | 12,282.5 μs | 606.24 μs | 1,787.51 μs |      baseline |         | 62.5000 | 15.6250 | 424.01 KB |             |
