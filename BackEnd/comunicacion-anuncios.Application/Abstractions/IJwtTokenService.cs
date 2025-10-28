namespace comunicacion_anuncios.Application.Abstractions;

public interface IJwtTokenService
{
    string CreateToken(
        Guid userId,
        string email,
        Guid roleId,
        string roleName,
        string fullName,
        out DateTime expiresAt);
}