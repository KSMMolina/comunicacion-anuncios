using comunicacion_anuncios.Application.Abstractions;

namespace comunicacion_anuncios.Infrastructure.Auth;

public class BcryptPasswordHasher : IPasswordHasher
{
    private const int WorkFactor = 10;
    public string Hash(string plain) => BCrypt.Net.BCrypt.HashPassword(plain, workFactor: WorkFactor);
}