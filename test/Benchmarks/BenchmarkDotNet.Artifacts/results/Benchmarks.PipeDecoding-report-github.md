```

BenchmarkDotNet v0.13.11, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-ZWGTBM : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-ZQLZSO : .NET Framework 4.8.1 (4.8.9181.0), X64 RyuJIT VectorSize=256


```
| Method                                   | Runtime            | InputChunkSize | MessageCount | Mean     | Error     | StdDev    | Ratio        | RatioSD | Gen0     | Gen1   | Allocated | Alloc Ratio |
|----------------------------------------- |------------------- |--------------- |------------- |---------:|----------:|----------:|-------------:|--------:|---------:|-------:|----------:|------------:|
| **Message_Can_Decode_From_Chunked_Sequence** | **.NET 8.0**           | **16**             | **500**          | **3.028 ms** | **0.0384 ms** | **0.0359 ms** | **2.58x faster** |   **0.05x** | **148.4375** | **7.8125** |   **1.86 MB** |  **1.03x less** |
| Message_Can_Decode_From_Chunked_Sequence | .NET Framework 4.8 | 16             | 500          | 7.811 ms | 0.0829 ms | 0.0776 ms |     baseline |         | 312.5000 | 7.8125 |   1.91 MB |             |
|                                          |                    |                |              |          |           |           |              |         |          |        |           |             |
| **Message_Can_Decode_From_Chunked_Sequence** | **.NET 8.0**           | **64**             | **500**          | **2.377 ms** | **0.0047 ms** | **0.0042 ms** | **2.59x faster** |   **0.01x** | **148.4375** | **7.8125** |   **1.86 MB** |  **1.03x less** |
| Message_Can_Decode_From_Chunked_Sequence | .NET Framework 4.8 | 64             | 500          | 6.164 ms | 0.0123 ms | 0.0109 ms |     baseline |         | 312.5000 | 7.8125 |   1.91 MB |             |
|                                          |                    |                |              |          |           |           |              |         |          |        |           |             |
| **Message_Can_Decode_From_Chunked_Sequence** | **.NET 8.0**           | **256**            | **500**          | **2.349 ms** | **0.0054 ms** | **0.0050 ms** | **2.53x faster** |   **0.01x** | **148.4375** | **7.8125** |   **1.86 MB** |  **1.03x less** |
| Message_Can_Decode_From_Chunked_Sequence | .NET Framework 4.8 | 256            | 500          | 5.951 ms | 0.0307 ms | 0.0288 ms |     baseline |         | 312.5000 | 7.8125 |   1.91 MB |             |
