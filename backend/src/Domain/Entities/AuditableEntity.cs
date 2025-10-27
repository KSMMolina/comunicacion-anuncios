namespace Communication.Announcements.Domain.Entities;

/// <summary>
/// Base entity that captures audit metadata for creation and updates.
/// </summary>
public abstract class AuditableEntity
{
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
}
