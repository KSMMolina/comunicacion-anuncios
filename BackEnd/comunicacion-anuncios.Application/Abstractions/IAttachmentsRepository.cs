using comunicacion_anuncios.Domain.Entities;

namespace comunicacion_anuncios.Application.Abstractions;

public interface IAttachmentsRepository
{
    Task AddRangeAsync(IEnumerable<Attachment> entities, CancellationToken ct);
}