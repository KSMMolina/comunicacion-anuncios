namespace Communication.Announcements.Domain.Abstractions;

/// <summary>
/// Represents a unit of work for persisting changes to the data store.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
