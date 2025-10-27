namespace Communication.Announcements.Domain.Entities;

/// <summary>
/// Represents an authenticated person inside the system.
/// </summary>
public class User : AuditableEntity
{
    public Guid Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public bool IsActive { get; set; } = true;

    public int RoleId { get; set; }
    public Role? Role { get; set; }

    public ICollection<Announcement> CreatedAnnouncements { get; set; } = new HashSet<Announcement>();
    public ICollection<Announcement> UpdatedAnnouncements { get; set; } = new HashSet<Announcement>();
    public ICollection<ReadConfirmation> ReadConfirmations { get; set; } = new HashSet<ReadConfirmation>();
}
