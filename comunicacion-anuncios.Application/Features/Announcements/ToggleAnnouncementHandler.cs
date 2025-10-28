using comunicacion_anuncios.Application.Abstractions;
using MediatR;

namespace comunicacion_anuncios.Application.Features.Announcements;

public class ToggleAnnouncementHandler : IRequestHandler<ToggleAnnouncementCommand, Unit>
{
    private readonly IAnnouncementsRepository _repo;
    private readonly IUnitOfWork _uow;

    public ToggleAnnouncementHandler(IAnnouncementsRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(ToggleAnnouncementCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.Id, ct) ?? throw new KeyNotFoundException("Anuncio no encontrado");
        entity.IsActive = !entity.IsActive;
        _repo.Update(entity);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}