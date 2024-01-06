```

BenchmarkDotNet v0.13.11, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-ZWGTBM : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-ZQLZSO : .NET Framework 4.8.1 (4.8.9181.0), X64 RyuJIT VectorSize=256


```
| Method | Runtime            | ItemCount | Mean       | Error    | StdDev   | Ratio        | RatioSD | Allocated | Alloc Ratio |
|------- |------------------- |---------- |-----------:|---------:|---------:|-------------:|--------:|----------:|------------:|
| **Encode** | **.NET 8.0**           | **0**         |   **117.7 ns** |  **0.63 ns** |  **0.59 ns** | **6.10x faster** |   **0.04x** |      **40 B** |  **1.00x more** |
| Encode | .NET Framework 4.8 | 0         |   717.8 ns |  1.26 ns |  1.18 ns |     baseline |         |      40 B |             |
|        |                    |           |            |          |          |              |         |           |             |
| Decode | .NET 8.0           | 0         | 1,152.2 ns | 15.10 ns | 14.13 ns | 3.44x faster |   0.05x |     560 B |  1.00x less |
| Decode | .NET Framework 4.8 | 0         | 3,958.1 ns | 25.40 ns | 23.76 ns |     baseline |         |     562 B |             |
|        |                    |           |            |          |          |              |         |           |             |
| **Encode** | **.NET 8.0**           | **64**        |   **578.8 ns** |  **0.92 ns** |  **0.76 ns** | **4.56x faster** |   **0.02x** |      **40 B** |  **1.00x more** |
| Encode | .NET Framework 4.8 | 64        | 2,640.8 ns | 10.63 ns |  9.94 ns |     baseline |         |      40 B |             |
|        |                    |           |            |          |          |              |         |           |             |
| Decode | .NET 8.0           | 64        | 2,415.4 ns | 14.38 ns | 12.01 ns | 2.89x faster |   0.03x |    7248 B |  1.00x less |
| Decode | .NET Framework 4.8 | 64        | 6,978.1 ns | 43.33 ns | 40.53 ns |     baseline |         |    7269 B |             |
|        |                    |           |            |          |          |              |         |           |             |
| **Encode** | **.NET 8.0**           | **128**       |   **899.9 ns** |  **5.84 ns** |  **5.46 ns** | **4.04x faster** |   **0.04x** |      **40 B** |  **1.00x more** |
| Encode | .NET Framework 4.8 | 128       | 3,634.0 ns | 24.46 ns | 22.88 ns |     baseline |         |      40 B |             |
|        |                    |           |            |          |          |              |         |           |             |
| Decode | .NET 8.0           | 128       | 2,894.7 ns | 11.05 ns | 10.34 ns | 2.80x faster |   0.02x |    9112 B |  1.00x less |
| Decode | .NET Framework 4.8 | 128       | 8,119.2 ns | 63.38 ns | 59.29 ns |     baseline |         |    9139 B |             |
