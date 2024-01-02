```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-ZQSTYQ : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-LEHGCO : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method                                   | Runtime  | InputChunkSize | MessageCount | Mean     | Error     | StdDev    | Ratio        | RatioSD | Gen0     | Gen1   | Allocated | Alloc Ratio |
|----------------------------------------- |--------- |--------------- |------------- |---------:|----------:|----------:|-------------:|--------:|---------:|-------:|----------:|------------:|
| **Message_Can_Decode_From_Chunked_Sequence** | **.NET 6.0** | **16**             | **500**          | **3.879 ms** | **0.0491 ms** | **0.0410 ms** |     **baseline** |        **** | **171.8750** | **7.8125** |   **2.18 MB** |            **** |
| Message_Can_Decode_From_Chunked_Sequence | .NET 8.0 | 16             | 500          | 3.005 ms | 0.0201 ms | 0.0178 ms | 1.29x faster |   0.02x | 175.7813 | 7.8125 |   2.19 MB |  1.01x more |
|                                          |          |                |              |          |           |           |              |         |          |        |           |             |
| **Message_Can_Decode_From_Chunked_Sequence** | **.NET 6.0** | **64**             | **500**          | **2.930 ms** | **0.0204 ms** | **0.0181 ms** |     **baseline** |        **** | **171.8750** | **7.8125** |   **2.18 MB** |            **** |
| Message_Can_Decode_From_Chunked_Sequence | .NET 8.0 | 64             | 500          | 2.410 ms | 0.0049 ms | 0.0041 ms | 1.22x faster |   0.01x | 171.8750 |      - |   2.19 MB |  1.01x more |
|                                          |          |                |              |          |           |           |              |         |          |        |           |             |
| **Message_Can_Decode_From_Chunked_Sequence** | **.NET 6.0** | **256**            | **500**          | **2.903 ms** | **0.0112 ms** | **0.0100 ms** |     **baseline** |        **** | **171.8750** | **7.8125** |   **2.18 MB** |            **** |
| Message_Can_Decode_From_Chunked_Sequence | .NET 8.0 | 256            | 500          | 2.457 ms | 0.0225 ms | 0.0211 ms | 1.18x faster |   0.01x | 171.8750 |      - |   2.19 MB |  1.01x more |
