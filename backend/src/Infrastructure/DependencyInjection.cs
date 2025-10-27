using Communication.Announcements.Application.Abstractions.Auth;
using Communication.Announcements.Application.Abstractions.Services;
using Communication.Announcements.Domain.Abstractions;
using Communication.Announcements.Domain.Repositories;
using Communication.Announcements.Infrastructure.Auth;
using Communication.Announcements.Infrastructure.Persistence;
using Communication.Announcements.Infrastructure.Persistence.Seed;
using Communication.Announcements.Infrastructure.Repositories;
using Communication.Announcements.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Communication.Announcements.Infrastructure;

// ...

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default")));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IAnnouncementsRepository, AnnouncementsRepository>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddScoped<DbInitializer>();

        return services;
    }
}
