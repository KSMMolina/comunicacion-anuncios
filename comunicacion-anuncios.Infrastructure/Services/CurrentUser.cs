using System.Security.Claims;
using comunicacion_anuncios.Application.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace comunicacion_anuncios.Infrastructure.Services;

public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _http;
    public CurrentUser(IHttpContextAccessor http) => _http = http;

    private ClaimsPrincipal? User => _http.HttpContext?.User;

    public Guid GetUserId()
    {
        // Preferimos NameIdentifier; fallback a sub
        var idValue =
            User?.FindFirst(ClaimTypes.NameIdentifier).Value ??
            User?.FindFirst(JwtRegisteredClaimNames.Sub).Value ??
            User?.FindFirst("sub").Value;

        return Guid.TryParse(idValue, out var id) ? id : Guid.Empty;
    }

    public string GetUserEmail() =>
        User?.FindFirst(ClaimTypes.Email).Value
        ?? User?.FindFirst(JwtRegisteredClaimNames.Email).Value
        ?? User?.FindFirst("email").Value
        ?? string.Empty;

    public string GetUserRole() =>
        User?.FindFirst(ClaimTypes.Role).Value
        ?? User?.FindFirst("role").Value
        ?? string.Empty;

    public string GetUserName() =>
        User?.FindFirst(ClaimTypes.Name).Value
        ?? User?.Identity?.Name
        ?? string.Empty;
}