``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1237 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-rc.1.21458.32
  [Host]     : .NET 6.0.0 (6.0.21.45113), X64 RyuJIT
  Job-POERRH : .NET 6.0.0 (6.0.21.45113), X64 RyuJIT
  Job-QPPLMB : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|                  Method |              Runtime | Categories | Size |      Mean |    Error |   StdDev |         Ratio | RatioSD |
|------------------------ |--------------------- |----------- |----- |----------:|---------:|---------:|--------------:|--------:|
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     UInt16 |   64 | 149.87 ns | 0.677 ns | 0.600 ns |  4.72x faster |   0.03x |
| ReverseEndiannessHelper |             .NET 6.0 |     UInt16 |   64 |  28.65 ns | 0.115 ns | 0.108 ns | 24.67x faster |   0.11x |
|        BinaryPrimitives |             .NET 6.0 |     UInt16 |   64 |  27.78 ns | 0.110 ns | 0.098 ns | 25.46x faster |   0.10x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     UInt16 |   64 | 706.96 ns | 2.015 ns | 1.683 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |     UInt16 |   64 |  59.52 ns | 0.577 ns | 0.511 ns | 11.88x faster |   0.09x |
|        BinaryPrimitives | .NET Framework 4.7.2 |     UInt16 |   64 |  51.82 ns | 0.078 ns | 0.073 ns | 13.64x faster |   0.04x |
|                         |                      |            |      |           |          |          |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     UInt32 |   64 | 183.19 ns | 1.032 ns | 0.966 ns |  4.07x faster |   0.02x |
| ReverseEndiannessHelper |             .NET 6.0 |     UInt32 |   64 |  29.53 ns | 0.124 ns | 0.116 ns | 25.25x faster |   0.10x |
|        BinaryPrimitives |             .NET 6.0 |     UInt32 |   64 |  27.46 ns | 0.170 ns | 0.151 ns | 27.17x faster |   0.16x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     UInt32 |   64 | 745.86 ns | 1.043 ns | 0.924 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |     UInt32 |   64 |  63.83 ns | 0.272 ns | 0.241 ns | 11.69x faster |   0.05x |
|        BinaryPrimitives | .NET Framework 4.7.2 |     UInt32 |   64 |  55.97 ns | 0.092 ns | 0.077 ns | 13.33x faster |   0.02x |
|                         |                      |            |      |           |          |          |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     UInt64 |   64 | 237.14 ns | 1.217 ns | 1.139 ns |  3.41x faster |   0.02x |
| ReverseEndiannessHelper |             .NET 6.0 |     UInt64 |   64 |  36.25 ns | 0.128 ns | 0.107 ns | 22.29x faster |   0.08x |
|        BinaryPrimitives |             .NET 6.0 |     UInt64 |   64 |  26.88 ns | 0.058 ns | 0.051 ns | 30.05x faster |   0.09x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     UInt64 |   64 | 807.85 ns | 1.936 ns | 1.717 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |     UInt64 |   64 | 102.72 ns | 0.440 ns | 0.412 ns |  7.86x faster |   0.04x |
|        BinaryPrimitives | .NET Framework 4.7.2 |     UInt64 |   64 |  93.66 ns | 0.254 ns | 0.225 ns |  8.63x faster |   0.03x |
|                         |                      |            |      |           |          |          |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |      Int16 |   64 | 135.19 ns | 0.836 ns | 0.782 ns |  5.22x faster |   0.03x |
| ReverseEndiannessHelper |             .NET 6.0 |      Int16 |   64 |  37.62 ns | 0.114 ns | 0.101 ns | 18.78x faster |   0.05x |
|        BinaryPrimitives |             .NET 6.0 |      Int16 |   64 |  35.56 ns | 0.097 ns | 0.081 ns | 19.87x faster |   0.09x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |      Int16 |   64 | 706.47 ns | 1.900 ns | 1.684 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |      Int16 |   64 |  67.87 ns | 0.092 ns | 0.082 ns | 10.41x faster |   0.02x |
|        BinaryPrimitives | .NET Framework 4.7.2 |      Int16 |   64 |  60.35 ns | 0.355 ns | 0.332 ns | 11.71x faster |   0.07x |
|                         |                      |            |      |           |          |          |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |      Int32 |   64 | 186.34 ns | 0.754 ns | 0.668 ns |  3.97x faster |   0.02x |
| ReverseEndiannessHelper |             .NET 6.0 |      Int32 |   64 |  29.30 ns | 0.215 ns | 0.180 ns | 25.23x faster |   0.16x |
|        BinaryPrimitives |             .NET 6.0 |      Int32 |   64 |  26.75 ns | 0.056 ns | 0.052 ns | 27.64x faster |   0.05x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |      Int32 |   64 | 739.22 ns | 1.213 ns | 1.013 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |      Int32 |   64 |  62.74 ns | 0.191 ns | 0.179 ns | 11.78x faster |   0.03x |
|        BinaryPrimitives | .NET Framework 4.7.2 |      Int32 |   64 |  63.35 ns | 0.406 ns | 0.380 ns | 11.69x faster |   0.05x |
|                         |                      |            |      |           |          |          |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |      Int64 |   64 | 238.96 ns | 1.183 ns | 1.106 ns |  3.32x faster |   0.03x |
| ReverseEndiannessHelper |             .NET 6.0 |      Int64 |   64 |  36.92 ns | 0.180 ns | 0.169 ns | 21.51x faster |   0.21x |
|        BinaryPrimitives |             .NET 6.0 |      Int64 |   64 |  26.72 ns | 0.051 ns | 0.040 ns | 29.77x faster |   0.26x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |      Int64 |   64 | 794.03 ns | 7.313 ns | 6.840 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |      Int64 |   64 | 102.74 ns | 0.344 ns | 0.305 ns |  7.73x faster |   0.07x |
|        BinaryPrimitives | .NET Framework 4.7.2 |      Int64 |   64 |  95.13 ns | 0.448 ns | 0.420 ns |  8.35x faster |   0.08x |
|                         |                      |            |      |           |          |          |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     Single |   64 | 184.25 ns | 0.522 ns | 0.488 ns |  4.05x faster |   0.01x |
| ReverseEndiannessHelper |             .NET 6.0 |     Single |   64 |  31.86 ns | 0.128 ns | 0.120 ns | 23.40x faster |   0.10x |
|        BinaryPrimitives |             .NET 6.0 |     Single |   64 |  35.37 ns | 0.375 ns | 0.350 ns | 21.11x faster |   0.19x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     Single |   64 | 745.72 ns | 1.470 ns | 1.303 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |     Single |   64 | 157.29 ns | 0.530 ns | 0.470 ns |  4.74x faster |   0.02x |
|        BinaryPrimitives | .NET Framework 4.7.2 |     Single |   64 | 125.37 ns | 0.477 ns | 0.399 ns |  5.95x faster |   0.02x |
|                         |                      |            |      |           |          |          |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     Double |   64 | 237.23 ns | 0.473 ns | 0.395 ns |  3.41x faster |   0.01x |
| ReverseEndiannessHelper |             .NET 6.0 |     Double |   64 |  36.57 ns | 0.242 ns | 0.226 ns | 22.09x faster |   0.13x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     Double |   64 | 808.06 ns | 1.342 ns | 1.189 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |     Double |   64 | 187.95 ns | 0.841 ns | 0.787 ns |  4.30x faster |   0.02x |
|                         |                      |            |      |           |          |          |               |         |
|        BinaryPrimitives |             .NET 6.0 |     Doulbe |   64 |  35.14 ns | 0.072 ns | 0.067 ns |             ? |       ? |
|        BinaryPrimitives | .NET Framework 4.7.2 |     Doulbe |   64 | 166.44 ns | 0.769 ns | 0.682 ns |             ? |       ? |
