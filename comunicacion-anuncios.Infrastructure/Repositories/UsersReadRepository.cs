using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using comunicacion_anuncios.Application.Abstractions;
using comunicacion_anuncios.Domain.Entities;
using comunicacion_anuncios.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace comunicacion_anuncios.Infrastructure.Repositories;

public class UsersReadRepository : IUsersReadRepository
{
    private readonly AppDbContext _ctx;
    public UsersReadRepository(AppDbContext ctx) => _ctx = ctx;

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct) =>
        _ctx.Users
            .Include(u => u.Role)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, ct);

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct) =>
        await _ctx.Users
            .AsNoTracking()
            .ToListAsync(ct);
}