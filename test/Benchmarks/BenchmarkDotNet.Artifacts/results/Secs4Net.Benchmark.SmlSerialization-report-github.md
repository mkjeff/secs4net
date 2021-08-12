``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1165 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.7.21379.14
  [Host]     : .NET 6.0.0 (6.0.21.37719), X64 RyuJIT
  Job-GVDIFM : .NET 6.0.0 (6.0.21.37719), X64 RyuJIT
  Job-WCNVCL : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|     Method |              Runtime | ItemCount |       Mean |     Error |    StdDev |        Ratio | RatioSD |   Gen 0 |  Gen 1 | Allocated |
|----------- |--------------------- |---------- |-----------:|----------:|----------:|-------------:|--------:|--------:|-------:|----------:|
|  **Serialize** |             **.NET 6.0** |         **0** |   **4.126 μs** | **0.0690 μs** | **0.0646 μs** | **1.96x faster** |   **0.03x** |  **0.5569** |      **-** |      **5 KB** |
|  Serialize | .NET Framework 4.7.2 |         0 |   8.083 μs | 0.0435 μs | 0.0407 μs |     baseline |         |  0.9918 |      - |      6 KB |
|            |                      |           |            |           |           |              |         |         |        |           |
| Deserialze |             .NET 6.0 |         0 |   6.955 μs | 0.0495 μs | 0.0463 μs | 2.37x faster |   0.02x |  0.6409 |      - |      5 KB |
| Deserialze | .NET Framework 4.7.2 |         0 |  16.497 μs | 0.0792 μs | 0.0702 μs |     baseline |         |  1.7395 |      - |     11 KB |
|            |                      |           |            |           |           |              |         |         |        |           |
|  **Serialize** |             **.NET 6.0** |        **64** |  **68.006 μs** | **0.1664 μs** | **0.1475 μs** | **1.88x faster** |   **0.02x** |  **2.4414** | **0.1221** |     **21 KB** |
|  Serialize | .NET Framework 4.7.2 |        64 | 127.723 μs | 1.1151 μs | 1.0431 μs |     baseline |         |  9.5215 | 0.4883 |     59 KB |
|            |                      |           |            |           |           |              |         |         |        |           |
| Deserialze |             .NET 6.0 |        64 |  69.235 μs | 0.5029 μs | 0.4704 μs | 3.48x faster |   0.03x |  2.8076 | 0.1221 |     24 KB |
| Deserialze | .NET Framework 4.7.2 |        64 | 241.006 μs | 0.8707 μs | 0.7271 μs |     baseline |         | 27.0996 | 1.4648 |    168 KB |
|            |                      |           |            |           |           |              |         |         |        |           |
|  **Serialize** |             **.NET 6.0** |       **128** | **124.871 μs** | **0.9085 μs** | **0.8498 μs** | **1.94x faster** |   **0.02x** |  **4.3945** | **0.4883** |     **36 KB** |
|  Serialize | .NET Framework 4.7.2 |       128 | 242.348 μs | 0.5571 μs | 0.4652 μs |     baseline |         | 18.3105 | 1.4648 |    113 KB |
|            |                      |           |            |           |           |              |         |         |        |           |
| Deserialze |             .NET 6.0 |       128 | 129.625 μs | 1.4124 μs | 1.3211 μs | 3.58x faster |   0.04x |  4.8828 | 0.2441 |     40 KB |
| Deserialze | .NET Framework 4.7.2 |       128 | 464.634 μs | 1.1838 μs | 1.0494 μs |     baseline |         | 52.2461 | 4.3945 |    322 KB |
