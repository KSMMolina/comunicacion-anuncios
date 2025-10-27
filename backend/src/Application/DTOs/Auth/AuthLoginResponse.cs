namespace Communication.Announcements.Application.DTOs.Auth;

public class AuthLoginResponse
{
    public required string Token { get; init; }
    public DateTime ExpiresAt { get; init; }
    public required AuthenticatedUserResponse User { get; init; }
}

public class AuthenticatedUserResponse
{
    public required Guid Id { get; init; }
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public required string Role { get; init; }
}
