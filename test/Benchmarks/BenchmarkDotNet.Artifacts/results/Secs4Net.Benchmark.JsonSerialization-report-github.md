``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1165 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.7.21379.14
  [Host]     : .NET 6.0.0 (6.0.21.37719), X64 RyuJIT
  Job-YOHFXW : .NET 6.0.0 (6.0.21.37719), X64 RyuJIT
  Job-LEFBHX : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|     Method |              Runtime | ItemCount |         Mean |     Error |    StdDev |        Ratio | RatioSD |   Gen 0 |  Gen 1 | Allocated |
|----------- |--------------------- |---------- |-------------:|----------:|----------:|-------------:|--------:|--------:|-------:|----------:|
|  **Serialize** |             **.NET 6.0** |         **0** |     **3.778 μs** | **0.0536 μs** | **0.0475 μs** | **2.38x faster** |   **0.03x** |  **0.1602** |      **-** |      **1 KB** |
|  Serialize | .NET Framework 4.7.2 |         0 |     8.983 μs | 0.0383 μs | 0.0339 μs |     baseline |         |  0.1984 |      - |      1 KB |
|            |                      |           |              |           |           |              |         |         |        |           |
| Deserialze |             .NET 6.0 |         0 |    23.444 μs | 0.4127 μs | 0.3860 μs | 2.44x faster |   0.04x |  1.1597 | 0.0305 |     10 KB |
| Deserialze | .NET Framework 4.7.2 |         0 |    57.151 μs | 0.2506 μs | 0.2093 μs |     baseline |         |  2.2583 |      - |     14 KB |
|            |                      |           |              |           |           |              |         |         |        |           |
|  **Serialize** |             **.NET 6.0** |        **64** |    **51.007 μs** | **0.7389 μs** | **0.6912 μs** | **3.10x faster** |   **0.05x** |  **1.8311** | **0.0610** |     **15 KB** |
|  Serialize | .NET Framework 4.7.2 |        64 |   158.378 μs | 0.6975 μs | 0.5824 μs |     baseline |         |  6.8359 | 0.2441 |     43 KB |
|            |                      |           |              |           |           |              |         |         |        |           |
| Deserialze |             .NET 6.0 |        64 |   229.052 μs | 2.4321 μs | 2.1560 μs | 2.92x faster |   0.03x |  5.1270 | 0.2441 |     42 KB |
| Deserialze | .NET Framework 4.7.2 |        64 |   667.968 μs | 3.7975 μs | 3.5522 μs |     baseline |         |  6.8359 |      - |     47 KB |
|            |                      |           |              |           |           |              |         |         |        |           |
|  **Serialize** |             **.NET 6.0** |       **128** |    **94.453 μs** | **0.3676 μs** | **0.3438 μs** | **3.21x faster** |   **0.03x** |  **3.4180** | **0.2441** |     **29 KB** |
|  Serialize | .NET Framework 4.7.2 |       128 |   303.213 μs | 2.8537 μs | 2.6693 μs |     baseline |         | 13.1836 | 0.9766 |     85 KB |
|            |                      |           |              |           |           |              |         |         |        |           |
| Deserialze |             .NET 6.0 |       128 |   426.351 μs | 8.0065 μs | 7.8635 μs | 2.98x faster |   0.06x |  8.7891 | 1.4648 |     75 KB |
| Deserialze | .NET Framework 4.7.2 |       128 | 1,272.638 μs | 4.8741 μs | 4.3207 μs |     baseline |         | 11.7188 | 1.9531 |     80 KB |
