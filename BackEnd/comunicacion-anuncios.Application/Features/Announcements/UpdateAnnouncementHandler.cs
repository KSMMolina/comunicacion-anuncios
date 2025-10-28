using comunicacion_anuncios.Application.Abstractions;
using comunicacion_anuncios.Application.DTOs;
using MediatR;

namespace comunicacion_anuncios.Application.Features.Announcements;

public class UpdateAnnouncementHandler : IRequestHandler<UpdateAnnouncementCommand, AnnouncementResponse>
{
    private readonly IAnnouncementsRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateAnnouncementHandler(IAnnouncementsRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<AnnouncementResponse> Handle(UpdateAnnouncementCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.Id, ct) ?? throw new KeyNotFoundException("Anuncio no encontrado");
        entity.Title = request.Request.Title;
        entity.Message = request.Request.Message;
        entity.TargetGroup = request.Request.TargetGroup;

        _repo.Update(entity);
        await _uow.SaveChangesAsync(ct);

        return new AnnouncementResponse(
            entity.Id,
            entity.Title,
            entity.Message,
            entity.TargetGroup,
            entity.IsActive,
            entity.CreatedAt,
            entity.CreatedBy,
            entity.CreatedByUser?.FullName ?? ""
        );
    }
}