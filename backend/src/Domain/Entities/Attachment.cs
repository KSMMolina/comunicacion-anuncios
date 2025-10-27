namespace Communication.Announcements.Domain.Entities;

/// <summary>
/// Represents an optional file linked to an announcement.
/// </summary>
public class Attachment
{
    public Guid Id { get; set; }
    public Guid AnnouncementId { get; set; }
    public required string FileName { get; set; }
    public required string FileUrl { get; set; }
    public DateTime UploadedAt { get; set; }

    public Announcement? Announcement { get; set; }
}
