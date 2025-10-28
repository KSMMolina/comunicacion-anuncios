using comunicacion_anuncios.Application.Abstractions;
using comunicacion_anuncios.Application.Common;
using comunicacion_anuncios.Application.DTOs;
using MediatR;

namespace comunicacion_anuncios.Application.Features.Auth;

public sealed class LoginHandler : IRequestHandler<AuthLoginRequest, AuthLoginResponse>
{
    private readonly IUsersReadRepository _users;
    private readonly IJwtTokenGenerator _tokenGen;
    private readonly JwtSettings _jwtSettings;
    private readonly IPasswordVerifier _passwordVerifier;

    public LoginHandler(
        IUsersReadRepository users, 
        IJwtTokenGenerator tokenGen, 
        JwtSettings jwtSettings,
        IPasswordVerifier passwordVerifier)
    {
        _users = users ?? throw new ArgumentNullException(nameof(users));
        _tokenGen = tokenGen ?? throw new ArgumentNullException(nameof(tokenGen));
        _jwtSettings = jwtSettings ?? throw new ArgumentNullException(nameof(jwtSettings));
        _passwordVerifier = passwordVerifier ?? throw new ArgumentNullException(nameof(passwordVerifier));
    }

    public async Task<AuthLoginResponse> Handle(AuthLoginRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            throw new UnauthorizedAccessException("Credenciales inválidas.");

        var user = await _users.GetByEmailAsync(request.Email, ct);
        if (user is null || !user.IsActive)
            throw new UnauthorizedAccessException("Credenciales inválidas.");

        // Asegúrate que la propiedad sea realmente el hash (renombra a PasswordHash si procede).
        var hash = user.Password;
        if (string.IsNullOrWhiteSpace(hash))
            throw new UnauthorizedAccessException("Credenciales inválidas.");

        if (!_passwordVerifier.Verify(request.Password, hash))
            throw new UnauthorizedAccessException("Credenciales inválidas.");

        var (token, expires) = _tokenGen.Generate(user, _jwtSettings);
        var info = new UserInfo(user.Id, user.FullName, user.Email, user.Role?.Name ?? "Resident");
        return new AuthLoginResponse(token, expires, info);
    }
}