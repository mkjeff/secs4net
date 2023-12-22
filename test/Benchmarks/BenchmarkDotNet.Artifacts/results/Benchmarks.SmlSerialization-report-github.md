```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2792/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-VAUECX : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-NTIFBX : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method     | Runtime  | ItemCount | Mean      | Error     | StdDev    | Ratio        | RatioSD | Allocated | Alloc Ratio |
|----------- |--------- |---------- |----------:|----------:|----------:|-------------:|--------:|----------:|------------:|
| **Serialize**  | **.NET 6.0** | **0**         |  **4.032 μs** | **0.0514 μs** | **0.0480 μs** |     **baseline** |        **** |   **9.78 KB** |            **** |
| Serialize  | .NET 8.0 | 0         |  3.126 μs | 0.0499 μs | 0.0467 μs | 1.29x faster |   0.02x |   9.78 KB |  1.00x more |
|            |          |           |           |           |           |              |         |           |             |
| Deserialze | .NET 6.0 | 0         |  3.680 μs | 0.0485 μs | 0.0454 μs |     baseline |         |   5.26 KB |             |
| Deserialze | .NET 8.0 | 0         |  2.774 μs | 0.0546 μs | 0.0629 μs | 1.32x faster |   0.03x |   5.26 KB |  1.00x more |
|            |          |           |           |           |           |              |         |           |             |
| **Serialize**  | **.NET 6.0** | **64**        | **37.380 μs** | **0.7411 μs** | **0.6932 μs** |     **baseline** |        **** |  **24.87 KB** |            **** |
| Serialize  | .NET 8.0 | 64        | 29.852 μs | 0.3930 μs | 0.3676 μs | 1.25x faster |   0.03x |  30.87 KB |  1.24x more |
|            |          |           |           |           |           |              |         |           |             |
| Deserialze | .NET 6.0 | 64        | 35.893 μs | 0.4430 μs | 0.3458 μs |     baseline |         |  23.77 KB |             |
| Deserialze | .NET 8.0 | 64        | 25.069 μs | 0.3599 μs | 0.3366 μs | 1.43x faster |   0.02x |  23.77 KB |  1.00x more |
|            |          |           |           |           |           |              |         |           |             |
| **Serialize**  | **.NET 6.0** | **128**       | **69.330 μs** | **1.1277 μs** | **0.9997 μs** |     **baseline** |        **** |  **40.71 KB** |            **** |
| Serialize  | .NET 8.0 | 128       | 53.666 μs | 0.8397 μs | 0.7854 μs | 1.29x faster |   0.03x |  52.71 KB |  1.29x more |
|            |          |           |           |           |           |              |         |           |             |
| Deserialze | .NET 6.0 | 128       | 66.644 μs | 1.2369 μs | 1.0965 μs |     baseline |         |  40.42 KB |             |
| Deserialze | .NET 8.0 | 128       | 45.227 μs | 0.5870 μs | 0.5491 μs | 1.47x faster |   0.03x |  40.42 KB |  1.00x more |
