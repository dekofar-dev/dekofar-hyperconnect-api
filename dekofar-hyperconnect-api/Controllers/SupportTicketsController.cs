using Dekofar.HyperConnect.Application.DTOs;
using Dekofar.HyperConnect.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dekofar.API.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize(Roles = "admin")]
public class SupportTicketsController : ControllerBase
{
    private readonly ISupportTicketService _service;

    public SupportTicketsController(ISupportTicketService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tickets = await _service.GetAllAsync();
        return Ok(tickets);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var ticket = await _service.GetByIdAsync(id);
        if (ticket == null) return NotFound();
        return Ok(ticket);
    }

    [HttpPost]
    public async Task<IActionResult> Create(SupportTicketDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return Ok(created);
    }

    [HttpPost("{ticketId}/assign/{userId}")]
    public async Task<IActionResult> Assign(Guid ticketId, Guid userId)
    {
        await _service.AssignUserAsync(ticketId, userId);
        return NoContent();
    }

    [HttpPost("note")]
    public async Task<IActionResult> AddNote(TicketNoteDto noteDto)
    {
        await _service.AddNoteAsync(noteDto);
        return NoContent();
    }
}
