namespace Amaris.Api.Middleware;

public class RequestHeaderLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestHeaderLoggingMiddleware> _logger;

    public RequestHeaderLoggingMiddleware(RequestDelegate next, ILogger<RequestHeaderLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("X-Correlation-Id", out var correlationId))
        {
            _logger.LogInformation("Request {Method} {Path} - X-Correlation-Id: {CorrelationId}",
                context.Request.Method, context.Request.Path, correlationId.ToString());
        }
        else
        {
            _logger.LogInformation("Request {Method} {Path} - No X-Correlation-Id header",
                context.Request.Method, context.Request.Path);
        }

        await _next(context);
    }
}
