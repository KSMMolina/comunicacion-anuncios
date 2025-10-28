using comunicacion_anuncios.Application.Common;
using comunicacion_anuncios.Application.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace comunicacion_anuncios.Controllers
{
    [ApiController]
    [Route("api/public/users")]
    [Tags("PublicUsers")]
    public class PublicUsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PublicUsersController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<RegisterUserResult>), StatusCodes.Status201Created)]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse.Ok(result, "Usuario registrado"));
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            // Implementación futura; por ahora devolvemos 404 con sobre.
            return NotFound(ApiResponse.Fail<object>(new[] { "No encontrado" }, "Recurso no encontrado"));
        }
    }
}
