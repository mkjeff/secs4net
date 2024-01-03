```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-LLCSMO : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-LSZLMA : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method     | Runtime  | ItemCount | Mean      | Error     | StdDev    | Ratio        | RatioSD | Allocated | Alloc Ratio |
|----------- |--------- |---------- |----------:|----------:|----------:|-------------:|--------:|----------:|------------:|
| **Serialize**  | **.NET 6.0** | **0**         |  **4.064 μs** | **0.0540 μs** | **0.0422 μs** |     **baseline** |        **** |   **9.78 KB** |            **** |
| Serialize  | .NET 8.0 | 0         |  3.215 μs | 0.0614 μs | 0.0603 μs | 1.27x faster |   0.03x |   9.78 KB |  1.00x more |
|            |          |           |           |           |           |              |         |           |             |
| Deserialze | .NET 6.0 | 0         |  3.633 μs | 0.0415 μs | 0.0368 μs |     baseline |         |   5.26 KB |             |
| Deserialze | .NET 8.0 | 0         |  2.929 μs | 0.0579 μs | 0.0690 μs | 1.24x faster |   0.04x |   5.26 KB |  1.00x more |
|            |          |           |           |           |           |              |         |           |             |
| **Serialize**  | **.NET 6.0** | **64**        | **38.139 μs** | **0.6314 μs** | **0.5907 μs** |     **baseline** |        **** |  **24.87 KB** |            **** |
| Serialize  | .NET 8.0 | 64        | 29.500 μs | 0.5683 μs | 0.5316 μs | 1.29x faster |   0.03x |  30.87 KB |  1.24x more |
|            |          |           |           |           |           |              |         |           |             |
| Deserialze | .NET 6.0 | 64        | 36.019 μs | 0.7000 μs | 0.6547 μs |     baseline |         |  23.77 KB |             |
| Deserialze | .NET 8.0 | 64        | 25.311 μs | 0.3032 μs | 0.2836 μs | 1.42x faster |   0.03x |  23.77 KB |  1.00x more |
|            |          |           |           |           |           |              |         |           |             |
| **Serialize**  | **.NET 6.0** | **128**       | **68.782 μs** | **1.2549 μs** | **1.1125 μs** |     **baseline** |        **** |  **40.71 KB** |            **** |
| Serialize  | .NET 8.0 | 128       | 55.003 μs | 0.7716 μs | 0.7217 μs | 1.25x faster |   0.03x |  52.71 KB |  1.29x more |
|            |          |           |           |           |           |              |         |           |             |
| Deserialze | .NET 6.0 | 128       | 68.600 μs | 1.1580 μs | 1.0832 μs |     baseline |         |  40.42 KB |             |
| Deserialze | .NET 8.0 | 128       | 45.136 μs | 0.3481 μs | 0.2907 μs | 1.52x faster |   0.03x |  40.42 KB |  1.00x more |
