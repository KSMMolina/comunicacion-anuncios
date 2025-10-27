using Communication.Announcements.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Communication.Announcements.Infrastructure.Persistence.Configurations;

public class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
{
    public void Configure(EntityTypeBuilder<Announcement> builder)
    {
        builder.ToTable("Announcements");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Message)
            .HasMaxLength(5000)
            .IsRequired();

        builder.Property(x => x.TargetGroup)
            .HasMaxLength(100);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .IsRequired();

        builder.HasOne(x => x.CreatedByUser)
            .WithMany(u => u.CreatedAnnouncements)
            .HasForeignKey(x => x.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.UpdatedByUser)
            .WithMany(u => u.UpdatedAnnouncements)
            .HasForeignKey(x => x.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.Title);
        builder.HasIndex(x => x.CreatedAt);

        builder.HasCheckConstraint("CK_Announcements_Title", "LEN([Title]) > 0");
        builder.HasCheckConstraint("CK_Announcements_Message", "LEN([Message]) > 0");
    }
}
