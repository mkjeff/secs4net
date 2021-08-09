``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1151 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.6.21355.2
  [Host]     : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-PSNALV : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-DJMMZE : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|     Method |              Runtime | ItemCount |         Mean |     Error |    StdDev |        Ratio | RatioSD |   Gen 0 |  Gen 1 | Gen 2 | Allocated |
|----------- |--------------------- |---------- |-------------:|----------:|----------:|-------------:|--------:|--------:|-------:|------:|----------:|
|  **Serialize** |             **.NET 6.0** |         **0** |     **3.920 μs** | **0.0239 μs** | **0.0224 μs** |     **baseline** |        **** |  **0.1602** |      **-** |     **-** |      **1 KB** |
|  Serialize | .NET Framework 4.7.2 |         0 |     8.952 μs | 0.0186 μs | 0.0174 μs | 2.28x slower |   0.01x |  0.1984 |      - |     - |      1 KB |
|            |                      |           |              |           |           |              |         |         |        |       |           |
| Deserialze |             .NET 6.0 |         0 |    31.303 μs | 0.0907 μs | 0.0804 μs |     baseline |         |  1.6479 |      - |     - |     14 KB |
| Deserialze | .NET Framework 4.7.2 |         0 |    51.188 μs | 0.1012 μs | 0.0845 μs | 1.64x slower |   0.01x |  2.3804 | 0.0610 |     - |     15 KB |
|            |                      |           |              |           |           |              |         |         |        |       |           |
|  **Serialize** |             **.NET 6.0** |        **64** |    **53.647 μs** | **0.2550 μs** | **0.2385 μs** |     **baseline** |        **** |  **1.8311** | **0.0610** |     **-** |     **15 KB** |
|  Serialize | .NET Framework 4.7.2 |        64 |   158.213 μs | 0.3556 μs | 0.3152 μs | 2.95x slower |   0.02x |  6.8359 | 0.2441 |     - |     43 KB |
|            |                      |           |              |           |           |              |         |         |        |       |           |
| Deserialze |             .NET 6.0 |        64 |   278.280 μs | 0.7490 μs | 0.7006 μs |     baseline |         |  6.8359 | 0.4883 |     - |     57 KB |
| Deserialze | .NET Framework 4.7.2 |        64 |   700.646 μs | 1.4928 μs | 1.3964 μs | 2.52x slower |   0.01x |  7.8125 | 0.9766 |     - |     55 KB |
|            |                      |           |              |           |           |              |         |         |        |       |           |
|  **Serialize** |             **.NET 6.0** |       **128** |   **100.739 μs** | **0.3277 μs** | **0.2736 μs** |     **baseline** |        **** |  **3.4180** | **0.2441** |     **-** |     **29 KB** |
|  Serialize | .NET Framework 4.7.2 |       128 |   297.155 μs | 1.4930 μs | 1.3966 μs | 2.95x slower |   0.01x | 13.1836 | 0.9766 |     - |     85 KB |
|            |                      |           |              |           |           |              |         |         |        |       |           |
| Deserialze |             .NET 6.0 |       128 |   532.220 μs | 1.8243 μs | 1.7064 μs |     baseline |         | 10.7422 | 0.9766 |     - |     95 KB |
| Deserialze | .NET Framework 4.7.2 |       128 | 1,341.564 μs | 2.0809 μs | 1.8447 μs | 2.52x slower |   0.01x | 13.6719 | 1.9531 |     - |     93 KB |
