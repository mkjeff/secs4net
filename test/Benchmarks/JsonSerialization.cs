using BenchmarkDotNet.Attributes;
using Secs4Net.Json;
using System.Text.Json;
using static Secs4Net.Item;

namespace Secs4Net.Benchmark
{
    [Config(typeof(BenchmarkConfig))]
    [MemoryDiagnoser]
    //[NativeMemoryProfiler]
    public class JsonSerialization
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            Converters = { new ItemJsonConverter() },
        };

        private static readonly SecsMessage Message = new(s: 1, f: 2, replyExpected: false)
        {
            Name = "Test",
            SecsItem =
                L(
                    L(),
                    U1(122, 34, 0),
                    U2(34531, 23123, 24),
                    U4(2123513, 52451141, 1),
                    F4(23123.21323f, 2324.221f, -20.131f),
                    A("A string"),
                    Boolean(true, false, false, true),
                        B(0x1C, 0x01, 0xFF),
                        L(
                            A("A string"),
                            J("sdsad"),
                            Boolean(true, false, false, true),
                            B(0x1C, 0x01, 0xFF)),
                        F8(231.00002321d, 0.2913212312d),
                    J("sdsad"),
                    F8(231.00002321d, 0.2913212312d, -124.42002d),
                    L(
                        I1(122, 34, -13),
                        I2(4531, -23123, 12),
                        I4(2123513, 52451141, -11),
                        F4(23123.21323f, 2324.221f),
                        Boolean(true, false, false, true),
                        B(0x1C, 0x01, 0xFF),
                        L(
                            A("A string"),
                            J("sdsad"),
                            Boolean(true, false, false, true),
                            B(0x1C, 0x01, 0xFF)),
                        F8(231.00002321d, 0.2913212312d)))
        };

        private static readonly string JsonString = JsonSerializer.Serialize(Message, JsonSerializerOptions);

        [Benchmark]
        public string Serialize()
            => JsonSerializer.Serialize(Message, JsonSerializerOptions);

        [Benchmark]
        public SecsMessage Deserialze()
            => JsonSerializer.Deserialize<SecsMessage>(JsonString, JsonSerializerOptions);
    }
}
