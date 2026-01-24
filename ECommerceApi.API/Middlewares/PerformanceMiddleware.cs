using System.Diagnostics;

namespace ECommerceApi.API.Middlewares;

public class PerformanceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<PerformanceMiddleware> _logger;

    public PerformanceMiddleware(RequestDelegate next, ILogger<PerformanceMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopWatch = Stopwatch.StartNew();

        try
        {
            await _next(context);
        }
        finally
        {
            stopWatch.Stop();

            var elapsedMilliseconds = stopWatch.ElapsedMilliseconds;

            if (elapsedMilliseconds > 500)
            {
                _logger.LogWarning("yavas istek: {Method} {Path} islemi {Time} ms sürdü",
                context.Request.Method,
                context.Request.Path,
                elapsedMilliseconds);
            }
            else
            {
                _logger.LogInformation("{Method} {Path} işlemi {Time} ms sürdü.",
                    context.Request.Method,
                    context.Request.Path,
                    elapsedMilliseconds);
            }
        }
    }
}
