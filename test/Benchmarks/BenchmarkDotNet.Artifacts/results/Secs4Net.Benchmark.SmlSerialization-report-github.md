``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1165 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.7.21379.14
  [Host]     : .NET 6.0.0 (6.0.21.37719), X64 RyuJIT
  Job-OMATBZ : .NET 6.0.0 (6.0.21.37719), X64 RyuJIT
  Job-NJTDJS : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|     Method |              Runtime | ItemCount |       Mean |     Error |    StdDev |        Ratio | RatioSD | Allocated |
|----------- |--------------------- |---------- |-----------:|----------:|----------:|-------------:|--------:|----------:|
|  **Serialize** |             **.NET 6.0** |         **0** |   **3.633 μs** | **0.0263 μs** | **0.0246 μs** | **2.16x faster** |   **0.02x** |      **4 KB** |
|  Serialize | .NET Framework 4.7.2 |         0 |   7.845 μs | 0.0233 μs | 0.0195 μs |     baseline |         |      6 KB |
|            |                      |           |            |           |           |              |         |           |
| Deserialze |             .NET 6.0 |         0 |   6.945 μs | 0.0245 μs | 0.0229 μs | 2.37x faster |   0.01x |      5 KB |
| Deserialze | .NET Framework 4.7.2 |         0 |  16.456 μs | 0.0433 μs | 0.0362 μs |     baseline |         |     11 KB |
|            |                      |           |            |           |           |              |         |           |
|  **Serialize** |             **.NET 6.0** |        **64** |  **68.290 μs** | **0.2615 μs** | **0.2446 μs** | **1.86x faster** |   **0.01x** |     **21 KB** |
|  Serialize | .NET Framework 4.7.2 |        64 | 127.119 μs | 0.5519 μs | 0.5162 μs |     baseline |         |     59 KB |
|            |                      |           |            |           |           |              |         |           |
| Deserialze |             .NET 6.0 |        64 |  67.921 μs | 0.3000 μs | 0.2807 μs | 3.66x faster |   0.02x |     24 KB |
| Deserialze | .NET Framework 4.7.2 |        64 | 248.513 μs | 0.7820 μs | 0.7315 μs |     baseline |         |    168 KB |
|            |                      |           |            |           |           |              |         |           |
|  **Serialize** |             **.NET 6.0** |       **128** | **130.443 μs** | **0.4391 μs** | **0.4108 μs** | **1.85x faster** |   **0.01x** |     **36 KB** |
|  Serialize | .NET Framework 4.7.2 |       128 | 241.882 μs | 0.5010 μs | 0.4441 μs |     baseline |         |    113 KB |
|            |                      |           |            |           |           |              |         |           |
| Deserialze |             .NET 6.0 |       128 | 125.342 μs | 0.3631 μs | 0.3396 μs | 3.75x faster |   0.01x |     40 KB |
| Deserialze | .NET Framework 4.7.2 |       128 | 469.671 μs | 1.1319 μs | 1.0588 μs |     baseline |         |    322 KB |
