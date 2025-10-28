using comunicacion_anuncios.Application.Abstractions;
using comunicacion_anuncios.Domain.Entities;
using comunicacion_anuncios.Infrastructure.Persistence;

namespace comunicacion_anuncios.Infrastructure.Repositories;

public class UsersWriteRepository : IUsersWriteRepository
{
    private readonly AppDbContext _ctx;
    public UsersWriteRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task AddAsync(User user, CancellationToken ct)
    {
        await _ctx.Users.AddAsync(user, ct);
    }
}