using Communication.Announcements.Domain.Entities;
using Communication.Announcements.Domain.Repositories;
using Communication.Announcements.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Communication.Announcements.Infrastructure.Repositories;

public class AnnouncementsRepository : Repository<Announcement>, IAnnouncementsRepository
{
    public AnnouncementsRepository(AppDbContext context) : base(context)
    {
    }

    public IQueryable<Announcement> GetAnnouncementsQueryable()
    {
        return DbContext.Set<Announcement>()
            .AsNoTracking()
            .Include(a => a.CreatedByUser)
            .Include(a => a.UpdatedByUser);
    }
}
