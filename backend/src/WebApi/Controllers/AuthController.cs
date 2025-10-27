using Communication.Announcements.Application.DTOs.Auth;
using Communication.Announcements.Application.Features.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Communication.Announcements.WebApi.Controllers;

/// <summary>
/// Autenticación JWT para el módulo Comunicación y Anuncios (ver Metadatos del reto en README-backend.md).
/// </summary>
[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Autentica un usuario y entrega un token JWT.
    /// </summary>
    /// <param name="request">Credenciales del usuario.</param>
    /// <returns>Token JWT y datos del usuario.</returns>
    /// <response code="200">Autenticación exitosa.</response>
    /// <response code="401">Credenciales inválidas (ProblemDetails).</response>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthLoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] AuthLoginRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new LoginCommand(request), cancellationToken);
        return Ok(response);
    }
}
