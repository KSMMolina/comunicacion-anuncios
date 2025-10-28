using comunicacion_anuncios.Application.Abstractions;

namespace comunicacion_anuncios.Infrastructure.Auth;

public class BcryptPasswordVerifier : IPasswordVerifier
{
    public bool Verify(string plain, string hash) => BCrypt.Net.BCrypt.Verify(plain, hash);
}