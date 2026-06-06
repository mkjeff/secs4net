```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8524/25H2/2025Update/HudsonValley2)
12th Gen Intel Core i7-12700 2.10GHz, 1 CPU, 20 logical and 12 physical cores
.NET SDK 10.0.300
  [Host]    : .NET 10.0.8 (10.0.8, 10.0.826.23019), X64 RyuJIT x86-64-v3
  .NET 10.0 : .NET 10.0.8 (10.0.8, 10.0.826.23019), X64 RyuJIT x86-64-v3
  .NET 8.0  : .NET 8.0.27 (8.0.27, 8.0.2726.22922), X64 RyuJIT x86-64-v3


```
| Method        | Runtime   | Categories | Size | Mean     | Error    | StdDev   | Median   | Ratio        | RatioSD |
|-------------- |---------- |----------- |----- |---------:|---------:|---------:|---------:|-------------:|--------:|
| &#39;Unsafe Loop&#39; | .NET 10.0 | Double     | 64   | 20.69 ns | 0.429 ns | 0.728 ns | 20.95 ns | 1.07x faster |   0.04x |
| &#39;Unsafe Loop&#39; | .NET 8.0  | Double     | 64   | 22.19 ns | 0.291 ns | 0.272 ns | 22.21 ns |     baseline |         |
|               |           |            |      |          |          |          |          |              |         |
| ForeachRef    | .NET 10.0 | Double     | 64   | 19.25 ns | 0.358 ns | 0.453 ns | 19.35 ns | 1.48x faster |   0.04x |
| ForeachRef    | .NET 8.0  | Double     | 64   | 28.45 ns | 0.215 ns | 0.191 ns | 28.46 ns |     baseline |         |
|               |           |            |      |          |          |          |          |              |         |
| &#39;Unsafe Loop&#39; | .NET 10.0 | Int16      | 64   | 15.81 ns | 0.339 ns | 0.929 ns | 15.66 ns | 1.01x slower |   0.09x |
| &#39;Unsafe Loop&#39; | .NET 8.0  | Int16      | 64   | 15.71 ns | 0.390 ns | 1.150 ns | 15.03 ns |     baseline |         |
|               |           |            |      |          |          |          |          |              |         |
| ForeachRef    | .NET 10.0 | Int16      | 64   | 15.57 ns | 0.333 ns | 0.312 ns | 15.46 ns | 1.18x faster |   0.07x |
| ForeachRef    | .NET 8.0  | Int16      | 64   | 18.41 ns | 0.393 ns | 1.055 ns | 18.39 ns |     baseline |         |
|               |           |            |      |          |          |          |          |              |         |
| &#39;Unsafe Loop&#39; | .NET 10.0 | Int32      | 64   | 15.53 ns | 0.330 ns | 0.711 ns | 15.37 ns | 1.12x faster |   0.07x |
| &#39;Unsafe Loop&#39; | .NET 8.0  | Int32      | 64   | 17.40 ns | 0.370 ns | 0.667 ns | 17.62 ns |     baseline |         |
|               |           |            |      |          |          |          |          |              |         |
| ForeachRef    | .NET 10.0 | Int32      | 64   | 16.38 ns | 0.352 ns | 0.933 ns | 16.19 ns | 1.40x faster |   0.10x |
| ForeachRef    | .NET 8.0  | Int32      | 64   | 22.79 ns | 0.479 ns | 1.071 ns | 23.11 ns |     baseline |         |
|               |           |            |      |          |          |          |          |              |         |
| &#39;Unsafe Loop&#39; | .NET 10.0 | Int64      | 64   | 16.35 ns | 0.351 ns | 1.013 ns | 16.29 ns | 1.06x slower |   0.07x |
| &#39;Unsafe Loop&#39; | .NET 8.0  | Int64      | 64   | 15.48 ns | 0.328 ns | 0.426 ns | 15.38 ns |     baseline |         |
|               |           |            |      |          |          |          |          |              |         |
| ForeachRef    | .NET 10.0 | Int64      | 64   | 17.40 ns | 0.372 ns | 1.011 ns | 17.56 ns | 1.01x faster |   0.06x |
| ForeachRef    | .NET 8.0  | Int64      | 64   | 17.58 ns | 0.281 ns | 0.249 ns | 17.53 ns |     baseline |         |
|               |           |            |      |          |          |          |          |              |         |
| &#39;Unsafe Loop&#39; | .NET 10.0 | Single     | 64   | 18.56 ns | 0.351 ns | 0.328 ns | 18.61 ns | 1.11x faster |   0.05x |
| &#39;Unsafe Loop&#39; | .NET 8.0  | Single     | 64   | 20.63 ns | 0.436 ns | 0.786 ns | 20.81 ns |     baseline |         |
|               |           |            |      |          |          |          |          |              |         |
| ForeachRef    | .NET 10.0 | Single     | 64   | 16.43 ns | 0.336 ns | 0.694 ns | 16.29 ns | 1.24x faster |   0.05x |
| ForeachRef    | .NET 8.0  | Single     | 64   | 20.40 ns | 0.144 ns | 0.127 ns | 20.33 ns |     baseline |         |
|               |           |            |      |          |          |          |          |              |         |
| &#39;Unsafe Loop&#39; | .NET 10.0 | UInt16     | 64   | 21.31 ns | 0.459 ns | 0.840 ns | 21.62 ns | 1.00x faster |   0.04x |
| &#39;Unsafe Loop&#39; | .NET 8.0  | UInt16     | 64   | 21.33 ns | 0.259 ns | 0.242 ns | 21.33 ns |     baseline |         |
|               |           |            |      |          |          |          |          |              |         |
| ForeachRef    | .NET 10.0 | UInt16     | 64   | 16.48 ns | 0.354 ns | 0.933 ns | 16.60 ns | 1.08x faster |   0.07x |
| ForeachRef    | .NET 8.0  | UInt16     | 64   | 17.67 ns | 0.376 ns | 0.607 ns | 17.46 ns |     baseline |         |
|               |           |            |      |          |          |          |          |              |         |
| &#39;Unsafe Loop&#39; | .NET 10.0 | UInt32     | 64   | 17.55 ns | 0.374 ns | 0.772 ns | 17.79 ns | 1.03x faster |   0.05x |
| &#39;Unsafe Loop&#39; | .NET 8.0  | UInt32     | 64   | 18.06 ns | 0.360 ns | 0.319 ns | 18.09 ns |     baseline |         |
|               |           |            |      |          |          |          |          |              |         |
| ForeachRef    | .NET 10.0 | UInt32     | 64   | 16.44 ns | 0.351 ns | 0.875 ns | 16.40 ns | 1.43x faster |   0.08x |
| ForeachRef    | .NET 8.0  | UInt32     | 64   | 23.39 ns | 0.357 ns | 0.334 ns | 23.48 ns |     baseline |         |
|               |           |            |      |          |          |          |          |              |         |
| &#39;Unsafe Loop&#39; | .NET 10.0 | UInt64     | 64   | 21.45 ns | 0.273 ns | 0.255 ns | 21.56 ns | 1.01x faster |   0.02x |
| &#39;Unsafe Loop&#39; | .NET 8.0  | UInt64     | 64   | 21.59 ns | 0.412 ns | 0.385 ns | 21.79 ns |     baseline |         |
|               |           |            |      |          |          |          |          |              |         |
| ForeachRef    | .NET 10.0 | UInt64     | 64   | 16.96 ns | 0.363 ns | 1.019 ns | 16.57 ns | 1.05x faster |   0.06x |
| ForeachRef    | .NET 8.0  | UInt64     | 64   | 17.69 ns | 0.221 ns | 0.173 ns | 17.66 ns |     baseline |         |
