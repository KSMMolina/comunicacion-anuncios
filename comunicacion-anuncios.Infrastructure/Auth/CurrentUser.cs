using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using comunicacion_anuncios.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace comunicacion_anuncios.Infrastructure.Auth;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUser(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

    private ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User ?? throw new InvalidOperationException("No HttpContext/User disponible");

    public Guid GetUserId()
    {
        var sub = User.FindFirst(JwtRegisteredClaimNames.Sub).Value;
        if (Guid.TryParse(sub, out var id)) return id;
        throw new InvalidOperationException("Claim sub inválida");
    }

    public string GetUserEmail()
    {
        return User.FindFirst(ClaimTypes.Email)?.Value ??
               User.FindFirst(JwtRegisteredClaimNames.Email)?.Value ??
               throw new InvalidOperationException("Claim email no encontrada");
    }

    public string GetUserRole()
    {
        return User.FindFirst("role")?.Value ??
               User.FindFirst(ClaimTypes.Role)?.Value ??
               string.Empty;
    }

    public string GetUserName()
    {
        return User.FindFirst("name")?.Value ??
               User.FindFirst(ClaimTypes.Name)?.Value ??
               string.Empty;
    }
}