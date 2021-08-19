``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1165 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.7.21379.14
  [Host]     : .NET 6.0.0 (6.0.21.37719), X64 RyuJIT
  Job-OMATBZ : .NET 6.0.0 (6.0.21.37719), X64 RyuJIT
  Job-NJTDJS : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|     Method |              Runtime | ItemCount |         Mean |     Error |    StdDev |        Ratio | RatioSD | Allocated |
|----------- |--------------------- |---------- |-------------:|----------:|----------:|-------------:|--------:|----------:|
|  **Serialize** |             **.NET 6.0** |         **0** |     **3.355 μs** | **0.0177 μs** | **0.0165 μs** | **2.61x faster** |   **0.01x** |      **1 KB** |
|  Serialize | .NET Framework 4.7.2 |         0 |     8.761 μs | 0.0211 μs | 0.0197 μs |     baseline |         |      1 KB |
|            |                      |           |              |           |           |              |         |           |
| Deserialze |             .NET 6.0 |         0 |    22.636 μs | 0.1025 μs | 0.0959 μs | 2.61x faster |   0.01x |     10 KB |
| Deserialze | .NET Framework 4.7.2 |         0 |    59.076 μs | 0.1326 μs | 0.1241 μs |     baseline |         |     14 KB |
|            |                      |           |              |           |           |              |         |           |
|  **Serialize** |             **.NET 6.0** |        **64** |    **50.165 μs** | **0.2769 μs** | **0.2590 μs** | **3.07x faster** |   **0.02x** |     **15 KB** |
|  Serialize | .NET Framework 4.7.2 |        64 |   153.939 μs | 0.3581 μs | 0.3175 μs |     baseline |         |     43 KB |
|            |                      |           |              |           |           |              |         |           |
| Deserialze |             .NET 6.0 |        64 |   225.621 μs | 0.8368 μs | 0.7828 μs | 2.94x faster |   0.01x |     42 KB |
| Deserialze | .NET Framework 4.7.2 |        64 |   663.412 μs | 2.0785 μs | 1.9442 μs |     baseline |         |     47 KB |
|            |                      |           |              |           |           |              |         |           |
|  **Serialize** |             **.NET 6.0** |       **128** |    **97.766 μs** | **0.4391 μs** | **0.4107 μs** | **3.03x faster** |   **0.02x** |     **28 KB** |
|  Serialize | .NET Framework 4.7.2 |       128 |   295.752 μs | 1.0816 μs | 1.0118 μs |     baseline |         |     85 KB |
|            |                      |           |              |           |           |              |         |           |
| Deserialze |             .NET 6.0 |       128 |   427.027 μs | 2.6000 μs | 2.4320 μs | 2.96x faster |   0.02x |     75 KB |
| Deserialze | .NET Framework 4.7.2 |       128 | 1,262.879 μs | 2.5919 μs | 2.1644 μs |     baseline |         |     80 KB |
