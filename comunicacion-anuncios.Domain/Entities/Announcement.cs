namespace comunicacion_anuncios.Domain.Entities;

public class Announcement
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;
    public string? TargetGroup { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }

    public User? CreatedByUser { get; set; }
    public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    public ICollection<ReadConfirmation> ReadConfirmations { get; set; } = new List<ReadConfirmation>();
}