``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1151 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.6.21355.2
  [Host]     : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-GAZESS : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-HKKFQD : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
| Method |              Runtime | ItemCount |        Mean |    Error |   StdDev |        Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|------- |--------------------- |---------- |------------:|---------:|---------:|-------------:|--------:|-------:|-------:|------:|----------:|
| **Encode** |             **.NET 6.0** |         **0** |    **513.2 ns** |  **3.96 ns** |  **3.31 ns** | **3.61x faster** |   **0.03x** | **0.0048** |      **-** |     **-** |      **40 B** |
| Encode | .NET Framework 4.7.2 |         0 |  1,855.8 ns |  5.63 ns |  5.27 ns |     baseline |         | 0.0057 |      - |     - |      40 B |
|        |                      |           |             |          |          |              |         |        |        |       |           |
| Decode |             .NET 6.0 |         0 |  3,184.2 ns | 15.97 ns | 14.15 ns | 1.97x faster |   0.01x | 0.0648 |      - |     - |     560 B |
| Decode | .NET Framework 4.7.2 |         0 |  6,278.8 ns | 15.69 ns | 13.10 ns |     baseline |         | 0.0839 |      - |     - |     562 B |
|        |                      |           |             |          |          |              |         |        |        |       |           |
| **Encode** |             **.NET 6.0** |        **64** |  **1,637.6 ns** |  **6.35 ns** |  **5.94 ns** | **3.35x faster** |   **0.02x** | **0.0038** |      **-** |     **-** |      **40 B** |
| Encode | .NET Framework 4.7.2 |        64 |  5,484.2 ns | 13.54 ns | 12.67 ns |     baseline |         |      - |      - |     - |      40 B |
|        |                      |           |             |          |          |              |         |        |        |       |           |
| Decode |             .NET 6.0 |        64 |  5,587.1 ns | 20.64 ns | 18.30 ns | 2.08x faster |   0.01x | 0.8469 | 0.0153 |     - |   7,088 B |
| Decode | .NET Framework 4.7.2 |        64 | 11,619.8 ns | 30.76 ns | 28.77 ns |     baseline |         | 1.1444 | 0.0153 |     - |   7,269 B |
|        |                      |           |             |          |          |              |         |        |        |       |           |
| **Encode** |             **.NET 6.0** |       **128** |  **2,261.0 ns** |  **7.35 ns** |  **6.51 ns** | **3.16x faster** |   **0.01x** | **0.0038** |      **-** |     **-** |      **40 B** |
| Encode | .NET Framework 4.7.2 |       128 |  7,151.0 ns | 16.53 ns | 15.46 ns |     baseline |         |      - |      - |     - |      40 B |
|        |                      |           |             |          |          |              |         |        |        |       |           |
| Decode |             .NET 6.0 |       128 |  6,577.2 ns | 24.68 ns | 23.08 ns | 2.08x faster |   0.01x | 1.0681 | 0.0305 |     - |   8,952 B |
| Decode | .NET Framework 4.7.2 |       128 | 13,700.5 ns | 37.44 ns | 35.03 ns |     baseline |         | 1.4343 | 0.0305 |     - |   9,139 B |
