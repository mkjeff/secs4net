```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-OOEVDF : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-AKZPHK : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method | Runtime  | ItemCount | Mean       | Error    | StdDev   | Ratio        | RatioSD | Allocated | Alloc Ratio |
|------- |--------- |---------- |-----------:|---------:|---------:|-------------:|--------:|----------:|------------:|
| **Encode** | **.NET 6.0** | **0**         |   **199.4 ns** |  **1.67 ns** |  **1.48 ns** |     **baseline** |        **** |      **40 B** |            **** |
| Encode | .NET 8.0 | 0         |   122.7 ns |  1.67 ns |  1.56 ns | 1.63x faster |   0.03x |      40 B |  1.00x more |
|        |          |           |            |          |          |              |         |           |             |
| Decode | .NET 6.0 | 0         | 1,338.1 ns | 20.43 ns | 19.11 ns |     baseline |         |     560 B |             |
| Decode | .NET 8.0 | 0         | 1,205.0 ns | 15.76 ns | 14.74 ns | 1.11x faster |   0.02x |     560 B |  1.00x more |
|        |          |           |            |          |          |              |         |           |             |
| **Encode** | **.NET 6.0** | **64**        |   **745.2 ns** |  **9.73 ns** |  **9.10 ns** |     **baseline** |        **** |      **40 B** |            **** |
| Encode | .NET 8.0 | 64        |   636.7 ns |  5.55 ns |  4.33 ns | 1.17x faster |   0.02x |      40 B |  1.00x more |
|        |          |           |            |          |          |              |         |           |             |
| Decode | .NET 6.0 | 64        | 2,721.1 ns | 51.64 ns | 57.40 ns |     baseline |         |    7248 B |             |
| Decode | .NET 8.0 | 64        | 2,445.7 ns | 48.33 ns | 47.47 ns | 1.11x faster |   0.02x |    7248 B |  1.00x more |
|        |          |           |            |          |          |              |         |           |             |
| **Encode** | **.NET 6.0** | **128**       | **1,129.3 ns** | **19.60 ns** | **18.34 ns** |     **baseline** |        **** |      **40 B** |            **** |
| Encode | .NET 8.0 | 128       |   963.3 ns | 18.02 ns | 16.85 ns | 1.17x faster |   0.03x |      40 B |  1.00x more |
|        |          |           |            |          |          |              |         |           |             |
| Decode | .NET 6.0 | 128       | 3,270.3 ns | 27.20 ns | 25.44 ns |     baseline |         |    9112 B |             |
| Decode | .NET 8.0 | 128       | 3,038.6 ns | 33.79 ns | 26.38 ns | 1.08x faster |   0.01x |    9112 B |  1.00x more |
