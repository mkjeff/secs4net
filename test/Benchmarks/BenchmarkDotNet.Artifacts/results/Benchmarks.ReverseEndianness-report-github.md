```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-OOEVDF : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-AKZPHK : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method                  | Runtime  | Categories | Size | Mean     | Error    | StdDev   | Ratio        | RatioSD |
|------------------------ |--------- |----------- |----- |---------:|---------:|---------:|-------------:|--------:|
| ReverseEndiannessHelper | .NET 6.0 | Double     | 64   | 19.98 ns | 0.245 ns | 0.229 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | Double     | 64   | 20.09 ns | 0.316 ns | 0.280 ns | 1.01x slower |   0.02x |
|                         |          |            |      |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | Double     | 64   | 18.87 ns | 0.179 ns | 0.149 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | Double     | 64   | 21.14 ns | 0.258 ns | 0.229 ns | 1.12x slower |   0.01x |
|                         |          |            |      |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | Double     | 64   | 38.59 ns | 0.680 ns | 0.636 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | Double     | 64   | 30.22 ns | 0.619 ns | 0.760 ns | 1.28x faster |   0.04x |
|                         |          |            |      |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | Int16      | 64   | 23.79 ns | 0.286 ns | 0.254 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | Int16      | 64   | 18.76 ns | 0.397 ns | 0.390 ns | 1.27x faster |   0.03x |
|                         |          |            |      |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | Int16      | 64   | 16.13 ns | 0.343 ns | 0.592 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | Int16      | 64   | 22.64 ns | 0.320 ns | 0.299 ns | 1.40x slower |   0.05x |
|                         |          |            |      |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | Int16      | 64   | 28.17 ns | 0.209 ns | 0.186 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | Int16      | 64   | 18.74 ns | 0.381 ns | 0.392 ns | 1.50x faster |   0.03x |
|                         |          |            |      |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | Int32      | 64   | 17.82 ns | 0.379 ns | 0.355 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | Int32      | 64   | 17.45 ns | 0.357 ns | 0.334 ns | 1.02x faster |   0.02x |
|                         |          |            |      |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | Int32      | 64   | 18.80 ns | 0.325 ns | 0.304 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | Int32      | 64   | 18.87 ns | 0.255 ns | 0.239 ns | 1.00x slower |   0.02x |
|                         |          |            |      |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | Int32      | 64   | 23.60 ns | 0.383 ns | 0.340 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | Int32      | 64   | 24.64 ns | 0.271 ns | 0.253 ns | 1.04x slower |   0.01x |
|                         |          |            |      |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | Int64      | 64   | 23.60 ns | 0.345 ns | 0.323 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | Int64      | 64   | 19.37 ns | 0.412 ns | 0.405 ns | 1.22x faster |   0.03x |
|                         |          |            |      |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | Int64      | 64   | 22.41 ns | 0.269 ns | 0.252 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | Int64      | 64   | 22.28 ns | 0.163 ns | 0.144 ns | 1.01x faster |   0.01x |
|                         |          |            |      |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | Int64      | 64   | 27.05 ns | 0.438 ns | 0.410 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | Int64      | 64   | 18.31 ns | 0.338 ns | 0.316 ns | 1.48x faster |   0.02x |
|                         |          |            |      |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | Single     | 64   | 23.86 ns | 0.268 ns | 0.250 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | Single     | 64   | 23.93 ns | 0.273 ns | 0.242 ns | 1.00x slower |   0.01x |
|                         |          |            |      |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | Single     | 64   | 16.75 ns | 0.358 ns | 0.397 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | Single     | 64   | 17.33 ns | 0.371 ns | 0.380 ns | 1.04x slower |   0.04x |
|                         |          |            |      |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | Single     | 64   | 26.99 ns | 0.398 ns | 0.372 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | Single     | 64   | 23.58 ns | 0.264 ns | 0.247 ns | 1.14x faster |   0.02x |
|                         |          |            |      |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | UInt16     | 64   | 19.66 ns | 0.414 ns | 0.387 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | UInt16     | 64   | 28.37 ns | 0.439 ns | 0.410 ns | 1.44x slower |   0.03x |
|                         |          |            |      |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | UInt16     | 64   | 21.90 ns | 0.266 ns | 0.248 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | UInt16     | 64   | 22.18 ns | 0.353 ns | 0.330 ns | 1.01x slower |   0.02x |
|                         |          |            |      |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | UInt16     | 64   | 27.78 ns | 0.146 ns | 0.122 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | UInt16     | 64   | 20.43 ns | 0.422 ns | 0.469 ns | 1.36x faster |   0.03x |
|                         |          |            |      |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | UInt32     | 64   | 20.67 ns | 0.399 ns | 0.373 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | UInt32     | 64   | 17.51 ns | 0.242 ns | 0.214 ns | 1.18x faster |   0.02x |
|                         |          |            |      |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | UInt32     | 64   | 15.43 ns | 0.314 ns | 0.430 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | UInt32     | 64   | 18.74 ns | 0.310 ns | 0.290 ns | 1.22x slower |   0.05x |
|                         |          |            |      |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | UInt32     | 64   | 31.38 ns | 0.529 ns | 0.495 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | UInt32     | 64   | 24.52 ns | 0.265 ns | 0.248 ns | 1.28x faster |   0.02x |
|                         |          |            |      |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | UInt64     | 64   | 19.18 ns | 0.361 ns | 0.320 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | UInt64     | 64   | 28.41 ns | 0.381 ns | 0.357 ns | 1.48x slower |   0.03x |
|                         |          |            |      |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | UInt64     | 64   | 22.41 ns | 0.223 ns | 0.209 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | UInt64     | 64   | 22.34 ns | 0.236 ns | 0.221 ns | 1.00x faster |   0.01x |
|                         |          |            |      |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | UInt64     | 64   | 26.94 ns | 0.275 ns | 0.257 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | UInt64     | 64   | 18.48 ns | 0.348 ns | 0.326 ns | 1.46x faster |   0.03x |
