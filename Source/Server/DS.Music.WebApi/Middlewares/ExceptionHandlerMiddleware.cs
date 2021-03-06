using Bogus;
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
        catch (Exception e) when (e is ContentNotFoundException or EntityNotFoundException)
        {
            context.Response.ContentType = "text/*";
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync(e.Message);
        }
        catch (Exception e) when (e is UnauthorizedException)
        {
            context.Response.ContentType = "text/*";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync(e.Message);
        }
        catch (Exception e) when (e is DoSvyaziMusicException or ValidationException)
        {
            context.Response.ContentType = "text/*";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync(e.Message);
        }
        catch (Exception e)
        {
            context.Response.ContentType = "text/*";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;   
            await context.Response.WriteAsync(e.ToString());
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