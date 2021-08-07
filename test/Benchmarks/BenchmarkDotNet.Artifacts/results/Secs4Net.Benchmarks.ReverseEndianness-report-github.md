``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1151 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.6.21355.2
  [Host]     : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-GAZESS : .NET 6.0.0 (6.0.21.35212), X64 RyuJIT
  Job-HKKFQD : .NET Framework 4.8 (4.8.4400.0), X64 RyuJIT


```
|                   Method |              Runtime | Categories | Size |      Mean |    Error |   StdDev |         Ratio | RatioSD | Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------------- |--------------------- |----------- |----- |----------:|---------:|---------:|--------------:|--------:|------:|------:|------:|----------:|
|        &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     UInt16 |   64 | 147.37 ns | 0.798 ns | 0.708 ns |  4.70x faster |   0.02x |     - |     - |     - |         - |
|  ReverseEndiannessHelper |             .NET 6.0 |     UInt16 |   64 | 100.72 ns | 0.443 ns | 0.414 ns |  6.87x faster |   0.03x |     - |     - |     - |         - |
|         BinaryPrimitives |             .NET 6.0 |     UInt16 |   64 |  33.29 ns | 0.075 ns | 0.067 ns | 20.79x faster |   0.06x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) |             .NET 6.0 |     UInt16 |   64 |  26.34 ns | 0.068 ns | 0.057 ns | 26.27x faster |   0.07x |     - |     - |     - |         - |
|        &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     UInt16 |   64 | 691.94 ns | 1.338 ns | 1.252 ns |      baseline |         |     - |     - |     - |         - |
|  ReverseEndiannessHelper | .NET Framework 4.7.2 |     UInt16 |   64 | 126.58 ns | 0.224 ns | 0.199 ns |  5.47x faster |   0.01x |     - |     - |     - |         - |
|         BinaryPrimitives | .NET Framework 4.7.2 |     UInt16 |   64 | 173.45 ns | 0.255 ns | 0.238 ns |  3.99x faster |   0.01x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) | .NET Framework 4.7.2 |     UInt16 |   64 |  50.27 ns | 0.174 ns | 0.163 ns | 13.77x faster |   0.06x |     - |     - |     - |         - |
|                          |                      |            |      |           |          |          |               |         |       |       |       |           |
|        &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     UInt32 |   64 | 182.94 ns | 0.980 ns | 0.916 ns |  4.00x faster |   0.03x |     - |     - |     - |         - |
|  ReverseEndiannessHelper |             .NET 6.0 |     UInt32 |   64 | 101.31 ns | 0.214 ns | 0.201 ns |  7.22x faster |   0.03x |     - |     - |     - |         - |
|         BinaryPrimitives |             .NET 6.0 |     UInt32 |   64 |  33.14 ns | 0.082 ns | 0.077 ns | 22.07x faster |   0.06x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) |             .NET 6.0 |     UInt32 |   64 |  26.15 ns | 0.068 ns | 0.064 ns | 27.96x faster |   0.10x |     - |     - |     - |         - |
|        &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     UInt32 |   64 | 731.31 ns | 2.156 ns | 2.017 ns |      baseline |         |     - |     - |     - |         - |
|  ReverseEndiannessHelper | .NET Framework 4.7.2 |     UInt32 |   64 | 126.69 ns | 0.256 ns | 0.239 ns |  5.77x faster |   0.02x |     - |     - |     - |         - |
|         BinaryPrimitives | .NET Framework 4.7.2 |     UInt32 |   64 | 142.86 ns | 1.000 ns | 0.935 ns |  5.12x faster |   0.04x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) | .NET Framework 4.7.2 |     UInt32 |   64 |  63.38 ns | 0.195 ns | 0.183 ns | 11.54x faster |   0.05x |     - |     - |     - |         - |
|                          |                      |            |      |           |          |          |               |         |       |       |       |           |
|        &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     UInt64 |   64 | 233.18 ns | 1.074 ns | 0.952 ns |  3.39x faster |   0.02x |     - |     - |     - |         - |
|  ReverseEndiannessHelper |             .NET 6.0 |     UInt64 |   64 | 100.75 ns | 0.135 ns | 0.126 ns |  7.85x faster |   0.01x |     - |     - |     - |         - |
|         BinaryPrimitives |             .NET 6.0 |     UInt64 |   64 |  33.19 ns | 0.079 ns | 0.074 ns | 23.82x faster |   0.05x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) |             .NET 6.0 |     UInt64 |   64 |  26.09 ns | 0.076 ns | 0.067 ns | 30.30x faster |   0.10x |     - |     - |     - |         - |
|        &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     UInt64 |   64 | 790.48 ns | 1.315 ns | 1.230 ns |      baseline |         |     - |     - |     - |         - |
|  ReverseEndiannessHelper | .NET Framework 4.7.2 |     UInt64 |   64 | 129.36 ns | 0.640 ns | 0.599 ns |  6.11x faster |   0.03x |     - |     - |     - |         - |
|         BinaryPrimitives | .NET Framework 4.7.2 |     UInt64 |   64 | 193.03 ns | 0.788 ns | 0.737 ns |  4.10x faster |   0.02x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) | .NET Framework 4.7.2 |     UInt64 |   64 |  94.81 ns | 0.210 ns | 0.186 ns |  8.34x faster |   0.02x |     - |     - |     - |         - |
|                          |                      |            |      |           |          |          |               |         |       |       |       |           |
|        &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |      Int16 |   64 | 147.69 ns | 1.090 ns | 1.020 ns |  4.68x faster |   0.03x |     - |     - |     - |         - |
|  ReverseEndiannessHelper |             .NET 6.0 |      Int16 |   64 | 101.76 ns | 0.122 ns | 0.108 ns |  6.79x faster |   0.01x |     - |     - |     - |         - |
|         BinaryPrimitives |             .NET 6.0 |      Int16 |   64 |  33.35 ns | 0.226 ns | 0.188 ns | 20.71x faster |   0.12x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) |             .NET 6.0 |      Int16 |   64 |  34.88 ns | 0.047 ns | 0.042 ns | 19.80x faster |   0.05x |     - |     - |     - |         - |
|        &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |      Int16 |   64 | 690.55 ns | 1.405 ns | 1.314 ns |      baseline |         |     - |     - |     - |         - |
|  ReverseEndiannessHelper | .NET Framework 4.7.2 |      Int16 |   64 | 127.38 ns | 0.433 ns | 0.405 ns |  5.42x faster |   0.02x |     - |     - |     - |         - |
|         BinaryPrimitives | .NET Framework 4.7.2 |      Int16 |   64 | 173.48 ns | 0.669 ns | 0.626 ns |  3.98x faster |   0.01x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) | .NET Framework 4.7.2 |      Int16 |   64 |  63.77 ns | 0.250 ns | 0.234 ns | 10.83x faster |   0.05x |     - |     - |     - |         - |
|                          |                      |            |      |           |          |          |               |         |       |       |       |           |
|        &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |      Int32 |   64 | 182.74 ns | 0.879 ns | 0.822 ns |  4.01x faster |   0.02x |     - |     - |     - |         - |
|  ReverseEndiannessHelper |             .NET 6.0 |      Int32 |   64 | 114.07 ns | 0.195 ns | 0.183 ns |  6.42x faster |   0.01x |     - |     - |     - |         - |
|         BinaryPrimitives |             .NET 6.0 |      Int32 |   64 |  33.20 ns | 0.090 ns | 0.084 ns | 22.05x faster |   0.08x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) |             .NET 6.0 |      Int32 |   64 |  26.20 ns | 0.126 ns | 0.118 ns | 27.94x faster |   0.12x |     - |     - |     - |         - |
|        &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |      Int32 |   64 | 731.96 ns | 1.592 ns | 1.489 ns |      baseline |         |     - |     - |     - |         - |
|  ReverseEndiannessHelper | .NET Framework 4.7.2 |      Int32 |   64 | 113.65 ns | 0.216 ns | 0.202 ns |  6.44x faster |   0.01x |     - |     - |     - |         - |
|         BinaryPrimitives | .NET Framework 4.7.2 |      Int32 |   64 | 175.47 ns | 0.932 ns | 0.872 ns |  4.17x faster |   0.02x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) | .NET Framework 4.7.2 |      Int32 |   64 |  55.78 ns | 0.271 ns | 0.254 ns | 13.12x faster |   0.07x |     - |     - |     - |         - |
|                          |                      |            |      |           |          |          |               |         |       |       |       |           |
|        &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |      Int64 |   64 | 236.02 ns | 1.214 ns | 1.136 ns |  3.35x faster |   0.02x |     - |     - |     - |         - |
|  ReverseEndiannessHelper |             .NET 6.0 |      Int64 |   64 | 100.03 ns | 0.201 ns | 0.168 ns |  7.90x faster |   0.02x |     - |     - |     - |         - |
|         BinaryPrimitives |             .NET 6.0 |      Int64 |   64 |  33.26 ns | 0.115 ns | 0.102 ns | 23.77x faster |   0.07x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) |             .NET 6.0 |      Int64 |   64 |  26.05 ns | 0.109 ns | 0.091 ns | 30.34x faster |   0.12x |     - |     - |     - |         - |
|        &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |      Int64 |   64 | 790.46 ns | 0.960 ns | 0.898 ns |      baseline |         |     - |     - |     - |         - |
|  ReverseEndiannessHelper | .NET Framework 4.7.2 |      Int64 |   64 | 128.74 ns | 0.387 ns | 0.343 ns |  6.14x faster |   0.02x |     - |     - |     - |         - |
|         BinaryPrimitives | .NET Framework 4.7.2 |      Int64 |   64 | 194.44 ns | 0.792 ns | 0.740 ns |  4.07x faster |   0.02x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) | .NET Framework 4.7.2 |      Int64 |   64 |  94.85 ns | 0.206 ns | 0.182 ns |  8.33x faster |   0.02x |     - |     - |     - |         - |
|                          |                      |            |      |           |          |          |               |         |       |       |       |           |
|        &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     Single |   64 | 183.66 ns | 1.731 ns | 1.619 ns |  3.98x faster |   0.03x |     - |     - |     - |         - |
|  ReverseEndiannessHelper |             .NET 6.0 |     Single |   64 | 115.90 ns | 0.314 ns | 0.279 ns |  6.31x faster |   0.02x |     - |     - |     - |         - |
|         BinaryPrimitives |             .NET 6.0 |     Single |   64 |  38.40 ns | 0.170 ns | 0.159 ns | 19.04x faster |   0.07x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) |             .NET 6.0 |     Single |   64 |  30.74 ns | 0.085 ns | 0.080 ns | 23.79x faster |   0.08x |     - |     - |     - |         - |
|        &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     Single |   64 | 731.32 ns | 1.215 ns | 1.137 ns |      baseline |         |     - |     - |     - |         - |
|  ReverseEndiannessHelper | .NET Framework 4.7.2 |     Single |   64 | 561.92 ns | 1.219 ns | 1.081 ns |  1.30x faster |   0.00x |     - |     - |     - |         - |
|         BinaryPrimitives | .NET Framework 4.7.2 |     Single |   64 | 220.14 ns | 0.467 ns | 0.414 ns |  3.32x faster |   0.01x |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) | .NET Framework 4.7.2 |     Single |   64 | 134.11 ns | 0.587 ns | 0.520 ns |  5.45x faster |   0.03x |     - |     - |     - |         - |
|                          |                      |            |      |           |          |          |               |         |       |       |       |           |
|        &#39;Slice &amp; Reverse&#39; |             .NET 6.0 |     Double |   64 | 232.83 ns | 1.184 ns | 1.108 ns |  3.39x faster |   0.02x |     - |     - |     - |         - |
|  ReverseEndiannessHelper |             .NET 6.0 |     Double |   64 | 115.66 ns | 0.168 ns | 0.157 ns |  6.83x faster |   0.01x |     - |     - |     - |         - |
|        &#39;Slice &amp; Reverse&#39; | .NET Framework 4.7.2 |     Double |   64 | 789.84 ns | 1.057 ns | 0.937 ns |      baseline |         |     - |     - |     - |         - |
|  ReverseEndiannessHelper | .NET Framework 4.7.2 |     Double |   64 | 624.35 ns | 1.332 ns | 1.112 ns |  1.27x faster |   0.00x |     - |     - |     - |         - |
|                          |                      |            |      |           |          |          |               |         |       |       |       |           |
|         BinaryPrimitives |             .NET 6.0 |     Doulbe |   64 |  38.41 ns | 0.120 ns | 0.112 ns |             ? |       ? |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) |             .NET 6.0 |     Doulbe |   64 |  34.70 ns | 0.282 ns | 0.263 ns |             ? |       ? |     - |     - |     - |         - |
|         BinaryPrimitives | .NET Framework 4.7.2 |     Doulbe |   64 | 239.60 ns | 0.502 ns | 0.445 ns |             ? |       ? |     - |     - |     - |         - |
| BinaryPrimitives(Unsafe) | .NET Framework 4.7.2 |     Doulbe |   64 | 161.51 ns | 0.665 ns | 0.555 ns |             ? |       ? |     - |     - |     - |         - |
