```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-LLCSMO : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-LSZLMA : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method               | Runtime  | Size | Format  | Mean         | Error      | StdDev     | Ratio        | RatioSD | Allocated | Alloc Ratio |
|--------------------- |--------- |----- |-------- |-------------:|-----------:|-----------:|-------------:|--------:|----------:|------------:|
| **EncodeTo**             | **.NET 6.0** | **0**    | **List**    |     **16.77 ns** |   **0.275 ns** |   **0.258 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | List    |     16.38 ns |   0.309 ns |   0.289 ns | 1.02x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | List    |     37.72 ns |   0.772 ns |   0.758 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | List    |     26.38 ns |   0.468 ns |   0.438 ns | 1.43x faster |   0.04x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **Binary**  |     **17.49 ns** |   **0.369 ns** |   **0.379 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | Binary  |     16.71 ns |   0.213 ns |   0.178 ns | 1.05x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | Binary  |     47.47 ns |   0.907 ns |   0.757 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | Binary  |     34.58 ns |   0.658 ns |   0.616 ns | 1.37x faster |   0.03x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **Boolean** |     **17.39 ns** |   **0.283 ns** |   **0.251 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | Boolean |     16.82 ns |   0.315 ns |   0.295 ns | 1.04x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | Boolean |     49.14 ns |   0.914 ns |   0.855 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | Boolean |     35.86 ns |   0.599 ns |   0.560 ns | 1.37x faster |   0.03x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **ASCII**   |     **17.60 ns** |   **0.237 ns** |   **0.210 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | ASCII   |     16.35 ns |   0.291 ns |   0.273 ns | 1.08x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | ASCII   |     50.41 ns |   0.783 ns |   0.732 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | ASCII   |     34.18 ns |   0.462 ns |   0.386 ns | 1.47x faster |   0.03x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **JIS8**    |     **17.75 ns** |   **0.357 ns** |   **0.366 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | JIS8    |     16.53 ns |   0.355 ns |   0.332 ns | 1.07x faster |   0.04x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | JIS8    |     49.93 ns |   0.639 ns |   0.566 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | JIS8    |     34.58 ns |   0.692 ns |   0.741 ns | 1.44x faster |   0.04x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **I8**      |     **17.49 ns** |   **0.171 ns** |   **0.143 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | I8      |     16.89 ns |   0.351 ns |   0.329 ns | 1.04x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | I8      |     49.03 ns |   0.995 ns |   1.064 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | I8      |     37.19 ns |   0.539 ns |   0.478 ns | 1.33x faster |   0.02x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **I1**      |     **17.44 ns** |   **0.283 ns** |   **0.264 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | I1      |     16.89 ns |   0.358 ns |   0.367 ns | 1.03x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | I1      |     49.43 ns |   0.846 ns |   0.791 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | I1      |     33.81 ns |   0.576 ns |   0.565 ns | 1.46x faster |   0.04x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **I2**      |     **17.44 ns** |   **0.343 ns** |   **0.304 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | I2      |     16.73 ns |   0.255 ns |   0.226 ns | 1.04x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | I2      |     49.54 ns |   0.736 ns |   0.688 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | I2      |     34.84 ns |   0.529 ns |   0.494 ns | 1.42x faster |   0.03x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **I4**      |     **17.47 ns** |   **0.254 ns** |   **0.225 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | I4      |     16.64 ns |   0.307 ns |   0.287 ns | 1.05x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | I4      |     48.29 ns |   0.809 ns |   0.757 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | I4      |     35.75 ns |   0.703 ns |   0.657 ns | 1.35x faster |   0.03x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **F8**      |     **17.51 ns** |   **0.361 ns** |   **0.386 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | F8      |     16.71 ns |   0.198 ns |   0.166 ns | 1.05x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | F8      |     51.62 ns |   0.615 ns |   0.576 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | F8      |     34.24 ns |   0.439 ns |   0.410 ns | 1.51x faster |   0.02x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **F4**      |     **17.35 ns** |   **0.238 ns** |   **0.211 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | F4      |     16.89 ns |   0.348 ns |   0.372 ns | 1.03x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | F4      |     52.30 ns |   0.728 ns |   0.681 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | F4      |     34.76 ns |   0.426 ns |   0.332 ns | 1.50x faster |   0.03x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **U8**      |     **17.40 ns** |   **0.341 ns** |   **0.303 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | U8      |     16.75 ns |   0.320 ns |   0.300 ns | 1.04x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | U8      |     51.36 ns |   0.929 ns |   0.869 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | U8      |     35.98 ns |   0.556 ns |   0.492 ns | 1.43x faster |   0.03x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **U1**      |     **17.54 ns** |   **0.275 ns** |   **0.243 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | U1      |     16.82 ns |   0.357 ns |   0.351 ns | 1.04x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | U1      |     50.16 ns |   1.024 ns |   0.958 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | U1      |     36.12 ns |   0.696 ns |   0.651 ns | 1.39x faster |   0.04x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **U2**      |     **17.47 ns** |   **0.309 ns** |   **0.289 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | U2      |     16.66 ns |   0.311 ns |   0.276 ns | 1.05x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | U2      |     52.18 ns |   0.791 ns |   0.740 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | U2      |     34.57 ns |   0.566 ns |   0.529 ns | 1.51x faster |   0.03x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **U4**      |     **17.39 ns** |   **0.264 ns** |   **0.234 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | U4      |     16.68 ns |   0.239 ns |   0.200 ns | 1.04x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | U4      |     49.80 ns |   0.416 ns |   0.389 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | U4      |     34.39 ns |   0.455 ns |   0.355 ns | 1.45x faster |   0.02x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **List**    |  **4,905.33 ns** |  **68.504 ns** |  **64.079 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | List    |  3,114.26 ns |  42.277 ns |  39.546 ns | 1.58x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | List    | 28,417.57 ns | 337.282 ns | 315.494 ns |     baseline |         |    8248 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | List    | 23,938.12 ns | 297.550 ns | 248.468 ns | 1.19x faster |   0.01x |    8248 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **Binary**  |     **32.14 ns** |   **0.534 ns** |   **0.474 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | Binary  |     30.77 ns |   0.631 ns |   0.619 ns | 1.05x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | Binary  |     81.84 ns |   1.579 ns |   1.622 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | Binary  |     68.09 ns |   1.308 ns |   1.343 ns | 1.20x faster |   0.03x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **Boolean** |     **31.88 ns** |   **0.642 ns** |   **0.834 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | Boolean |     31.29 ns |   0.615 ns |   0.604 ns | 1.02x faster |   0.05x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | Boolean |     78.75 ns |   1.543 ns |   1.651 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | Boolean |     66.20 ns |   1.303 ns |   1.448 ns | 1.19x faster |   0.04x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **ASCII**   |     **55.39 ns** |   **0.557 ns** |   **0.494 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | ASCII   |     48.79 ns |   0.882 ns |   0.825 ns | 1.14x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | ASCII   |    219.11 ns |   2.538 ns |   2.374 ns |     baseline |         |    2256 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | ASCII   |    197.72 ns |   2.345 ns |   2.079 ns | 1.11x faster |   0.02x |    2256 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **JIS8**    |  **6,396.98 ns** |  **72.253 ns** |  **64.051 ns** |     **baseline** |        **** |     **488 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | JIS8    |  5,025.75 ns |  62.058 ns |  55.013 ns | 1.27x faster |   0.02x |     488 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | JIS8    |  3,547.04 ns |  47.007 ns |  43.970 ns |     baseline |         |    2704 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | JIS8    |  3,355.02 ns |  42.926 ns |  38.053 ns | 1.06x faster |   0.02x |    2720 B |  1.01x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **I8**      |    **314.85 ns** |   **6.263 ns** |   **6.151 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | I8      |    331.70 ns |   5.138 ns |   4.806 ns | 1.05x slower |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | I8      |    369.01 ns |   4.494 ns |   3.984 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | I8      |    374.90 ns |   7.077 ns |   6.273 ns | 1.02x slower |   0.01x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **I1**      |     **32.33 ns** |   **0.626 ns** |   **0.721 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | I1      |     30.61 ns |   0.623 ns |   0.718 ns | 1.06x faster |   0.04x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | I1      |     83.86 ns |   1.398 ns |   1.308 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | I1      |     70.22 ns |   1.349 ns |   1.195 ns | 1.20x faster |   0.03x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **I2**      |    **284.00 ns** |   **3.859 ns** |   **3.609 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | I2      |    402.81 ns |   2.479 ns |   2.070 ns | 1.42x slower |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | I2      |    330.68 ns |   4.001 ns |   3.547 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | I2      |    317.98 ns |   5.890 ns |   5.510 ns | 1.04x faster |   0.02x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **I4**      |    **277.70 ns** |   **3.643 ns** |   **3.042 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | I4      |    294.26 ns |   4.617 ns |   4.092 ns | 1.06x slower |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | I4      |    335.73 ns |   6.708 ns |   6.588 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | I4      |    323.52 ns |   5.741 ns |   5.638 ns | 1.04x faster |   0.02x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **F8**      |    **386.35 ns** |   **5.085 ns** |   **4.757 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | F8      |    399.43 ns |   3.466 ns |   2.894 ns | 1.03x slower |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | F8      |    443.57 ns |   3.806 ns |   3.179 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | F8      |    427.17 ns |   5.593 ns |   4.958 ns | 1.04x faster |   0.01x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **F4**      |    **334.89 ns** |   **4.712 ns** |   **4.408 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | F4      |    334.43 ns |   4.804 ns |   4.494 ns | 1.00x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | F4      |    392.34 ns |   5.457 ns |   4.838 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | F4      |    362.48 ns |   5.154 ns |   4.569 ns | 1.08x faster |   0.02x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **U8**      |    **313.50 ns** |   **4.959 ns** |   **4.396 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | U8      |    329.08 ns |   4.563 ns |   4.045 ns | 1.05x slower |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | U8      |    369.20 ns |   4.598 ns |   3.839 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | U8      |    359.26 ns |   7.179 ns |   6.715 ns | 1.03x faster |   0.03x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **U1**      |     **32.25 ns** |   **0.653 ns** |   **0.642 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | U1      |     30.64 ns |   0.589 ns |   0.578 ns | 1.05x faster |   0.03x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | U1      |    104.49 ns |   1.341 ns |   1.255 ns |     baseline |         |    1088 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | U1      |     84.39 ns |   0.772 ns |   0.645 ns | 1.24x faster |   0.02x |    1088 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **U2**      |    **279.76 ns** |   **3.722 ns** |   **3.482 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | U2      |    293.43 ns |   5.072 ns |   4.745 ns | 1.05x slower |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | U2      |    330.99 ns |   1.384 ns |   1.227 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | U2      |    312.87 ns |   4.301 ns |   3.813 ns | 1.06x faster |   0.01x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **U4**      |    **282.37 ns** |   **4.767 ns** |   **4.459 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | U4      |    296.05 ns |   4.107 ns |   3.842 ns | 1.05x slower |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | U4      |    338.36 ns |   4.003 ns |   3.549 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | U4      |    325.19 ns |   1.833 ns |   1.531 ns | 1.04x faster |   0.01x |      88 B |  1.00x more |
