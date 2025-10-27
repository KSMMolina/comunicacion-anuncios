using Communication.Announcements.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Communication.Announcements.Infrastructure.Persistence.Configurations;

public class ReadConfirmationConfiguration : IEntityTypeConfiguration<ReadConfirmation>
{
    public void Configure(EntityTypeBuilder<ReadConfirmation> builder)
    {
        builder.ToTable("ReadConfirmations");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ReadAt)
            .IsRequired();

        builder.HasIndex(x => new { x.AnnouncementId, x.UserId })
            .IsUnique();

        builder.HasOne(x => x.Announcement)
            .WithMany(a => a.ReadConfirmations)
            .HasForeignKey(x => x.AnnouncementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany(u => u.ReadConfirmations)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
