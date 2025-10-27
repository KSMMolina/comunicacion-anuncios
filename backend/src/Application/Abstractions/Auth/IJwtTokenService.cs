using Communication.Announcements.Application.DTOs.Auth;
using Communication.Announcements.Domain.Entities;

namespace Communication.Announcements.Application.Abstractions.Auth;

public interface IJwtTokenService
{
    AuthLoginResponse CreateToken(User user);
}
