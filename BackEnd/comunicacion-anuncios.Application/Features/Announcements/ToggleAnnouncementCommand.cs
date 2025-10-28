using MediatR;

namespace comunicacion_anuncios.Application.Features.Announcements;

public record ToggleAnnouncementCommand(Guid Id) : IRequest<Unit>;