``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1151 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.6.21355.2
  [Host]     : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-CDNPOR : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-ONIXFO : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|                   Method |        Job |              Runtime | Toolchain | Categories | Size |        Mean |    Error |   StdDev | Ratio | RatioSD |
|------------------------- |----------- |--------------------- |---------- |----------- |----- |------------:|---------:|---------:|------:|--------:|
|        &#39;Slice &amp; Reverse&#39; | Job-CDNPOR |             .NET 6.0 |    net6.0 |     UInt16 |  128 |   286.83 ns | 1.156 ns | 1.081 ns |  0.21 |    0.00 |
|  ReverseEndiannessHelper | Job-CDNPOR |             .NET 6.0 |    net6.0 |     UInt16 |  128 |   221.85 ns | 0.695 ns | 0.542 ns |  0.16 |    0.00 |
|         BinaryPrimitives | Job-CDNPOR |             .NET 6.0 |    net6.0 |     UInt16 |  128 |    64.03 ns | 0.226 ns | 0.212 ns |  0.05 |    0.00 |
| BinaryPrimitives(Unsafe) | Job-CDNPOR |             .NET 6.0 |    net6.0 |     UInt16 |  128 |    50.33 ns | 0.233 ns | 0.194 ns |  0.04 |    0.00 |
|        &#39;Slice &amp; Reverse&#39; | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |     UInt16 |  128 | 1,374.92 ns | 3.036 ns | 2.840 ns |  1.00 |    0.00 |
|  ReverseEndiannessHelper | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |     UInt16 |  128 |   234.56 ns | 0.577 ns | 0.511 ns |  0.17 |    0.00 |
|         BinaryPrimitives | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |     UInt16 |  128 |   335.61 ns | 2.381 ns | 2.110 ns |  0.24 |    0.00 |
| BinaryPrimitives(Unsafe) | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |     UInt16 |  128 |    90.85 ns | 0.391 ns | 0.366 ns |  0.07 |    0.00 |
|                          |            |                      |           |            |      |             |          |          |       |         |
|        &#39;Slice &amp; Reverse&#39; | Job-CDNPOR |             .NET 6.0 |    net6.0 |     UInt32 |  128 |   362.88 ns | 0.998 ns | 0.885 ns |  0.26 |    0.00 |
|  ReverseEndiannessHelper | Job-CDNPOR |             .NET 6.0 |    net6.0 |     UInt32 |  128 |   221.46 ns | 0.907 ns | 0.757 ns |  0.16 |    0.00 |
|         BinaryPrimitives | Job-CDNPOR |             .NET 6.0 |    net6.0 |     UInt32 |  128 |    64.61 ns | 0.249 ns | 0.233 ns |  0.05 |    0.00 |
| BinaryPrimitives(Unsafe) | Job-CDNPOR |             .NET 6.0 |    net6.0 |     UInt32 |  128 |    51.54 ns | 0.229 ns | 0.215 ns |  0.04 |    0.00 |
|        &#39;Slice &amp; Reverse&#39; | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |     UInt32 |  128 | 1,419.11 ns | 3.401 ns | 3.181 ns |  1.00 |    0.00 |
|  ReverseEndiannessHelper | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |     UInt32 |  128 |   204.16 ns | 0.586 ns | 0.519 ns |  0.14 |    0.00 |
|         BinaryPrimitives | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |     UInt32 |  128 |   267.08 ns | 2.728 ns | 2.552 ns |  0.19 |    0.00 |
| BinaryPrimitives(Unsafe) | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |     UInt32 |  128 |   113.27 ns | 0.427 ns | 0.399 ns |  0.08 |    0.00 |
|                          |            |                      |           |            |      |             |          |          |       |         |
|        &#39;Slice &amp; Reverse&#39; | Job-CDNPOR |             .NET 6.0 |    net6.0 |     UInt64 |  128 |   465.56 ns | 1.459 ns | 1.364 ns |  0.30 |    0.00 |
|  ReverseEndiannessHelper | Job-CDNPOR |             .NET 6.0 |    net6.0 |     UInt64 |  128 |   192.61 ns | 0.765 ns | 0.715 ns |  0.12 |    0.00 |
|         BinaryPrimitives | Job-CDNPOR |             .NET 6.0 |    net6.0 |     UInt64 |  128 |    64.35 ns | 0.290 ns | 0.271 ns |  0.04 |    0.00 |
| BinaryPrimitives(Unsafe) | Job-CDNPOR |             .NET 6.0 |    net6.0 |     UInt64 |  128 |    51.36 ns | 0.130 ns | 0.109 ns |  0.03 |    0.00 |
|        &#39;Slice &amp; Reverse&#39; | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |     UInt64 |  128 | 1,566.86 ns | 5.420 ns | 5.070 ns |  1.00 |    0.00 |
|  ReverseEndiannessHelper | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |     UInt64 |  128 |   239.08 ns | 0.963 ns | 0.804 ns |  0.15 |    0.00 |
|         BinaryPrimitives | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |     UInt64 |  128 |   370.10 ns | 1.172 ns | 1.096 ns |  0.24 |    0.00 |
| BinaryPrimitives(Unsafe) | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |     UInt64 |  128 |   174.59 ns | 0.760 ns | 0.710 ns |  0.11 |    0.00 |
|                          |            |                      |           |            |      |             |          |          |       |         |
|        &#39;Slice &amp; Reverse&#39; | Job-CDNPOR |             .NET 6.0 |    net6.0 |      Int16 |  128 |   286.93 ns | 1.928 ns | 1.804 ns |  0.21 |    0.00 |
|  ReverseEndiannessHelper | Job-CDNPOR |             .NET 6.0 |    net6.0 |      Int16 |  128 |   193.51 ns | 0.938 ns | 0.877 ns |  0.14 |    0.00 |
|         BinaryPrimitives | Job-CDNPOR |             .NET 6.0 |    net6.0 |      Int16 |  128 |    64.60 ns | 0.236 ns | 0.220 ns |  0.05 |    0.00 |
| BinaryPrimitives(Unsafe) | Job-CDNPOR |             .NET 6.0 |    net6.0 |      Int16 |  128 |    70.17 ns | 0.321 ns | 0.300 ns |  0.05 |    0.00 |
|        &#39;Slice &amp; Reverse&#39; | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |      Int16 |  128 | 1,366.95 ns | 2.903 ns | 2.573 ns |  1.00 |    0.00 |
|  ReverseEndiannessHelper | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |      Int16 |  128 |   235.75 ns | 0.827 ns | 0.774 ns |  0.17 |    0.00 |
|         BinaryPrimitives | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |      Int16 |  128 |   335.59 ns | 1.437 ns | 1.200 ns |  0.25 |    0.00 |
| BinaryPrimitives(Unsafe) | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |      Int16 |  128 |   114.14 ns | 0.571 ns | 0.534 ns |  0.08 |    0.00 |
|                          |            |                      |           |            |      |             |          |          |       |         |
|        &#39;Slice &amp; Reverse&#39; | Job-CDNPOR |             .NET 6.0 |    net6.0 |      Int32 |  128 |   362.11 ns | 0.946 ns | 0.885 ns |  0.25 |    0.00 |
|  ReverseEndiannessHelper | Job-CDNPOR |             .NET 6.0 |    net6.0 |      Int32 |  128 |   191.75 ns | 0.621 ns | 0.580 ns |  0.13 |    0.00 |
|         BinaryPrimitives | Job-CDNPOR |             .NET 6.0 |    net6.0 |      Int32 |  128 |    64.18 ns | 0.226 ns | 0.211 ns |  0.04 |    0.00 |
| BinaryPrimitives(Unsafe) | Job-CDNPOR |             .NET 6.0 |    net6.0 |      Int32 |  128 |    51.43 ns | 0.207 ns | 0.194 ns |  0.04 |    0.00 |
|        &#39;Slice &amp; Reverse&#39; | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |      Int32 |  128 | 1,447.03 ns | 2.903 ns | 2.573 ns |  1.00 |    0.00 |
|  ReverseEndiannessHelper | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |      Int32 |  128 |   203.93 ns | 0.878 ns | 0.822 ns |  0.14 |    0.00 |
|         BinaryPrimitives | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |      Int32 |  128 |   334.76 ns | 1.690 ns | 1.581 ns |  0.23 |    0.00 |
| BinaryPrimitives(Unsafe) | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |      Int32 |  128 |    98.63 ns | 0.404 ns | 0.378 ns |  0.07 |    0.00 |
|                          |            |                      |           |            |      |             |          |          |       |         |
|        &#39;Slice &amp; Reverse&#39; | Job-CDNPOR |             .NET 6.0 |    net6.0 |      Int64 |  128 |   466.17 ns | 1.671 ns | 1.482 ns |  0.30 |    0.00 |
|  ReverseEndiannessHelper | Job-CDNPOR |             .NET 6.0 |    net6.0 |      Int64 |  128 |   191.43 ns | 0.554 ns | 0.518 ns |  0.12 |    0.00 |
|         BinaryPrimitives | Job-CDNPOR |             .NET 6.0 |    net6.0 |      Int64 |  128 |    64.05 ns | 0.198 ns | 0.165 ns |  0.04 |    0.00 |
| BinaryPrimitives(Unsafe) | Job-CDNPOR |             .NET 6.0 |    net6.0 |      Int64 |  128 |    51.54 ns | 0.259 ns | 0.242 ns |  0.03 |    0.00 |
|        &#39;Slice &amp; Reverse&#39; | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |      Int64 |  128 | 1,561.10 ns | 5.512 ns | 5.156 ns |  1.00 |    0.00 |
|  ReverseEndiannessHelper | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |      Int64 |  128 |   238.05 ns | 0.855 ns | 0.799 ns |  0.15 |    0.00 |
|         BinaryPrimitives | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |      Int64 |  128 |   370.90 ns | 1.344 ns | 1.257 ns |  0.24 |    0.00 |
| BinaryPrimitives(Unsafe) | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |      Int64 |  128 |   173.74 ns | 0.731 ns | 0.684 ns |  0.11 |    0.00 |
|                          |            |                      |           |            |      |             |          |          |       |         |
|        &#39;Slice &amp; Reverse&#39; | Job-CDNPOR |             .NET 6.0 |    net6.0 |     Single |  128 |   362.86 ns | 1.024 ns | 0.908 ns |  0.26 |    0.00 |
|  ReverseEndiannessHelper | Job-CDNPOR |             .NET 6.0 |    net6.0 |     Single |  128 |   223.18 ns | 0.787 ns | 0.736 ns |  0.16 |    0.00 |
|         BinaryPrimitives | Job-CDNPOR |             .NET 6.0 |    net6.0 |     Single |  128 |    76.06 ns | 0.323 ns | 0.286 ns |  0.05 |    0.00 |
| BinaryPrimitives(Unsafe) | Job-CDNPOR |             .NET 6.0 |    net6.0 |     Single |  128 |    59.76 ns | 0.252 ns | 0.236 ns |  0.04 |    0.00 |
|        &#39;Slice &amp; Reverse&#39; | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |     Single |  128 | 1,417.58 ns | 3.655 ns | 3.419 ns |  1.00 |    0.00 |
|  ReverseEndiannessHelper | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |     Single |  128 | 1,091.53 ns | 2.139 ns | 2.000 ns |  0.77 |    0.00 |
|         BinaryPrimitives | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |     Single |  128 |   422.59 ns | 1.046 ns | 0.978 ns |  0.30 |    0.00 |
| BinaryPrimitives(Unsafe) | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |     Single |  128 |   222.99 ns | 0.633 ns | 0.561 ns |  0.16 |    0.00 |
|                          |            |                      |           |            |      |             |          |          |       |         |
|        &#39;Slice &amp; Reverse&#39; | Job-CDNPOR |             .NET 6.0 |    net6.0 |     Double |  128 |   464.35 ns | 1.720 ns | 1.609 ns |  0.30 |    0.00 |
|  ReverseEndiannessHelper | Job-CDNPOR |             .NET 6.0 |    net6.0 |     Double |  128 |   193.84 ns | 1.102 ns | 1.030 ns |  0.12 |    0.00 |
|        &#39;Slice &amp; Reverse&#39; | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |     Double |  128 | 1,565.42 ns | 3.130 ns | 2.928 ns |  1.00 |    0.00 |
|  ReverseEndiannessHelper | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |     Double |  128 | 1,239.22 ns | 3.673 ns | 3.256 ns |  0.79 |    0.00 |
|                          |            |                      |           |            |      |             |          |          |       |         |
|         BinaryPrimitives | Job-CDNPOR |             .NET 6.0 |    net6.0 |     Doulbe |  128 |    75.81 ns | 0.310 ns | 0.275 ns |     ? |       ? |
| BinaryPrimitives(Unsafe) | Job-CDNPOR |             .NET 6.0 |    net6.0 |     Doulbe |  128 |    60.66 ns | 0.324 ns | 0.303 ns |     ? |       ? |
|         BinaryPrimitives | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |     Doulbe |  128 |   456.94 ns | 0.955 ns | 0.893 ns |     ? |       ? |
| BinaryPrimitives(Unsafe) | Job-ONIXFO | .NET Framework 4.7.2 |    net472 |     Doulbe |  128 |   297.84 ns | 0.990 ns | 0.926 ns |     ? |       ? |
