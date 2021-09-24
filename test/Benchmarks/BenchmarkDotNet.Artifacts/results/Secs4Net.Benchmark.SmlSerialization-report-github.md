``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1237 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-rc.1.21458.32
  [Host]     : .NET 6.0.0 (6.0.21.45113), X64 RyuJIT
  Job-POERRH : .NET 6.0.0 (6.0.21.45113), X64 RyuJIT
  Job-QPPLMB : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|     Method |              Runtime | ItemCount |       Mean |     Error |    StdDev |        Ratio | RatioSD | Allocated |
|----------- |--------------------- |---------- |-----------:|----------:|----------:|-------------:|--------:|----------:|
|  **Serialize** |             **.NET 6.0** |         **0** |   **3.553 μs** | **0.0181 μs** | **0.0169 μs** | **2.22x faster** |   **0.01x** |      **4 KB** |
|  Serialize | .NET Framework 4.7.2 |         0 |   7.887 μs | 0.0318 μs | 0.0297 μs |     baseline |         |      6 KB |
|            |                      |           |            |           |           |              |         |           |
| Deserialze |             .NET 6.0 |         0 |   7.180 μs | 0.0200 μs | 0.0187 μs | 2.30x faster |   0.02x |      5 KB |
| Deserialze | .NET Framework 4.7.2 |         0 |  16.539 μs | 0.1063 μs | 0.0994 μs |     baseline |         |     11 KB |
|            |                      |           |            |           |           |              |         |           |
|  **Serialize** |             **.NET 6.0** |        **64** |  **64.894 μs** | **0.1998 μs** | **0.1668 μs** | **1.98x faster** |   **0.01x** |     **21 KB** |
|  Serialize | .NET Framework 4.7.2 |        64 | 128.519 μs | 0.3985 μs | 0.3533 μs |     baseline |         |     59 KB |
|            |                      |           |            |           |           |              |         |           |
| Deserialze |             .NET 6.0 |        64 |  70.699 μs | 0.2235 μs | 0.1981 μs | 3.45x faster |   0.01x |     24 KB |
| Deserialze | .NET Framework 4.7.2 |        64 | 243.781 μs | 1.0483 μs | 0.9293 μs |     baseline |         |    168 KB |
|            |                      |           |            |           |           |              |         |           |
|  **Serialize** |             **.NET 6.0** |       **128** | **130.630 μs** | **0.2227 μs** | **0.1860 μs** | **1.88x faster** |   **0.01x** |     **36 KB** |
|  Serialize | .NET Framework 4.7.2 |       128 | 245.765 μs | 0.9043 μs | 0.8459 μs |     baseline |         |    113 KB |
|            |                      |           |            |           |           |              |         |           |
| Deserialze |             .NET 6.0 |       128 | 131.970 μs | 0.8117 μs | 0.7592 μs | 3.54x faster |   0.02x |     40 KB |
| Deserialze | .NET Framework 4.7.2 |       128 | 467.460 μs | 1.8865 μs | 1.7646 μs |     baseline |         |    322 KB |
