```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2792/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-VAUECX : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-NTIFBX : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method     | Runtime  | Count | Mean       | Error     | StdDev    | Median     | Ratio        | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------- |--------- |------ |-----------:|----------:|----------:|-----------:|-------------:|--------:|-------:|----------:|------------:|
| **Sequential** | **.NET 6.0** | **16**    |   **625.9 μs** |  **12.30 μs** |  **21.85 μs** |   **623.9 μs** |     **baseline** |        **** | **0.9766** |  **20.05 KB** |            **** |
| Sequential | .NET 8.0 | 16    |   601.0 μs |  11.85 μs |  18.80 μs |   601.3 μs | 1.04x faster |   0.05x | 0.9766 |  19.79 KB |  1.01x less |
|            |          |       |            |           |           |            |              |         |        |           |             |
| Parallel   | .NET 6.0 | 16    | 1,629.9 μs | 110.11 μs | 317.69 μs | 1,592.4 μs |     baseline |         |      - |  17.88 KB |             |
| Parallel   | .NET 8.0 | 16    |   729.7 μs |  73.68 μs | 216.10 μs |   676.9 μs | 2.43x faster |   0.83x | 0.9766 |   17.7 KB |  1.01x less |
|            |          |       |            |           |           |            |              |         |        |           |             |
| **Sequential** | **.NET 6.0** | **64**    | **2,253.1 μs** |  **60.36 μs** | **177.02 μs** | **2,178.9 μs** |     **baseline** |        **** | **3.9063** |  **79.68 KB** |            **** |
| Sequential | .NET 8.0 | 64    | 2,374.6 μs |  46.61 μs |  87.54 μs | 2,358.6 μs | 1.01x faster |   0.08x | 3.9063 |  78.67 KB |  1.01x less |
|            |          |       |            |           |           |            |              |         |        |           |             |
| Parallel   | .NET 6.0 | 64    | 2,770.0 μs | 269.30 μs | 785.56 μs | 2,618.1 μs |     baseline |         | 3.9063 |  69.33 KB |             |
| Parallel   | .NET 8.0 | 64    | 1,483.5 μs | 173.39 μs | 503.03 μs | 1,520.5 μs | 2.12x faster |   1.03x | 3.9063 |  68.28 KB |  1.02x less |
