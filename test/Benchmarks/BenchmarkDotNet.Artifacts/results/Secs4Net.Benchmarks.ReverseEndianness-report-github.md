``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1165 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.7.21379.14
  [Host]     : .NET 6.0.0 (6.0.21.37719), X64 RyuJIT
  Job-EQIOJP : .NET 6.0.0 (6.0.21.37719), X64 RyuJIT
  Job-RGTUSF : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|                  Method |              Runtime | Categories | Size |      Mean |    Error |   StdDev |         Ratio | RatioSD | Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------------ |--------------------- |----------- |----- |----------:|---------:|---------:|--------------:|--------:|------:|------:|------:|----------:|
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     UInt16 |   64 | 133.14 ns | 0.348 ns | 0.308 ns |  5.24x faster |   0.01x |     - |     - |     - |         - |
| ReverseEndiannessHelper |             .NET 6.0 |     UInt16 |   64 |  28.60 ns | 0.086 ns | 0.076 ns | 24.38x faster |   0.08x |     - |     - |     - |         - |
|        BinaryPrimitives |             .NET 6.0 |     UInt16 |   64 |  27.06 ns | 0.086 ns | 0.076 ns | 25.76x faster |   0.10x |     - |     - |     - |         - |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     UInt16 |   64 | 697.23 ns | 2.035 ns | 1.903 ns |      baseline |         |     - |     - |     - |         - |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |     UInt16 |   64 |  58.62 ns | 0.161 ns | 0.143 ns | 11.89x faster |   0.04x |     - |     - |     - |         - |
|        BinaryPrimitives | .NET Framework 4.7.2 |     UInt16 |   64 |  52.00 ns | 0.148 ns | 0.131 ns | 13.41x faster |   0.06x |     - |     - |     - |         - |
|                         |                      |            |      |           |          |          |               |         |       |       |       |           |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     UInt32 |   64 | 181.38 ns | 1.671 ns | 1.481 ns |  4.07x faster |   0.04x |     - |     - |     - |         - |
| ReverseEndiannessHelper |             .NET 6.0 |     UInt32 |   64 |  28.39 ns | 0.092 ns | 0.086 ns | 26.00x faster |   0.18x |     - |     - |     - |         - |
|        BinaryPrimitives |             .NET 6.0 |     UInt32 |   64 |  26.62 ns | 0.121 ns | 0.101 ns | 27.73x faster |   0.10x |     - |     - |     - |         - |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     UInt32 |   64 | 737.92 ns | 4.545 ns | 3.549 ns |      baseline |         |     - |     - |     - |         - |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |     UInt32 |   64 |  70.85 ns | 0.170 ns | 0.151 ns | 10.41x faster |   0.07x |     - |     - |     - |         - |
|        BinaryPrimitives | .NET Framework 4.7.2 |     UInt32 |   64 |  55.50 ns | 0.166 ns | 0.155 ns | 13.30x faster |   0.07x |     - |     - |     - |         - |
|                         |                      |            |      |           |          |          |               |         |       |       |       |           |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     UInt64 |   64 | 235.99 ns | 0.291 ns | 0.243 ns |  3.38x faster |   0.01x |     - |     - |     - |         - |
| ReverseEndiannessHelper |             .NET 6.0 |     UInt64 |   64 |  28.78 ns | 0.124 ns | 0.116 ns | 27.70x faster |   0.12x |     - |     - |     - |         - |
|        BinaryPrimitives |             .NET 6.0 |     UInt64 |   64 |  26.55 ns | 0.096 ns | 0.089 ns | 30.03x faster |   0.12x |     - |     - |     - |         - |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     UInt64 |   64 | 797.19 ns | 1.748 ns | 1.635 ns |      baseline |         |     - |     - |     - |         - |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |     UInt64 |   64 | 103.17 ns | 0.252 ns | 0.236 ns |  7.73x faster |   0.02x |     - |     - |     - |         - |
|        BinaryPrimitives | .NET Framework 4.7.2 |     UInt64 |   64 |  95.54 ns | 0.159 ns | 0.148 ns |  8.34x faster |   0.02x |     - |     - |     - |         - |
|                         |                      |            |      |           |          |          |               |         |       |       |       |           |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |      Int16 |   64 | 133.40 ns | 0.331 ns | 0.406 ns |  5.20x faster |   0.02x |     - |     - |     - |         - |
| ReverseEndiannessHelper |             .NET 6.0 |      Int16 |   64 |  36.91 ns | 0.098 ns | 0.087 ns | 18.80x faster |   0.05x |     - |     - |     - |         - |
|        BinaryPrimitives |             .NET 6.0 |      Int16 |   64 |  35.32 ns | 0.083 ns | 0.069 ns | 19.65x faster |   0.04x |     - |     - |     - |         - |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |      Int16 |   64 | 694.16 ns | 0.810 ns | 0.677 ns |      baseline |         |     - |     - |     - |         - |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |      Int16 |   64 |  66.66 ns | 0.499 ns | 0.467 ns | 10.41x faster |   0.08x |     - |     - |     - |         - |
|        BinaryPrimitives | .NET Framework 4.7.2 |      Int16 |   64 |  58.41 ns | 0.456 ns | 0.427 ns | 11.88x faster |   0.09x |     - |     - |     - |         - |
|                         |                      |            |      |           |          |          |               |         |       |       |       |           |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |      Int32 |   64 | 179.91 ns | 0.652 ns | 0.544 ns |  4.08x faster |   0.01x |     - |     - |     - |         - |
| ReverseEndiannessHelper |             .NET 6.0 |      Int32 |   64 |  28.42 ns | 0.054 ns | 0.048 ns | 25.81x faster |   0.05x |     - |     - |     - |         - |
|        BinaryPrimitives |             .NET 6.0 |      Int32 |   64 |  26.71 ns | 0.060 ns | 0.050 ns | 27.46x faster |   0.08x |     - |     - |     - |         - |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |      Int32 |   64 | 733.47 ns | 1.462 ns | 1.221 ns |      baseline |         |     - |     - |     - |         - |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |      Int32 |   64 |  62.15 ns | 0.450 ns | 0.420 ns | 11.79x faster |   0.08x |     - |     - |     - |         - |
|        BinaryPrimitives | .NET Framework 4.7.2 |      Int32 |   64 |  63.11 ns | 0.459 ns | 0.430 ns | 11.61x faster |   0.09x |     - |     - |     - |         - |
|                         |                      |            |      |           |          |          |               |         |       |       |       |           |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |      Int64 |   64 | 237.61 ns | 0.597 ns | 0.498 ns |  3.35x faster |   0.01x |     - |     - |     - |         - |
| ReverseEndiannessHelper |             .NET 6.0 |      Int64 |   64 |  39.18 ns | 0.182 ns | 0.170 ns | 20.32x faster |   0.10x |     - |     - |     - |         - |
|        BinaryPrimitives |             .NET 6.0 |      Int64 |   64 |  26.29 ns | 0.071 ns | 0.067 ns | 30.29x faster |   0.09x |     - |     - |     - |         - |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |      Int64 |   64 | 796.37 ns | 1.381 ns | 1.224 ns |      baseline |         |     - |     - |     - |         - |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |      Int64 |   64 | 102.48 ns | 0.290 ns | 0.272 ns |  7.77x faster |   0.02x |     - |     - |     - |         - |
|        BinaryPrimitives | .NET Framework 4.7.2 |      Int64 |   64 |  94.19 ns | 0.621 ns | 0.518 ns |  8.46x faster |   0.05x |     - |     - |     - |         - |
|                         |                      |            |      |           |          |          |               |         |       |       |       |           |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     Single |   64 | 181.19 ns | 0.974 ns | 0.911 ns |  3.99x faster |   0.02x |     - |     - |     - |         - |
| ReverseEndiannessHelper |             .NET 6.0 |     Single |   64 |  36.38 ns | 0.092 ns | 0.082 ns | 19.86x faster |   0.05x |     - |     - |     - |         - |
|        BinaryPrimitives |             .NET 6.0 |     Single |   64 |  30.77 ns | 0.038 ns | 0.032 ns | 23.48x faster |   0.05x |     - |     - |     - |         - |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     Single |   64 | 722.24 ns | 1.398 ns | 1.308 ns |      baseline |         |     - |     - |     - |         - |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |     Single |   64 | 143.26 ns | 0.354 ns | 0.331 ns |  5.04x faster |   0.01x |     - |     - |     - |         - |
|        BinaryPrimitives | .NET Framework 4.7.2 |     Single |   64 | 149.25 ns | 0.230 ns | 0.204 ns |  4.84x faster |   0.01x |     - |     - |     - |         - |
|                         |                      |            |      |           |          |          |               |         |       |       |       |           |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     Double |   64 | 236.60 ns | 0.466 ns | 0.435 ns |  3.36x faster |   0.01x |     - |     - |     - |         - |
| ReverseEndiannessHelper |             .NET 6.0 |     Double |   64 |  31.70 ns | 0.090 ns | 0.084 ns | 25.07x faster |   0.08x |     - |     - |     - |         - |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     Double |   64 | 794.88 ns | 1.824 ns | 1.617 ns |      baseline |         |     - |     - |     - |         - |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |     Double |   64 | 172.80 ns | 0.326 ns | 0.272 ns |  4.60x faster |   0.01x |     - |     - |     - |         - |
|                         |                      |            |      |           |          |          |               |         |       |       |       |           |
|        BinaryPrimitives |             .NET 6.0 |     Doulbe |   64 |  34.96 ns | 0.084 ns | 0.074 ns |             ? |       ? |     - |     - |     - |         - |
|        BinaryPrimitives | .NET Framework 4.7.2 |     Doulbe |   64 | 159.66 ns | 0.433 ns | 0.361 ns |             ? |       ? |     - |     - |     - |         - |
