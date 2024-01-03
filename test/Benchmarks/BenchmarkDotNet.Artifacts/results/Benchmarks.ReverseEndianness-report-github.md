```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-LLCSMO : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-LSZLMA : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method                  | Runtime  | Categories | Size | Mean     | Error    | StdDev   | Median   | Ratio        | RatioSD |
|------------------------ |--------- |----------- |----- |---------:|---------:|---------:|---------:|-------------:|--------:|
| ReverseEndiannessHelper | .NET 6.0 | Double     | 64   | 19.94 ns | 0.300 ns | 0.266 ns | 19.97 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | Double     | 64   | 20.03 ns | 0.384 ns | 0.341 ns | 20.13 ns | 1.00x slower |   0.02x |
|                         |          |            |      |          |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | Double     | 64   | 31.78 ns | 0.636 ns | 0.849 ns | 31.67 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | Double     | 64   | 29.60 ns | 0.599 ns | 0.713 ns | 29.58 ns | 1.08x faster |   0.04x |
|                         |          |            |      |          |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | Double     | 64   | 24.48 ns | 0.273 ns | 0.255 ns | 24.37 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | Double     | 64   | 18.94 ns | 0.340 ns | 0.318 ns | 18.90 ns | 1.29x faster |   0.03x |
|                         |          |            |      |          |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | Int16      | 64   | 18.54 ns | 0.293 ns | 0.260 ns | 18.48 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | Int16      | 64   | 25.57 ns | 0.364 ns | 0.341 ns | 25.57 ns | 1.38x slower |   0.02x |
|                         |          |            |      |          |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | Int16      | 64   | 16.71 ns | 0.353 ns | 0.550 ns | 16.62 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | Int16      | 64   | 17.01 ns | 0.336 ns | 0.315 ns | 16.95 ns | 1.02x slower |   0.03x |
|                         |          |            |      |          |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | Int16      | 64   | 27.97 ns | 0.204 ns | 0.181 ns | 28.03 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | Int16      | 64   | 23.66 ns | 0.381 ns | 0.357 ns | 23.60 ns | 1.18x faster |   0.01x |
|                         |          |            |      |          |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | Int32      | 64   | 20.39 ns | 0.201 ns | 0.179 ns | 20.42 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | Int32      | 64   | 18.27 ns | 0.374 ns | 0.349 ns | 18.31 ns | 1.11x faster |   0.02x |
|                         |          |            |      |          |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | Int32      | 64   | 18.81 ns | 0.239 ns | 0.223 ns | 18.83 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | Int32      | 64   | 15.63 ns | 0.323 ns | 0.332 ns | 15.57 ns | 1.20x faster |   0.03x |
|                         |          |            |      |          |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | Int32      | 64   | 24.51 ns | 0.257 ns | 0.228 ns | 24.49 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | Int32      | 64   | 17.53 ns | 0.371 ns | 0.364 ns | 17.51 ns | 1.40x faster |   0.03x |
|                         |          |            |      |          |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | Int64      | 64   | 20.39 ns | 0.430 ns | 0.402 ns | 20.41 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | Int64      | 64   | 29.24 ns | 0.610 ns | 0.571 ns | 29.14 ns | 1.43x slower |   0.04x |
|                         |          |            |      |          |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | Int64      | 64   | 22.38 ns | 0.417 ns | 0.390 ns | 22.39 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | Int64      | 64   | 16.73 ns | 0.276 ns | 0.258 ns | 16.77 ns | 1.34x faster |   0.04x |
|                         |          |            |      |          |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | Int64      | 64   | 25.88 ns | 0.239 ns | 0.223 ns | 25.93 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | Int64      | 64   | 20.21 ns | 0.386 ns | 0.361 ns | 20.25 ns | 1.28x faster |   0.02x |
|                         |          |            |      |          |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | Single     | 64   | 19.09 ns | 0.366 ns | 0.342 ns | 19.15 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | Single     | 64   | 33.25 ns | 0.676 ns | 1.335 ns | 33.90 ns | 1.72x slower |   0.08x |
|                         |          |            |      |          |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | Single     | 64   | 16.32 ns | 0.257 ns | 0.227 ns | 16.36 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | Single     | 64   | 32.20 ns | 0.659 ns | 1.120 ns | 32.79 ns | 1.98x slower |   0.09x |
|                         |          |            |      |          |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | Single     | 64   | 40.47 ns | 0.827 ns | 2.045 ns | 40.72 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | Single     | 64   | 36.64 ns | 0.757 ns | 0.744 ns | 36.94 ns | 1.12x faster |   0.07x |
|                         |          |            |      |          |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | UInt16     | 64   | 31.20 ns | 0.645 ns | 1.041 ns | 31.43 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | UInt16     | 64   | 18.72 ns | 0.392 ns | 0.481 ns | 18.72 ns | 1.66x faster |   0.08x |
|                         |          |            |      |          |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | UInt16     | 64   | 21.84 ns | 0.196 ns | 0.417 ns | 21.82 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | UInt16     | 64   | 17.07 ns | 0.351 ns | 0.328 ns | 17.07 ns | 1.28x faster |   0.05x |
|                         |          |            |      |          |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | UInt16     | 64   | 27.79 ns | 0.261 ns | 0.244 ns | 27.84 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | UInt16     | 64   | 23.62 ns | 0.167 ns | 0.148 ns | 23.64 ns | 1.18x faster |   0.01x |
|                         |          |            |      |          |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | UInt32     | 64   | 17.76 ns | 0.311 ns | 0.275 ns | 17.80 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | UInt32     | 64   | 18.86 ns | 0.378 ns | 0.389 ns | 18.94 ns | 1.07x slower |   0.03x |
|                         |          |            |      |          |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | UInt32     | 64   | 15.53 ns | 0.332 ns | 0.326 ns | 15.53 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | UInt32     | 64   | 15.68 ns | 0.260 ns | 0.243 ns | 15.74 ns | 1.01x slower |   0.03x |
|                         |          |            |      |          |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | UInt32     | 64   | 23.83 ns | 0.241 ns | 0.214 ns | 23.88 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | UInt32     | 64   | 17.97 ns | 0.380 ns | 0.423 ns | 17.95 ns | 1.32x faster |   0.04x |
|                         |          |            |      |          |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | UInt64     | 64   | 23.62 ns | 0.272 ns | 0.254 ns | 23.65 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | UInt64     | 64   | 19.79 ns | 0.255 ns | 0.226 ns | 19.78 ns | 1.19x faster |   0.02x |
|                         |          |            |      |          |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | UInt64     | 64   | 22.44 ns | 0.314 ns | 0.294 ns | 22.45 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | UInt64     | 64   | 16.34 ns | 0.340 ns | 0.465 ns | 16.31 ns | 1.38x faster |   0.06x |
|                         |          |            |      |          |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | UInt64     | 64   | 26.79 ns | 0.364 ns | 0.341 ns | 26.83 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | UInt64     | 64   | 21.81 ns | 0.461 ns | 0.549 ns | 21.77 ns | 1.23x faster |   0.04x |
