```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-OOEVDF : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-AKZPHK : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method               | Runtime  | Size | Format  | Mean         | Error      | StdDev     | Median       | Ratio        | RatioSD | Allocated | Alloc Ratio |
|--------------------- |--------- |----- |-------- |-------------:|-----------:|-----------:|-------------:|-------------:|--------:|----------:|------------:|
| **EncodeTo**             | **.NET 6.0** | **0**    | **List**    |     **17.13 ns** |   **0.356 ns** |   **0.381 ns** |     **17.13 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | List    |     16.29 ns |   0.334 ns |   0.312 ns |     16.29 ns | 1.06x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | List    |     36.64 ns |   0.511 ns |   0.478 ns |     36.72 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | List    |     25.91 ns |   0.274 ns |   0.243 ns |     25.88 ns | 1.42x faster |   0.02x |         - |          NA |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **Binary**  |     **17.57 ns** |   **0.373 ns** |   **0.349 ns** |     **17.52 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | Binary  |     16.88 ns |   0.181 ns |   0.161 ns |     16.87 ns | 1.04x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | Binary  |     48.90 ns |   0.881 ns |   0.824 ns |     49.02 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | Binary  |     32.68 ns |   0.474 ns |   0.421 ns |     32.76 ns | 1.50x faster |   0.03x |         - |          NA |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **Boolean** |     **17.60 ns** |   **0.366 ns** |   **0.359 ns** |     **17.54 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | Boolean |     16.78 ns |   0.310 ns |   0.275 ns |     16.83 ns | 1.05x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | Boolean |     47.05 ns |   0.674 ns |   0.630 ns |     46.96 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | Boolean |     32.34 ns |   0.585 ns |   0.574 ns |     32.41 ns | 1.46x faster |   0.03x |         - |          NA |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **ASCII**   |     **17.75 ns** |   **0.360 ns** |   **0.337 ns** |     **17.72 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | ASCII   |     16.32 ns |   0.319 ns |   0.299 ns |     16.32 ns | 1.09x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | ASCII   |     49.62 ns |   0.796 ns |   0.745 ns |     49.92 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | ASCII   |     32.60 ns |   0.586 ns |   0.548 ns |     32.51 ns | 1.52x faster |   0.03x |         - |          NA |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **JIS8**    |     **17.68 ns** |   **0.283 ns** |   **0.237 ns** |     **17.69 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | JIS8    |     16.54 ns |   0.305 ns |   0.271 ns |     16.53 ns | 1.07x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | JIS8    |     48.46 ns |   0.869 ns |   0.813 ns |     48.72 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | JIS8    |     32.32 ns |   0.653 ns |   0.641 ns |     32.29 ns | 1.50x faster |   0.04x |         - |          NA |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **I8**      |     **17.32 ns** |   **0.308 ns** |   **0.273 ns** |     **17.34 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | I8      |     16.96 ns |   0.334 ns |   0.313 ns |     16.91 ns | 1.02x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | I8      |     47.40 ns |   0.641 ns |   0.599 ns |     47.52 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | I8      |     33.83 ns |   0.543 ns |   0.508 ns |     33.74 ns | 1.40x faster |   0.03x |         - |          NA |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **I1**      |     **17.55 ns** |   **0.261 ns** |   **0.244 ns** |     **17.55 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | I1      |     16.73 ns |   0.293 ns |   0.244 ns |     16.72 ns | 1.05x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | I1      |     48.30 ns |   0.646 ns |   0.605 ns |     48.20 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | I1      |     33.26 ns |   0.504 ns |   0.446 ns |     33.27 ns | 1.45x faster |   0.03x |         - |          NA |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **I2**      |     **17.51 ns** |   **0.173 ns** |   **0.145 ns** |     **17.54 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | I2      |     16.94 ns |   0.352 ns |   0.376 ns |     16.91 ns | 1.03x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | I2      |     47.58 ns |   0.480 ns |   0.449 ns |     47.51 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | I2      |     33.91 ns |   0.607 ns |   0.538 ns |     34.00 ns | 1.40x faster |   0.03x |         - |          NA |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **I4**      |     **17.67 ns** |   **0.373 ns** |   **0.383 ns** |     **17.53 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | I4      |     16.74 ns |   0.166 ns |   0.139 ns |     16.73 ns | 1.06x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | I4      |     46.98 ns |   0.937 ns |   1.003 ns |     46.90 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | I4      |     33.20 ns |   0.509 ns |   0.451 ns |     33.14 ns | 1.41x faster |   0.03x |         - |          NA |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **F8**      |     **17.36 ns** |   **0.227 ns** |   **0.177 ns** |     **17.44 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | F8      |     16.91 ns |   0.360 ns |   0.370 ns |     16.89 ns | 1.04x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | F8      |     47.41 ns |   0.932 ns |   0.872 ns |     47.45 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | F8      |     33.09 ns |   0.567 ns |   0.530 ns |     33.15 ns | 1.43x faster |   0.03x |         - |          NA |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **F4**      |     **17.57 ns** |   **0.280 ns** |   **0.234 ns** |     **17.61 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | F4      |     16.83 ns |   0.297 ns |   0.277 ns |     16.84 ns | 1.04x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | F4      |     47.25 ns |   0.477 ns |   0.446 ns |     47.19 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | F4      |     33.38 ns |   0.497 ns |   0.440 ns |     33.44 ns | 1.42x faster |   0.02x |         - |          NA |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **U8**      |     **17.39 ns** |   **0.268 ns** |   **0.238 ns** |     **17.46 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | U8      |     16.79 ns |   0.302 ns |   0.282 ns |     16.84 ns | 1.04x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | U8      |     49.21 ns |   0.944 ns |   0.883 ns |     49.10 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | U8      |     33.61 ns |   0.691 ns |   0.646 ns |     33.50 ns | 1.47x faster |   0.05x |         - |          NA |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **U1**      |     **17.55 ns** |   **0.305 ns** |   **0.285 ns** |     **17.57 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | U1      |     16.89 ns |   0.356 ns |   0.365 ns |     16.82 ns | 1.04x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | U1      |     51.79 ns |   0.456 ns |   0.381 ns |     51.83 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | U1      |     33.47 ns |   0.614 ns |   0.575 ns |     33.56 ns | 1.55x faster |   0.03x |         - |          NA |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **U2**      |     **17.60 ns** |   **0.373 ns** |   **0.367 ns** |     **17.67 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | U2      |     16.73 ns |   0.297 ns |   0.263 ns |     16.72 ns | 1.05x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | U2      |     46.25 ns |   0.930 ns |   1.034 ns |     46.37 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | U2      |     33.97 ns |   0.374 ns |   0.331 ns |     33.97 ns | 1.36x faster |   0.03x |         - |          NA |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **U4**      |     **17.50 ns** |   **0.372 ns** |   **0.365 ns** |     **17.53 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | U4      |     17.20 ns |   0.343 ns |   0.321 ns |     17.19 ns | 1.02x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | U4      |     50.00 ns |   0.631 ns |   0.590 ns |     49.90 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | U4      |     32.69 ns |   0.663 ns |   0.620 ns |     32.80 ns | 1.53x faster |   0.04x |         - |          NA |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **List**    |  **4,819.79 ns** |  **77.213 ns** |  **75.834 ns** |  **4,825.99 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | List    |  3,225.07 ns |  41.425 ns |  38.749 ns |  3,223.14 ns | 1.49x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | List    | 27,640.27 ns | 372.723 ns | 348.645 ns | 27,689.83 ns |     baseline |         |    8248 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | List    | 23,571.28 ns | 407.274 ns | 380.965 ns | 23,474.78 ns | 1.17x faster |   0.03x |    8248 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **Binary**  |     **35.44 ns** |   **0.597 ns** |   **0.586 ns** |     **35.38 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | Binary  |     35.10 ns |   0.619 ns |   0.579 ns |     35.17 ns | 1.01x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | Binary  |     80.60 ns |   1.547 ns |   1.519 ns |     80.71 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | Binary  |     66.65 ns |   0.968 ns |   0.858 ns |     66.74 ns | 1.21x faster |   0.03x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **Boolean** |     **33.16 ns** |   **0.484 ns** |   **0.453 ns** |     **33.15 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | Boolean |     37.01 ns |   0.390 ns |   0.346 ns |     37.09 ns | 1.12x slower |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | Boolean |     80.73 ns |   1.454 ns |   1.428 ns |     81.03 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | Boolean |     67.60 ns |   0.917 ns |   0.858 ns |     67.76 ns | 1.19x faster |   0.03x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **ASCII**   |     **60.31 ns** |   **1.210 ns** |   **1.440 ns** |     **60.25 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | ASCII   |     49.08 ns |   0.981 ns |   1.007 ns |     48.66 ns | 1.23x faster |   0.04x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | ASCII   |    219.34 ns |   3.364 ns |   3.147 ns |    218.35 ns |     baseline |         |    2256 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | ASCII   |    197.78 ns |   2.860 ns |   2.535 ns |    197.42 ns | 1.11x faster |   0.02x |    2256 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **JIS8**    |     **82.95 ns** |   **0.716 ns** |   **0.634 ns** |     **82.81 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | JIS8    |     68.59 ns |   1.399 ns |   1.308 ns |     68.42 ns | 1.21x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | JIS8    |    238.91 ns |   3.515 ns |   3.116 ns |    239.08 ns |     baseline |         |    2256 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | JIS8    |    189.55 ns |   3.555 ns |   3.326 ns |    189.95 ns | 1.26x faster |   0.02x |    2256 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **I8**      |    **319.29 ns** |   **5.432 ns** |   **5.335 ns** |    **317.97 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | I8      |    333.55 ns |   5.196 ns |   4.860 ns |    332.41 ns | 1.05x slower |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | I8      |    373.32 ns |   4.403 ns |   3.677 ns |    371.67 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | I8      |    354.88 ns |   4.374 ns |   3.878 ns |    354.64 ns | 1.05x faster |   0.01x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **I1**      |     **32.78 ns** |   **0.294 ns** |   **0.261 ns** |     **32.87 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | I1      |     79.02 ns |   2.711 ns |   7.993 ns |     81.38 ns | 2.20x slower |   0.52x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | I1      |    145.50 ns |   3.180 ns |   9.376 ns |    147.74 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | I1      |    127.77 ns |   5.133 ns |  15.136 ns |    132.88 ns | 1.16x faster |   0.20x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **I2**      |  **1,438.73 ns** | **217.020 ns** | **639.888 ns** |  **1,729.10 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | I2      |    292.69 ns |   3.310 ns |   2.934 ns |    292.58 ns | 4.48x faster |   2.37x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | I2      |    336.84 ns |   6.089 ns |   5.695 ns |    336.21 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | I2      |    316.50 ns |   5.499 ns |   5.144 ns |    316.89 ns | 1.06x faster |   0.02x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **I4**      |    **281.11 ns** |   **3.976 ns** |   **3.719 ns** |    **281.62 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | I4      |    293.33 ns |   4.995 ns |   4.672 ns |    294.17 ns | 1.04x slower |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | I4      |    335.52 ns |   4.029 ns |   3.572 ns |    335.33 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | I4      |    323.44 ns |   4.347 ns |   4.067 ns |    322.76 ns | 1.04x faster |   0.02x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **F8**      |    **387.80 ns** |   **4.693 ns** |   **4.160 ns** |    **387.48 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | F8      |    398.72 ns |   4.194 ns |   3.717 ns |    399.17 ns | 1.03x slower |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | F8      |    445.99 ns |   5.848 ns |   5.470 ns |    445.65 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | F8      |    426.55 ns |   4.814 ns |   4.020 ns |    425.89 ns | 1.05x faster |   0.01x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **F4**      |    **332.94 ns** |   **1.790 ns** |   **1.495 ns** |    **333.29 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | F4      |    335.88 ns |   6.506 ns |   6.681 ns |    335.95 ns | 1.01x slower |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | F4      |    386.60 ns |   4.792 ns |   4.248 ns |    387.01 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | F4      |    363.95 ns |   6.568 ns |   6.144 ns |    362.72 ns | 1.06x faster |   0.02x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **U8**      |    **315.68 ns** |   **4.444 ns** |   **4.157 ns** |    **315.27 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | U8      |    327.29 ns |   5.463 ns |   5.110 ns |    326.28 ns | 1.04x slower |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | U8      |    371.12 ns |   6.327 ns |   5.918 ns |    368.96 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | U8      |    358.64 ns |   5.935 ns |   5.552 ns |    358.72 ns | 1.04x faster |   0.03x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **U1**      |     **35.34 ns** |   **0.723 ns** |   **1.340 ns** |     **35.63 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | U1      |     35.14 ns |   0.694 ns |   0.649 ns |     35.13 ns | 1.02x slower |   0.06x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | U1      |    102.76 ns |   2.071 ns |   2.216 ns |    102.16 ns |     baseline |         |    1088 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | U1      |     86.65 ns |   1.652 ns |   1.546 ns |     86.81 ns | 1.19x faster |   0.04x |    1088 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **U2**      |    **279.70 ns** |   **5.609 ns** |   **5.508 ns** |    **280.09 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | U2      |    293.64 ns |   4.534 ns |   4.241 ns |    293.47 ns | 1.05x slower |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | U2      |    339.71 ns |   6.037 ns |   5.352 ns |    339.98 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | U2      |    316.91 ns |   3.333 ns |   2.954 ns |    316.84 ns | 1.07x faster |   0.02x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **U4**      |    **281.49 ns** |   **4.783 ns** |   **4.697 ns** |    **281.39 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | U4      |    294.76 ns |   4.940 ns |   4.852 ns |    293.54 ns | 1.05x slower |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | U4      |    333.98 ns |   5.208 ns |   4.871 ns |    333.79 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | U4      |    320.83 ns |   5.437 ns |   4.819 ns |    320.61 ns | 1.04x faster |   0.01x |      88 B |  1.00x more |
