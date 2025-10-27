using BCrypt.Net;
using Communication.Announcements.Application.Abstractions.Auth;

namespace Communication.Announcements.Infrastructure.Auth;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verify(string hash, string password)
    {
        // Fallback temporal: si en BD está en texto plano, lo permitirá.
        if (hash == password) return true;
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
