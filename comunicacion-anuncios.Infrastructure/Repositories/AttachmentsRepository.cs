using comunicacion_anuncios.Application.Abstractions;
using comunicacion_anuncios.Domain.Entities;
using comunicacion_anuncios.Infrastructure.Persistence;

namespace comunicacion_anuncios.Infrastructure.Repositories;

public class AttachmentsRepository : IAttachmentsRepository
{
    private readonly AppDbContext _ctx;
    public AttachmentsRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task AddRangeAsync(IEnumerable<Attachment> entities, CancellationToken ct)
    {
        await _ctx.Attachments.AddRangeAsync(entities, ct);
    }
}