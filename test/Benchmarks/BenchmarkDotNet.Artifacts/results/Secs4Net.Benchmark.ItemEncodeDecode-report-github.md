``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1151 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.6.21355.2
  [Host]     : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-GAZESS : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-HKKFQD : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|               Method |              Runtime | Size |  Format |          Mean |        Error |       StdDev |        Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|--------------------- |--------------------- |----- |-------- |--------------:|-------------:|-------------:|-------------:|--------:|-------:|-------:|------:|----------:|
|             **EncodeTo** |             **.NET 6.0** |    **0** |   **ASCII** |      **73.62 ns** |     **0.920 ns** |     **0.768 ns** | **1.74x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |   ASCII |     128.02 ns |     1.225 ns |     1.146 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |   ASCII |     104.43 ns |     0.545 ns |     0.510 ns | 2.09x faster |   0.01x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |   ASCII |     218.54 ns |     0.257 ns |     0.240 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |  **Binary** |      **77.98 ns** |     **0.510 ns** |     **0.477 ns** | **1.77x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |  Binary |     137.94 ns |     1.363 ns |     1.275 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |  Binary |     107.27 ns |     0.463 ns |     0.433 ns | 2.14x faster |   0.01x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |  Binary |     229.17 ns |     0.271 ns |     0.253 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** | **Boolean** |      **76.36 ns** |     **0.482 ns** |     **0.451 ns** | **1.78x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 | Boolean |     135.69 ns |     1.450 ns |     1.356 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 | Boolean |     119.60 ns |     1.383 ns |     1.294 ns | 1.90x faster |   0.02x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 | Boolean |     227.62 ns |     0.846 ns |     0.792 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **F4** |      **83.40 ns** |     **0.461 ns** |     **0.432 ns** | **1.57x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      F4 |     131.22 ns |     0.846 ns |     0.792 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      F4 |     116.31 ns |     1.951 ns |     1.825 ns | 1.95x faster |   0.03x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      F4 |     226.42 ns |     0.774 ns |     0.724 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **F8** |      **83.76 ns** |     **0.759 ns** |     **0.710 ns** | **1.58x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      F8 |     132.40 ns |     0.950 ns |     0.889 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      F8 |     113.89 ns |     1.513 ns |     1.416 ns | 1.99x faster |   0.03x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      F8 |     227.17 ns |     0.796 ns |     0.745 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I1** |      **76.46 ns** |     **0.617 ns** |     **0.577 ns** | **1.75x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I1 |     133.93 ns |     1.099 ns |     1.028 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I1 |     111.24 ns |     2.192 ns |     2.692 ns | 2.06x faster |   0.05x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I1 |     228.85 ns |     0.773 ns |     0.723 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I2** |      **85.78 ns** |     **0.664 ns** |     **0.621 ns** | **1.52x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I2 |     130.48 ns |     0.842 ns |     0.747 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I2 |     106.86 ns |     1.008 ns |     0.943 ns | 2.13x faster |   0.02x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I2 |     227.18 ns |     0.572 ns |     0.535 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I4** |      **79.45 ns** |     **1.119 ns** |     **1.047 ns** | **1.69x faster** |   **0.03x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I4 |     134.03 ns |     1.529 ns |     1.431 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I4 |     115.42 ns |     0.761 ns |     0.712 ns | 1.97x faster |   0.01x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I4 |     226.94 ns |     0.903 ns |     0.844 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I8** |      **79.31 ns** |     **0.540 ns** |     **0.478 ns** | **1.65x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I8 |     130.90 ns |     1.193 ns |     1.116 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I8 |     114.93 ns |     0.459 ns |     0.383 ns | 1.98x faster |   0.01x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I8 |     227.96 ns |     0.935 ns |     0.874 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |    **JIS8** |      **73.60 ns** |     **1.471 ns** |     **1.964 ns** | **1.73x faster** |   **0.04x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |    JIS8 |     127.67 ns |     0.909 ns |     0.850 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |    JIS8 |     103.85 ns |     0.869 ns |     0.813 ns | 2.10x faster |   0.02x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |    JIS8 |     217.61 ns |     0.255 ns |     0.226 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |    **List** |      **72.67 ns** |     **0.943 ns** |     **0.882 ns** | **1.80x faster** |   **0.03x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |    List |     131.03 ns |     1.218 ns |     1.139 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |    List |      57.82 ns |     1.022 ns |     0.956 ns | 3.98x faster |   0.06x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |    List |     230.09 ns |     1.085 ns |     0.962 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U1** |      **77.97 ns** |     **1.318 ns** |     **1.233 ns** | **1.78x faster** |   **0.04x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U1 |     138.59 ns |     2.519 ns |     2.357 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U1 |     113.92 ns |     0.439 ns |     0.411 ns | 2.01x faster |   0.01x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U1 |     228.69 ns |     0.370 ns |     0.346 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U2** |      **77.04 ns** |     **1.564 ns** |     **1.463 ns** | **1.70x faster** |   **0.04x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U2 |     130.86 ns |     1.385 ns |     1.296 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U2 |     118.76 ns |     1.770 ns |     1.655 ns | 1.91x faster |   0.03x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U2 |     227.08 ns |     0.521 ns |     0.488 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U4** |      **83.54 ns** |     **0.557 ns** |     **0.521 ns** | **1.58x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U4 |     132.10 ns |     1.249 ns |     1.169 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U4 |     116.99 ns |     1.104 ns |     1.033 ns | 1.95x faster |   0.02x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U4 |     228.60 ns |     0.620 ns |     0.518 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U8** |      **75.49 ns** |     **0.339 ns** |     **0.317 ns** | **1.74x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U8 |     131.60 ns |     0.974 ns |     0.911 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U8 |     118.41 ns |     2.223 ns |     2.079 ns | 1.91x faster |   0.03x |      - |      - |     - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U8 |     226.11 ns |     0.593 ns |     0.555 ns |     baseline |         |      - |      - |     - |         - |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |   **ASCII** |      **94.27 ns** |     **0.866 ns** |     **0.810 ns** | **1.93x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |   ASCII |     181.85 ns |     1.463 ns |     1.368 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |   ASCII |     227.16 ns |     1.260 ns |     1.179 ns | 2.16x faster |   0.03x | 0.0038 |      - |     - |      32 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |   ASCII |     489.72 ns |     6.906 ns |     6.460 ns |     baseline |         | 0.0048 |      - |     - |      32 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |  **Binary** |      **96.36 ns** |     **1.898 ns** |     **2.109 ns** | **1.82x faster** |   **0.05x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |  Binary |     175.59 ns |     1.887 ns |     1.765 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |  Binary |     157.45 ns |     0.687 ns |     0.642 ns | 1.98x faster |   0.01x | 0.0095 |      - |     - |      80 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |  Binary |     311.07 ns |     1.659 ns |     1.552 ns |     baseline |         | 0.0124 |      - |     - |      80 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** | **Boolean** |     **102.73 ns** |     **0.977 ns** |     **0.914 ns** | **1.70x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 | Boolean |     174.24 ns |     1.790 ns |     1.674 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 | Boolean |     154.17 ns |     0.436 ns |     0.408 ns | 1.92x faster |   0.01x | 0.0048 |      - |     - |      40 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 | Boolean |     296.55 ns |     0.981 ns |     0.918 ns |     baseline |         | 0.0124 |      - |     - |      80 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **F4** |     **105.28 ns** |     **0.402 ns** |     **0.376 ns** | **1.90x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      F4 |     200.26 ns |     1.930 ns |     1.805 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      F4 |     160.19 ns |     0.605 ns |     0.536 ns | 2.18x faster |   0.01x | 0.0124 |      - |     - |     104 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      F4 |     348.69 ns |     0.921 ns |     0.861 ns |     baseline |         | 0.0162 |      - |     - |     104 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **F8** |     **104.84 ns** |     **0.414 ns** |     **0.387 ns** | **2.04x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      F8 |     213.38 ns |     1.468 ns |     1.373 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      F8 |     157.67 ns |     0.636 ns |     0.531 ns | 2.26x faster |   0.01x | 0.0172 |      - |     - |     144 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      F8 |     356.82 ns |     0.997 ns |     0.933 ns |     baseline |         | 0.0224 |      - |     - |     144 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **I1** |     **101.62 ns** |     **0.495 ns** |     **0.463 ns** | **1.73x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      I1 |     175.30 ns |     1.727 ns |     1.615 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      I1 |     158.89 ns |     0.716 ns |     0.634 ns | 1.88x faster |   0.01x | 0.0095 |      - |     - |      80 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      I1 |     299.42 ns |     0.993 ns |     0.929 ns |     baseline |         | 0.0124 |      - |     - |      80 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **I2** |     **103.32 ns** |     **0.198 ns** |     **0.185 ns** | **1.88x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      I2 |     193.90 ns |     1.402 ns |     1.311 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      I2 |     163.57 ns |     0.698 ns |     0.618 ns | 2.02x faster |   0.01x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      I2 |     330.41 ns |     0.918 ns |     0.859 ns |     baseline |         | 0.0138 |      - |     - |      88 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **I4** |      **96.89 ns** |     **0.741 ns** |     **0.693 ns** | **1.96x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      I4 |     189.55 ns |     1.703 ns |     1.593 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      I4 |     160.25 ns |     1.206 ns |     1.128 ns | 2.10x faster |   0.02x | 0.0124 |      - |     - |     104 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      I4 |     337.20 ns |     0.725 ns |     0.643 ns |     baseline |         | 0.0162 |      - |     - |     104 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **I8** |     **103.37 ns** |     **0.785 ns** |     **0.734 ns** | **1.91x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      I8 |     197.52 ns |     1.609 ns |     1.505 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      I8 |     164.39 ns |     0.641 ns |     0.599 ns | 2.08x faster |   0.01x | 0.0172 |      - |     - |     144 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      I8 |     342.20 ns |     1.275 ns |     1.193 ns |     baseline |         | 0.0224 |      - |     - |     144 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |    **JIS8** |     **404.75 ns** |     **3.631 ns** |     **3.396 ns** | **1.13x faster** |   **0.01x** | **0.0582** |      **-** |     **-** |     **488 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |    JIS8 |     458.16 ns |     2.089 ns |     1.954 ns |     baseline |         | 0.0596 |      - |     - |     377 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |    JIS8 |     325.40 ns |     1.597 ns |     1.494 ns | 1.85x faster |   0.01x | 0.0315 |      - |     - |     264 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |    JIS8 |     601.18 ns |     3.479 ns |     3.254 ns |     baseline |         | 0.0334 |      - |     - |     217 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |    **List** |     **166.81 ns** |     **0.833 ns** |     **0.739 ns** | **3.09x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |    List |     514.83 ns |     2.543 ns |     2.379 ns |     baseline |         | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |    List |     470.16 ns |     2.239 ns |     1.869 ns | 4.36x faster |   0.02x | 0.0162 |      - |     - |     136 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |    List |   2,050.14 ns |     4.304 ns |     4.026 ns |     baseline |         | 0.0191 |      - |     - |     136 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **U1** |      **96.73 ns** |     **1.890 ns** |     **1.857 ns** | **1.82x faster** |   **0.04x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      U1 |     175.88 ns |     1.348 ns |     1.261 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      U1 |     147.87 ns |     0.391 ns |     0.366 ns | 2.08x faster |   0.01x | 0.0095 |      - |     - |      80 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      U1 |     307.69 ns |     1.078 ns |     0.956 ns |     baseline |         | 0.0124 |      - |     - |      80 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **U2** |     **101.68 ns** |     **0.287 ns** |     **0.269 ns** | **1.91x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      U2 |     194.49 ns |     1.971 ns |     1.843 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      U2 |     156.13 ns |     0.506 ns |     0.474 ns | 2.13x faster |   0.01x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      U2 |     332.55 ns |     0.893 ns |     0.835 ns |     baseline |         | 0.0138 |      - |     - |      88 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **U4** |     **104.12 ns** |     **0.625 ns** |     **0.584 ns** | **1.84x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      U4 |     191.66 ns |     1.966 ns |     1.839 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      U4 |     159.26 ns |     0.537 ns |     0.502 ns | 2.12x faster |   0.01x | 0.0124 |      - |     - |     104 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      U4 |     337.61 ns |     0.741 ns |     0.693 ns |     baseline |         | 0.0162 |      - |     - |     104 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** |   **10** |      **U8** |      **99.97 ns** |     **0.416 ns** |     **0.348 ns** | **2.09x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |   10 |      U8 |     208.87 ns |     0.794 ns |     0.743 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 |   10 |      U8 |     161.80 ns |     0.708 ns |     0.662 ns | 2.11x faster |   0.01x | 0.0172 |      - |     - |     144 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |   10 |      U8 |     340.90 ns |     1.090 ns |     1.020 ns |     baseline |         | 0.0224 |      - |     - |     144 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |   **ASCII** |     **134.69 ns** |     **0.474 ns** |     **0.420 ns** | **8.85x faster** |   **0.04x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |   ASCII |   1,191.80 ns |     3.196 ns |     2.990 ns |     baseline |         | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |   ASCII |     436.26 ns |     2.488 ns |     2.206 ns | 3.98x faster |   0.03x | 0.2694 | 0.0019 |     - |   2,256 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |   ASCII |   1,735.39 ns |     5.232 ns |     4.894 ns |     baseline |         | 0.3624 | 0.0019 |     - |   2,295 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |  **Binary** |     **107.29 ns** |     **0.474 ns** |     **0.420 ns** | **1.95x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |  Binary |     209.17 ns |     2.304 ns |     2.155 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |  Binary |     256.78 ns |     1.405 ns |     1.314 ns | 1.71x faster |   0.01x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |  Binary |     439.71 ns |     2.207 ns |     2.064 ns |     baseline |         | 0.0138 |      - |     - |      88 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** | **Boolean** |     **115.24 ns** |     **0.385 ns** |     **0.360 ns** | **1.94x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 | Boolean |     223.53 ns |     0.616 ns |     0.576 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 | Boolean |     273.97 ns |     0.728 ns |     0.646 ns | 1.58x faster |   0.01x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 | Boolean |     431.79 ns |     1.396 ns |     1.306 ns |     baseline |         | 0.0138 |      - |     - |      88 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **F4** |     **537.24 ns** |     **1.527 ns** |     **1.428 ns** | **3.93x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      F4 |   2,111.22 ns |     7.581 ns |     6.330 ns |     baseline |         | 0.0038 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      F4 |     778.97 ns |     5.564 ns |     4.933 ns | 2.78x faster |   0.02x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      F4 |   2,161.82 ns |     7.029 ns |     6.575 ns |     baseline |         | 0.0114 |      - |     - |      88 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **F8** |     **586.91 ns** |     **2.828 ns** |     **2.507 ns** | **4.76x faster** |   **0.03x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      F8 |   2,796.00 ns |    15.862 ns |    14.837 ns |     baseline |         | 0.0038 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      F8 |     816.69 ns |     2.175 ns |     2.034 ns | 3.52x faster |   0.02x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      F8 |   2,872.28 ns |    14.700 ns |    13.751 ns |     baseline |         | 0.0114 |      - |     - |      88 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I1** |     **116.03 ns** |     **0.349 ns** |     **0.291 ns** | **1.72x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I1 |     200.24 ns |     1.456 ns |     1.362 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I1 |     255.24 ns |     0.934 ns |     0.874 ns | 1.70x faster |   0.01x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I1 |     432.94 ns |     1.991 ns |     1.862 ns |     baseline |         | 0.0138 |      - |     - |      88 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I2** |     **610.86 ns** |     **2.523 ns** |     **2.360 ns** | **1.58x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I2 |     967.08 ns |     3.635 ns |     3.400 ns |     baseline |         | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I2 |     748.78 ns |     2.820 ns |     2.638 ns | 1.64x faster |   0.01x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I2 |   1,228.58 ns |     4.069 ns |     3.807 ns |     baseline |         | 0.0134 |      - |     - |      88 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I4** |     **490.30 ns** |     **2.383 ns** |     **2.229 ns** | **2.13x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I4 |   1,046.68 ns |     4.772 ns |     4.464 ns |     baseline |         | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I4 |     546.04 ns |     2.062 ns |     1.828 ns | 2.37x faster |   0.01x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I4 |   1,292.73 ns |     6.730 ns |     6.295 ns |     baseline |         | 0.0134 |      - |     - |      88 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I8** |     **513.12 ns** |     **1.790 ns** |     **1.495 ns** | **3.22x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I8 |   1,652.05 ns |     6.007 ns |     5.619 ns |     baseline |         | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I8 |     575.56 ns |     3.057 ns |     2.860 ns | 3.34x faster |   0.02x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I8 |   1,921.49 ns |     7.191 ns |     6.005 ns |     baseline |         | 0.0114 |      - |     - |      88 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |    **JIS8** |  **20,540.26 ns** |   **355.207 ns** |   **332.261 ns** | **1.13x slower** |   **0.02x** | **0.0305** |      **-** |     **-** |     **488 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |    JIS8 |  18,243.81 ns |   188.995 ns |   176.786 ns |     baseline |         | 0.0305 |      - |     - |     377 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |    JIS8 |   9,804.09 ns |    46.080 ns |    38.479 ns | 1.04x faster |   0.01x | 0.3204 |      - |     - |   2,720 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |    JIS8 |  10,183.54 ns |    87.386 ns |    81.741 ns |     baseline |         | 0.4120 |      - |     - |   2,664 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |    **List** |   **8,124.56 ns** |   **119.167 ns** |   **111.469 ns** | **3.53x faster** |   **0.05x** |      **-** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |    List |  28,666.23 ns |    88.143 ns |    82.449 ns |     baseline |         |      - |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |    List |  43,778.18 ns |   296.623 ns |   247.694 ns | 3.80x faster |   0.04x | 0.9766 |      - |     - |   8,248 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |    List | 166,206.61 ns | 1,231.504 ns | 1,151.950 ns |     baseline |         | 1.2207 |      - |     - |   8,274 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U1** |     **108.05 ns** |     **1.215 ns** |     **1.136 ns** | **1.92x faster** |   **0.04x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U1 |     207.05 ns |     2.845 ns |     2.661 ns |     baseline |         | 0.0062 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U1 |     248.40 ns |     1.408 ns |     1.248 ns | 1.77x faster |   0.01x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U1 |     440.47 ns |     2.734 ns |     2.558 ns |     baseline |         | 0.0138 |      - |     - |      88 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U2** |     **475.11 ns** |     **3.586 ns** |     **3.355 ns** | **1.61x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U2 |     767.87 ns |     3.075 ns |     2.726 ns |     baseline |         | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U2 |     529.93 ns |     2.176 ns |     1.929 ns | 1.88x faster |   0.01x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U2 |     993.65 ns |     6.213 ns |     5.811 ns |     baseline |         | 0.0134 |      - |     - |      88 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U4** |     **474.16 ns** |     **1.887 ns** |     **1.765 ns** | **2.09x faster** |   **0.01x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U4 |     989.63 ns |     3.313 ns |     3.099 ns |     baseline |         | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U4 |     550.19 ns |     3.365 ns |     3.147 ns | 2.36x faster |   0.02x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U4 |   1,296.93 ns |     4.917 ns |     4.599 ns |     baseline |         | 0.0134 |      - |     - |      88 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U8** |     **505.66 ns** |     **2.390 ns** |     **2.236 ns** | **3.27x faster** |   **0.02x** | **0.0048** |      **-** |     **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U8 |   1,655.39 ns |     7.982 ns |     7.467 ns |     baseline |         | 0.0057 |      - |     - |      40 B |
|                      |                      |      |         |               |              |              |              |         |        |        |       |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U8 |     568.84 ns |     3.922 ns |     3.669 ns | 3.36x faster |   0.02x | 0.0105 |      - |     - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U8 |   1,911.93 ns |     7.729 ns |     7.229 ns |     baseline |         | 0.0114 |      - |     - |      88 B |
