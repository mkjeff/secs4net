```

BenchmarkDotNet v0.13.11, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-ZWGTBM : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-ZQLZSO : .NET Framework 4.8.1 (4.8.9181.0), X64 RyuJIT VectorSize=256


```
| Method     | Runtime            | ItemCount | Mean         | Error     | StdDev    | Ratio        | RatioSD | Allocated | Alloc Ratio |
|----------- |------------------- |---------- |-------------:|----------:|----------:|-------------:|--------:|----------:|------------:|
| **Serialize**  | **.NET 8.0**           | **0**         |     **1.332 μs** | **0.0078 μs** | **0.0069 μs** | **4.57x faster** |   **0.04x** |   **1.02 KB** |  **1.01x less** |
| Serialize  | .NET Framework 4.8 | 0         |     6.088 μs | 0.0388 μs | 0.0363 μs |     baseline |         |   1.03 KB |             |
|            |                    |           |              |           |           |              |         |           |             |
| Deserialze | .NET 8.0           | 0         |     8.276 μs | 0.0467 μs | 0.0437 μs | 5.47x faster |   0.04x |   9.55 KB |  1.49x less |
| Deserialze | .NET Framework 4.8 | 0         |    45.229 μs | 0.2820 μs | 0.2638 μs |     baseline |         |  14.23 KB |             |
|            |                    |           |              |           |           |              |         |           |             |
| **Serialize**  | **.NET 8.0**           | **64**        |    **22.574 μs** | **0.0290 μs** | **0.0257 μs** | **4.41x faster** |   **0.02x** |  **15.05 KB** |  **2.87x less** |
| Serialize  | .NET Framework 4.8 | 64        |    99.643 μs | 0.4394 μs | 0.4110 μs |     baseline |         |  43.17 KB |             |
|            |                    |           |              |           |           |              |         |           |             |
| Deserialze | .NET 8.0           | 64        |    87.519 μs | 0.3611 μs | 0.3201 μs | 6.48x faster |   0.03x |  42.23 KB |  1.11x less |
| Deserialze | .NET Framework 4.8 | 64        |   567.064 μs | 2.4475 μs | 2.1696 μs |     baseline |         |  47.02 KB |             |
|            |                    |           |              |           |           |              |         |           |             |
| **Serialize**  | **.NET 8.0**           | **128**       |    **42.411 μs** | **0.2819 μs** | **0.2637 μs** | **4.44x faster** |   **0.03x** |   **28.3 KB** |  **2.99x less** |
| Serialize  | .NET Framework 4.8 | 128       |   188.163 μs | 1.0986 μs | 1.0277 μs |     baseline |         |   84.5 KB |             |
|            |                    |           |              |           |           |              |         |           |             |
| Deserialze | .NET 8.0           | 128       |   164.348 μs | 0.9145 μs | 0.8554 μs | 6.57x faster |   0.05x |  74.61 KB |  1.06x less |
| Deserialze | .NET Framework 4.8 | 128       | 1,080.616 μs | 5.1112 μs | 4.2681 μs |     baseline |         |  79.45 KB |             |
