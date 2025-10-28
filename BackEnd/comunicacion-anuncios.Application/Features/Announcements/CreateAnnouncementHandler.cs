using comunicacion_anuncios.Application.Abstractions;
using comunicacion_anuncios.Application.DTOs;
using comunicacion_anuncios.Domain.Entities;
using MediatR;

namespace comunicacion_anuncios.Application.Features.Announcements;

public class CreateAnnouncementHandler : IRequestHandler<CreateAnnouncementCommand, AnnouncementResponse>
{
    private readonly IAnnouncementsRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateAnnouncementHandler(IAnnouncementsRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<AnnouncementResponse> Handle(CreateAnnouncementCommand request, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var entity = new Announcement
        {
            Title = request.Request.Title,
            Message = request.Request.Message,
            TargetGroup = request.Request.TargetGroup,
            CreatedBy = request.CurrentUserId,
            CreatedAt = now,
            IsActive = true
        };

        await _repo.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);

        return new AnnouncementResponse(
            entity.Id,
            entity.Title,
            entity.Message,
            entity.TargetGroup,
            entity.IsActive,
            entity.CreatedAt,
            entity.CreatedBy,
            "" // CreatedByName se puede completar en otra consulta si no se cargó navegación
        );
    }
}