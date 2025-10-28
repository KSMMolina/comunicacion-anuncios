using comunicacion_anuncios.Application.DTOs;
using MediatR;

namespace comunicacion_anuncios.Application.Features.Announcements;

public record GetAnnouncementReadersQuery(Guid Id) : IRequest<IReadOnlyList<ReaderItem>>;