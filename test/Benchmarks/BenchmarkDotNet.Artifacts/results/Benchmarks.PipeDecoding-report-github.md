```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-LLCSMO : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-LSZLMA : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method                                   | Runtime  | InputChunkSize | MessageCount | Mean     | Error     | StdDev    | Ratio        | RatioSD | Gen0     | Gen1   | Allocated | Alloc Ratio |
|----------------------------------------- |--------- |--------------- |------------- |---------:|----------:|----------:|-------------:|--------:|---------:|-------:|----------:|------------:|
| **Message_Can_Decode_From_Chunked_Sequence** | **.NET 6.0** | **16**             | **500**          | **3.805 ms** | **0.0587 ms** | **0.0549 ms** |     **baseline** |        **** | **171.8750** | **7.8125** |   **2.18 MB** |            **** |
| Message_Can_Decode_From_Chunked_Sequence | .NET 8.0 | 16             | 500          | 2.902 ms | 0.0569 ms | 0.0532 ms | 1.31x faster |   0.03x | 171.8750 |      - |   2.19 MB |  1.01x more |
|                                          |          |                |              |          |           |           |              |         |          |        |           |             |
| **Message_Can_Decode_From_Chunked_Sequence** | **.NET 6.0** | **64**             | **500**          | **3.044 ms** | **0.0410 ms** | **0.0363 ms** |     **baseline** |        **** | **171.8750** | **7.8125** |   **2.18 MB** |            **** |
| Message_Can_Decode_From_Chunked_Sequence | .NET 8.0 | 64             | 500          | 2.503 ms | 0.0412 ms | 0.0365 ms | 1.22x faster |   0.02x | 175.7813 | 7.8125 |   2.19 MB |  1.01x more |
|                                          |          |                |              |          |           |           |              |         |          |        |           |             |
| **Message_Can_Decode_From_Chunked_Sequence** | **.NET 6.0** | **256**            | **500**          | **3.049 ms** | **0.0301 ms** | **0.0267 ms** |     **baseline** |        **** | **171.8750** | **7.8125** |   **2.18 MB** |            **** |
| Message_Can_Decode_From_Chunked_Sequence | .NET 8.0 | 256            | 500          | 2.446 ms | 0.0200 ms | 0.0156 ms | 1.25x faster |   0.01x | 171.8750 |      - |   2.19 MB |  1.01x more |
