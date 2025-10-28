namespace comunicacion_anuncios.Application.Common;

public class ApiResponse<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public string? Message { get; init; }
    public IReadOnlyList<string>? Errors { get; init; }
    public IReadOnlyDictionary<string, string[]>? FieldErrors { get; init; }

    private ApiResponse() { }

    public static ApiResponse<T> Ok(T data, string? message = null) =>
        new() { Success = true, Data = data, Message = message };

    public static ApiResponse<T> OkMessage(string? message = null) =>
        new() { Success = true, Message = message };

    public static ApiResponse<T> Fail(IReadOnlyList<string>? errors, string? message = null) =>
        new() { Success = false, Message = message, Errors = errors };

    public static ApiResponse<T> FailFields(IReadOnlyDictionary<string, string[]> fieldErrors, string? message = null) =>
        new() { Success = false, Message = message, FieldErrors = fieldErrors };
}

public static class ApiResponse
{
    public static ApiResponse<object> Ok(string? message = null) =>
        ApiResponse<object>.OkMessage(message);

    public static ApiResponse<T> Ok<T>(T data, string? message = null) =>
        ApiResponse<T>.Ok(data, message);

    public static ApiResponse<T> Fail<T>(IReadOnlyList<string>? errors, string? message = null) =>
        ApiResponse<T>.Fail(errors, message);

    public static ApiResponse<T> FailFields<T>(IReadOnlyDictionary<string, string[]> fieldErrors, string? message = null) =>
        ApiResponse<T>.FailFields(fieldErrors, message);
}
