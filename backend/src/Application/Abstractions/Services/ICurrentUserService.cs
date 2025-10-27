namespace Communication.Announcements.Application.Abstractions.Services;

public interface ICurrentUserService
{
    Guid UserId { get; }
    string? Email { get; }
    string? Role { get; }
    string? FullName { get; }
    bool IsAuthenticated { get; }
}
