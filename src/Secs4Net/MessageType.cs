namespace Secs4Net;

public enum MessageType : byte
{
    DataMessage = 0b0000_0000,
    SelectRequest = 0b0000_0001,
    SelectResponse = 0b0000_0010,
    Deselect_req = 0b0000_0011,
    Deselect_rsp = 0b0000_0100,
    LinkTestRequest = 0b0000_0101,
    LinkTestResponse = 0b0000_0110,
    Reject_req = 0b0000_0111,
    SeparateRequest = 0b0000_1001
}
