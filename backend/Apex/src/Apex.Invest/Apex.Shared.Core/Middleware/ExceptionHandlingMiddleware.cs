using Apex.Shared.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Apex.Shared.Core.Middleware;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            var (statusCode, errors) = exception switch
            {
                ValidationException => (StatusCodes.Status400BadRequest, new[] { exception.Message }),
                BadRequestException => (StatusCodes.Status400BadRequest, new[] { exception.Message }),
                NotFoundException => (StatusCodes.Status404NotFound, new[] { exception.Message }),
                ConflictException => (StatusCodes.Status409Conflict, new[] { exception.Message }),
                UnauthorizedException => (StatusCodes.Status401Unauthorized, new[] { exception.Message }),
                _ => (StatusCodes.Status500InternalServerError, new[] { "Internal server error" })
            };

            if (statusCode == StatusCodes.Status500InternalServerError)
                logger.LogError(exception, "Unhandled exception");

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(new { errors });
        }
    }
}