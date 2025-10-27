using comunicacion_anuncios.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace comunicacion_anuncios.Infrastructure.Persistence.Configurations;

public class AnnouncementsConfiguration : IEntityTypeConfiguration<Announcement>
{
    public void Configure(EntityTypeBuilder<Announcement> builder)
    {
        builder.ToTable("Announcements", "dbo");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        builder.Property(x => x.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Message)
            .IsRequired();

        builder.Property(x => x.TargetGroup)
            .HasMaxLength(200);

        builder.Property(x => x.CreatedAt)
            .HasColumnType("datetime2(0)");

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.HasOne(a => a.CreatedByUser)
            .WithMany(u => u.AnnouncementsCreated)
            .HasForeignKey(a => a.CreatedBy)
            .IsRequired();

        builder.HasIndex(x => x.Title);
        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => x.IsActive);
    }
}