using DS.Common.Exceptions;
using DS.Domain;

namespace DS.Music.WebApi.Extensions;

public static class HttpContextExtension
{
    public static MusicUser GetUserFromContext(this HttpContext context)
    {
        if (context.Items["User"] is MusicUser userModel)
            return userModel;
        else
            throw new UnauthorizedException();
    }
}