namespace Communication.Announcements.Application.DTOs.Announcements;

public class ReaderItemResponse
{
    public Guid UserId { get; init; }
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public bool Read { get; init; }
    public DateTime? ReadAt { get; init; }
}
