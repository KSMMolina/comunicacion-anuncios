using AutoMapper;
using Communication.Announcements.Application.DTOs.Announcements;
using Communication.Announcements.Application.Features.Announcements.Commands;
using Communication.Announcements.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Communication.Announcements.Application.Features.Announcements.Queries;

public record GetAnnouncementByIdQuery(Guid Id) : IRequest<AnnouncementResponse>;

public class GetAnnouncementByIdQueryHandler : IRequestHandler<GetAnnouncementByIdQuery, AnnouncementResponse>
{
    private readonly IAnnouncementsRepository _announcementsRepository;
    private readonly IMapper _mapper;

    public GetAnnouncementByIdQueryHandler(IAnnouncementsRepository announcementsRepository, IMapper mapper)
    {
        _announcementsRepository = announcementsRepository;
        _mapper = mapper;
    }

    public async Task<AnnouncementResponse> Handle(GetAnnouncementByIdQuery request, CancellationToken cancellationToken)
    {
        var announcement = await _announcementsRepository.GetAnnouncementsQueryable()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (announcement is null)
        {
            throw new NotFoundException($"Announcement {request.Id} was not found.");
        }

        return _mapper.Map<AnnouncementResponse>(announcement);
    }
}
