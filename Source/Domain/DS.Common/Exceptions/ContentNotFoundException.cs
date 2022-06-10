namespace DS.Common.Exceptions;

public class ContentNotFoundException : DoSvyaziMusicException
{
    protected ContentNotFoundException()
    {
    }

    public ContentNotFoundException(string message) 
        : base(message)
    {
    }

    public ContentNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}