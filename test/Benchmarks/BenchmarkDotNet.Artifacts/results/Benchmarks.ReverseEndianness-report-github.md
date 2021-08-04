``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1151 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.6.21355.2
  [Host]     : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-AYIXQQ : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-IPVSTG : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|                  Method |        Job |              Runtime | Toolchain | Categories | Size |        Mean |     Error |    StdDev | Ratio | RatioSD |
|------------------------ |----------- |--------------------- |---------- |----------- |----- |------------:|----------:|----------:|------:|--------:|
|       &#39;Slice &amp; Reverse&#39; | Job-AYIXQQ |             .NET 6.0 |    net6.0 |     UInt16 |  128 |   287.53 ns |  1.159 ns |  1.027 ns |  0.21 |    0.00 |
| ReverseEndiannessHelper | Job-AYIXQQ |             .NET 6.0 |    net6.0 |     UInt16 |  128 |   225.31 ns |  1.553 ns |  1.453 ns |  0.16 |    0.00 |
|        BinaryPrimitives | Job-AYIXQQ |             .NET 6.0 |    net6.0 |     UInt16 |  128 |    64.17 ns |  0.271 ns |  0.253 ns |  0.05 |    0.00 |
|       &#39;Slice &amp; Reverse&#39; | Job-IPVSTG | .NET Framework 4.7.2 |    net472 |     UInt16 |  128 | 1,392.98 ns |  3.765 ns |  3.144 ns |  1.00 |    0.00 |
| ReverseEndiannessHelper | Job-IPVSTG | .NET Framework 4.7.2 |    net472 |     UInt16 |  128 |   476.74 ns |  0.741 ns |  0.657 ns |  0.34 |    0.00 |
|        BinaryPrimitives | Job-IPVSTG | .NET Framework 4.7.2 |    net472 |     UInt16 |  128 |   335.24 ns |  1.302 ns |  1.154 ns |  0.24 |    0.00 |
|                         |            |                      |           |            |      |             |           |           |       |         |
|       &#39;Slice &amp; Reverse&#39; | Job-AYIXQQ |             .NET 6.0 |    net6.0 |     UInt32 |  128 |   365.72 ns |  0.499 ns |  0.442 ns |  0.25 |    0.00 |
| ReverseEndiannessHelper | Job-AYIXQQ |             .NET 6.0 |    net6.0 |     UInt32 |  128 |   224.40 ns |  0.532 ns |  0.497 ns |  0.15 |    0.00 |
|        BinaryPrimitives | Job-AYIXQQ |             .NET 6.0 |    net6.0 |     UInt32 |  128 |    64.36 ns |  0.233 ns |  0.206 ns |  0.04 |    0.00 |
|       &#39;Slice &amp; Reverse&#39; | Job-IPVSTG | .NET Framework 4.7.2 |    net472 |     UInt32 |  128 | 1,451.79 ns |  1.657 ns |  1.384 ns |  1.00 |    0.00 |
| ReverseEndiannessHelper | Job-IPVSTG | .NET Framework 4.7.2 |    net472 |     UInt32 |  128 |   387.84 ns |  1.178 ns |  1.102 ns |  0.27 |    0.00 |
|        BinaryPrimitives | Job-IPVSTG | .NET Framework 4.7.2 |    net472 |     UInt32 |  128 |   341.11 ns |  0.561 ns |  0.525 ns |  0.23 |    0.00 |
|                         |            |                      |           |            |      |             |           |           |       |         |
|       &#39;Slice &amp; Reverse&#39; | Job-AYIXQQ |             .NET 6.0 |    net6.0 |     UInt64 |  128 |   463.08 ns |  1.395 ns |  1.237 ns |  0.30 |    0.00 |
| ReverseEndiannessHelper | Job-AYIXQQ |             .NET 6.0 |    net6.0 |     UInt64 |  128 |   224.88 ns |  0.990 ns |  0.926 ns |  0.15 |    0.00 |
|        BinaryPrimitives | Job-AYIXQQ |             .NET 6.0 |    net6.0 |     UInt64 |  128 |    64.25 ns |  0.103 ns |  0.091 ns |  0.04 |    0.00 |
|       &#39;Slice &amp; Reverse&#39; | Job-IPVSTG | .NET Framework 4.7.2 |    net472 |     UInt64 |  128 | 1,546.06 ns |  6.312 ns |  5.905 ns |  1.00 |    0.00 |
| ReverseEndiannessHelper | Job-IPVSTG | .NET Framework 4.7.2 |    net472 |     UInt64 |  128 |   403.69 ns |  1.129 ns |  1.056 ns |  0.26 |    0.00 |
|        BinaryPrimitives | Job-IPVSTG | .NET Framework 4.7.2 |    net472 |     UInt64 |  128 |   368.17 ns |  0.551 ns |  0.515 ns |  0.24 |    0.00 |
|                         |            |                      |           |            |      |             |           |           |       |         |
|       &#39;Slice &amp; Reverse&#39; | Job-AYIXQQ |             .NET 6.0 |    net6.0 |      Int16 |  128 |   286.60 ns |  0.591 ns |  0.524 ns |  0.20 |    0.00 |
| ReverseEndiannessHelper | Job-AYIXQQ |             .NET 6.0 |    net6.0 |      Int16 |  128 |   223.53 ns |  0.353 ns |  0.276 ns |  0.16 |    0.00 |
|        BinaryPrimitives | Job-AYIXQQ |             .NET 6.0 |    net6.0 |      Int16 |  128 |    64.54 ns |  0.199 ns |  0.166 ns |  0.05 |    0.00 |
|       &#39;Slice &amp; Reverse&#39; | Job-IPVSTG | .NET Framework 4.7.2 |    net472 |      Int16 |  128 | 1,398.91 ns |  7.002 ns |  6.550 ns |  1.00 |    0.00 |
| ReverseEndiannessHelper | Job-IPVSTG | .NET Framework 4.7.2 |    net472 |      Int16 |  128 |   419.30 ns |  1.066 ns |  0.945 ns |  0.30 |    0.00 |
|        BinaryPrimitives | Job-IPVSTG | .NET Framework 4.7.2 |    net472 |      Int16 |  128 |   333.80 ns |  0.784 ns |  0.734 ns |  0.24 |    0.00 |
|                         |            |                      |           |            |      |             |           |           |       |         |
|       &#39;Slice &amp; Reverse&#39; | Job-AYIXQQ |             .NET 6.0 |    net6.0 |      Int32 |  128 |   363.55 ns |  1.371 ns |  1.216 ns |  0.25 |    0.00 |
| ReverseEndiannessHelper | Job-AYIXQQ |             .NET 6.0 |    net6.0 |      Int32 |  128 |   192.00 ns |  0.604 ns |  0.504 ns |  0.13 |    0.00 |
|        BinaryPrimitives | Job-AYIXQQ |             .NET 6.0 |    net6.0 |      Int32 |  128 |    64.13 ns |  0.110 ns |  0.092 ns |  0.04 |    0.00 |
|       &#39;Slice &amp; Reverse&#39; | Job-IPVSTG | .NET Framework 4.7.2 |    net472 |      Int32 |  128 | 1,444.16 ns |  2.087 ns |  1.743 ns |  1.00 |    0.00 |
| ReverseEndiannessHelper | Job-IPVSTG | .NET Framework 4.7.2 |    net472 |      Int32 |  128 |   387.07 ns |  1.049 ns |  0.982 ns |  0.27 |    0.00 |
|        BinaryPrimitives | Job-IPVSTG | .NET Framework 4.7.2 |    net472 |      Int32 |  128 |   340.56 ns |  0.728 ns |  0.681 ns |  0.24 |    0.00 |
|                         |            |                      |           |            |      |             |           |           |       |         |
|       &#39;Slice &amp; Reverse&#39; | Job-AYIXQQ |             .NET 6.0 |    net6.0 |      Int64 |  128 |   466.13 ns |  0.450 ns |  0.351 ns |  0.29 |    0.00 |
| ReverseEndiannessHelper | Job-AYIXQQ |             .NET 6.0 |    net6.0 |      Int64 |  128 |   192.63 ns |  0.582 ns |  0.545 ns |  0.12 |    0.00 |
|        BinaryPrimitives | Job-AYIXQQ |             .NET 6.0 |    net6.0 |      Int64 |  128 |    65.62 ns |  1.306 ns |  1.398 ns |  0.04 |    0.00 |
|       &#39;Slice &amp; Reverse&#39; | Job-IPVSTG | .NET Framework 4.7.2 |    net472 |      Int64 |  128 | 1,592.46 ns |  8.456 ns |  7.910 ns |  1.00 |    0.00 |
| ReverseEndiannessHelper | Job-IPVSTG | .NET Framework 4.7.2 |    net472 |      Int64 |  128 |   401.84 ns |  0.512 ns |  0.454 ns |  0.25 |    0.00 |
|        BinaryPrimitives | Job-IPVSTG | .NET Framework 4.7.2 |    net472 |      Int64 |  128 |   373.88 ns |  1.273 ns |  1.191 ns |  0.23 |    0.00 |
|                         |            |                      |           |            |      |             |           |           |       |         |
|       &#39;Slice &amp; Reverse&#39; | Job-AYIXQQ |             .NET 6.0 |    net6.0 |     Single |  128 |   363.47 ns |  1.293 ns |  1.209 ns |  0.25 |    0.00 |
| ReverseEndiannessHelper | Job-AYIXQQ |             .NET 6.0 |    net6.0 |     Single |  128 |   225.70 ns |  0.439 ns |  0.366 ns |  0.16 |    0.00 |
|        BinaryPrimitives | Job-AYIXQQ |             .NET 6.0 |    net6.0 |     Single |  128 |    76.92 ns |  1.297 ns |  1.214 ns |  0.05 |    0.00 |
|       &#39;Slice &amp; Reverse&#39; | Job-IPVSTG | .NET Framework 4.7.2 |    net472 |     Single |  128 | 1,445.38 ns |  4.663 ns |  4.362 ns |  1.00 |    0.00 |
| ReverseEndiannessHelper | Job-IPVSTG | .NET Framework 4.7.2 |    net472 |     Single |  128 | 2,535.72 ns |  8.162 ns |  7.235 ns |  1.75 |    0.01 |
|        BinaryPrimitives | Job-IPVSTG | .NET Framework 4.7.2 |    net472 |     Single |  128 | 2,492.97 ns |  7.282 ns |  6.812 ns |  1.72 |    0.01 |
|                         |            |                      |           |            |      |             |           |           |       |         |
|       &#39;Slice &amp; Reverse&#39; | Job-AYIXQQ |             .NET 6.0 |    net6.0 |     Double |  128 |   468.71 ns |  0.974 ns |  0.813 ns |  0.30 |    0.00 |
| ReverseEndiannessHelper | Job-AYIXQQ |             .NET 6.0 |    net6.0 |     Double |  128 |   225.17 ns |  0.562 ns |  0.469 ns |  0.15 |    0.00 |
|       &#39;Slice &amp; Reverse&#39; | Job-IPVSTG | .NET Framework 4.7.2 |    net472 |     Double |  128 | 1,549.11 ns | 11.902 ns | 11.133 ns |  1.00 |    0.00 |
| ReverseEndiannessHelper | Job-IPVSTG | .NET Framework 4.7.2 |    net472 |     Double |  128 | 2,780.14 ns | 22.747 ns | 20.165 ns |  1.80 |    0.02 |
|                         |            |                      |           |            |      |             |           |           |       |         |
|        BinaryPrimitives | Job-AYIXQQ |             .NET 6.0 |    net6.0 |     Doulbe |  128 |    76.46 ns |  0.237 ns |  0.210 ns |     ? |       ? |
|        BinaryPrimitives | Job-IPVSTG | .NET Framework 4.7.2 |    net472 |     Doulbe |  128 | 2,752.18 ns | 14.237 ns | 12.621 ns |     ? |       ? |
