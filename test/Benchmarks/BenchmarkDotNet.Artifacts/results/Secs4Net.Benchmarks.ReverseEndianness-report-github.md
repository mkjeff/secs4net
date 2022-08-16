``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.400
  [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  Job-GVXGYT : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  Job-DSSDDM : .NET Framework 4.8 (4.8.4515.0), X64 RyuJIT


```
|                  Method |              Runtime | Categories | Size |      Mean |     Error |    StdDev |         Ratio | RatioSD |
|------------------------ |--------------------- |----------- |----- |----------:|----------:|----------:|--------------:|--------:|
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     UInt16 |   64 | 145.58 ns |  1.081 ns |  1.011 ns |  4.73x faster |   0.04x |
| ReverseEndiannessHelper |             .NET 6.0 |     UInt16 |   64 |  32.27 ns |  0.078 ns |  0.069 ns | 21.36x faster |   0.12x |
|        BinaryPrimitives |             .NET 6.0 |     UInt16 |   64 |  22.87 ns |  0.057 ns |  0.047 ns | 30.16x faster |   0.12x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     UInt16 |   64 | 689.07 ns |  2.885 ns |  2.698 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |     UInt16 |   64 |  51.33 ns |  0.231 ns |  0.216 ns | 13.43x faster |   0.06x |
|        BinaryPrimitives | .NET Framework 4.7.2 |     UInt16 |   64 |  43.37 ns |  0.338 ns |  0.316 ns | 15.89x faster |   0.14x |
|                         |                      |            |      |           |           |           |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     UInt32 |   64 | 177.95 ns |  1.191 ns |  1.056 ns |  4.14x faster |   0.07x |
| ReverseEndiannessHelper |             .NET 6.0 |     UInt32 |   64 |  26.29 ns |  0.181 ns |  0.170 ns | 28.00x faster |   0.39x |
|        BinaryPrimitives |             .NET 6.0 |     UInt32 |   64 |  22.17 ns |  0.153 ns |  0.143 ns | 33.21x faster |   0.40x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     UInt32 |   64 | 736.08 ns | 10.707 ns | 10.015 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |     UInt32 |   64 |  54.85 ns |  0.204 ns |  0.181 ns | 13.41x faster |   0.19x |
|        BinaryPrimitives | .NET Framework 4.7.2 |     UInt32 |   64 |  48.04 ns |  0.143 ns |  0.120 ns | 15.31x faster |   0.22x |
|                         |                      |            |      |           |           |           |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     UInt64 |   64 | 233.17 ns |  0.723 ns |  0.641 ns |  3.62x faster |   0.01x |
| ReverseEndiannessHelper |             .NET 6.0 |     UInt64 |   64 |  23.86 ns |  0.083 ns |  0.073 ns | 35.39x faster |   0.13x |
|        BinaryPrimitives |             .NET 6.0 |     UInt64 |   64 |  22.09 ns |  0.130 ns |  0.122 ns | 38.23x faster |   0.23x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     UInt64 |   64 | 844.37 ns |  1.875 ns |  1.754 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |     UInt64 |   64 |  95.48 ns |  0.576 ns |  0.539 ns |  8.84x faster |   0.04x |
|        BinaryPrimitives | .NET Framework 4.7.2 |     UInt64 |   64 |  88.77 ns |  0.751 ns |  0.702 ns |  9.51x faster |   0.08x |
|                         |                      |            |      |           |           |           |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |      Int16 |   64 | 144.77 ns |  0.928 ns |  0.822 ns |  4.85x faster |   0.04x |
| ReverseEndiannessHelper |             .NET 6.0 |      Int16 |   64 |  33.07 ns |  0.267 ns |  0.237 ns | 21.21x faster |   0.20x |
|        BinaryPrimitives |             .NET 6.0 |      Int16 |   64 |  29.58 ns |  0.220 ns |  0.206 ns | 23.72x faster |   0.17x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |      Int16 |   64 | 701.55 ns |  3.096 ns |  2.896 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |      Int16 |   64 |  59.11 ns |  0.355 ns |  0.332 ns | 11.87x faster |   0.08x |
|        BinaryPrimitives | .NET Framework 4.7.2 |      Int16 |   64 |  53.17 ns |  0.287 ns |  0.254 ns | 13.20x faster |   0.07x |
|                         |                      |            |      |           |           |           |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |      Int32 |   64 | 179.10 ns |  2.162 ns |  2.022 ns |  4.12x faster |   0.05x |
| ReverseEndiannessHelper |             .NET 6.0 |      Int32 |   64 |  25.99 ns |  0.150 ns |  0.141 ns | 28.42x faster |   0.20x |
|        BinaryPrimitives |             .NET 6.0 |      Int32 |   64 |  22.59 ns |  0.089 ns |  0.083 ns | 32.70x faster |   0.17x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |      Int32 |   64 | 738.59 ns |  2.128 ns |  1.887 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |      Int32 |   64 |  54.76 ns |  0.181 ns |  0.160 ns | 13.49x faster |   0.05x |
|        BinaryPrimitives | .NET Framework 4.7.2 |      Int32 |   64 |  47.94 ns |  0.445 ns |  0.416 ns | 15.39x faster |   0.13x |
|                         |                      |            |      |           |           |           |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |      Int64 |   64 | 232.00 ns |  1.103 ns |  1.032 ns |  3.65x faster |   0.02x |
| ReverseEndiannessHelper |             .NET 6.0 |      Int64 |   64 |  24.64 ns |  0.091 ns |  0.080 ns | 34.37x faster |   0.18x |
|        BinaryPrimitives |             .NET 6.0 |      Int64 |   64 |  22.00 ns |  0.151 ns |  0.141 ns | 38.49x faster |   0.34x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |      Int64 |   64 | 846.86 ns |  3.562 ns |  3.332 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |      Int64 |   64 |  93.40 ns |  0.370 ns |  0.328 ns |  9.07x faster |   0.05x |
|        BinaryPrimitives | .NET Framework 4.7.2 |      Int64 |   64 |  88.99 ns |  0.622 ns |  0.582 ns |  9.52x faster |   0.06x |
|                         |                      |            |      |           |           |           |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     Single |   64 | 178.15 ns |  1.227 ns |  1.087 ns |  4.22x faster |   0.03x |
| ReverseEndiannessHelper |             .NET 6.0 |     Single |   64 |  28.95 ns |  0.201 ns |  0.188 ns | 25.96x faster |   0.17x |
|        BinaryPrimitives |             .NET 6.0 |     Single |   64 |  24.75 ns |  0.137 ns |  0.128 ns | 30.36x faster |   0.19x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     Single |   64 | 751.43 ns |  2.822 ns |  2.639 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |     Single |   64 | 164.12 ns |  0.645 ns |  0.571 ns |  4.58x faster |   0.02x |
|        BinaryPrimitives | .NET Framework 4.7.2 |     Single |   64 | 155.33 ns |  0.614 ns |  0.544 ns |  4.84x faster |   0.03x |
|                         |                      |            |      |           |           |           |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     Double |   64 | 234.00 ns |  1.489 ns |  1.393 ns |  3.46x faster |   0.02x |
| ReverseEndiannessHelper |             .NET 6.0 |     Double |   64 |  28.83 ns |  0.106 ns |  0.099 ns | 28.06x faster |   0.12x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     Double |   64 | 808.84 ns |  2.179 ns |  2.039 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |     Double |   64 | 196.71 ns |  1.031 ns |  0.914 ns |  4.11x faster |   0.02x |
|                         |                      |            |      |           |           |           |               |         |
|        BinaryPrimitives |             .NET 6.0 |     Doulbe |   64 |  24.70 ns |  0.114 ns |  0.107 ns |             ? |       ? |
|        BinaryPrimitives | .NET Framework 4.7.2 |     Doulbe |   64 | 194.23 ns |  0.752 ns |  0.666 ns |             ? |       ? |
