using comunicacion_anuncios.Application.Abstractions;
using comunicacion_anuncios.Application.DTOs;
using comunicacion_anuncios.Domain.Entities;
using MediatR;

namespace comunicacion_anuncios.Application.Features.Announcements;

public class ConfirmReadHandler : IRequestHandler<ConfirmReadCommand, ConfirmReadResponse>
{
    private readonly IReadConfirmationsRepository _readRepo;
    private readonly IAnnouncementsRepository _annRepo;
    private readonly IUnitOfWork _uow;

    public ConfirmReadHandler(IReadConfirmationsRepository readRepo, IAnnouncementsRepository annRepo, IUnitOfWork uow)
    {
        _readRepo = readRepo;
        _annRepo = annRepo;
        _uow = uow;
    }

    public async Task<ConfirmReadResponse> Handle(ConfirmReadCommand request, CancellationToken ct)
    {
        var announcement = await _annRepo.GetByIdAsync(request.Id, ct) ?? throw new KeyNotFoundException("Anuncio no encontrado");

        var existing = await _readRepo.GetAsync(request.Id, request.CurrentUserId, ct);
        if (existing is not null)
            return new ConfirmReadResponse(existing.AnnouncementId, existing.UserId, existing.ReadAt);

        var now = DateTime.UtcNow;
        var rc = new ReadConfirmation
        {
            AnnouncementId = request.Id,
            UserId = request.CurrentUserId,
            ReadAt = now
        };
        await _readRepo.AddAsync(rc, ct);
        await _uow.SaveChangesAsync(ct);

        return new ConfirmReadResponse(rc.AnnouncementId, rc.UserId, rc.ReadAt);
    }
}