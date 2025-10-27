using Communication.Announcements.Application.Abstractions.Services;
using Communication.Announcements.Domain.Abstractions;
using Communication.Announcements.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Communication.Announcements.Application.Features.Announcements.Commands;

public record ToggleAnnouncementCommand(Guid Id) : IRequest<Unit>;

public class ToggleAnnouncementCommandHandler : IRequestHandler<ToggleAnnouncementCommand, Unit>
{
    private readonly IAnnouncementsRepository _announcementsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public ToggleAnnouncementCommandHandler(
        IAnnouncementsRepository announcementsRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _announcementsRepository = announcementsRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(ToggleAnnouncementCommand request, CancellationToken cancellationToken)
    {
        var announcement = await _announcementsRepository.GetAnnouncementsQueryable()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (announcement is null)
        {
            throw new NotFoundException($"Announcement {request.Id} was not found.");
        }

        announcement.IsActive = !announcement.IsActive;
        announcement.UpdatedAt = DateTime.UtcNow;
        announcement.UpdatedBy = _currentUserService.UserId;

        _announcementsRepository.Update(announcement);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
