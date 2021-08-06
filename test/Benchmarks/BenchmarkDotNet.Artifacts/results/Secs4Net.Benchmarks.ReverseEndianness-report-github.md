``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1151 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.6.21355.2
  [Host]     : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-IIWPMA : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-DEBAEW : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|                   Method |              Runtime | Categories | Size |      Mean |    Error |   StdDev | Ratio | RatioSD | Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------------- |--------------------- |----------- |----- |----------:|---------:|---------:|------:|--------:|------:|------:|------:|----------:|
|        &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     UInt16 |   64 | 149.06 ns | 0.308 ns | 0.273 ns |  0.21 |    0.00 |     - |     - |     - |         - |
|  ReverseEndiannessHelper |             .NET 6.0 |     UInt16 |   64 | 101.99 ns | 0.281 ns | 0.263 ns |  0.15 |    0.00 |     - |     - |     - |         - |
|         BinaryPrimitives |             .NET 6.0 |     UInt16 |   64 |  33.72 ns | 0.121 ns | 0.108 ns |  0.05 |    0.00 |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) |             .NET 6.0 |     UInt16 |   64 |  26.71 ns | 0.133 ns | 0.124 ns |  0.04 |    0.00 |     - |     - |     - |         - |
|        &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     UInt16 |   64 | 701.80 ns | 1.086 ns | 0.963 ns |  1.00 |    0.00 |     - |     - |     - |         - |
|  ReverseEndiannessHelper | .NET Framework 4.7.2 |     UInt16 |   64 | 128.86 ns | 0.594 ns | 0.555 ns |  0.18 |    0.00 |     - |     - |     - |         - |
|         BinaryPrimitives | .NET Framework 4.7.2 |     UInt16 |   64 | 174.77 ns | 0.439 ns | 0.389 ns |  0.25 |    0.00 |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) | .NET Framework 4.7.2 |     UInt16 |   64 |  50.85 ns | 0.201 ns | 0.188 ns |  0.07 |    0.00 |     - |     - |     - |         - |
|                          |                      |            |      |           |          |          |       |         |       |       |       |           |
|        &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     UInt32 |   64 | 184.94 ns | 0.704 ns | 0.625 ns |  0.26 |    0.00 |     - |     - |     - |         - |
|  ReverseEndiannessHelper |             .NET 6.0 |     UInt32 |   64 | 102.57 ns | 0.271 ns | 0.253 ns |  0.14 |    0.00 |     - |     - |     - |         - |
|         BinaryPrimitives |             .NET 6.0 |     UInt32 |   64 |  33.55 ns | 0.087 ns | 0.081 ns |  0.05 |    0.00 |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) |             .NET 6.0 |     UInt32 |   64 |  26.53 ns | 0.074 ns | 0.065 ns |  0.04 |    0.00 |     - |     - |     - |         - |
|        &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     UInt32 |   64 | 725.10 ns | 1.655 ns | 1.548 ns |  1.00 |    0.00 |     - |     - |     - |         - |
|  ReverseEndiannessHelper | .NET Framework 4.7.2 |     UInt32 |   64 | 128.32 ns | 0.334 ns | 0.313 ns |  0.18 |    0.00 |     - |     - |     - |         - |
|         BinaryPrimitives | .NET Framework 4.7.2 |     UInt32 |   64 | 144.62 ns | 0.879 ns | 0.822 ns |  0.20 |    0.00 |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) | .NET Framework 4.7.2 |     UInt32 |   64 |  64.06 ns | 0.249 ns | 0.221 ns |  0.09 |    0.00 |     - |     - |     - |         - |
|                          |                      |            |      |           |          |          |       |         |       |       |       |           |
|        &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     UInt64 |   64 | 236.11 ns | 1.547 ns | 1.447 ns |  0.30 |    0.00 |     - |     - |     - |         - |
|  ReverseEndiannessHelper |             .NET 6.0 |     UInt64 |   64 | 114.89 ns | 0.192 ns | 0.161 ns |  0.14 |    0.00 |     - |     - |     - |         - |
|         BinaryPrimitives |             .NET 6.0 |     UInt64 |   64 |  33.61 ns | 0.117 ns | 0.110 ns |  0.04 |    0.00 |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) |             .NET 6.0 |     UInt64 |   64 |  26.60 ns | 0.110 ns | 0.103 ns |  0.03 |    0.00 |     - |     - |     - |         - |
|        &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     UInt64 |   64 | 797.58 ns | 0.980 ns | 0.819 ns |  1.00 |    0.00 |     - |     - |     - |         - |
|  ReverseEndiannessHelper | .NET Framework 4.7.2 |     UInt64 |   64 | 130.81 ns | 0.437 ns | 0.409 ns |  0.16 |    0.00 |     - |     - |     - |         - |
|         BinaryPrimitives | .NET Framework 4.7.2 |     UInt64 |   64 | 195.33 ns | 0.466 ns | 0.436 ns |  0.24 |    0.00 |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) | .NET Framework 4.7.2 |     UInt64 |   64 |  96.27 ns | 0.419 ns | 0.392 ns |  0.12 |    0.00 |     - |     - |     - |         - |
|                          |                      |            |      |           |          |          |       |         |       |       |       |           |
|        &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |      Int16 |   64 | 149.19 ns | 0.601 ns | 0.562 ns |  0.22 |    0.00 |     - |     - |     - |         - |
|  ReverseEndiannessHelper |             .NET 6.0 |      Int16 |   64 | 115.69 ns | 0.260 ns | 0.243 ns |  0.17 |    0.00 |     - |     - |     - |         - |
|         BinaryPrimitives |             .NET 6.0 |      Int16 |   64 |  33.69 ns | 0.114 ns | 0.107 ns |  0.05 |    0.00 |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) |             .NET 6.0 |      Int16 |   64 |  35.26 ns | 0.080 ns | 0.074 ns |  0.05 |    0.00 |     - |     - |     - |         - |
|        &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |      Int16 |   64 | 690.41 ns | 1.190 ns | 1.113 ns |  1.00 |    0.00 |     - |     - |     - |         - |
|  ReverseEndiannessHelper | .NET Framework 4.7.2 |      Int16 |   64 | 129.77 ns | 0.349 ns | 0.291 ns |  0.19 |    0.00 |     - |     - |     - |         - |
|         BinaryPrimitives | .NET Framework 4.7.2 |      Int16 |   64 | 174.92 ns | 0.414 ns | 0.346 ns |  0.25 |    0.00 |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) | .NET Framework 4.7.2 |      Int16 |   64 |  64.60 ns | 0.160 ns | 0.133 ns |  0.09 |    0.00 |     - |     - |     - |         - |
|                          |                      |            |      |           |          |          |       |         |       |       |       |           |
|        &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |      Int32 |   64 | 184.92 ns | 0.787 ns | 0.736 ns |  0.25 |    0.00 |     - |     - |     - |         - |
|  ReverseEndiannessHelper |             .NET 6.0 |      Int32 |   64 | 115.56 ns | 0.205 ns | 0.192 ns |  0.16 |    0.00 |     - |     - |     - |         - |
|         BinaryPrimitives |             .NET 6.0 |      Int32 |   64 |  33.49 ns | 0.036 ns | 0.030 ns |  0.05 |    0.00 |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) |             .NET 6.0 |      Int32 |   64 |  26.50 ns | 0.098 ns | 0.092 ns |  0.04 |    0.00 |     - |     - |     - |         - |
|        &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |      Int32 |   64 | 734.10 ns | 1.834 ns | 1.716 ns |  1.00 |    0.00 |     - |     - |     - |         - |
|  ReverseEndiannessHelper | .NET Framework 4.7.2 |      Int32 |   64 | 114.86 ns | 0.355 ns | 0.332 ns |  0.16 |    0.00 |     - |     - |     - |         - |
|         BinaryPrimitives | .NET Framework 4.7.2 |      Int32 |   64 | 175.12 ns | 0.362 ns | 0.321 ns |  0.24 |    0.00 |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) | .NET Framework 4.7.2 |      Int32 |   64 |  55.51 ns | 0.404 ns | 0.378 ns |  0.08 |    0.00 |     - |     - |     - |         - |
|                          |                      |            |      |           |          |          |       |         |       |       |       |           |
|        &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |      Int64 |   64 | 237.28 ns | 1.138 ns | 1.064 ns |  0.30 |    0.00 |     - |     - |     - |         - |
|  ReverseEndiannessHelper |             .NET 6.0 |      Int64 |   64 | 114.97 ns | 0.191 ns | 0.169 ns |  0.14 |    0.00 |     - |     - |     - |         - |
|         BinaryPrimitives |             .NET 6.0 |      Int64 |   64 |  33.63 ns | 0.095 ns | 0.080 ns |  0.04 |    0.00 |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) |             .NET 6.0 |      Int64 |   64 |  26.77 ns | 0.096 ns | 0.090 ns |  0.03 |    0.00 |     - |     - |     - |         - |
|        &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |      Int64 |   64 | 794.27 ns | 2.190 ns | 1.941 ns |  1.00 |    0.00 |     - |     - |     - |         - |
|  ReverseEndiannessHelper | .NET Framework 4.7.2 |      Int64 |   64 | 130.73 ns | 0.413 ns | 0.387 ns |  0.16 |    0.00 |     - |     - |     - |         - |
|         BinaryPrimitives | .NET Framework 4.7.2 |      Int64 |   64 | 194.55 ns | 0.360 ns | 0.300 ns |  0.24 |    0.00 |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) | .NET Framework 4.7.2 |      Int64 |   64 |  96.33 ns | 0.368 ns | 0.307 ns |  0.12 |    0.00 |     - |     - |     - |         - |
|                          |                      |            |      |           |          |          |       |         |       |       |       |           |
|        &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     Single |   64 | 185.02 ns | 0.446 ns | 0.418 ns |  0.26 |    0.00 |     - |     - |     - |         - |
|  ReverseEndiannessHelper |             .NET 6.0 |     Single |   64 | 117.10 ns | 0.199 ns | 0.155 ns |  0.16 |    0.00 |     - |     - |     - |         - |
|         BinaryPrimitives |             .NET 6.0 |     Single |   64 |  38.85 ns | 0.167 ns | 0.156 ns |  0.05 |    0.00 |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) |             .NET 6.0 |     Single |   64 |  30.85 ns | 0.144 ns | 0.135 ns |  0.04 |    0.00 |     - |     - |     - |         - |
|        &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     Single |   64 | 724.65 ns | 1.875 ns | 1.754 ns |  1.00 |    0.00 |     - |     - |     - |         - |
|  ReverseEndiannessHelper | .NET Framework 4.7.2 |     Single |   64 | 567.68 ns | 1.309 ns | 1.160 ns |  0.78 |    0.00 |     - |     - |     - |         - |
|         BinaryPrimitives | .NET Framework 4.7.2 |     Single |   64 | 223.11 ns | 0.774 ns | 0.724 ns |  0.31 |    0.00 |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) | .NET Framework 4.7.2 |     Single |   64 | 134.93 ns | 0.845 ns | 0.749 ns |  0.19 |    0.00 |     - |     - |     - |         - |
|                          |                      |            |      |           |          |          |       |         |       |       |       |           |
|        &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     Double |   64 | 237.69 ns | 1.072 ns | 0.950 ns |  0.30 |    0.00 |     - |     - |     - |         - |
|  ReverseEndiannessHelper |             .NET 6.0 |     Double |   64 | 130.58 ns | 0.274 ns | 0.256 ns |  0.16 |    0.00 |     - |     - |     - |         - |
|        &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     Double |   64 | 796.40 ns | 1.358 ns | 1.270 ns |  1.00 |    0.00 |     - |     - |     - |         - |
|  ReverseEndiannessHelper | .NET Framework 4.7.2 |     Double |   64 | 631.12 ns | 1.211 ns | 1.074 ns |  0.79 |    0.00 |     - |     - |     - |         - |
|                          |                      |            |      |           |          |          |       |         |       |       |       |           |
|         BinaryPrimitives |             .NET 6.0 |     Doulbe |   64 |  38.84 ns | 0.177 ns | 0.157 ns |     ? |       ? |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) |             .NET 6.0 |     Doulbe |   64 |  35.03 ns | 0.108 ns | 0.095 ns |     ? |       ? |     - |     - |     - |         - |
|         BinaryPrimitives | .NET Framework 4.7.2 |     Doulbe |   64 | 241.89 ns | 1.025 ns | 0.908 ns |     ? |       ? |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) | .NET Framework 4.7.2 |     Doulbe |   64 | 162.65 ns | 0.997 ns | 0.932 ns |     ? |       ? |     - |     - |     - |         - |
