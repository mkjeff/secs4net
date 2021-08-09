``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1151 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.6.21355.2
  [Host]     : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-PSNALV : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-DJMMZE : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|               Method |              Runtime | Size |  Format |         Mean |      Error |     StdDev |        Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|--------------------- |--------------------- |----- |-------- |-------------:|-----------:|-----------:|-------------:|--------:|-------:|-------:|------:|----------:|
|             **EncodeTo** |             **.NET 6.0** |    **0** |   **ASCII** |     **77.82 ns** |   **0.396 ns** |   **0.371 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |   ASCII |    130.76 ns |   0.911 ns |   0.807 ns | 1.68x slower |   0.01x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |   ASCII |     97.16 ns |   0.814 ns |   0.762 ns |     baseline |         |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |   ASCII |    215.59 ns |   0.324 ns |   0.287 ns | 2.22x slower |   0.02x |      - |      - |     - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |  **Binary** |     **84.24 ns** |   **1.105 ns** |   **0.979 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |  Binary |    140.41 ns |   0.980 ns |   0.869 ns | 1.67x slower |   0.02x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |  Binary |     97.32 ns |   0.335 ns |   0.313 ns |     baseline |         |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |  Binary |    216.30 ns |   0.385 ns |   0.341 ns | 2.22x slower |   0.01x |      - |      - |     - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** | **Boolean** |     **88.81 ns** |   **0.311 ns** |   **0.276 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 | Boolean |    136.17 ns |   2.117 ns |   1.981 ns | 1.54x slower |   0.02x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 | Boolean |    101.21 ns |   0.225 ns |   0.211 ns |     baseline |         |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 | Boolean |    221.19 ns |   0.510 ns |   0.477 ns | 2.19x slower |   0.01x |      - |      - |     - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **F4** |     **80.56 ns** |   **0.325 ns** |   **0.304 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      F4 |    138.27 ns |   1.509 ns |   1.412 ns | 1.72x slower |   0.02x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      F4 |    113.12 ns |   0.390 ns |   0.365 ns |     baseline |         |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      F4 |    218.89 ns |   0.777 ns |   0.727 ns | 1.93x slower |   0.01x |      - |      - |     - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **F8** |     **82.97 ns** |   **0.206 ns** |   **0.193 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      F8 |    137.30 ns |   0.806 ns |   0.754 ns | 1.65x slower |   0.01x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      F8 |     92.45 ns |   1.115 ns |   1.043 ns |     baseline |         |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      F8 |    217.84 ns |   0.363 ns |   0.340 ns | 2.36x slower |   0.03x |      - |      - |     - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I1** |     **84.45 ns** |   **0.495 ns** |   **0.463 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I1 |    133.37 ns |   0.714 ns |   0.668 ns | 1.58x slower |   0.01x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I1 |     94.32 ns |   0.306 ns |   0.271 ns |     baseline |         |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I1 |    218.19 ns |   1.417 ns |   1.325 ns | 2.31x slower |   0.01x |      - |      - |     - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I2** |     **82.62 ns** |   **0.409 ns** |   **0.383 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I2 |    136.43 ns |   0.837 ns |   0.783 ns | 1.65x slower |   0.01x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I2 |     99.80 ns |   0.341 ns |   0.319 ns |     baseline |         |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I2 |    217.67 ns |   0.228 ns |   0.202 ns | 2.18x slower |   0.01x |      - |      - |     - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I4** |     **82.91 ns** |   **0.519 ns** |   **0.486 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I4 |    135.47 ns |   0.799 ns |   0.748 ns | 1.63x slower |   0.01x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I4 |    103.86 ns |   0.312 ns |   0.292 ns |     baseline |         |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I4 |    219.28 ns |   0.360 ns |   0.319 ns | 2.11x slower |   0.01x |      - |      - |     - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I8** |     **80.42 ns** |   **0.467 ns** |   **0.437 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I8 |    136.11 ns |   0.929 ns |   0.869 ns | 1.69x slower |   0.01x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I8 |     95.59 ns |   0.398 ns |   0.372 ns |     baseline |         |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I8 |    217.12 ns |   0.336 ns |   0.314 ns | 2.27x slower |   0.01x |      - |      - |     - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |    **JIS8** |     **75.27 ns** |   **0.410 ns** |   **0.384 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |    JIS8 |    130.10 ns |   0.503 ns |   0.470 ns | 1.73x slower |   0.01x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |    JIS8 |     90.06 ns |   0.501 ns |   0.469 ns |     baseline |         |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |    JIS8 |    215.59 ns |   0.645 ns |   0.572 ns | 2.39x slower |   0.01x |      - |      - |     - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |    **List** |     **78.08 ns** |   **0.345 ns** |   **0.323 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |    List |    125.80 ns |   0.507 ns |   0.474 ns | 1.61x slower |   0.01x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |    List |     58.33 ns |   0.282 ns |   0.250 ns |     baseline |         |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |    List |    158.56 ns |   0.323 ns |   0.287 ns | 2.72x slower |   0.01x |      - |      - |     - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U1** |     **83.36 ns** |   **0.360 ns** |   **0.336 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U1 |    139.86 ns |   0.901 ns |   0.843 ns | 1.68x slower |   0.01x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U1 |     95.90 ns |   0.411 ns |   0.384 ns |     baseline |         |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U1 |    216.37 ns |   0.362 ns |   0.321 ns | 2.26x slower |   0.01x |      - |      - |     - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U2** |     **79.27 ns** |   **0.735 ns** |   **0.687 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U2 |    135.98 ns |   0.950 ns |   0.889 ns | 1.72x slower |   0.02x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U2 |    102.42 ns |   0.148 ns |   0.131 ns |     baseline |         |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U2 |    218.29 ns |   0.351 ns |   0.311 ns | 2.13x slower |   0.00x |      - |      - |     - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U4** |     **83.22 ns** |   **0.639 ns** |   **0.598 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U4 |    137.80 ns |   1.124 ns |   1.051 ns | 1.66x slower |   0.01x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U4 |    112.14 ns |   0.772 ns |   0.723 ns |     baseline |         |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U4 |    219.52 ns |   0.790 ns |   0.701 ns | 1.96x slower |   0.01x |      - |      - |     - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U8** |     **79.22 ns** |   **0.684 ns** |   **0.640 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U8 |    137.77 ns |   1.908 ns |   1.785 ns | 1.74x slower |   0.03x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U8 |    100.41 ns |   1.010 ns |   0.844 ns |     baseline |         |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U8 |    217.57 ns |   1.059 ns |   0.990 ns | 2.17x slower |   0.02x |      - |      - |     - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |   **ASCII** |     **98.72 ns** |   **0.328 ns** |   **0.291 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |   ASCII |    177.61 ns |   1.720 ns |   1.608 ns | 1.80x slower |   0.02x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |   ASCII |    211.79 ns |   0.932 ns |   0.778 ns |     baseline |         | 0.0038 |      - |     - |      32 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |   ASCII |    464.85 ns |   1.650 ns |   1.378 ns | 2.19x slower |   0.01x | 0.0048 |      - |     - |      32 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |  **Binary** |     **97.65 ns** |   **0.535 ns** |   **0.447 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |  Binary |    179.14 ns |   1.722 ns |   1.438 ns | 1.83x slower |   0.02x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |  Binary |    140.25 ns |   0.912 ns |   0.853 ns |     baseline |         | 0.0095 |      - |     - |      80 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |  Binary |    294.31 ns |   0.914 ns |   0.810 ns | 2.10x slower |   0.01x | 0.0124 |      - |     - |      80 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** | **Boolean** |    **101.23 ns** |   **1.210 ns** |   **1.132 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 | Boolean |    171.10 ns |   2.849 ns |   2.665 ns | 1.69x slower |   0.03x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 | Boolean |    139.38 ns |   0.517 ns |   0.484 ns |     baseline |         | 0.0095 |      - |     - |      80 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 | Boolean |    283.58 ns |   1.347 ns |   1.194 ns | 2.04x slower |   0.01x | 0.0124 |      - |     - |      80 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **F4** |    **101.13 ns** |   **0.387 ns** |   **0.343 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      F4 |    204.64 ns |   1.571 ns |   1.470 ns | 2.02x slower |   0.02x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      F4 |    135.03 ns |   0.677 ns |   0.634 ns |     baseline |         | 0.0124 |      - |     - |     104 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      F4 |    335.60 ns |   0.643 ns |   0.570 ns | 2.49x slower |   0.01x | 0.0162 |      - |     - |     104 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **F8** |     **99.87 ns** |   **1.160 ns** |   **1.085 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      F8 |    207.85 ns |   1.620 ns |   1.515 ns | 2.08x slower |   0.03x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      F8 |    136.94 ns |   0.819 ns |   0.726 ns |     baseline |         | 0.0172 |      - |     - |     144 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      F8 |    347.37 ns |   1.100 ns |   0.975 ns | 2.54x slower |   0.02x | 0.0224 |      - |     - |     144 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **I1** |     **96.58 ns** |   **1.024 ns** |   **0.957 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      I1 |    169.53 ns |   1.438 ns |   1.201 ns | 1.76x slower |   0.02x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      I1 |    130.25 ns |   0.782 ns |   0.694 ns |     baseline |         | 0.0095 |      - |     - |      80 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      I1 |    293.80 ns |   0.741 ns |   0.619 ns | 2.26x slower |   0.01x | 0.0124 |      - |     - |      80 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **I2** |    **100.75 ns** |   **0.423 ns** |   **0.395 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      I2 |    191.70 ns |   0.735 ns |   0.688 ns | 1.90x slower |   0.01x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      I2 |    136.45 ns |   0.720 ns |   0.674 ns |     baseline |         | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      I2 |    331.03 ns |   0.294 ns |   0.245 ns | 2.43x slower |   0.01x | 0.0138 |      - |     - |      88 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **I4** |    **105.84 ns** |   **0.292 ns** |   **0.273 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      I4 |    187.35 ns |   0.477 ns |   0.446 ns | 1.77x slower |   0.01x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      I4 |    130.09 ns |   0.778 ns |   0.728 ns |     baseline |         | 0.0124 |      - |     - |     104 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      I4 |    338.78 ns |   0.490 ns |   0.409 ns | 2.60x slower |   0.01x | 0.0162 |      - |     - |     104 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **I8** |     **99.53 ns** |   **0.446 ns** |   **0.417 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      I8 |    193.63 ns |   0.789 ns |   0.738 ns | 1.95x slower |   0.01x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      I8 |    145.32 ns |   0.555 ns |   0.519 ns |     baseline |         | 0.0172 |      - |     - |     144 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      I8 |    340.95 ns |   0.900 ns |   0.798 ns | 2.35x slower |   0.01x | 0.0224 |      - |     - |     144 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |    **JIS8** |    **384.38 ns** |   **1.931 ns** |   **1.712 ns** |     **baseline** |        **** | **0.0582** |      **-** |     **-** |     **488 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |    JIS8 |    453.15 ns |   1.243 ns |   1.102 ns | 1.18x slower |   0.00x | 0.0596 |      - |     - |     377 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |    JIS8 |    315.94 ns |   1.244 ns |   1.164 ns |     baseline |         | 0.0315 |      - |     - |     264 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |    JIS8 |    577.89 ns |   2.361 ns |   2.208 ns | 1.83x slower |   0.01x | 0.0334 |      - |     - |     217 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |    **List** |    **162.31 ns** |   **1.063 ns** |   **0.942 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |    List |    380.23 ns |   1.882 ns |   1.761 ns | 2.34x slower |   0.02x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |    List |    489.21 ns |   4.852 ns |   4.538 ns |     baseline |         | 0.0162 |      - |     - |     136 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |    List |  1,091.35 ns |   3.681 ns |   3.443 ns | 2.23x slower |   0.02x | 0.0210 |      - |     - |     136 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **U1** |    **103.04 ns** |   **0.220 ns** |   **0.195 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      U1 |    180.85 ns |   1.883 ns |   1.669 ns | 1.76x slower |   0.02x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      U1 |    144.37 ns |   0.530 ns |   0.442 ns |     baseline |         | 0.0095 |      - |     - |      80 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      U1 |    297.21 ns |   2.679 ns |   2.506 ns | 2.06x slower |   0.02x | 0.0124 |      - |     - |      80 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **U2** |     **98.98 ns** |   **0.873 ns** |   **0.817 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      U2 |    188.32 ns |   1.910 ns |   1.787 ns | 1.90x slower |   0.02x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      U2 |    140.52 ns |   1.298 ns |   1.214 ns |     baseline |         | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      U2 |    341.57 ns |   1.329 ns |   1.178 ns | 2.43x slower |   0.03x | 0.0138 |      - |     - |      88 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **U4** |    **100.30 ns** |   **1.305 ns** |   **1.089 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      U4 |    191.02 ns |   1.886 ns |   1.672 ns | 1.91x slower |   0.02x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      U4 |    132.93 ns |   0.739 ns |   0.655 ns |     baseline |         | 0.0124 |      - |     - |     104 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      U4 |    327.45 ns |   1.435 ns |   1.342 ns | 2.46x slower |   0.02x | 0.0162 |      - |     - |     104 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **U8** |    **101.64 ns** |   **1.162 ns** |   **1.087 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      U8 |    194.98 ns |   1.074 ns |   1.005 ns | 1.92x slower |   0.02x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      U8 |    131.59 ns |   0.567 ns |   0.503 ns |     baseline |         | 0.0172 |      - |     - |     144 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      U8 |    337.85 ns |   1.441 ns |   1.348 ns | 2.57x slower |   0.01x | 0.0224 |      - |     - |     144 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |   **ASCII** |    **135.21 ns** |   **2.050 ns** |   **1.917 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |   ASCII |  1,191.23 ns |   4.305 ns |   3.816 ns | 8.80x slower |   0.13x | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |   ASCII |    426.07 ns |   2.866 ns |   2.681 ns |     baseline |         | 0.2694 | 0.0019 |     - |   2,256 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |   ASCII |  1,739.89 ns |   7.240 ns |   6.418 ns | 4.08x slower |   0.03x | 0.3624 | 0.0019 |     - |   2,295 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |  **Binary** |    **111.86 ns** |   **0.405 ns** |   **0.338 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |  Binary |    204.83 ns |   3.217 ns |   3.009 ns | 1.83x slower |   0.03x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |  Binary |    250.65 ns |   0.879 ns |   0.822 ns |     baseline |         | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |  Binary |    424.14 ns |   3.830 ns |   3.583 ns | 1.69x slower |   0.01x | 0.0138 |      - |     - |      88 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** | **Boolean** |    **107.57 ns** |   **1.061 ns** |   **0.992 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 | Boolean |    196.21 ns |   2.204 ns |   2.062 ns | 1.82x slower |   0.03x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 | Boolean |    246.43 ns |   1.622 ns |   1.518 ns |     baseline |         | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 | Boolean |    423.45 ns |   3.287 ns |   3.074 ns | 1.72x slower |   0.02x | 0.0138 |      - |     - |      88 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **F4** |    **537.59 ns** |   **3.131 ns** |   **2.928 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      F4 |  2,301.37 ns |  18.401 ns |  17.212 ns | 4.28x slower |   0.03x | 0.0038 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      F4 |    677.67 ns |   4.735 ns |   4.430 ns |     baseline |         | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      F4 |  2,164.42 ns |  12.838 ns |  12.009 ns | 3.19x slower |   0.03x | 0.0114 |      - |     - |      88 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **F8** |    **670.74 ns** |   **3.706 ns** |   **3.466 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      F8 |  2,783.41 ns |  20.846 ns |  18.480 ns | 4.15x slower |   0.04x | 0.0038 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      F8 |    699.63 ns |   5.422 ns |   5.072 ns |     baseline |         | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      F8 |  2,907.45 ns |  21.909 ns |  20.493 ns | 4.16x slower |   0.03x | 0.0114 |      - |     - |      88 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I1** |    **120.63 ns** |   **1.119 ns** |   **1.047 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I1 |    195.96 ns |   2.538 ns |   2.374 ns | 1.62x slower |   0.03x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I1 |    239.80 ns |   1.066 ns |   0.945 ns |     baseline |         | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I1 |    425.13 ns |   4.215 ns |   3.737 ns | 1.77x slower |   0.02x | 0.0138 |      - |     - |      88 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I2** |    **604.18 ns** |   **3.118 ns** |   **2.916 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I2 |    985.66 ns |   2.346 ns |   2.080 ns | 1.63x slower |   0.01x | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I2 |    729.97 ns |   3.552 ns |   3.322 ns |     baseline |         | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I2 |  1,234.72 ns |   1.799 ns |   1.683 ns | 1.69x slower |   0.01x | 0.0134 |      - |     - |      88 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I4** |    **486.36 ns** |   **0.631 ns** |   **0.559 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I4 |    983.32 ns |   2.098 ns |   1.860 ns | 2.02x slower |   0.00x | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I4 |    526.06 ns |   2.738 ns |   2.287 ns |     baseline |         | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I4 |  1,220.91 ns |   3.445 ns |   3.222 ns | 2.32x slower |   0.01x | 0.0134 |      - |     - |      88 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I8** |    **521.54 ns** |   **1.477 ns** |   **1.310 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I8 |  1,667.26 ns |   4.834 ns |   4.522 ns | 3.20x slower |   0.01x | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I8 |    557.50 ns |   2.415 ns |   2.141 ns |     baseline |         | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I8 |  1,906.76 ns |  10.283 ns |   9.619 ns | 3.42x slower |   0.02x | 0.0134 |      - |     - |      88 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |    **JIS8** | **18,992.55 ns** | **117.839 ns** | **110.226 ns** |     **baseline** |        **** | **0.0305** |      **-** |     **-** |     **488 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |    JIS8 | 18,269.15 ns |  99.206 ns |  92.797 ns | 1.04x faster |   0.01x | 0.0305 |      - |     - |     377 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |    JIS8 |  9,970.45 ns |  61.518 ns |  51.371 ns |     baseline |         | 0.3204 |      - |     - |   2,720 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |    JIS8 | 10,281.13 ns |  70.657 ns |  66.093 ns | 1.03x slower |   0.01x | 0.4120 |      - |     - |   2,664 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |    **List** |  **7,821.07 ns** |  **32.256 ns** |  **30.173 ns** |     **baseline** |        **** |      **-** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |    List | 24,442.63 ns |  36.412 ns |  30.406 ns | 3.12x slower |   0.01x |      - |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |    List | 40,524.58 ns | 298.827 ns | 249.534 ns |     baseline |         | 0.9766 |      - |     - |   8,248 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |    List | 93,308.42 ns | 186.098 ns | 174.076 ns | 2.30x slower |   0.01x | 1.2207 |      - |     - |   8,273 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U1** |    **112.15 ns** |   **0.512 ns** |   **0.479 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U1 |    205.12 ns |   1.081 ns |   0.959 ns | 1.83x slower |   0.01x | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U1 |    232.63 ns |   0.312 ns |   0.292 ns |     baseline |         | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U1 |    426.87 ns |   1.542 ns |   1.288 ns | 1.84x slower |   0.01x | 0.0138 |      - |     - |      88 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U2** |    **485.33 ns** |   **0.882 ns** |   **0.782 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U2 |    784.54 ns |   1.611 ns |   1.428 ns | 1.62x slower |   0.00x | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U2 |    592.31 ns |   2.107 ns |   1.868 ns |     baseline |         | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U2 |    988.69 ns |   6.299 ns |   5.260 ns | 1.67x slower |   0.01x | 0.0134 |      - |     - |      88 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U4** |    **477.46 ns** |   **3.093 ns** |   **2.742 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U4 |    986.93 ns |   3.458 ns |   3.065 ns | 2.07x slower |   0.01x | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U4 |    530.57 ns |   6.426 ns |   5.697 ns |     baseline |         | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U4 |  1,231.19 ns |   2.765 ns |   2.587 ns | 2.32x slower |   0.03x | 0.0134 |      - |     - |      88 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U8** |    **515.38 ns** |   **1.352 ns** |   **1.264 ns** |     **baseline** |        **** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U8 |  1,675.04 ns |   8.719 ns |   8.156 ns | 3.25x slower |   0.02x | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U8 |    648.84 ns |   4.519 ns |   4.227 ns |     baseline |         | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U8 |  1,911.06 ns |   4.171 ns |   3.697 ns | 2.95x slower |   0.02x | 0.0114 |      - |     - |      88 B |
