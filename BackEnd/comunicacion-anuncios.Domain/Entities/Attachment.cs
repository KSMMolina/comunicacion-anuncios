namespace comunicacion_anuncios.Domain.Entities;

public class Attachment
{
    public Guid Id { get; set; }
    public Guid AnnouncementId { get; set; }
    public string FileName { get; set; } = null!;
    public string FileUrl { get; set; } = null!;
    public DateTime UploadedAt { get; set; }

    public Announcement? Announcement { get; set; }
}