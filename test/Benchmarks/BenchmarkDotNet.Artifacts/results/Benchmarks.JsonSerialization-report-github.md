```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-OOEVDF : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-AKZPHK : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method     | Runtime  | ItemCount | Mean       | Error     | StdDev    | Ratio        | RatioSD | Allocated | Alloc Ratio |
|----------- |--------- |---------- |-----------:|----------:|----------:|-------------:|--------:|----------:|------------:|
| **Serialize**  | **.NET 6.0** | **0**         |   **2.083 μs** | **0.0415 μs** | **0.0525 μs** |     **baseline** |        **** |   **1.02 KB** |            **** |
| Serialize  | .NET 8.0 | 0         |   1.407 μs | 0.0265 μs | 0.0235 μs | 1.47x faster |   0.05x |   1.02 KB |  1.00x more |
|            |          |           |            |           |           |              |         |           |             |
| Deserialze | .NET 6.0 | 0         |  14.858 μs | 0.1986 μs | 0.1760 μs |     baseline |         |   9.55 KB |             |
| Deserialze | .NET 8.0 | 0         |   8.766 μs | 0.1723 μs | 0.1611 μs | 1.70x faster |   0.03x |   9.55 KB |  1.00x more |
|            |          |           |            |           |           |              |         |           |             |
| **Serialize**  | **.NET 6.0** | **64**        |  **30.238 μs** | **0.5057 μs** | **0.4483 μs** |     **baseline** |        **** |  **15.05 KB** |            **** |
| Serialize  | .NET 8.0 | 64        |  24.168 μs | 0.4302 μs | 0.4024 μs | 1.25x faster |   0.02x |  15.05 KB |  1.00x more |
|            |          |           |            |           |           |              |         |           |             |
| Deserialze | .NET 6.0 | 64        | 145.236 μs | 2.6199 μs | 2.4507 μs |     baseline |         |  42.23 KB |             |
| Deserialze | .NET 8.0 | 64        |  92.474 μs | 1.7891 μs | 1.8373 μs | 1.57x faster |   0.04x |  42.23 KB |  1.00x less |
|            |          |           |            |           |           |              |         |           |             |
| **Serialize**  | **.NET 6.0** | **128**       |  **55.047 μs** | **0.8278 μs** | **0.7744 μs** |     **baseline** |        **** |   **28.3 KB** |            **** |
| Serialize  | .NET 8.0 | 128       |  45.690 μs | 0.8181 μs | 0.7252 μs | 1.21x faster |   0.03x |   28.3 KB |  1.00x more |
|            |          |           |            |           |           |              |         |           |             |
| Deserialze | .NET 6.0 | 128       | 269.452 μs | 5.3342 μs | 5.4778 μs |     baseline |         |  74.61 KB |             |
| Deserialze | .NET 8.0 | 128       | 194.519 μs | 2.2089 μs | 1.8445 μs | 1.39x faster |   0.03x |  74.61 KB |  1.00x less |
