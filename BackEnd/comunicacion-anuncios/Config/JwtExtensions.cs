using comunicacion_anuncios.Application.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace comunicacion_anuncios.Config;

public static class JwtExtensions
{
    public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection("Jwt");
        var jwtSettings = new JwtSettings
        {
            Issuer = jwtSection["Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer no configurado"),
            Audience = jwtSection["Audience"] ?? throw new InvalidOperationException("Jwt:Audience no configurado"),
            Key = jwtSection["Key"] ?? throw new InvalidOperationException("Jwt:Key no configurado"),
            ExpiresMinutes = int.Parse(jwtSection["ExpiresMinutes"] ?? "120")
        };

        services.AddSingleton(jwtSettings);

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization(opt =>
        {
            opt.AddPolicy("AdminOnly", policy => policy.RequireClaim("role", "Admin"));
        });

        return services;
    }
}