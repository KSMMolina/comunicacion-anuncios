using comunicacion_anuncios.Application.Abstractions;
using comunicacion_anuncios.Domain.Entities;
using comunicacion_anuncios.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace comunicacion_anuncios.Infrastructure.Repositories;

public class AnnouncementsRepository : IAnnouncementsRepository
{
    private readonly AppDbContext _ctx;
    public AnnouncementsRepository(AppDbContext ctx) => _ctx = ctx;

    public IQueryable<Announcement> Query() =>
        _ctx.Announcements
            .Include(a => a.CreatedByUser)
            .AsNoTracking()
            .AsQueryable();

    public Task<Announcement?> GetByIdAsync(Guid id, CancellationToken ct) =>
        _ctx.Announcements
            .Include(a => a.CreatedByUser)
            .FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task AddAsync(Announcement entity, CancellationToken ct)
    {
        await _ctx.Announcements.AddAsync(entity, ct);
    }

    public void Update(Announcement entity)
    {
        _ctx.Announcements.Update(entity);
    }
}