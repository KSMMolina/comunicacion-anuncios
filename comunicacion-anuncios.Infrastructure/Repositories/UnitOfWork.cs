using comunicacion_anuncios.Application.Abstractions;
using comunicacion_anuncios.Infrastructure.Persistence;

namespace comunicacion_anuncios.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _ctx;
    public UnitOfWork(AppDbContext ctx) => _ctx = ctx;

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        _ctx.SaveChangesAsync(ct);
}