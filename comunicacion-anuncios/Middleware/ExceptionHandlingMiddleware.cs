using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace comunicacion_anuncios.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await WriteProblemAsync(context, ex);
        }
    }

    private static async Task WriteProblemAsync(HttpContext context, Exception ex)
    {
        if (context.Response.HasStarted)
            return; // No se puede modificar cabeceras si ya comenzó

        var (status, type, title, errors) = MapException(ex);

        var problem = new ProblemDetails
        {
            Type = type,
            Title = title,
            Status = status,
            Detail = ex.Message,
            Instance = context.Request.Path
        };

        problem.Extensions["traceId"] = context.TraceIdentifier;
        if (errors is not null) problem.Extensions["errors"] = errors;

        context.Response.Clear();
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = status; // corrección: quitar ?? operador

        var json = JsonSerializer.Serialize(problem, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }

    private static (int status, string type, string title, object? errors) MapException(Exception ex) =>
        ex switch
        {
            ValidationException ve => (
                StatusCodes.Status400BadRequest,
                "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                "ValidationFailed",
                ve.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
            ),
            UnauthorizedAccessException => (
                StatusCodes.Status401Unauthorized,
                "https://tools.ietf.org/html/rfc7235#section-3.1",
                "Unauthorized",
                null
            ),
            KeyNotFoundException => (
                StatusCodes.Status404NotFound,
                "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                "NotFound",
                null
            ),
            _ => (
                StatusCodes.Status500InternalServerError,
                "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                "ServerError",
                null
            )
        };
}