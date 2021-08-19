``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1165 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.7.21379.14
  [Host]     : .NET 6.0.0 (6.0.21.37719), X64 RyuJIT
  Job-OMATBZ : .NET 6.0.0 (6.0.21.37719), X64 RyuJIT
  Job-NJTDJS : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
| Method |              Runtime | ItemCount |        Mean |    Error |   StdDev |        Ratio | RatioSD | Allocated |
|------- |--------------------- |---------- |------------:|---------:|---------:|-------------:|--------:|----------:|
| **Encode** |             **.NET 6.0** |         **0** |    **434.1 ns** |  **5.45 ns** |  **5.10 ns** | **2.84x faster** |   **0.04x** |      **40 B** |
| Encode | .NET Framework 4.7.2 |         0 |  1,232.7 ns |  6.26 ns |  5.86 ns |     baseline |         |      40 B |
|        |                      |           |             |          |          |              |         |           |
| Decode |             .NET 6.0 |         0 |  2,543.0 ns | 16.02 ns | 14.99 ns | 2.43x faster |   0.02x |     560 B |
| Decode | .NET Framework 4.7.2 |         0 |  6,174.9 ns | 17.14 ns | 16.03 ns |     baseline |         |     562 B |
|        |                      |           |             |          |          |              |         |           |
| **Encode** |             **.NET 6.0** |        **64** |  **1,420.9 ns** |  **7.86 ns** |  **6.57 ns** | **3.19x faster** |   **0.03x** |      **40 B** |
| Encode | .NET Framework 4.7.2 |        64 |  4,522.0 ns | 37.38 ns | 34.97 ns |     baseline |         |      40 B |
|        |                      |           |             |          |          |              |         |           |
| Decode |             .NET 6.0 |        64 |  4,362.1 ns | 36.58 ns | 34.22 ns | 4.82x faster |   0.04x |   7,248 B |
| Decode | .NET Framework 4.7.2 |        64 | 21,060.9 ns | 37.31 ns | 29.13 ns |     baseline |         |   7,269 B |
|        |                      |           |             |          |          |              |         |           |
| **Encode** |             **.NET 6.0** |       **128** |  **2,024.5 ns** | **12.94 ns** | **12.11 ns** | **3.12x faster** |   **0.02x** |      **40 B** |
| Encode | .NET Framework 4.7.2 |       128 |  6,332.0 ns | 11.93 ns |  9.97 ns |     baseline |         |      40 B |
|        |                      |           |             |          |          |              |         |           |
| Decode |             .NET 6.0 |       128 |  5,593.0 ns | 29.85 ns | 27.92 ns | 4.45x faster |   0.03x |   9,112 B |
| Decode | .NET Framework 4.7.2 |       128 | 24,869.4 ns | 71.79 ns | 59.95 ns |     baseline |         |   9,139 B |
