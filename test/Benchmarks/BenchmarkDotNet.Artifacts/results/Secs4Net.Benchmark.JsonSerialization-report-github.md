``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1237 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-rc.1.21458.32
  [Host]     : .NET 6.0.0 (6.0.21.45113), X64 RyuJIT
  Job-POERRH : .NET 6.0.0 (6.0.21.45113), X64 RyuJIT
  Job-QPPLMB : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|     Method |              Runtime | ItemCount |         Mean |     Error |    StdDev |        Ratio | RatioSD | Allocated |
|----------- |--------------------- |---------- |-------------:|----------:|----------:|-------------:|--------:|----------:|
|  **Serialize** |             **.NET 6.0** |         **0** |     **3.369 μs** | **0.0093 μs** | **0.0077 μs** | **2.57x faster** |   **0.01x** |      **1 KB** |
|  Serialize | .NET Framework 4.7.2 |         0 |     8.671 μs | 0.0275 μs | 0.0257 μs |     baseline |         |      1 KB |
|            |                      |           |              |           |           |              |         |           |
| Deserialze |             .NET 6.0 |         0 |    22.146 μs | 0.0711 μs | 0.0665 μs | 2.61x faster |   0.01x |     10 KB |
| Deserialze | .NET Framework 4.7.2 |         0 |    57.794 μs | 0.1199 μs | 0.1121 μs |     baseline |         |     14 KB |
|            |                      |           |              |           |           |              |         |           |
|  **Serialize** |             **.NET 6.0** |        **64** |    **50.667 μs** | **0.2053 μs** | **0.1820 μs** | **3.12x faster** |   **0.02x** |     **15 KB** |
|  Serialize | .NET Framework 4.7.2 |        64 |   158.250 μs | 0.6176 μs | 0.5777 μs |     baseline |         |     43 KB |
|            |                      |           |              |           |           |              |         |           |
| Deserialze |             .NET 6.0 |        64 |   226.194 μs | 0.9810 μs | 0.9176 μs | 2.97x faster |   0.01x |     42 KB |
| Deserialze | .NET Framework 4.7.2 |        64 |   671.714 μs | 1.7642 μs | 1.5639 μs |     baseline |         |     47 KB |
|            |                      |           |              |           |           |              |         |           |
|  **Serialize** |             **.NET 6.0** |       **128** |    **95.185 μs** | **0.3665 μs** | **0.3249 μs** | **3.20x faster** |   **0.02x** |     **28 KB** |
|  Serialize | .NET Framework 4.7.2 |       128 |   304.237 μs | 1.7343 μs | 1.5374 μs |     baseline |         |     85 KB |
|            |                      |           |              |           |           |              |         |           |
| Deserialze |             .NET 6.0 |       128 |   440.323 μs | 1.5098 μs | 1.2608 μs | 2.92x faster |   0.01x |     75 KB |
| Deserialze | .NET Framework 4.7.2 |       128 | 1,285.136 μs | 4.3349 μs | 3.8428 μs |     baseline |         |     80 KB |
