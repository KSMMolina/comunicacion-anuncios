using comunicacion_anuncios.Application.DTOs;
using MediatR;

namespace comunicacion_anuncios.Application.Features.Announcements;

public record CreateAnnouncementCommand(AnnouncementCreateRequest Request, Guid CurrentUserId)
    : IRequest<AnnouncementResponse>;