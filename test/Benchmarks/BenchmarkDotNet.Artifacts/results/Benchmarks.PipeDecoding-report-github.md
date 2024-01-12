```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3007/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  Job-ERYFHQ : .NET 6.0.26 (6.0.2623.60508), X64 RyuJIT AVX2
  Job-GVMCZA : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  Job-BVWEYE : .NET Framework 4.8.1 (4.8.9181.0), X64 RyuJIT VectorSize=256


```
| Method           | Runtime            | InputChunkSize | MessageCount | Mean     | Error     | StdDev    | Median   | Ratio        | RatioSD | Gen0     | Gen1   | Allocated | Alloc Ratio |
|----------------- |------------------- |--------------- |------------- |---------:|----------:|----------:|---------:|-------------:|--------:|---------:|-------:|----------:|------------:|
| **Chunked_Sequence** | **.NET 6.0**           | **16**             | **500**          | **4.027 ms** | **0.0764 ms** | **0.0715 ms** | **4.016 ms** | **2.06x faster** |   **0.08x** | **156.2500** | **7.8125** |   **2.02 MB** |  **1.04x less** |
| Chunked_Sequence | .NET 8.0           | 16             | 500          | 3.089 ms | 0.0497 ms | 0.1273 ms | 3.035 ms | 2.70x faster |   0.14x | 160.1563 | 7.8125 |   2.02 MB |  1.04x less |
| Chunked_Sequence | .NET Framework 4.8 | 16             | 500          | 8.214 ms | 0.1628 ms | 0.2675 ms | 8.214 ms |     baseline |         | 343.7500 |      - |   2.09 MB |             |
|                  |                    |                |              |          |           |           |          |              |         |          |        |           |             |
| **Chunked_Sequence** | **.NET 6.0**           | **64**             | **500**          | **2.915 ms** | **0.0362 ms** | **0.0338 ms** | **2.925 ms** | **2.14x faster** |   **0.03x** | **160.1563** | **7.8125** |   **2.02 MB** |  **1.04x less** |
| Chunked_Sequence | .NET 8.0           | 64             | 500          | 2.348 ms | 0.0308 ms | 0.0273 ms | 2.343 ms | 2.66x faster |   0.03x | 156.2500 | 7.8125 |   2.02 MB |  1.04x less |
| Chunked_Sequence | .NET Framework 4.8 | 64             | 500          | 6.254 ms | 0.0465 ms | 0.0412 ms | 6.246 ms |     baseline |         | 343.7500 | 7.8125 |   2.09 MB |             |
|                  |                    |                |              |          |           |           |          |              |         |          |        |           |             |
| **Chunked_Sequence** | **.NET 6.0**           | **256**            | **500**          | **2.837 ms** | **0.0297 ms** | **0.0248 ms** | **2.835 ms** | **2.11x faster** |   **0.02x** | **160.1563** | **7.8125** |   **2.02 MB** |  **1.04x less** |
| Chunked_Sequence | .NET 8.0           | 256            | 500          | 2.380 ms | 0.0463 ms | 0.0514 ms | 2.376 ms | 2.52x faster |   0.05x | 156.2500 | 7.8125 |   2.02 MB |  1.04x less |
| Chunked_Sequence | .NET Framework 4.8 | 256            | 500          | 5.989 ms | 0.0412 ms | 0.0344 ms | 5.983 ms |     baseline |         | 343.7500 | 7.8125 |    2.1 MB |             |
