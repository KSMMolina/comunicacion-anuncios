using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using comunicacion_anuncios.Domain.Entities;

namespace comunicacion_anuncios.Application.Abstractions;

public interface IUsersReadRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct);
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct);
}