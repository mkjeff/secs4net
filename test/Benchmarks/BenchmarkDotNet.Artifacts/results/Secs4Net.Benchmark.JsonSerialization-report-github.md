``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1151 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.6.21355.2
  [Host]     : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-ZOWWTU : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-HGHVYG : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|     Method |              Runtime | ItemCount |         Mean |     Error |    StdDev |        Ratio | RatioSD |   Gen 0 |  Gen 1 | Gen 2 | Allocated |
|----------- |--------------------- |---------- |-------------:|----------:|----------:|-------------:|--------:|--------:|-------:|------:|----------:|
|  **Serialize** |             **.NET 6.0** |         **0** |     **3.660 μs** | **0.0112 μs** | **0.0099 μs** | **2.32x faster** |   **0.01x** |  **0.1564** |      **-** |     **-** |      **1 KB** |
|  Serialize | .NET Framework 4.7.2 |         0 |     8.502 μs | 0.0157 μs | 0.0147 μs |     baseline |         |  0.1984 |      - |     - |      1 KB |
|            |                      |           |              |           |           |              |         |         |        |       |           |
| Deserialze |             .NET 6.0 |         0 |    30.253 μs | 0.0864 μs | 0.0808 μs | 1.65x faster |   0.01x |  1.5259 |      - |     - |     13 KB |
| Deserialze | .NET Framework 4.7.2 |         0 |    49.766 μs | 0.0671 μs | 0.0594 μs |     baseline |         |  2.1973 |      - |     - |     14 KB |
|            |                      |           |              |           |           |              |         |         |        |       |           |
|  **Serialize** |             **.NET 6.0** |        **64** |    **51.170 μs** | **0.2344 μs** | **0.2078 μs** | **3.03x faster** |   **0.01x** |  **1.7700** | **0.0610** |     **-** |     **15 KB** |
|  Serialize | .NET Framework 4.7.2 |        64 |   154.813 μs | 0.3764 μs | 0.3521 μs |     baseline |         |  6.8359 | 0.2441 |     - |     43 KB |
|            |                      |           |              |           |           |              |         |         |        |       |           |
| Deserialze |             .NET 6.0 |        64 |   277.306 μs | 1.0377 μs | 0.9199 μs | 2.46x faster |   0.01x |  6.8359 | 0.4883 |     - |     56 KB |
| Deserialze | .NET Framework 4.7.2 |        64 |   682.785 μs | 2.3670 μs | 2.0983 μs |     baseline |         |  7.8125 | 0.9766 |     - |     54 KB |
|            |                      |           |              |           |           |              |         |         |        |       |           |
|  **Serialize** |             **.NET 6.0** |       **128** |    **95.237 μs** | **0.2726 μs** | **0.2550 μs** | **3.12x faster** |   **0.01x** |  **3.2959** | **0.2441** |     **-** |     **27 KB** |
|  Serialize | .NET Framework 4.7.2 |       128 |   297.279 μs | 0.6716 μs | 0.6283 μs |     baseline |         | 13.1836 | 0.9766 |     - |     83 KB |
|            |                      |           |              |           |           |              |         |         |        |       |           |
| Deserialze |             .NET 6.0 |       128 |   524.833 μs | 1.6346 μs | 1.3650 μs | 2.49x faster |   0.01x | 10.7422 | 0.9766 |     - |     94 KB |
| Deserialze | .NET Framework 4.7.2 |       128 | 1,310.244 μs | 5.5499 μs | 5.1914 μs |     baseline |         | 13.6719 | 1.9531 |     - |     92 KB |
