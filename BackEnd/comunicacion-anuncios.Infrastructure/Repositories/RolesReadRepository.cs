using comunicacion_anuncios.Application.Abstractions;
using comunicacion_anuncios.Domain.Entities;
using comunicacion_anuncios.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace comunicacion_anuncios.Infrastructure.Repositories;

public class RolesReadRepository : IRolesReadRepository
{
    private readonly AppDbContext _ctx;
    public RolesReadRepository(AppDbContext ctx) => _ctx = ctx;

    public Task<Role?> GetByIdAsync(Guid id, CancellationToken ct) =>
        _ctx.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id, ct);

    public Task<Role?> GetByNameAsync(string name, CancellationToken ct) =>
        _ctx.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Name == name, ct);
}