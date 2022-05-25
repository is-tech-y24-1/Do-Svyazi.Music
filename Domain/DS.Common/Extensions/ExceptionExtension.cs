using DS.Common.Exceptions;

namespace DS.Common.Extensions;

public static class ExceptionExtension
{
    public static void ThrowIfNullOrEmpty(this object? obj, string message)
    {
        switch (obj)
        {
            case string stringToCheck:
                if (string.IsNullOrEmpty(stringToCheck))
                    throw new DoSvyaziMusicException(message);
                break;
            default:
                if (obj is null)
                    throw new DoSvyaziMusicException(message);
                break;
        }
            
    }
}