```

BenchmarkDotNet v0.13.11, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-ZWGTBM : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-ZQLZSO : .NET Framework 4.8.1 (4.8.9181.0), X64 RyuJIT VectorSize=256


```
| Method               | Runtime            | Size | Format  | Mean          | Error      | StdDev     | Ratio         | RatioSD | Allocated | Alloc Ratio |
|--------------------- |------------------- |----- |-------- |--------------:|-----------:|-----------:|--------------:|--------:|----------:|------------:|
| **EncodeTo**             | **.NET 8.0**           | **0**    | **List**    |      **15.25 ns** |   **0.097 ns** |   **0.086 ns** |  **4.92x faster** |   **0.05x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 0    | List    |      74.89 ns |   0.586 ns |   0.548 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 0    | List    |      25.07 ns |   0.199 ns |   0.186 ns |  5.64x faster |   0.06x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | List    |     141.47 ns |   0.904 ns |   0.846 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **0**    | **Binary**  |      **15.76 ns** |   **0.085 ns** |   **0.079 ns** |  **2.59x faster** |   **0.02x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 0    | Binary  |      40.80 ns |   0.189 ns |   0.168 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 0    | Binary  |      32.30 ns |   0.091 ns |   0.086 ns |  4.19x faster |   0.02x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | Binary  |     135.47 ns |   0.689 ns |   0.644 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **0**    | **Boolean** |      **15.79 ns** |   **0.032 ns** |   **0.030 ns** |  **2.49x faster** |   **0.01x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 0    | Boolean |      39.34 ns |   0.057 ns |   0.051 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 0    | Boolean |      33.25 ns |   0.380 ns |   0.356 ns |  4.08x faster |   0.04x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | Boolean |     135.69 ns |   1.320 ns |   1.170 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **0**    | **ASCII**   |      **15.53 ns** |   **0.091 ns** |   **0.081 ns** |  **2.70x faster** |   **0.02x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 0    | ASCII   |      41.88 ns |   0.232 ns |   0.217 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 0    | ASCII   |      31.77 ns |   0.192 ns |   0.180 ns |  4.29x faster |   0.04x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | ASCII   |     136.34 ns |   0.810 ns |   0.757 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **0**    | **JIS8**    |      **15.30 ns** |   **0.066 ns** |   **0.055 ns** |  **2.69x faster** |   **0.01x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 0    | JIS8    |      41.20 ns |   0.069 ns |   0.062 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 0    | JIS8    |      32.11 ns |   0.225 ns |   0.210 ns |  4.28x faster |   0.03x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | JIS8    |     137.32 ns |   0.815 ns |   0.763 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **0**    | **I8**      |      **15.86 ns** |   **0.193 ns** |   **0.171 ns** |  **2.52x faster** |   **0.04x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 0    | I8      |      39.89 ns |   0.233 ns |   0.218 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 0    | I8      |      31.14 ns |   0.264 ns |   0.247 ns |  4.37x faster |   0.05x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | I8      |     136.15 ns |   1.087 ns |   1.017 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **0**    | **I1**      |      **15.70 ns** |   **0.074 ns** |   **0.062 ns** |  **2.50x faster** |   **0.01x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 0    | I1      |      39.21 ns |   0.153 ns |   0.143 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 0    | I1      |      32.11 ns |   0.200 ns |   0.187 ns |  4.25x faster |   0.03x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | I1      |     136.44 ns |   0.389 ns |   0.345 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **0**    | **I2**      |      **15.69 ns** |   **0.092 ns** |   **0.077 ns** |  **2.55x faster** |   **0.02x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 0    | I2      |      40.07 ns |   0.111 ns |   0.104 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 0    | I2      |      32.21 ns |   0.240 ns |   0.224 ns |  4.31x faster |   0.04x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | I2      |     138.82 ns |   1.065 ns |   0.996 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **0**    | **I4**      |      **15.74 ns** |   **0.031 ns** |   **0.026 ns** |  **2.55x faster** |   **0.01x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 0    | I4      |      40.06 ns |   0.192 ns |   0.170 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 0    | I4      |      31.40 ns |   0.168 ns |   0.157 ns |  4.37x faster |   0.02x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | I4      |     137.33 ns |   0.213 ns |   0.189 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **0**    | **F8**      |      **15.62 ns** |   **0.075 ns** |   **0.067 ns** |  **2.53x faster** |   **0.01x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 0    | F8      |      39.44 ns |   0.145 ns |   0.128 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 0    | F8      |      32.78 ns |   0.296 ns |   0.276 ns |  4.12x faster |   0.04x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | F8      |     135.05 ns |   0.983 ns |   0.920 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **0**    | **F4**      |      **15.69 ns** |   **0.064 ns** |   **0.060 ns** |  **2.53x faster** |   **0.02x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 0    | F4      |      39.64 ns |   0.251 ns |   0.235 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 0    | F4      |      33.68 ns |   0.273 ns |   0.255 ns |  4.02x faster |   0.04x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | F4      |     135.36 ns |   0.902 ns |   0.800 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **0**    | **U8**      |      **15.66 ns** |   **0.121 ns** |   **0.113 ns** |  **2.56x faster** |   **0.02x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 0    | U8      |      40.08 ns |   0.103 ns |   0.096 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 0    | U8      |      32.80 ns |   0.250 ns |   0.233 ns |  4.19x faster |   0.03x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | U8      |     137.40 ns |   0.116 ns |   0.103 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **0**    | **U1**      |      **15.79 ns** |   **0.087 ns** |   **0.068 ns** |  **2.57x faster** |   **0.03x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 0    | U1      |      40.57 ns |   0.394 ns |   0.369 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 0    | U1      |      33.13 ns |   0.226 ns |   0.212 ns |  4.13x faster |   0.03x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | U1      |     136.72 ns |   0.593 ns |   0.555 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **0**    | **U2**      |      **15.68 ns** |   **0.076 ns** |   **0.064 ns** |  **2.55x faster** |   **0.01x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 0    | U2      |      39.97 ns |   0.126 ns |   0.118 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 0    | U2      |      33.11 ns |   0.423 ns |   0.396 ns |  4.15x faster |   0.05x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | U2      |     137.31 ns |   0.342 ns |   0.320 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **0**    | **U4**      |      **15.70 ns** |   **0.021 ns** |   **0.019 ns** |  **2.55x faster** |   **0.02x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 0    | U4      |      39.95 ns |   0.242 ns |   0.227 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 0    | U4      |      32.75 ns |   0.199 ns |   0.176 ns |  4.17x faster |   0.03x |         - |          NA |
| DecodeFromFullBuffer | .NET Framework 4.8 | 0    | U4      |     136.49 ns |   1.049 ns |   0.982 ns |      baseline |         |         - |          NA |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **1024** | **List**    |   **2,946.35 ns** |  **17.069 ns** |  **15.966 ns** | **14.78x faster** |   **0.13x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 1024 | List    |  43,558.14 ns | 353.493 ns | 330.658 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | List    |  23,265.98 ns |  96.176 ns |  89.964 ns |  4.70x faster |   0.04x |    8248 B |  1.00x less |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | List    | 109,355.30 ns | 804.459 ns | 752.491 ns |      baseline |         |    8273 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **1024** | **Binary**  |      **36.67 ns** |   **0.221 ns** |   **0.207 ns** |  **2.29x faster** |   **0.01x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 1024 | Binary  |      83.79 ns |   0.366 ns |   0.325 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | Binary  |      62.10 ns |   0.530 ns |   0.443 ns |  3.45x faster |   0.02x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | Binary  |     214.00 ns |   0.473 ns |   0.443 ns |      baseline |         |      88 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **1024** | **Boolean** |      **34.42 ns** |   **0.526 ns** |   **0.492 ns** |  **2.43x faster** |   **0.03x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 1024 | Boolean |      83.86 ns |   0.641 ns |   0.568 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | Boolean |      67.39 ns |   0.367 ns |   0.344 ns |  3.13x faster |   0.02x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | Boolean |     210.91 ns |   0.289 ns |   0.270 ns |      baseline |         |      88 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **1024** | **ASCII**   |      **45.66 ns** |   **0.064 ns** |   **0.054 ns** | **13.15x faster** |   **0.13x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 1024 | ASCII   |     599.82 ns |   5.922 ns |   5.540 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | ASCII   |     184.78 ns |   0.920 ns |   0.768 ns |  5.07x faster |   0.04x |    2256 B |  1.02x less |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | ASCII   |     935.89 ns |   7.331 ns |   6.858 ns |      baseline |         |    2295 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **1024** | **JIS8**    |      **64.90 ns** |   **0.308 ns** |   **0.411 ns** |  **7.06x faster** |   **0.34x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 1024 | JIS8    |     462.88 ns |   9.146 ns |  20.457 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | JIS8    |     178.51 ns |   1.613 ns |   1.509 ns |  4.08x faster |   0.04x |    2256 B |  1.02x less |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | JIS8    |     728.41 ns |   4.659 ns |   4.358 ns |      baseline |         |    2295 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **1024** | **I8**      |     **308.32 ns** |   **2.102 ns** |   **1.966 ns** |  **3.36x faster** |   **0.04x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 1024 | I8      |   1,036.13 ns |  11.486 ns |  10.744 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | I8      |     345.95 ns |   5.504 ns |   5.148 ns |  3.34x faster |   0.06x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | I8      |   1,157.08 ns |   4.598 ns |   4.076 ns |      baseline |         |      88 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **1024** | **I1**      |      **36.42 ns** |   **0.294 ns** |   **0.261 ns** |  **2.31x faster** |   **0.02x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 1024 | I1      |      84.05 ns |   0.125 ns |   0.111 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | I1      |      67.07 ns |   0.434 ns |   0.406 ns |  3.16x faster |   0.03x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | I1      |     211.70 ns |   1.206 ns |   1.128 ns |      baseline |         |      88 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **1024** | **I2**      |     **403.24 ns** |   **7.735 ns** |   **7.596 ns** |  **1.35x faster** |   **0.03x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 1024 | I2      |     546.57 ns |   1.959 ns |   1.833 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | I2      |     303.68 ns |   2.010 ns |   1.782 ns |  2.13x faster |   0.02x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | I2      |     645.92 ns |   3.829 ns |   3.394 ns |      baseline |         |      88 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **1024** | **I4**      |     **279.01 ns** |   **2.288 ns** |   **2.140 ns** |  **2.07x faster** |   **0.03x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 1024 | I4      |     576.12 ns |   4.953 ns |   4.633 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | I4      |     309.81 ns |   2.028 ns |   1.798 ns |  2.17x faster |   0.02x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | I4      |     671.33 ns |   4.302 ns |   4.024 ns |      baseline |         |      88 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **1024** | **F8**      |     **381.06 ns** |   **1.922 ns** |   **1.704 ns** |  **4.43x faster** |   **0.02x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 1024 | F8      |   1,687.19 ns |   3.009 ns |   2.667 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | F8      |     404.87 ns |   2.508 ns |   2.346 ns |  4.53x faster |   0.03x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | F8      |   1,832.38 ns |   7.158 ns |   6.696 ns |      baseline |         |      88 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **1024** | **F4**      |     **318.61 ns** |   **1.975 ns** |   **1.847 ns** |  **3.88x faster** |   **0.03x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 1024 | F4      |   1,236.91 ns |   5.209 ns |   4.872 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | F4      |     348.02 ns |   4.721 ns |   4.416 ns |  3.96x faster |   0.04x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | F4      |   1,382.63 ns |   2.334 ns |   1.822 ns |      baseline |         |      88 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **1024** | **U8**      |     **308.68 ns** |   **1.092 ns** |   **1.022 ns** |  **3.38x faster** |   **0.03x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 1024 | U8      |   1,044.24 ns |   9.296 ns |   8.696 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | U8      |     336.21 ns |   2.186 ns |   1.826 ns |  3.44x faster |   0.02x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | U8      |   1,157.75 ns |   5.144 ns |   4.812 ns |      baseline |         |      88 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **1024** | **U1**      |      **33.62 ns** |   **0.294 ns** |   **0.261 ns** |  **2.49x faster** |   **0.02x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 1024 | U1      |      83.82 ns |   0.418 ns |   0.391 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | U1      |      77.77 ns |   0.289 ns |   0.270 ns |  2.76x faster |   0.01x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | U1      |     214.61 ns |   0.843 ns |   0.789 ns |      baseline |         |      88 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **1024** | **U2**      |     **279.67 ns** |   **1.062 ns** |   **0.994 ns** |  **1.59x faster** |   **0.01x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 1024 | U2      |     443.93 ns |   1.700 ns |   1.590 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | U2      |     306.26 ns |   0.610 ns |   0.509 ns |  1.94x faster |   0.01x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | U2      |     593.23 ns |   4.072 ns |   3.610 ns |      baseline |         |      88 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| **EncodeTo**             | **.NET 8.0**           | **1024** | **U4**      |     **279.60 ns** |   **1.246 ns** |   **1.166 ns** |  **2.06x faster** |   **0.01x** |      **40 B** |  **1.00x more** |
| EncodeTo             | .NET Framework 4.8 | 1024 | U4      |     577.17 ns |   3.804 ns |   3.558 ns |      baseline |         |      40 B |             |
|                      |                    |      |         |               |            |            |               |         |           |             |
| DecodeFromFullBuffer | .NET 8.0           | 1024 | U4      |     311.23 ns |   3.029 ns |   2.685 ns |  2.18x faster |   0.03x |      88 B |  1.00x more |
| DecodeFromFullBuffer | .NET Framework 4.8 | 1024 | U4      |     677.77 ns |   3.519 ns |   3.292 ns |      baseline |         |      88 B |             |
