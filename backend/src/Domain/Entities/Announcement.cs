namespace Communication.Announcements.Domain.Entities;

/// <summary>
/// Represents a communication or announcement delivered to residents.
/// </summary>
public class Announcement : AuditableEntity
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Message { get; set; }
    public string? TargetGroup { get; set; }
    public bool IsActive { get; set; }

    public User? CreatedByUser { get; set; }
    public User? UpdatedByUser { get; set; }

    public ICollection<Attachment> Attachments { get; set; } = new HashSet<Attachment>();
    public ICollection<ReadConfirmation> ReadConfirmations { get; set; } = new HashSet<ReadConfirmation>();
}
