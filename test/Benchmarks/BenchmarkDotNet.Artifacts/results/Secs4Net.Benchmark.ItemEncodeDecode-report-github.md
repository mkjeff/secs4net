``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.400
  [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  Job-AITDSN : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  Job-TPKEIE : .NET Framework 4.8 (4.8.4515.0), X64 RyuJIT


```
|               Method |              Runtime | Size |  Format |         Mean |      Error |     StdDev |        Ratio | RatioSD | Allocated |
|--------------------- |--------------------- |----- |-------- |-------------:|-----------:|-----------:|-------------:|--------:|----------:|
|             **EncodeTo** |             **.NET 6.0** |    **0** |    **List** |     **68.09 ns** |   **0.318 ns** |   **0.297 ns** | **1.79x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |    List |    121.71 ns |   0.595 ns |   0.557 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |    List |     61.56 ns |   0.212 ns |   0.198 ns | 2.68x faster |   0.02x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |    List |    164.86 ns |   0.737 ns |   0.689 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |  **Binary** |     **70.73 ns** |   **0.467 ns** |   **0.437 ns** | **1.80x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |  Binary |    127.25 ns |   0.521 ns |   0.488 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |  Binary |     80.88 ns |   0.413 ns |   0.386 ns | 2.76x faster |   0.02x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |  Binary |    222.95 ns |   0.756 ns |   0.707 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** | **Boolean** |     **70.38 ns** |   **0.345 ns** |   **0.323 ns** | **1.88x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 | Boolean |    132.50 ns |   0.675 ns |   0.631 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 | Boolean |     77.22 ns |   0.468 ns |   0.438 ns | 2.82x faster |   0.02x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 | Boolean |    217.67 ns |   0.805 ns |   0.753 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |   **ASCII** |     **72.22 ns** |   **0.282 ns** |   **0.264 ns** | **1.78x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |   ASCII |    128.86 ns |   0.514 ns |   0.481 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |   ASCII |     79.98 ns |   0.337 ns |   0.299 ns | 3.09x faster |   0.02x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |   ASCII |    247.36 ns |   0.809 ns |   0.756 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |    **JIS8** |     **72.07 ns** |   **0.548 ns** |   **0.513 ns** | **1.78x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |    JIS8 |    128.61 ns |   0.482 ns |   0.376 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |    JIS8 |     81.04 ns |   0.612 ns |   0.572 ns | 3.12x faster |   0.03x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |    JIS8 |    253.07 ns |   0.846 ns |   0.750 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I8** |     **71.51 ns** |   **0.423 ns** |   **0.395 ns** | **1.91x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I8 |    136.19 ns |   0.912 ns |   0.808 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I8 |     78.58 ns |   0.660 ns |   0.618 ns | 3.23x faster |   0.03x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I8 |    254.06 ns |   2.034 ns |   1.902 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I1** |     **76.18 ns** |   **0.371 ns** |   **0.347 ns** | **1.77x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I1 |    134.65 ns |   0.637 ns |   0.596 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I1 |     77.86 ns |   0.308 ns |   0.288 ns | 3.23x faster |   0.02x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I1 |    251.60 ns |   0.927 ns |   0.822 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I2** |     **71.45 ns** |   **0.454 ns** |   **0.402 ns** | **1.90x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I2 |    135.59 ns |   0.745 ns |   0.660 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I2 |     78.15 ns |   0.300 ns |   0.281 ns | 3.17x faster |   0.01x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I2 |    247.93 ns |   0.858 ns |   0.802 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I4** |     **75.25 ns** |   **0.439 ns** |   **0.411 ns** | **1.80x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I4 |    135.76 ns |   0.606 ns |   0.537 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I4 |     78.30 ns |   0.261 ns |   0.218 ns | 3.23x faster |   0.02x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I4 |    253.22 ns |   1.131 ns |   1.058 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **F8** |     **75.91 ns** |   **0.351 ns** |   **0.293 ns** | **1.78x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      F8 |    134.98 ns |   0.507 ns |   0.396 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      F8 |     83.18 ns |   0.416 ns |   0.389 ns | 3.06x faster |   0.02x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      F8 |    254.24 ns |   1.302 ns |   1.218 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **F4** |     **71.38 ns** |   **0.208 ns** |   **0.184 ns** | **1.89x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      F4 |    135.09 ns |   0.719 ns |   0.672 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      F4 |     76.65 ns |   0.401 ns |   0.375 ns | 3.30x faster |   0.02x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      F4 |    253.06 ns |   1.174 ns |   1.041 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U8** |     **75.63 ns** |   **0.463 ns** |   **0.433 ns** | **1.80x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U8 |    135.88 ns |   0.921 ns |   0.816 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U8 |     76.81 ns |   0.348 ns |   0.325 ns | 3.13x faster |   0.02x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U8 |    240.25 ns |   0.916 ns |   0.812 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U1** |     **71.64 ns** |   **0.330 ns** |   **0.292 ns** | **1.81x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U1 |    129.82 ns |   0.497 ns |   0.465 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U1 |     83.02 ns |   0.441 ns |   0.391 ns | 3.11x faster |   0.02x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U1 |    258.07 ns |   1.069 ns |   0.947 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U2** |     **72.34 ns** |   **0.351 ns** |   **0.329 ns** | **1.88x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U2 |    135.91 ns |   0.758 ns |   0.709 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U2 |     81.45 ns |   0.461 ns |   0.431 ns | 3.05x faster |   0.02x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U2 |    248.19 ns |   1.021 ns |   0.905 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U4** |     **75.19 ns** |   **1.129 ns** |   **1.057 ns** | **1.78x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U4 |    133.83 ns |   0.634 ns |   0.593 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U4 |     78.69 ns |   0.462 ns |   0.432 ns | 3.06x faster |   0.04x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U4 |    240.67 ns |   3.022 ns |   2.827 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |    **List** |  **8,060.95 ns** |  **45.383 ns** |  **42.452 ns** | **3.19x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |    List | 25,689.22 ns |  69.083 ns |  57.687 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |    List | 42,474.13 ns | 288.288 ns | 269.665 ns | 2.34x faster |   0.02x |   8,248 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |    List | 99,397.86 ns | 190.173 ns | 148.474 ns |     baseline |         |   8,273 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |  **Binary** |    **102.43 ns** |   **0.495 ns** |   **0.463 ns** | **2.04x faster** |   **0.04x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |  Binary |    209.03 ns |   3.439 ns |   3.822 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |  Binary |    192.40 ns |   0.849 ns |   0.794 ns | 2.14x faster |   0.02x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |  Binary |    411.43 ns |   2.963 ns |   2.771 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** | **Boolean** |    **104.05 ns** |   **0.669 ns** |   **0.625 ns** | **2.00x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 | Boolean |    208.43 ns |   0.807 ns |   0.755 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 | Boolean |    196.17 ns |   1.046 ns |   0.927 ns | 2.19x faster |   0.01x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 | Boolean |    429.00 ns |   1.948 ns |   1.822 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |   **ASCII** |    **125.35 ns** |   **1.267 ns** |   **1.185 ns** | **9.72x faster** |   **0.12x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |   ASCII |  1,217.92 ns |  10.890 ns |   9.093 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |   ASCII |    411.25 ns |   3.811 ns |   3.565 ns | 4.52x faster |   0.04x |   2,256 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |   ASCII |  1,860.28 ns |   6.170 ns |   5.771 ns |     baseline |         |   2,295 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |    **JIS8** | **12,657.63 ns** | **113.218 ns** | **105.905 ns** | **1.46x faster** |   **0.01x** |     **488 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |    JIS8 | 18,452.62 ns |  94.896 ns |  88.766 ns |     baseline |         |     377 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |    JIS8 |  7,794.07 ns |  52.412 ns |  46.462 ns | 1.34x faster |   0.01x |   2,720 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |    JIS8 | 10,469.89 ns |  73.393 ns |  65.061 ns |     baseline |         |   2,664 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I8** |    **658.30 ns** |   **3.526 ns** |   **3.125 ns** | **2.58x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I8 |  1,698.76 ns |   6.821 ns |   6.380 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I8 |    782.87 ns |   4.045 ns |   3.586 ns | 2.41x faster |   0.02x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I8 |  1,887.06 ns |   8.866 ns |   7.859 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I1** |     **98.87 ns** |   **0.458 ns** |   **0.428 ns** | **2.11x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I1 |    208.32 ns |   0.690 ns |   0.645 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I1 |    197.75 ns |   1.326 ns |   1.240 ns | 2.21x faster |   0.02x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I1 |    437.18 ns |   1.883 ns |   1.762 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I2** |    **620.09 ns** |   **3.625 ns** |   **3.391 ns** | **1.59x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I2 |    985.49 ns |   3.918 ns |   3.665 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I2 |    705.69 ns |   3.188 ns |   2.982 ns | 1.71x faster |   0.01x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I2 |  1,204.88 ns |   5.626 ns |   5.262 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I4** |    **474.03 ns** |   **2.807 ns** |   **2.625 ns** | **2.22x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I4 |  1,051.54 ns |   7.032 ns |   6.578 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I4 |    531.92 ns |   4.509 ns |   4.218 ns | 2.38x faster |   0.02x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I4 |  1,268.20 ns |   2.566 ns |   2.400 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **F8** |    **575.23 ns** |   **1.538 ns** |   **1.200 ns** | **5.95x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      F8 |  3,424.01 ns |  10.311 ns |   9.645 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      F8 |    651.95 ns |   3.405 ns |   2.843 ns | 4.47x faster |   0.02x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      F8 |  2,912.02 ns |   8.468 ns |   7.507 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **F4** |    **518.77 ns** |   **1.235 ns** |   **1.155 ns** | **4.91x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      F4 |  2,549.24 ns |   6.352 ns |   5.631 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      F4 |    625.16 ns |   2.480 ns |   2.320 ns | 3.46x faster |   0.02x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      F4 |  2,161.59 ns |   7.680 ns |   7.184 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U8** |    **485.51 ns** |   **2.135 ns** |   **1.893 ns** | **3.46x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U8 |  1,679.37 ns |  10.257 ns |   8.008 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U8 |    520.84 ns |   3.309 ns |   2.933 ns | 3.57x faster |   0.02x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U8 |  1,861.83 ns |   7.234 ns |   6.412 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U1** |    **100.92 ns** |   **0.634 ns** |   **0.593 ns** | **2.05x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U1 |    207.27 ns |   1.569 ns |   1.468 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U1 |    195.29 ns |   1.921 ns |   1.604 ns | 2.23x faster |   0.02x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U1 |    436.44 ns |   1.642 ns |   1.536 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U2** |    **625.14 ns** |   **2.958 ns** |   **2.622 ns** | **1.21x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U2 |    758.02 ns |   3.917 ns |   3.664 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U2 |    514.83 ns |   3.785 ns |   3.355 ns | 2.01x faster |   0.02x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U2 |  1,035.61 ns |   3.898 ns |   3.646 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U4** |    **475.61 ns** |   **2.080 ns** |   **1.946 ns** | **2.24x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U4 |  1,064.72 ns |   4.546 ns |   4.030 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U4 |    499.87 ns |   2.803 ns |   2.622 ns | 2.59x faster |   0.02x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U4 |  1,294.55 ns |   5.752 ns |   5.380 ns |     baseline |         |      88 B |
