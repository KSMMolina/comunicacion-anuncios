using comunicacion_anuncios.Application.Abstractions;
using comunicacion_anuncios.Application.Common;
using comunicacion_anuncios.Application.DTOs;
using comunicacion_anuncios.Application.Features.Announcements;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace comunicacion_anuncios.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class AnnouncementsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUser _currentUser;

    public AnnouncementsController(IMediator mediator, ICurrentUser currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<AnnouncementResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int size = 10,
        [FromQuery] string? search = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] bool? activeOnly = null,
        CancellationToken ct = default)
    {
        var role = _currentUser.GetUserRole();
        var req = new PagedRequest(page, size, search, fromDate, toDate, activeOnly);
        var result = await _mediator.Send(new GetAnnouncementsPagedQuery(req, role), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AnnouncementResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAnnouncementByIdQuery(id), ct);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(AnnouncementResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] AnnouncementCreateRequest body, CancellationToken ct)
    {
        var userId = _currentUser.GetUserId();
        var result = await _mediator.Send(new CreateAnnouncementCommand(body, userId), ct);
        return Created($"/api/announcements/{result.Id}", result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(AnnouncementResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] AnnouncementUpdateRequest body, CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateAnnouncementCommand(id, body), ct);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/toggle")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Toggle(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new ToggleAnnouncementCommand(id), ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/confirm")]
    [ProducesResponseType(typeof(ConfirmReadResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmRead(Guid id, CancellationToken ct)
    {
        var userId = _currentUser.GetUserId();
        var result = await _mediator.Send(new ConfirmReadCommand(id, userId), ct);
        return Created($"/api/announcements/{id}/confirm", result);
    }

    [HttpGet("{id:guid}/readers")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(IReadOnlyList<ReaderItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReaders(Guid id, CancellationToken ct)
    {
        var readers = await _mediator.Send(new GetAnnouncementReadersQuery(id), ct);
        return Ok(readers);
    }
}