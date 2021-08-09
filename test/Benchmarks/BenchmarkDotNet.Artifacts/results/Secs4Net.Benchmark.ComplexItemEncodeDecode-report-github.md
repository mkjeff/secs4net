``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1151 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.6.21355.2
  [Host]     : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-PSNALV : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-DJMMZE : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
| Method |              Runtime | ItemCount |        Mean |    Error |   StdDev |        Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|------- |--------------------- |---------- |------------:|---------:|---------:|-------------:|--------:|-------:|-------:|------:|----------:|
| **Encode** |             **.NET 6.0** |         **0** |    **634.4 ns** |  **6.56 ns** |  **6.14 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
| Encode | .NET Framework 4.7.2 |         0 |  1,416.5 ns |  3.24 ns |  2.87 ns | 2.23x slower |   0.02x | 0.0057 |      - |     - |      40 B |
|        |                      |           |             |          |          |              |         |        |        |       |           |
| Decode |             .NET 6.0 |         0 |  2,951.8 ns |  8.20 ns |  7.67 ns |     baseline |         | 0.0648 |      - |     - |     560 B |
| Decode | .NET Framework 4.7.2 |         0 |  5,740.7 ns | 15.37 ns | 14.38 ns | 1.94x slower |   0.01x | 0.0839 |      - |     - |     562 B |
|        |                      |           |             |          |          |              |         |        |        |       |           |
| **Encode** |             **.NET 6.0** |        **64** |  **1,652.5 ns** |  **6.50 ns** |  **6.08 ns** |     **baseline** |        **** | **0.0038** |      **-** |     **-** |      **40 B** |
| Encode | .NET Framework 4.7.2 |        64 |  4,365.6 ns | 15.49 ns | 14.49 ns | 2.64x slower |   0.01x |      - |      - |     - |      40 B |
|        |                      |           |             |          |          |              |         |        |        |       |           |
| Decode |             .NET 6.0 |        64 |  4,913.7 ns | 26.02 ns | 24.34 ns |     baseline |         | 0.8621 | 0.0153 |     - |   7,248 B |
| Decode | .NET Framework 4.7.2 |        64 | 10,702.1 ns | 97.10 ns | 86.07 ns | 2.18x slower |   0.02x | 1.1444 | 0.0153 |     - |   7,269 B |
|        |                      |           |             |          |          |              |         |        |        |       |           |
| **Encode** |             **.NET 6.0** |       **128** |  **2,224.0 ns** |  **7.30 ns** |  **6.47 ns** |     **baseline** |        **** | **0.0038** |      **-** |     **-** |      **40 B** |
| Encode | .NET Framework 4.7.2 |       128 |  5,984.1 ns | 13.14 ns | 10.97 ns | 2.69x slower |   0.01x |      - |      - |     - |      40 B |
|        |                      |           |             |          |          |              |         |        |        |       |           |
| Decode |             .NET 6.0 |       128 |  5,928.8 ns | 40.28 ns | 35.71 ns |     baseline |         | 1.0834 | 0.0305 |     - |   9,112 B |
| Decode | .NET Framework 4.7.2 |       128 | 12,730.1 ns | 37.11 ns | 32.90 ns | 2.15x slower |   0.02x | 1.4343 | 0.0305 |     - |   9,139 B |
