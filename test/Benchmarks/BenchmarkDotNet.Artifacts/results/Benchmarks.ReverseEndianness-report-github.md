```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3007/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  Job-ISCVXC : .NET 6.0.26 (6.0.2623.60508), X64 RyuJIT AVX2
  Job-DINVIM : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  Job-OKZTLT : .NET Framework 4.8.1 (4.8.9181.0), X64 RyuJIT VectorSize=256


```
| Method                  | Runtime            | Categories | Size | Mean      | Error    | StdDev    | Median    | Ratio        | RatioSD |
|------------------------ |------------------- |----------- |----- |----------:|---------:|----------:|----------:|-------------:|--------:|
| ReverseEndiannessHelper | .NET 6.0           | Double     | 64   |  36.78 ns | 0.848 ns |  2.501 ns |  37.53 ns | 6.24x faster |   0.69x |
| ReverseEndiannessHelper | .NET 8.0           | Double     | 64   |  33.94 ns | 0.698 ns |  1.737 ns |  34.65 ns | 6.69x faster |   0.52x |
| ReverseEndiannessHelper | .NET Framework 4.8 | Double     | 64   | 226.66 ns | 4.501 ns | 11.457 ns | 231.04 ns |     baseline |         |
|                         |                    |            |      |           |          |           |           |              |         |
| ReverseEndiannessHelper | .NET 6.0           | Int16      | 64   |  35.47 ns | 0.911 ns |  2.685 ns |  36.48 ns | 2.10x faster |   0.20x |
| ReverseEndiannessHelper | .NET 8.0           | Int16      | 64   |  39.72 ns | 0.805 ns |  1.047 ns |  40.22 ns | 1.87x faster |   0.11x |
| ReverseEndiannessHelper | .NET Framework 4.8 | Int16      | 64   |  74.31 ns | 1.507 ns |  3.640 ns |  75.52 ns |     baseline |         |
|                         |                    |            |      |           |          |           |           |              |         |
| ReverseEndiannessHelper | .NET 6.0           | Int32      | 64   |  28.68 ns | 0.594 ns |  1.646 ns |  29.17 ns | 2.78x faster |   0.25x |
| ReverseEndiannessHelper | .NET 8.0           | Int32      | 64   |  27.09 ns | 0.562 ns |  1.481 ns |  27.70 ns | 2.94x faster |   0.21x |
| ReverseEndiannessHelper | .NET Framework 4.8 | Int32      | 64   |  78.90 ns | 1.588 ns |  2.653 ns |  80.29 ns |     baseline |         |
|                         |                    |            |      |           |          |           |           |              |         |
| ReverseEndiannessHelper | .NET 6.0           | Int64      | 64   |  29.44 ns | 0.578 ns |  0.618 ns |  29.59 ns | 3.93x faster |   0.90x |
| ReverseEndiannessHelper | .NET 8.0           | Int64      | 64   |  39.21 ns | 0.904 ns |  2.665 ns |  39.99 ns | 3.28x faster |   0.29x |
| ReverseEndiannessHelper | .NET Framework 4.8 | Int64      | 64   | 128.85 ns | 4.954 ns | 14.606 ns | 134.95 ns |     baseline |         |
|                         |                    |            |      |           |          |           |           |              |         |
| ReverseEndiannessHelper | .NET 6.0           | Single     | 64   |  37.45 ns | 0.761 ns |  1.503 ns |  38.10 ns | 4.59x faster |   0.32x |
| ReverseEndiannessHelper | .NET 8.0           | Single     | 64   |  34.50 ns | 0.685 ns |  0.914 ns |  34.75 ns | 4.97x faster |   0.32x |
| ReverseEndiannessHelper | .NET Framework 4.8 | Single     | 64   | 172.60 ns | 3.468 ns |  8.763 ns | 176.16 ns |     baseline |         |
|                         |                    |            |      |           |          |           |           |              |         |
| ReverseEndiannessHelper | .NET 6.0           | UInt16     | 64   |  31.14 ns | 0.494 ns |  0.462 ns |  31.27 ns | 1.96x faster |   0.24x |
| ReverseEndiannessHelper | .NET 8.0           | UInt16     | 64   |  36.98 ns | 1.316 ns |  3.880 ns |  37.95 ns | 1.73x faster |   0.17x |
| ReverseEndiannessHelper | .NET Framework 4.8 | UInt16     | 64   |  63.23 ns | 1.307 ns |  3.853 ns |  64.30 ns |     baseline |         |
|                         |                    |            |      |           |          |           |           |              |         |
| ReverseEndiannessHelper | .NET 6.0           | UInt32     | 64   |  28.75 ns | 0.668 ns |  1.970 ns |  29.48 ns | 2.83x faster |   0.38x |
| ReverseEndiannessHelper | .NET 8.0           | UInt32     | 64   |  27.35 ns | 0.615 ns |  1.813 ns |  28.06 ns | 2.96x faster |   0.36x |
| ReverseEndiannessHelper | .NET Framework 4.8 | UInt32     | 64   |  79.82 ns | 1.614 ns |  3.187 ns |  81.08 ns |     baseline |         |
|                         |                    |            |      |           |          |           |           |              |         |
| ReverseEndiannessHelper | .NET 6.0           | UInt64     | 64   |  29.49 ns | 0.597 ns |  0.929 ns |  29.78 ns | 2.41x faster |   0.09x |
| ReverseEndiannessHelper | .NET 8.0           | UInt64     | 64   |  38.88 ns | 0.978 ns |  2.884 ns |  40.00 ns | 1.91x faster |   0.36x |
| ReverseEndiannessHelper | .NET Framework 4.8 | UInt64     | 64   |  70.88 ns | 0.913 ns |  1.550 ns |  70.64 ns |     baseline |         |
