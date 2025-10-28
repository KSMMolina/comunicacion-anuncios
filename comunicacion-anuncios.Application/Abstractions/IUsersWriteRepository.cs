using comunicacion_anuncios.Domain.Entities;

namespace comunicacion_anuncios.Application.Abstractions;

public interface IUsersWriteRepository
{
    Task AddAsync(User user, CancellationToken ct);
}