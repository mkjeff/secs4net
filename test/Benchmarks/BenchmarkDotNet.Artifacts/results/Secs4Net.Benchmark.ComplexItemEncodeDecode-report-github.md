``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1237 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-rc.1.21458.32
  [Host]     : .NET 6.0.0 (6.0.21.45113), X64 RyuJIT
  Job-POERRH : .NET 6.0.0 (6.0.21.45113), X64 RyuJIT
  Job-QPPLMB : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
| Method |              Runtime | ItemCount |        Mean |     Error |    StdDev |        Ratio | RatioSD | Allocated |
|------- |--------------------- |---------- |------------:|----------:|----------:|-------------:|--------:|----------:|
| **Encode** |             **.NET 6.0** |         **0** |    **445.6 ns** |   **1.72 ns** |   **1.61 ns** | **2.75x faster** |   **0.01x** |      **40 B** |
| Encode | .NET Framework 4.7.2 |         0 |  1,224.6 ns |   2.71 ns |   2.53 ns |     baseline |         |      40 B |
|        |                      |           |             |           |           |              |         |           |
| Decode |             .NET 6.0 |         0 |  2,657.4 ns |   7.48 ns |   6.99 ns | 2.36x faster |   0.01x |     560 B |
| Decode | .NET Framework 4.7.2 |         0 |  6,271.4 ns |  14.94 ns |  13.97 ns |     baseline |         |     562 B |
|        |                      |           |             |           |           |              |         |           |
| **Encode** |             **.NET 6.0** |        **64** |  **1,397.5 ns** |   **7.45 ns** |   **6.60 ns** | **3.08x faster** |   **0.02x** |      **40 B** |
| Encode | .NET Framework 4.7.2 |        64 |  4,297.7 ns |  16.18 ns |  14.34 ns |     baseline |         |      40 B |
|        |                      |           |             |           |           |              |         |           |
| Decode |             .NET 6.0 |        64 |  4,299.2 ns |  22.76 ns |  20.18 ns | 4.99x faster |   0.02x |   7,248 B |
| Decode | .NET Framework 4.7.2 |        64 | 21,450.6 ns |  60.60 ns |  56.68 ns |     baseline |         |   7,269 B |
|        |                      |           |             |           |           |              |         |           |
| **Encode** |             **.NET 6.0** |       **128** |  **2,052.0 ns** |   **5.79 ns** |   **5.14 ns** | **3.05x faster** |   **0.01x** |      **40 B** |
| Encode | .NET Framework 4.7.2 |       128 |  6,248.7 ns |  16.89 ns |  14.98 ns |     baseline |         |      40 B |
|        |                      |           |             |           |           |              |         |           |
| Decode |             .NET 6.0 |       128 |  5,528.7 ns |  47.70 ns |  44.62 ns | 4.51x faster |   0.04x |   9,112 B |
| Decode | .NET Framework 4.7.2 |       128 | 24,889.5 ns | 120.12 ns | 106.48 ns |     baseline |         |   9,139 B |
