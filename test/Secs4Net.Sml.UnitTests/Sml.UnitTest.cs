using FluentAssertions;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Secs4Net.Sml.UnitTests;

public class SmlUnitTest
{
    private readonly SecsMessage message = new(s: 1, f: 2, replyExpected: false)
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

    [Fact]
    public void SecsMessage_Can_Serialize_And_Deserialize()
    {
        var sml = message.ToSml();
        var deserialized = sml.ToSecsMessage();

        deserialized.Should().BeEquivalentTo(message);
    }

    [Fact]
    public async Task Multiple_SecsMessage_Can_Serialize_And_Deserialize_From_Stream()
    {
        var messages = Enumerable.Repeat(message, 5).ToList();

        var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream);
        foreach (var m in messages)
        {
            m.WriteSmlTo(writer);
        }
        writer.Flush();

        memoryStream.Position = 0;


        using var reader = new StreamReader(memoryStream);
        var i = 0;
        await foreach (var m in reader.ToSecsMessages())
        {
            m.Should().BeEquivalentTo(messages[i]);
            i++;
        }

        i.Should().Be(messages.Count);
    }

    [Fact]
    public void Class_Can_To_Sml_And_Sml_Can_To_Class()
    {
        var testClass = new TestClass();
        string str = testClass.ToSml();
        str = $"TEST:'S6F1' W\r\n{str}";
        var message = str.ToSecsMessage();
        var testClass1 = message.ToType<TestClass>();

        testClass1.Should().BeEquivalentTo(testClass);
    }

    private class TestClass
    {
        public byte ALCD = 1;
        public uint ALID = 2;
        public TestSubClass testSubClass;
        public TestClass()
        {
            testSubClass = new TestSubClass();
        }
        public class TestSubClass
        {
            public string DeviceID = "DeviceID";
            public ushort UnitID = 666;
            public float[] FloatInfo = new float[2] { 1, 2 };
            public int[] IntInfo = new int[2] { 4, 5 };
        }
    }
}
