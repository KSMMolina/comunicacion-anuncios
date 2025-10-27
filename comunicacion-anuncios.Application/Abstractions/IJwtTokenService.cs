namespace comunicacion_anuncios.Application.Abstractions;

public interface IJwtTokenService
{
    string CreateToken(Guid userId, string email, string role, string fullName, out DateTime expiresAt);
}