```

BenchmarkDotNet v0.13.11, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-ZWGTBM : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-ZQLZSO : .NET Framework 4.8.1 (4.8.9181.0), X64 RyuJIT VectorSize=256


```
| Method                  | Runtime            | Categories | Size | Mean      | Error    | StdDev   | Ratio        | RatioSD |
|------------------------ |------------------- |----------- |----- |----------:|---------:|---------:|-------------:|--------:|
| ReverseEndiannessHelper | .NET 8.0           | Double     | 64   |  19.07 ns | 0.124 ns | 0.116 ns | 5.71x faster |   0.03x |
| ReverseEndiannessHelper | .NET Framework 4.8 | Double     | 64   | 109.07 ns | 0.574 ns | 0.480 ns |     baseline |         |
|                         |                    |            |      |           |          |          |              |         |
| BinaryPrimitives        | .NET 8.0           | Double     | 64   |  21.13 ns | 0.433 ns | 0.578 ns | 4.77x faster |   0.13x |
| BinaryPrimitives        | .NET Framework 4.8 | Double     | 64   | 100.84 ns | 0.576 ns | 0.538 ns |     baseline |         |
|                         |                    |            |      |           |          |          |              |         |
| ForeachRef              | .NET 8.0           | Double     | 64   |  31.72 ns | 0.288 ns | 0.269 ns | 3.89x faster |   0.05x |
| ForeachRef              | .NET Framework 4.8 | Double     | 64   | 123.33 ns | 0.737 ns | 0.690 ns |     baseline |         |
|                         |                    |            |      |           |          |          |              |         |
| ReverseEndiannessHelper | .NET 8.0           | Int16      | 64   |  18.15 ns | 0.384 ns | 0.851 ns | 2.40x faster |   0.13x |
| ReverseEndiannessHelper | .NET Framework 4.8 | Int16      | 64   |  43.44 ns | 0.363 ns | 0.339 ns |     baseline |         |
|                         |                    |            |      |           |          |          |              |         |
| BinaryPrimitives        | .NET 8.0           | Int16      | 64   |  21.69 ns | 0.098 ns | 0.091 ns | 1.95x faster |   0.01x |
| BinaryPrimitives        | .NET Framework 4.8 | Int16      | 64   |  42.28 ns | 0.215 ns | 0.201 ns |     baseline |         |
|                         |                    |            |      |           |          |          |              |         |
| ForeachRef              | .NET 8.0           | Int16      | 64   |  17.76 ns | 0.361 ns | 0.355 ns | 2.53x faster |   0.05x |
| ForeachRef              | .NET Framework 4.8 | Int16      | 64   |  45.08 ns | 0.302 ns | 0.252 ns |     baseline |         |
|                         |                    |            |      |           |          |          |              |         |
| ReverseEndiannessHelper | .NET 8.0           | Int32      | 64   |  16.40 ns | 0.338 ns | 0.451 ns | 3.00x faster |   0.09x |
| ReverseEndiannessHelper | .NET Framework 4.8 | Int32      | 64   |  48.97 ns | 0.456 ns | 0.427 ns |     baseline |         |
|                         |                    |            |      |           |          |          |              |         |
| BinaryPrimitives        | .NET 8.0           | Int32      | 64   |  18.04 ns | 0.121 ns | 0.113 ns | 2.32x faster |   0.03x |
| BinaryPrimitives        | .NET Framework 4.8 | Int32      | 64   |  41.82 ns | 0.502 ns | 0.469 ns |     baseline |         |
|                         |                    |            |      |           |          |          |              |         |
| ForeachRef              | .NET 8.0           | Int32      | 64   |  23.60 ns | 0.140 ns | 0.131 ns | 1.86x faster |   0.04x |
| ForeachRef              | .NET Framework 4.8 | Int32      | 64   |  43.95 ns | 0.871 ns | 1.003 ns |     baseline |         |
|                         |                    |            |      |           |          |          |              |         |
| ReverseEndiannessHelper | .NET 8.0           | Int64      | 64   |  17.19 ns | 0.363 ns | 0.664 ns | 3.99x faster |   0.18x |
| ReverseEndiannessHelper | .NET Framework 4.8 | Int64      | 64   |  68.19 ns | 0.518 ns | 0.484 ns |     baseline |         |
|                         |                    |            |      |           |          |          |              |         |
| BinaryPrimitives        | .NET 8.0           | Int64      | 64   |  21.40 ns | 0.142 ns | 0.118 ns | 2.79x faster |   0.03x |
| BinaryPrimitives        | .NET Framework 4.8 | Int64      | 64   |  59.79 ns | 0.483 ns | 0.428 ns |     baseline |         |
|                         |                    |            |      |           |          |          |              |         |
| ForeachRef              | .NET 8.0           | Int64      | 64   |  17.42 ns | 0.013 ns | 0.011 ns | 3.76x faster |   0.01x |
| ForeachRef              | .NET Framework 4.8 | Int64      | 64   |  65.49 ns | 0.216 ns | 0.191 ns |     baseline |         |
|                         |                    |            |      |           |          |          |              |         |
| ReverseEndiannessHelper | .NET 8.0           | Single     | 64   |  22.89 ns | 0.106 ns | 0.100 ns | 3.69x faster |   0.02x |
| ReverseEndiannessHelper | .NET Framework 4.8 | Single     | 64   |  84.46 ns | 0.196 ns | 0.183 ns |     baseline |         |
|                         |                    |            |      |           |          |          |              |         |
| BinaryPrimitives        | .NET 8.0           | Single     | 64   |  16.47 ns | 0.346 ns | 0.371 ns | 4.52x faster |   0.10x |
| BinaryPrimitives        | .NET Framework 4.8 | Single     | 64   |  74.65 ns | 0.416 ns | 0.389 ns |     baseline |         |
|                         |                    |            |      |           |          |          |              |         |
| ForeachRef              | .NET 8.0           | Single     | 64   |  23.76 ns | 0.474 ns | 0.527 ns | 4.07x faster |   0.10x |
| ForeachRef              | .NET Framework 4.8 | Single     | 64   |  96.36 ns | 0.105 ns | 0.098 ns |     baseline |         |
|                         |                    |            |      |           |          |          |              |         |
| ReverseEndiannessHelper | .NET 8.0           | UInt16     | 64   |  26.91 ns | 0.207 ns | 0.194 ns | 1.53x faster |   0.03x |
| ReverseEndiannessHelper | .NET Framework 4.8 | UInt16     | 64   |  41.24 ns | 0.533 ns | 0.499 ns |     baseline |         |
|                         |                    |            |      |           |          |          |              |         |
| BinaryPrimitives        | .NET 8.0           | UInt16     | 64   |  21.17 ns | 0.136 ns | 0.121 ns | 1.43x faster |   0.02x |
| BinaryPrimitives        | .NET Framework 4.8 | UInt16     | 64   |  30.18 ns | 0.409 ns | 0.383 ns |     baseline |         |
|                         |                    |            |      |           |          |          |              |         |
| ForeachRef              | .NET 8.0           | UInt16     | 64   |  17.89 ns | 0.379 ns | 0.405 ns | 2.39x faster |   0.06x |
| ForeachRef              | .NET Framework 4.8 | UInt16     | 64   |  42.94 ns | 0.099 ns | 0.088 ns |     baseline |         |
|                         |                    |            |      |           |          |          |              |         |
| ReverseEndiannessHelper | .NET 8.0           | UInt32     | 64   |  16.60 ns | 0.353 ns | 0.420 ns | 2.75x faster |   0.08x |
| ReverseEndiannessHelper | .NET Framework 4.8 | UInt32     | 64   |  45.87 ns | 0.910 ns | 0.934 ns |     baseline |         |
|                         |                    |            |      |           |          |          |              |         |
| BinaryPrimitives        | .NET 8.0           | UInt32     | 64   |  17.99 ns | 0.136 ns | 0.127 ns | 2.35x faster |   0.02x |
| BinaryPrimitives        | .NET Framework 4.8 | UInt32     | 64   |  42.19 ns | 0.201 ns | 0.188 ns |     baseline |         |
|                         |                    |            |      |           |          |          |              |         |
| ForeachRef              | .NET 8.0           | UInt32     | 64   |  23.62 ns | 0.064 ns | 0.060 ns | 1.86x faster |   0.03x |
| ForeachRef              | .NET Framework 4.8 | UInt32     | 64   |  43.86 ns | 0.752 ns | 0.704 ns |     baseline |         |
|                         |                    |            |      |           |          |          |              |         |
| ReverseEndiannessHelper | .NET 8.0           | UInt64     | 64   |  27.47 ns | 0.205 ns | 0.160 ns | 2.45x faster |   0.02x |
| ReverseEndiannessHelper | .NET Framework 4.8 | UInt64     | 64   |  67.38 ns | 0.262 ns | 0.205 ns |     baseline |         |
|                         |                    |            |      |           |          |          |              |         |
| BinaryPrimitives        | .NET 8.0           | UInt64     | 64   |  21.38 ns | 0.146 ns | 0.137 ns | 2.77x faster |   0.03x |
| BinaryPrimitives        | .NET Framework 4.8 | UInt64     | 64   |  59.24 ns | 0.489 ns | 0.457 ns |     baseline |         |
|                         |                    |            |      |           |          |          |              |         |
| ForeachRef              | .NET 8.0           | UInt64     | 64   |  17.52 ns | 0.090 ns | 0.075 ns | 3.74x faster |   0.02x |
| ForeachRef              | .NET Framework 4.8 | UInt64     | 64   |  65.49 ns | 0.274 ns | 0.257 ns |     baseline |         |
