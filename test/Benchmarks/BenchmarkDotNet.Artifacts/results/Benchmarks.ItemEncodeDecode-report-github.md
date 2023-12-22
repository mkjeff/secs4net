```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2792/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-VAUECX : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-NTIFBX : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method               | Runtime  | Size | Format  | Mean         | Error      | StdDev     | Ratio        | RatioSD | Allocated | Alloc Ratio |
|--------------------- |--------- |----- |-------- |-------------:|-----------:|-----------:|-------------:|--------:|----------:|------------:|
| **EncodeTo**             | **.NET 6.0** | **0**    | **List**    |     **16.84 ns** |   **0.170 ns** |   **0.159 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | List    |     15.99 ns |   0.329 ns |   0.324 ns | 1.05x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | List    |     36.05 ns |   0.746 ns |   0.888 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | List    |     27.64 ns |   0.232 ns |   0.206 ns | 1.30x faster |   0.04x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **Binary**  |     **17.44 ns** |   **0.370 ns** |   **0.396 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | Binary  |     16.70 ns |   0.319 ns |   0.298 ns | 1.04x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | Binary  |     52.81 ns |   0.777 ns |   0.727 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | Binary  |     36.94 ns |   0.738 ns |   0.724 ns | 1.43x faster |   0.04x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **Boolean** |     **17.49 ns** |   **0.358 ns** |   **0.299 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | Boolean |     16.72 ns |   0.353 ns |   0.331 ns | 1.05x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | Boolean |     50.03 ns |   0.591 ns |   0.553 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | Boolean |     36.96 ns |   0.535 ns |   0.500 ns | 1.35x faster |   0.02x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **ASCII**   |     **17.54 ns** |   **0.282 ns** |   **0.250 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | ASCII   |     16.27 ns |   0.209 ns |   0.186 ns | 1.08x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | ASCII   |     53.15 ns |   1.059 ns |   1.133 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | ASCII   |     42.24 ns |   0.389 ns |   0.345 ns | 1.25x faster |   0.03x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **JIS8**    |     **17.63 ns** |   **0.311 ns** |   **0.291 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | JIS8    |     16.26 ns |   0.255 ns |   0.226 ns | 1.09x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | JIS8    |     56.10 ns |   0.988 ns |   0.925 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | JIS8    |     41.08 ns |   0.487 ns |   0.455 ns | 1.37x faster |   0.03x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **I8**      |     **17.49 ns** |   **0.349 ns** |   **0.343 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | I8      |     17.08 ns |   0.215 ns |   0.191 ns | 1.03x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | I8      |     50.03 ns |   0.394 ns |   0.349 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | I8      |     40.12 ns |   0.403 ns |   0.357 ns | 1.25x faster |   0.02x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **I1**      |     **17.30 ns** |   **0.261 ns** |   **0.244 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | I1      |     16.70 ns |   0.251 ns |   0.235 ns | 1.04x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | I1      |     47.61 ns |   0.525 ns |   0.466 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | I1      |     40.78 ns |   0.590 ns |   0.552 ns | 1.17x faster |   0.02x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **I2**      |     **17.29 ns** |   **0.310 ns** |   **0.290 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | I2      |     16.68 ns |   0.220 ns |   0.195 ns | 1.03x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | I2      |     53.30 ns |   0.716 ns |   0.669 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | I2      |     39.54 ns |   0.578 ns |   0.540 ns | 1.35x faster |   0.02x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **I4**      |     **17.32 ns** |   **0.264 ns** |   **0.247 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | I4      |     16.59 ns |   0.116 ns |   0.103 ns | 1.04x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | I4      |     56.56 ns |   0.599 ns |   0.560 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | I4      |     39.90 ns |   0.467 ns |   0.437 ns | 1.42x faster |   0.02x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **F8**      |     **17.48 ns** |   **0.356 ns** |   **0.381 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | F8      |     16.51 ns |   0.154 ns |   0.129 ns | 1.06x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | F8      |     50.60 ns |   0.689 ns |   0.645 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | F8      |     39.98 ns |   0.322 ns |   0.301 ns | 1.27x faster |   0.02x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **F4**      |     **17.22 ns** |   **0.166 ns** |   **0.139 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | F4      |     16.63 ns |   0.255 ns |   0.238 ns | 1.04x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | F4      |     51.83 ns |   0.515 ns |   0.481 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | F4      |     39.69 ns |   0.436 ns |   0.387 ns | 1.31x faster |   0.02x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **U8**      |     **18.23 ns** |   **0.315 ns** |   **0.294 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | U8      |     16.63 ns |   0.238 ns |   0.223 ns | 1.10x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | U8      |     54.57 ns |   0.697 ns |   0.652 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | U8      |     38.97 ns |   0.366 ns |   0.306 ns | 1.40x faster |   0.02x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **U1**      |     **17.20 ns** |   **0.205 ns** |   **0.171 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | U1      |     16.73 ns |   0.332 ns |   0.311 ns | 1.03x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | U1      |     49.84 ns |   0.890 ns |   0.833 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | U1      |     40.32 ns |   0.614 ns |   0.574 ns | 1.24x faster |   0.03x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **U2**      |     **17.30 ns** |   **0.195 ns** |   **0.183 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | U2      |     16.67 ns |   0.190 ns |   0.158 ns | 1.04x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | U2      |     56.81 ns |   0.321 ns |   0.268 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | U2      |     39.41 ns |   0.513 ns |   0.479 ns | 1.44x faster |   0.02x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **U4**      |     **17.24 ns** |   **0.159 ns** |   **0.141 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | U4      |     16.61 ns |   0.143 ns |   0.127 ns | 1.04x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | U4      |     48.29 ns |   0.766 ns |   0.716 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | U4      |     39.54 ns |   0.488 ns |   0.457 ns | 1.22x faster |   0.03x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **List**    |  **4,554.14 ns** |  **52.839 ns** |  **49.426 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | List    |  4,571.33 ns |  66.588 ns |  62.286 ns | 1.00x slower |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | List    | 28,519.87 ns | 237.440 ns | 222.101 ns |     baseline |         |    8248 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | List    | 26,142.99 ns | 172.642 ns | 153.042 ns | 1.09x faster |   0.01x |    8248 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **Binary**  |     **32.21 ns** |   **0.622 ns** |   **0.582 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | Binary  |     30.51 ns |   0.597 ns |   0.558 ns | 1.06x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | Binary  |     83.20 ns |   0.880 ns |   0.780 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | Binary  |     76.38 ns |   0.937 ns |   0.831 ns | 1.09x faster |   0.02x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **Boolean** |     **33.15 ns** |   **0.678 ns** |   **1.169 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | Boolean |     31.53 ns |   0.334 ns |   0.296 ns | 1.08x faster |   0.05x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | Boolean |     84.26 ns |   0.674 ns |   0.597 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | Boolean |     73.84 ns |   0.852 ns |   0.797 ns | 1.14x faster |   0.01x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **ASCII**   |     **54.79 ns** |   **0.363 ns** |   **0.322 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | ASCII   |     48.66 ns |   0.936 ns |   0.876 ns | 1.13x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | ASCII   |    220.96 ns |   3.530 ns |   3.302 ns |     baseline |         |    2256 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | ASCII   |    201.47 ns |   2.095 ns |   1.857 ns | 1.10x faster |   0.02x |    2256 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **JIS8**    |  **6,387.00 ns** |  **49.622 ns** |  **43.988 ns** |     **baseline** |        **** |     **488 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | JIS8    |  5,004.51 ns |  61.324 ns |  54.362 ns | 1.28x faster |   0.02x |     488 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | JIS8    |  4,121.36 ns |  43.837 ns |  36.606 ns |     baseline |         |    2704 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | JIS8    |  3,399.96 ns |  23.302 ns |  20.657 ns | 1.21x faster |   0.01x |    2720 B |  1.01x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **I8**      |    **314.87 ns** |   **3.471 ns** |   **3.246 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | I8      |    331.61 ns |   5.830 ns |   5.453 ns | 1.05x slower |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | I8      |    382.55 ns |   4.383 ns |   4.100 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | I8      |    359.05 ns |   4.055 ns |   3.595 ns | 1.06x faster |   0.02x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **I1**      |     **32.10 ns** |   **0.372 ns** |   **0.330 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | I1      |     30.44 ns |   0.237 ns |   0.210 ns | 1.05x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | I1      |     85.43 ns |   0.759 ns |   0.633 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | I1      |     76.91 ns |   0.998 ns |   0.885 ns | 1.11x faster |   0.02x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **I2**      |    **280.60 ns** |   **2.622 ns** |   **2.452 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | I2      |    296.32 ns |   2.627 ns |   2.329 ns | 1.06x slower |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | I2      |    337.63 ns |   1.996 ns |   1.667 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | I2      |    323.74 ns |   2.544 ns |   2.255 ns | 1.04x faster |   0.01x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **I4**      |    **281.23 ns** |   **3.531 ns** |   **3.303 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | I4      |    294.12 ns |   4.383 ns |   4.100 ns | 1.05x slower |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | I4      |    340.49 ns |   3.606 ns |   3.373 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | I4      |    326.88 ns |   4.374 ns |   4.092 ns | 1.04x faster |   0.02x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **F8**      |    **389.86 ns** |   **3.753 ns** |   **3.327 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | F8      |    397.83 ns |   4.582 ns |   4.286 ns | 1.02x slower |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | F8      |    452.72 ns |   4.025 ns |   3.765 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | F8      |    444.69 ns |   4.146 ns |   3.878 ns | 1.02x faster |   0.02x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **F4**      |    **333.94 ns** |   **2.736 ns** |   **2.285 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | F4      |    332.93 ns |   3.928 ns |   3.674 ns | 1.00x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | F4      |    395.00 ns |   4.827 ns |   4.516 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | F4      |    371.91 ns |   3.774 ns |   3.530 ns | 1.06x faster |   0.02x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **U8**      |    **313.64 ns** |   **3.568 ns** |   **3.163 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | U8      |    327.35 ns |   2.108 ns |   1.868 ns | 1.04x slower |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | U8      |    378.31 ns |   3.219 ns |   3.011 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | U8      |    361.16 ns |   4.676 ns |   4.373 ns | 1.05x faster |   0.01x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **U1**      |     **32.16 ns** |   **0.334 ns** |   **0.296 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | U1      |     30.55 ns |   0.212 ns |   0.177 ns | 1.05x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | U1      |     87.07 ns |   1.047 ns |   0.980 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | U1      |     75.13 ns |   0.516 ns |   0.458 ns | 1.16x faster |   0.02x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **U2**      |    **281.83 ns** |   **3.752 ns** |   **3.510 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | U2      |    295.32 ns |   1.899 ns |   1.483 ns | 1.05x slower |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | U2      |    389.16 ns |   4.746 ns |   4.440 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | U2      |    314.59 ns |   2.405 ns |   1.877 ns | 1.24x faster |   0.02x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **U4**      |    **278.60 ns** |   **3.505 ns** |   **3.107 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | U4      |    299.08 ns |   3.387 ns |   3.169 ns | 1.07x slower |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | U4      |    341.30 ns |   2.723 ns |   2.414 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | U4      |    335.37 ns |   5.128 ns |   4.797 ns | 1.02x faster |   0.02x |      88 B |  1.00x more |
