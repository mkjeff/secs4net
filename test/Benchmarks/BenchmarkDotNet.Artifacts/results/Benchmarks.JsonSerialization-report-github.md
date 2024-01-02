```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-ZQSTYQ : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-LEHGCO : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method     | Runtime  | ItemCount | Mean       | Error     | StdDev    | Ratio        | RatioSD | Allocated | Alloc Ratio |
|----------- |--------- |---------- |-----------:|----------:|----------:|-------------:|--------:|----------:|------------:|
| **Serialize**  | **.NET 6.0** | **0**         |   **1.902 μs** | **0.0133 μs** | **0.0118 μs** |     **baseline** |        **** |   **1.02 KB** |            **** |
| Serialize  | .NET 8.0 | 0         |   1.315 μs | 0.0087 μs | 0.0077 μs | 1.45x faster |   0.01x |   1.02 KB |  1.00x more |
|            |          |           |            |           |           |              |         |           |             |
| Deserialze | .NET 6.0 | 0         |  13.916 μs | 0.0833 μs | 0.0779 μs |     baseline |         |   9.55 KB |             |
| Deserialze | .NET 8.0 | 0         |   7.996 μs | 0.0340 μs | 0.0318 μs | 1.74x faster |   0.01x |   9.55 KB |  1.00x more |
|            |          |           |            |           |           |              |         |           |             |
| **Serialize**  | **.NET 6.0** | **64**        |  **27.933 μs** | **0.1516 μs** | **0.1418 μs** |     **baseline** |        **** |  **15.05 KB** |            **** |
| Serialize  | .NET 8.0 | 64        |  22.478 μs | 0.0851 μs | 0.0796 μs | 1.24x faster |   0.01x |  15.05 KB |  1.00x more |
|            |          |           |            |           |           |              |         |           |             |
| Deserialze | .NET 6.0 | 64        | 136.572 μs | 0.7437 μs | 0.6592 μs |     baseline |         |  42.23 KB |             |
| Deserialze | .NET 8.0 | 64        |  87.711 μs | 0.1836 μs | 0.1628 μs | 1.56x faster |   0.01x |  42.23 KB |  1.00x less |
|            |          |           |            |           |           |              |         |           |             |
| **Serialize**  | **.NET 6.0** | **128**       |  **51.931 μs** | **0.3025 μs** | **0.2829 μs** |     **baseline** |        **** |   **28.3 KB** |            **** |
| Serialize  | .NET 8.0 | 128       |  42.367 μs | 0.2430 μs | 0.2273 μs | 1.23x faster |   0.01x |   28.3 KB |  1.00x more |
|            |          |           |            |           |           |              |         |           |             |
| Deserialze | .NET 6.0 | 128       | 244.020 μs | 1.2472 μs | 1.0415 μs |     baseline |         |  74.61 KB |             |
| Deserialze | .NET 8.0 | 128       | 185.094 μs | 0.1490 μs | 0.1321 μs | 1.32x faster |   0.01x |  74.61 KB |  1.00x less |
