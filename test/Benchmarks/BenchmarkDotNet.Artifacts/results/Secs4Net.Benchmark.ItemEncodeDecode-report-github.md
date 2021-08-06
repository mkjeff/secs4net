``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1151 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.6.21355.2
  [Host]     : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-IIWPMA : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-DEBAEW : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|               Method |              Runtime | Size |  Format |          Mean |      Error |     StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|--------------------- |--------------------- |----- |-------- |--------------:|-----------:|-----------:|------:|-------:|-------:|------:|----------:|
|             **EncodeTo** |             **.NET 6.0** |    **0** |   **ASCII** |      **73.71 ns** |   **0.312 ns** |   **0.276 ns** |  **0.58** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |   ASCII |     126.66 ns |   0.538 ns |   0.504 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |   ASCII |     155.48 ns |   0.381 ns |   0.338 ns |  0.66 |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |   ASCII |     235.00 ns |   0.612 ns |   0.511 ns |  1.00 |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |  **Binary** |      **91.72 ns** |   **0.626 ns** |   **0.585 ns** |  **0.64** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |  Binary |     143.55 ns |   0.701 ns |   0.656 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |  Binary |     154.80 ns |   0.152 ns |   0.135 ns |  0.64 |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |  Binary |     242.90 ns |   0.402 ns |   0.336 ns |  1.00 |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** | **Boolean** |      **93.87 ns** |   **0.268 ns** |   **0.250 ns** |  **0.68** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 | Boolean |     137.51 ns |   0.516 ns |   0.482 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 | Boolean |     157.77 ns |   0.427 ns |   0.400 ns |  0.66 |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 | Boolean |     238.53 ns |   0.567 ns |   0.503 ns |  1.00 |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **F4** |     **101.85 ns** |   **0.251 ns** |   **0.223 ns** |  **0.72** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      F4 |     141.02 ns |   0.675 ns |   0.632 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      F4 |     155.11 ns |   0.425 ns |   0.398 ns |  0.65 |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      F4 |     237.64 ns |   0.883 ns |   0.826 ns |  1.00 |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **F8** |      **98.42 ns** |   **0.369 ns** |   **0.327 ns** |  **0.69** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      F8 |     143.55 ns |   0.735 ns |   0.651 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      F8 |     157.90 ns |   0.404 ns |   0.378 ns |  0.65 |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      F8 |     242.14 ns |   0.760 ns |   0.634 ns |  1.00 |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I1** |      **93.42 ns** |   **0.216 ns** |   **0.169 ns** |  **0.68** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I1 |     138.03 ns |   0.756 ns |   0.707 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I1 |     152.68 ns |   0.200 ns |   0.156 ns |  0.63 |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I1 |     243.47 ns |   0.849 ns |   0.794 ns |  1.00 |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I2** |      **88.76 ns** |   **0.278 ns** |   **0.247 ns** |  **0.64** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I2 |     137.88 ns |   0.783 ns |   0.732 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I2 |     153.48 ns |   0.425 ns |   0.377 ns |  0.64 |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I2 |     239.13 ns |   0.764 ns |   0.715 ns |  1.00 |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I4** |      **87.93 ns** |   **0.447 ns** |   **0.373 ns** |  **0.64** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I4 |     137.93 ns |   0.577 ns |   0.539 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I4 |     153.54 ns |   0.232 ns |   0.193 ns |  0.64 |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I4 |     241.02 ns |   0.450 ns |   0.421 ns |  1.00 |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I8** |      **88.40 ns** |   **0.566 ns** |   **0.529 ns** |  **0.63** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I8 |     140.68 ns |   0.450 ns |   0.399 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I8 |     155.26 ns |   0.237 ns |   0.210 ns |  0.63 |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I8 |     245.63 ns |   0.423 ns |   0.375 ns |  1.00 |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |    **JIS8** |      **73.47 ns** |   **0.384 ns** |   **0.359 ns** |  **0.58** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |    JIS8 |     126.48 ns |   0.340 ns |   0.318 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |    JIS8 |     154.90 ns |   0.602 ns |   0.563 ns |  0.64 |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |    JIS8 |     240.49 ns |   0.696 ns |   0.651 ns |  1.00 |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |    **List** |      **79.03 ns** |   **0.214 ns** |   **0.200 ns** |  **0.62** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |    List |     127.74 ns |   0.511 ns |   0.453 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |    List |     136.31 ns |   0.357 ns |   0.334 ns |  0.64 |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |    List |     211.50 ns |   0.680 ns |   0.636 ns |  1.00 |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U1** |      **93.06 ns** |   **0.318 ns** |   **0.297 ns** |  **0.66** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U1 |     140.44 ns |   0.461 ns |   0.431 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U1 |     153.65 ns |   0.391 ns |   0.366 ns |  0.64 |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U1 |     241.79 ns |   0.725 ns |   0.643 ns |  1.00 |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U2** |      **95.89 ns** |   **0.432 ns** |   **0.383 ns** |  **0.69** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U2 |     138.10 ns |   0.705 ns |   0.659 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U2 |     153.82 ns |   0.287 ns |   0.240 ns |  0.64 |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U2 |     240.04 ns |   0.458 ns |   0.428 ns |  1.00 |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U4** |      **88.62 ns** |   **0.253 ns** |   **0.197 ns** |  **0.63** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U4 |     141.40 ns |   0.935 ns |   0.875 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U4 |     154.47 ns |   0.307 ns |   0.272 ns |  0.65 |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U4 |     236.06 ns |   0.617 ns |   0.577 ns |  1.00 |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U8** |      **88.61 ns** |   **0.388 ns** |   **0.363 ns** |  **0.63** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U8 |     140.87 ns |   0.670 ns |   0.626 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U8 |     152.55 ns |   0.384 ns |   0.359 ns |  0.63 |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U8 |     240.94 ns |   0.444 ns |   0.371 ns |  1.00 |      - |      - |     - |         - |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |   **ASCII** |      **97.88 ns** |   **0.407 ns** |   **0.380 ns** |  **0.56** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |   ASCII |     173.79 ns |   0.517 ns |   0.484 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |   ASCII |     258.82 ns |   0.576 ns |   0.511 ns |  0.54 | 0.0038 |      - |     - |      32 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |   ASCII |     475.07 ns |   0.979 ns |   0.867 ns |  1.00 | 0.0048 |      - |     - |      32 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |  **Binary** |     **105.62 ns** |   **0.485 ns** |   **0.430 ns** |  **0.57** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |  Binary |     185.12 ns |   1.054 ns |   0.986 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |  Binary |     186.46 ns |   0.447 ns |   0.373 ns |  0.59 | 0.0095 |      - |     - |      80 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |  Binary |     315.01 ns |   0.632 ns |   0.560 ns |  1.00 | 0.0124 |      - |     - |      80 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** | **Boolean** |     **106.73 ns** |   **0.309 ns** |   **0.274 ns** |  **0.61** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 | Boolean |     174.65 ns |   0.607 ns |   0.568 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 | Boolean |     196.53 ns |   0.478 ns |   0.399 ns |  0.63 | 0.0095 |      - |     - |      80 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 | Boolean |     311.00 ns |   0.568 ns |   0.474 ns |  1.00 | 0.0124 |      - |     - |      80 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **F4** |     **236.21 ns** |   **1.067 ns** |   **0.891 ns** |  **0.64** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      F4 |     366.77 ns |   0.669 ns |   0.593 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      F4 |     205.03 ns |   0.587 ns |   0.520 ns |  0.55 | 0.0124 |      - |     - |     104 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      F4 |     373.54 ns |   0.660 ns |   0.618 ns |  1.00 | 0.0162 |      - |     - |     104 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **F8** |     **235.53 ns** |   **0.579 ns** |   **0.513 ns** |  **0.63** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      F8 |     376.50 ns |   0.717 ns |   0.671 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      F8 |     207.62 ns |   0.630 ns |   0.526 ns |  0.55 | 0.0172 |      - |     - |     144 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      F8 |     375.07 ns |   0.696 ns |   0.617 ns |  1.00 | 0.0224 |      - |     - |     144 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **I1** |     **102.81 ns** |   **0.392 ns** |   **0.367 ns** |  **0.59** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      I1 |     174.36 ns |   0.398 ns |   0.373 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      I1 |     186.40 ns |   0.506 ns |   0.473 ns |  0.59 | 0.0095 |      - |     - |      80 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      I1 |     314.07 ns |   0.838 ns |   0.784 ns |  1.00 | 0.0124 |      - |     - |      80 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **I2** |     **112.84 ns** |   **0.342 ns** |   **0.303 ns** |  **0.58** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      I2 |     193.75 ns |   1.138 ns |   1.064 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      I2 |     203.01 ns |   0.459 ns |   0.429 ns |  0.58 | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      I2 |     351.43 ns |   0.881 ns |   0.824 ns |  1.00 | 0.0138 |      - |     - |      88 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **I4** |     **111.71 ns** |   **0.236 ns** |   **0.209 ns** |  **0.52** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      I4 |     216.84 ns |   0.377 ns |   0.353 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      I4 |     199.64 ns |   0.447 ns |   0.418 ns |  0.57 | 0.0124 |      - |     - |     104 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      I4 |     352.75 ns |   0.600 ns |   0.562 ns |  1.00 | 0.0162 |      - |     - |     104 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **I8** |     **112.54 ns** |   **0.563 ns** |   **0.499 ns** |  **0.56** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      I8 |     202.54 ns |   0.667 ns |   0.624 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      I8 |     203.18 ns |   0.462 ns |   0.409 ns |  0.56 | 0.0172 |      - |     - |     144 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      I8 |     365.89 ns |   0.792 ns |   0.741 ns |  1.00 | 0.0224 |      - |     - |     144 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |    **JIS8** |     **419.72 ns** |   **1.408 ns** |   **1.248 ns** |  **0.92** | **0.0582** |      **-** |     **-** |     **488 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |    JIS8 |     457.38 ns |   1.490 ns |   1.394 ns |  1.00 | 0.0596 |      - |     - |     377 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |    JIS8 |     374.79 ns |   7.192 ns |   6.376 ns |  0.63 | 0.0315 |      - |     - |     264 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |    JIS8 |     592.24 ns |   2.668 ns |   2.496 ns |  1.00 | 0.0334 |      - |     - |     217 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |    **List** |     **225.31 ns** |   **0.573 ns** |   **0.536 ns** |  **0.49** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |    List |     463.59 ns |   1.041 ns |   0.974 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |    List |   1,435.39 ns |   2.489 ns |   2.206 ns |  0.76 | 0.0191 |      - |     - |     168 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |    List |   1,898.22 ns |   5.968 ns |   5.290 ns |  1.00 | 0.0267 |      - |     - |     177 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **U1** |     **106.09 ns** |   **0.287 ns** |   **0.255 ns** |  **0.56** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      U1 |     188.87 ns |   0.790 ns |   0.739 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      U1 |     184.60 ns |   0.450 ns |   0.399 ns |  0.58 | 0.0095 |      - |     - |      80 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      U1 |     317.19 ns |   0.575 ns |   0.480 ns |  1.00 | 0.0124 |      - |     - |      80 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **U2** |     **120.94 ns** |   **0.394 ns** |   **0.369 ns** |  **0.63** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      U2 |     193.45 ns |   0.604 ns |   0.565 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      U2 |     197.05 ns |   0.563 ns |   0.499 ns |  0.56 | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      U2 |     354.39 ns |   0.915 ns |   0.811 ns |  1.00 | 0.0138 |      - |     - |      88 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **U4** |     **120.99 ns** |   **0.552 ns** |   **0.461 ns** |  **0.61** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      U4 |     197.17 ns |   1.014 ns |   0.949 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      U4 |     200.96 ns |   0.376 ns |   0.352 ns |  0.57 | 0.0124 |      - |     - |     104 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      U4 |     352.02 ns |   0.873 ns |   0.774 ns |  1.00 | 0.0162 |      - |     - |     104 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **U8** |     **115.93 ns** |   **0.313 ns** |   **0.278 ns** |  **0.53** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      U8 |     217.99 ns |   0.577 ns |   0.540 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      U8 |     204.30 ns |   0.854 ns |   0.798 ns |  0.57 | 0.0172 |      - |     - |     144 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      U8 |     360.30 ns |   0.915 ns |   0.856 ns |  1.00 | 0.0224 |      - |     - |     144 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |   **ASCII** |     **136.32 ns** |   **0.469 ns** |   **0.416 ns** |  **0.11** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |   ASCII |   1,206.62 ns |   2.999 ns |   2.658 ns |  1.00 | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |   ASCII |     512.73 ns |   2.054 ns |   1.604 ns |  0.28 | 0.2689 | 0.0019 |     - |   2,256 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |   ASCII |   1,811.36 ns |   6.826 ns |   6.385 ns |  1.00 | 0.3624 | 0.0019 |     - |   2,295 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |  **Binary** |     **117.02 ns** |   **0.528 ns** |   **0.494 ns** |  **0.54** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |  Binary |     216.92 ns |   0.522 ns |   0.463 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |  Binary |     299.20 ns |   0.728 ns |   0.681 ns |  0.66 | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |  Binary |     452.87 ns |   1.177 ns |   1.101 ns |  1.00 | 0.0138 |      - |     - |      88 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** | **Boolean** |     **118.93 ns** |   **0.367 ns** |   **0.326 ns** |  **0.58** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 | Boolean |     203.52 ns |   0.590 ns |   0.552 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 | Boolean |     314.44 ns |   1.353 ns |   1.265 ns |  0.68 | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 | Boolean |     463.49 ns |   0.939 ns |   0.879 ns |  1.00 | 0.0138 |      - |     - |      88 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **F4** |  **12,987.96 ns** |   **8.038 ns** |   **7.125 ns** |  **0.65** | **0.0038** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      F4 |  20,055.42 ns |  14.420 ns |  12.783 ns |  1.00 |      - |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      F4 |     834.69 ns |   3.170 ns |   2.647 ns |  0.38 | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      F4 |   2,185.59 ns |   5.135 ns |   4.552 ns |  1.00 | 0.0114 |      - |     - |      88 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **F8** |  **12,497.91 ns** |   **9.967 ns** |   **9.323 ns** |  **0.60** |      **-** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      F8 |  20,711.72 ns |  62.376 ns |  58.346 ns |  1.00 |      - |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      F8 |     767.56 ns |   2.954 ns |   2.618 ns |  0.26 | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      F8 |   2,912.77 ns |  11.496 ns |  10.191 ns |  1.00 | 0.0114 |      - |     - |      88 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I1** |     **124.27 ns** |   **0.244 ns** |   **0.217 ns** |  **0.60** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I1 |     206.58 ns |   0.348 ns |   0.308 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I1 |     309.20 ns |   0.267 ns |   0.208 ns |  0.68 | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I1 |     453.86 ns |   1.497 ns |   1.327 ns |  1.00 | 0.0138 |      - |     - |      88 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I2** |     **650.88 ns** |   **3.019 ns** |   **2.824 ns** |  **0.66** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I2 |     985.76 ns |   1.886 ns |   1.672 ns |  1.00 | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I2 |     798.08 ns |   1.810 ns |   1.511 ns |  0.64 | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I2 |   1,239.94 ns |   5.405 ns |   5.056 ns |  1.00 | 0.0134 |      - |     - |      88 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I4** |     **500.75 ns** |   **2.599 ns** |   **2.170 ns** |  **0.50** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I4 |     999.91 ns |   5.689 ns |   5.043 ns |  1.00 | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I4 |     591.73 ns |   2.953 ns |   2.618 ns |  0.45 | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I4 |   1,323.59 ns |   5.111 ns |   4.781 ns |  1.00 | 0.0134 |      - |     - |      88 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I8** |     **597.39 ns** |   **6.937 ns** |   **6.489 ns** |  **0.36** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I8 |   1,675.97 ns |   3.847 ns |   3.598 ns |  1.00 | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I8 |     747.38 ns |   2.610 ns |   2.442 ns |  0.38 | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I8 |   1,946.20 ns |   8.221 ns |   7.690 ns |  1.00 | 0.0114 |      - |     - |      88 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |    **JIS8** |  **19,128.42 ns** | **120.782 ns** | **112.980 ns** |  **1.04** | **0.0305** |      **-** |     **-** |     **488 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |    JIS8 |  18,338.60 ns |  66.124 ns |  61.853 ns |  1.00 | 0.0305 |      - |     - |     377 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |    JIS8 |  13,021.54 ns |  35.199 ns |  32.925 ns |  1.25 | 0.3204 |      - |     - |   2,720 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |    JIS8 |  10,385.31 ns |  39.374 ns |  32.879 ns |  1.00 | 0.4120 |      - |     - |   2,664 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |    **List** |  **13,402.35 ns** |  **43.851 ns** |  **38.873 ns** |  **0.42** |      **-** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |    List |  31,809.23 ns |  87.913 ns |  77.933 ns |  1.00 |      - |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |    List | 131,079.66 ns | 295.261 ns | 276.187 ns |  0.76 | 0.9766 |      - |     - |   8,280 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |    List | 172,523.11 ns | 278.172 ns | 260.202 ns |  1.00 | 1.2207 |      - |     - |   8,316 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U1** |     **118.61 ns** |   **0.709 ns** |   **0.663 ns** |  **0.54** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U1 |     218.89 ns |   0.782 ns |   0.693 ns |  1.00 | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U1 |     291.23 ns |   0.937 ns |   0.876 ns |  0.64 | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U1 |     454.10 ns |   1.492 ns |   1.396 ns |  1.00 | 0.0138 |      - |     - |      88 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U2** |     **514.61 ns** |   **2.263 ns** |   **1.889 ns** |  **0.64** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U2 |     807.34 ns |   1.809 ns |   1.692 ns |  1.00 | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U2 |     584.14 ns |   2.932 ns |   2.599 ns |  0.54 | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U2 |   1,088.65 ns |   3.987 ns |   3.730 ns |  1.00 | 0.0134 |      - |     - |      88 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U4** |     **515.89 ns** |   **3.217 ns** |   **3.009 ns** |  **0.51** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U4 |   1,016.92 ns |   1.898 ns |   1.585 ns |  1.00 | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U4 |     587.27 ns |   1.744 ns |   1.631 ns |  0.46 | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U4 |   1,270.22 ns |   4.703 ns |   4.399 ns |  1.00 | 0.0134 |      - |     - |      88 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U8** |     **555.69 ns** |   **2.327 ns** |   **2.177 ns** |  **0.33** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U8 |   1,705.83 ns |   8.457 ns |   7.911 ns |  1.00 | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |               |            |            |       |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U8 |     636.42 ns |   1.759 ns |   1.646 ns |  0.33 | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U8 |   1,927.93 ns |   7.004 ns |   6.551 ns |  1.00 | 0.0114 |      - |     - |      88 B |
