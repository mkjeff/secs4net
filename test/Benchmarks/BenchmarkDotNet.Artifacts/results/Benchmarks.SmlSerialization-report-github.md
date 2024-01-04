```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-OOEVDF : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-AKZPHK : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method     | Runtime  | ItemCount | Mean      | Error     | StdDev    | Ratio        | RatioSD | Allocated | Alloc Ratio |
|----------- |--------- |---------- |----------:|----------:|----------:|-------------:|--------:|----------:|------------:|
| **Serialize**  | **.NET 6.0** | **0**         |  **4.028 μs** | **0.0803 μs** | **0.0751 μs** |     **baseline** |        **** |   **9.78 KB** |            **** |
| Serialize  | .NET 8.0 | 0         |  3.172 μs | 0.0345 μs | 0.0288 μs | 1.27x faster |   0.02x |   9.78 KB |  1.00x more |
|            |          |           |           |           |           |              |         |           |             |
| Deserialze | .NET 6.0 | 0         |  3.719 μs | 0.0519 μs | 0.0460 μs |     baseline |         |   5.26 KB |             |
| Deserialze | .NET 8.0 | 0         |  2.765 μs | 0.0362 μs | 0.0303 μs | 1.35x faster |   0.02x |   5.26 KB |  1.00x more |
|            |          |           |           |           |           |              |         |           |             |
| **Serialize**  | **.NET 6.0** | **64**        | **37.981 μs** | **0.7462 μs** | **0.7984 μs** |     **baseline** |        **** |  **24.87 KB** |            **** |
| Serialize  | .NET 8.0 | 64        | 29.758 μs | 0.2525 μs | 0.2109 μs | 1.28x faster |   0.03x |  30.87 KB |  1.24x more |
|            |          |           |           |           |           |              |         |           |             |
| Deserialze | .NET 6.0 | 64        | 36.839 μs | 0.7058 μs | 0.7248 μs |     baseline |         |  23.77 KB |             |
| Deserialze | .NET 8.0 | 64        | 24.806 μs | 0.3647 μs | 0.3233 μs | 1.48x faster |   0.04x |  23.77 KB |  1.00x more |
|            |          |           |           |           |           |              |         |           |             |
| **Serialize**  | **.NET 6.0** | **128**       | **69.884 μs** | **1.2330 μs** | **1.1533 μs** |     **baseline** |        **** |  **40.71 KB** |            **** |
| Serialize  | .NET 8.0 | 128       | 53.641 μs | 0.6031 μs | 0.5641 μs | 1.30x faster |   0.03x |  52.71 KB |  1.29x more |
|            |          |           |           |           |           |              |         |           |             |
| Deserialze | .NET 6.0 | 128       | 67.628 μs | 0.6775 μs | 0.5658 μs |     baseline |         |  40.42 KB |             |
| Deserialze | .NET 8.0 | 128       | 47.127 μs | 0.8006 μs | 0.7097 μs | 1.44x faster |   0.03x |  40.42 KB |  1.00x more |
