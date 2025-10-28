namespace comunicacion_anuncios.Domain.Entities;

public class ReadConfirmation
{
    public Guid Id { get; set; }
    public Guid AnnouncementId { get; set; }
    public Guid UserId { get; set; }
    public DateTime ReadAt { get; set; }

    public Announcement? Announcement { get; set; }
    public User? User { get; set; }
}