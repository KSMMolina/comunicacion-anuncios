using comunicacion_anuncios.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace comunicacion_anuncios.Infrastructure.Persistence.Configurations;

public class AttachmentsConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.ToTable("Attachments", "dbo");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        builder.Property(x => x.FileName)
            .HasMaxLength(260)
            .IsRequired();

        builder.Property(x => x.FileUrl)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.UploadedAt)
            .HasColumnType("datetime2(0)");

        builder.HasOne(att => att.Announcement)
            .WithMany(a => a.Attachments)
            .HasForeignKey(att => att.AnnouncementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.AnnouncementId);
        builder.HasIndex(x => x.UploadedAt);
    }
}