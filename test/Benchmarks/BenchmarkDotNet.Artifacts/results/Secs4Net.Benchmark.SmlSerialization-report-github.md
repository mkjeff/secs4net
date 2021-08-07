``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1151 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.6.21355.2
  [Host]     : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-TQNDQR : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-TKTIJP : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|     Method |              Runtime | ItemCount |       Mean |     Error |    StdDev |        Ratio | RatioSD |   Gen 0 |  Gen 1 | Gen 2 | Allocated |
|----------- |--------------------- |---------- |-----------:|----------:|----------:|-------------:|--------:|--------:|-------:|------:|----------:|
|  **Serialize** |             **.NET 6.0** |         **0** |   **4.792 μs** | **0.0173 μs** | **0.0162 μs** | **1.59x faster** |   **0.01x** |  **0.5493** |      **-** |     **-** |      **5 KB** |
|  Serialize | .NET Framework 4.7.2 |         0 |   7.602 μs | 0.0210 μs | 0.0175 μs |     baseline |         |  0.9766 |      - |     - |      6 KB |
|            |                      |           |            |           |           |              |         |         |        |       |           |
| Deserialze |             .NET 6.0 |         0 |   6.602 μs | 0.0379 μs | 0.0355 μs | 2.40x faster |   0.02x |  0.6256 |      - |     - |      5 KB |
| Deserialze | .NET Framework 4.7.2 |         0 |  15.842 μs | 0.0690 μs | 0.0645 μs |     baseline |         |  1.7090 |      - |     - |     11 KB |
|            |                      |           |            |           |           |              |         |         |        |       |           |
|  **Serialize** |             **.NET 6.0** |        **64** |  **77.053 μs** | **0.5625 μs** | **0.4987 μs** | **1.66x faster** |   **0.02x** |  **2.4414** | **0.1221** |     **-** |     **21 KB** |
|  Serialize | .NET Framework 4.7.2 |        64 | 127.922 μs | 0.7411 μs | 0.6932 μs |     baseline |         |  9.5215 | 0.4883 |     - |     59 KB |
|            |                      |           |            |           |           |              |         |         |        |       |           |
| Deserialze |             .NET 6.0 |        64 |  77.230 μs | 0.3546 μs | 0.3317 μs | 3.12x faster |   0.01x |  2.8076 | 0.1221 |     - |     23 KB |
| Deserialze | .NET Framework 4.7.2 |        64 | 240.496 μs | 1.0023 μs | 0.8885 μs |     baseline |         | 26.8555 | 0.9766 |     - |    168 KB |
|            |                      |           |            |           |           |              |         |         |        |       |           |
|  **Serialize** |             **.NET 6.0** |       **128** | **148.082 μs** | **0.8049 μs** | **0.7529 μs** | **1.64x faster** |   **0.01x** |  **4.3945** | **0.2441** |     **-** |     **36 KB** |
|  Serialize | .NET Framework 4.7.2 |       128 | 242.966 μs | 0.8488 μs | 0.7939 μs |     baseline |         | 18.3105 | 1.7090 |     - |    113 KB |
|            |                      |           |            |           |           |              |         |         |        |       |           |
| Deserialze |             .NET 6.0 |       128 | 146.461 μs | 0.6002 μs | 0.5614 μs | 3.15x faster |   0.01x |  4.6387 | 0.2441 |     - |     40 KB |
| Deserialze | .NET Framework 4.7.2 |       128 | 462.049 μs | 0.9924 μs | 0.8798 μs |     baseline |         | 51.7578 | 3.9063 |     - |    321 KB |
