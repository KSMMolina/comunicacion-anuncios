namespace Communication.Announcements.Domain.Entities;

/// <summary>
/// Tracks the read confirmation of an announcement by a user.
/// </summary>
public class ReadConfirmation
{
    public Guid Id { get; set; }
    public Guid AnnouncementId { get; set; }
    public Guid UserId { get; set; }
    public DateTime ReadAt { get; set; }

    public Announcement? Announcement { get; set; }
    public User? User { get; set; }
}
