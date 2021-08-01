``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1151 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.6.21355.2
  [Host]     : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-ORWWCQ : .NET 5.0.8 (5.0.821.31504), X64 RyuJIT
  Job-BRIRGZ : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
| Method |        Job |              Runtime | Toolchain | ItemCount |       Mean |     Error |    StdDev | Ratio |  Gen 0 | Gen 1 | Gen 2 | Allocated | Allocated native memory | Native memory leak |
|------- |----------- |--------------------- |---------- |---------- |-----------:|----------:|----------:|------:|-------:|------:|------:|----------:|------------------------:|-------------------:|
| **Decode** | **Job-ORWWCQ** |             **.NET 5.0** |    **net5.0** |         **0** |   **4.370 μs** | **0.0146 μs** | **0.0136 μs** |  **0.58** | **0.0839** |     **-** |     **-** |     **744 B** |                       **-** |                  **-** |
| Decode | Job-BRIRGZ | .NET Framework 4.7.2 |    net472 |         0 |   7.580 μs | 0.0140 μs | 0.0125 μs |  1.00 | 0.1068 |     - |     - |     786 B |                       - |                  - |
|        |            |                      |           |           |            |           |           |       |        |       |       |           |                         |                    |
| **Decode** | **Job-ORWWCQ** |             **.NET 5.0** |    **net5.0** |        **10** |   **7.013 μs** | **0.0182 μs** | **0.0161 μs** |  **0.53** | **0.4959** |     **-** |     **-** |   **4,208 B** |                       **-** |                  **-** |
| Decode | Job-BRIRGZ | .NET Framework 4.7.2 |    net472 |        10 |  13.222 μs | 0.0182 μs | 0.0161 μs |  1.00 | 0.6409 |     - |     - |   4,116 B |                       - |                  - |
|        |            |                      |           |           |            |           |           |       |        |       |       |           |                         |                    |
| **Decode** | **Job-ORWWCQ** |             **.NET 5.0** |    **net5.0** |      **1025** |  **69.255 μs** | **0.4225 μs** | **0.3952 μs** |  **0.35** | **0.3662** |     **-** |     **-** |   **4,008 B** |                       **-** |                  **-** |
| Decode | Job-BRIRGZ | .NET Framework 4.7.2 |    net472 |      1025 | 195.484 μs | 0.2294 μs | 0.2033 μs |  1.00 | 0.4883 |     - |     - |   3,916 B |                       - |                  - |
