```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-LLCSMO : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-LSZLMA : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method | Runtime  | ItemCount | Mean       | Error    | StdDev   | Ratio        | RatioSD | Allocated | Alloc Ratio |
|------- |--------- |---------- |-----------:|---------:|---------:|-------------:|--------:|----------:|------------:|
| **Encode** | **.NET 6.0** | **0**         |   **198.8 ns** |  **1.90 ns** |  **1.69 ns** |     **baseline** |        **** |      **40 B** |            **** |
| Encode | .NET 8.0 | 0         |   122.2 ns |  2.41 ns |  2.13 ns | 1.63x faster |   0.03x |      40 B |  1.00x more |
|        |          |           |            |          |          |              |         |           |             |
| Decode | .NET 6.0 | 0         | 1,411.0 ns | 19.15 ns | 16.97 ns |     baseline |         |     560 B |             |
| Decode | .NET 8.0 | 0         | 1,269.6 ns | 21.72 ns | 20.32 ns | 1.11x faster |   0.02x |     560 B |  1.00x more |
|        |          |           |            |          |          |              |         |           |             |
| **Encode** | **.NET 6.0** | **64**        |   **731.4 ns** |  **8.99 ns** |  **7.97 ns** |     **baseline** |        **** |      **40 B** |            **** |
| Encode | .NET 8.0 | 64        |   559.5 ns | 10.67 ns | 11.42 ns | 1.30x faster |   0.03x |      40 B |  1.00x more |
|        |          |           |            |          |          |              |         |           |             |
| Decode | .NET 6.0 | 64        | 2,685.3 ns | 41.63 ns | 36.90 ns |     baseline |         |    7248 B |             |
| Decode | .NET 8.0 | 64        | 2,499.6 ns | 36.32 ns | 32.20 ns | 1.07x faster |   0.02x |    7248 B |  1.00x more |
|        |          |           |            |          |          |              |         |           |             |
| **Encode** | **.NET 6.0** | **128**       | **1,127.0 ns** | **19.01 ns** | **17.78 ns** |     **baseline** |        **** |      **40 B** |            **** |
| Encode | .NET 8.0 | 128       |   956.6 ns | 12.05 ns |  9.41 ns | 1.17x faster |   0.02x |      40 B |  1.00x more |
|        |          |           |            |          |          |              |         |           |             |
| Decode | .NET 6.0 | 128       | 3,297.3 ns | 41.69 ns | 36.96 ns |     baseline |         |    9112 B |             |
| Decode | .NET 8.0 | 128       | 3,107.9 ns | 44.40 ns | 39.36 ns | 1.06x faster |   0.01x |    9112 B |  1.00x more |
