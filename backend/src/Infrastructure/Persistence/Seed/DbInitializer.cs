using Communication.Announcements.Application.Abstractions.Auth;
using Communication.Announcements.Domain.Constants;
using Communication.Announcements.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Communication.Announcements.Infrastructure.Persistence.Seed;

public class DbInitializer
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<DbInitializer> _logger;

    public DbInitializer(AppDbContext context, IPasswordHasher passwordHasher, ILogger<DbInitializer> logger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Running database migrations for CommunicationDb.");
        await _context.Database.MigrateAsync(cancellationToken);
        await SeedRolesAsync(cancellationToken);
        await SeedUsersAsync(cancellationToken);
        await SeedAnnouncementsAsync(cancellationToken);
    }

    private async Task SeedRolesAsync(CancellationToken cancellationToken)
    {
        if (await _context.Roles.AnyAsync(cancellationToken))
        {
            return;
        }

        _logger.LogInformation("Seeding default roles.");

        _context.Roles.AddRange(
            new Role { Id = 1, Name = SystemRoles.Admin },
            new Role { Id = 2, Name = SystemRoles.Resident }
        );

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedUsersAsync(CancellationToken cancellationToken)
    {
        if (await _context.Users.AnyAsync(cancellationToken))
        {
            return;
        }

        _logger.LogInformation("Seeding default users.");

        var now = DateTime.UtcNow;

        var admin = new User
        {
            Id = Guid.NewGuid(),
            FullName = "Administrador Ofirma",
            Email = "admin@ofirma.com",
            PasswordHash = _passwordHasher.Hash("P@ssw0rd!"),
            RoleId = 1,
            IsActive = true,
            CreatedAt = now,
            CreatedBy = Guid.Empty
        };

        var juan = new User
        {
            Id = Guid.NewGuid(),
            FullName = "Juan Pérez",
            Email = "juan@ofirma.com",
            PasswordHash = _passwordHasher.Hash("P@ssw0rd!"),
            RoleId = 2,
            IsActive = true,
            CreatedAt = now,
            CreatedBy = Guid.Empty
        };

        var maria = new User
        {
            Id = Guid.NewGuid(),
            FullName = "María González",
            Email = "maria@ofirma.com",
            PasswordHash = _passwordHasher.Hash("P@ssw0rd!"),
            RoleId = 2,
            IsActive = true,
            CreatedAt = now,
            CreatedBy = Guid.Empty
        };

        _context.Users.AddRange(admin, juan, maria);
        await _context.SaveChangesAsync(cancellationToken);

        await SeedAnnouncementsDataAsync(admin, juan, maria, cancellationToken);
    }

    private async Task SeedAnnouncementsAsync(CancellationToken cancellationToken)
    {
        if (await _context.Announcements.AnyAsync(cancellationToken))
        {
            return;
        }

        var admin = await _context.Users.FirstAsync(u => u.Email == "admin@ofirma.com", cancellationToken);
        var juan = await _context.Users.FirstAsync(u => u.Email == "juan@ofirma.com", cancellationToken);
        var maria = await _context.Users.FirstAsync(u => u.Email == "maria@ofirma.com", cancellationToken);

        await SeedAnnouncementsDataAsync(admin, juan, maria, cancellationToken);
    }

    private async Task SeedAnnouncementsDataAsync(User admin, User juan, User maria, CancellationToken cancellationToken)
    {
        if (await _context.Announcements.AnyAsync(cancellationToken))
        {
            return;
        }

        _logger.LogInformation("Seeding sample announcements and confirmations.");

        var now = DateTime.UtcNow;

        var announcement1 = new Announcement
        {
            Id = Guid.NewGuid(),
            Title = "Mantenimiento de áreas comunes",
            Message = "El próximo lunes se realizará mantenimiento en el lobby. Favor programar sus actividades.",
            TargetGroup = "Todos",
            IsActive = true,
            CreatedAt = now.AddDays(-5),
            CreatedBy = admin.Id
        };

        var announcement2 = new Announcement
        {
            Id = Guid.NewGuid(),
            Title = "Corte de agua programado",
            Message = "Se informa que el miércoles habrá corte de agua desde las 8:00 a.m. hasta las 12:00 p.m.",
            TargetGroup = "Torre A",
            IsActive = true,
            CreatedAt = now.AddDays(-2),
            CreatedBy = admin.Id
        };

        var announcement3 = new Announcement
        {
            Id = Guid.NewGuid(),
            Title = "Actualización de reglamento interno",
            Message = "Se ha publicado la nueva versión del reglamento interno para su revisión.",
            TargetGroup = "Todos",
            IsActive = false,
            CreatedAt = now.AddDays(-1),
            CreatedBy = admin.Id
        };

        _context.Announcements.AddRange(announcement1, announcement2, announcement3);
        await _context.SaveChangesAsync(cancellationToken);

        _context.ReadConfirmations.AddRange(
            new ReadConfirmation
            {
                Id = Guid.NewGuid(),
                AnnouncementId = announcement1.Id,
                UserId = juan.Id,
                ReadAt = now.AddDays(-4)
            },
            new ReadConfirmation
            {
                Id = Guid.NewGuid(),
                AnnouncementId = announcement1.Id,
                UserId = maria.Id,
                ReadAt = now.AddDays(-3)
            },
            new ReadConfirmation
            {
                Id = Guid.NewGuid(),
                AnnouncementId = announcement2.Id,
                UserId = juan.Id,
                ReadAt = now.AddDays(-1)
            }
        );

        await _context.SaveChangesAsync(cancellationToken);
    }
}
