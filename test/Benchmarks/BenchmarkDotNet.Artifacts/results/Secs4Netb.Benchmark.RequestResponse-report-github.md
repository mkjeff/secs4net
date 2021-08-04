``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1151 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.6.21355.2
  [Host]     : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-ZJYKDZ : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-BOERDN : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|     Method |        Job |              Runtime | Toolchain | Count |     Mean |     Error |    StdDev | Ratio | RatioSD |   Gen 0 |  Gen 1 | Gen 2 | Allocated |
|----------- |----------- |--------------------- |---------- |------ |---------:|----------:|----------:|------:|--------:|--------:|-------:|------:|----------:|
| Sequential | Job-ZJYKDZ |             .NET 6.0 |    net6.0 |    16 | 1.671 ms | 0.0322 ms | 0.0345 ms |  1.04 |    0.02 |  3.9063 |      - |     - |     47 KB |
| Sequential | Job-BOERDN | .NET Framework 4.7.2 |    net472 |    16 | 1.603 ms | 0.0198 ms | 0.0176 ms |  1.00 |    0.00 | 19.5313 | 1.9531 |     - |    134 KB |
