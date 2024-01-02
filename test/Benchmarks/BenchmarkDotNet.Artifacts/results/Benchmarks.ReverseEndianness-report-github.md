```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-ZQSTYQ : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-LEHGCO : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method                  | Runtime  | Categories | Size | Mean     | Error    | StdDev   | Ratio        | RatioSD |
|------------------------ |--------- |----------- |----- |---------:|---------:|---------:|-------------:|--------:|
| ReverseEndiannessHelper | .NET 6.0 | Double     | 64   | 19.01 ns | 0.120 ns | 0.100 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | Double     | 64   | 19.01 ns | 0.061 ns | 0.057 ns | 1.00x faster |   0.01x |
|                         |          |            |      |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | Double     | 64   | 37.41 ns | 0.203 ns | 0.190 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | Double     | 64   | 28.45 ns | 0.409 ns | 0.382 ns | 1.32x faster |   0.02x |
|                         |          |            |      |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | Doulbe     | 64   | 23.38 ns | 0.196 ns | 0.184 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | Doulbe     | 64   | 17.92 ns | 0.105 ns | 0.093 ns | 1.31x faster |   0.01x |
|                         |          |            |      |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | Int16      | 64   | 17.87 ns | 0.265 ns | 0.221 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | Int16      | 64   | 26.74 ns | 0.156 ns | 0.146 ns | 1.50x slower |   0.02x |
|                         |          |            |      |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | Int16      | 64   | 15.48 ns | 0.323 ns | 0.907 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | Int16      | 64   | 15.89 ns | 0.341 ns | 0.777 ns | 1.02x slower |   0.08x |
|                         |          |            |      |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | Int16      | 64   | 26.95 ns | 0.352 ns | 0.329 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | Int16      | 64   | 22.55 ns | 0.147 ns | 0.137 ns | 1.20x faster |   0.01x |
|                         |          |            |      |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | Int32      | 64   | 19.61 ns | 0.051 ns | 0.048 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | Int32      | 64   | 18.04 ns | 0.379 ns | 0.774 ns | 1.09x faster |   0.05x |
|                         |          |            |      |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | Int32      | 64   | 17.77 ns | 0.301 ns | 0.282 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | Int32      | 64   | 14.59 ns | 0.119 ns | 0.105 ns | 1.22x faster |   0.02x |
|                         |          |            |      |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | Int32      | 64   | 30.15 ns | 0.188 ns | 0.175 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | Int32      | 64   | 15.97 ns | 0.283 ns | 0.265 ns | 1.89x faster |   0.03x |
|                         |          |            |      |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | Int64      | 64   | 19.07 ns | 0.326 ns | 0.305 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | Int64      | 64   | 27.10 ns | 0.017 ns | 0.015 ns | 1.42x slower |   0.02x |
|                         |          |            |      |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | Int64      | 64   | 21.43 ns | 0.139 ns | 0.130 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | Int64      | 64   | 15.30 ns | 0.323 ns | 0.303 ns | 1.40x faster |   0.03x |
|                         |          |            |      |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | Int64      | 64   | 24.63 ns | 0.262 ns | 0.245 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | Int64      | 64   | 19.30 ns | 0.253 ns | 0.237 ns | 1.28x faster |   0.01x |
|                         |          |            |      |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | Single     | 64   | 18.18 ns | 0.354 ns | 0.295 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | Single     | 64   | 18.74 ns | 0.399 ns | 0.427 ns | 1.03x slower |   0.04x |
|                         |          |            |      |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | Single     | 64   | 15.34 ns | 0.035 ns | 0.031 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | Single     | 64   | 20.88 ns | 0.142 ns | 0.133 ns | 1.36x slower |   0.01x |
|                         |          |            |      |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | Single     | 64   | 25.61 ns | 0.112 ns | 0.105 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | Single     | 64   | 20.09 ns | 0.074 ns | 0.062 ns | 1.27x faster |   0.01x |
|                         |          |            |      |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | UInt16     | 64   | 22.13 ns | 0.148 ns | 0.139 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | UInt16     | 64   | 17.39 ns | 0.363 ns | 1.007 ns | 1.28x faster |   0.07x |
|                         |          |            |      |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | UInt16     | 64   | 20.70 ns | 0.421 ns | 0.468 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | UInt16     | 64   | 15.80 ns | 0.335 ns | 0.560 ns | 1.31x faster |   0.07x |
|                         |          |            |      |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | UInt16     | 64   | 27.99 ns | 0.157 ns | 0.139 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | UInt16     | 64   | 22.70 ns | 0.159 ns | 0.149 ns | 1.23x faster |   0.01x |
|                         |          |            |      |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | UInt32     | 64   | 16.82 ns | 0.239 ns | 0.223 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | UInt32     | 64   | 18.29 ns | 0.390 ns | 0.769 ns | 1.08x slower |   0.05x |
|                         |          |            |      |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | UInt32     | 64   | 14.05 ns | 0.172 ns | 0.143 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | UInt32     | 64   | 15.09 ns | 0.325 ns | 0.361 ns | 1.08x slower |   0.03x |
|                         |          |            |      |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | UInt32     | 64   | 21.61 ns | 0.147 ns | 0.122 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | UInt32     | 64   | 16.74 ns | 0.353 ns | 0.406 ns | 1.29x faster |   0.03x |
|                         |          |            |      |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | UInt64     | 64   | 21.43 ns | 0.450 ns | 1.201 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | UInt64     | 64   | 18.65 ns | 0.396 ns | 0.800 ns | 1.15x faster |   0.08x |
|                         |          |            |      |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | UInt64     | 64   | 21.47 ns | 0.140 ns | 0.131 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | UInt64     | 64   | 15.01 ns | 0.199 ns | 0.186 ns | 1.43x faster |   0.02x |
|                         |          |            |      |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | UInt64     | 64   | 24.74 ns | 0.047 ns | 0.044 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | UInt64     | 64   | 19.28 ns | 0.300 ns | 0.266 ns | 1.28x faster |   0.02x |
