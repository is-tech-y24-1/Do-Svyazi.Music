using DS.Common.Exceptions;

namespace DS.Music.WebApi.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            switch (e)
            {
                case ContentNotFoundException:
                case EntityNotFoundException:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    context.Response.ContentType = "text/*";

                    await context.Response.WriteAsync(e.Message);
                    break;
                case DoSvyaziMusicException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.ContentType = "text/*";

                    await context.Response.WriteAsync(e.Message);
                    break;
                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "text/*";

                    await context.Response.WriteAsync(e.Message);
                    break;
            }
        }
    }
}

public static class ExceptionHandlerMiddlewareExtension
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}