using comunicacion_anuncios.Application.Abstractions;
using comunicacion_anuncios.Application.DTOs;
using MediatR;

namespace comunicacion_anuncios.Application.Features.Announcements;

public class GetAnnouncementReadersHandler : IRequestHandler<GetAnnouncementReadersQuery, IReadOnlyList<ReaderItem>>
{
    private readonly IAnnouncementsRepository _annRepo;
    private readonly IReadConfirmationsRepository _readRepo;
    private readonly IUsersReadRepository _usersRepo;

    public GetAnnouncementReadersHandler(
        IAnnouncementsRepository annRepo,
        IReadConfirmationsRepository readRepo,
        IUsersReadRepository usersRepo)
    {
        _annRepo = annRepo;
        _readRepo = readRepo;
        _usersRepo = usersRepo;
    }

    public async Task<IReadOnlyList<ReaderItem>> Handle(GetAnnouncementReadersQuery request, CancellationToken ct)
    {
        _ = await _annRepo.GetByIdAsync(request.Id, ct) ?? throw new KeyNotFoundException("Anuncio no encontrado");

        var confirmations = await _readRepo.GetByAnnouncementIdAsync(request.Id, ct);

        var readers = confirmations
            .Select(c => new ReaderItem(
                c.UserId,
                c.User?.FullName ?? string.Empty,
                c.User?.Email ?? string.Empty,
                true,
                c.ReadAt))
            .ToList();

        return readers;
    }
}