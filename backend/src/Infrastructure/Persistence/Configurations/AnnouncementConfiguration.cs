using Communication.Announcements.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Communication.Announcements.Infrastructure.Persistence.Configurations;

public class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
{
    public void Configure(EntityTypeBuilder<Announcement> builder)
    {
        builder.ToTable("Announcements", "dbo");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        builder.Property(x => x.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Message)
            .HasColumnType("nvarchar(max)")
            .IsRequired();

        builder.Property(x => x.TargetGroup)
            .HasMaxLength(200);

        builder.Property(x => x.CreatedBy)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("SYSUTCDATETIME()")
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.HasOne(x => x.CreatedByUser)
            .WithMany(u => u.CreatedAnnouncements)
            .HasForeignKey(x => x.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(x => x.UpdatedAt);
        builder.Ignore(x => x.UpdatedBy);
        builder.Ignore(x => x.UpdatedByUser);

        builder.HasIndex(x => x.Title);
        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => x.IsActive);

        builder.HasCheckConstraint("CK_Announcements_Title_NotEmpty", "LEN(LTRIM(RTRIM([Title]))) > 0");
    }
}
