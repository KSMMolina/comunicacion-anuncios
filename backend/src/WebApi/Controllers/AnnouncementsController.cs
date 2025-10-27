using Communication.Announcements.Application.Common;
using Communication.Announcements.Application.DTOs.Announcements;
using Communication.Announcements.Application.Features.Announcements.Commands;
using Communication.Announcements.Application.Features.Announcements.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Communication.Announcements.WebApi.Controllers;

/// <summary>
/// Gestión de anuncios para el módulo Comunicación y Anuncios (ver Metadatos del reto en README-backend.md).
/// </summary>
[ApiController]
[Route("api/v1/announcements")]
[Authorize]
public class AnnouncementsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AnnouncementsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lista anuncios con paginación y filtros.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<AnnouncementResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAnnouncements([FromQuery] AnnouncementFilter filter, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetAnnouncementsQuery(filter), cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Obtiene un anuncio por identificador.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AnnouncementResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetAnnouncementByIdQuery(id), cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Crea un anuncio (solo Admin).
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(AnnouncementResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] AnnouncementCreateRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new CreateAnnouncementCommand(request), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    /// <summary>
    /// Actualiza un anuncio existente (solo Admin).
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(AnnouncementResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(Guid id, [FromBody] AnnouncementUpdateRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new UpdateAnnouncementCommand(id, request), cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Cambia el estado activo/inactivo de un anuncio (solo Admin).
    /// </summary>
    [HttpPatch("{id:guid}/toggle")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Toggle(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ToggleAnnouncementCommand(id), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Confirma la lectura del anuncio para el usuario autenticado.
    /// </summary>
    [HttpPost("{id:guid}/confirm")]
    [ProducesResponseType(typeof(ConfirmReadResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> ConfirmRead(Guid id, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new ConfirmReadCommand(id), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, response);
    }

    /// <summary>
    /// Lista quiénes han leído el anuncio (solo Admin).
    /// </summary>
    [HttpGet("{id:guid}/readers")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(IReadOnlyCollection<ReaderItemResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReaders(Guid id, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetReadersByAnnouncementQuery(id), cancellationToken);
        return Ok(response);
    }
}
