using comunicacion_anuncios.Application.Abstractions;
using comunicacion_anuncios.Application.DTOs;
using MediatR;

namespace comunicacion_anuncios.Application.Features.Auth;

public class AuthLoginHandler : IRequestHandler<AuthLoginRequest, AuthLoginResponse>
{
    private readonly IUsersReadRepository _users;
    private readonly IPasswordVerifier _passwords;
    private readonly IJwtTokenService _jwt;

    public AuthLoginHandler(IUsersReadRepository users, IPasswordVerifier passwords, IJwtTokenService jwt)
    {
        _users = users;
        _passwords = passwords;
        _jwt = jwt;
    }

    public async Task<AuthLoginResponse> Handle(AuthLoginRequest request, CancellationToken ct)
    {
        try
        {
            var user = await _users.GetByEmailAsync(request.Email, ct);
            if (user is null || !user.IsActive || !_passwords.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Credenciales inválidas");

            var roleName = user.Role?.Name ?? string.Empty;
            var token = _jwt.CreateToken(user.Id, user.Email, roleName, user.FullName, out var expires);

            var info = new UserInfo(user.Id, user.FullName, user.Email, roleName);
            return new AuthLoginResponse(token, expires, info);
        }
        catch (Exception)
        {
            Console.WriteLine("error");
            throw;
        }
        
        
    }
}