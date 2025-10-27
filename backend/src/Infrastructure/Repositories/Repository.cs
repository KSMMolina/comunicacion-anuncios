using Communication.Announcements.Domain.Abstractions;
using Communication.Announcements.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Communication.Announcements.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext DbContext;

    public Repository(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await DbContext.Set<T>().AddAsync(entity, cancellationToken);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>().FindAsync(new object?[] { id }, cancellationToken);
    }

    public IQueryable<T> Query(Expression<Func<T, bool>>? predicate = null)
    {
        return predicate is null ? DbContext.Set<T>() : DbContext.Set<T>().Where(predicate);
    }

    public void Remove(T entity)
    {
        DbContext.Set<T>().Remove(entity);
    }

    public void Update(T entity)
    {
        DbContext.Set<T>().Update(entity);
    }
}
