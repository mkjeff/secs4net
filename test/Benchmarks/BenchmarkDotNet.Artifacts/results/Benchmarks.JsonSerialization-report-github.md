```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2792/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-VAUECX : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-NTIFBX : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method     | Runtime  | ItemCount | Mean       | Error     | StdDev    | Ratio        | RatioSD | Allocated | Alloc Ratio |
|----------- |--------- |---------- |-----------:|----------:|----------:|-------------:|--------:|----------:|------------:|
| **Serialize**  | **.NET 6.0** | **0**         |   **2.023 μs** | **0.0262 μs** | **0.0245 μs** |     **baseline** |        **** |   **1.02 KB** |            **** |
| Serialize  | .NET 8.0 | 0         |   1.383 μs | 0.0167 μs | 0.0156 μs | 1.46x faster |   0.02x |   1.02 KB |  1.00x more |
|            |          |           |            |           |           |              |         |           |             |
| Deserialze | .NET 6.0 | 0         |  15.146 μs | 0.1350 μs | 0.1263 μs |     baseline |         |   9.55 KB |             |
| Deserialze | .NET 8.0 | 0         |   8.656 μs | 0.1206 μs | 0.1128 μs | 1.75x faster |   0.02x |   9.55 KB |  1.00x more |
|            |          |           |            |           |           |              |         |           |             |
| **Serialize**  | **.NET 6.0** | **64**        |  **29.343 μs** | **0.2725 μs** | **0.2549 μs** |     **baseline** |        **** |  **15.05 KB** |            **** |
| Serialize  | .NET 8.0 | 64        |  23.762 μs | 0.2354 μs | 0.2202 μs | 1.23x faster |   0.01x |  15.05 KB |  1.00x more |
|            |          |           |            |           |           |              |         |           |             |
| Deserialze | .NET 6.0 | 64        | 142.363 μs | 1.0520 μs | 0.9840 μs |     baseline |         |  42.23 KB |             |
| Deserialze | .NET 8.0 | 64        |  92.122 μs | 0.6939 μs | 0.5794 μs | 1.54x faster |   0.02x |  42.23 KB |  1.00x less |
|            |          |           |            |           |           |              |         |           |             |
| **Serialize**  | **.NET 6.0** | **128**       |  **57.030 μs** | **0.5233 μs** | **0.4895 μs** |     **baseline** |        **** |   **28.3 KB** |            **** |
| Serialize  | .NET 8.0 | 128       |  44.687 μs | 0.6176 μs | 0.5777 μs | 1.28x faster |   0.02x |   28.3 KB |  1.00x more |
|            |          |           |            |           |           |              |         |           |             |
| Deserialze | .NET 6.0 | 128       | 266.524 μs | 3.0243 μs | 2.6810 μs |     baseline |         |  74.61 KB |             |
| Deserialze | .NET 8.0 | 128       | 170.501 μs | 1.8329 μs | 1.5305 μs | 1.56x faster |   0.02x |  74.61 KB |  1.00x less |
