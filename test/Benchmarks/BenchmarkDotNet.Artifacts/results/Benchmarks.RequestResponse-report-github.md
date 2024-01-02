```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-JZAXRR : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-NGSZRM : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method     | Runtime  | Count | Mean       | Error     | StdDev    | Median     | Ratio        | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |--------- |------ |-----------:|----------:|----------:|-----------:|-------------:|--------:|-------:|----------:|------------:|
| **Sequential** | **.NET 6.0** | **16**    |   **931.9 μs** |  **60.27 μs** | **175.82 μs** | **1,001.1 μs** |     **baseline** |        **** | **0.9766** |  **20.05 KB** |            **** |
| Sequential | .NET 8.0 | 16    |   685.5 μs |  58.04 μs | 171.12 μs |   826.9 μs | 1.45x faster |   0.49x | 0.9766 |  19.79 KB |  1.01x less |
|            |          |       |            |           |           |            |              |         |        |           |             |
| Parallel   | .NET 6.0 | 16    |   686.2 μs |  35.14 μs | 102.51 μs |   676.2 μs |     baseline |         |      - |   18.1 KB |             |
| Parallel   | .NET 8.0 | 16    |   573.3 μs |  52.10 μs | 152.81 μs |   548.3 μs | 1.29x faster |   0.42x | 0.9766 |  17.33 KB |  1.04x less |
|            |          |       |            |           |           |            |              |         |        |           |             |
| **Sequential** | **.NET 6.0** | **64**    | **2,089.9 μs** |  **73.03 μs** | **188.50 μs** | **2,143.1 μs** |     **baseline** |        **** | **5.8594** |  **79.68 KB** |            **** |
| Sequential | .NET 8.0 | 64    | 2,005.3 μs |  14.96 μs |  13.26 μs | 2,005.4 μs | 1.05x slower |   0.03x | 3.9063 |  78.67 KB |  1.01x less |
|            |          |       |            |           |           |            |              |         |        |           |             |
| Parallel   | .NET 6.0 | 64    | 2,077.2 μs | 147.08 μs | 426.70 μs | 2,023.2 μs |     baseline |         | 3.9063 |  69.56 KB |             |
| Parallel   | .NET 8.0 | 64    | 1,155.8 μs |  62.79 μs | 181.16 μs | 1,156.4 μs | 1.85x faster |   0.54x | 3.9063 |  67.42 KB |  1.03x less |
