namespace Secs4Net;

public sealed class SecsException : Exception
{
    public SecsMessage? SecsMessage { get; }

    public SecsException(SecsMessage? secsMessage, string errorMessage)
        : base(errorMessage)
    {
        SecsMessage = secsMessage;
    }

    public SecsException(string msg)
        : base(msg)
    {
    }
}
