```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-LLCSMO : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-LSZLMA : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method     | Runtime  | Count | Mean       | Error     | StdDev    | Ratio        | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |--------- |------ |-----------:|----------:|----------:|-------------:|--------:|-------:|----------:|------------:|
| **Sequential** | **.NET 6.0** | **16**    |   **582.3 μs** |  **11.62 μs** |  **25.26 μs** |     **baseline** |        **** | **0.9766** |  **20.05 KB** |            **** |
| Sequential | .NET 8.0 | 16    |   576.0 μs |  11.22 μs |  18.43 μs | 1.01x faster |   0.05x | 0.9766 |  19.79 KB |  1.01x less |
|            |          |       |            |           |           |              |         |        |           |             |
| Parallel   | .NET 6.0 | 16    | 1,463.0 μs | 101.18 μs | 295.14 μs |     baseline |         |      - |  17.89 KB |             |
| Parallel   | .NET 8.0 | 16    |   815.5 μs |  64.64 μs | 188.55 μs | 1.90x faster |   0.60x | 0.9766 |  17.66 KB |  1.01x less |
|            |          |       |            |           |           |              |         |        |           |             |
| **Sequential** | **.NET 6.0** | **64**    | **2,274.4 μs** |  **45.30 μs** |  **97.52 μs** |     **baseline** |        **** | **3.9063** |  **79.67 KB** |            **** |
| Sequential | .NET 8.0 | 64    | 2,252.4 μs |  44.99 μs | 101.56 μs | 1.01x faster |   0.06x | 3.9063 |  78.66 KB |  1.01x less |
|            |          |       |            |           |           |              |         |        |           |             |
| Parallel   | .NET 6.0 | 64    | 2,416.2 μs | 251.87 μs | 738.68 μs |     baseline |         | 3.9063 |  69.35 KB |             |
| Parallel   | .NET 8.0 | 64    | 1,054.5 μs |  86.59 μs | 226.59 μs | 2.34x faster |   0.91x |      - |  68.08 KB |  1.02x less |
