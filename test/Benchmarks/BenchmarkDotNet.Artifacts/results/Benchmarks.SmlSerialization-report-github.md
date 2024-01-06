```

BenchmarkDotNet v0.13.11, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-ZWGTBM : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-ZQLZSO : .NET Framework 4.8.1 (4.8.9181.0), X64 RyuJIT VectorSize=256


```
| Method     | Runtime            | ItemCount | Mean       | Error     | StdDev    | Ratio        | RatioSD | Allocated | Alloc Ratio |
|----------- |------------------- |---------- |-----------:|----------:|----------:|-------------:|--------:|----------:|------------:|
| **Serialize**  | **.NET 8.0**           | **0**         |   **3.062 μs** | **0.0181 μs** | **0.0170 μs** | **2.72x faster** |   **0.02x** |   **9.78 KB** |  **1.19x less** |
| Serialize  | .NET Framework 4.8 | 0         |   8.323 μs | 0.0596 μs | 0.0528 μs |     baseline |         |  11.62 KB |             |
|            |                    |           |            |           |           |              |         |           |             |
| Deserialze | .NET 8.0           | 0         |   2.573 μs | 0.0129 μs | 0.0121 μs | 3.93x faster |   0.02x |   5.26 KB |  2.05x less |
| Deserialze | .NET Framework 4.8 | 0         |  10.113 μs | 0.0554 μs | 0.0491 μs |     baseline |         |  10.76 KB |             |
|            |                    |           |            |           |           |              |         |           |             |
| **Serialize**  | **.NET 8.0**           | **64**        |  **27.878 μs** | **0.1066 μs** | **0.0997 μs** | **2.80x faster** |   **0.01x** |  **30.87 KB** |  **2.10x less** |
| Serialize  | .NET Framework 4.8 | 64        |  78.103 μs | 0.2760 μs | 0.2447 μs |     baseline |         |  64.91 KB |             |
|            |                    |           |            |           |           |              |         |           |             |
| Deserialze | .NET 8.0           | 64        |  23.901 μs | 0.1722 μs | 0.1611 μs | 5.99x faster |   0.05x |  23.77 KB |  7.07x less |
| Deserialze | .NET Framework 4.8 | 64        | 143.181 μs | 0.7637 μs | 0.7144 μs |     baseline |         | 168.05 KB |             |
|            |                    |           |            |           |           |              |         |           |             |
| **Serialize**  | **.NET 8.0**           | **128**       |  **50.976 μs** | **0.2377 μs** | **0.2107 μs** | **2.79x faster** |   **0.02x** |  **52.71 KB** |  **2.25x less** |
| Serialize  | .NET Framework 4.8 | 128       | 142.509 μs | 0.8476 μs | 0.7928 μs |     baseline |         | 118.74 KB |             |
|            |                    |           |            |           |           |              |         |           |             |
| Deserialze | .NET 8.0           | 128       |  42.961 μs | 0.3362 μs | 0.3145 μs | 6.36x faster |   0.05x |  40.42 KB |  7.97x less |
| Deserialze | .NET Framework 4.8 | 128       | 273.225 μs | 1.1358 μs | 1.0625 μs |     baseline |         | 322.05 KB |             |
