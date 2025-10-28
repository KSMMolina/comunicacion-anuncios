using comunicacion_anuncios.Application.Common;
using comunicacion_anuncios.Domain.Entities;

namespace comunicacion_anuncios.Application.Abstractions;

public interface IJwtTokenGenerator
{
    (string Token, DateTime ExpiresAt) Generate(User user, JwtSettings settings);
}