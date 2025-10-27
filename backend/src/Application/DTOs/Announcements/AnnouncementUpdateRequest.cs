namespace Communication.Announcements.Application.DTOs.Announcements;

public class AnnouncementUpdateRequest
{
    public required string Title { get; set; }
    public required string Message { get; set; }
    public string? TargetGroup { get; set; }
}
