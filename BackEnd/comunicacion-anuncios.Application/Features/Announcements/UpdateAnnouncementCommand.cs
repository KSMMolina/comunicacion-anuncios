using comunicacion_anuncios.Application.DTOs;
using MediatR;

namespace comunicacion_anuncios.Application.Features.Announcements;

public record UpdateAnnouncementCommand(Guid Id, AnnouncementUpdateRequest Request)
    : IRequest<AnnouncementResponse>;