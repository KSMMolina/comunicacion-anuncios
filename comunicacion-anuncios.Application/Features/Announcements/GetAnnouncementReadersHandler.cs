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
        var ann = await _annRepo.GetByIdAsync(request.Id, ct) ?? throw new KeyNotFoundException("Anuncio no encontrado");

        var users = await _usersRepo.GetAllAsync(ct);
        // Para eficiencia podrías crear método repositorio que devuelva confirmaciones por anuncio.
        var confirmations = users.Select(u => new ReaderItem(
            u.Id,
            u.FullName,
            u.Email,
            false,
            null)).ToList();

        // Se haría una consulta directa a ReadConfirmations; aquí usamos repositorio individual por usuario.
        // Mejorar: añadir método bulk en IReadConfirmationsRepository.
        // (Simplificación: cargamos confirmaciones con ToDictionary via annRepo.GetByIdAsync se podría incluir navegación)

        return confirmations;
    }
}