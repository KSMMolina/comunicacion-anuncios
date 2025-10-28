using comunicacion_anuncios.Application.Abstractions;
using comunicacion_anuncios.Application.Common;
using comunicacion_anuncios.Application.DTOs;
using comunicacion_anuncios.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace comunicacion_anuncios.Application.Features.Announcements;

public class GetAnnouncementsPagedHandler : IRequestHandler<GetAnnouncementsPagedQuery, PagedResult<AnnouncementResponse>>
{
    private readonly IAnnouncementsRepository _repo;

    public GetAnnouncementsPagedHandler(IAnnouncementsRepository repo)
    {
        _repo = repo;
    }

    public async Task<PagedResult<AnnouncementResponse>> Handle(GetAnnouncementsPagedQuery request, CancellationToken ct)
    {
        var (page, size, search, fromDate, toDate, activeOnly) = request.Request;

        IQueryable<Announcement> query = _repo.Query();

        // Resident solo ve activos
        if (request.CallerRole == "Resident")
            query = query.Where(a => a.IsActive);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var pattern = search.Trim();
            query = query.Where(a => a.Title.Contains(pattern) || a.Message.Contains(pattern));
        }

        if (fromDate.HasValue)
            query = query.Where(a => a.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(a => a.CreatedAt <= toDate.Value);

        if (activeOnly.HasValue)
            query = query.Where(a => a.IsActive == activeOnly.Value);

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .Select(a => new AnnouncementResponse(
                a.Id,
                a.Title,
                a.Message,
                a.TargetGroup,
                a.IsActive,
                a.CreatedAt,
                a.CreatedBy,
                a.CreatedByUser!.FullName))
            .ToListAsync(ct);

        return new PagedResult<AnnouncementResponse>(items, total, page, size);
    }
}