using comunicacion_anuncios.Application.DTOs;
using MediatR;

namespace comunicacion_anuncios.Application.Features.Announcements;

public record ConfirmReadCommand(Guid Id, Guid CurrentUserId) : IRequest<ConfirmReadResponse>;