namespace comunicacion_anuncios.Application.Common;

public record PagedRequest(
    int Page = 1,
    int Size = 10,
    string? Search = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    bool? ActiveOnly = null);

public record PagedResult<T>(
    IReadOnlyList<T> Items,
    int Total,
    int Page,
    int Size);