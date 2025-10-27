namespace Communication.Announcements.Application.DTOs.Announcements;

public class AnnouncementResponse
{
    public Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Message { get; init; }
    public string? TargetGroup { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public Guid CreatedBy { get; init; }
    public string? CreatedByName { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public Guid? UpdatedBy { get; init; }
    public string? UpdatedByName { get; init; }
}
