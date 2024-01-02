```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-ZQSTYQ : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-LEHGCO : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method     | Runtime  | ItemCount | Mean      | Error     | StdDev    | Ratio        | RatioSD | Allocated | Alloc Ratio |
|----------- |--------- |---------- |----------:|----------:|----------:|-------------:|--------:|----------:|------------:|
| **Serialize**  | **.NET 6.0** | **0**         |  **3.861 μs** | **0.0254 μs** | **0.0238 μs** |     **baseline** |        **** |   **9.78 KB** |            **** |
| Serialize  | .NET 8.0 | 0         |  3.051 μs | 0.0302 μs | 0.0282 μs | 1.27x faster |   0.01x |   9.78 KB |  1.00x more |
|            |          |           |           |           |           |              |         |           |             |
| Deserialze | .NET 6.0 | 0         |  3.416 μs | 0.0127 μs | 0.0119 μs |     baseline |         |   5.26 KB |             |
| Deserialze | .NET 8.0 | 0         |  2.582 μs | 0.0106 μs | 0.0094 μs | 1.32x faster |   0.01x |   5.26 KB |  1.00x more |
|            |          |           |           |           |           |              |         |           |             |
| **Serialize**  | **.NET 6.0** | **64**        | **37.488 μs** | **0.1566 μs** | **0.1465 μs** |     **baseline** |        **** |  **24.87 KB** |            **** |
| Serialize  | .NET 8.0 | 64        | 28.365 μs | 0.0874 μs | 0.0730 μs | 1.32x faster |   0.00x |  30.87 KB |  1.24x more |
|            |          |           |           |           |           |              |         |           |             |
| Deserialze | .NET 6.0 | 64        | 34.195 μs | 0.0630 μs | 0.0559 μs |     baseline |         |  23.77 KB |             |
| Deserialze | .NET 8.0 | 64        | 24.047 μs | 0.1030 μs | 0.0913 μs | 1.42x faster |   0.01x |  23.77 KB |  1.00x more |
|            |          |           |           |           |           |              |         |           |             |
| **Serialize**  | **.NET 6.0** | **128**       | **70.302 μs** | **0.1595 μs** | **0.1492 μs** |     **baseline** |        **** |  **40.71 KB** |            **** |
| Serialize  | .NET 8.0 | 128       | 50.821 μs | 0.1879 μs | 0.1665 μs | 1.38x faster |   0.00x |  52.71 KB |  1.29x more |
|            |          |           |           |           |           |              |         |           |             |
| Deserialze | .NET 6.0 | 128       | 63.249 μs | 0.3108 μs | 0.2595 μs |     baseline |         |  40.42 KB |             |
| Deserialze | .NET 8.0 | 128       | 42.524 μs | 0.1631 μs | 0.1446 μs | 1.49x faster |   0.01x |  40.42 KB |  1.00x more |
