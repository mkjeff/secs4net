```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3007/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  Job-ISCVXC : .NET 6.0.26 (6.0.2623.60508), X64 RyuJIT AVX2
  Job-DINVIM : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  Job-OKZTLT : .NET Framework 4.8.1 (4.8.9181.0), X64 RyuJIT VectorSize=256


```
| Method     | Runtime            | ItemCount | Mean       | Error      | StdDev     | Median     | Ratio        | RatioSD | Allocated | Alloc Ratio |
|----------- |------------------- |---------- |-----------:|-----------:|-----------:|-----------:|-------------:|--------:|----------:|------------:|
| **Serialize**  | **.NET 6.0**           | **0**         |   **4.273 μs** |  **0.0847 μs** |  **0.0750 μs** |   **4.296 μs** | **2.08x faster** |   **0.06x** |   **9.78 KB** |  **1.19x less** |
| Serialize  | .NET 8.0           | 0         |   3.261 μs |  0.0568 μs |  0.0504 μs |   3.253 μs | 2.73x faster |   0.06x |   9.78 KB |  1.19x less |
| Serialize  | .NET Framework 4.8 | 0         |   8.917 μs |  0.1684 μs |  0.1802 μs |   8.910 μs |     baseline |         |  11.62 KB |             |
|            |                    |           |            |            |            |            |              |         |           |             |
| Deserialze | .NET 6.0           | 0         |   3.681 μs |  0.0454 μs |  0.0379 μs |   3.684 μs | 2.90x faster |   0.04x |   5.26 KB |  2.05x less |
| Deserialze | .NET 8.0           | 0         |   2.879 μs |  0.0553 μs |  0.0517 μs |   2.878 μs | 3.70x faster |   0.05x |   5.26 KB |  2.05x less |
| Deserialze | .NET Framework 4.8 | 0         |  10.670 μs |  0.0977 μs |  0.0866 μs |  10.655 μs |     baseline |         |  10.76 KB |             |
|            |                    |           |            |            |            |            |              |         |           |             |
| **Serialize**  | **.NET 6.0**           | **64**        |  **38.140 μs** |  **0.7336 μs** |  **0.6503 μs** |  **38.225 μs** | **3.47x faster** |   **0.23x** |  **24.87 KB** |  **2.61x less** |
| Serialize  | .NET 8.0           | 64        |  30.346 μs |  0.5505 μs |  0.4880 μs |  30.377 μs | 4.36x faster |   0.24x |  30.87 KB |  2.10x less |
| Serialize  | .NET Framework 4.8 | 64        | 133.818 μs |  3.2974 μs |  9.7223 μs | 136.810 μs |     baseline |         |  64.91 KB |             |
|            |                    |           |            |            |            |            |              |         |           |             |
| Deserialze | .NET 6.0           | 64        |  37.218 μs |  0.3944 μs |  0.3496 μs |  37.261 μs | 5.87x faster |   1.51x |  23.77 KB |  7.07x less |
| Deserialze | .NET 8.0           | 64        |  42.174 μs |  2.7085 μs |  7.9860 μs |  46.032 μs | 6.17x faster |   1.57x |  23.77 KB |  7.07x less |
| Deserialze | .NET Framework 4.8 | 64        | 249.978 μs |  8.9619 μs | 26.4245 μs | 259.593 μs |     baseline |         | 168.05 KB |             |
|            |                    |           |            |            |            |            |              |         |           |             |
| **Serialize**  | **.NET 6.0**           | **128**       | **124.015 μs** |  **4.6209 μs** | **13.6248 μs** | **128.919 μs** | **2.00x faster** |   **0.16x** |  **40.71 KB** |  **2.92x less** |
| Serialize  | .NET 8.0           | 128       |  94.919 μs |  1.8861 μs |  5.0017 μs |  97.008 μs | 2.59x faster |   0.29x |  52.71 KB |  2.25x less |
| Serialize  | .NET Framework 4.8 | 128       | 246.127 μs |  8.0884 μs | 23.8489 μs | 256.717 μs |     baseline |         | 118.74 KB |             |
|            |                    |           |            |            |            |            |              |         |           |             |
| Deserialze | .NET 6.0           | 128       | 111.931 μs |  4.4010 μs | 12.9764 μs | 116.768 μs | 4.29x faster |   0.59x |  40.42 KB |  7.97x less |
| Deserialze | .NET 8.0           | 128       |  83.434 μs |  2.1832 μs |  6.4372 μs |  85.326 μs | 5.71x faster |   0.52x |  40.42 KB |  7.97x less |
| Deserialze | .NET Framework 4.8 | 128       | 476.006 μs | 17.5744 μs | 51.8184 μs | 498.305 μs |     baseline |         | 322.05 KB |             |
