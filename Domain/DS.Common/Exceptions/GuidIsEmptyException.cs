namespace DS.Common.Exceptions;

public class GuidIsEmptyException : DoSvyaziMusicException
{
    public GuidIsEmptyException(string message) 
        : base(message)
    {
    }

    public GuidIsEmptyException()
    {
    }

    public GuidIsEmptyException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }

    public static GuidIsEmptyException Create(Guid guid) => 
        new GuidIsEmptyException($"[{guid}] Is empty.");
}