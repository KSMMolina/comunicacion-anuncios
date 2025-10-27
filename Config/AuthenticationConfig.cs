using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace comunicacion_anuncios.Config;

public static class AuthenticationConfig
{
    public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration config)
    {
        var section = config.GetSection("Jwt");
        var issuer = section["Issuer"] ?? "local";
        var audience = section["Audience"] ?? issuer;
        var keyStr = section["Key"] ?? "0123456789abcdef0123456789abcdef";

        if (keyStr.Length < 32)
            keyStr = keyStr.PadRight(32, '0');

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr));

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization(opt =>
        {
            opt.AddPolicy("AdminOnly", p => p.RequireClaim("role", "Admin"));
        });

        return services;
    }
}