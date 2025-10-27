using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using comunicacion_anuncios.Application.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace comunicacion_anuncios.Infrastructure.Auth;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtOptions _options;
    private SymmetricSecurityKey? _key;
    private SigningCredentials? _creds;

    public JwtTokenService(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string CreateToken(
        Guid userId,
        string email,
        Guid roleId,
        string roleName,
        string fullName,
        out DateTime expiresAt)
    {
        // Defaults simples para pruebas
        var issuer = string.IsNullOrWhiteSpace(_options.Issuer) ? "local" : _options.Issuer;
        var audience = string.IsNullOrWhiteSpace(_options.Audience) ? issuer : _options.Audience;
        var minutes = _options.ExpiresMinutes > 0 ? _options.ExpiresMinutes : 120;
        var keyStr = string.IsNullOrWhiteSpace(_options.Key)
            ? "0123456789abcdef0123456789abcdef"
            : _options.Key;

        if (keyStr.Length < 32)
            keyStr = keyStr.PadRight(32, '0');

        _key ??= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr));
        _creds ??= new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

        expiresAt = DateTime.UtcNow.AddMinutes(minutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email ?? string.Empty),
            new Claim("role", roleName ?? string.Empty),
            new Claim("roleId", roleId.ToString()),
            new Claim("fullName", fullName ?? string.Empty)
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAt,
            signingCredentials: _creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}