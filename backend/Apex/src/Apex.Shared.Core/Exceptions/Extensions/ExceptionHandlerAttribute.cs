
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Apex.Shared.Core.Exceptions.Extensions;

public class ExceptionHandlerAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case BadRequestException:
                HandleBadRequest(context);
                break;
            case NotFoundException:
                HandleNotFound(context);
                break;
            case ValidationException:
                HandleNotValid(context);
                break;
            case ConflictException:
                HandleConflict(context);
                break;
            case UnauthorizedException:
                HandleUnauthorized(context);
                break;
            default:
                HandleInternalError(context);
                break;
        }
    }

    private static void HandleNotFound(ExceptionContext context)
        => SetProblem(context, HttpStatusCode.NotFound, context.Exception.Message);

    private static void HandleNotValid(ExceptionContext context)
    {
        if (context.Exception is not ValidationException validation || validation.Errors.Count == 0)
        {
            HandleBadRequest(context);
            return;
        }

        var problem = CreateProblem(context, HttpStatusCode.BadRequest, "Validation failed");
        problem.Extensions["errors"] = validation.Errors;
        context.Result = new ObjectResult(problem) { StatusCode = (int)HttpStatusCode.BadRequest };
        context.ExceptionHandled = true;
    }

    private static void HandleBadRequest(ExceptionContext context)
        => SetProblem(context, HttpStatusCode.BadRequest, context.Exception.Message);

    private static void HandleConflict(ExceptionContext context)
        => SetProblem(context, HttpStatusCode.Conflict, context.Exception.Message);

    private static void HandleUnauthorized(ExceptionContext context)
        => SetProblem(context, HttpStatusCode.Unauthorized, context.Exception.Message);

    private static void HandleInternalError(ExceptionContext context)
        => SetProblem(context, HttpStatusCode.InternalServerError, "Internal Server Error");

    private static ProblemDetails CreateProblem(ExceptionContext context, HttpStatusCode status, string title)
    {
        var traceId = context.HttpContext.TraceIdentifier;
        var correlationId = context.HttpContext.Request.Headers["X-Correlation-Id"].FirstOrDefault();

        var problem = new ProblemDetails
        {
            Status = (int)status,
            Type = $"https://httpstatuses.com/{(int)status}",
            Title = title,
            Instance = context.HttpContext.Request.Path
        };

        problem.Extensions["traceId"] = traceId;
        if (!string.IsNullOrWhiteSpace(correlationId))
            problem.Extensions["correlationId"] = correlationId;

        return problem;
    }

    private static void SetProblem(ExceptionContext context, HttpStatusCode status, string title, string? detail = null)
    {
        var problem = CreateProblem(context, status, title);
        problem.Detail = detail;
        context.Result = new ObjectResult(problem) { StatusCode = (int)status };
        context.ExceptionHandled = true;
    }
}