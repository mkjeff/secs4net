``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.400
  [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  Job-AITDSN : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  Job-TPKEIE : .NET Framework 4.8 (4.8.4515.0), X64 RyuJIT


```
|                                   Method |              Runtime | InputChunkSize | MessageCount |      Mean |     Error |    StdDev |        Ratio | RatioSD |    Gen 0 |   Gen 1 | Allocated |
|----------------------------------------- |--------------------- |--------------- |------------- |----------:|----------:|----------:|-------------:|--------:|---------:|--------:|----------:|
| **Message_Can_Decode_From_Chunked_Sequence** |             **.NET 6.0** |             **16** |          **500** |  **5.057 ms** | **0.0178 ms** | **0.0139 ms** | **2.77x faster** |   **0.03x** | **273.4375** | **15.6250** |      **2 MB** |
| Message_Can_Decode_From_Chunked_Sequence | .NET Framework 4.7.2 |             16 |          500 | 13.969 ms | 0.2045 ms | 0.1813 ms |     baseline |         | 359.3750 |       - |      2 MB |
|                                          |                      |                |              |           |           |           |              |         |          |         |           |
| **Message_Can_Decode_From_Chunked_Sequence** |             **.NET 6.0** |             **64** |          **500** |  **4.955 ms** | **0.0370 ms** | **0.0346 ms** | **2.34x faster** |   **0.04x** | **273.4375** | **15.6250** |      **2 MB** |
| Message_Can_Decode_From_Chunked_Sequence | .NET Framework 4.7.2 |             64 |          500 | 11.575 ms | 0.1836 ms | 0.1717 ms |     baseline |         | 359.3750 |       - |      2 MB |
|                                          |                      |                |              |           |           |           |              |         |          |         |           |
| **Message_Can_Decode_From_Chunked_Sequence** |             **.NET 6.0** |            **256** |          **500** |  **4.754 ms** | **0.0517 ms** | **0.0483 ms** | **2.31x faster** |   **0.02x** | **273.4375** | **15.6250** |      **2 MB** |
| Message_Can_Decode_From_Chunked_Sequence | .NET Framework 4.7.2 |            256 |          500 | 10.984 ms | 0.1017 ms | 0.0951 ms |     baseline |         | 359.3750 |       - |      2 MB |
