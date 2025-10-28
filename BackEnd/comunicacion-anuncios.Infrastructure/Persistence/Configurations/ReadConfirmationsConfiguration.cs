using comunicacion_anuncios.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace comunicacion_anuncios.Infrastructure.Persistence.Configurations;

public class ReadConfirmationsConfiguration : IEntityTypeConfiguration<ReadConfirmation>
{
    public void Configure(EntityTypeBuilder<ReadConfirmation> builder)
    {
        builder.ToTable("ReadConfirmations", "dbo");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        builder.Property(x => x.ReadAt)
            .HasColumnType("datetime2(0)");

        builder.HasOne(rc => rc.Announcement)
            .WithMany(a => a.ReadConfirmations)
            .HasForeignKey(rc => rc.AnnouncementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rc => rc.User)
            .WithMany(u => u.ReadConfirmations)
            .HasForeignKey(rc => rc.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ReadAt);
        builder.HasIndex(x => new { x.AnnouncementId, x.UserId }).IsUnique();
    }
}