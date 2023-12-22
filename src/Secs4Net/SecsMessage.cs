namespace Secs4Net;

public sealed class SecsMessage : IDisposable
{
    public override string ToString() => $"'S{S}F{F}' {(ReplyExpected ? "W" : string.Empty)} {Name ?? string.Empty}";

    /// <summary>
    /// message stream number
    /// </summary>
    public byte S { get; }

    /// <summary>
    /// message function number
    /// </summary>
    public byte F { get; }

    /// <summary>
    /// expect reply message
    /// </summary>
    public bool ReplyExpected { get; internal set; }

    public string? Name { get; set; }

    /// <summary>
    /// the root item of message
    /// </summary>
    public Item? SecsItem { get; set; }

    /// <summary>
    /// constructor of SecsMessage
    /// </summary>
    /// <param name="s">message stream number</param>
    /// <param name="f">message function number</param>
    /// <param name="replyExpected">expect reply message</param>
    public SecsMessage(byte s, byte f, bool replyExpected = true)
    {
        if (s > 0b0111_1111)
        {
            throw new ArgumentOutOfRangeException(nameof(s), s, Resources.SecsMessageStreamNumberMustLessThan127);
        }

        S = s;
        F = f;
        ReplyExpected = replyExpected;
    }

    public void Dispose()
    {
        SecsItem?.Dispose();
    }
}
