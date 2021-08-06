``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1151 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.6.21355.2
  [Host]     : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-BPLYAK : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-JUFGSU : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|     Method |              Runtime |      Mean |     Error |    StdDev |        Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|----------- |--------------------- |----------:|----------:|----------:|-------------:|--------:|-------:|-------:|------:|----------:|
|  Serialize |             .NET 6.0 |  8.084 μs | 0.0914 μs | 0.0764 μs | 1.82x faster |   0.02x | 1.0834 | 0.0153 |     - |      9 KB |
|  Serialize | .NET Framework 4.7.2 | 14.712 μs | 0.1132 μs | 0.0945 μs |     baseline |         | 1.8463 | 0.0305 |     - |     11 KB |
|            |                      |           |           |           |              |         |        |        |       |           |
| Deserialze |             .NET 6.0 |  9.955 μs | 0.0827 μs | 0.0733 μs | 2.26x faster |   0.02x | 0.7935 |      - |     - |      7 KB |
| Deserialze | .NET Framework 4.7.2 | 22.486 μs | 0.1359 μs | 0.1271 μs |     baseline |         | 2.6550 | 0.0305 |     - |     16 KB |
