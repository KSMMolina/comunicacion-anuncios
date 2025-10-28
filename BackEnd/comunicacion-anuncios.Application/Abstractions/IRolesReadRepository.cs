using comunicacion_anuncios.Domain.Entities;

namespace comunicacion_anuncios.Application.Abstractions;

public interface IRolesReadRepository
{
    Task<Role?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Role?> GetByNameAsync(string name, CancellationToken ct);
}