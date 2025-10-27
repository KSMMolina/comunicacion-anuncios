using AutoMapper;
using AutoMapper.QueryableExtensions;
using Communication.Announcements.Application.Abstractions.Services;
using Communication.Announcements.Application.Common;
using Communication.Announcements.Application.DTOs.Announcements;
using Communication.Announcements.Domain.Constants;
using Communication.Announcements.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Communication.Announcements.Application.Features.Announcements.Queries;

public record GetAnnouncementsQuery(AnnouncementFilter Filter) : IRequest<PagedResult<AnnouncementResponse>>;

public class GetAnnouncementsQueryHandler : IRequestHandler<GetAnnouncementsQuery, PagedResult<AnnouncementResponse>>
{
    private readonly IAnnouncementsRepository _announcementsRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetAnnouncementsQueryHandler(
        IAnnouncementsRepository announcementsRepository,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _announcementsRepository = announcementsRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<PagedResult<AnnouncementResponse>> Handle(GetAnnouncementsQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;
        var query = _announcementsRepository.GetAnnouncementsQueryable();

        if (_currentUserService.Role == SystemRoles.Resident)
        {
            query = query.Where(x => x.IsActive);
        }
        else if (filter.ActiveOnly.HasValue)
        {
            query = query.Where(x => x.IsActive == filter.ActiveOnly.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim().ToLowerInvariant();
            query = query.Where(x => EF.Functions.Like(x.Title.ToLower(), $"%{search}%")
                || EF.Functions.Like(x.Message.ToLower(), $"%{search}%"));
        }

        if (filter.FromDate.HasValue)
        {
            query = query.Where(x => x.CreatedAt >= filter.FromDate.Value);
        }

        if (filter.ToDate.HasValue)
        {
            query = query.Where(x => x.CreatedAt <= filter.ToDate.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.TargetGroup))
        {
            var tg = filter.TargetGroup.Trim().ToLowerInvariant();
            query = query.Where(x => x.TargetGroup != null && x.TargetGroup.ToLower().Contains(tg));
        }

        query = query.OrderByDescending(x => x.CreatedAt);

        var page = Math.Max(filter.Page, 1);
        var size = Math.Clamp(filter.Size, 1, 100);

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ProjectTo<AnnouncementResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PagedResult<AnnouncementResponse>
        {
            Items = items,
            Total = total,
            Page = page,
            Size = size
        };
    }
}
