using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace comunicacion_anuncios.Config;

public static class AuthExtensions
{
    public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration cfg)
    {
        var issuer = cfg["Jwt:Issuer"]!;
        var audience = cfg["Jwt:Audience"]!;
        var key = cfg["Jwt:Key"]!;
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = signingKey,
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireClaim("role", "Admin"));
        });

        return services;
    }

    public static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services)
    {
        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Comunicación y Anuncios API", Version = "v1" });

            var scheme = new OpenApiSecurityScheme
            {
                Description = "JWT Bearer. Formato: 'Bearer {token}'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            };

            opt.AddSecurityDefinition("Bearer", scheme);
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { scheme, Array.Empty<string>() }
            });
        });

        return services;
    }
}