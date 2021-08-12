``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1165 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.7.21379.14
  [Host]     : .NET 6.0.0 (6.0.21.37719), X64 RyuJIT
  Job-GVDIFM : .NET 6.0.0 (6.0.21.37719), X64 RyuJIT
  Job-WCNVCL : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
| Method |              Runtime | ItemCount |        Mean |     Error |    StdDev |        Ratio | RatioSD |  Gen 0 |  Gen 1 | Allocated |
|------- |--------------------- |---------- |------------:|----------:|----------:|-------------:|--------:|-------:|-------:|----------:|
| **Encode** |             **.NET 6.0** |         **0** |    **458.6 ns** |   **2.25 ns** |   **1.99 ns** | **2.69x faster** |   **0.01x** | **0.0048** |      **-** |      **40 B** |
| Encode | .NET Framework 4.7.2 |         0 |  1,233.2 ns |   3.81 ns |   3.38 ns |     baseline |         | 0.0057 |      - |      40 B |
|        |                      |           |             |           |           |              |         |        |        |           |
| Decode |             .NET 6.0 |         0 |  2,564.3 ns |   7.83 ns |   7.32 ns | 2.46x faster |   0.01x | 0.0648 |      - |     560 B |
| Decode | .NET Framework 4.7.2 |         0 |  6,297.5 ns |  12.07 ns |  10.08 ns |     baseline |         | 0.0839 |      - |     562 B |
|        |                      |           |             |           |           |              |         |        |        |           |
| **Encode** |             **.NET 6.0** |        **64** |  **1,493.6 ns** |   **3.80 ns** |   **3.56 ns** | **2.95x faster** |   **0.03x** | **0.0038** |      **-** |      **40 B** |
| Encode | .NET Framework 4.7.2 |        64 |  4,413.0 ns |  42.38 ns |  39.64 ns |     baseline |         |      - |      - |      40 B |
|        |                      |           |             |           |           |              |         |        |        |           |
| Decode |             .NET 6.0 |        64 |  4,573.3 ns |  23.31 ns |  21.81 ns | 4.62x faster |   0.03x | 0.8621 | 0.0153 |   7,248 B |
| Decode | .NET Framework 4.7.2 |        64 | 21,116.0 ns |  58.55 ns |  54.77 ns |     baseline |         | 1.1292 |      - |   7,269 B |
|        |                      |           |             |           |           |              |         |        |        |           |
| **Encode** |             **.NET 6.0** |       **128** |  **1,967.1 ns** |  **11.60 ns** |  **10.85 ns** | **3.19x faster** |   **0.02x** | **0.0038** |      **-** |      **40 B** |
| Encode | .NET Framework 4.7.2 |       128 |  6,271.6 ns |  19.94 ns |  16.65 ns |     baseline |         |      - |      - |      40 B |
|        |                      |           |             |           |           |              |         |        |        |           |
| Decode |             .NET 6.0 |       128 |  5,460.1 ns |  31.64 ns |  29.59 ns | 4.52x faster |   0.03x | 1.0834 | 0.0305 |   9,112 B |
| Decode | .NET Framework 4.7.2 |       128 | 24,682.6 ns | 165.26 ns | 154.59 ns |     baseline |         | 1.4343 | 0.0305 |   9,139 B |
