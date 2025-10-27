namespace Communication.Announcements.Application.DTOs.Announcements;

public class AnnouncementCreateRequest
{
    public required string Title { get; set; }
    public required string Message { get; set; }
    public string? TargetGroup { get; set; }
    public bool IsActive { get; set; } = true;
}
