``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.400
  [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  Job-AITDSN : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  Job-TPKEIE : .NET Framework 4.8 (4.8.4515.0), X64 RyuJIT


```
|     Method |              Runtime | ItemCount |         Mean |     Error |    StdDev |        Ratio | RatioSD | Allocated |
|----------- |--------------------- |---------- |-------------:|----------:|----------:|-------------:|--------:|----------:|
|  **Serialize** |             **.NET 6.0** |         **0** |     **3.339 μs** | **0.0144 μs** | **0.0134 μs** | **2.73x faster** |   **0.02x** |      **1 KB** |
|  Serialize | .NET Framework 4.7.2 |         0 |     9.119 μs | 0.0328 μs | 0.0307 μs |     baseline |         |      1 KB |
|            |                      |           |              |           |           |              |         |           |
| Deserialze |             .NET 6.0 |         0 |    22.787 μs | 0.1289 μs | 0.1206 μs | 2.58x faster |   0.01x |     10 KB |
| Deserialze | .NET Framework 4.7.2 |         0 |    58.814 μs | 0.1777 μs | 0.1662 μs |     baseline |         |     14 KB |
|            |                      |           |              |           |           |              |         |           |
|  **Serialize** |             **.NET 6.0** |        **64** |    **51.820 μs** | **0.1656 μs** | **0.1549 μs** | **3.12x faster** |   **0.02x** |     **15 KB** |
|  Serialize | .NET Framework 4.7.2 |        64 |   161.585 μs | 0.9502 μs | 0.8888 μs |     baseline |         |     43 KB |
|            |                      |           |              |           |           |              |         |           |
| Deserialze |             .NET 6.0 |        64 |   239.503 μs | 1.7045 μs | 1.5944 μs | 2.82x faster |   0.02x |     42 KB |
| Deserialze | .NET Framework 4.7.2 |        64 |   674.463 μs | 2.1541 μs | 1.9096 μs |     baseline |         |     47 KB |
|            |                      |           |              |           |           |              |         |           |
|  **Serialize** |             **.NET 6.0** |       **128** |    **97.684 μs** | **0.2445 μs** | **0.2287 μs** | **3.19x faster** |   **0.02x** |     **28 KB** |
|  Serialize | .NET Framework 4.7.2 |       128 |   311.806 μs | 2.0823 μs | 1.7388 μs |     baseline |         |     85 KB |
|            |                      |           |              |           |           |              |         |           |
| Deserialze |             .NET 6.0 |       128 |   444.558 μs | 2.3833 μs | 2.2293 μs | 2.89x faster |   0.02x |     75 KB |
| Deserialze | .NET Framework 4.7.2 |       128 | 1,286.624 μs | 4.2175 μs | 3.7387 μs |     baseline |         |     80 KB |
