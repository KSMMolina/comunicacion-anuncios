using comunicacion_anuncios.Application.Common;
using comunicacion_anuncios.Application.DTOs;
using MediatR;

namespace comunicacion_anuncios.Application.Features.Announcements;

public record GetAnnouncementsPagedQuery(PagedRequest Request, string CallerRole)
    : IRequest<PagedResult<AnnouncementResponse>>;