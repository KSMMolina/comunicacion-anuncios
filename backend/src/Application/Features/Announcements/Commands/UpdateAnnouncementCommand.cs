using AutoMapper;
using Communication.Announcements.Application.Abstractions.Services;
using Communication.Announcements.Application.DTOs.Announcements;
using Communication.Announcements.Domain.Abstractions;
using Communication.Announcements.Domain.Entities;
using Communication.Announcements.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Communication.Announcements.Application.Features.Announcements.Commands;

public record UpdateAnnouncementCommand(Guid Id, AnnouncementUpdateRequest Request) : IRequest<AnnouncementResponse>;

public class UpdateAnnouncementCommandHandler : IRequestHandler<UpdateAnnouncementCommand, AnnouncementResponse>
{
    private readonly IAnnouncementsRepository _announcementsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public UpdateAnnouncementCommandHandler(
        IAnnouncementsRepository announcementsRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _announcementsRepository = announcementsRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<AnnouncementResponse> Handle(UpdateAnnouncementCommand request, CancellationToken cancellationToken)
    {
        var announcement = await _announcementsRepository.GetAnnouncementsQueryable()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (announcement is null)
        {
            throw new NotFoundException($"Announcement {request.Id} was not found.");
        }

        announcement.Title = request.Request.Title.Trim();
        announcement.Message = request.Request.Message.Trim();
        announcement.TargetGroup = string.IsNullOrWhiteSpace(request.Request.TargetGroup)
            ? null
            : request.Request.TargetGroup.Trim();
        announcement.UpdatedAt = DateTime.UtcNow;
        announcement.UpdatedBy = _currentUserService.UserId;

        _announcementsRepository.Update(announcement);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AnnouncementResponse>(announcement);
    }
}

public class NotFoundException : Exception
{
    public NotFoundException(string? message) : base(message)
    {
    }
}
