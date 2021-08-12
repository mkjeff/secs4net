``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1165 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.7.21379.14
  [Host]     : .NET 6.0.0 (6.0.21.37719), X64 RyuJIT
  Job-GVDIFM : .NET 6.0.0 (6.0.21.37719), X64 RyuJIT
  Job-WCNVCL : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|     Method |              Runtime | ItemCount |         Mean |     Error |    StdDev |        Ratio | RatioSD |   Gen 0 |  Gen 1 | Allocated |
|----------- |--------------------- |---------- |-------------:|----------:|----------:|-------------:|--------:|--------:|-------:|----------:|
|  **Serialize** |             **.NET 6.0** |         **0** |     **3.827 μs** | **0.0170 μs** | **0.0159 μs** | **2.35x faster** |   **0.01x** |  **0.1602** |      **-** |      **1 KB** |
|  Serialize | .NET Framework 4.7.2 |         0 |     8.984 μs | 0.0198 μs | 0.0185 μs |     baseline |         |  0.1984 |      - |      1 KB |
|            |                      |           |              |           |           |              |         |         |        |           |
| Deserialze |             .NET 6.0 |         0 |    23.618 μs | 0.1331 μs | 0.1180 μs | 2.19x faster |   0.01x |  1.6785 | 0.0305 |     14 KB |
| Deserialze | .NET Framework 4.7.2 |         0 |    51.818 μs | 0.1244 μs | 0.1163 μs |     baseline |         |  2.3804 | 0.0610 |     15 KB |
|            |                      |           |              |           |           |              |         |         |        |           |
|  **Serialize** |             **.NET 6.0** |        **64** |    **52.864 μs** | **0.2832 μs** | **0.2649 μs** | **3.02x faster** |   **0.03x** |  **1.8311** | **0.0610** |     **15 KB** |
|  Serialize | .NET Framework 4.7.2 |        64 |   159.411 μs | 1.4403 μs | 1.3472 μs |     baseline |         |  6.8359 | 0.2441 |     43 KB |
|            |                      |           |              |           |           |              |         |         |        |           |
| Deserialze |             .NET 6.0 |        64 |   240.976 μs | 0.8710 μs | 0.8147 μs | 2.90x faster |   0.02x |  6.8359 | 0.7324 |     57 KB |
| Deserialze | .NET Framework 4.7.2 |        64 |   697.816 μs | 2.2498 μs | 2.1044 μs |     baseline |         |  7.8125 |      - |     55 KB |
|            |                      |           |              |           |           |              |         |         |        |           |
|  **Serialize** |             **.NET 6.0** |       **128** |   **103.275 μs** | **0.2875 μs** | **0.2689 μs** | **2.96x faster** |   **0.05x** |  **3.4180** | **0.2441** |     **29 KB** |
|  Serialize | .NET Framework 4.7.2 |       128 |   305.192 μs | 5.0167 μs | 4.6926 μs |     baseline |         | 13.1836 | 0.9766 |     85 KB |
|            |                      |           |              |           |           |              |         |         |        |           |
| Deserialze |             .NET 6.0 |       128 |   479.483 μs | 1.4148 μs | 1.3234 μs | 2.79x faster |   0.01x | 11.2305 | 1.4648 |     95 KB |
| Deserialze | .NET Framework 4.7.2 |       128 | 1,337.176 μs | 4.3680 μs | 3.8721 μs |     baseline |         | 13.6719 | 1.9531 |     93 KB |
