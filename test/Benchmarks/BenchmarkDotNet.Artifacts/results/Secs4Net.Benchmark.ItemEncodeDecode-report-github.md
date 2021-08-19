``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1165 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.7.21379.14
  [Host]     : .NET 6.0.0 (6.0.21.37719), X64 RyuJIT
  Job-OMATBZ : .NET 6.0.0 (6.0.21.37719), X64 RyuJIT
  Job-NJTDJS : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|               Method |              Runtime | Size |  Format |         Mean |      Error |     StdDev |        Ratio | RatioSD | Allocated |
|--------------------- |--------------------- |----- |-------- |-------------:|-----------:|-----------:|-------------:|--------:|----------:|
|             **EncodeTo** |             **.NET 6.0** |    **0** |    **List** |     **77.63 ns** |   **0.582 ns** |   **0.544 ns** | **1.60x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |    List |    123.98 ns |   1.374 ns |   1.285 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |    List |     56.50 ns |   1.098 ns |   1.128 ns | 2.82x faster |   0.06x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |    List |    159.14 ns |   0.359 ns |   0.336 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |  **Binary** |     **79.07 ns** |   **0.592 ns** |   **0.553 ns** | **1.63x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |  Binary |    128.79 ns |   1.164 ns |   1.089 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |  Binary |     80.14 ns |   0.749 ns |   0.700 ns | 2.75x faster |   0.03x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |  Binary |    220.25 ns |   1.862 ns |   1.742 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** | **Boolean** |     **77.93 ns** |   **0.710 ns** |   **0.629 ns** | **1.65x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 | Boolean |    128.45 ns |   1.228 ns |   1.148 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 | Boolean |     82.29 ns |   1.653 ns |   2.150 ns | 2.57x faster |   0.07x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 | Boolean |    213.17 ns |   0.540 ns |   0.479 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |   **ASCII** |     **78.85 ns** |   **1.007 ns** |   **0.942 ns** | **1.63x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |   ASCII |    128.82 ns |   0.646 ns |   0.604 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |   ASCII |     74.87 ns |   0.967 ns |   0.904 ns | 3.17x faster |   0.04x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |   ASCII |    237.45 ns |   0.330 ns |   0.293 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |    **JIS8** |     **76.72 ns** |   **0.525 ns** |   **0.466 ns** | **1.69x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |    JIS8 |    129.15 ns |   0.960 ns |   0.898 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |    JIS8 |     77.56 ns |   0.737 ns |   0.615 ns | 3.10x faster |   0.03x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |    JIS8 |    240.77 ns |   1.675 ns |   1.567 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I8** |     **78.35 ns** |   **0.441 ns** |   **0.412 ns** | **1.56x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I8 |    122.34 ns |   1.036 ns |   0.969 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I8 |     75.77 ns |   0.647 ns |   0.540 ns | 3.03x faster |   0.03x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I8 |    229.56 ns |   0.700 ns |   0.654 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I1** |     **78.16 ns** |   **0.218 ns** |   **0.193 ns** | **1.65x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I1 |    129.12 ns |   1.585 ns |   1.482 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I1 |     86.64 ns |   1.749 ns |   2.148 ns | 2.86x faster |   0.08x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I1 |    246.28 ns |   0.567 ns |   0.503 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I2** |     **79.02 ns** |   **0.632 ns** |   **0.591 ns** | **1.61x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I2 |    126.86 ns |   0.917 ns |   0.857 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I2 |     76.72 ns |   1.066 ns |   0.945 ns | 3.08x faster |   0.04x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I2 |    236.06 ns |   0.577 ns |   0.540 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I4** |     **79.91 ns** |   **0.580 ns** |   **0.543 ns** | **1.54x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I4 |    123.25 ns |   1.479 ns |   1.383 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I4 |     79.37 ns |   0.818 ns |   0.765 ns | 2.99x faster |   0.03x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I4 |    237.84 ns |   0.988 ns |   0.876 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **F8** |     **79.25 ns** |   **0.558 ns** |   **0.522 ns** | **1.56x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      F8 |    123.55 ns |   0.737 ns |   0.689 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      F8 |     76.89 ns |   0.749 ns |   0.700 ns | 3.08x faster |   0.03x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      F8 |    236.51 ns |   1.391 ns |   1.302 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **F4** |     **78.60 ns** |   **0.642 ns** |   **0.601 ns** | **1.55x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      F4 |    122.18 ns |   0.385 ns |   0.322 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      F4 |     78.66 ns |   0.624 ns |   0.583 ns | 3.05x faster |   0.02x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      F4 |    240.21 ns |   0.373 ns |   0.331 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U8** |     **78.56 ns** |   **1.002 ns** |   **0.837 ns** | **1.57x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U8 |    123.10 ns |   0.938 ns |   0.877 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U8 |     80.69 ns |   0.604 ns |   0.535 ns | 2.89x faster |   0.02x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U8 |    233.11 ns |   1.268 ns |   1.186 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U1** |     **79.17 ns** |   **0.765 ns** |   **0.715 ns** | **1.61x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U1 |    127.54 ns |   0.866 ns |   0.767 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U1 |     77.22 ns |   0.600 ns |   0.561 ns | 3.29x faster |   0.03x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U1 |    254.28 ns |   0.904 ns |   0.845 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U2** |     **78.88 ns** |   **0.369 ns** |   **0.345 ns** | **1.60x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U2 |    126.44 ns |   0.780 ns |   0.730 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U2 |     81.52 ns |   0.730 ns |   0.683 ns | 2.96x faster |   0.03x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U2 |    240.96 ns |   0.506 ns |   0.423 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U4** |     **78.78 ns** |   **0.279 ns** |   **0.233 ns** | **1.56x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U4 |    122.65 ns |   1.082 ns |   1.012 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U4 |     77.53 ns |   1.210 ns |   1.073 ns | 3.08x faster |   0.04x |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U4 |    238.85 ns |   0.905 ns |   0.847 ns |     baseline |         |         - |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |    **List** |  **7,665.52 ns** |  **54.367 ns** |  **50.855 ns** | **3.19x faster** |   **0.03x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |    List | 24,476.02 ns |  88.709 ns |  82.979 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |    List | 42,601.94 ns | 317.011 ns | 296.533 ns | 2.26x faster |   0.02x |   8,248 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |    List | 96,065.73 ns | 225.378 ns | 188.201 ns |     baseline |         |   8,273 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |  **Binary** |    **110.97 ns** |   **0.764 ns** |   **0.715 ns** | **1.72x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |  Binary |    191.34 ns |   0.501 ns |   0.444 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |  Binary |    203.67 ns |   1.092 ns |   1.021 ns | 1.96x faster |   0.01x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |  Binary |    399.50 ns |   2.323 ns |   2.173 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** | **Boolean** |    **109.24 ns** |   **0.954 ns** |   **0.893 ns** | **1.85x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 | Boolean |    202.73 ns |   2.201 ns |   1.951 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 | Boolean |    200.03 ns |   0.493 ns |   0.437 ns | 2.08x faster |   0.01x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 | Boolean |    416.53 ns |   1.958 ns |   1.831 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |   **ASCII** |    **134.26 ns** |   **0.544 ns** |   **0.454 ns** | **8.85x faster** |   **0.07x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |   ASCII |  1,189.20 ns |   8.778 ns |   8.211 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |   ASCII |    393.33 ns |   2.263 ns |   2.117 ns | 4.41x faster |   0.03x |   2,256 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |   ASCII |  1,733.04 ns |   6.323 ns |   5.915 ns |     baseline |         |   2,295 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |    **JIS8** | **12,526.26 ns** | **236.531 ns** | **221.252 ns** | **1.46x faster** |   **0.02x** |     **488 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |    JIS8 | 18,219.56 ns |  38.582 ns |  34.202 ns |     baseline |         |     377 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |    JIS8 |  7,655.55 ns |  74.177 ns |  69.385 ns | 1.32x faster |   0.01x |   2,720 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |    JIS8 | 10,133.11 ns |  35.595 ns |  29.723 ns |     baseline |         |   2,664 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I8** |    **655.50 ns** |   **4.087 ns** |   **3.823 ns** | **2.53x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I8 |  1,656.72 ns |  10.958 ns |  10.250 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I8 |    572.11 ns |  11.369 ns |  13.534 ns | 3.26x faster |   0.08x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I8 |  1,852.06 ns |   4.661 ns |   4.131 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I1** |    **119.83 ns** |   **0.469 ns** |   **0.439 ns** | **1.68x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I1 |    201.78 ns |   0.704 ns |   0.624 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I1 |    211.38 ns |   1.628 ns |   1.523 ns | 1.96x faster |   0.02x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I1 |    414.60 ns |   1.238 ns |   1.034 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I2** |    **636.38 ns** |   **3.180 ns** |   **2.974 ns** | **1.54x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I2 |    977.27 ns |   6.629 ns |   6.201 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I2 |    615.92 ns |   7.796 ns |   7.292 ns | 1.93x faster |   0.03x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I2 |  1,188.77 ns |   4.999 ns |   4.431 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I4** |    **479.37 ns** |   **1.483 ns** |   **1.315 ns** | **2.04x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I4 |    977.38 ns |   5.371 ns |   4.761 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I4 |    502.30 ns |   1.800 ns |   1.684 ns | 2.35x faster |   0.01x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I4 |  1,180.49 ns |   2.399 ns |   1.873 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **F8** |    **580.53 ns** |   **2.621 ns** |   **2.451 ns** | **4.76x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      F8 |  2,765.55 ns |  12.086 ns |  11.305 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      F8 |    775.24 ns |   5.011 ns |   4.185 ns | 3.71x faster |   0.02x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      F8 |  2,872.12 ns |   4.024 ns |   3.141 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **F4** |    **626.89 ns** |   **3.850 ns** |   **3.601 ns** | **3.42x faster** |   **0.03x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      F4 |  2,140.20 ns |   9.167 ns |   8.127 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      F4 |    737.98 ns |   6.335 ns |   5.926 ns | 3.05x faster |   0.02x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      F4 |  2,254.12 ns |   9.918 ns |   9.278 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U8** |    **681.17 ns** |   **3.410 ns** |   **3.189 ns** | **2.42x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U8 |  1,647.42 ns |   2.690 ns |   2.516 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U8 |    603.09 ns |   9.195 ns |   8.601 ns | 3.08x faster |   0.05x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U8 |  1,858.82 ns |   6.162 ns |   5.463 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U1** |    **111.24 ns** |   **0.789 ns** |   **0.738 ns** | **1.73x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U1 |    191.85 ns |   0.884 ns |   0.784 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U1 |    203.99 ns |   2.610 ns |   2.441 ns | 2.06x faster |   0.03x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U1 |    419.53 ns |   1.870 ns |   1.749 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U2** |    **468.29 ns** |   **1.925 ns** |   **1.800 ns** | **1.59x faster** |   **0.01x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U2 |    743.37 ns |   2.690 ns |   2.516 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U2 |    474.12 ns |   3.676 ns |   3.259 ns | 2.06x faster |   0.01x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U2 |    975.68 ns |   1.616 ns |   1.350 ns |     baseline |         |      88 B |
|                      |                      |      |         |              |            |            |              |         |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U4** |    **484.14 ns** |   **3.226 ns** |   **3.018 ns** | **2.03x faster** |   **0.02x** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U4 |    984.86 ns |   7.557 ns |   7.069 ns |     baseline |         |      40 B |
|                      |                      |      |         |              |            |            |              |         |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U4 |    486.78 ns |   3.600 ns |   3.367 ns | 2.45x faster |   0.02x |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U4 |  1,192.01 ns |   6.914 ns |   6.467 ns |     baseline |         |      88 B |
