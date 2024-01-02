```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-ZQSTYQ : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-LEHGCO : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method               | Runtime  | Size | Format  | Mean         | Error      | StdDev     | Ratio        | RatioSD | Allocated | Alloc Ratio |
|--------------------- |--------- |----- |-------- |-------------:|-----------:|-----------:|-------------:|--------:|----------:|------------:|
| **EncodeTo**             | **.NET 6.0** | **0**    | **List**    |     **15.91 ns** |   **0.084 ns** |   **0.074 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | List    |     15.22 ns |   0.104 ns |   0.097 ns | 1.05x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | List    |     34.93 ns |   0.366 ns |   0.342 ns |     baseline |         |         - |          NA |
| DecodeFromFullBuffer | .NET 8.0 | 0    | List    |     24.03 ns |   0.174 ns |   0.163 ns | 1.45x faster |   0.02x |         - |          NA |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **Binary**  |     **16.45 ns** |   **0.224 ns** |   **0.198 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | Binary  |     15.73 ns |   0.071 ns |   0.066 ns | 1.05x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | Binary  |     43.66 ns |   0.161 ns |   0.150 ns |     baseline |         |      40 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 0    | Binary  |     33.65 ns |   0.159 ns |   0.148 ns | 1.30x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **Boolean** |     **16.47 ns** |   **0.047 ns** |   **0.041 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | Boolean |     15.78 ns |   0.037 ns |   0.035 ns | 1.04x faster |   0.00x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | Boolean |     46.12 ns |   0.070 ns |   0.062 ns |     baseline |         |      40 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 0    | Boolean |     33.16 ns |   0.077 ns |   0.068 ns | 1.39x faster |   0.00x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **ASCII**   |     **16.59 ns** |   **0.048 ns** |   **0.037 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | ASCII   |     15.61 ns |   0.332 ns |   0.294 ns | 1.06x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | ASCII   |     45.85 ns |   0.161 ns |   0.142 ns |     baseline |         |      32 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 0    | ASCII   |     35.36 ns |   0.109 ns |   0.102 ns | 1.30x faster |   0.01x |      32 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **JIS8**    |     **16.56 ns** |   **0.032 ns** |   **0.027 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | JIS8    |     15.36 ns |   0.067 ns |   0.062 ns | 1.08x faster |   0.00x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | JIS8    |     49.61 ns |   0.180 ns |   0.168 ns |     baseline |         |      32 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 0    | JIS8    |     34.24 ns |   0.426 ns |   0.399 ns | 1.45x faster |   0.02x |      32 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **I8**      |     **16.37 ns** |   **0.088 ns** |   **0.082 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | I8      |     15.74 ns |   0.093 ns |   0.087 ns | 1.04x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | I8      |     46.16 ns |   0.262 ns |   0.232 ns |     baseline |         |      40 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 0    | I8      |     34.12 ns |   0.269 ns |   0.251 ns | 1.35x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **I1**      |     **16.39 ns** |   **0.089 ns** |   **0.083 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | I1      |     15.71 ns |   0.100 ns |   0.089 ns | 1.04x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | I1      |     45.42 ns |   0.147 ns |   0.131 ns |     baseline |         |      40 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 0    | I1      |     32.55 ns |   0.216 ns |   0.202 ns | 1.39x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **I2**      |     **16.39 ns** |   **0.030 ns** |   **0.025 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | I2      |     15.76 ns |   0.035 ns |   0.033 ns | 1.04x faster |   0.00x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | I2      |     48.23 ns |   0.121 ns |   0.101 ns |     baseline |         |      40 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 0    | I2      |     34.74 ns |   0.065 ns |   0.061 ns | 1.39x faster |   0.00x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **I4**      |     **16.43 ns** |   **0.031 ns** |   **0.024 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | I4      |     15.73 ns |   0.094 ns |   0.088 ns | 1.05x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | I4      |     47.01 ns |   0.227 ns |   0.202 ns |     baseline |         |      40 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 0    | I4      |     33.75 ns |   0.378 ns |   0.354 ns | 1.39x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **F8**      |     **16.39 ns** |   **0.060 ns** |   **0.053 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | F8      |     16.02 ns |   0.090 ns |   0.079 ns | 1.02x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | F8      |     46.61 ns |   0.235 ns |   0.208 ns |     baseline |         |      40 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 0    | F8      |     33.09 ns |   0.097 ns |   0.086 ns | 1.41x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **F4**      |     **16.53 ns** |   **0.261 ns** |   **0.244 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | F4      |     15.85 ns |   0.065 ns |   0.054 ns | 1.04x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | F4      |     47.75 ns |   0.220 ns |   0.206 ns |     baseline |         |      40 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 0    | F4      |     34.30 ns |   0.293 ns |   0.274 ns | 1.39x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **U8**      |     **16.40 ns** |   **0.109 ns** |   **0.102 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | U8      |     15.73 ns |   0.080 ns |   0.075 ns | 1.04x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | U8      |     47.39 ns |   0.332 ns |   0.277 ns |     baseline |         |      40 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 0    | U8      |     33.77 ns |   0.124 ns |   0.116 ns | 1.40x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **U1**      |     **16.43 ns** |   **0.083 ns** |   **0.073 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | U1      |     15.74 ns |   0.027 ns |   0.025 ns | 1.04x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | U1      |     47.06 ns |   0.295 ns |   0.246 ns |     baseline |         |      40 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 0    | U1      |     33.75 ns |   0.145 ns |   0.136 ns | 1.39x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **U2**      |     **16.40 ns** |   **0.063 ns** |   **0.056 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | U2      |     15.79 ns |   0.063 ns |   0.056 ns | 1.04x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | U2      |     46.92 ns |   0.146 ns |   0.137 ns |     baseline |         |      40 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 0    | U2      |     35.08 ns |   0.248 ns |   0.194 ns | 1.34x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **0**    | **U4**      |     **16.39 ns** |   **0.077 ns** |   **0.068 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 0    | U4      |     15.71 ns |   0.084 ns |   0.070 ns | 1.04x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 0    | U4      |     46.90 ns |   0.191 ns |   0.179 ns |     baseline |         |      40 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 0    | U4      |     34.79 ns |   0.369 ns |   0.345 ns | 1.35x faster |   0.02x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **List**    |  **4,701.78 ns** |  **19.251 ns** |  **17.066 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | List    |  2,972.33 ns |  21.749 ns |  20.344 ns | 1.58x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | List    | 25,983.64 ns | 213.169 ns | 199.399 ns |     baseline |         |    8248 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | List    | 22,217.64 ns |  85.419 ns |  75.722 ns | 1.17x faster |   0.01x |    8248 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **Binary**  |     **30.17 ns** |   **0.138 ns** |   **0.129 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | Binary  |     29.68 ns |   0.143 ns |   0.111 ns | 1.02x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | Binary  |     78.02 ns |   0.533 ns |   0.416 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | Binary  |     63.45 ns |   0.106 ns |   0.099 ns | 1.23x faster |   0.01x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **Boolean** |     **30.73 ns** |   **0.023 ns** |   **0.018 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | Boolean |     29.45 ns |   0.033 ns |   0.031 ns | 1.04x faster |   0.00x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | Boolean |     79.69 ns |   0.492 ns |   0.460 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | Boolean |     66.41 ns |   0.205 ns |   0.160 ns | 1.20x faster |   0.01x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **ASCII**   |     **56.58 ns** |   **0.265 ns** |   **0.235 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | ASCII   |     45.33 ns |   0.055 ns |   0.049 ns | 1.25x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | ASCII   |    208.66 ns |   0.595 ns |   0.557 ns |     baseline |         |    2256 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | ASCII   |    182.33 ns |   0.683 ns |   0.570 ns | 1.14x faster |   0.00x |    2256 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **JIS8**    |  **6,160.21 ns** |  **30.715 ns** |  **27.228 ns** |     **baseline** |        **** |     **488 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | JIS8    |  4,850.19 ns |  34.772 ns |  32.525 ns | 1.27x faster |   0.01x |     488 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | JIS8    |  3,344.98 ns |  20.038 ns |  18.744 ns |     baseline |         |    2704 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | JIS8    |  3,170.70 ns |  17.796 ns |  13.894 ns | 1.05x faster |   0.01x |    2720 B |  1.01x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **I8**      |    **299.33 ns** |   **1.501 ns** |   **1.253 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | I8      |    314.96 ns |   1.653 ns |   1.465 ns | 1.05x slower |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | I8      |    351.44 ns |   2.292 ns |   2.144 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | I8      |    335.79 ns |   2.277 ns |   2.130 ns | 1.05x faster |   0.01x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **I1**      |     **30.91 ns** |   **0.250 ns** |   **0.333 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | I1      |     29.59 ns |   0.147 ns |   0.138 ns | 1.05x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | I1      |     77.39 ns |   0.305 ns |   0.270 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | I1      |     63.25 ns |   0.127 ns |   0.113 ns | 1.22x faster |   0.01x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **I2**      |    **269.29 ns** |   **0.366 ns** |   **0.342 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | I2      |    285.53 ns |   0.470 ns |   0.416 ns | 1.06x slower |   0.00x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | I2      |    314.29 ns |   1.416 ns |   1.182 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | I2      |    302.35 ns |   0.432 ns |   0.383 ns | 1.04x faster |   0.00x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **I4**      |    **267.73 ns** |   **0.511 ns** |   **0.399 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | I4      |    277.51 ns |   1.800 ns |   1.683 ns | 1.04x slower |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | I4      |    318.58 ns |   0.372 ns |   0.348 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | I4      |    304.54 ns |   1.485 ns |   1.389 ns | 1.05x faster |   0.00x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **F8**      |    **368.23 ns** |   **1.535 ns** |   **1.436 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | F8      |    379.01 ns |   3.511 ns |   3.113 ns | 1.03x slower |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | F8      |    427.49 ns |   0.834 ns |   0.739 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | F8      |    405.93 ns |   1.267 ns |   1.123 ns | 1.05x faster |   0.00x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **F4**      |    **319.36 ns** |   **2.029 ns** |   **1.898 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | F4      |    314.65 ns |   1.831 ns |   1.623 ns | 1.02x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | F4      |    365.30 ns |   2.807 ns |   2.625 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | F4      |    343.42 ns |   2.103 ns |   1.756 ns | 1.06x faster |   0.01x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **U8**      |    **298.55 ns** |   **1.101 ns** |   **0.920 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | U8      |    313.03 ns |   0.391 ns |   0.327 ns | 1.05x slower |   0.00x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | U8      |    348.32 ns |   1.508 ns |   1.337 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | U8      |    336.49 ns |   1.384 ns |   1.227 ns | 1.04x faster |   0.01x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **U1**      |     **30.55 ns** |   **0.129 ns** |   **0.115 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | U1      |     28.97 ns |   0.212 ns |   0.199 ns | 1.05x faster |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | U1      |     77.68 ns |   0.294 ns |   0.275 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | U1      |     61.82 ns |   0.255 ns |   0.226 ns | 1.26x faster |   0.00x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **U2**      |    **268.65 ns** |   **1.689 ns** |   **1.580 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | U2      |    285.31 ns |   2.091 ns |   1.853 ns | 1.06x slower |   0.01x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | U2      |    314.24 ns |   0.618 ns |   0.483 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | U2      |    303.49 ns |   2.141 ns |   1.898 ns | 1.04x faster |   0.01x |      88 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| **EncodeTo**             | **.NET 6.0** | **1024** | **U4**      |    **268.31 ns** |   **0.231 ns** |   **0.205 ns** |     **baseline** |        **** |      **40 B** |            **** |
| EncodeTo             | .NET 8.0 | 1024 | U4      |    278.46 ns |   1.329 ns |   1.178 ns | 1.04x slower |   0.00x |      40 B |  1.00x more |
|                      |          |      |         |              |            |            |              |         |           |             |
| DecodeFromFullBuffer | .NET 6.0 | 1024 | U4      |    318.21 ns |   1.732 ns |   1.620 ns |     baseline |         |      88 B |             |
| DecodeFromFullBuffer | .NET 8.0 | 1024 | U4      |    306.87 ns |   1.488 ns |   1.319 ns | 1.04x faster |   0.00x |      88 B |  1.00x more |
