``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1151 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.6.21355.2
  [Host]     : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-KDEKXM : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-MYICZP : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|     Method |              Runtime |      Mean |     Error |    StdDev |        Ratio | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------- |--------------------- |----------:|----------:|----------:|-------------:|--------:|-------:|------:|------:|----------:|
|  Serialize |             .NET 6.0 |  6.546 μs | 0.0485 μs | 0.0454 μs | 2.57x faster |   0.03x | 0.2899 |     - |     - |      2 KB |
|  Serialize | .NET Framework 4.7.2 | 16.813 μs | 0.1051 μs | 0.0983 μs |     baseline |         | 0.5798 |     - |     - |      4 KB |
|            |                      |           |           |           |              |         |        |       |       |           |
| Deserialze |             .NET 6.0 | 34.786 μs | 0.1400 μs | 0.1310 μs | 1.89x faster |   0.01x | 1.5869 |     - |     - |     13 KB |
| Deserialze | .NET Framework 4.7.2 | 65.672 μs | 0.1503 μs | 0.1406 μs |     baseline |         | 2.1973 |     - |     - |     14 KB |
