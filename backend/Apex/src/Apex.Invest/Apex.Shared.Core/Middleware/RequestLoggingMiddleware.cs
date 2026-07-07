using Apex.Shared.Core.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Apex.Shared.Core.Middleware;

public class RequestLoggingMiddleware(RequestDelegate _next, ILogger<RequestLoggingMiddleware> _logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        await using var ls = new LoggerStopwatch(t => _logger.LogInformation($"{context.Request.Method}/{context.Request.Path} - spend {t}"));
        await _next(context);
    }
}
