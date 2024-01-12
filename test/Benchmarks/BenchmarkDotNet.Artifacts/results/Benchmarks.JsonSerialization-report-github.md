```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3007/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  Job-ISCVXC : .NET 6.0.26 (6.0.2623.60508), X64 RyuJIT AVX2
  Job-DINVIM : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  Job-OKZTLT : .NET Framework 4.8.1 (4.8.9181.0), X64 RyuJIT VectorSize=256


```
| Method     | Runtime            | ItemCount | Mean         | Error      | StdDev     | Ratio        | RatioSD | Allocated | Alloc Ratio |
|----------- |------------------- |---------- |-------------:|-----------:|-----------:|-------------:|--------:|----------:|------------:|
| **Serialize**  | **.NET 6.0**           | **0**         |     **2.081 μs** |  **0.0400 μs** |  **0.0428 μs** | **3.06x faster** |   **0.07x** |   **1.02 KB** |  **1.01x less** |
| Serialize  | .NET 8.0           | 0         |     1.413 μs |  0.0173 μs |  0.0145 μs | 4.50x faster |   0.07x |   1.02 KB |  1.01x less |
| Serialize  | .NET Framework 4.8 | 0         |     6.365 μs |  0.0584 μs |  0.0518 μs |     baseline |         |   1.03 KB |             |
|            |                    |           |              |            |            |              |         |           |             |
| Deserialze | .NET 6.0           | 0         |    14.862 μs |  0.1605 μs |  0.1423 μs | 3.25x faster |   0.05x |   9.55 KB |  1.49x less |
| Deserialze | .NET 8.0           | 0         |     8.736 μs |  0.1747 μs |  0.1548 μs | 5.54x faster |   0.13x |   9.55 KB |  1.49x less |
| Deserialze | .NET Framework 4.8 | 0         |    48.260 μs |  0.6721 μs |  0.6287 μs |     baseline |         |  14.23 KB |             |
|            |                    |           |              |            |            |              |         |           |             |
| **Serialize**  | **.NET 6.0**           | **64**        |    **30.436 μs** |  **0.2796 μs** |  **0.2479 μs** | **3.48x faster** |   **0.05x** |  **15.05 KB** |  **2.87x less** |
| Serialize  | .NET 8.0           | 64        |    24.001 μs |  0.2593 μs |  0.2298 μs | 4.41x faster |   0.06x |  15.05 KB |  2.87x less |
| Serialize  | .NET Framework 4.8 | 64        |   105.791 μs |  1.1450 μs |  1.0710 μs |     baseline |         |  43.17 KB |             |
|            |                    |           |              |            |            |              |         |           |             |
| Deserialze | .NET 6.0           | 64        |   143.098 μs |  1.7480 μs |  1.5495 μs | 4.13x faster |   0.07x |  42.23 KB |  1.11x less |
| Deserialze | .NET 8.0           | 64        |    92.824 μs |  1.1412 μs |  1.0675 μs | 6.38x faster |   0.17x |  42.23 KB |  1.11x less |
| Deserialze | .NET Framework 4.8 | 64        |   592.253 μs | 11.3500 μs | 11.6557 μs |     baseline |         |  47.02 KB |             |
|            |                    |           |              |            |            |              |         |           |             |
| **Serialize**  | **.NET 6.0**           | **128**       |    **56.813 μs** |  **0.5838 μs** |  **0.5461 μs** | **3.50x faster** |   **0.08x** |   **28.3 KB** |  **2.99x less** |
| Serialize  | .NET 8.0           | 128       |    47.051 μs |  0.4885 μs |  0.4569 μs | 4.23x faster |   0.05x |   28.3 KB |  2.99x less |
| Serialize  | .NET Framework 4.8 | 128       |   199.065 μs |  3.2163 μs |  2.8512 μs |     baseline |         |   84.5 KB |             |
|            |                    |           |              |            |            |              |         |           |             |
| Deserialze | .NET 6.0           | 128       |   268.396 μs |  4.9262 μs |  4.6080 μs | 4.20x faster |   0.08x |  74.61 KB |  1.06x less |
| Deserialze | .NET 8.0           | 128       |   172.645 μs |  2.3313 μs |  2.0666 μs | 6.53x faster |   0.11x |  74.61 KB |  1.06x less |
| Deserialze | .NET Framework 4.8 | 128       | 1,127.991 μs | 13.8243 μs | 12.2549 μs |     baseline |         |  79.45 KB |             |
