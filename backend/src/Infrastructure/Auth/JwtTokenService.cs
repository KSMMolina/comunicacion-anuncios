using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Communication.Announcements.Application.Abstractions.Auth;
using Communication.Announcements.Application.DTOs.Auth;
using Communication.Announcements.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Communication.Announcements.Infrastructure.Auth;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtOptions _options;

    public JwtTokenService(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public AuthLoginResponse CreateToken(User user)
    {
        var now = DateTime.UtcNow;
        var expires = now.AddMinutes(_options.ExpiresMinutes);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role?.Name ?? string.Empty),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim("userId", user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        var handler = new JwtSecurityTokenHandler();
        var tokenString = handler.WriteToken(token);

        return new AuthLoginResponse
        {
            Token = tokenString,
            ExpiresAt = expires,
            User = new AuthenticatedUserResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role?.Name ?? string.Empty
            }
        };
    }
}
