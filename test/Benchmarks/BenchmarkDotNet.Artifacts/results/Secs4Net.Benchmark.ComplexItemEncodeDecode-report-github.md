``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1151 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.6.21355.2
  [Host]     : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-WGPAHI : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-CDVTON : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
| Method |              Runtime | ItemCount |        Mean |     Error |    StdDev |        Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|------- |--------------------- |---------- |------------:|----------:|----------:|-------------:|--------:|-------:|-------:|------:|----------:|
| **Encode** |             **.NET 6.0** |         **0** |    **814.1 ns** |   **1.90 ns** |   **1.78 ns** | **1.86x faster** |   **0.00x** | **0.0048** |      **-** |     **-** |      **40 B** |
| Encode | .NET Framework 4.7.2 |         0 |  1,512.7 ns |   2.42 ns |   2.14 ns |     baseline |         | 0.0057 |      - |     - |      40 B |
|        |                      |           |             |           |           |              |         |        |        |       |           |
| Decode |             .NET 6.0 |         0 |  4,981.8 ns |  10.20 ns |   9.54 ns | 1.32x faster |   0.00x | 0.0610 |      - |     - |     560 B |
| Decode | .NET Framework 4.7.2 |         0 |  6,583.4 ns |  15.04 ns |  14.07 ns |     baseline |         | 0.0839 |      - |     - |     562 B |
|        |                      |           |             |           |           |              |         |        |        |       |           |
| **Encode** |             **.NET 6.0** |        **64** |  **7,126.8 ns** |   **6.79 ns** |   **6.02 ns** | **1.68x faster** |   **0.00x** |      **-** |      **-** |     **-** |      **40 B** |
| Encode | .NET Framework 4.7.2 |        64 | 11,954.0 ns |  14.02 ns |  12.43 ns |     baseline |         |      - |      - |     - |      40 B |
|        |                      |           |             |           |           |              |         |        |        |       |           |
| Decode |             .NET 6.0 |        64 |  7,470.6 ns |  22.97 ns |  20.36 ns | 1.57x faster |   0.01x | 0.8621 | 0.0153 |     - |   7,248 B |
| Decode | .NET Framework 4.7.2 |        64 | 11,744.2 ns |  44.24 ns |  34.54 ns |     baseline |         | 1.1444 | 0.0153 |     - |   7,269 B |
|        |                      |           |             |           |           |              |         |        |        |       |           |
| **Encode** |             **.NET 6.0** |       **128** | **12,783.3 ns** |  **11.30 ns** |  **10.57 ns** | **1.65x faster** |   **0.00x** |      **-** |      **-** |     **-** |      **40 B** |
| Encode | .NET Framework 4.7.2 |       128 | 21,078.1 ns |  19.18 ns |  17.00 ns |     baseline |         |      - |      - |     - |      40 B |
|        |                      |           |             |           |           |              |         |        |        |       |           |
| Decode |             .NET 6.0 |       128 |  8,638.5 ns |  47.87 ns |  44.78 ns | 1.61x faster |   0.02x | 1.0834 | 0.0305 |     - |   9,112 B |
| Decode | .NET Framework 4.7.2 |       128 | 13,894.1 ns | 142.13 ns | 132.95 ns |     baseline |         | 1.4343 | 0.0305 |     - |   9,139 B |
