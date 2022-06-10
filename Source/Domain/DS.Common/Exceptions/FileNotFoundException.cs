namespace DS.Common.Exceptions;

public class FileNotFoundException : DoSvyaziMusicException
{
    protected FileNotFoundException()
    {
    }

    public FileNotFoundException(string message) 
        : base(message)
    {
    }

    public FileNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}