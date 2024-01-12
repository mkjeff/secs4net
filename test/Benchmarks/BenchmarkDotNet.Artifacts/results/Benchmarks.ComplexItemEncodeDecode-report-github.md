```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3007/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  Job-ERYFHQ : .NET 6.0.26 (6.0.2623.60508), X64 RyuJIT AVX2
  Job-GVMCZA : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  Job-BVWEYE : .NET Framework 4.8.1 (4.8.9181.0), X64 RyuJIT VectorSize=256


```
| Method | Runtime            | ItemCount | Mean       | Error     | StdDev    | Ratio        | RatioSD | Allocated | Alloc Ratio |
|------- |------------------- |---------- |-----------:|----------:|----------:|-------------:|--------:|----------:|------------:|
| **Encode** | **.NET 6.0**           | **0**         |   **191.4 ns** |   **2.29 ns** |   **2.15 ns** | **3.91x faster** |   **0.05x** |      **40 B** |  **1.00x more** |
| Encode | .NET 8.0           | 0         |   121.2 ns |   1.29 ns |   1.08 ns | 6.17x faster |   0.08x |      40 B |  1.00x more |
| Encode | .NET Framework 4.8 | 0         |   747.9 ns |   7.58 ns |   6.72 ns |     baseline |         |      40 B |             |
|        |                    |           |            |           |           |              |         |           |             |
| Decode | .NET 6.0           | 0         | 1,497.3 ns |  23.17 ns |  21.67 ns | 2.86x faster |   0.04x |     560 B |  1.00x less |
| Decode | .NET 8.0           | 0         | 1,245.6 ns |  17.28 ns |  15.32 ns | 3.44x faster |   0.07x |     560 B |  1.00x less |
| Decode | .NET Framework 4.8 | 0         | 4,283.4 ns |  67.62 ns |  63.25 ns |     baseline |         |     562 B |             |
|        |                    |           |            |           |           |              |         |           |             |
| **Encode** | **.NET 6.0**           | **64**        |   **750.1 ns** |  **10.31 ns** |   **9.14 ns** | **3.72x faster** |   **0.07x** |      **40 B** |  **1.00x more** |
| Encode | .NET 8.0           | 64        |   616.0 ns |  10.61 ns |   9.92 ns | 4.54x faster |   0.10x |      40 B |  1.00x more |
| Encode | .NET Framework 4.8 | 64        | 2,793.4 ns |  40.06 ns |  37.47 ns |     baseline |         |      40 B |             |
|        |                    |           |            |           |           |              |         |           |             |
| Decode | .NET 6.0           | 64        | 2,669.1 ns |  26.61 ns |  23.59 ns | 2.67x faster |   0.03x |    7704 B |  1.01x less |
| Decode | .NET 8.0           | 64        | 2,363.6 ns |  45.89 ns |  42.93 ns | 3.01x faster |   0.07x |    7704 B |  1.01x less |
| Decode | .NET Framework 4.8 | 64        | 7,113.2 ns |  67.42 ns |  59.77 ns |     baseline |         |    7751 B |             |
|        |                    |           |            |           |           |              |         |           |             |
| **Encode** | **.NET 6.0**           | **128**       | **1,141.0 ns** |  **20.50 ns** |  **19.17 ns** | **3.33x faster** |   **0.07x** |      **40 B** |  **1.00x more** |
| Encode | .NET 8.0           | 128       |   978.5 ns |  19.06 ns |  17.83 ns | 3.89x faster |   0.10x |      40 B |  1.00x more |
| Encode | .NET Framework 4.8 | 128       | 3,799.2 ns |  44.26 ns |  39.23 ns |     baseline |         |      40 B |             |
|        |                    |           |            |           |           |              |         |           |             |
| Decode | .NET 6.0           | 128       | 3,271.0 ns |  30.54 ns |  27.07 ns | 2.60x faster |   0.05x |    9952 B |  1.01x less |
| Decode | .NET 8.0           | 128       | 2,896.4 ns |  49.87 ns |  46.65 ns | 2.94x faster |   0.06x |    9952 B |  1.01x less |
| Decode | .NET Framework 4.8 | 128       | 8,500.2 ns | 154.15 ns | 144.19 ns |     baseline |         |   10006 B |             |
