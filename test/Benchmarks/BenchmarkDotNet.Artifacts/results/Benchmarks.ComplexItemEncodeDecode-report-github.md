```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2792/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-VAUECX : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-NTIFBX : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method | Runtime  | ItemCount | Mean       | Error    | StdDev   | Ratio        | RatioSD | Allocated | Alloc Ratio |
|------- |--------- |---------- |-----------:|---------:|---------:|-------------:|--------:|----------:|------------:|
| **Encode** | **.NET 6.0** | **0**         |   **172.4 ns** |  **1.88 ns** |  **1.67 ns** |     **baseline** |        **** |      **40 B** |            **** |
| Encode | .NET 8.0 | 0         |   120.1 ns |  1.71 ns |  1.60 ns | 1.44x faster |   0.02x |      40 B |  1.00x more |
|        |          |           |            |          |          |              |         |           |             |
| Decode | .NET 6.0 | 0         | 1,612.0 ns | 17.13 ns | 16.02 ns |     baseline |         |     560 B |             |
| Decode | .NET 8.0 | 0         | 1,448.9 ns | 12.36 ns | 11.56 ns | 1.11x faster |   0.01x |     560 B |  1.00x more |
|        |          |           |            |          |          |              |         |           |             |
| **Encode** | **.NET 6.0** | **64**        |   **712.1 ns** |  **6.56 ns** |  **5.48 ns** |     **baseline** |        **** |      **40 B** |            **** |
| Encode | .NET 8.0 | 64        |   581.3 ns |  5.40 ns |  4.78 ns | 1.22x faster |   0.01x |      40 B |  1.00x more |
|        |          |           |            |          |          |              |         |           |             |
| Decode | .NET 6.0 | 64        | 2,829.6 ns | 23.53 ns | 19.65 ns |     baseline |         |    7248 B |             |
| Decode | .NET 8.0 | 64        | 2,543.6 ns | 35.77 ns | 33.46 ns | 1.11x faster |   0.02x |    7248 B |  1.00x more |
|        |          |           |            |          |          |              |         |           |             |
| **Encode** | **.NET 6.0** | **128**       | **1,114.3 ns** |  **9.36 ns** |  **8.29 ns** |     **baseline** |        **** |      **40 B** |            **** |
| Encode | .NET 8.0 | 128       |   904.1 ns |  5.28 ns |  4.68 ns | 1.23x faster |   0.01x |      40 B |  1.00x more |
|        |          |           |            |          |          |              |         |           |             |
| Decode | .NET 6.0 | 128       | 3,422.5 ns | 35.69 ns | 29.80 ns |     baseline |         |    9112 B |             |
| Decode | .NET 8.0 | 128       | 3,186.4 ns | 45.94 ns | 42.97 ns | 1.08x faster |   0.02x |    9112 B |  1.00x more |
