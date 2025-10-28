using MediatR;

namespace comunicacion_anuncios.Application.DTOs;

public record AuthLoginRequest(string Email, string Password) : IRequest<AuthLoginResponse>;
public record UserInfo(Guid Id, string FullName, string Email, string Role);
public record AuthLoginResponse(string Token, DateTime ExpiresAt, UserInfo User);