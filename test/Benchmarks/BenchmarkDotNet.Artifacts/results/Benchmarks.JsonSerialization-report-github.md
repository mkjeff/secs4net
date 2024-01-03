```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-LLCSMO : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-LSZLMA : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method     | Runtime  | ItemCount | Mean       | Error     | StdDev    | Ratio        | RatioSD | Allocated | Alloc Ratio |
|----------- |--------- |---------- |-----------:|----------:|----------:|-------------:|--------:|----------:|------------:|
| **Serialize**  | **.NET 6.0** | **0**         |   **2.055 μs** | **0.0405 μs** | **0.0466 μs** |     **baseline** |        **** |   **1.02 KB** |            **** |
| Serialize  | .NET 8.0 | 0         |   1.400 μs | 0.0179 μs | 0.0158 μs | 1.47x faster |   0.05x |   1.02 KB |  1.00x more |
|            |          |           |            |           |           |              |         |           |             |
| Deserialze | .NET 6.0 | 0         |  15.064 μs | 0.2881 μs | 0.3318 μs |     baseline |         |   9.55 KB |             |
| Deserialze | .NET 8.0 | 0         |   8.713 μs | 0.1685 μs | 0.1655 μs | 1.73x faster |   0.04x |   9.55 KB |  1.00x more |
|            |          |           |            |           |           |              |         |           |             |
| **Serialize**  | **.NET 6.0** | **64**        |  **29.649 μs** | **0.5179 μs** | **0.4844 μs** |     **baseline** |        **** |  **15.05 KB** |            **** |
| Serialize  | .NET 8.0 | 64        |  23.805 μs | 0.3196 μs | 0.2833 μs | 1.25x faster |   0.02x |  15.05 KB |  1.00x more |
|            |          |           |            |           |           |              |         |           |             |
| Deserialze | .NET 6.0 | 64        | 140.490 μs | 2.0292 μs | 1.8981 μs |     baseline |         |  42.23 KB |             |
| Deserialze | .NET 8.0 | 64        |  92.651 μs | 1.8082 μs | 1.8569 μs | 1.52x faster |   0.04x |  42.23 KB |  1.00x less |
|            |          |           |            |           |           |              |         |           |             |
| **Serialize**  | **.NET 6.0** | **128**       |  **54.629 μs** | **0.4846 μs** | **0.4296 μs** |     **baseline** |        **** |   **28.3 KB** |            **** |
| Serialize  | .NET 8.0 | 128       |  44.967 μs | 0.8929 μs | 0.8769 μs | 1.22x faster |   0.03x |   28.3 KB |  1.00x more |
|            |          |           |            |           |           |              |         |           |             |
| Deserialze | .NET 6.0 | 128       | 269.203 μs | 3.8650 μs | 3.6153 μs |     baseline |         |  74.61 KB |             |
| Deserialze | .NET 8.0 | 128       | 173.503 μs | 1.6810 μs | 1.4037 μs | 1.55x faster |   0.01x |  74.61 KB |  1.00x less |
