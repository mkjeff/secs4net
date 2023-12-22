```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2792/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  Job-VAUECX : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  Job-NTIFBX : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method                  | Runtime  | Categories | Size | Mean     | Error    | StdDev   | Ratio        | RatioSD |
|------------------------ |--------- |----------- |----- |---------:|---------:|---------:|-------------:|--------:|
| ReverseEndiannessHelper | .NET 6.0 | Double     | 64   | 20.07 ns | 0.190 ns | 0.168 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | Double     | 64   | 20.05 ns | 0.176 ns | 0.156 ns | 1.00x faster |   0.01x |
|                         |          |            |      |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | Double     | 64   | 38.47 ns | 0.275 ns | 0.244 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | Double     | 64   | 30.16 ns | 0.457 ns | 0.427 ns | 1.28x faster |   0.02x |
|                         |          |            |      |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | Doulbe     | 64   | 24.35 ns | 0.143 ns | 0.120 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | Doulbe     | 64   | 18.74 ns | 0.083 ns | 0.073 ns | 1.30x faster |   0.01x |
|                         |          |            |      |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | Int16      | 64   | 18.99 ns | 0.387 ns | 0.431 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | Int16      | 64   | 27.36 ns | 0.111 ns | 0.093 ns | 1.44x slower |   0.03x |
|                         |          |            |      |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | Int16      | 64   | 16.59 ns | 0.350 ns | 0.666 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | Int16      | 64   | 16.64 ns | 0.329 ns | 0.308 ns | 1.01x slower |   0.05x |
|                         |          |            |      |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | Int16      | 64   | 28.92 ns | 0.448 ns | 0.419 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | Int16      | 64   | 23.63 ns | 0.221 ns | 0.196 ns | 1.22x faster |   0.02x |
|                         |          |            |      |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | Int32      | 64   | 20.52 ns | 0.240 ns | 0.224 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | Int32      | 64   | 18.04 ns | 0.288 ns | 0.269 ns | 1.14x faster |   0.02x |
|                         |          |            |      |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | Int32      | 64   | 18.68 ns | 0.100 ns | 0.088 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | Int32      | 64   | 15.34 ns | 0.211 ns | 0.187 ns | 1.22x faster |   0.02x |
|                         |          |            |      |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | Int32      | 64   | 24.65 ns | 0.338 ns | 0.316 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | Int32      | 64   | 16.79 ns | 0.307 ns | 0.287 ns | 1.47x faster |   0.03x |
|                         |          |            |      |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | Int64      | 64   | 20.19 ns | 0.391 ns | 0.346 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | Int64      | 64   | 28.04 ns | 0.130 ns | 0.115 ns | 1.39x slower |   0.02x |
|                         |          |            |      |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | Int64      | 64   | 22.17 ns | 0.353 ns | 0.330 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | Int64      | 64   | 15.80 ns | 0.337 ns | 0.361 ns | 1.41x faster |   0.04x |
|                         |          |            |      |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | Int64      | 64   | 26.64 ns | 0.207 ns | 0.184 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | Int64      | 64   | 19.22 ns | 0.205 ns | 0.182 ns | 1.39x faster |   0.02x |
|                         |          |            |      |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | Single     | 64   | 18.85 ns | 0.329 ns | 0.307 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | Single     | 64   | 19.02 ns | 0.307 ns | 0.272 ns | 1.01x slower |   0.02x |
|                         |          |            |      |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | Single     | 64   | 15.91 ns | 0.169 ns | 0.141 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | Single     | 64   | 21.48 ns | 0.238 ns | 0.211 ns | 1.35x slower |   0.01x |
|                         |          |            |      |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | Single     | 64   | 26.70 ns | 0.192 ns | 0.180 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | Single     | 64   | 20.96 ns | 0.163 ns | 0.145 ns | 1.27x faster |   0.01x |
|                         |          |            |      |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | UInt16     | 64   | 23.03 ns | 0.169 ns | 0.150 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | UInt16     | 64   | 28.10 ns | 0.193 ns | 0.171 ns | 1.22x slower |   0.01x |
|                         |          |            |      |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | UInt16     | 64   | 21.31 ns | 0.390 ns | 0.365 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | UInt16     | 64   | 16.78 ns | 0.267 ns | 0.237 ns | 1.27x faster |   0.03x |
|                         |          |            |      |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | UInt16     | 64   | 28.96 ns | 0.368 ns | 0.344 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | UInt16     | 64   | 23.69 ns | 0.200 ns | 0.187 ns | 1.22x faster |   0.02x |
|                         |          |            |      |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | UInt32     | 64   | 17.91 ns | 0.343 ns | 0.321 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | UInt32     | 64   | 18.19 ns | 0.343 ns | 0.321 ns | 1.02x slower |   0.03x |
|                         |          |            |      |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | UInt32     | 64   | 15.45 ns | 0.314 ns | 0.294 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | UInt32     | 64   | 15.76 ns | 0.262 ns | 0.245 ns | 1.02x slower |   0.02x |
|                         |          |            |      |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | UInt32     | 64   | 23.57 ns | 0.315 ns | 0.279 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | UInt32     | 64   | 17.62 ns | 0.293 ns | 0.274 ns | 1.34x faster |   0.03x |
|                         |          |            |      |          |          |          |              |         |
| ReverseEndiannessHelper | .NET 6.0 | UInt64     | 64   | 22.52 ns | 0.469 ns | 0.703 ns |     baseline |         |
| ReverseEndiannessHelper | .NET 8.0 | UInt64     | 64   | 19.70 ns | 0.278 ns | 0.232 ns | 1.14x faster |   0.05x |
|                         |          |            |      |          |          |          |              |         |
| BinaryPrimitives        | .NET 6.0 | UInt64     | 64   | 22.37 ns | 0.173 ns | 0.154 ns |     baseline |         |
| BinaryPrimitives        | .NET 8.0 | UInt64     | 64   | 16.65 ns | 0.348 ns | 0.414 ns | 1.34x faster |   0.03x |
|                         |          |            |      |          |          |          |              |         |
| ForeachRef              | .NET 6.0 | UInt64     | 64   | 26.11 ns | 0.257 ns | 0.240 ns |     baseline |         |
| ForeachRef              | .NET 8.0 | UInt64     | 64   | 22.16 ns | 0.448 ns | 0.480 ns | 1.17x faster |   0.03x |
