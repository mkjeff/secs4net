```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-OOEVDF : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-AKZPHK : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method                                   | Runtime  | InputChunkSize | MessageCount | Mean     | Error     | StdDev    | Ratio        | RatioSD | Gen0     | Gen1   | Allocated | Alloc Ratio |
|----------------------------------------- |--------- |--------------- |------------- |---------:|----------:|----------:|-------------:|--------:|---------:|-------:|----------:|------------:|
| **Message_Can_Decode_From_Chunked_Sequence** | **.NET 6.0** | **16**             | **500**          | **3.964 ms** | **0.0780 ms** | **0.1041 ms** |     **baseline** |        **** | **148.4375** | **7.8125** |   **1.86 MB** |            **** |
| Message_Can_Decode_From_Chunked_Sequence | .NET 8.0 | 16             | 500          | 3.012 ms | 0.0344 ms | 0.0305 ms | 1.30x faster |   0.04x | 148.4375 | 7.8125 |   1.86 MB |  1.00x more |
|                                          |          |                |              |          |           |           |              |         |          |        |           |             |
| **Message_Can_Decode_From_Chunked_Sequence** | **.NET 6.0** | **64**             | **500**          | **2.968 ms** | **0.0366 ms** | **0.0343 ms** |     **baseline** |        **** | **148.4375** | **7.8125** |   **1.86 MB** |            **** |
| Message_Can_Decode_From_Chunked_Sequence | .NET 8.0 | 64             | 500          | 2.419 ms | 0.0412 ms | 0.0386 ms | 1.23x faster |   0.03x | 148.4375 | 7.8125 |   1.86 MB |  1.00x less |
|                                          |          |                |              |          |           |           |              |         |          |        |           |             |
| **Message_Can_Decode_From_Chunked_Sequence** | **.NET 6.0** | **256**            | **500**          | **2.867 ms** | **0.0295 ms** | **0.0276 ms** |     **baseline** |        **** | **148.4375** | **7.8125** |   **1.86 MB** |            **** |
| Message_Can_Decode_From_Chunked_Sequence | .NET 8.0 | 256            | 500          | 2.400 ms | 0.0477 ms | 0.0490 ms | 1.20x faster |   0.02x | 148.4375 | 7.8125 |   1.86 MB |  1.00x less |
