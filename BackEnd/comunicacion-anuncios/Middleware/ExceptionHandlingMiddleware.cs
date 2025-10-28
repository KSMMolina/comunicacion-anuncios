using FluentValidation;
using System.Text.Json;
using comunicacion_anuncios.Application.Common;

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
            await WriteApiErrorAsync(context, ex);
        }
    }

    private static async Task WriteApiErrorAsync(HttpContext context, Exception ex)
    {
        if (context.Response.HasStarted)
            return;

        var (status, message, errorsSimple, fieldErrors) = MapException(ex);

        object envelope;
        if (fieldErrors is not null)
            envelope = ApiResponse.FailFields<object>(fieldErrors, message);
        else
            envelope = ApiResponse.Fail<object>(errorsSimple, message);

        context.Response.Clear();
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = status;

        var json = JsonSerializer.Serialize(envelope, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }

    private static (int status, string message, IReadOnlyList<string>? errorsSimple, IReadOnlyDictionary<string, string[]>? fieldErrors)
        MapException(Exception ex) =>
        ex switch
        {
            ValidationException ve => (
                StatusCodes.Status400BadRequest,
                "Errores de validación",
                null,
                ve.Errors
                  .GroupBy(e => e.PropertyName)
                  .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
            ),
            UnauthorizedAccessException => (
                StatusCodes.Status401Unauthorized,
                "No autorizado",
                new[] { "Credenciales inválidas o faltantes" },
                null
            ),
            KeyNotFoundException => (
                StatusCodes.Status404NotFound,
                "Recurso no encontrado",
                new[] { "El recurso solicitado no existe" },
                null
            ),
            _ => (
                StatusCodes.Status500InternalServerError,
                "Error interno",
                new[] { "Ha ocurrido un error inesperado" },
                null
            )
        };
}