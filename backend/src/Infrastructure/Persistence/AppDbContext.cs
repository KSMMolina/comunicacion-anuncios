using Communication.Announcements.Domain.Abstractions;
using Communication.Announcements.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Communication.Announcements.Infrastructure.Persistence;

public class AppDbContext : DbContext, IUnitOfWork
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Announcement> Announcements => Set<Announcement>();
    public DbSet<ReadConfirmation> ReadConfirmations => Set<ReadConfirmation>();
    public DbSet<Attachment> Attachments => Set<Attachment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyUtcDateTimes();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyUtcDateTimes()
    {
        foreach (var entry in ChangeTracker.Entries().Where(e => e.Entity is AuditableEntity && (e.State == EntityState.Added || e.State == EntityState.Modified)))
        {
            if (entry.Entity is AuditableEntity auditable)
            {
                auditable.CreatedAt = DateTime.SpecifyKind(auditable.CreatedAt, DateTimeKind.Utc);
                if (auditable.UpdatedAt.HasValue)
                {
                    auditable.UpdatedAt = DateTime.SpecifyKind(auditable.UpdatedAt.Value, DateTimeKind.Utc);
                }
            }
        }

        foreach (var entry in ChangeTracker.Entries().Where(e => e.Entity is ReadConfirmation))
        {
            if (entry.Entity is ReadConfirmation confirmation)
            {
                confirmation.ReadAt = DateTime.SpecifyKind(confirmation.ReadAt, DateTimeKind.Utc);
            }
        }

        foreach (var entry in ChangeTracker.Entries().Where(e => e.Entity is Attachment))
        {
            if (entry.Entity is Attachment attachment)
            {
                attachment.UploadedAt = DateTime.SpecifyKind(attachment.UploadedAt, DateTimeKind.Utc);
            }
        }
    }
}
