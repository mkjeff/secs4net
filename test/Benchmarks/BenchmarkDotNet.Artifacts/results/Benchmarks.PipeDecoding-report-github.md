```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2792/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-VAUECX : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-NTIFBX : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method                                   | Runtime  | InputChunkSize | MessageCount | Mean     | Error     | StdDev    | Ratio        | RatioSD | Gen0     | Gen1   | Allocated | Alloc Ratio |
|----------------------------------------- |--------- |--------------- |------------- |---------:|----------:|----------:|-------------:|--------:|---------:|-------:|----------:|------------:|
| **Message_Can_Decode_From_Chunked_Sequence** | **.NET 6.0** | **16**             | **500**          | **4.024 ms** | **0.0777 ms** | **0.0831 ms** |     **baseline** |        **** | **171.8750** | **7.8125** |   **2.18 MB** |            **** |
| Message_Can_Decode_From_Chunked_Sequence | .NET 8.0 | 16             | 500          | 3.048 ms | 0.0144 ms | 0.0128 ms | 1.32x faster |   0.03x | 175.7813 | 7.8125 |   2.19 MB |  1.01x more |
|                                          |          |                |              |          |           |           |              |         |          |        |           |             |
| **Message_Can_Decode_From_Chunked_Sequence** | **.NET 6.0** | **64**             | **500**          | **3.150 ms** | **0.0231 ms** | **0.0205 ms** |     **baseline** |        **** | **171.8750** | **7.8125** |   **2.18 MB** |            **** |
| Message_Can_Decode_From_Chunked_Sequence | .NET 8.0 | 64             | 500          | 2.627 ms | 0.0497 ms | 0.0465 ms | 1.20x faster |   0.03x | 171.8750 |      - |   2.19 MB |  1.01x more |
|                                          |          |                |              |          |           |           |              |         |          |        |           |             |
| **Message_Can_Decode_From_Chunked_Sequence** | **.NET 6.0** | **256**            | **500**          | **3.185 ms** | **0.0210 ms** | **0.0186 ms** |     **baseline** |        **** | **171.8750** | **7.8125** |   **2.18 MB** |            **** |
| Message_Can_Decode_From_Chunked_Sequence | .NET 8.0 | 256            | 500          | 2.620 ms | 0.0235 ms | 0.0209 ms | 1.22x faster |   0.01x | 175.7813 | 7.8125 |   2.19 MB |  1.01x more |
