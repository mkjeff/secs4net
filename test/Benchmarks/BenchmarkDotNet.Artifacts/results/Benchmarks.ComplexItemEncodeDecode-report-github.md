```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-ZQSTYQ : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-LEHGCO : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method | Runtime  | ItemCount | Mean       | Error    | StdDev   | Ratio        | RatioSD | Allocated | Alloc Ratio |
|------- |--------- |---------- |-----------:|---------:|---------:|-------------:|--------:|----------:|------------:|
| **Encode** | **.NET 6.0** | **0**         |   **170.0 ns** |  **1.41 ns** |  **1.32 ns** |     **baseline** |        **** |      **40 B** |            **** |
| Encode | .NET 8.0 | 0         |   119.0 ns |  0.81 ns |  0.76 ns | 1.43x faster |   0.01x |      40 B |  1.00x more |
|        |          |           |            |          |          |              |         |           |             |
| Decode | .NET 6.0 | 0         | 1,454.1 ns |  5.72 ns |  5.07 ns |     baseline |         |    1736 B |             |
| Decode | .NET 8.0 | 0         | 1,222.6 ns |  7.04 ns |  6.24 ns | 1.19x faster |   0.01x |    1736 B |  1.00x more |
|        |          |           |            |          |          |              |         |           |             |
| **Encode** | **.NET 6.0** | **64**        |   **696.5 ns** |  **3.40 ns** |  **3.18 ns** |     **baseline** |        **** |      **40 B** |            **** |
| Encode | .NET 8.0 | 64        |   591.7 ns |  5.49 ns |  5.13 ns | 1.18x faster |   0.01x |      40 B |  1.00x more |
|        |          |           |            |          |          |              |         |           |             |
| Decode | .NET 6.0 | 64        | 2,560.5 ns | 11.05 ns | 10.34 ns |     baseline |         |    7248 B |             |
| Decode | .NET 8.0 | 64        | 2,309.8 ns |  8.78 ns |  7.78 ns | 1.11x faster |   0.01x |    7248 B |  1.00x more |
|        |          |           |            |          |          |              |         |           |             |
| **Encode** | **.NET 6.0** | **128**       | **1,086.3 ns** |  **5.70 ns** |  **5.34 ns** |     **baseline** |        **** |      **40 B** |            **** |
| Encode | .NET 8.0 | 128       |   919.0 ns |  5.88 ns |  4.91 ns | 1.18x faster |   0.01x |      40 B |  1.00x more |
|        |          |           |            |          |          |              |         |           |             |
| Decode | .NET 6.0 | 128       | 3,080.1 ns |  7.82 ns |  6.93 ns |     baseline |         |    9112 B |             |
| Decode | .NET 8.0 | 128       | 2,786.5 ns | 15.24 ns | 14.25 ns | 1.11x faster |   0.01x |    9112 B |  1.00x more |
