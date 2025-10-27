using AutoMapper;
using Communication.Announcements.Application.Abstractions.Services;
using Communication.Announcements.Application.DTOs.Announcements;
using Communication.Announcements.Domain.Abstractions;
using Communication.Announcements.Domain.Entities;
using Communication.Announcements.Domain.Repositories;
using MediatR;

namespace Communication.Announcements.Application.Features.Announcements.Commands;

public record CreateAnnouncementCommand(AnnouncementCreateRequest Request) : IRequest<AnnouncementResponse>;

public class CreateAnnouncementCommandHandler : IRequestHandler<CreateAnnouncementCommand, AnnouncementResponse>
{
    private readonly IAnnouncementsRepository _announcementsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CreateAnnouncementCommandHandler(
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

    public async Task<AnnouncementResponse> Handle(CreateAnnouncementCommand request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var entity = new Announcement
        {
            Id = Guid.NewGuid(),
            Title = request.Request.Title.Trim(),
            Message = request.Request.Message.Trim(),
            TargetGroup = string.IsNullOrWhiteSpace(request.Request.TargetGroup) ? null : request.Request.TargetGroup.Trim(),
            IsActive = request.Request.IsActive,
            CreatedAt = now,
            CreatedBy = _currentUserService.UserId,
            UpdatedAt = null,
            UpdatedBy = null
        };

        await _announcementsRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AnnouncementResponse>(entity);
    }
}
