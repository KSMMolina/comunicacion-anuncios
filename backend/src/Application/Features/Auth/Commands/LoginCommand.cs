using Communication.Announcements.Application.Abstractions.Auth;
using Communication.Announcements.Application.DTOs.Auth;
using Communication.Announcements.Domain.Abstractions;
using Communication.Announcements.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Communication.Announcements.Application.Features.Auth.Commands;

public record LoginCommand(AuthLoginRequest Request) : IRequest<AuthLoginResponse>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthLoginResponse>
{
    private readonly IRepository<User> _usersRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(
        IRepository<User> usersRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _usersRepository = usersRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthLoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var email = request.Request.Email.Trim();

        var user = await _usersRepository
            .Query(u => u.Email == email)
            .Include(u => u.Role)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            throw new InvalidCredentialsException();
        }

        if (!user.IsActive)
        {
            throw new InvalidCredentialsException();
        }

        if (!_passwordHasher.Verify(user.PasswordHash, request.Request.Password))
        {
            throw new InvalidCredentialsException();
        }

        return _jwtTokenService.CreateToken(user);
    }
}

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException()
        : base("Invalid credentials for announcements module authentication.")
    {
    }
}
