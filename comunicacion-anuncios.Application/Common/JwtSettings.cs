namespace comunicacion_anuncios.Application.Common;

public sealed class JwtSettings
{
    public string Issuer { get; init; } = default!;
    public string Audience { get; init; } = default!;
    public string Key { get; init; } = default!;
    public int ExpiresMinutes { get; init; }
}