namespace DS.Common.Exceptions;

public class EntityNotFoundException : DoSvyaziMusicException
{
    public EntityNotFoundException(string message) 
        : base(message)
    {
    }

    public EntityNotFoundException()
    {
    }

    public EntityNotFoundException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }

    public static EntityNotFoundException Throw<TType>(TType type)
    {
        return new EntityNotFoundException($"[{type}] Entity was not found.");
    }
}