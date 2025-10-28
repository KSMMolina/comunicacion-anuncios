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
[Route("api/announcements")]
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
    public async Task<IActionResult> GetPaged(
        int page = 1,
        int size = 10,
        string? search = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        bool? activeOnly = null,
        CancellationToken ct = default)
    {
        var role = _currentUser.GetUserRole();
        var req = new PagedRequest(page, size, search, fromDate, toDate, activeOnly);
        var result = await _mediator.Send(new GetAnnouncementsPagedQuery(req, role), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAnnouncementByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(AnnouncementCreateRequest body, CancellationToken ct)
    {
        var userId = _currentUser.GetUserId();
        var created = await _mediator.Send(new CreateAnnouncementCommand(body, userId), ct);
        return Created($"/api/announcements/{created.Id}", created);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, AnnouncementUpdateRequest body, CancellationToken ct)
    {
        var updated = await _mediator.Send(new UpdateAnnouncementCommand(id, body), ct);
        return Ok(updated);
    }

    [HttpPatch("{id:guid}/toggle")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Toggle(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new ToggleAnnouncementCommand(id), ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/confirm")]
    public async Task<IActionResult> ConfirmRead(Guid id, CancellationToken ct)
    {
        var userId = _currentUser.GetUserId();
        var result = await _mediator.Send(new ConfirmReadCommand(id, userId), ct);
        return Created($"/api/announcements/{id}/confirm", result);
    }

    [HttpGet("{id:guid}/readers")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetReaders(Guid id, CancellationToken ct)
    {
        var readers = await _mediator.Send(new GetAnnouncementReadersQuery(id), ct);
        return Ok(readers);
    }
}