using comunicacion_anuncios.Application.Abstractions;
using comunicacion_anuncios.Application.DTOs;
using MediatR;

namespace comunicacion_anuncios.Application.Features.Announcements;

public class GetAnnouncementByIdHandler : IRequestHandler<GetAnnouncementByIdQuery, AnnouncementResponse?>
{
    private readonly IAnnouncementsRepository _repo;

    public GetAnnouncementByIdHandler(IAnnouncementsRepository repo) => _repo = repo;

    public async Task<AnnouncementResponse?> Handle(GetAnnouncementByIdQuery request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.Id, ct);
        if (entity == null) return null;

        return new AnnouncementResponse(
            entity.Id,
            entity.Title,
            entity.Message,
            entity.TargetGroup,
            entity.IsActive,
            entity.CreatedAt,
            entity.CreatedBy,
            entity.CreatedByUser!.FullName);
    }
}