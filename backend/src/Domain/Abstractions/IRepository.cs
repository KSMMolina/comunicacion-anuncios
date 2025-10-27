using System.Linq.Expressions;

namespace Communication.Announcements.Domain.Abstractions;

/// <summary>
/// Generic repository abstraction for aggregate roots.
/// </summary>
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    void Remove(T entity);
    IQueryable<T> Query(Expression<Func<T, bool>>? predicate = null);
}
