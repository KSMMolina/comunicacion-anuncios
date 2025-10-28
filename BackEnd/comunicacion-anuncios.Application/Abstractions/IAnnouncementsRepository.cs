using comunicacion_anuncios.Domain.Entities;

namespace comunicacion_anuncios.Application.Abstractions;

public interface IAnnouncementsRepository
{
    IQueryable<Announcement> Query();
    Task<Announcement?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(Announcement entity, CancellationToken ct);
    void Update(Announcement entity);
}