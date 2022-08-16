``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.400
  [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  Job-AITDSN : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  Job-TPKEIE : .NET Framework 4.8 (4.8.4515.0), X64 RyuJIT


```
|     Method |              Runtime | ItemCount |       Mean |     Error |    StdDev |        Ratio | RatioSD | Allocated |
|----------- |--------------------- |---------- |-----------:|----------:|----------:|-------------:|--------:|----------:|
|  **Serialize** |             **.NET 6.0** |         **0** |   **3.785 μs** | **0.0178 μs** | **0.0166 μs** | **2.16x faster** |   **0.01x** |      **4 KB** |
|  Serialize | .NET Framework 4.7.2 |         0 |   8.185 μs | 0.0462 μs | 0.0432 μs |     baseline |         |      6 KB |
|            |                      |           |            |           |           |              |         |           |
| Deserialze |             .NET 6.0 |         0 |   7.030 μs | 0.0363 μs | 0.0340 μs | 2.48x faster |   0.01x |      5 KB |
| Deserialze | .NET Framework 4.7.2 |         0 |  17.444 μs | 0.0966 μs | 0.0856 μs |     baseline |         |     11 KB |
|            |                      |           |            |           |           |              |         |           |
|  **Serialize** |             **.NET 6.0** |        **64** |  **66.964 μs** | **0.3351 μs** | **0.2798 μs** | **1.95x faster** |   **0.01x** |     **21 KB** |
|  Serialize | .NET Framework 4.7.2 |        64 | 130.423 μs | 0.6598 μs | 0.6172 μs |     baseline |         |     59 KB |
|            |                      |           |            |           |           |              |         |           |
| Deserialze |             .NET 6.0 |        64 |  69.143 μs | 0.2714 μs | 0.2538 μs | 3.68x faster |   0.02x |     24 KB |
| Deserialze | .NET Framework 4.7.2 |        64 | 254.638 μs | 0.8899 μs | 0.7431 μs |     baseline |         |    168 KB |
|            |                      |           |            |           |           |              |         |           |
|  **Serialize** |             **.NET 6.0** |       **128** | **127.863 μs** | **0.6187 μs** | **0.5166 μs** | **1.95x faster** |   **0.01x** |     **36 KB** |
|  Serialize | .NET Framework 4.7.2 |       128 | 249.088 μs | 1.0951 μs | 0.9708 μs |     baseline |         |    113 KB |
|            |                      |           |            |           |           |              |         |           |
| Deserialze |             .NET 6.0 |       128 | 130.828 μs | 0.5640 μs | 0.4710 μs | 3.73x faster |   0.02x |     40 KB |
| Deserialze | .NET Framework 4.7.2 |       128 | 487.841 μs | 1.7927 μs | 1.6769 μs |     baseline |         |    322 KB |
