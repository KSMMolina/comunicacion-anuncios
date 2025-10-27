namespace Communication.Announcements.Application.DTOs.Auth;

public class AuthLoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
