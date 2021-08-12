``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1165 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.7.21379.14
  [Host]     : .NET 6.0.0 (6.0.21.37719), X64 RyuJIT
  Job-GVDIFM : .NET 6.0.0 (6.0.21.37719), X64 RyuJIT
  Job-WCNVCL : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|               Method |              Runtime | Size |  Format |         Mean |      Error |     StdDev |        Ratio | RatioSD |  Gen 0 |  Gen 1 | Allocated |
|--------------------- |--------------------- |----- |-------- |-------------:|-----------:|-----------:|-------------:|--------:|-------:|-------:|----------:|
|             **EncodeTo** |             **.NET 6.0** |    **0** |    **List** |     **76.57 ns** |   **0.602 ns** |   **0.563 ns** | **1.59x faster** |   **0.02x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |    List |    121.63 ns |   1.443 ns |   1.279 ns |     baseline |         | 0.0062 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |    List |     55.81 ns |   0.772 ns |   0.722 ns | 2.75x faster |   0.03x |      - |      - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |    List |    153.71 ns |   0.372 ns |   0.348 ns |     baseline |         |      - |      - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |  **Binary** |     **80.60 ns** |   **0.242 ns** |   **0.202 ns** | **1.58x faster** |   **0.01x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |  Binary |    127.68 ns |   0.430 ns |   0.381 ns |     baseline |         | 0.0062 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |  Binary |     78.19 ns |   0.669 ns |   0.626 ns | 2.78x faster |   0.02x |      - |      - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |  Binary |    217.06 ns |   0.665 ns |   0.622 ns |     baseline |         |      - |      - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** | **Boolean** |     **79.58 ns** |   **0.269 ns** |   **0.252 ns** | **1.60x faster** |   **0.02x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 | Boolean |    127.00 ns |   1.555 ns |   1.455 ns |     baseline |         | 0.0062 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 | Boolean |     76.71 ns |   0.253 ns |   0.237 ns | 2.86x faster |   0.01x |      - |      - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 | Boolean |    219.33 ns |   0.397 ns |   0.371 ns |     baseline |         |      - |      - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |   **ASCII** |     **76.65 ns** |   **0.398 ns** |   **0.372 ns** | **1.66x faster** |   **0.01x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |   ASCII |    127.08 ns |   0.348 ns |   0.326 ns |     baseline |         | 0.0062 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |   ASCII |     73.98 ns |   0.135 ns |   0.120 ns | 3.24x faster |   0.01x |      - |      - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |   ASCII |    239.78 ns |   0.567 ns |   0.503 ns |     baseline |         |      - |      - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |    **JIS8** |     **78.09 ns** |   **0.275 ns** |   **0.257 ns** | **1.63x faster** |   **0.01x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |    JIS8 |    127.13 ns |   0.278 ns |   0.260 ns |     baseline |         | 0.0062 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |    JIS8 |     80.40 ns |   0.487 ns |   0.455 ns | 2.98x faster |   0.02x |      - |      - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |    JIS8 |    239.84 ns |   0.596 ns |   0.557 ns |     baseline |         |      - |      - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I8** |     **80.85 ns** |   **0.363 ns** |   **0.339 ns** | **1.51x faster** |   **0.01x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I8 |    122.32 ns |   0.421 ns |   0.373 ns |     baseline |         | 0.0062 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I8 |     82.19 ns |   0.303 ns |   0.283 ns | 3.01x faster |   0.01x |      - |      - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I8 |    247.65 ns |   0.350 ns |   0.328 ns |     baseline |         |      - |      - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I1** |     **80.84 ns** |   **0.274 ns** |   **0.243 ns** | **1.58x faster** |   **0.01x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I1 |    127.86 ns |   0.872 ns |   0.815 ns |     baseline |         | 0.0062 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I1 |     73.23 ns |   0.479 ns |   0.448 ns | 3.43x faster |   0.03x |      - |      - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I1 |    251.29 ns |   0.928 ns |   0.823 ns |     baseline |         |      - |      - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I2** |     **78.93 ns** |   **0.353 ns** |   **0.330 ns** | **1.62x faster** |   **0.01x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I2 |    127.84 ns |   1.046 ns |   0.978 ns |     baseline |         | 0.0062 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I2 |     75.30 ns |   0.510 ns |   0.452 ns | 3.07x faster |   0.02x |      - |      - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I2 |    231.09 ns |   0.527 ns |   0.493 ns |     baseline |         |      - |      - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **I4** |     **78.38 ns** |   **0.379 ns** |   **0.354 ns** | **1.56x faster** |   **0.01x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      I4 |    122.57 ns |   0.602 ns |   0.563 ns |     baseline |         | 0.0062 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      I4 |     81.20 ns |   0.253 ns |   0.237 ns | 3.03x faster |   0.01x |      - |      - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      I4 |    245.91 ns |   0.535 ns |   0.475 ns |     baseline |         |      - |      - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **F8** |     **78.30 ns** |   **0.249 ns** |   **0.221 ns** | **1.57x faster** |   **0.01x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      F8 |    122.63 ns |   0.663 ns |   0.621 ns |     baseline |         | 0.0062 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      F8 |     81.80 ns |   0.219 ns |   0.205 ns | 3.15x faster |   0.01x |      - |      - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      F8 |    257.84 ns |   0.458 ns |   0.406 ns |     baseline |         |      - |      - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **F4** |     **78.53 ns** |   **0.288 ns** |   **0.270 ns** | **1.55x faster** |   **0.01x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      F4 |    122.11 ns |   0.419 ns |   0.372 ns |     baseline |         | 0.0062 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      F4 |     76.69 ns |   0.372 ns |   0.348 ns | 3.26x faster |   0.01x |      - |      - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      F4 |    249.85 ns |   0.300 ns |   0.251 ns |     baseline |         |      - |      - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U8** |     **80.94 ns** |   **0.262 ns** |   **0.245 ns** | **1.51x faster** |   **0.01x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U8 |    122.55 ns |   0.541 ns |   0.506 ns |     baseline |         | 0.0062 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U8 |     74.42 ns |   0.395 ns |   0.370 ns | 3.26x faster |   0.02x |      - |      - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U8 |    242.62 ns |   0.581 ns |   0.543 ns |     baseline |         |      - |      - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U1** |     **80.95 ns** |   **0.318 ns** |   **0.297 ns** | **1.59x faster** |   **0.01x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U1 |    128.48 ns |   0.644 ns |   0.603 ns |     baseline |         | 0.0062 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U1 |     83.04 ns |   0.392 ns |   0.348 ns | 2.99x faster |   0.01x |      - |      - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U1 |    247.88 ns |   0.415 ns |   0.368 ns |     baseline |         |      - |      - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U2** |     **76.86 ns** |   **0.249 ns** |   **0.233 ns** | **1.64x faster** |   **0.01x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U2 |    126.28 ns |   0.898 ns |   0.840 ns |     baseline |         | 0.0062 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U2 |     78.31 ns |   0.310 ns |   0.290 ns | 2.96x faster |   0.01x |      - |      - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U2 |    232.17 ns |   0.361 ns |   0.338 ns |     baseline |         |      - |      - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** |    **0** |      **U4** |     **81.18 ns** |   **0.393 ns** |   **0.368 ns** | **1.51x faster** |   **0.01x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 |    0 |      U4 |    122.44 ns |   0.667 ns |   0.624 ns |     baseline |         | 0.0062 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 |    0 |      U4 |     78.59 ns |   0.480 ns |   0.449 ns | 3.14x faster |   0.02x |      - |      - |         - |
| DecodeFromFullBuffer | .NET Framework 4.7.2 |    0 |      U4 |    246.67 ns |   0.586 ns |   0.548 ns |     baseline |         |      - |      - |         - |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |    **List** |  **8,099.92 ns** |  **39.952 ns** |  **37.371 ns** | **3.03x faster** |   **0.01x** |      **-** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |    List | 24,518.17 ns |  28.479 ns |  25.246 ns |     baseline |         |      - |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |    List | 43,745.32 ns | 206.943 ns | 183.449 ns | 2.22x faster |   0.01x | 0.9766 |      - |   8,248 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |    List | 97,122.94 ns | 150.194 ns | 140.491 ns |     baseline |         | 1.2207 |      - |   8,273 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |  **Binary** |    **103.88 ns** |   **0.239 ns** |   **0.223 ns** | **1.86x faster** |   **0.02x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |  Binary |    193.48 ns |   1.904 ns |   1.781 ns |     baseline |         | 0.0062 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |  Binary |    204.37 ns |   0.561 ns |   0.525 ns | 1.98x faster |   0.01x | 0.0105 |      - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |  Binary |    404.76 ns |   0.976 ns |   0.865 ns |     baseline |         | 0.0138 |      - |      88 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** | **Boolean** |    **100.79 ns** |   **0.221 ns** |   **0.206 ns** | **2.01x faster** |   **0.01x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 | Boolean |    202.87 ns |   0.916 ns |   0.857 ns |     baseline |         | 0.0062 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 | Boolean |    201.38 ns |   1.116 ns |   1.044 ns | 2.10x faster |   0.01x | 0.0105 |      - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 | Boolean |    423.15 ns |   1.268 ns |   1.186 ns |     baseline |         | 0.0138 |      - |      88 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |   **ASCII** |    **123.52 ns** |   **0.319 ns** |   **0.267 ns** | **9.68x faster** |   **0.03x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |   ASCII |  1,195.61 ns |   3.225 ns |   2.859 ns |     baseline |         | 0.0057 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |   ASCII |    408.89 ns |   3.226 ns |   3.017 ns | 4.29x faster |   0.04x | 0.2694 | 0.0019 |   2,256 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |   ASCII |  1,753.13 ns |   8.404 ns |   7.861 ns |     baseline |         | 0.3624 | 0.0019 |   2,295 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |    **JIS8** | **12,572.18 ns** | **155.748 ns** | **145.687 ns** | **1.47x faster** |   **0.02x** | **0.0458** |      **-** |     **488 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |    JIS8 | 18,487.06 ns | 158.341 ns | 148.112 ns |     baseline |         | 0.0305 |      - |     377 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |    JIS8 | 10,625.92 ns |  59.164 ns |  49.405 ns | 1.03x slower |   0.01x | 0.3204 |      - |   2,720 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |    JIS8 | 10,321.77 ns |  67.710 ns |  63.336 ns |     baseline |         | 0.4120 |      - |   2,664 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I8** |    **512.55 ns** |   **1.867 ns** |   **1.559 ns** | **3.26x faster** |   **0.01x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I8 |  1,670.66 ns |   2.906 ns |   2.718 ns |     baseline |         | 0.0057 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I8 |    772.69 ns |   3.342 ns |   3.126 ns | 2.42x faster |   0.01x | 0.0105 |      - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I8 |  1,868.86 ns |   6.000 ns |   5.319 ns |     baseline |         | 0.0134 |      - |      88 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I1** |    **112.21 ns** |   **0.228 ns** |   **0.190 ns** | **1.81x faster** |   **0.01x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I1 |    202.72 ns |   1.140 ns |   1.067 ns |     baseline |         | 0.0062 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I1 |    210.48 ns |   0.684 ns |   0.607 ns | 2.00x faster |   0.02x | 0.0105 |      - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I1 |    421.54 ns |   2.530 ns |   2.366 ns |     baseline |         | 0.0138 |      - |      88 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I2** |    **613.26 ns** |   **1.055 ns** |   **0.935 ns** | **1.61x faster** |   **0.01x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I2 |    986.46 ns |   4.097 ns |   3.632 ns |     baseline |         | 0.0057 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I2 |    599.15 ns |   2.677 ns |   2.504 ns | 1.99x faster |   0.01x | 0.0105 |      - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I2 |  1,192.32 ns |   2.694 ns |   2.250 ns |     baseline |         | 0.0134 |      - |      88 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **I4** |    **484.62 ns** |   **2.133 ns** |   **1.781 ns** | **2.04x faster** |   **0.01x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      I4 |    988.58 ns |   3.747 ns |   3.321 ns |     baseline |         | 0.0057 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      I4 |    495.67 ns |   1.341 ns |   1.120 ns | 2.43x faster |   0.01x | 0.0105 |      - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      I4 |  1,202.18 ns |   3.290 ns |   2.916 ns |     baseline |         | 0.0134 |      - |      88 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **F8** |    **661.27 ns** |   **3.879 ns** |   **3.439 ns** | **4.20x faster** |   **0.02x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      F8 |  2,774.25 ns |   6.517 ns |   5.442 ns |     baseline |         | 0.0038 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      F8 |    774.51 ns |   2.726 ns |   2.416 ns | 3.79x faster |   0.03x | 0.0105 |      - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      F8 |  2,937.93 ns |  17.002 ns |  15.072 ns |     baseline |         | 0.0114 |      - |      88 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **F4** |    **540.24 ns** |   **0.992 ns** |   **0.879 ns** | **3.98x faster** |   **0.02x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      F4 |  2,147.80 ns |   9.434 ns |   8.825 ns |     baseline |         | 0.0038 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      F4 |    637.19 ns |   2.457 ns |   2.299 ns | 3.37x faster |   0.02x | 0.0105 |      - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      F4 |  2,146.68 ns |   5.724 ns |   5.354 ns |     baseline |         | 0.0114 |      - |      88 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U8** |    **668.98 ns** |   **1.183 ns** |   **0.988 ns** | **2.49x faster** |   **0.01x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U8 |  1,665.33 ns |   3.055 ns |   2.708 ns |     baseline |         | 0.0057 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U8 |    592.22 ns |   2.621 ns |   2.452 ns | 3.16x faster |   0.02x | 0.0105 |      - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U8 |  1,873.66 ns |   8.899 ns |   6.948 ns |     baseline |         | 0.0134 |      - |      88 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U1** |    **103.17 ns** |   **0.346 ns** |   **0.324 ns** | **1.87x faster** |   **0.01x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U1 |    192.90 ns |   0.622 ns |   0.581 ns |     baseline |         | 0.0062 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U1 |    200.26 ns |   0.935 ns |   0.875 ns | 2.10x faster |   0.01x | 0.0105 |      - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U1 |    419.98 ns |   2.070 ns |   1.937 ns |     baseline |         | 0.0138 |      - |      88 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U2** |    **623.45 ns** |   **1.963 ns** |   **1.639 ns** | **1.25x faster** |   **0.01x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U2 |    781.28 ns |   8.542 ns |   7.990 ns |     baseline |         | 0.0057 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U2 |    479.03 ns |   2.755 ns |   2.442 ns | 2.07x faster |   0.02x | 0.0105 |      - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U2 |    991.09 ns |   4.162 ns |   3.893 ns |     baseline |         | 0.0134 |      - |      88 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
|             **EncodeTo** |             **.NET 6.0** | **1024** |      **U4** |    **487.15 ns** |   **0.684 ns** |   **0.640 ns** | **2.02x faster** |   **0.01x** | **0.0048** |      **-** |      **40 B** |
|             EncodeTo | .NET Framework 4.7.2 | 1024 |      U4 |    983.31 ns |   3.040 ns |   2.695 ns |     baseline |         | 0.0057 |      - |      40 B |
|                      |                      |      |         |              |            |            |              |         |        |        |           |
| DecodeFromFullBuffer |             .NET 6.0 | 1024 |      U4 |    569.77 ns |   1.247 ns |   0.974 ns | 2.10x faster |   0.01x | 0.0105 |      - |      88 B |
| DecodeFromFullBuffer | .NET Framework 4.7.2 | 1024 |      U4 |  1,194.35 ns |   2.924 ns |   2.592 ns |     baseline |         | 0.0134 |      - |      88 B |
