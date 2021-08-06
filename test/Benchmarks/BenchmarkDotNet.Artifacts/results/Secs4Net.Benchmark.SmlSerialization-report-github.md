``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1151 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.6.21355.2
  [Host]     : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-GFMGBH : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-BGFIPZ : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|     Method |              Runtime |      Mean |     Error |    StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|----------- |--------------------- |----------:|----------:|----------:|------:|-------:|-------:|------:|----------:|
|  Serialize |             .NET 6.0 |  7.891 μs | 0.0369 μs | 0.0345 μs |  0.54 | 1.0834 | 0.0153 |     - |      9 KB |
|  Serialize | .NET Framework 4.7.2 | 14.606 μs | 0.0516 μs | 0.0483 μs |  1.00 | 1.8463 | 0.0305 |     - |     11 KB |
|            |                      |           |           |           |       |        |        |       |           |
| Deserialze |             .NET 6.0 | 10.410 μs | 0.0522 μs | 0.0463 μs |  0.48 | 0.9460 | 0.0153 |     - |      8 KB |
| Deserialze | .NET Framework 4.7.2 | 21.868 μs | 0.1393 μs | 0.1303 μs |  1.00 | 2.8687 | 0.0305 |     - |     18 KB |
