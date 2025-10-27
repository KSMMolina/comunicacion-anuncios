using Communication.Announcements.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Communication.Announcements.Infrastructure.Persistence.Configurations;

public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.ToTable("Attachments");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FileName)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(x => x.FileUrl)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(x => x.UploadedAt)
            .IsRequired();

        builder.HasOne(x => x.Announcement)
            .WithMany(a => a.Attachments)
            .HasForeignKey(x => x.AnnouncementId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
