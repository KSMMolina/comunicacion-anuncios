using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using comunicacion_anuncios.Application.Abstractions;
using comunicacion_anuncios.Application.Common;
using comunicacion_anuncios.Domain.Entities;

namespace comunicacion_anuncios.Infrastructure.Auth;

public sealed class JwtTokenGenerator : IJwtTokenGenerator
{
    public (string Token, DateTime ExpiresAt) Generate(User user, JwtSettings settings)
    {
        if (user is null) throw new ArgumentNullException(nameof(user));
        if (string.IsNullOrWhiteSpace(settings.Key))
            throw new InvalidOperationException("JwtSettings.Key no configurado.");
        
        var keyBytes = Encoding.UTF8.GetBytes(settings.Key);
        if (keyBytes.Length < 32)
            throw new InvalidOperationException("La clave JWT debe tener >= 32 caracteres.");

        var signingKey = new SymmetricSecurityKey(keyBytes);
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(settings.ExpiresMinutes <= 0 ? 120 : settings.ExpiresMinutes);

        var roleName = user.Role?.Name ?? "Resident";

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, roleName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: string.IsNullOrWhiteSpace(settings.Issuer) ? null : settings.Issuer,
            audience: string.IsNullOrWhiteSpace(settings.Audience) ? null : settings.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expires,
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return (tokenString, expires);
    }
}