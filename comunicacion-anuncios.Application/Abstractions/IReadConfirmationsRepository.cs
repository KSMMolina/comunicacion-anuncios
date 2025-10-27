using comunicacion_anuncios.Domain.Entities;

namespace comunicacion_anuncios.Application.Abstractions;

public interface IReadConfirmationsRepository
{
    Task<ReadConfirmation?> GetAsync(Guid announcementId, Guid userId, CancellationToken ct);
    Task AddAsync(ReadConfirmation entity, CancellationToken ct);
}