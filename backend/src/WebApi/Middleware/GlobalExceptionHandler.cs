using Communication.Announcements.Application.Features.Auth.Commands;
using Communication.Announcements.Application.Features.Announcements.Commands;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Communication.Announcements.WebApi.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unhandled exception occurred while processing the request.");

        ProblemDetails problemDetails = exception switch
        {
            ValidationException validationException => CreateValidationProblem(httpContext, validationException),
            InvalidCredentialsException invalidCredentials => CreateUnauthorizedProblem(httpContext, invalidCredentials),
            NotFoundException notFoundException => CreateProblem(httpContext, StatusCodes.Status404NotFound, notFoundException.Message),
            _ => CreateProblem(httpContext, StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };

        httpContext.Response.ContentType = "application/problem+json";
        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static ProblemDetails CreateValidationProblem(HttpContext context, ValidationException exception)
    {
        var problemDetails = new ValidationProblemDetails(exception.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()))
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "One or more validation errors occurred.",
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Instance = context.Request.Path
        };

        return problemDetails;
    }

    private static ProblemDetails CreateUnauthorizedProblem(HttpContext context, InvalidCredentialsException exception)
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Authentication failed",
            Detail = exception.Message,
            Type = "https://datatracker.ietf.org/doc/html/rfc7235",
            Instance = context.Request.Path
        };
    }

    private static ProblemDetails CreateProblem(HttpContext context, int statusCode, string message)
    {
        return new ProblemDetails
        {
            Status = statusCode,
            Title = ReasonPhrases.GetReasonPhrase(statusCode),
            Detail = message,
            Instance = context.Request.Path
        };
    }
}
