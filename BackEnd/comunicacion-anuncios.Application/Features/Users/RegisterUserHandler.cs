using comunicacion_anuncios.Application.Abstractions;
using comunicacion_anuncios.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace comunicacion_anuncios.Application.Features.Users;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, RegisterUserResult>
{
    private readonly IUsersReadRepository _usersRead;
    private readonly IUsersWriteRepository _usersWrite;
    private readonly IRolesReadRepository _rolesRead;
    private readonly IPasswordHasher _hasher;
    private readonly IUnitOfWork _uow;
    private readonly ILogger<RegisterUserHandler> _logger;

    public RegisterUserHandler(
        IUsersReadRepository usersRead,
        IUsersWriteRepository usersWrite,
        IRolesReadRepository rolesRead,
        IPasswordHasher hasher,
        IUnitOfWork uow,
        ILogger<RegisterUserHandler> logger)
    {
        _usersRead = usersRead;
        _usersWrite = usersWrite;
        _rolesRead = rolesRead;
        _hasher = hasher;
        _uow = uow;
        _logger = logger;
    }

    public async Task<RegisterUserResult> Handle(RegisterUserCommand request, CancellationToken ct)
    {
        var emailInput = request.Email.Trim();

        // Búsqueda case-insensitive (normalizamos ambos lados)
        var existing = await _usersRead.GetByEmailAsync(emailInput, ct);
        if (existing is not null)
            throw new InvalidOperationException("El correo ya está registrado.");

        Guid roleId;
        string roleName;

        if (request.RoleId.HasValue)
        {
            var role = await _rolesRead.GetByIdAsync(request.RoleId.Value, ct);
            if (role is null)
                throw new KeyNotFoundException("Rol no encontrado.");
            roleId = role.Id;
            roleName = role.Name;
        }
        else
        {
            var resident = await _rolesRead.GetByNameAsync("Resident", ct);
            if (resident is null)
                throw new InvalidOperationException("Rol 'Resident' no existe.");
            roleId = resident.Id;
            roleName = resident.Name;
        }

        var user = new User
        {
            FullName = request.FullName.Trim(),
            Email = emailInput,
            Password = _hasher.Hash(request.Password),
            RoleId = roleId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _usersWrite.AddAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        _logger.LogInformation("Usuario registrado {UserId} ({Email})", user.Id, user.Email);

        return new RegisterUserResult(
            user.Id,
            user.FullName,
            user.Email,
            roleName,
            user.IsActive,
            user.CreatedAt);
    }
}