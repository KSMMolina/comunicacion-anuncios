using comunicacion_anuncios.Application.Abstractions;
using comunicacion_anuncios.Infrastructure.Auth;
using comunicacion_anuncios.Infrastructure.Persistence;
using comunicacion_anuncios.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace comunicacion_anuncios.Infrastructure.Config;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
    {
        services.Configure<JwtOptions>(cfg.GetSection("Jwt"));

        services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(cfg.GetConnectionString("Default")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUsersReadRepository, UsersReadRepository>();
        services.AddScoped<IAnnouncementsRepository, AnnouncementsRepository>();
        services.AddScoped<IReadConfirmationsRepository, ReadConfirmationsRepository>();
        services.AddScoped<IAttachmentsRepository, AttachmentsRepository>();

        services.AddSingleton<IPasswordVerifier, BcryptPasswordVerifier>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();

        return services;
    }
}