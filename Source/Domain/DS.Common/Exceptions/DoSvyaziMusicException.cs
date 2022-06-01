namespace DS.Common.Exceptions;

public class DoSvyaziMusicException : Exception
{
    public DoSvyaziMusicException()
    {
    }

    public DoSvyaziMusicException(string message) 
        : base(message)
    {
    }

    public DoSvyaziMusicException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}