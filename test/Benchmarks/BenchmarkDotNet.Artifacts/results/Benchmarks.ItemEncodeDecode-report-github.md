```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3007/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  Job-ERYFHQ : .NET 6.0.26 (6.0.2623.60508), X64 RyuJIT AVX2
  Job-GVMCZA : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  Job-BVWEYE : .NET Framework 4.8.1 (4.8.9181.0), X64 RyuJIT VectorSize=256


```
| Method               | Runtime            | Size | Format  | Mean          | Error        | StdDev     | Median        | Ratio         | RatioSD | Allocated | Alloc Ratio |
|--------------------- |------------------- |----- |-------- |--------------:|-------------:|-----------:|--------------:|--------------:|--------:|----------:|------------:|
| **EncodeTo**             | **.NET 6.0**           | **0**    | **List**    |      **17.31 ns** |     **0.272 ns** |   **0.255 ns** |      **17.35 ns** |  **4.64x faster** |   **0.07x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 0    | List    |      16.36 ns |     0.291 ns |   0.272 ns |      16.38 ns |  4.92x faster |   0.10x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 0    | List    |      80.40 ns |     1.324 ns |   1.174 ns |      80.43 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 0    | List    |      36.94 ns |     0.729 ns |   0.682 ns |      36.90 ns |  4.06x faster |   0.09x |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0           | 0    | List    |      26.19 ns |     0.294 ns |   0.261 ns |      26.18 ns |  5.72x faster |   0.09x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | List    |     149.83 ns |     2.008 ns |   1.780 ns |     150.02 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **0**    | **Binary**  |      **17.47 ns** |     **0.232 ns** |   **0.205 ns** |      **17.51 ns** |  **2.50x faster** |   **0.05x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 0    | Binary  |      16.98 ns |     0.344 ns |   0.368 ns |      17.04 ns |  2.58x faster |   0.05x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 0    | Binary  |      43.66 ns |     0.612 ns |   0.572 ns |      43.59 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 0    | Binary  |      52.95 ns |     0.643 ns |   0.601 ns |      53.17 ns |  2.70x faster |   0.05x |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0           | 0    | Binary  |      34.77 ns |     0.456 ns |   0.427 ns |      34.75 ns |  4.11x faster |   0.08x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | Binary  |     142.84 ns |     2.235 ns |   2.091 ns |     142.67 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **0**    | **Boolean** |      **17.75 ns** |     **0.327 ns** |   **0.290 ns** |      **17.86 ns** |  **2.48x faster** |   **0.05x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 0    | Boolean |      16.86 ns |     0.337 ns |   0.315 ns |      16.86 ns |  2.61x faster |   0.06x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 0    | Boolean |      44.04 ns |     0.780 ns |   0.730 ns |      43.87 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 0    | Boolean |      50.18 ns |     0.616 ns |   0.576 ns |      50.22 ns |  2.84x faster |   0.04x |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0           | 0    | Boolean |      34.01 ns |     0.573 ns |   0.536 ns |      33.99 ns |  4.19x faster |   0.09x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | Boolean |     142.57 ns |     2.024 ns |   1.893 ns |     142.69 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **0**    | **ASCII**   |      **17.63 ns** |     **0.311 ns** |   **0.305 ns** |      **17.66 ns** |  **2.50x faster** |   **0.06x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 0    | ASCII   |      16.63 ns |     0.241 ns |   0.226 ns |      16.61 ns |  2.65x faster |   0.03x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 0    | ASCII   |      44.02 ns |     0.547 ns |   0.511 ns |      44.03 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 0    | ASCII   |      50.80 ns |     0.465 ns |   0.412 ns |      50.81 ns |  2.81x faster |   0.03x |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0           | 0    | ASCII   |      35.26 ns |     0.631 ns |   0.591 ns |      35.04 ns |  4.06x faster |   0.06x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | ASCII   |     142.90 ns |     1.928 ns |   1.610 ns |     143.11 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **0**    | **JIS8**    |      **17.90 ns** |     **0.327 ns** |   **0.306 ns** |      **17.83 ns** |  **2.34x faster** |   **0.06x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 0    | JIS8    |      16.59 ns |     0.272 ns |   0.227 ns |      16.61 ns |  2.53x faster |   0.06x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 0    | JIS8    |      41.86 ns |     0.726 ns |   0.680 ns |      41.71 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 0    | JIS8    |      53.74 ns |     0.552 ns |   0.516 ns |      53.62 ns |  2.69x faster |   0.03x |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0           | 0    | JIS8    |      34.13 ns |     0.453 ns |   0.402 ns |      34.23 ns |  4.23x faster |   0.08x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | JIS8    |     144.38 ns |     1.886 ns |   1.764 ns |     144.29 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **0**    | **I8**      |      **17.64 ns** |     **0.365 ns** |   **0.342 ns** |      **17.60 ns** |  **2.54x faster** |   **0.05x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 0    | I8      |      16.98 ns |     0.294 ns |   0.275 ns |      16.89 ns |  2.65x faster |   0.05x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 0    | I8      |      45.01 ns |     0.393 ns |   0.328 ns |      45.03 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 0    | I8      |      50.34 ns |     0.561 ns |   0.525 ns |      50.29 ns |  3.79x faster |   1.08x |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0           | 0    | I8      |      34.98 ns |     0.681 ns |   0.637 ns |      35.10 ns |  5.45x faster |   1.53x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | I8      |     239.37 ns |    11.091 ns |  32.701 ns |     253.80 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **0**    | **I1**      |      **31.48 ns** |     **1.032 ns** |   **3.042 ns** |      **32.55 ns** |  **1.58x faster** |   **0.40x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 0    | I1      |      17.55 ns |     0.362 ns |   0.823 ns |      17.58 ns |  2.48x faster |   0.17x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 0    | I1      |      43.78 ns |     0.443 ns |   0.392 ns |      43.86 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 0    | I1      |      81.29 ns |     1.614 ns |   2.784 ns |      81.61 ns |  1.75x faster |   0.07x |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0           | 0    | I1      |      35.04 ns |     0.487 ns |   0.456 ns |      35.09 ns |  4.04x faster |   0.06x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | I1      |     141.78 ns |     1.728 ns |   1.532 ns |     141.93 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **0**    | **I2**      |      **17.67 ns** |     **0.351 ns** |   **0.311 ns** |      **17.69 ns** |  **2.57x faster** |   **0.06x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 0    | I2      |      17.00 ns |     0.237 ns |   0.210 ns |      16.97 ns |  2.67x faster |   0.05x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 0    | I2      |      45.37 ns |     0.785 ns |   0.734 ns |      45.32 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 0    | I2      |      50.14 ns |     0.618 ns |   0.516 ns |      50.24 ns |  2.89x faster |   0.05x |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0           | 0    | I2      |      34.65 ns |     0.341 ns |   0.302 ns |      34.61 ns |  4.18x faster |   0.04x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | I2      |     145.11 ns |     1.656 ns |   1.549 ns |     145.19 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **0**    | **I4**      |      **18.48 ns** |     **0.236 ns** |   **0.209 ns** |      **18.53 ns** |  **2.46x faster** |   **0.05x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 0    | I4      |      17.19 ns |     0.358 ns |   0.317 ns |      17.25 ns |  2.65x faster |   0.06x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 0    | I4      |      45.48 ns |     0.757 ns |   0.708 ns |      45.46 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 0    | I4      |      50.89 ns |     0.606 ns |   0.537 ns |      51.14 ns |  2.79x faster |   0.05x |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0           | 0    | I4      |      35.00 ns |     0.419 ns |   0.371 ns |      34.99 ns |  4.06x faster |   0.08x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | I4      |     142.33 ns |     2.411 ns |   2.255 ns |     142.01 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **0**    | **F8**      |      **17.98 ns** |     **0.378 ns** |   **0.405 ns** |      **17.96 ns** |  **2.36x faster** |   **0.06x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 0    | F8      |      16.82 ns |     0.327 ns |   0.306 ns |      16.86 ns |  2.53x faster |   0.04x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 0    | F8      |      42.52 ns |     0.457 ns |   0.428 ns |      42.53 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 0    | F8      |      50.27 ns |     0.906 ns |   0.848 ns |      50.28 ns |  2.84x faster |   0.06x |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0           | 0    | F8      |      34.93 ns |     0.582 ns |   0.544 ns |      34.97 ns |  4.09x faster |   0.05x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | F8      |     142.92 ns |     1.931 ns |   1.712 ns |     142.70 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **0**    | **F4**      |      **17.89 ns** |     **0.365 ns** |   **0.375 ns** |      **17.89 ns** |  **2.37x faster** |   **0.06x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 0    | F4      |      17.00 ns |     0.322 ns |   0.301 ns |      17.03 ns |  2.49x faster |   0.05x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 0    | F4      |      42.42 ns |     0.594 ns |   0.527 ns |      42.47 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 0    | F4      |      51.08 ns |     1.022 ns |   0.956 ns |      51.05 ns |  2.83x faster |   0.05x |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0           | 0    | F4      |      34.66 ns |     0.398 ns |   0.353 ns |      34.65 ns |  4.16x faster |   0.07x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | F4      |     144.47 ns |     1.835 ns |   1.716 ns |     144.46 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **0**    | **U8**      |      **17.63 ns** |     **0.333 ns** |   **0.312 ns** |      **17.64 ns** |  **2.53x faster** |   **0.04x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 0    | U8      |      16.76 ns |     0.240 ns |   0.420 ns |      16.70 ns |  2.62x faster |   0.09x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 0    | U8      |      44.45 ns |     0.261 ns |   0.218 ns |      44.42 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 0    | U8      |      51.13 ns |     0.545 ns |   0.509 ns |      51.11 ns |  2.79x faster |   0.04x |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0           | 0    | U8      |      34.61 ns |     0.384 ns |   0.340 ns |      34.71 ns |  4.11x faster |   0.07x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | U8      |     142.43 ns |     2.053 ns |   1.920 ns |     142.85 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **0**    | **U1**      |      **17.52 ns** |     **0.320 ns** |   **0.299 ns** |      **17.48 ns** |  **2.47x faster** |   **0.05x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 0    | U1      |      17.02 ns |     0.334 ns |   0.312 ns |      16.99 ns |  2.55x faster |   0.05x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 0    | U1      |      43.31 ns |     0.603 ns |   0.564 ns |      43.39 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 0    | U1      |      52.37 ns |     0.539 ns |   0.478 ns |      52.45 ns |  2.74x faster |   0.04x |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0           | 0    | U1      |      36.30 ns |     0.581 ns |   0.515 ns |      36.41 ns |  3.96x faster |   0.08x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | U1      |     143.76 ns |     1.889 ns |   1.767 ns |     144.20 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **0**    | **U2**      |      **17.44 ns** |     **0.250 ns** |   **0.222 ns** |      **17.45 ns** |  **2.56x faster** |   **0.03x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 0    | U2      |      16.80 ns |     0.304 ns |   0.269 ns |      16.79 ns |  2.66x faster |   0.06x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 0    | U2      |      44.68 ns |     0.529 ns |   0.469 ns |      44.72 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 0    | U2      |      51.30 ns |     0.999 ns |   0.935 ns |      51.40 ns |  2.78x faster |   0.06x |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0           | 0    | U2      |      36.16 ns |     0.446 ns |   0.396 ns |      36.17 ns |  3.94x faster |   0.05x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | U2      |     142.48 ns |     1.718 ns |   1.607 ns |     142.65 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **0**    | **U4**      |      **17.70 ns** |     **0.362 ns** |   **0.356 ns** |      **17.74 ns** |  **2.54x faster** |   **0.06x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 0    | U4      |      17.02 ns |     0.230 ns |   0.215 ns |      17.01 ns |  2.64x faster |   0.04x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 0    | U4      |      44.92 ns |     0.654 ns |   0.612 ns |      44.85 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 0    | U4      |      49.49 ns |     0.997 ns |   1.187 ns |      49.67 ns |  2.88x faster |   0.10x |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0           | 0    | U4      |      35.68 ns |     0.518 ns |   0.484 ns |      35.63 ns |  4.00x faster |   0.11x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | U4      |     142.82 ns |     2.682 ns |   2.509 ns |     143.06 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **1024** | **List**    |   **4,636.55 ns** |    **63.117 ns** |  **59.040 ns** |   **4,645.16 ns** | **10.08x faster** |   **0.23x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 1024 | List    |   3,205.45 ns |    61.659 ns |  65.975 ns |   3,182.99 ns | 14.52x faster |   0.44x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 1024 | List    |  46,704.29 ns |   767.779 ns | 718.181 ns |  46,524.91 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 1024 | List    |  27,221.26 ns |   356.416 ns | 315.953 ns |  27,273.01 ns |  4.14x faster |   0.07x |    8248 B |  1.00x less |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | List    |  23,249.71 ns |   394.656 ns | 369.161 ns |  23,186.11 ns |  4.85x faster |   0.09x |    8248 B |  1.00x less |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | List    | 112,780.83 ns | 1,081.306 ns | 958.549 ns | 112,843.40 ns |      baseline |         |    8273 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **1024** | **Binary**  |      **36.53 ns** |     **0.625 ns** |   **0.585 ns** |      **36.34 ns** |  **2.37x faster** |   **0.04x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 1024 | Binary  |      30.07 ns |     0.339 ns |   0.300 ns |      30.11 ns |  2.87x faster |   0.03x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 1024 | Binary  |      86.32 ns |     0.492 ns |   0.384 ns |      86.33 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 1024 | Binary  |      88.44 ns |     1.242 ns |   1.162 ns |      88.29 ns |  2.57x faster |   0.05x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | Binary  |      70.45 ns |     1.032 ns |   0.861 ns |      70.50 ns |  3.23x faster |   0.04x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | Binary  |     227.69 ns |     2.652 ns |   2.351 ns |     227.62 ns |      baseline |         |      88 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **1024** | **Boolean** |      **33.55 ns** |     **0.576 ns** |   **0.539 ns** |      **33.55 ns** |  **2.61x faster** |   **0.04x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 1024 | Boolean |      29.02 ns |     0.595 ns |   0.585 ns |      29.04 ns |  3.02x faster |   0.07x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 1024 | Boolean |      87.61 ns |     1.077 ns |   1.007 ns |      87.70 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 1024 | Boolean |      91.02 ns |     1.020 ns |   0.904 ns |      90.99 ns |  4.23x faster |   0.18x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | Boolean |      71.93 ns |     1.256 ns |   1.174 ns |      71.99 ns |  5.34x faster |   0.17x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | Boolean |     273.21 ns |    25.047 ns |  73.852 ns |     226.62 ns |      baseline |         |      88 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **1024** | **ASCII**   |      **55.45 ns** |     **0.623 ns** |   **0.552 ns** |      **55.59 ns** | **11.11x faster** |   **0.24x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 1024 | ASCII   |      48.32 ns |     0.651 ns |   0.609 ns |      48.25 ns | 12.75x faster |   0.25x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 1024 | ASCII   |     616.21 ns |     9.265 ns |   8.667 ns |     616.57 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 1024 | ASCII   |     172.38 ns |     3.459 ns |   3.067 ns |     171.81 ns |  4.77x faster |   0.10x |    2104 B |  1.01x less |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | ASCII   |     130.97 ns |     2.626 ns |   3.126 ns |     130.13 ns |  6.28x faster |   0.16x |    2104 B |  1.01x less |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | ASCII   |     822.74 ns |     9.991 ns |   8.343 ns |     823.27 ns |      baseline |         |    2118 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **1024** | **JIS8**    |      **86.38 ns** |     **0.975 ns** |   **0.864 ns** |      **86.41 ns** |  **5.19x faster** |   **0.08x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 1024 | JIS8    |      69.60 ns |     1.364 ns |   1.401 ns |      69.63 ns |  6.44x faster |   0.14x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 1024 | JIS8    |     448.06 ns |     6.668 ns |   6.237 ns |     448.47 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 1024 | JIS8    |     185.96 ns |     2.640 ns |   2.341 ns |     186.29 ns |  3.40x faster |   0.04x |    2104 B |  1.01x less |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | JIS8    |     144.53 ns |     2.258 ns |   1.885 ns |     144.63 ns |  4.38x faster |   0.07x |    2104 B |  1.01x less |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | JIS8    |     632.44 ns |     7.487 ns |   6.252 ns |     633.24 ns |      baseline |         |    2118 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **1024** | **I8**      |     **318.72 ns** |     **5.628 ns** |   **5.264 ns** |     **318.39 ns** |  **3.33x faster** |   **0.07x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 1024 | I8      |     319.43 ns |     6.108 ns |   6.789 ns |     318.01 ns |  3.31x faster |   0.08x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 1024 | I8      |   1,061.79 ns |    12.443 ns |  11.031 ns |   1,064.71 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 1024 | I8      |     380.48 ns |     3.890 ns |   3.248 ns |     379.86 ns |  3.24x faster |   0.04x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | I8      |     359.56 ns |     6.561 ns |   6.137 ns |     359.70 ns |  3.43x faster |   0.06x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | I8      |   1,231.95 ns |    14.382 ns |  12.749 ns |   1,234.76 ns |      baseline |         |      88 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **1024** | **I1**      |      **32.56 ns** |     **0.484 ns** |   **0.404 ns** |      **32.52 ns** |  **2.70x faster** |   **0.06x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 1024 | I1      |      28.17 ns |     0.586 ns |   0.575 ns |      28.19 ns |  3.12x faster |   0.09x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 1024 | I1      |      87.70 ns |     1.463 ns |   1.368 ns |      87.80 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 1024 | I1      |      90.32 ns |     1.196 ns |   0.999 ns |      90.33 ns |  2.53x faster |   0.05x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | I1      |      72.57 ns |     0.815 ns |   0.722 ns |      72.50 ns |  3.15x faster |   0.05x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | I1      |     228.64 ns |     2.883 ns |   2.556 ns |     228.89 ns |      baseline |         |      88 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **1024** | **I2**      |     **283.46 ns** |     **3.074 ns** |   **2.875 ns** |     **283.70 ns** |  **2.04x faster** |   **0.03x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 1024 | I2      |     275.10 ns |     3.027 ns |   2.683 ns |     274.85 ns |  2.10x faster |   0.03x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 1024 | I2      |     577.74 ns |     7.540 ns |   6.296 ns |     578.14 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 1024 | I2      |     343.66 ns |     4.650 ns |   4.122 ns |     343.78 ns |  2.06x faster |   0.03x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | I2      |     402.64 ns |     2.870 ns |   2.544 ns |     403.06 ns |  1.76x faster |   0.02x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | I2      |     709.51 ns |     9.124 ns |   8.088 ns |     710.06 ns |      baseline |         |      88 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **1024** | **I4**      |     **282.60 ns** |     **5.220 ns** |   **4.883 ns** |     **281.91 ns** |  **2.15x faster** |   **0.06x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 1024 | I4      |     283.49 ns |     4.713 ns |   4.178 ns |     282.84 ns |  2.14x faster |   0.05x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 1024 | I4      |     606.46 ns |     9.372 ns |   8.767 ns |     605.15 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 1024 | I4      |     340.46 ns |     5.451 ns |   5.099 ns |     340.98 ns |  2.17x faster |   0.02x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | I4      |     326.31 ns |     5.379 ns |   4.768 ns |     327.08 ns |  2.26x faster |   0.03x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | I4      |     738.12 ns |     7.599 ns |   6.346 ns |     737.27 ns |      baseline |         |      88 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **1024** | **F8**      |     **393.04 ns** |     **3.984 ns** |   **3.726 ns** |     **395.02 ns** |  **4.55x faster** |   **0.07x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 1024 | F8      |     387.68 ns |     3.736 ns |   3.312 ns |     388.18 ns |  4.61x faster |   0.06x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 1024 | F8      |   1,788.64 ns |    16.798 ns |  15.713 ns |   1,792.31 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 1024 | F8      |     454.25 ns |     5.252 ns |   4.656 ns |     453.58 ns |  4.26x faster |   0.08x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | F8      |     436.50 ns |     2.963 ns |   2.627 ns |     435.90 ns |  4.44x faster |   0.05x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | F8      |   1,936.58 ns |    23.962 ns |  21.242 ns |   1,942.11 ns |      baseline |         |      88 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **1024** | **F4**      |     **341.27 ns** |     **5.152 ns** |   **4.819 ns** |     **341.31 ns** |  **3.84x faster** |   **0.08x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 1024 | F4      |     322.67 ns |     4.445 ns |   3.941 ns |     323.98 ns |  4.06x faster |   0.07x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 1024 | F4      |   1,308.85 ns |    16.956 ns |  15.861 ns |   1,313.38 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 1024 | F4      |     402.13 ns |     4.675 ns |   4.145 ns |     401.43 ns |  3.59x faster |   0.09x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | F4      |     368.65 ns |     3.364 ns |   2.982 ns |     369.10 ns |  3.91x faster |   0.08x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | F4      |   1,442.45 ns |    27.819 ns |  26.022 ns |   1,436.60 ns |      baseline |         |      88 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **1024** | **U8**      |     **317.09 ns** |     **3.599 ns** |   **3.190 ns** |     **317.28 ns** |  **3.33x faster** |   **0.07x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 1024 | U8      |     319.93 ns |     2.374 ns |   5.058 ns |     319.00 ns |  3.28x faster |   0.07x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 1024 | U8      |   1,056.70 ns |    14.300 ns |  12.676 ns |   1,060.70 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 1024 | U8      |     670.88 ns |    35.597 ns | 104.960 ns |     707.53 ns |  2.95x faster |   0.57x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | U8      |     364.83 ns |     5.077 ns |   4.749 ns |     363.59 ns |  3.35x faster |   0.06x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | U8      |   1,223.98 ns |    12.735 ns |  10.635 ns |   1,226.31 ns |      baseline |         |      88 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **1024** | **U1**      |      **34.96 ns** |     **0.705 ns** |   **0.625 ns** |      **34.92 ns** |  **2.46x faster** |   **0.07x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 1024 | U1      |      31.42 ns |     0.644 ns |   0.571 ns |      31.58 ns |  2.74x faster |   0.06x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 1024 | U1      |      85.94 ns |     1.109 ns |   1.037 ns |      85.91 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 1024 | U1      |      91.93 ns |     1.269 ns |   1.187 ns |      91.80 ns |  2.48x faster |   0.05x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | U1      |      72.07 ns |     1.401 ns |   1.311 ns |      71.94 ns |  3.17x faster |   0.08x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | U1      |     228.36 ns |     2.747 ns |   2.569 ns |     228.96 ns |      baseline |         |      88 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **1024** | **U2**      |     **278.84 ns** |     **5.551 ns** |   **5.940 ns** |     **280.66 ns** |  **1.69x faster** |   **0.04x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 1024 | U2      |     275.60 ns |     3.922 ns |   3.477 ns |     276.33 ns |  1.71x faster |   0.02x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 1024 | U2      |     470.28 ns |     3.432 ns |   3.042 ns |     470.71 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 1024 | U2      |     336.72 ns |     5.299 ns |   4.957 ns |     335.78 ns |  1.86x faster |   0.02x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | U2      |     315.96 ns |     5.788 ns |   5.414 ns |     317.81 ns |  1.99x faster |   0.04x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | U2      |     627.54 ns |     8.085 ns |   7.167 ns |     627.52 ns |      baseline |         |      88 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| **EncodeTo**             | **.NET 6.0**           | **1024** | **U4**      |     **282.87 ns** |     **3.382 ns** |   **3.163 ns** |     **280.91 ns** |  **2.17x faster** |   **0.05x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET 8.0           | 1024 | U4      |     287.00 ns |     5.303 ns |   4.960 ns |     285.22 ns |  2.14x faster |   0.04x |      40 B |  1.00x more |
| EncodeTo             | .NET Framework 4.8 | 1024 | U4      |     613.63 ns |     9.246 ns |   8.648 ns |     613.69 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |              |            |               |               |         |           |             |
| DecodeFromFullBuffer | .NET 6.0           | 1024 | U4      |     342.51 ns |     5.331 ns |   4.986 ns |     342.74 ns |  2.16x faster |   0.03x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | U4      |     331.40 ns |     4.601 ns |   4.304 ns |     330.70 ns |  2.24x faster |   0.04x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | U4      |     740.48 ns |     7.128 ns |   5.952 ns |     741.66 ns |      baseline |         |      88 B |             |
