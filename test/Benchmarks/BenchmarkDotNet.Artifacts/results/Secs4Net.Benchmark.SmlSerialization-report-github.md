``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1151 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.6.21355.2
  [Host]     : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-PSNALV : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-DJMMZE : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|     Method |              Runtime | ItemCount |       Mean |     Error |    StdDev |        Ratio | RatioSD |   Gen 0 |  Gen 1 | Gen 2 | Allocated |
|----------- |--------------------- |---------- |-----------:|----------:|----------:|-------------:|--------:|--------:|-------:|------:|----------:|
|  **Serialize** |             **.NET 6.0** |         **0** |   **5.628 μs** | **0.0426 μs** | **0.0378 μs** |     **baseline** |        **** |  **0.5569** |      **-** |     **-** |      **5 KB** |
|  Serialize | .NET Framework 4.7.2 |         0 |   8.130 μs | 0.0545 μs | 0.0510 μs | 1.45x slower |   0.01x |  0.9918 |      - |     - |      6 KB |
|            |                      |           |            |           |           |              |         |         |        |       |           |
| Deserialze |             .NET 6.0 |         0 |   7.061 μs | 0.1029 μs | 0.0963 μs |     baseline |         |  0.6409 |      - |     - |      5 KB |
| Deserialze | .NET Framework 4.7.2 |         0 |  16.690 μs | 0.1864 μs | 0.1557 μs | 2.37x slower |   0.04x |  1.7395 |      - |     - |     11 KB |
|            |                      |           |            |           |           |              |         |         |        |       |           |
|  **Serialize** |             **.NET 6.0** |        **64** |  **77.613 μs** | **0.3939 μs** | **0.3685 μs** |     **baseline** |        **** |  **2.4414** | **0.1221** |     **-** |     **21 KB** |
|  Serialize | .NET Framework 4.7.2 |        64 | 129.565 μs | 0.6838 μs | 0.6396 μs | 1.67x slower |   0.01x |  9.5215 | 0.4883 |     - |     59 KB |
|            |                      |           |            |           |           |              |         |         |        |       |           |
| Deserialze |             .NET 6.0 |        64 |  79.663 μs | 0.3746 μs | 0.3321 μs |     baseline |         |  2.8076 | 0.1221 |     - |     24 KB |
| Deserialze | .NET Framework 4.7.2 |        64 | 245.227 μs | 1.1228 μs | 1.0503 μs | 3.08x slower |   0.02x | 26.8555 | 0.9766 |     - |    168 KB |
|            |                      |           |            |           |           |              |         |         |        |       |           |
|  **Serialize** |             **.NET 6.0** |       **128** | **149.900 μs** | **0.7013 μs** | **0.6560 μs** |     **baseline** |        **** |  **4.3945** | **0.4883** |     **-** |     **36 KB** |
|  Serialize | .NET Framework 4.7.2 |       128 | 248.287 μs | 2.4548 μs | 2.2962 μs | 1.66x slower |   0.02x | 18.0664 | 1.4648 |     - |    113 KB |
|            |                      |           |            |           |           |              |         |         |        |       |           |
| Deserialze |             .NET 6.0 |       128 | 149.488 μs | 1.1206 μs | 1.0482 μs |     baseline |         |  4.8828 | 0.2441 |     - |     40 KB |
| Deserialze | .NET Framework 4.7.2 |       128 | 475.407 μs | 7.0717 μs | 6.6149 μs | 3.18x slower |   0.05x | 52.2461 | 4.3945 |     - |    322 KB |
