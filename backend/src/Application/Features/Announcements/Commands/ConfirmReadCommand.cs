using Communication.Announcements.Application.Abstractions.Services;
using Communication.Announcements.Application.DTOs.Announcements;
using Communication.Announcements.Domain.Abstractions;
using Communication.Announcements.Domain.Entities;
using Communication.Announcements.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Communication.Announcements.Application.Features.Announcements.Commands;

public record ConfirmReadCommand(Guid AnnouncementId) : IRequest<ConfirmReadResponse>;

public class ConfirmReadCommandHandler : IRequestHandler<ConfirmReadCommand, ConfirmReadResponse>
{
    private readonly IAnnouncementsRepository _announcementsRepository;
    private readonly IRepository<ReadConfirmation> _readConfirmationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public ConfirmReadCommandHandler(
        IAnnouncementsRepository announcementsRepository,
        IRepository<ReadConfirmation> readConfirmationRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _announcementsRepository = announcementsRepository;
        _readConfirmationRepository = readConfirmationRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<ConfirmReadResponse> Handle(ConfirmReadCommand request, CancellationToken cancellationToken)
    {
        var announcementExists = await _announcementsRepository.GetAnnouncementsQueryable()
            .AnyAsync(x => x.Id == request.AnnouncementId && x.IsActive, cancellationToken);

        if (!announcementExists)
        {
            throw new NotFoundException($"Announcement {request.AnnouncementId} was not found or inactive.");
        }

        var existingConfirmation = await _readConfirmationRepository
            .Query(x => x.AnnouncementId == request.AnnouncementId && x.UserId == _currentUserService.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingConfirmation is not null)
        {
            return new ConfirmReadResponse
            {
                AnnouncementId = existingConfirmation.AnnouncementId,
                UserId = existingConfirmation.UserId,
                ReadAt = existingConfirmation.ReadAt
            };
        }

        var confirmation = new ReadConfirmation
        {
            Id = Guid.NewGuid(),
            AnnouncementId = request.AnnouncementId,
            UserId = _currentUserService.UserId,
            ReadAt = DateTime.UtcNow
        };

        await _readConfirmationRepository.AddAsync(confirmation, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ConfirmReadResponse
        {
            AnnouncementId = confirmation.AnnouncementId,
            UserId = confirmation.UserId,
            ReadAt = confirmation.ReadAt
        };
    }
}
