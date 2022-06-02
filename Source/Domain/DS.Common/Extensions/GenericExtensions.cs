using System.Runtime.CompilerServices;
namespace DS.Common.Extensions;

public static class GenericExtensions
{
    private static TValue ThrowIfNull<TValue, TException>(this TValue? value, TException exception)
        where TException : Exception
    {
        if (value is string str && string.IsNullOrWhiteSpace(str))
            throw exception;
        
        if (value is null)
            throw exception;

        return value;
    }

    public static TValue ThrowIfNull<TValue>(this TValue? value, [CallerArgumentExpression("value")] string argumentName = "")
        => value.ThrowIfNull(new ArgumentNullException(argumentName));

    private static TValue ThrowIfNull<TValue, TException>(this TValue? value, TException exception)
        where TException : Exception
        where TValue : struct
    {
        if (value is null)
            throw exception;

        return value.Value;
    }

    public static TValue ThrowIfNull<TValue>(this TValue? value, [CallerArgumentExpression("value")] string argumentName = "")
        where TValue : struct
        => value.ThrowIfNull(new ArgumentNullException(argumentName));
}