using MediatR;

namespace comunicacion_anuncios.Application.Features.Users;

public record RegisterUserCommand(
    string FullName,
    string Email,
    string Password,
    Guid? RoleId
) : IRequest<RegisterUserResult>;

public record RegisterUserResult(
    Guid Id,
    string FullName,
    string Email,
    string Role,
    bool IsActive,
    DateTime CreatedAt
);