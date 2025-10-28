using System.Threading;
using System.Threading.Tasks;

namespace comunicacion_anuncios.Application.Abstractions;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}