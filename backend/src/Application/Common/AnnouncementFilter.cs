namespace Communication.Announcements.Application.Common;

public class AnnouncementFilter : PagedRequest
{
    public string? TargetGroup { get; set; }
}
