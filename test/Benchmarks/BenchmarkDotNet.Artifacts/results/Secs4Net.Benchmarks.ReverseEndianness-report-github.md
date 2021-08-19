``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1165 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.7.21379.14
  [Host]     : .NET 6.0.0 (6.0.21.37719), X64 RyuJIT
  Job-OMATBZ : .NET 6.0.0 (6.0.21.37719), X64 RyuJIT
  Job-NJTDJS : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|                  Method |              Runtime | Categories | Size |      Mean |    Error |   StdDev |         Ratio | RatioSD |
|------------------------ |--------------------- |----------- |----- |----------:|---------:|---------:|--------------:|--------:|
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     UInt16 |   64 | 132.94 ns | 0.388 ns | 0.363 ns |  5.22x faster |   0.02x |
| ReverseEndiannessHelper |             .NET 6.0 |     UInt16 |   64 |  35.59 ns | 0.104 ns | 0.097 ns | 19.49x faster |   0.06x |
|        BinaryPrimitives |             .NET 6.0 |     UInt16 |   64 |  26.91 ns | 0.067 ns | 0.059 ns | 25.78x faster |   0.07x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     UInt16 |   64 | 693.71 ns | 0.931 ns | 0.825 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |     UInt16 |   64 |  58.96 ns | 0.190 ns | 0.178 ns | 11.76x faster |   0.04x |
|        BinaryPrimitives | .NET Framework 4.7.2 |     UInt16 |   64 |  52.12 ns | 0.127 ns | 0.113 ns | 13.31x faster |   0.04x |
|                         |                      |            |      |           |          |          |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     UInt32 |   64 | 182.22 ns | 1.223 ns | 1.144 ns |  4.03x faster |   0.03x |
| ReverseEndiannessHelper |             .NET 6.0 |     UInt32 |   64 |  28.18 ns | 0.092 ns | 0.086 ns | 26.07x faster |   0.11x |
|        BinaryPrimitives |             .NET 6.0 |     UInt32 |   64 |  26.31 ns | 0.062 ns | 0.058 ns | 27.94x faster |   0.05x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     UInt32 |   64 | 734.58 ns | 1.807 ns | 1.602 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |     UInt32 |   64 |  63.25 ns | 0.436 ns | 0.408 ns | 11.61x faster |   0.08x |
|        BinaryPrimitives | .NET Framework 4.7.2 |     UInt32 |   64 |  54.90 ns | 0.110 ns | 0.086 ns | 13.38x faster |   0.04x |
|                         |                      |            |      |           |          |          |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     UInt64 |   64 | 235.95 ns | 0.514 ns | 0.456 ns |  3.35x faster |   0.01x |
| ReverseEndiannessHelper |             .NET 6.0 |     UInt64 |   64 |  35.65 ns | 0.102 ns | 0.095 ns | 22.17x faster |   0.06x |
|        BinaryPrimitives |             .NET 6.0 |     UInt64 |   64 |  26.43 ns | 0.080 ns | 0.075 ns | 29.91x faster |   0.10x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     UInt64 |   64 | 790.40 ns | 1.338 ns | 1.251 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |     UInt64 |   64 | 101.08 ns | 0.431 ns | 0.403 ns |  7.82x faster |   0.03x |
|        BinaryPrimitives | .NET Framework 4.7.2 |     UInt64 |   64 |  92.78 ns | 0.599 ns | 0.560 ns |  8.52x faster |   0.05x |
|                         |                      |            |      |           |          |          |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |      Int16 |   64 | 132.78 ns | 0.360 ns | 0.319 ns |  5.20x faster |   0.02x |
| ReverseEndiannessHelper |             .NET 6.0 |      Int16 |   64 |  38.54 ns | 0.198 ns | 0.185 ns | 17.91x faster |   0.07x |
|        BinaryPrimitives |             .NET 6.0 |      Int16 |   64 |  35.04 ns | 0.099 ns | 0.093 ns | 19.69x faster |   0.07x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |      Int16 |   64 | 690.06 ns | 1.296 ns | 1.213 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |      Int16 |   64 |  66.52 ns | 0.178 ns | 0.167 ns | 10.37x faster |   0.02x |
|        BinaryPrimitives | .NET Framework 4.7.2 |      Int16 |   64 |  58.79 ns | 0.339 ns | 0.317 ns | 11.74x faster |   0.07x |
|                         |                      |            |      |           |          |          |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |      Int32 |   64 | 180.06 ns | 1.265 ns | 1.121 ns |  4.08x faster |   0.03x |
| ReverseEndiannessHelper |             .NET 6.0 |      Int32 |   64 |  29.42 ns | 0.245 ns | 0.229 ns | 24.93x faster |   0.20x |
|        BinaryPrimitives |             .NET 6.0 |      Int32 |   64 |  26.64 ns | 0.180 ns | 0.168 ns | 27.56x faster |   0.21x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |      Int32 |   64 | 733.76 ns | 1.859 ns | 1.648 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |      Int32 |   64 |  62.11 ns | 0.348 ns | 0.326 ns | 11.81x faster |   0.06x |
|        BinaryPrimitives | .NET Framework 4.7.2 |      Int32 |   64 |  62.35 ns | 0.214 ns | 0.200 ns | 11.77x faster |   0.04x |
|                         |                      |            |      |           |          |          |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |      Int64 |   64 | 235.71 ns | 1.010 ns | 0.945 ns |  3.43x faster |   0.01x |
| ReverseEndiannessHelper |             .NET 6.0 |      Int64 |   64 |  38.96 ns | 0.199 ns | 0.186 ns | 20.72x faster |   0.12x |
|        BinaryPrimitives |             .NET 6.0 |      Int64 |   64 |  27.12 ns | 0.132 ns | 0.123 ns | 29.77x faster |   0.16x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |      Int64 |   64 | 807.34 ns | 1.732 ns | 1.621 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |      Int64 |   64 | 101.49 ns | 0.331 ns | 0.310 ns |  7.95x faster |   0.02x |
|        BinaryPrimitives | .NET Framework 4.7.2 |      Int64 |   64 |  93.70 ns | 0.191 ns | 0.179 ns |  8.62x faster |   0.02x |
|                         |                      |            |      |           |          |          |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     Single |   64 | 180.39 ns | 2.674 ns | 2.502 ns |  4.07x faster |   0.06x |
| ReverseEndiannessHelper |             .NET 6.0 |     Single |   64 |  31.82 ns | 0.098 ns | 0.087 ns | 23.05x faster |   0.07x |
|        BinaryPrimitives |             .NET 6.0 |     Single |   64 |  34.80 ns | 0.124 ns | 0.110 ns | 21.07x faster |   0.07x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     Single |   64 | 733.56 ns | 1.447 ns | 1.354 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |     Single |   64 | 155.50 ns | 0.154 ns | 0.137 ns |  4.72x faster |   0.01x |
|        BinaryPrimitives | .NET Framework 4.7.2 |     Single |   64 | 124.10 ns | 0.321 ns | 0.300 ns |  5.91x faster |   0.02x |
|                         |                      |            |      |           |          |          |               |         |
|       &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     Double |   64 | 236.22 ns | 1.471 ns | 1.228 ns |  3.35x faster |   0.02x |
| ReverseEndiannessHelper |             .NET 6.0 |     Double |   64 |  36.28 ns | 0.168 ns | 0.157 ns | 21.79x faster |   0.13x |
|       &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     Double |   64 | 790.60 ns | 2.111 ns | 1.975 ns |      baseline |         |
| ReverseEndiannessHelper | .NET Framework 4.7.2 |     Double |   64 | 186.27 ns | 0.447 ns | 0.418 ns |  4.24x faster |   0.02x |
|                         |                      |            |      |           |          |          |               |         |
|        BinaryPrimitives |             .NET 6.0 |     Doulbe |   64 |  30.86 ns | 0.129 ns | 0.121 ns |             ? |       ? |
|        BinaryPrimitives | .NET Framework 4.7.2 |     Doulbe |   64 | 164.43 ns | 0.219 ns | 0.194 ns |             ? |       ? |
