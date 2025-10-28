using comunicacion_anuncios.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace comunicacion_anuncios.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthLoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] AuthLoginRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(request, ct);
        return Ok(result);
    }
}