using comunicacion_anuncios.Application.Abstractions;
using comunicacion_anuncios.Domain.Entities;
using comunicacion_anuncios.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace comunicacion_anuncios.Infrastructure.Repositories;

public class ReadConfirmationsRepository : IReadConfirmationsRepository
{
    private readonly AppDbContext _ctx;
    public ReadConfirmationsRepository(AppDbContext ctx) => _ctx = ctx;

    public Task<ReadConfirmation?> GetAsync(Guid announcementId, Guid userId, CancellationToken ct) =>
        _ctx.ReadConfirmations
            .AsNoTracking()
            .FirstOrDefaultAsync(rc => rc.AnnouncementId == announcementId && rc.UserId == userId, ct);

    public async Task AddAsync(ReadConfirmation entity, CancellationToken ct)
    {
        await _ctx.ReadConfirmations.AddAsync(entity, ct);
    }
}