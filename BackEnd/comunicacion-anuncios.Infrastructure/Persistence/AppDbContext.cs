using comunicacion_anuncios.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace comunicacion_anuncios.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Announcement> Announcements => Set<Announcement>();
    public DbSet<ReadConfirmation> ReadConfirmations => Set<ReadConfirmation>();
    public DbSet<Attachment> Attachments => Set<Attachment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}