``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1237 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-rc.1.21458.32
  [Host]     : .NET 6.0.0 (6.0.21.45113), X64 RyuJIT
  Job-POERRH : .NET 6.0.0 (6.0.21.45113), X64 RyuJIT
  Job-QPPLMB : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|               Method |              Runtime | Size |  Format |         Mean |      Error |     StdDev |        Ratio | RatioSD | Allocated |
|--------------------- |--------------------- |----- |-------- |-------------:|-----------:|-----------:|-------------:|--------:|----------:|
|             **EncodeTo** |             **.NET 6.0** |    **0** |    **List** |     **77.15 ns** |   **0.431 ns** |   **0.382 ns** | **1.61x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |    List |    123.93 ns |   0.619 ns |   0.549 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |    List |     61.31 ns |   0.326 ns |   0.305 ns | 2.62x faster |   0.01x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |    List |    160.83 ns |   0.523 ns |   0.489 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |  **Binary** |     **81.51 ns** |   **0.140 ns** |   **0.124 ns** | **1.58x faster** |   **0.00x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |  Binary |    128.44 ns |   0.357 ns |   0.316 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |  Binary |     76.59 ns |   0.493 ns |   0.461 ns | 2.88x faster |   0.02x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |  Binary |    220.18 ns |   0.560 ns |   0.496 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** | **Boolean** |     **74.43 ns** |   **0.346 ns** |   **0.324 ns** | **1.71x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 | Boolean |    127.01 ns |   0.771 ns |   0.721 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 | Boolean |     79.37 ns |   0.360 ns |   0.337 ns | 2.76x faster |   0.01x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 | Boolean |    219.24 ns |   0.325 ns |   0.272 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |   **ASCII** |     **72.18 ns** |   **0.289 ns** |   **0.256 ns** | **1.79x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |   ASCII |    129.21 ns |   0.431 ns |   0.382 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |   ASCII |     81.97 ns |   0.445 ns |   0.417 ns | 2.98x faster |   0.01x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |   ASCII |    244.56 ns |   0.776 ns |   0.648 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |    **JIS8** |     **80.88 ns** |   **0.240 ns** |   **0.224 ns** | **1.60x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |    JIS8 |    129.32 ns |   0.483 ns |   0.451 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |    JIS8 |     77.74 ns |   0.529 ns |   0.495 ns | 3.14x faster |   0.02x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |    JIS8 |    243.83 ns |   0.648 ns |   0.606 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I8** |     **82.41 ns** |   **0.493 ns** |   **0.461 ns** | **1.53x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I8 |    125.74 ns |   0.578 ns |   0.541 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I8 |     80.91 ns |   0.364 ns |   0.323 ns | 2.97x faster |   0.01x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I8 |    240.55 ns |   0.958 ns |   0.849 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I1** |     **75.14 ns** |   **0.490 ns** |   **0.458 ns** | **1.69x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I1 |    127.21 ns |   0.651 ns |   0.544 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I1 |     82.13 ns |   0.430 ns |   0.402 ns | 3.00x faster |   0.02x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I1 |    245.94 ns |   0.910 ns |   0.760 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I2** |     **74.34 ns** |   **0.510 ns** |   **0.477 ns** | **1.70x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I2 |    126.45 ns |   0.827 ns |   0.773 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I2 |     80.06 ns |   0.290 ns |   0.257 ns | 3.01x faster |   0.01x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I2 |    241.02 ns |   0.589 ns |   0.522 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I4** |     **78.77 ns** |   **0.236 ns** |   **0.221 ns** | **1.60x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I4 |    126.31 ns |   0.467 ns |   0.414 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I4 |     75.19 ns |   0.392 ns |   0.367 ns | 3.30x faster |   0.02x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I4 |    247.63 ns |   1.246 ns |   1.105 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **F8** |     **75.48 ns** |   **0.350 ns** |   **0.328 ns** | **1.67x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      F8 |    126.35 ns |   0.831 ns |   0.777 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      F8 |     79.09 ns |   0.618 ns |   0.578 ns | 3.13x faster |   0.02x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      F8 |    247.90 ns |   0.611 ns |   0.477 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **F4** |     **74.03 ns** |   **0.474 ns** |   **0.420 ns** | **1.70x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      F4 |    126.16 ns |   0.846 ns |   0.660 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      F4 |     80.84 ns |   0.207 ns |   0.173 ns | 3.08x faster |   0.01x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      F4 |    249.20 ns |   0.441 ns |   0.413 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U8** |     **75.75 ns** |   **0.475 ns** |   **0.444 ns** | **1.67x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U8 |    126.54 ns |   1.041 ns |   0.973 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U8 |     80.89 ns |   0.212 ns |   0.177 ns | 3.05x faster |   0.01x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U8 |    246.76 ns |   0.517 ns |   0.458 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U1** |     **78.25 ns** |   **0.395 ns** |   **0.350 ns** | **1.65x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U1 |    128.93 ns |   0.633 ns |   0.592 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U1 |     82.01 ns |   0.445 ns |   0.372 ns | 3.07x faster |   0.01x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U1 |    251.53 ns |   0.904 ns |   0.755 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U2** |     **72.43 ns** |   **0.346 ns** |   **0.324 ns** | **1.74x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U2 |    126.00 ns |   0.644 ns |   0.602 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U2 |     77.94 ns |   0.487 ns |   0.431 ns | 3.09x faster |   0.02x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U2 |    241.12 ns |   0.754 ns |   0.668 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U4** |     **75.24 ns** |   **0.267 ns** |   **0.236 ns** | **1.68x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U4 |    126.43 ns |   0.494 ns |   0.462 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U4 |     77.00 ns |   0.338 ns |   0.300 ns | 3.14x faster |   0.02x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U4 |    242.03 ns |   1.100 ns |   1.029 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |    **List** |  **8,106.25 ns** |  **36.077 ns** |  **33.747 ns** | **3.06x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |    List | 24,809.70 ns |  62.336 ns |  55.259 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |    List | 44,800.07 ns | 179.534 ns | 149.919 ns | 2.18x faster |   0.01x |   8,248 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |    List | 97,643.23 ns | 189.256 ns | 147.758 ns |     baseline |         |   8,273 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |  **Binary** |    **102.68 ns** |   **0.426 ns** |   **0.377 ns** | **1.92x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |  Binary |    197.08 ns |   0.768 ns |   0.719 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |  Binary |    192.74 ns |   0.648 ns |   0.607 ns | 2.12x faster |   0.01x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |  Binary |    409.15 ns |   1.047 ns |   0.980 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** | **Boolean** |    **105.26 ns** |   **0.624 ns** |   **0.583 ns** | **1.89x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 | Boolean |    199.32 ns |   0.721 ns |   0.639 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 | Boolean |    199.20 ns |   0.896 ns |   0.795 ns | 2.09x faster |   0.01x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 | Boolean |    416.42 ns |   1.740 ns |   1.628 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |   **ASCII** |    **130.47 ns** |   **0.403 ns** |   **0.357 ns** | **9.21x faster** |   **0.03x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |   ASCII |  1,202.18 ns |   2.383 ns |   2.113 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |   ASCII |    400.83 ns |   2.155 ns |   2.015 ns | 4.41x faster |   0.02x |   2,256 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |   ASCII |  1,767.70 ns |   3.147 ns |   2.790 ns |     baseline |         |   2,295 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |    **JIS8** | **12,569.08 ns** |  **93.401 ns** |  **87.367 ns** | **1.46x faster** |   **0.01x** |     **488 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |    JIS8 | 18,399.47 ns |  98.047 ns |  86.916 ns |     baseline |         |     377 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |    JIS8 | 10,687.82 ns |  36.547 ns |  34.186 ns | 1.04x slower |   0.01x |   2,720 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |    JIS8 | 10,315.43 ns |  37.828 ns |  33.534 ns |     baseline |         |   2,664 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I8** |    **662.14 ns** |   **1.794 ns** |   **1.678 ns** | **2.53x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I8 |  1,675.74 ns |   5.864 ns |   5.198 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I8 |    775.45 ns |   1.694 ns |   1.584 ns | 2.45x faster |   0.01x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I8 |  1,902.42 ns |   5.206 ns |   4.870 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I1** |    **104.61 ns** |   **0.161 ns** |   **0.126 ns** | **1.90x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I1 |    198.82 ns |   0.888 ns |   0.830 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I1 |    200.21 ns |   0.851 ns |   0.755 ns | 2.07x faster |   0.01x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I1 |    415.29 ns |   1.268 ns |   1.186 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I2** |    **639.24 ns** |   **5.518 ns** |   **5.162 ns** | **1.53x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I2 |    978.46 ns |   3.105 ns |   2.904 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I2 |    728.64 ns |   2.597 ns |   2.430 ns | 1.65x faster |   0.00x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I2 |  1,201.78 ns |   1.933 ns |   1.714 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I4** |    **482.16 ns** |   **0.977 ns** |   **0.816 ns** | **2.07x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I4 |    997.13 ns |   2.900 ns |   2.712 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I4 |    491.31 ns |   4.798 ns |   4.007 ns | 2.44x faster |   0.02x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I4 |  1,201.54 ns |   4.134 ns |   3.866 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **F8** |    **578.49 ns** |   **4.023 ns** |   **3.763 ns** | **5.10x faster** |   **0.04x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      F8 |  2,951.80 ns |   6.740 ns |   5.262 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      F8 |    665.16 ns |   2.576 ns |   2.410 ns | 4.43x faster |   0.02x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      F8 |  2,948.14 ns |  15.283 ns |  14.296 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **F4** |    **538.98 ns** |   **1.289 ns** |   **1.076 ns** | **4.30x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      F4 |  2,318.36 ns |   8.861 ns |   7.855 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      F4 |    632.81 ns |   2.959 ns |   2.768 ns | 3.39x faster |   0.02x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      F4 |  2,146.22 ns |   7.630 ns |   6.371 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U8** |    **505.20 ns** |   **1.930 ns** |   **1.805 ns** | **3.33x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U8 |  1,682.96 ns |   3.722 ns |   3.108 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U8 |    614.82 ns |   1.773 ns |   1.572 ns | 3.11x faster |   0.01x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U8 |  1,910.44 ns |   5.552 ns |   4.922 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U1** |    **103.64 ns** |   **0.347 ns** |   **0.325 ns** | **1.90x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U1 |    197.37 ns |   0.993 ns |   0.929 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U1 |    198.57 ns |   0.752 ns |   0.703 ns | 2.12x faster |   0.01x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U1 |    420.85 ns |   1.296 ns |   1.082 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U2** |    **619.47 ns** |   **1.664 ns** |   **1.475 ns** | **1.29x faster** |   **0.00x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U2 |    796.67 ns |   1.967 ns |   1.643 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U2 |    495.39 ns |   1.786 ns |   1.491 ns | 1.98x faster |   0.02x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U2 |    981.68 ns |  10.177 ns |   9.519 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U4** |    **484.52 ns** |   **1.929 ns** |   **1.710 ns** | **2.05x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U4 |    990.90 ns |   2.223 ns |   2.079 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U4 |    500.54 ns |   2.570 ns |   2.278 ns | 2.26x faster |   0.02x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U4 |  1,131.60 ns |   7.707 ns |   6.832 ns |     baseline |         |      88 B |
