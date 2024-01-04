```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-OOEVDF : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-AKZPHK : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method     | Runtime  | Count | Mean       | Error     | StdDev    | Median     | Ratio        | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |--------- |------ |-----------:|----------:|----------:|-----------:|-------------:|--------:|-------:|----------:|------------:|
| **Sequential** | **.NET 6.0** | **16**    |   **573.7 μs** |  **12.73 μs** |  **37.14 μs** |   **571.3 μs** |     **baseline** |        **** | **0.9766** |  **20.05 KB** |            **** |
| Sequential | .NET 8.0 | 16    |   569.0 μs |  10.92 μs |  30.98 μs |   552.3 μs | 1.01x faster |   0.08x |      - |  19.79 KB |  1.01x less |
|            |          |       |            |           |           |            |              |         |        |           |             |
| Parallel   | .NET 6.0 | 16    | 1,248.7 μs |  89.42 μs | 255.12 μs | 1,231.5 μs |     baseline |         |      - |  17.98 KB |             |
| Parallel   | .NET 8.0 | 16    |   852.7 μs |  75.94 μs | 223.92 μs |   808.1 μs | 1.57x faster |   0.54x | 0.9766 |  17.51 KB |  1.03x less |
|            |          |       |            |           |           |            |              |         |        |           |             |
| **Sequential** | **.NET 6.0** | **64**    | **2,116.6 μs** |  **42.27 μs** | **123.96 μs** | **2,115.0 μs** |     **baseline** |        **** | **5.8594** |  **79.67 KB** |            **** |
| Sequential | .NET 8.0 | 64    | 2,006.1 μs |  39.79 μs |  57.07 μs | 1,995.9 μs | 1.01x slower |   0.06x | 3.9063 |  78.66 KB |  1.01x less |
|            |          |       |            |           |           |            |              |         |        |           |             |
| Parallel   | .NET 6.0 | 64    | 2,291.9 μs | 243.09 μs | 701.36 μs | 2,057.5 μs |     baseline |         | 3.9063 |  69.04 KB |             |
| Parallel   | .NET 8.0 | 64    |   974.3 μs | 100.30 μs | 269.46 μs |   948.5 μs | 2.53x faster |   1.08x |      - |  68.01 KB |  1.02x less |
