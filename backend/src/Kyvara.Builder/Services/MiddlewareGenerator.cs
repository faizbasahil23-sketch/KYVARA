namespace Kyvara.Builder.Services;

public sealed class MiddlewareGenerator
{
    public string GenerateExceptionMiddleware()
    {
        return
"""
using Microsoft.AspNetCore.Http;

namespace Generated.Middlewares;

public sealed class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(Exception ex)
        {
            context.Response.StatusCode = 500;

            await context.Response.WriteAsync(ex.Message);
        }
    }
}
""";
    }

    public string GenerateLoggingMiddleware()
    {
        return
"""
using Microsoft.AspNetCore.Http;

namespace Generated.Middlewares;

public sealed class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Console.WriteLine($"{context.Request.Method} {context.Request.Path}");

        await _next(context);
    }
}
""";
    }
}
