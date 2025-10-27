using Communication.Announcements.Domain.Entities;

namespace Communication.Announcements.Domain.Repositories;

/// <summary>
/// Repository abstraction for announcement aggregate queries.
/// </summary>
public interface IAnnouncementsRepository : Domain.Abstractions.IRepository<Announcement>
{
    IQueryable<Announcement> GetAnnouncementsQueryable();
}
