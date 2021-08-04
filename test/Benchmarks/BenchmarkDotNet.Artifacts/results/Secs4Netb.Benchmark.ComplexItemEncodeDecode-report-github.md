``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1151 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.6.21355.2
  [Host]     : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-JFBEZI : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-ILLKVN : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
| Method |        Job |              Runtime | Toolchain | ItemCount |        Mean |    Error |   StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|------- |----------- |--------------------- |---------- |---------- |------------:|---------:|---------:|------:|-------:|-------:|------:|----------:|
| **Encode** | **Job-JFBEZI** |             **.NET 6.0** |    **net6.0** |         **0** |    **881.2 ns** |  **3.92 ns** |  **3.67 ns** |  **0.55** | **0.0048** |      **-** |     **-** |      **40 B** |
| Encode | Job-ILLKVN | .NET Framework 4.7.2 |    net472 |         0 |  1,614.7 ns |  3.52 ns |  2.94 ns |  1.00 | 0.0057 |      - |     - |      40 B |
|        |            |                      |           |           |             |          |          |       |        |        |       |           |
| Decode | Job-JFBEZI |             .NET 6.0 |    net6.0 |         0 |  5,208.6 ns |  8.45 ns |  7.49 ns |  0.69 | 0.0839 |      - |     - |     720 B |
| Decode | Job-ILLKVN | .NET Framework 4.7.2 |    net472 |         0 |  7,518.2 ns | 14.51 ns | 12.12 ns |  1.00 | 0.1144 |      - |     - |     762 B |
|        |            |                      |           |           |             |          |          |       |        |        |       |           |
| **Encode** | **Job-JFBEZI** |             **.NET 6.0** |    **net6.0** |        **64** |  **2,224.3 ns** |  **9.48 ns** |  **8.87 ns** |  **0.23** | **0.0038** |      **-** |     **-** |      **40 B** |
| Encode | Job-ILLKVN | .NET Framework 4.7.2 |    net472 |        64 |  9,496.6 ns | 14.23 ns | 13.31 ns |  1.00 |      - |      - |     - |      40 B |
|        |            |                      |           |           |             |          |          |       |        |        |       |           |
| Decode | Job-JFBEZI |             .NET 6.0 |    net6.0 |        64 |  7,863.1 ns | 59.52 ns | 55.68 ns |  0.44 | 0.8850 | 0.0153 |     - |   7,408 B |
| Decode | Job-ILLKVN | .NET Framework 4.7.2 |    net472 |        64 | 17,762.0 ns | 54.58 ns | 51.06 ns |  1.00 | 1.1597 |      - |     - |   7,470 B |
|        |            |                      |           |           |             |          |          |       |        |        |       |           |
| **Encode** | **Job-JFBEZI** |             **.NET 6.0** |    **net6.0** |       **128** |  **3,113.1 ns** | **15.72 ns** | **13.94 ns** |  **0.19** | **0.0038** |      **-** |     **-** |      **40 B** |
| Encode | Job-ILLKVN | .NET Framework 4.7.2 |    net472 |       128 | 16,005.0 ns | 33.13 ns | 27.66 ns |  1.00 |      - |      - |     - |      40 B |
|        |            |                      |           |           |             |          |          |       |        |        |       |           |
| Decode | Job-JFBEZI |             .NET 6.0 |    net6.0 |       128 |  9,146.7 ns | 37.61 ns | 33.34 ns |  0.36 | 1.0986 | 0.0305 |     - |   9,272 B |
| Decode | Job-ILLKVN | .NET Framework 4.7.2 |    net472 |       128 | 25,073.7 ns | 99.29 ns | 92.87 ns |  1.00 | 1.4648 | 0.0305 |     - |   9,340 B |
