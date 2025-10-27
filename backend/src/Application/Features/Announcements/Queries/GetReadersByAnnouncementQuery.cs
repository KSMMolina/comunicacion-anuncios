using Communication.Announcements.Application.DTOs.Announcements;
using Communication.Announcements.Application.Features.Announcements.Commands;
using Communication.Announcements.Domain.Abstractions;
using Communication.Announcements.Domain.Constants;
using Communication.Announcements.Domain.Entities;
using Communication.Announcements.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Communication.Announcements.Application.Features.Announcements.Queries;

public record GetReadersByAnnouncementQuery(Guid AnnouncementId) : IRequest<IReadOnlyCollection<ReaderItemResponse>>;

public class GetReadersByAnnouncementQueryHandler : IRequestHandler<GetReadersByAnnouncementQuery, IReadOnlyCollection<ReaderItemResponse>>
{
    private readonly IAnnouncementsRepository _announcementsRepository;
    private readonly IRepository<User> _usersRepository;
    private readonly IRepository<ReadConfirmation> _readConfirmationsRepository;

    public GetReadersByAnnouncementQueryHandler(
        IAnnouncementsRepository announcementsRepository,
        IRepository<User> usersRepository,
        IRepository<ReadConfirmation> readConfirmationsRepository)
    {
        _announcementsRepository = announcementsRepository;
        _usersRepository = usersRepository;
        _readConfirmationsRepository = readConfirmationsRepository;
    }

    public async Task<IReadOnlyCollection<ReaderItemResponse>> Handle(GetReadersByAnnouncementQuery request, CancellationToken cancellationToken)
    {
        var announcementExists = await _announcementsRepository.GetAnnouncementsQueryable()
            .AnyAsync(x => x.Id == request.AnnouncementId, cancellationToken);

        if (!announcementExists)
        {
            throw new NotFoundException($"Announcement {request.AnnouncementId} was not found.");
        }

        var residents = await _usersRepository.Query(u => u.Role != null && u.Role.Name == SystemRoles.Resident)
            .Include(u => u.Role!)
            .ToListAsync(cancellationToken);

        var confirmations = await _readConfirmationsRepository
            .Query(rc => rc.AnnouncementId == request.AnnouncementId)
            .ToListAsync(cancellationToken);

        var responses = residents
            .Select(user =>
            {
                var confirmation = confirmations.FirstOrDefault(rc => rc.UserId == user.Id);
                return new ReaderItemResponse
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Read = confirmation is not null,
                    ReadAt = confirmation?.ReadAt
                };
            })
            .OrderByDescending(r => r.Read)
            .ThenBy(r => r.FullName)
            .ToList();

        return responses;
    }
}
