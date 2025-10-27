using comunicacion_anuncios.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace comunicacion_anuncios.Presentation.Extensions;

public static class ControllerResponseExtensions
{
    public static IActionResult OkResponse<T>(this ControllerBase controller, T data, string? message = null) =>
        controller.Ok(ApiResponse.Ok(data, message));

    public static IActionResult OkMessage(this ControllerBase controller, string? message = null) =>
        controller.Ok(ApiResponse.Ok(message));

    public static IActionResult CreatedResponse<T>(this ControllerBase controller, string location, T data, string? message = null) =>
        controller.Created(location, ApiResponse.Ok(data, message));

    public static IActionResult FailResponse(this ControllerBase controller, int statusCode, IEnumerable<string>? errors, string? message = null)
    {
        var list = errors?.ToList();
        return controller.StatusCode(statusCode, ApiResponse.Fail<object>(list, message));
    }

    public static IActionResult FailFieldResponse(this ControllerBase controller, int statusCode, IReadOnlyDictionary<string, string[]> fieldErrors, string? message = null) =>
        controller.StatusCode(statusCode, ApiResponse.FailFields<object>(fieldErrors, message));
}