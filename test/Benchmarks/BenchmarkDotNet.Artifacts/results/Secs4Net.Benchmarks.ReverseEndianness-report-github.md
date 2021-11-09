``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100
  [Host]     : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
  Job-DQZPTL : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
  Job-APIYYI : .NET Framework 4.8 (4.8.4420.0), X64 RyuJIT


```
|                  Method |              Runtime | Categories | Size |      Mean |     Error |    StdDev |         Ratio | RatioSD |
|------------------------ |--------------------- |----------- |----- |----------:|----------:|----------:|--------------:|--------:|
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     UInt16 |   64 | 118.60 ns |  1.691 ns |  1.582 ns |  5.62x faster |   0.08x |
| ReverseEndiannessHelper |             .NET 6.0 |     UInt16 |   64 |  35.63 ns |  0.145 ns |  0.135 ns | 18.70x faster |   0.09x |
|        BinaryPrimitives |             .NET 6.0 |     UInt16 |   64 |  27.19 ns |  0.115 ns |  0.102 ns | 24.51x faster |   0.09x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     UInt16 |   64 | 666.57 ns |  1.361 ns |  1.207 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |     UInt16 |   64 |  57.96 ns |  0.123 ns |  0.109 ns | 11.50x faster |   0.03x |
|        BinaryPrimitives | .NET Framework 4.7.2 |     UInt16 |   64 |  51.66 ns |  0.586 ns |  0.548 ns | 12.92x faster |   0.13x |
|                         |                      |            |      |           |           |           |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     UInt32 |   64 | 182.23 ns |  0.651 ns |  0.609 ns |  4.07x faster |   0.02x |
| ReverseEndiannessHelper |             .NET 6.0 |     UInt32 |   64 |  28.06 ns |  0.060 ns |  0.050 ns | 26.44x faster |   0.07x |
|        BinaryPrimitives |             .NET 6.0 |     UInt32 |   64 |  26.38 ns |  0.133 ns |  0.124 ns | 28.12x faster |   0.15x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     UInt32 |   64 | 741.84 ns |  1.183 ns |  1.106 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |     UInt32 |   64 |  62.47 ns |  0.233 ns |  0.207 ns | 11.87x faster |   0.04x |
|        BinaryPrimitives | .NET Framework 4.7.2 |     UInt32 |   64 |  56.07 ns |  0.416 ns |  0.389 ns | 13.23x faster |   0.09x |
|                         |                      |            |      |           |           |           |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     UInt64 |   64 | 232.26 ns |  0.649 ns |  0.575 ns |  3.48x faster |   0.01x |
| ReverseEndiannessHelper |             .NET 6.0 |     UInt64 |   64 |  35.33 ns |  0.093 ns |  0.087 ns | 22.90x faster |   0.08x |
|        BinaryPrimitives |             .NET 6.0 |     UInt64 |   64 |  26.90 ns |  0.074 ns |  0.066 ns | 30.08x faster |   0.11x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     UInt64 |   64 | 809.16 ns |  2.122 ns |  1.881 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |     UInt64 |   64 | 103.17 ns |  0.337 ns |  0.316 ns |  7.84x faster |   0.03x |
|        BinaryPrimitives | .NET Framework 4.7.2 |     UInt64 |   64 |  98.11 ns |  0.612 ns |  0.573 ns |  8.25x faster |   0.05x |
|                         |                      |            |      |           |           |           |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |      Int16 |   64 | 146.68 ns |  0.784 ns |  0.733 ns |  4.79x faster |   0.03x |
| ReverseEndiannessHelper |             .NET 6.0 |      Int16 |   64 |  36.73 ns |  0.059 ns |  0.055 ns | 19.10x faster |   0.06x |
|        BinaryPrimitives |             .NET 6.0 |      Int16 |   64 |  34.81 ns |  0.071 ns |  0.055 ns | 20.15x faster |   0.08x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |      Int16 |   64 | 701.73 ns |  2.643 ns |  2.343 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |      Int16 |   64 |  67.87 ns |  0.369 ns |  0.345 ns | 10.33x faster |   0.07x |
|        BinaryPrimitives | .NET Framework 4.7.2 |      Int16 |   64 |  59.27 ns |  0.296 ns |  0.247 ns | 11.84x faster |   0.05x |
|                         |                      |            |      |           |           |           |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |      Int32 |   64 | 181.77 ns |  1.023 ns |  0.957 ns |  4.04x faster |   0.06x |
| ReverseEndiannessHelper |             .NET 6.0 |      Int32 |   64 |  28.44 ns |  0.260 ns |  0.244 ns | 25.84x faster |   0.52x |
|        BinaryPrimitives |             .NET 6.0 |      Int32 |   64 |  26.43 ns |  0.246 ns |  0.230 ns | 27.81x faster |   0.46x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |      Int32 |   64 | 734.81 ns | 11.693 ns | 10.938 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |      Int32 |   64 |  61.91 ns |  0.265 ns |  0.221 ns | 11.91x faster |   0.16x |
|        BinaryPrimitives | .NET Framework 4.7.2 |      Int32 |   64 |  62.55 ns |  0.564 ns |  0.528 ns | 11.75x faster |   0.18x |
|                         |                      |            |      |           |           |           |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |      Int64 |   64 | 231.45 ns |  0.618 ns |  0.578 ns |  3.37x faster |   0.01x |
| ReverseEndiannessHelper |             .NET 6.0 |      Int64 |   64 |  35.42 ns |  0.120 ns |  0.100 ns | 22.00x faster |   0.06x |
|        BinaryPrimitives |             .NET 6.0 |      Int64 |   64 |  26.75 ns |  0.185 ns |  0.164 ns | 29.14x faster |   0.17x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |      Int64 |   64 | 779.38 ns |  1.243 ns |  1.101 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |      Int64 |   64 | 101.57 ns |  0.486 ns |  0.455 ns |  7.67x faster |   0.04x |
|        BinaryPrimitives | .NET Framework 4.7.2 |      Int64 |   64 |  92.68 ns |  0.309 ns |  0.258 ns |  8.41x faster |   0.03x |
|                         |                      |            |      |           |           |           |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     Single |   64 | 178.90 ns |  1.164 ns |  1.032 ns |  4.06x faster |   0.03x |
| ReverseEndiannessHelper |             .NET 6.0 |     Single |   64 |  31.21 ns |  0.238 ns |  0.223 ns | 23.28x faster |   0.15x |
|        BinaryPrimitives |             .NET 6.0 |     Single |   64 |  30.58 ns |  0.149 ns |  0.132 ns | 23.76x faster |   0.16x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     Single |   64 | 726.57 ns |  3.025 ns |  2.830 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |     Single |   64 | 158.13 ns |  0.961 ns |  0.852 ns |  4.60x faster |   0.03x |
|        BinaryPrimitives | .NET Framework 4.7.2 |     Single |   64 | 149.41 ns |  0.726 ns |  0.679 ns |  4.86x faster |   0.03x |
|                         |                      |            |      |           |           |           |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     Double |   64 | 232.76 ns |  1.498 ns |  1.401 ns |  3.42x faster |   0.02x |
| ReverseEndiannessHelper |             .NET 6.0 |     Double |   64 |  31.23 ns |  0.232 ns |  0.217 ns | 25.48x faster |   0.19x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     Double |   64 | 795.83 ns |  1.451 ns |  1.286 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |     Double |   64 | 187.18 ns |  0.827 ns |  0.774 ns |  4.25x faster |   0.02x |
|                         |                      |            |      |           |           |           |               |         |
|        BinaryPrimitives |             .NET 6.0 |     Doulbe |   64 |  30.31 ns |  0.107 ns |  0.100 ns |             ? |       ? |
|        BinaryPrimitives | .NET Framework 4.7.2 |     Doulbe |   64 | 159.98 ns |  0.887 ns |  0.786 ns |             ? |       ? |
