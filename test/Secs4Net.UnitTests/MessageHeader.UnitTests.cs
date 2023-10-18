using Xunit;

namespace Secs4Net.UnitTests;

public class MessageHeaderUnitTests
{
    [Fact]
    public void Header_should_ignore_direction_in_device_id()
    {
        /*
         * Equipment to Host
         * Device ID 1
         * Reply expected
         * S6F11
         * Select request
         * ID: 1
         */
        var headerBytes = new byte[] { 128, 1, 134, 11, 0, 1, 0, 0, 0, 1 };
        MessageHeader.Decode(headerBytes, out MessageHeader header);

        Assert.Multiple(
            () => Assert.Equal(1, header.DeviceId),
            () => Assert.True(header.ReplyExpected),
            () => Assert.Equal(6, header.S),
            () => Assert.Equal(11, header.F),
            () => Assert.Equal(MessageType.SelectRequest, header.MessageType),
            () => Assert.Equal(1, header.Id));
    }
}
