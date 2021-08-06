``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1151 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.6.21355.2
  [Host]     : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-KDEKXM : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-MYICZP : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|               Method |              Runtime | Size |  Format |          Mean |      Error |     StdDev |        Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|--------------------- |--------------------- |----- |-------- |--------------:|-----------:|-----------:|-------------:|--------:|-------:|-------:|------:|----------:|
|             **EncodeTo** |             **.NET 6.0** |    **0** |   **ASCII** |      **78.57 ns** |   **0.482 ns** |   **0.428 ns** | **1.63x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |   ASCII |     127.81 ns |   0.545 ns |   0.510 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |   ASCII |     154.45 ns |   2.498 ns |   2.086 ns | 1.57x faster |   0.02x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |   ASCII |     241.80 ns |   0.708 ns |   0.628 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |  **Binary** |      **89.98 ns** |   **1.163 ns** |   **1.088 ns** | **1.53x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |  Binary |     137.81 ns |   0.506 ns |   0.449 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |  Binary |     152.78 ns |   0.208 ns |   0.195 ns | 1.53x faster |   0.01x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |  Binary |     234.16 ns |   0.891 ns |   0.833 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** | **Boolean** |      **88.72 ns** |   **0.929 ns** |   **0.869 ns** | **1.59x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 | Boolean |     141.53 ns |   0.795 ns |   0.705 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 | Boolean |     153.21 ns |   0.417 ns |   0.370 ns | 1.52x faster |   0.00x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 | Boolean |     232.18 ns |   0.350 ns |   0.274 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **F4** |      **87.46 ns** |   **0.482 ns** |   **0.427 ns** | **1.56x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      F4 |     136.96 ns |   1.327 ns |   1.242 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      F4 |     154.53 ns |   0.196 ns |   0.153 ns | 1.52x faster |   0.01x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      F4 |     234.24 ns |   0.886 ns |   0.829 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **F8** |      **87.07 ns** |   **0.789 ns** |   **0.738 ns** | **1.56x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      F8 |     135.92 ns |   1.091 ns |   0.967 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      F8 |     154.89 ns |   0.287 ns |   0.268 ns | 1.56x faster |   0.00x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      F8 |     241.28 ns |   0.733 ns |   0.685 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I1** |      **90.39 ns** |   **0.730 ns** |   **0.683 ns** | **1.53x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I1 |     138.68 ns |   0.755 ns |   0.631 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I1 |     153.38 ns |   0.346 ns |   0.323 ns | 1.58x faster |   0.00x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I1 |     241.58 ns |   0.348 ns |   0.291 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I2** |      **86.81 ns** |   **0.359 ns** |   **0.336 ns** | **1.58x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I2 |     137.53 ns |   1.016 ns |   0.951 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I2 |     152.54 ns |   0.216 ns |   0.202 ns | 1.59x faster |   0.00x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I2 |     242.58 ns |   0.376 ns |   0.352 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I4** |      **88.15 ns** |   **0.351 ns** |   **0.328 ns** | **1.60x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I4 |     141.13 ns |   0.855 ns |   0.800 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I4 |     152.58 ns |   0.523 ns |   0.464 ns | 1.58x faster |   0.01x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I4 |     240.63 ns |   0.549 ns |   0.487 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I8** |      **87.96 ns** |   **0.792 ns** |   **0.741 ns** | **1.59x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I8 |     140.38 ns |   0.603 ns |   0.504 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I8 |     152.59 ns |   0.343 ns |   0.321 ns | 1.57x faster |   0.00x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I8 |     239.81 ns |   0.285 ns |   0.252 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |    **JIS8** |      **78.04 ns** |   **0.157 ns** |   **0.122 ns** | **1.63x faster** |   **0.00x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |    JIS8 |     127.04 ns |   0.302 ns |   0.268 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |    JIS8 |     152.23 ns |   0.368 ns |   0.326 ns | 1.59x faster |   0.01x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |    JIS8 |     241.81 ns |   2.141 ns |   1.898 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |    **List** |      **73.62 ns** |   **0.179 ns** |   **0.158 ns** | **1.68x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |    List |     123.53 ns |   0.594 ns |   0.526 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |    List |     134.46 ns |   0.266 ns |   0.236 ns | 1.52x faster |   0.01x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |    List |     204.92 ns |   0.623 ns |   0.583 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U1** |      **89.85 ns** |   **0.264 ns** |   **0.220 ns** | **1.54x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U1 |     138.14 ns |   0.654 ns |   0.612 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U1 |     152.69 ns |   0.384 ns |   0.360 ns | 1.59x faster |   0.01x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U1 |     242.74 ns |   0.533 ns |   0.499 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U2** |      **88.42 ns** |   **0.371 ns** |   **0.347 ns** | **1.59x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U2 |     140.52 ns |   0.493 ns |   0.461 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U2 |     152.78 ns |   1.241 ns |   1.036 ns | 1.60x faster |   0.01x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U2 |     244.55 ns |   0.580 ns |   0.514 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U4** |      **90.56 ns** |   **0.240 ns** |   **0.225 ns** | **1.54x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U4 |     139.34 ns |   1.094 ns |   1.023 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U4 |     159.65 ns |   0.254 ns |   0.238 ns | 1.52x faster |   0.00x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U4 |     242.12 ns |   0.437 ns |   0.409 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U8** |      **89.63 ns** |   **0.383 ns** |   **0.358 ns** | **1.53x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U8 |     137.06 ns |   0.989 ns |   0.925 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U8 |     154.47 ns |   0.257 ns |   0.228 ns | 1.55x faster |   0.00x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U8 |     239.05 ns |   0.403 ns |   0.377 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |   **ASCII** |      **97.52 ns** |   **0.456 ns** |   **0.427 ns** | **1.79x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |   ASCII |     174.41 ns |   0.436 ns |   0.364 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |   ASCII |     260.90 ns |   0.607 ns |   0.538 ns | 1.89x faster |   0.01x | 0.0038 |      - |     - |      32 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |   ASCII |     493.08 ns |   3.609 ns |   3.376 ns |     baseline |         | 0.0048 |      - |     - |      32 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |  **Binary** |     **106.57 ns** |   **0.468 ns** |   **0.438 ns** | **1.98x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |  Binary |     210.72 ns |   1.005 ns |   0.940 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |  Binary |     183.28 ns |   0.435 ns |   0.364 ns | 1.75x faster |   0.01x | 0.0095 |      - |     - |      80 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |  Binary |     320.83 ns |   0.663 ns |   0.620 ns |     baseline |         | 0.0124 |      - |     - |      80 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** | **Boolean** |     **105.75 ns** |   **0.501 ns** |   **0.469 ns** | **1.78x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 | Boolean |     187.74 ns |   2.366 ns |   2.213 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 | Boolean |     202.62 ns |   0.397 ns |   0.371 ns | 1.58x faster |   0.00x | 0.0095 |      - |     - |      80 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 | Boolean |     320.35 ns |   0.625 ns |   0.522 ns |     baseline |         | 0.0124 |      - |     - |      80 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **F4** |     **235.21 ns** |   **0.499 ns** |   **0.443 ns** | **1.55x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      F4 |     363.57 ns |   1.359 ns |   1.271 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      F4 |     196.69 ns |   0.373 ns |   0.330 ns | 1.85x faster |   0.01x | 0.0124 |      - |     - |     104 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      F4 |     363.04 ns |   1.245 ns |   1.164 ns |     baseline |         | 0.0162 |      - |     - |     104 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **F8** |     **235.27 ns** |   **0.427 ns** |   **0.378 ns** | **1.59x faster** |   **0.00x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      F8 |     373.73 ns |   0.705 ns |   0.660 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      F8 |     201.78 ns |   0.612 ns |   0.542 ns | 1.91x faster |   0.01x | 0.0172 |      - |     - |     144 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      F8 |     385.07 ns |   0.846 ns |   0.750 ns |     baseline |         | 0.0224 |      - |     - |     144 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **I1** |     **102.11 ns** |   **0.490 ns** |   **0.434 ns** | **1.78x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      I1 |     182.00 ns |   1.151 ns |   1.020 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      I1 |     181.58 ns |   0.524 ns |   0.490 ns | 1.75x faster |   0.01x | 0.0095 |      - |     - |      80 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      I1 |     318.07 ns |   1.368 ns |   1.280 ns |     baseline |         | 0.0124 |      - |     - |      80 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **I2** |     **110.03 ns** |   **0.492 ns** |   **0.436 ns** | **1.81x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      I2 |     198.78 ns |   0.905 ns |   0.847 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      I2 |     199.07 ns |   1.258 ns |   1.177 ns | 1.76x faster |   0.01x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      I2 |     351.35 ns |   0.831 ns |   0.736 ns |     baseline |         | 0.0138 |      - |     - |      88 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **I4** |     **109.19 ns** |   **0.356 ns** |   **0.333 ns** | **1.84x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      I4 |     201.19 ns |   1.403 ns |   1.312 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      I4 |     196.77 ns |   0.332 ns |   0.295 ns | 1.76x faster |   0.00x | 0.0124 |      - |     - |     104 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      I4 |     345.95 ns |   0.612 ns |   0.573 ns |     baseline |         | 0.0162 |      - |     - |     104 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **I8** |     **112.94 ns** |   **0.518 ns** |   **0.485 ns** | **1.79x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      I8 |     201.89 ns |   1.273 ns |   1.191 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      I8 |     199.93 ns |   0.933 ns |   0.873 ns | 1.81x faster |   0.01x | 0.0172 |      - |     - |     144 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      I8 |     361.80 ns |   0.557 ns |   0.465 ns |     baseline |         | 0.0224 |      - |     - |     144 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |    **JIS8** |     **403.39 ns** |   **2.318 ns** |   **2.168 ns** | **1.10x faster** |   **0.01x** | **0.0582** |      **-** |     **-** |     **488 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |    JIS8 |     443.07 ns |   2.317 ns |   1.935 ns |     baseline |         | 0.0596 |      - |     - |     377 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |    JIS8 |     358.18 ns |   0.902 ns |   0.844 ns | 1.72x faster |   0.01x | 0.0315 |      - |     - |     264 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |    JIS8 |     614.43 ns |   3.529 ns |   3.301 ns |     baseline |         | 0.0334 |      - |     - |     217 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |    **List** |     **172.86 ns** |   **0.983 ns** |   **0.919 ns** | **2.32x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |    List |     400.84 ns |   2.687 ns |   2.513 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |    List |   1,326.94 ns |   2.426 ns |   2.025 ns | 1.25x faster |   0.00x | 0.0153 |      - |     - |     136 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |    List |   1,664.19 ns |   4.583 ns |   4.287 ns |     baseline |         | 0.0210 |      - |     - |     136 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **U1** |     **106.47 ns** |   **0.734 ns** |   **0.687 ns** | **1.83x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      U1 |     194.26 ns |   2.053 ns |   1.714 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      U1 |     185.64 ns |   0.460 ns |   0.407 ns | 1.75x faster |   0.00x | 0.0095 |      - |     - |      80 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      U1 |     325.21 ns |   1.066 ns |   0.890 ns |     baseline |         | 0.0124 |      - |     - |      80 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **U2** |     **121.77 ns** |   **0.602 ns** |   **0.563 ns** | **1.59x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      U2 |     194.19 ns |   1.094 ns |   1.023 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      U2 |     195.90 ns |   1.037 ns |   0.970 ns | 1.79x faster |   0.01x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      U2 |     349.72 ns |   0.996 ns |   0.932 ns |     baseline |         | 0.0138 |      - |     - |      88 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **U4** |     **116.36 ns** |   **0.549 ns** |   **0.459 ns** | **1.69x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      U4 |     196.48 ns |   1.271 ns |   1.189 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      U4 |     193.83 ns |   0.534 ns |   0.499 ns | 1.79x faster |   0.01x | 0.0124 |      - |     - |     104 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      U4 |     346.64 ns |   1.827 ns |   1.709 ns |     baseline |         | 0.0162 |      - |     - |     104 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **U8** |     **113.60 ns** |   **0.992 ns** |   **0.928 ns** | **1.82x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      U8 |     206.36 ns |   1.064 ns |   0.888 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      U8 |     198.37 ns |   0.702 ns |   0.656 ns | 1.79x faster |   0.01x | 0.0172 |      - |     - |     144 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      U8 |     355.13 ns |   0.891 ns |   0.789 ns |     baseline |         | 0.0224 |      - |     - |     144 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |   **ASCII** |     **138.21 ns** |   **1.743 ns** |   **1.545 ns** | **8.67x faster** |   **0.11x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |   ASCII |   1,198.34 ns |   7.067 ns |   6.265 ns |     baseline |         | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |   ASCII |     495.59 ns |   5.886 ns |   5.506 ns | 3.59x faster |   0.05x | 0.2689 | 0.0019 |     - |   2,256 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |   ASCII |   1,778.44 ns |  12.441 ns |  11.638 ns |     baseline |         | 0.3624 | 0.0019 |     - |   2,295 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |  **Binary** |     **119.76 ns** |   **1.267 ns** |   **1.185 ns** | **1.77x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |  Binary |     211.67 ns |   2.111 ns |   1.763 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |  Binary |     292.06 ns |   1.714 ns |   1.603 ns | 1.53x faster |   0.02x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |  Binary |     446.62 ns |   3.138 ns |   2.935 ns |     baseline |         | 0.0138 |      - |     - |      88 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** | **Boolean** |     **118.42 ns** |   **0.900 ns** |   **0.841 ns** | **1.79x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 | Boolean |     211.87 ns |   2.216 ns |   2.073 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 | Boolean |     313.25 ns |   1.719 ns |   1.608 ns | 1.47x faster |   0.01x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 | Boolean |     460.56 ns |   2.156 ns |   2.017 ns |     baseline |         | 0.0138 |      - |     - |      88 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **F4** |  **12,478.31 ns** |  **29.329 ns** |  **27.434 ns** | **1.59x faster** |   **0.01x** |      **-** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      F4 |  19,867.39 ns |  49.461 ns |  46.266 ns |     baseline |         |      - |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      F4 |     721.19 ns |   2.143 ns |   2.004 ns | 3.22x faster |   0.01x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      F4 |   2,323.68 ns |   7.669 ns |   6.799 ns |     baseline |         | 0.0114 |      - |     - |      88 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **F8** |  **12,649.42 ns** |  **25.152 ns** |  **22.296 ns** | **1.62x faster** |   **0.00x** |      **-** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      F8 |  20,514.97 ns |  41.532 ns |  36.817 ns |     baseline |         |      - |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      F8 |     763.23 ns |   4.015 ns |   3.756 ns | 3.82x faster |   0.02x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      F8 |   2,914.70 ns |  15.296 ns |  14.308 ns |     baseline |         | 0.0114 |      - |     - |      88 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I1** |     **124.26 ns** |   **0.897 ns** |   **0.839 ns** | **1.68x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I1 |     208.48 ns |   2.201 ns |   2.059 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I1 |     305.16 ns |   1.973 ns |   1.846 ns | 1.44x faster |   0.01x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I1 |     438.49 ns |   2.610 ns |   2.442 ns |     baseline |         | 0.0138 |      - |     - |      88 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I2** |     **639.28 ns** |   **1.489 ns** |   **1.320 ns** | **1.54x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I2 |     986.49 ns |   5.138 ns |   4.806 ns |     baseline |         | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I2 |     797.56 ns |   3.851 ns |   3.602 ns | 1.54x faster |   0.01x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I2 |   1,231.02 ns |   6.648 ns |   5.551 ns |     baseline |         | 0.0134 |      - |     - |      88 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I4** |     **498.94 ns** |   **1.804 ns** |   **1.687 ns** | **2.13x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I4 |   1,062.44 ns |   7.222 ns |   6.756 ns |     baseline |         | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I4 |     598.36 ns |   4.542 ns |   4.248 ns | 2.19x faster |   0.02x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I4 |   1,309.91 ns |   3.888 ns |   3.035 ns |     baseline |         | 0.0134 |      - |     - |      88 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I8** |     **596.93 ns** |   **2.881 ns** |   **2.695 ns** | **2.81x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I8 |   1,679.14 ns |  12.885 ns |  12.052 ns |     baseline |         | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I8 |     775.82 ns |   4.085 ns |   3.821 ns | 2.50x faster |   0.02x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I8 |   1,942.46 ns |  12.295 ns |  11.501 ns |     baseline |         | 0.0114 |      - |     - |      88 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |    **JIS8** |  **17,773.30 ns** | **348.755 ns** | **342.524 ns** | **1.03x faster** |   **0.02x** | **0.0305** |      **-** |     **-** |     **488 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |    JIS8 |  18,292.84 ns |  88.382 ns |  82.673 ns |     baseline |         | 0.0305 |      - |     - |     377 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |    JIS8 |   9,951.07 ns |  51.529 ns |  48.200 ns | 1.03x faster |   0.01x | 0.3204 |      - |     - |   2,720 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |    JIS8 |  10,224.20 ns |  59.819 ns |  53.028 ns |     baseline |         | 0.4120 |      - |     - |   2,664 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |    **List** |   **8,023.83 ns** |  **25.035 ns** |  **23.418 ns** | **3.30x faster** |   **0.01x** |      **-** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |    List |  26,501.34 ns |  45.859 ns |  38.295 ns |     baseline |         |      - |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |    List | 121,299.27 ns | 434.675 ns | 406.596 ns | 1.26x faster |   0.00x | 0.9766 |      - |     - |   8,248 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |    List | 152,197.36 ns | 287.706 ns | 240.248 ns |     baseline |         | 1.2207 |      - |     - |   8,274 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U1** |     **122.07 ns** |   **1.009 ns** |   **0.944 ns** | **1.74x faster** |   **0.03x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U1 |     212.41 ns |   3.471 ns |   3.247 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U1 |     296.42 ns |   1.752 ns |   1.639 ns | 1.51x faster |   0.01x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U1 |     447.99 ns |   2.221 ns |   2.077 ns |     baseline |         | 0.0138 |      - |     - |      88 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U2** |     **512.04 ns** |   **1.761 ns** |   **1.647 ns** | **1.57x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U2 |     802.30 ns |   2.100 ns |   1.639 ns |     baseline |         | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U2 |     574.37 ns |   2.739 ns |   2.562 ns | 1.85x faster |   0.01x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U2 |   1,062.66 ns |   8.286 ns |   7.751 ns |     baseline |         | 0.0134 |      - |     - |      88 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U4** |     **511.17 ns** |   **3.788 ns** |   **3.543 ns** | **1.87x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U4 |     954.10 ns |   8.129 ns |   7.604 ns |     baseline |         | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U4 |     601.75 ns |   5.771 ns |   5.116 ns | 2.06x faster |   0.02x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U4 |   1,239.34 ns |   8.514 ns |   7.964 ns |     baseline |         | 0.0134 |      - |     - |      88 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U8** |     **554.11 ns** |   **2.752 ns** |   **2.574 ns** | **3.05x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U8 |   1,690.95 ns |   5.553 ns |   4.923 ns |     baseline |         | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U8 |     653.53 ns |   4.938 ns |   4.619 ns | 2.93x faster |   0.03x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U8 |   1,917.67 ns |  12.899 ns |  12.066 ns |     baseline |         | 0.0114 |      - |     - |      88 B |
