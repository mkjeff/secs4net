``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1151 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.6.21355.2
  [Host]     : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-WGPAHI : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-CDVTON : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|                   Method |              Runtime | Categories | Size |      Mean |    Error |   StdDev |         Ratio | RatioSD | Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------------- |--------------------- |----------- |----- |----------:|---------:|---------:|--------------:|--------:|------:|------:|------:|----------:|
|        &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     UInt16 |   64 | 148.85 ns | 0.805 ns | 0.753 ns |  4.67x faster |   0.03x |     - |     - |     - |         - |
|  ReverseEndiannessHelper |             .NET 6.0 |     UInt16 |   64 | 101.10 ns | 0.346 ns | 0.307 ns |  6.88x faster |   0.04x |     - |     - |     - |         - |
|         BinaryPrimitives |             .NET 6.0 |     UInt16 |   64 |  33.31 ns | 0.116 ns | 0.103 ns | 20.89x faster |   0.14x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) |             .NET 6.0 |     UInt16 |   64 |  26.84 ns | 0.129 ns | 0.120 ns | 25.92x faster |   0.15x |     - |     - |     - |         - |
|        &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     UInt16 |   64 | 695.36 ns | 4.470 ns | 3.490 ns |      baseline |         |     - |     - |     - |         - |
|  ReverseEndiannessHelper | .NET Framework 4.7.2 |     UInt16 |   64 | 112.86 ns | 0.428 ns | 0.400 ns |  6.16x faster |   0.04x |     - |     - |     - |         - |
|         BinaryPrimitives | .NET Framework 4.7.2 |     UInt16 |   64 | 142.47 ns | 0.918 ns | 0.858 ns |  4.88x faster |   0.05x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) | .NET Framework 4.7.2 |     UInt16 |   64 |  51.25 ns | 0.262 ns | 0.245 ns | 13.56x faster |   0.09x |     - |     - |     - |         - |
|                          |                      |            |      |           |          |          |               |         |       |       |       |           |
|        &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     UInt32 |   64 | 183.77 ns | 0.591 ns | 0.493 ns |  4.00x faster |   0.01x |     - |     - |     - |         - |
|  ReverseEndiannessHelper |             .NET 6.0 |     UInt32 |   64 | 101.67 ns | 0.211 ns | 0.187 ns |  7.23x faster |   0.01x |     - |     - |     - |         - |
|         BinaryPrimitives |             .NET 6.0 |     UInt32 |   64 |  33.51 ns | 0.166 ns | 0.155 ns | 21.92x faster |   0.11x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) |             .NET 6.0 |     UInt32 |   64 |  26.33 ns | 0.104 ns | 0.097 ns | 27.91x faster |   0.08x |     - |     - |     - |         - |
|        &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     UInt32 |   64 | 734.90 ns | 1.886 ns | 1.672 ns |      baseline |         |     - |     - |     - |         - |
|  ReverseEndiannessHelper | .NET Framework 4.7.2 |     UInt32 |   64 | 112.82 ns | 0.333 ns | 0.311 ns |  6.51x faster |   0.03x |     - |     - |     - |         - |
|         BinaryPrimitives | .NET Framework 4.7.2 |     UInt32 |   64 | 173.94 ns | 0.849 ns | 0.794 ns |  4.23x faster |   0.02x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) | .NET Framework 4.7.2 |     UInt32 |   64 |  55.48 ns | 0.256 ns | 0.240 ns | 13.24x faster |   0.04x |     - |     - |     - |         - |
|                          |                      |            |      |           |          |          |               |         |       |       |       |           |
|        &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     UInt64 |   64 | 234.00 ns | 1.057 ns | 0.937 ns |  3.39x faster |   0.01x |     - |     - |     - |         - |
|  ReverseEndiannessHelper |             .NET 6.0 |     UInt64 |   64 | 101.24 ns | 0.332 ns | 0.311 ns |  7.84x faster |   0.02x |     - |     - |     - |         - |
|         BinaryPrimitives |             .NET 6.0 |     UInt64 |   64 |  33.53 ns | 0.132 ns | 0.123 ns | 23.67x faster |   0.11x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) |             .NET 6.0 |     UInt64 |   64 |  26.37 ns | 0.103 ns | 0.097 ns | 30.10x faster |   0.12x |     - |     - |     - |         - |
|        &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     UInt64 |   64 | 793.88 ns | 1.674 ns | 1.484 ns |      baseline |         |     - |     - |     - |         - |
|  ReverseEndiannessHelper | .NET Framework 4.7.2 |     UInt64 |   64 | 129.26 ns | 0.537 ns | 0.502 ns |  6.14x faster |   0.02x |     - |     - |     - |         - |
|         BinaryPrimitives | .NET Framework 4.7.2 |     UInt64 |   64 | 195.22 ns | 0.916 ns | 0.857 ns |  4.07x faster |   0.02x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) | .NET Framework 4.7.2 |     UInt64 |   64 |  95.89 ns | 0.351 ns | 0.328 ns |  8.28x faster |   0.04x |     - |     - |     - |         - |
|                          |                      |            |      |           |          |          |               |         |       |       |       |           |
|        &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |      Int16 |   64 | 148.78 ns | 0.838 ns | 0.784 ns |  4.67x faster |   0.02x |     - |     - |     - |         - |
|  ReverseEndiannessHelper |             .NET 6.0 |      Int16 |   64 | 102.29 ns | 0.372 ns | 0.330 ns |  6.79x faster |   0.03x |     - |     - |     - |         - |
|         BinaryPrimitives |             .NET 6.0 |      Int16 |   64 |  33.50 ns | 0.150 ns | 0.140 ns | 20.73x faster |   0.10x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) |             .NET 6.0 |      Int16 |   64 |  34.91 ns | 0.053 ns | 0.041 ns | 19.90x faster |   0.06x |     - |     - |     - |         - |
|        &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |      Int16 |   64 | 694.59 ns | 2.028 ns | 1.798 ns |      baseline |         |     - |     - |     - |         - |
|  ReverseEndiannessHelper | .NET Framework 4.7.2 |      Int16 |   64 | 114.45 ns | 0.504 ns | 0.471 ns |  6.07x faster |   0.03x |     - |     - |     - |         - |
|         BinaryPrimitives | .NET Framework 4.7.2 |      Int16 |   64 | 142.11 ns | 0.754 ns | 0.706 ns |  4.89x faster |   0.03x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) | .NET Framework 4.7.2 |      Int16 |   64 |  59.44 ns | 0.317 ns | 0.297 ns | 11.69x faster |   0.06x |     - |     - |     - |         - |
|                          |                      |            |      |           |          |          |               |         |       |       |       |           |
|        &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |      Int32 |   64 | 183.93 ns | 0.685 ns | 0.640 ns |  3.92x faster |   0.02x |     - |     - |     - |         - |
|  ReverseEndiannessHelper |             .NET 6.0 |      Int32 |   64 | 100.24 ns | 0.495 ns | 0.463 ns |  7.19x faster |   0.04x |     - |     - |     - |         - |
|         BinaryPrimitives |             .NET 6.0 |      Int32 |   64 |  33.43 ns | 0.150 ns | 0.141 ns | 21.55x faster |   0.10x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) |             .NET 6.0 |      Int32 |   64 |  26.35 ns | 0.151 ns | 0.141 ns | 27.34x faster |   0.15x |     - |     - |     - |         - |
|        &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |      Int32 |   64 | 720.45 ns | 1.767 ns | 1.653 ns |      baseline |         |     - |     - |     - |         - |
|  ReverseEndiannessHelper | .NET Framework 4.7.2 |      Int32 |   64 | 114.11 ns | 0.317 ns | 0.296 ns |  6.31x faster |   0.02x |     - |     - |     - |         - |
|         BinaryPrimitives | .NET Framework 4.7.2 |      Int32 |   64 | 171.91 ns | 3.409 ns | 4.058 ns |  4.19x faster |   0.11x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) | .NET Framework 4.7.2 |      Int32 |   64 |  63.28 ns | 0.236 ns | 0.221 ns | 11.39x faster |   0.05x |     - |     - |     - |         - |
|                          |                      |            |      |           |          |          |               |         |       |       |       |           |
|        &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |      Int64 |   64 | 236.21 ns | 1.042 ns | 0.975 ns |  3.33x faster |   0.01x |     - |     - |     - |         - |
|  ReverseEndiannessHelper |             .NET 6.0 |      Int64 |   64 | 100.24 ns | 0.203 ns | 0.180 ns |  7.85x faster |   0.02x |     - |     - |     - |         - |
|         BinaryPrimitives |             .NET 6.0 |      Int64 |   64 |  33.55 ns | 0.153 ns | 0.143 ns | 23.45x faster |   0.12x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) |             .NET 6.0 |      Int64 |   64 |  26.54 ns | 0.099 ns | 0.088 ns | 29.65x faster |   0.13x |     - |     - |     - |         - |
|        &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |      Int64 |   64 | 786.94 ns | 1.625 ns | 1.520 ns |      baseline |         |     - |     - |     - |         - |
|  ReverseEndiannessHelper | .NET Framework 4.7.2 |      Int64 |   64 | 131.20 ns | 0.430 ns | 0.402 ns |  6.00x faster |   0.02x |     - |     - |     - |         - |
|         BinaryPrimitives | .NET Framework 4.7.2 |      Int64 |   64 | 195.59 ns | 0.992 ns | 0.928 ns |  4.02x faster |   0.02x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) | .NET Framework 4.7.2 |      Int64 |   64 |  93.67 ns | 0.392 ns | 0.367 ns |  8.40x faster |   0.04x |     - |     - |     - |         - |
|                          |                      |            |      |           |          |          |               |         |       |       |       |           |
|        &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     Single |   64 | 184.16 ns | 0.539 ns | 0.477 ns |  3.99x faster |   0.01x |     - |     - |     - |         - |
|  ReverseEndiannessHelper |             .NET 6.0 |     Single |   64 | 116.72 ns | 0.506 ns | 0.473 ns |  6.29x faster |   0.03x |     - |     - |     - |         - |
|         BinaryPrimitives |             .NET 6.0 |     Single |   64 |  37.69 ns | 0.119 ns | 0.099 ns | 19.49x faster |   0.09x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) |             .NET 6.0 |     Single |   64 |  30.87 ns | 0.086 ns | 0.080 ns | 23.80x faster |   0.10x |     - |     - |     - |         - |
|        &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     Single |   64 | 734.68 ns | 2.527 ns | 2.363 ns |      baseline |         |     - |     - |     - |         - |
|  ReverseEndiannessHelper | .NET Framework 4.7.2 |     Single |   64 | 559.18 ns | 0.822 ns | 0.729 ns |  1.31x faster |   0.00x |     - |     - |     - |         - |
|         BinaryPrimitives | .NET Framework 4.7.2 |     Single |   64 | 222.12 ns | 0.921 ns | 0.862 ns |  3.31x faster |   0.02x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) | .NET Framework 4.7.2 |     Single |   64 | 134.11 ns | 0.420 ns | 0.393 ns |  5.48x faster |   0.02x |     - |     - |     - |         - |
|                          |                      |            |      |           |          |          |               |         |       |       |       |           |
|        &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     Double |   64 | 235.40 ns | 1.073 ns | 1.003 ns |  3.37x faster |   0.02x |     - |     - |     - |         - |
|  ReverseEndiannessHelper |             .NET 6.0 |     Double |   64 | 116.49 ns | 0.570 ns | 0.533 ns |  6.82x faster |   0.03x |     - |     - |     - |         - |
|        &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     Double |   64 | 794.25 ns | 2.171 ns | 2.031 ns |      baseline |         |     - |     - |     - |         - |
|  ReverseEndiannessHelper | .NET Framework 4.7.2 |     Double |   64 | 627.93 ns | 1.245 ns | 1.165 ns |  1.26x faster |   0.00x |     - |     - |     - |         - |
|                          |                      |            |      |           |          |          |               |         |       |       |       |           |
|         BinaryPrimitives |             .NET 6.0 |     Doulbe |   64 |  38.77 ns | 0.196 ns | 0.183 ns |             ? |       ? |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) |             .NET 6.0 |     Doulbe |   64 |  34.81 ns | 0.148 ns | 0.139 ns |             ? |       ? |     - |     - |     - |         - |
|         BinaryPrimitives | .NET Framework 4.7.2 |     Doulbe |   64 | 231.21 ns | 0.475 ns | 0.397 ns |             ? |       ? |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) | .NET Framework 4.7.2 |     Doulbe |   64 | 161.39 ns | 0.434 ns | 0.362 ns |             ? |       ? |     - |     - |     - |         - |
