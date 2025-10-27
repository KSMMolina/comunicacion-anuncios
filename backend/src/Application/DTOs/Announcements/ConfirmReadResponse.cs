namespace Communication.Announcements.Application.DTOs.Announcements;

public class ConfirmReadResponse
{
    public Guid AnnouncementId { get; init; }
    public Guid UserId { get; init; }
    public DateTime ReadAt { get; init; }
}
