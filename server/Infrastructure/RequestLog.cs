using System.Diagnostics;

namespace server.Infrastructure;

public sealed class RequestLog(RequestDelegate next, ILogger<RequestLog> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var started = Stopwatch.GetTimestamp();
        await next(context);
        logger.LogInformation("{Method} {Path} -> {StatusCode} in {ElapsedMs:0} ms",
            context.Request.Method, context.Request.Path, context.Response.StatusCode,
            Stopwatch.GetElapsedTime(started).TotalMilliseconds);
    }
}
