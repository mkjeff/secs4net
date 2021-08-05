``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1151 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.6.21355.2
  [Host]     : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-MBNKPF : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-ZEVWQK : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|     Method |        Job |              Runtime | Toolchain |      Mean |     Error |    StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|----------- |----------- |--------------------- |---------- |----------:|----------:|----------:|------:|-------:|-------:|------:|----------:|
|  Serialize | Job-MBNKPF |             .NET 6.0 |    net6.0 |  7.929 μs | 0.0313 μs | 0.0277 μs |  0.54 | 1.0834 | 0.0153 |     - |      9 KB |
|  Serialize | Job-ZEVWQK | .NET Framework 4.7.2 |    net472 | 14.667 μs | 0.0376 μs | 0.0294 μs |  1.00 | 1.8463 | 0.0305 |     - |     11 KB |
|            |            |                      |           |           |           |           |       |        |        |       |           |
| Deserialze | Job-MBNKPF |             .NET 6.0 |    net6.0 | 10.379 μs | 0.0325 μs | 0.0304 μs |  0.48 | 0.9460 | 0.0153 |     - |      8 KB |
| Deserialze | Job-ZEVWQK | .NET Framework 4.7.2 |    net472 | 21.799 μs | 0.0743 μs | 0.0659 μs |  1.00 | 2.8687 | 0.0305 |     - |     18 KB |
